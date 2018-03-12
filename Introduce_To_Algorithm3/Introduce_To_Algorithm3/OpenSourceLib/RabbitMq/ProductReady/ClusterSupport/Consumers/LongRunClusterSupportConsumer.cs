using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.strings;
using Introduce_To_Algorithm3.Common.Utils.threads;
using Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.Consumers;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.ClusterSupport.Consumers
{
    /// <summary>
    /// 支持集群的长期运行消费者
    /// 基本思想:
    /// 初始时随机选择一个节点，如果该节点挂了，重新选择下一个节点接收
    /// 使用定时器监控哪个节点挂了
    /// 
    /// 队列是固定在集群某个节点上的，如果是临时队列，这么做没有问题，如果是持久队列，队列所在节点挂了，该节点队列恢复后才能继续使用，切换节点实际上没有任何用处
    /// </summary>
    public class LongRunClusterSupportConsumer:IDisposable
    {
        #region fields

        /// <summary>
        /// 要发送的rmq的cluster集群列表
        /// </summary>
        private readonly List<RabbitMqConfig> _rmqConfigList = null;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// 是否需要创建exchange
        /// </summary>
        private readonly bool _needToCreateExchange;

        /// <summary>
        /// exchange的名字
        /// </summary>
        private readonly string _exchangeName;

        /// <summary>
        /// exchange的类型
        /// </summary>
        private readonly string _exchangeType;

        /// <summary>
        /// 需要创建队列
        /// </summary>
        private readonly bool _needToCreateQueue;

        /// <summary>
        /// 队列名称，如果为空或者null表示必须创建临时队列，忽略createQueue参数
        /// </summary>
        private readonly string _queueName;

        /// <summary>
        /// 如果需要创建队列则需要创建binding，否则不需要创建binding  队列exchange的binding
        /// </summary>
        private readonly string _queueExchangeBinding;

        /// <summary>
        /// 当前使用的rmq索引
        /// </summary>
        private volatile int _currentUsedRmqIndex = 0;
        
        /// <summary>
        /// exchange是否创建的exchange
        /// </summary>
        private readonly ConcurrentDictionary<string, bool> _exchangeCreatedDict = new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// 队列是否创建
        /// </summary>
        private readonly ConcurrentDictionary<string,bool> _queueCreateDict = new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// connectionFactory的字典
        /// </summary>
        private readonly ConcurrentDictionary<string, ConnectionFactory> _conFactoryDict = new ConcurrentDictionary<string, ConnectionFactory>();


        /// <summary>
        /// 一次同时运行定时器
        /// </summary>
        private readonly OneRunTimerEx _oneRunTimer = null;


        /// <summary>
        /// 当前使用的连接
        /// </summary>
        private volatile IConnection _curUsedConnection = null;

        /// <summary>
        /// 当前使用的channel
        /// </summary>
        private volatile IModel _curUsedChannel = null;

        /// <summary>
        /// 队列最大长度
        /// </summary>
        private const int MaxQueueLength = 9999;

        #endregion


        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rmqList">rmqList监听的List</param>
        /// <param name="createExhange">是否需要创建exchange</param>
        /// <param name="exchangeName">exchange的名字</param>
        /// <param name="exchangType">exchange的类型</param>
        /// <param name="createQueue">是否需要创建队列</param>
        /// <param name="queueName">队列名称，如果为空或者null表示必须创建临时队列，忽略createQueue参数</param>
        /// <param name="queueExchangeBindingKey">如果需要创建队列则需要创建binding，否则不需要创建binding  队列exchange的binding</param>
        public LongRunClusterSupportConsumer(List<RabbitMqConfig> rmqList, bool createExhange, string exchangeName,
            string exchangType, bool createQueue, string queueName, string queueExchangeBindingKey)
        {
            if (rmqList == null || rmqList.Count == 0)
            {
                throw new Exception("configList为空或null");
            }

            //重排增加随机
            this._rmqConfigList = CollectionUtils.Shuffle(rmqList);
            this._needToCreateExchange = createExhange;
            this._exchangeName = exchangeName;
            this._exchangeType = exchangType;
            this._needToCreateQueue = createQueue;
            this._queueName = StringUtils.TrimEx(queueName);
            this._queueExchangeBinding = queueExchangeBindingKey;

            //定义定时器
            _oneRunTimer = new OneRunTimerEx(OneRunTimerCallBack, dueTime: 1000, period: 19711, state: null, exceptionHandler: OneRunTimerExceptionHandler);
        }

        #endregion


        #region 消息接收事件

        /// <summary>
        /// 消息接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs args)
        {

            try
            {
                //获取消息
                byte[] body = args?.Body;


                if (body == null || body.Length == 0)
                {
                    NLogHelper.Warn($"接收到空消息");
                    return;
                }

                RMqMessage msg = new RMqMessage()
                {
                    Id = Guid.NewGuid().ToString(),
                    ContentBytes = body,
                    CreateTime = DateTime.Now
                };

                RMqMessageHandler.Add(msg);
            }
            catch (Exception e)
            {
                NLogHelper.Error($"mq处理消息失败:{e}");
            }

        }

        #endregion

        #region 连接事件回调

        /// <summary>
        /// 连接阻塞
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connectionBlockedEventArgs"></param>
        private static void ConnectionOnConnectionBlocked(object sender, ConnectionBlockedEventArgs connectionBlockedEventArgs)
        {
            NLogHelper.Warn($"OnConnectionBlocked:{connectionBlockedEventArgs?.Reason}");
        }

        /// <summary>
        /// 连接关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="shutdownEventArgs"></param>
        private static void ConnectionOnConnectionShutdown(object sender, ShutdownEventArgs shutdownEventArgs)
        {

            //lock (SLocker)
            //{
            //    _isAlive = false;
            //}

            NLogHelper.Warn($"OnConnectionShutdown");
        }

        /// <summary>
        /// 连接恢复成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void ConnectionOnRecoverySucceeded(object sender, EventArgs eventArgs)
        {

            //lock (SLocker)
            //{
            //    _isAlive = true;
            //}
            NLogHelper.Warn($"OnRecoverySucceeded");
        }

        /// <summary>
        /// 连接恢复失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connectionRecoveryErrorEventArgs"></param>
        private static void ConnectionOnConnectionRecoveryError(object sender, ConnectionRecoveryErrorEventArgs connectionRecoveryErrorEventArgs)
        {

            //lock (SLocker)
            //{
            //    _isAlive = false;
            //}

            NLogHelper.Warn($"OnConnectionRecoveryError:{connectionRecoveryErrorEventArgs?.Exception}");
        }
        /// <summary>
        /// 连接回调错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackExceptionEventArgs"></param>
        private static void ConnectionOnCallbackException(object sender, CallbackExceptionEventArgs callbackExceptionEventArgs)
        {
            NLogHelper.Warn($"OnCallbackException:{callbackExceptionEventArgs?.Exception}");
        }
        /// <summary>
        /// 连接畅通
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void ConnectionOnConnectionUnblocked(object sender, EventArgs eventArgs)
        {
            NLogHelper.Warn($"OnConnectionUnblocked");
        }

        #endregion


        #region 辅助方法

        /// <summary>
        /// 获取当前使用的Rmq
        /// </summary>
        /// <returns></returns>
        public RabbitMqConfig GetCurrentUsedRmq()
        {
            lock (_locker)
            {
                int currentIndex = _currentUsedRmqIndex;
                int count = _rmqConfigList.Count;

                if (currentIndex < 0 || currentIndex >= count)
                {
                    //重置索引
                    currentIndex = _currentUsedRmqIndex = 0;
                }
                return _rmqConfigList[currentIndex];
            }
        }


        /// <summary>
        /// 是否mq当前节点存活
        /// </summary>
        /// <returns></returns>
        private bool IsMqAlive()
        {
            try
            {
                IConnection conn = null;
                lock (_locker)
                {
                    conn = _curUsedConnection;
                }
                if (conn == null)
                {
                    return false;
                }

                return conn.IsOpen;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 启动mq
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        private bool StartMq(Action<Exception> exceptionHandler = null)
        {
            try
            {
                StopMq();

                RabbitMqConfig mqConfig = GetCurrentUsedRmq();
                string mqConfigStr = mqConfig.ToString();
                #region 获取ConnectionFactory

                ConnectionFactory conFactory = null;
                if (!_conFactoryDict.TryGetValue(mqConfigStr, out conFactory))
                {
                    //自动重连
                    conFactory = new ConnectionFactory() { UserName = mqConfig.UserName, Password = mqConfig.Password, VirtualHost = mqConfig.VirtualHost, HostName = mqConfig.HostIp, Port = mqConfig.Port, AutomaticRecoveryEnabled = true/*自动重连*/, NetworkRecoveryInterval = TimeSpan.FromMilliseconds(5753) };
                    _conFactoryDict.AddOrUpdate(mqConfigStr, conFactory, (key, oldV) => conFactory);
                }

                #endregion

                #region 创建connection channel
                IConnection tempConnect = conFactory.CreateConnection();

                //定义连接事件
                //连接阻塞的回调
                tempConnect.ConnectionBlocked += ConnectionOnConnectionBlocked;
                //连接未阻塞的回调
                tempConnect.ConnectionUnblocked += ConnectionOnConnectionUnblocked;
                //连接回调异常
                tempConnect.CallbackException += ConnectionOnCallbackException;
                //连接恢复错误
                tempConnect.ConnectionRecoveryError += ConnectionOnConnectionRecoveryError;
                //连接恢复成功
                tempConnect.RecoverySucceeded += ConnectionOnRecoverySucceeded;
                //连接关闭
                tempConnect.ConnectionShutdown += ConnectionOnConnectionShutdown;

                //创建channel
                IModel tempChannel = tempConnect.CreateModel();
                lock (_locker)
                {
                    _curUsedConnection = tempConnect;
                    _curUsedChannel = tempChannel;
                }

                #endregion


                #region 创建exchange

                //判断是否需要创建exchange
                bool needCreateExchange = this._needToCreateExchange;
                if (needCreateExchange)
                {
                    bool alreadyCreated = false;
                    if (_exchangeCreatedDict.TryGetValue(mqConfigStr, out alreadyCreated) && alreadyCreated)
                    {
                        needCreateExchange = false;
                    }
                }

                if (needCreateExchange)
                {
                    tempChannel.ExchangeDeclare(exchange: this._exchangeName, type: this._exchangeType, durable: true, autoDelete: false, arguments: null);
                    _exchangeCreatedDict.AddOrUpdate(mqConfigStr, true, (key, oldV) => true);
                }

                #endregion

                //使用的队列名，如果为空表示临时队列
                string usedQueueName = string.Empty;
                #region 创建queue

                //是否需要创建队列
                bool needCreateQueue = this._needToCreateQueue;
                bool isTempQueue = false;//是否是临时队列
                if (string.IsNullOrWhiteSpace(this._queueName))
                {
                    needCreateQueue = true;
                    usedQueueName = string.Empty;
                    isTempQueue = true;
                }
                else
                {
                    usedQueueName = this._queueName;
                    isTempQueue = false;
                    if (needCreateQueue)
                    {
                        var alreadyCreated = false;
                        if (_queueCreateDict.TryGetValue(mqConfigStr, out alreadyCreated) && alreadyCreated)
                        {
                            needCreateQueue = false;
                        }
                    }
                }

                if (isTempQueue)
                {
                    //临时队列
                    //2、创建queue
                    //由MQ服务器创建一个amp.开头的(如amq.gen-JzTY20BRgKO-HjmUJj0wLg)非持久、排他、自动删除的队列
                    //连接断开后，服务器将删除该队列
                    string tempQueueName = tempChannel.QueueDeclare().QueueName;
                    usedQueueName = tempQueueName;
                    tempChannel.QueueBind(queue:usedQueueName,exchange:this._exchangeName,routingKey:_queueExchangeBinding,arguments:null);
                }
                else
                {
                    if (needCreateQueue)
                    {
                        //定义创建队列时的参数
                        Dictionary<string, object> dictArgs = new Dictionary<string, object>();
                        //限制队列的最大长度，超过该长度将从队列头丢弃
                        dictArgs.Add("x-max-length", MaxQueueLength);

                        tempChannel.QueueDeclare(queue: usedQueueName, durable: true, exclusive: false,
                            autoDelete: false, arguments: dictArgs);
                        tempChannel.QueueBind(queue: usedQueueName, exchange: this._exchangeName, routingKey: _queueExchangeBinding, arguments: null);
                        _queueCreateDict.AddOrUpdate(mqConfigStr, true, (key, oldV) => true);
                    }
                }



                #endregion


                #region 消息接收

                //prefetchSize - maximum amount of content (measured in octets) that the server will deliver, 0 if unlimited
                //prefetchCount - maximum number of messages that the server will deliver, 0 if unlimited
                //global - true if the settings should be applied to the entire channel rather than each consumer
                //每次取一个消息，消息大小不限制
                //注释掉该行，将会使用Robbin轮盘分配，使用下面这行，只有空闲的Consumer接收消息
                tempChannel.BasicQos(prefetchSize: 0, prefetchCount: 5, global: false);

                //定义消息接收事件
                EventingBasicConsumer consumer = new EventingBasicConsumer(tempChannel);
                consumer.Received += ConsumerOnReceived;

                //自动应答
                tempChannel.BasicConsume(queue: usedQueueName, autoAck: true, consumer: consumer);

                #endregion


                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        /// <summary>
        /// 停止mq
        /// </summary>
        private void StopMq()
        {
            IConnection tempConnection = null;
            IModel tempChannel = null;

            lock (_locker)
            {
                tempChannel = _curUsedChannel;
                tempConnection = _curUsedConnection;
            }

            if (tempChannel != null)
            {
                try
                {
                    tempChannel.Dispose();
                }
                catch
                {
                }
            }

            if (tempConnection != null)
            {
                try
                {
                    tempConnection.Dispose();
                }
                catch
                {
                }
            }

            lock (_locker)
            {
                _curUsedChannel = null;
                _curUsedConnection = null;
            }
        }

        #endregion


        #region 定时器相关


        /// <summary>
        /// 是否第一次启动
        /// </summary>
        private volatile bool _isFirstTimeToStart = true;

        /// <summary>
        /// 不活动次数计数，如果第一次不活动将会在下一个周期尝试重启
        /// </summary>
        private volatile int _timerNonAliveCount = 0;

        /// <summary>
        /// 定时器回调
        /// </summary>
        private void OneRunTimerCallBack()
        {
            if (!IsMqAlive())
            {
                if (_isFirstTimeToStart)
                {
                    _isFirstTimeToStart = false;

                    NLogHelper.Debug($"首次尝试启动mq");
                    StartMq(ex =>
                    {
                        NLogHelper.Error($"首次尝试启动mq失败:{ex}");
                    });
                }
                else
                {
                    bool needToStartMq = false;
                    _timerNonAliveCount++;
                    if (_timerNonAliveCount > 1 || _rmqConfigList.Count == 1)
                    {
                        //count == 1表示单机版
                        if (_timerNonAliveCount > 1000000)
                        {
                            //避免溢出
                            _timerNonAliveCount = 100;
                        }

                        needToStartMq = true;
                    }

                    if (needToStartMq)
                    {

                        RabbitMqConfig oldConfig = GetCurrentUsedRmq();

                        //取下一个机器
                        lock (_locker)
                        {
                            _currentUsedRmqIndex++;
                        }

                        RabbitMqConfig newConfig = GetCurrentUsedRmq();



                        //第二次才尝试启动
                        StartMq(ex =>
                        {
                            NLogHelper.Error($"尝试启动mq失败:{ex}");
                        });

                        //mq切机
                        //_mqChangeCallback?.Invoke(oldConfig, newConfig);
                    }
                }
            }
            else
            {
                _timerNonAliveCount = 0;
            }
        }

        /// <summary>
        /// 定时器异常处理
        /// </summary>
        /// <param name="ex"></param>
        private void OneRunTimerExceptionHandler(Exception ex)
        {
            NLogHelper.Error($"检测定时器失败:{ex}");
        }


        #endregion


        #region IDispose接口

        /// <summary>
        /// Dispose接口
        /// </summary>
        public void Dispose()
        {
            if (_oneRunTimer != null)
            {
                //关闭定时器
                _oneRunTimer.Close();
            }

            StopMq();
        }
        #endregion
    }
}

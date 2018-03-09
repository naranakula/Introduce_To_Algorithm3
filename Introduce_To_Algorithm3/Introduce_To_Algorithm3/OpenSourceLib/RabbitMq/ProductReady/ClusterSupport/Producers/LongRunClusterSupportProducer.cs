using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.threads;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using RabbitMQ.Client;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.ClusterSupport.Producers
{
    /// <summary>
    /// 支持集群的长连接生产者
    /// 基本思想:
    /// 随机选择一个节点，如果该节点挂了，重新选择下一个节点
    /// 使用定时器来监控哪个节点挂了
    /// </summary>
    public class LongRunClusterSupportProducer:IDisposable
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
        /// 当前是否的rmq配置
        /// </summary>
        private volatile int _currentUsedRmqIndex = 0;

        /// <summary>
        /// 消息默认超时时间
        /// </summary>
        public const int MessageExpirationInMilliseconds = 2 * 60 * 60 * 1000;

        /// <summary>
        /// exchange是否创建的exchange
        /// </summary>
        private readonly ConcurrentDictionary<string, bool> _exchangeCreatedDict = new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// connectionFactory的字典
        /// </summary>
        private readonly ConcurrentDictionary<string, ConnectionFactory> _conFactoryDict = new ConcurrentDictionary<string, ConnectionFactory>();

        /// <summary>
        /// mq切换事件回调 第一个参数是旧的mq，第二个参数是新的mq
        /// </summary>
        private readonly Action<RabbitMqConfig, RabbitMqConfig> _mqChangeCallback = null;


        /// <summary>
        /// 一次同时运行定时器
        /// </summary>
        private readonly OneRunTimerEx _oneRunTimer = null;


        /// <summary>
        /// 当前使用的连接
        /// </summary>
        private volatile IConnection _curUsedConnection = null;
        
        #endregion


        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rmqList">rmq转发的列表</param>
        /// <param name="createExchange">是否创建Exchange</param>
        /// <param name="exchangeName">exchange的名字</param>
        /// <param name="exchangeType">exchange的类型</param>
        /// <param name="mqChangeCallBack">mq切换时的回调，第一个参数是旧的mq，第二个参数是新的mq</param>
        public LongRunClusterSupportProducer(List<RabbitMqConfig> rmqList, bool createExchange, string exchangeName, string exchangeType = ExchangeType.Direct, Action<RabbitMqConfig, RabbitMqConfig> mqChangeCallBack = null)
        {
            if (rmqList == null || rmqList.Count == 0)
            {
                throw new Exception("configList为空或null");
            }

            //重排增加随机
            this._rmqConfigList = CollectionUtils.Shuffle(rmqList);
            this._needToCreateExchange = createExchange;
            this._exchangeName = exchangeName;
            this._exchangeType = exchangeType;
            this._mqChangeCallback = mqChangeCallBack;

            //定义定时器
            _oneRunTimer = new OneRunTimerEx(OneRunTimerCallBack,dueTime:1000,period:19111,state:null,exceptionHandler:OneRunTimerExceptionHandler);
        }


        #endregion

        #region 发送消息

        /// <summary>
        /// 发送消息 多线程安全
        /// </summary>
        /// <param name="routingKey">routingkey</param>
        /// <param name="messageBytes">消息内容最大2g</param>
        /// <param name="exceptionHandler">异常处理，一般是消息丢失</param>
        /// <returns></returns>
        public bool SendMessage(string routingKey, byte[] messageBytes, Action<Exception> exceptionHandler = null)
        {

            //获取当前mq的配置
            RabbitMqConfig mqConfig = null;

            try
            {
                if (messageBytes == null || messageBytes.Length == 0)
                {
                    throw new Exception("消息内容为空");
                }

                //获取当前mq的配置
                mqConfig = GetCurrentUsedRmq();
                string mqConfigStr = mqConfig.ToString();

                #region 判断是否需要创建exchange
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

                #endregion

                IConnection conn = null;

                lock (_locker)
                {
                    conn = _curUsedConnection;
                }

                if (conn == null)
                {
                    throw new Exception("当前连接为null");
                }

                using (var channel = conn.CreateModel())
                {
                    if (needCreateExchange)
                    {
                        channel.ExchangeDeclare(exchange: this._exchangeName, type: this._exchangeType, durable: true, autoDelete: false, arguments: null);
                        _exchangeCreatedDict.AddOrUpdate(mqConfigStr, true, (key, oldV) => true);
                    }



                    IBasicProperties properties = channel.CreateBasicProperties();
                    //定义消息的持久性
                    properties.Persistent = true;

                    //设置单个消息的过期时间单位毫秒
                    properties.Expiration = MessageExpirationInMilliseconds.ToString();//两个小时

                    //Direct Exchange将会把消息发送到和routingkey一样的队列中
                    //使用指定的routing key发送到指定的exchange
                    channel.BasicPublish(exchange: this._exchangeName, routingKey: routingKey, basicProperties: properties, body: messageBytes);
                }

                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);

                return false;
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
                    if (_timerNonAliveCount > 1 || _rmqConfigList.Count==1)
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

                        _mqChangeCallback?.Invoke(oldConfig, newConfig);
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


                IConnection tempConnect = conFactory.CreateConnection();

                lock (_locker)
                {
                    _curUsedConnection = tempConnect;
                }


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
            lock (_locker)
            {
                tempConnection = _curUsedConnection;
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
                _curUsedConnection = null;
            }
        }

        #endregion

        #region IDispose接口

        /// <summary>
        /// 停止
        /// </summary>
        private void Stop()
        {
            if (_oneRunTimer != null)
            {
                //关闭定时器
                _oneRunTimer.Close();
            }

            StopMq();
        }

        /// <summary>
        /// Dispose接口
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        #endregion
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;
using RabbitMQ.Client;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady
{
    /// <summary>
    /// 支持集群的短链接生产者
    /// 
    /// 基本思想:
    /// 随机选择一个节点，如果该节点挂了，重新选择下一个节点
    /// </summary>
    public class ShortConnectionClusterSupportProducer
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
        private readonly ConcurrentDictionary<string,bool> _exchangeCreatedDict = new ConcurrentDictionary<string, bool>();
        
        /// <summary>
        /// connectionFactory的字典
        /// </summary>
        private readonly ConcurrentDictionary<string,ConnectionFactory> _conFactoryDict = new ConcurrentDictionary<string, ConnectionFactory>();

        /// <summary>
        /// mq切换事件回调 第一个参数是旧的mq，第二个参数是新的mq
        /// </summary>
        private readonly Action<RabbitMqConfig, RabbitMqConfig> _mqChangeCallback = null;

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
        public ShortConnectionClusterSupportProducer(List<RabbitMqConfig> rmqList,bool createExchange,string exchangeName,string exchangeType=ExchangeType.Direct,Action<RabbitMqConfig,RabbitMqConfig> mqChangeCallBack=null)
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
        }


        #endregion


        #region 发送消息

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="routingKey">发送的routing key</param>
        /// <param name="messageBytes">消息内容，最大2g</param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool SendMesssage(string routingKey, byte[] messageBytes, Action<Exception> exceptionHandler = null)
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

                #region 获取ConnectionFactory

                ConnectionFactory conFactory = null;
                if (!_conFactoryDict.TryGetValue(mqConfigStr, out conFactory))
                {
                    //不自动重连
                    conFactory = new ConnectionFactory(){UserName = mqConfig.UserName,Password = mqConfig.Password,VirtualHost = mqConfig.VirtualHost,HostName = mqConfig.HostIp,Port = mqConfig.Port,AutomaticRecoveryEnabled = false};
                    _conFactoryDict.AddOrUpdate(mqConfigStr, conFactory, (key, oldV) => conFactory);
                }

                
                #endregion


                #region 判断是否需要创建exchange
                //判断是否需要创建exchange
                bool needCreateExchange = this._needToCreateExchange;
                if (needCreateExchange)
                {
                    bool alreadyCreated = false;
                    if (_exchangeCreatedDict.TryGetValue(mqConfigStr, out alreadyCreated)&&alreadyCreated)
                    {
                        needCreateExchange = false;
                    }
                }
                
                #endregion

                using (var connection = conFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        if (needCreateExchange)
                        {
                            channel.ExchangeDeclare(exchange:this._exchangeName,type:this._exchangeType,durable:true,autoDelete:false,arguments:null);
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
                }


                return true;
            }
            catch (Exception e)
            {
                lock (_locker)
                {
                    //发送错误，移到到下一个rmq
                    _currentUsedRmqIndex++;
                }

                if (this._mqChangeCallback != null)
                {
                    //mq切换回调
                    this._mqChangeCallback?.Invoke(mqConfig,GetCurrentUsedRmq());
                }

                exceptionHandler?.Invoke(e);
                return false;
            }
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

        #endregion
        
    }
}

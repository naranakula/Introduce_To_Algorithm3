using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.Consumers
{

    /// <summary>
    /// RabbitMQ的长连接消费者
    /// 
    /// 使用方式
    /// LongRunRabbitConsumer.StartConsumer()
    /// ..............
    /// 
    /// 
    /// LongRunRabbitConsumer.StopConsumer()
    /// RMQMessageHandler.Stop();
    /// 
    /// </summary>
    public static class LongRunRabbitConsumer
    {
        /*
         *  IModel: represents an AMQP 0-9-1 channel, and provides most of the operations (protocol methods).  不是多线程安全的，并行使用加锁
            IConnection: represents an AMQP 0-9-1 connection 多线程安全的
            ConnectionFactory: constructs IConnection instances 多线程安全的
            IBasicConsumer: represents a message consumer
         * 
         * 
         * 只使用RabbitMQ的两种特性：队列和主题(事件广播)
         * 
         * 队列：多个发送者，多个消费者，每个消息只能被消费一次
         * 主题: 多个发送者，多个消费者，每个消息都会被每个消费者消费一次
         * 
         * RabbitMQ只能发送byte[]
         * 
         * The default exchange is a direct exchange with no name (empty string) pre-declared by the broker. It has one special property that makes it very useful for simple applications: every queue that is created is automatically bound to it with a routing key which is the same as the queue name.
         * 
         */

        /*
         * AMQP模型:
         * https://www.rabbitmq.com/tutorials/amqp-concepts.html
         * 生产者将消息发送到exchange(类似于邮局),Exchange将消息放到队列.
         * Exchanges distribute message copies to queues using rules called bindings. 
         * 消费者从队列中获取消息.
         * Exchange类型
Name	Default pre-declared names
Direct exchange	(Empty string) and amq.direct
Fanout exchange	amq.fanout
Topic exchange	amq.topic
Headers exchange	amq.match (and amq.headers in RabbitMQ)
         * Exchange可以是持久的或者短暂的，持久的当机器重启后，仍然存在
         * |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
         * Direct Exchange
         * 直接交换根据Routing key将消息放到队列，标准是队列名和Routing key一样
         * |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
         * Fanout Exchange
         * Fanout Exchange是发布订阅模型，Fanout Exchange将消息放入所有绑定到它的队列，忽略Routing key.
         * 在Fanout Exchange中，Routing key不起作用
         * |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
         * Topic Exchange
         * Topic Exchange将消息放入0或多个队列，根据Routing key和queue的绑定的pattern.
         * 绑定的队列可以只获取Topic Exchange的一个子集
         * Messages sent to a topic exchange can't have an arbitrary routing_key - it must be a list of words, delimited by dots. The words can be anything, but usually they specify some features connected to the message. A few valid routing key examples: "stock.usd.nyse", "nyse.vmw", "quick.orange.rabbit". 
         * topic消息的routing key必须使用.隔开
         * topic中 *替换一个单词 #替换0个或多个单词
         * * (star) can substitute for exactly one word.
         * # (hash) can substitute for zero or more words.
         * |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
         * Headers Exchange
         * Headers Exchange根据Header的一个或者多个attribute来放置消息，而不是根据Routing key
         * Headers exchanges ignore the routing key attribute. Instead, the attributes used for routing are taken from the headers attribute. 
         * 可以全部匹配也可以只匹配一个header的attribute
         * 
         * Queue队列
         * Queue有如下属性：
         * name:名字 最长255 utf8字符串;  Durable:持久性  Exclusive:排他性，只能在本连接中使用 Auto-delete:自动删除
         * Queue declaration will have no effect if the queue does already exist and its attributes are the same as those in the declaration. When the existing queue attributes are not the same as those in the declaration a channel-level exception with code 406 (PRECONDITION_FAILED) will be raised.
         * Queue names starting with "amq." are reserved for internal use by the broker. Attempts to declare a queue with a name that violates this rule will result in a channel-level exception with reply code 403 (ACCESS_REFUSED).
         * Durable queues are persisted to disk and thus survive broker restarts.
         * Durability of a queue does not make messages that are routed to that queue durable.
         * 
         * Bindings
         * Binding定义Exchange route消息到queue的规则。
         * 
         * Consumers
         * 有两种:push 和 pull  Have messages delivered to them ("push API") Fetch messages as needed ("pull API")
         * 自动通知:当消息发送到客户端时，从queue中删除
         * 手动通知:当客户端通知时，从queue中删除
         * 
         * 消息属性,消息具有如下属性(不限于):
Content type
Content encoding
Routing key
Delivery mode (persistent or not)
Message priority
Message publishing timestamp
Expiration period
Publisher application id
         * 
         * Connections
         * 表示一个tcp连接
         * 
         * Channels
         * tcp连接的一个多路复用channel
         * hannels can be thought of as "lightweight connections that share a single TCP connection".
         * 
         * Virtual Hosts
         * similar to virtual hosts used by many popular Web servers and provide completely isolated environments in which AMQP entities live.
         * 
         * 
         */



        // There are four building blocks you really care about in AMQP: virtual hosts, exchanges, queues and bindings. A virtual host holds a bundle of exchanges, queues and bindings.
        //Why would you want multiple virtual hosts? Easy. A username in RabbitMQ grants you access to a virtual host…in its entirety. So the only way to keep group A from accessing group B’s exchanges/queues/bindings/etc. is to create a virtual host for A and one for B. 
        //Virtualhost是虚拟主机,类似于命名空间
        //Rabbitmq的默认用户guest/guest，具有管理员权限 //guest拥有管理员权限，但只能本地访问,已测试
        private static readonly ConnectionFactory SFactory = new ConnectionFactory() { UserName = "admin", Password = "admin", VirtualHost = ConnectionFactory.DefaultVHost, HostName = "192.168.163.12", Port = 5672, AutomaticRecoveryEnabled = true/*自动重连*/, NetworkRecoveryInterval = TimeSpan.FromMilliseconds(5753)/*自动重连的时间间隔默认5s*/, RequestedConnectionTimeout = ConnectionFactory.DefaultConnectionTimeout, SocketReadTimeout = ConnectionFactory.DefaultConnectionTimeout, SocketWriteTimeout = ConnectionFactory.DefaultConnectionTimeout/*以上三个值就是不设置时使用的默认的值,30s*/,RequestedHeartbeat = ConnectionFactory.DefaultHeartbeat/*心跳检测，默认是60s*/};

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object SLocker = new object();

        /// <summary>
        /// 连接 多线程安全
        /// 表示一个TCP连接
        /// Each IConnection instance is, in the current implementation, backed by a single background thread that reads from the socket and dispatches the resulting events to the application
        /// </summary>
        private static volatile IConnection _connection = null;

        /// <summary>
        /// Common AMQP model, the union of the functionality 
        /// 一个tcp连接下的多路复用通道
        /// 不是多线程安全的，多线程使用必须加锁
        /// </summary>
        private static volatile IModel _channel = null;

        /// <summary>
        /// 是否存活
        /// </summary>
        private static volatile bool _isAlive = false;

        /// <summary>
        /// 是否底层MQ存活
        /// </summary>
        /// <returns></returns>
        public static bool IsAlive()
        {
            lock (SLocker)
            {
                return _isAlive;
            }
        }

        #region Exchange类型
        
        //使用类ExchangeType

        /// <summary>
        /// Direct exchange 类型
        /// </summary>
        public const string DirectExchangeTypeName = "direct";

        /// <summary>
        /// fanout exchange 类型
        /// </summary>
        public const string FanoutExchangeTypeName = "fanout";

        /// <summary>
        /// topic exchange 类型
        /// </summary>
        public const string TopicExchangeTypeName = "topic";

        #endregion


        #region 配置相关

        /// <summary>
        /// true表示使用queue,false表示使用主题
        /// 类似于ActiveMQ的抽象，实际上RabbitMQ只有队列
        /// </summary>
        private static readonly bool _isQueue = true;

        /// <summary>
        /// 当是Direct类型的Exchange时，使用的队列名称 不要使用""，因为它是默认的，并且每个队列自动绑定到它""上 非""不自动绑定
        /// </summary>
        private static readonly string _usedDirectQueueName = ConfigUtils.GetString("UsedDirectQueueName");
        

        /// <summary>
        /// 消费者使用的Exchange的名字
        /// </summary>
        private static readonly string _usedExchangeName = ConfigUtils.GetString("UsedExchangeName");

        /// <summary>
        /// 使用的RoutingKey
        /// </summary>
        private static readonly string _usedRoutingKeyName = ConfigUtils.GetString("UsedRoutingKey");


        #endregion

        /// <summary>
        /// 开启消费者
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool StartConsumer(Action<Exception> exceptionHandler = null)
        {
            try
            {
                StopConsumer();

                _connection = SFactory.CreateConnection();

                //定义连接事件
                //连接阻塞的回调
                _connection.ConnectionBlocked += ConnectionOnConnectionBlocked;
                //连接未阻塞的回调
                _connection.ConnectionUnblocked += ConnectionOnConnectionUnblocked;
                //连接回调异常
                _connection.CallbackException += ConnectionOnCallbackException;
                //连接恢复错误
                _connection.ConnectionRecoveryError += ConnectionOnConnectionRecoveryError;
                //连接恢复成功
                _connection.RecoverySucceeded += ConnectionOnRecoverySucceeded;
                //连接关闭
                _connection.ConnectionShutdown += ConnectionOnConnectionShutdown;


                //创建多路复用的channel
                _channel = _connection.CreateModel();

                if (_isQueue)
                {
                    // 1、创建exchange
                    //使用direct
                    //exchange:exchange的名字，//type:exchange的类型
                    //durable - true if we are declaring a durable exchange (the exchange will survive a server restart)
                    //autoDelete - true if the server should delete the exchange when it is no longer in use
                    //arguments - other properties (construction arguments) for the exchange
                    _channel.ExchangeDeclare(exchange: _usedExchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    //队列
                    //queue:队列名 
                    //durable:true if we are declaring a durable queue (the 队列 will survive a server restart) 当mq重启后队列是否仍然存在
                    //exclusive:排他性，如果为true,表示队列只能被当前连接使用，连接关闭队列自动消失
                    //autoDelete:当已经没有消费者时，服务器是否可以删除该队列,true if the server should delete the queue when it is no longer in use。
                   //队列创建时会默认绑定到default的direct的""的exchange
                    _channel.QueueDeclare(queue: _usedDirectQueueName, durable: true, exclusive: false, autoDelete: false,arguments: null);

                    //建立queue和exchange的绑定
                    _channel.QueueBind(queue: _usedDirectQueueName, exchange: _usedExchangeName, routingKey: _usedRoutingKeyName, arguments: null);

                    //prefetchSize - maximum amount of content (measured in octets) that the server will deliver, 0 if unlimited
                    //prefetchCount - maximum number of messages that the server will deliver, 0 if unlimited
                    //global - true if the settings should be applied to the entire channel rather than each consumer
                    //每次取一个消息，消息大小不限制
                    //注释掉该行，将会使用Robbin轮盘分配，使用下面这行，只有空闲的Consumer接收消息
                    //_channel.BasicQos(prefetchSize:0,prefetchCount:1,global:false);
                    
                    //定义消息接收事件
                    EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
                    consumer.Received += ConsumerOnReceived;

                    //自动应答
                    _channel.BasicConsume(queue:_usedDirectQueueName, autoAck:true, consumer:consumer);


                    //同步调用
                    //BasicGetResult getResult = _channel.BasicGet(queue:_usedDirectQueueName, autoAck: true);
                    //if(getResult == null)
                    //{
                    //    //没有消息
                    //}
                    //else
                    //{
                    //    byte[] bodys = getResult.Body;
                    //}
                }
                else
                {
                    // 1、创建exchange
                    //使用fanout
                    //exchange:exchange的名字，//type:exchange的类型
                    //durable - true if we are declaring a durable exchange (the exchange will survive a server restart)
                    //autoDelete - true if the server should delete the exchange when it is no longer in use
                    //arguments - other properties (construction arguments) for the exchange
                    _channel.ExchangeDeclare(exchange:_usedExchangeName,type:ExchangeType.Fanout,durable:true,autoDelete:false,arguments:null);

                    //2、创建queue
                    //由MQ服务器创建一个amp.开头的(如amq.gen-JzTY20BRgKO-HjmUJj0wLg)非持久、排他、自动删除的队列
                    //连接断开后，服务器将删除该队列
                    string tempQueueName = _channel.QueueDeclare().QueueName;

                    //3、绑定queue
                    //将队列绑定到exchange
                    //fanout的routingkey不起作用，将会被忽略
                    //使用指定的Routingkey将指定的queue绑定到指定的exchange上
                    //如果关心多种消息，一个channel可以创建多个bind，可以使用相同的队列名(一个队列关心多个routingKey)
                    _channel.QueueBind(queue:tempQueueName,exchange:_usedExchangeName,routingKey:"",arguments:null);

                    //定义消息接收事件
                    EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
                    consumer.Received += ConsumerOnReceived;

                    //自动应答
                    _channel.BasicConsume(queue: tempQueueName, autoAck: true, consumer: consumer);
                }

                lock (SLocker)
                {
                    _isAlive = true;
                }

                return true;
            }
            catch (Exception e)
            {

                lock (SLocker)
                {
                    _isAlive = false;
                }
                exceptionHandler?.Invoke(e);
                return false;
            }
        }
        

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
                    CreateTime =  DateTime.Now
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

            lock (SLocker)
            {
                _isAlive = false;
            }

            NLogHelper.Warn($"OnConnectionShutdown");
        }

        /// <summary>
        /// 连接恢复成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void ConnectionOnRecoverySucceeded(object sender, EventArgs eventArgs)
        {

            lock (SLocker)
            {
                _isAlive = true;
            }
            NLogHelper.Warn($"OnRecoverySucceeded");
        }

        /// <summary>
        /// 连接恢复失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connectionRecoveryErrorEventArgs"></param>
        private static void ConnectionOnConnectionRecoveryError(object sender, ConnectionRecoveryErrorEventArgs connectionRecoveryErrorEventArgs)
        {

            lock (SLocker)
            {
                _isAlive = false;
            }

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

        /// <summary>
        /// 停止消费者
        /// </summary>
        public static void StopConsumer()
        {

            if (_channel != null)
            {
                SafeInvokeUtils.Safe(() =>
                {
                    _channel.Dispose();
                }, ex =>
                {
                    NLogHelper.Error($"关闭RabbitMQ的channel失败:{ex}");
                });

                _channel = null;
            }

            if (_connection != null)
            {
                SafeInvokeUtils.Safe(() =>
                {
                    _connection.Dispose();
                }, ex =>
                {
                    NLogHelper.Error($"关闭RabbitMQ的connection失败:{ex}");
                });
                _connection = null;
            }

        }



    }
}

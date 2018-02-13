using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.Consumers
{

    /// <summary>
    /// RabbitMQ的长连接消费者
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
         * 
         * 
         */

        /*
         * AMQP模型:
         * https://www.rabbitmq.com/tutorials/amqp-concepts.html
         * 生产者将消息发送到exchange(类似于邮局),Exchange将消息放到队列.
         * Exchanges distribute message copies to queues using rules called bindings. 
         * 消费者从队列中获取消息.
         * 
         * 
         * 
         */



        // There are four building blocks you really care about in AMQP: virtual hosts, exchanges, queues and bindings. A virtual host holds a bundle of exchanges, queues and bindings.
        //Why would you want multiple virtual hosts? Easy. A username in RabbitMQ grants you access to a virtual host…in its entirety. So the only way to keep group A from accessing group B’s exchanges/queues/bindings/etc. is to create a virtual host for A and one for B. 
        //Virtualhost是虚拟主机,类似于命名空间
        private static readonly ConnectionFactory SFactory = new ConnectionFactory() { UserName = "guest", Password = "guest", VirtualHost = ConnectionFactory.DefaultVHost, HostName = "192.168.163.14", Port = 5672, AutomaticRecoveryEnabled = true/*自动重连*/, NetworkRecoveryInterval = TimeSpan.FromMilliseconds(4753)/*自动重连的时间间隔默认5s*/, RequestedConnectionTimeout = ConnectionFactory.DefaultConnectionTimeout, SocketReadTimeout = ConnectionFactory.DefaultConnectionTimeout, SocketWriteTimeout = ConnectionFactory.DefaultConnectionTimeout/*以上三个值就是不设置时使用的默认的值*/};

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object SLocker = new object();

        /// <summary>
        /// 连接 多线程安全
        /// </summary>
        private static volatile IConnection _connection = null;

        /// <summary>
        /// Common AMQP model, the union of the functionality 
        /// 一个连接下的通道
        /// </summary>
        private static volatile IModel _channel = null;

        #region 配置相关
        
        /// <summary>
        /// true表示使用queue,false表示使用主题
        /// </summary>
        private static readonly bool _isQueue = true;


        #endregion


    }
}

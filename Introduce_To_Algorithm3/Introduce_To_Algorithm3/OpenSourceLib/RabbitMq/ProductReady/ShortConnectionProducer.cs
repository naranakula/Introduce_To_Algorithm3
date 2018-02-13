using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady
{
    public class ShortConnectionProducer
    {
        /*
         *  IModel: represents an AMQP 0-9-1 channel, and provides most of the operations (protocol methods).  不是多线程安全的，并行使用加锁
            IConnection: represents an AMQP 0-9-1 connection 多线程安全的
            ConnectionFactory: constructs IConnection instances 多线程安全的
            IBasicConsumer: represents a message consumer
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         */

        // There are four building blocks you really care about in AMQP: virtual hosts, exchanges, queues and bindings. A virtual host holds a bundle of exchanges, queues and bindings.
        //Why would you want multiple virtual hosts? Easy. A username in RabbitMQ grants you access to a virtual host…in its entirety. So the only way to keep group A from accessing group B’s exchanges/queues/bindings/etc. is to create a virtual host for A and one for B. 
        //Virtualhost是虚拟主机,类似于命名空间
        //guest拥有管理员权限，但只能本地访问,已测试
        private static readonly ConnectionFactory SFactory = new ConnectionFactory() { UserName = "admin", Password = "admin", VirtualHost = ConnectionFactory.DefaultVHost, HostName = "192.168.163.12", Port = 5672, AutomaticRecoveryEnabled = false/*不自动重连*/, RequestedConnectionTimeout = ConnectionFactory.DefaultConnectionTimeout, SocketReadTimeout = ConnectionFactory.DefaultConnectionTimeout, SocketWriteTimeout = ConnectionFactory.DefaultConnectionTimeout/*以上三个值就是不设置时使用的默认的值*/};
        
        /// <summary>
        /// 默认mq创建的Direct类型的Exchange的名字
        /// </summary>
        public const string DefaultDirectExchangeName = "";

        /// <summary>
        /// Direct Exchange类型
        /// </summary>
        private const string DirectExchangeTypeName = "direct";

        /// <summary>
        /// Fanout Exchange
        /// </summary>
        private const string FanoutExchangeTypeName = "fanout";

        /// <summary>
        /// 发送队列消息
        /// 发送到direct exchang的默认的""exchange中
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="messageBytes"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool SendQueueMessage(string queueName, byte[] messageBytes,
            Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (var connection = SFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //queue:队列名 
                        //durable:true if we are declaring a durable 队列 (the 队列 will survive a server restart) 当mq重启后队列是否仍然存在
                        //exclusive:排他性，如果为true,表示队列只能被当前连接使用，连接关闭队列自动消失
                        //autoDelete:当已经没有消费者时，服务器是否可以删除该队列,true if the server should delete the queue when it is no longer in use。
                        //该queue默认会绑定到direct exchange的默认的实例""上，除非你显式的调用QueueBind
                        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false,
                            arguments: null);
                        //上面这一步是可选的



                        IBasicProperties properties = channel.CreateBasicProperties();
                        //定义消息的持久性
                        properties.Persistent = true;

                        //设置单个消息的过期时间单位毫秒
                        properties.Expiration = "7200000";//两个小时

                        //Direct Exchange将会把消息发送到和routingkey一样的队列中
                        //使用指定的routing key发送到指定的exchange
                        channel.BasicPublish(exchange: DefaultDirectExchangeName, routingKey: queueName, basicProperties: properties, body: messageBytes);
                    }
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
        /// 发送Fanout topic消息
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="messageBytes"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool SendFanoutTopicMessage(string exchangeName, byte[] messageBytes,
            Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (var connection = SFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //使用fanout
                        //exchange:exchange的名字
                        //type:exchange的类型
                        //durable - true if we are declaring a durable exchange (the exchange will survive a server restart)
                        //autoDelete - true if the server should delete the exchange when it is no longer in use
                        //arguments - other properties (construction arguments) for the exchange
                        channel.ExchangeDeclare(exchange: exchangeName, type: FanoutExchangeTypeName, durable: true, autoDelete: false, arguments: null);


                        IBasicProperties properties = channel.CreateBasicProperties();
                        //定义消息的持久性
                        properties.Persistent = false;
                        //properties.Expiration

                        //当fanout类型，routingKey不起作用，将被忽略
                        //使用指定的routing key发送到指定的exchange
                        channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: properties, body: messageBytes);

                    }
                }

                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }


    }
}

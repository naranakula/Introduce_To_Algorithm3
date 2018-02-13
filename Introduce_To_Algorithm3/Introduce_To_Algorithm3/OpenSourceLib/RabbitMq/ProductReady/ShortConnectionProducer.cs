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
        private static readonly ConnectionFactory SFactory = new ConnectionFactory() { UserName = "guest", Password = "guest", VirtualHost = ConnectionFactory.DefaultVHost, HostName = "192.168.163.14", Port = 5672, AutomaticRecoveryEnabled = false/*不自动重连*/, RequestedConnectionTimeout = ConnectionFactory.DefaultConnectionTimeout, SocketReadTimeout = ConnectionFactory.DefaultConnectionTimeout, SocketWriteTimeout = ConnectionFactory.DefaultConnectionTimeout/*以上三个值就是不设置时使用的默认的值*/};

        /// <summary>
        /// 
        /// </summary>
        private const string DefaultExchangeName = "";

        /// <summary>
        /// 发送队列消息
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
                        //queue:队列名 durable:true if we are declaring a durable exchange (the exchange will survive a server restart) 
                        //exclusive:排他性，如果为true,表示队列只能被当前连接使用，连接关闭队列自动消失
                        //autoDelete:当已经没有消费者时，服务器是否可以删除该Exchange,true if the server should delete the exchange when it is no longer in use。
                        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false,
                            arguments: null);

                        IBasicProperties properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        //properties.Expiration

                        channel.BasicPublish(exchange: DefaultExchangeName, routingKey: queueName, basicProperties: properties, body: messageBytes);
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

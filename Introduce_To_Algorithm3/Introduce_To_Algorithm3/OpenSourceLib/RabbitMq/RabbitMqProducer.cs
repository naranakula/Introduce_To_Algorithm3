using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq
{
    public static class RabbitMqProducer
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        private const string QueueName = "Hello2";

        /// <summary>
        /// HostName指定IP地址,port不设置使用默认的端口
        /// </summary>
        private static readonly ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost",AutomaticRecoveryEnabled = true};

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public static void Send(string message)
        {
            var factory = new ConnectionFactory(){HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                //建议根据实际情况自己实现客户端选举
                //connection.IsOpen
                //连接集群
                //factory.CreateConnection(new List<string>() {"", ""});
                //建议断线时自己写选择逻辑
                //Connection抽象socket连接
                using (var channel = connection.CreateModel())
                {
                    //如果队列不存在，创建一个队列，声明队列是幂等的，如果没有创建，新建，否则什么也不做
                    //Declaring a queue is idempotent - it will only be created if it doesn't exist already. 
                    channel.QueueDeclare(QueueName,false,false,false,null);
                    //消息是二进制的
                    byte[] body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("",QueueName,null,body);
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public static void Send2(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //如果队列不存在，创建一个队列
                    channel.QueueDeclare(QueueName, true, false, false, null);

                    byte[] body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;
                    channel.BasicPublish("", QueueName, properties, body);
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public static void Send3(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("logs","fanout");
                    byte[] body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("logs", "", null, body);
                }
            }
        }

        /// <summary>
        /// direct
        /// </summary>
        /// <param name="msg"></param>
        public static void Send4(string msg)
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};

            using(var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("direct_logs","direct");
                    var serverity = "info";
                    channel.BasicPublish("direct_logs",serverity,null,Encoding.UTF8.GetBytes(msg));
                }
            }
        }


        public static void Send5(string msg)
        {
            var factory = new ConnectionFactory(){HostName = "localhost"};

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("topic_logs","topic");

                    channel.BasicPublish("topic_logs","anonymous.info",null,Encoding.UTF8.GetBytes(msg));
                }
            }
        }

    }


    public static class RabbitMqConsumer
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        private const string QueueName = "Hello2";

        public static void Receive()
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //因为有可能先启动接收者，所以预先创建队列
                    channel.QueueDeclare(QueueName, false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received +=Consumer_Received;
                    channel.BasicConsume(QueueName, true, consumer);

                    Console.ReadLine();
                }
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine(message);
        }

        public static void Receive2()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //因为有可能先启动接收者，所以预先创建队列
                    channel.QueueDeclare(QueueName, true, false, false, null);
                    channel.BasicQos(0,1,false);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, args) =>
                    {
                        var buffer = args.Body;
                        string message = Encoding.UTF8.GetString(buffer);
                        Console.WriteLine(message);
                        channel.BasicAck(args.DeliveryTag,false);
                    };
                    channel.BasicConsume(QueueName, false, consumer);

                    Console.ReadLine();
                }
            }
        }

        public static void Receive3()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //因为有可能先启动接收者，所以预先创建队列
                    channel.ExchangeDeclare("logs", "fanout");
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queueName,"logs","");
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, args) =>
                    {
                        var buffer = args.Body;
                        string message = Encoding.UTF8.GetString(buffer);
                        Console.WriteLine(message);
                    };
                    channel.BasicConsume(queueName, true, consumer);
                    Console.ReadLine();
                    Console.ReadLine();
                    Console.ReadLine();
                }
            }
        }


        public static void Receive4()
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("direct_logs","direct");
                    var queueName = channel.QueueDeclare().QueueName;
                    string[] arrs = new[] {"info", "error", "warn"};
                    foreach (var item in arrs)
                    {
                        channel.QueueBind(queueName,"direct_logs",item);
                    }

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        Console.WriteLine( string.Format("{0}:{1}",ea.RoutingKey,Encoding.UTF8.GetString(ea.Body)));
                    };
                    channel.BasicConsume(queueName, true, consumer);
                    Console.ReadLine();
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// topic
        /// </summary>
        public static void Receive5()
        {
            var factory = new ConnectionFactory(){HostName = "localhost"};

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("topic_logs","topic");
                    var queueName = channel.QueueDeclare().QueueName;
                    string[] arr = new[] {"*.info", "#.error"};
                    foreach (var item in arr)
                    {
                        channel.QueueBind(queueName,"topic_logs",item);
                    }

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, args) =>
                    {
                        Console.WriteLine(string.Format("{0}:{1}",args.RoutingKey,Encoding.UTF8.GetString(args.Body)));
                    };

                    channel.BasicConsume(queueName, true, consumer);
                    Console.ReadLine();
                    Console.ReadLine();
                }
            }
        }


        public static void Receive6()
        {
            var factory = new ConnectionFactory(){HostName = "localhost",AutomaticRecoveryEnabled = true};
       
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("rpc_queue", false, false, false, null);
                    channel.BasicQos(0,1,false);

                    var consumer = new QueueingBasicConsumer(channel);

                    channel.BasicConsume("rpc_queue", false, consumer);
                    while (true)
                    {
                        string response = null;
                        var ea = (BasicDeliverEventArgs) consumer.Queue.Dequeue();
                        var body = ea.Body;
                        var props = ea.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;
                        string message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("received:"+message);
                        response = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        channel.BasicPublish("",props.ReplyTo,replyProps,Encoding.UTF8.GetBytes(response));
                        channel.BasicAck(ea.DeliveryTag,false);
                    }
                }
            }
        }

    }


    public class RPCClient
    {
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;

        public RPCClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare();
            consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(replyQueueName, true, consumer);
        }

        public string Call(string message)
        {
            var corrId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", "rpc_queue", props, messageBytes);

            while (true)
            {
                var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId == corrId)
                {
                    return Encoding.UTF8.GetString(ea.Body);
                }
            }
        }

        public void Close()
        {
            connection.Close();
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq
{
    public static class RabbitMqProducer
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        private const string QueueName = "Hello2";

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public static void Send(string message)
        {
            var factory = new ConnectionFactory(){HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //如果队列不存在，创建一个队列
                    channel.QueueDeclare(QueueName,false,false,false,null);

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

    }


}

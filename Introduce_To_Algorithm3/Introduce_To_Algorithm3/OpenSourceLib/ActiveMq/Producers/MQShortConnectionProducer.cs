using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq.Producers
{
    /// <summary>
    /// MQ产生者
    /// 每次发送消息一个连接
    /// </summary>
    public class MQShortConnectionProducer
    {
        /// <summary>
        /// MQ地址
        /// </summary>
        private const string MQUri = "MQUri";
        /// <summary>
        /// 创建连接工厂
        /// </summary>
        private static IConnectionFactory factory = new ConnectionFactory(ConfigUtils.GetString(MQUri));

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendQueueMessage(string msg)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var session = connection.CreateSession())
                {
                    using (
                        var producer =
                            session.CreateProducer(session.GetDestination(ConfigUtils.GetString("DestQueue"),//queue的名称 
                                DestinationType.Queue)))
                    {
                        connection.Start();
                        ITextMessage message = session.CreateTextMessage(msg);
                        //The timestamp of when the message was pubished in UTC time. If the publisher disables setting the timestamp on the message, the time will be set to the start of the UNIX epoc (1970-01-01 00:00:00).
                        message.NMSTimestamp = DateTime.UtcNow;
                        //The amount of time for which this message is valid.
                        message.NMSTimeToLive = TimeSpan.FromHours(2);
                        producer.Send(message);
                    }
                }
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendTopicMessage(string msg)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var session = connection.CreateSession())
                {
                    using (
                        var producer =
                            session.CreateProducer(session.GetDestination(ConfigUtils.GetString("DestTopic"),//topic名称
                                DestinationType.Topic)))
                    {
                        connection.Start();
                        ITextMessage message = session.CreateTextMessage(msg);
                        //The timestamp of when the message was pubished in UTC time. If the publisher disables setting the timestamp on the message, the time will be set to the start of the UNIX epoc (1970-01-01 00:00:00).
                        message.NMSTimestamp = DateTime.UtcNow;
                        //The amount of time for which this message is valid.
                        message.NMSTimeToLive = TimeSpan.FromMinutes(20);
                        producer.Send(message);
                    }
                }
            }
        }

    }

}

using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
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
        private static readonly IConnectionFactory factory = new ConnectionFactory(ConfigUtils.GetString(MQUri));

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendQueueMessage(string msg,Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (var connection = factory.CreateConnection())
                {
                    //超时16s
                    connection.RequestTimeout = new TimeSpan(0, 0, 16);
                    using (var session = connection.CreateSession())
                    {
                        using (
                            var producer =
                                session.CreateProducer(session.GetDestination(
                                    ConfigUtils.GetString("DestQueue"), //queue的名称 
                                    DestinationType.Queue)))
                        {
                            connection.Start();
                            ITextMessage message = session.CreateTextMessage(msg);
                            ////The timestamp of when the message was pubished in UTC time. If the publisher disables setting the timestamp on the message, the time will be set to the start of the UNIX epoc (1970-01-01 00:00:00).
                            //message.NMSTimestamp = DateTime.UtcNow;
                            ////The amount of time for which this message is valid.
                            //message.NMSTimeToLive = TimeSpan.FromHours(2);
                            ////是否持久消息
                            //message.NMSDeliveryMode = MsgDeliveryMode.Persistent;
                            //producer.Send(message);

                            producer.Send(message, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.FromHours(2));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendTopicMessage(string msg,Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (var connection = factory.CreateConnection())
                {
                    //超时16s
                    connection.RequestTimeout = new TimeSpan(0, 0, 16);
                    using (var session = connection.CreateSession())
                    {
                        using (
                            var producer =
                                session.CreateProducer(session.GetDestination(ConfigUtils.GetString("DestTopic"),//topic名称
                                    DestinationType.Topic)))
                        {
                            connection.Start();
                            ITextMessage message = session.CreateTextMessage(msg);
                            ////The timestamp of when the message was pubished in UTC time. If the publisher disables setting the timestamp on the message, the time will be set to the start of the UNIX epoc (1970-01-01 00:00:00).
                            //message.NMSTimestamp = DateTime.UtcNow;
                            ////The amount of time for which this message is valid.
                            //message.NMSTimeToLive = TimeSpan.FromMinutes(20);
                            ////是否持久消息
                            //message.NMSDeliveryMode = MsgDeliveryMode.Persistent;

                            producer.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.FromMinutes(30));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
            
        }

    }

}

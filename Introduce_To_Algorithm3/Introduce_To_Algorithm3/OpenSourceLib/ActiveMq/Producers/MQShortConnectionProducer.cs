using System;
using System.Collections.Generic;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq.Producers
{
    /// <summary>
    /// MQ产生者
    /// 每次发送消息一个连接
    /// 长连接uri使用failvoer,短链接不要使用
    /// 根据jms说明ConnectionFactory和Connection是Thread safe的，其它的不是
    /// </summary>
    public class MQShortConnectionProducer
    {
        /// <summary>
        /// MQ地址
        /// </summary>
        private const string MQUri = "MQUri";
        /// <summary>
        /// 创建连接工厂  多线程安全
        /// </summary>
        private static readonly IConnectionFactory factory = new ConnectionFactory(ConfigUtils.GetString(MQUri));

        /// <summary>
        /// 发送的目标队列
        /// </summary>
        private static readonly string destQueueName = ConfigUtils.GetString("DestQueue");

        /// <summary>
        /// 发送的目标主题
        /// </summary>
        private static readonly string destTopicName = ConfigUtils.GetString("DestTopic");

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendQueueMessage(string msg, Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    //超时16s
                    connection.RequestTimeout = new TimeSpan(0, 0, 16);
                    //start应该放在这
                    connection.Start();
                    using (var session = connection.CreateSession())
                    {
                        using (
                            var producer =
                                session.CreateProducer(session.GetDestination(
                                    destQueueName, //queue的名称 
                                    DestinationType.Queue)))
                        {
                            //start不能放在这
                            //connection.Start();
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
        public static void SendTopicMessage(string msg, Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    //超时16s
                    connection.RequestTimeout = new TimeSpan(0, 0, 16);
                    //start应该放在这
                    connection.Start();
                    using (var session = connection.CreateSession())
                    {
                        using (
                            var producer =
                                session.CreateProducer(session.GetDestination(destTopicName,//topic名称
                                    DestinationType.Topic)))
                        {
                            //start不能放在这
                            //connection.Start();
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


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendQueueMessage(byte[] msg, Action<Exception> exceptionHandler = null)
        {
            if (msg == null || msg.Length == 0)
            {
                return;
            }

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    //超时16s
                    connection.RequestTimeout = new TimeSpan(0, 0, 16);
                    //start应该放在这
                    connection.Start();
                    using (var session = connection.CreateSession())
                    {
                        using (
                            var producer =
                                session.CreateProducer(session.GetDestination(
                                    destQueueName, //queue的名称 
                                    DestinationType.Queue)))
                        {
                            //start不能放在这
                            //connection.Start();
                            IBytesMessage message = session.CreateBytesMessage(msg);
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
        public static void SendTopicMessage(byte[] msg, Action<Exception> exceptionHandler = null)
        {
            if (msg == null || msg.Length == 0)
            {
                return;
            }

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    //超时16s
                    connection.RequestTimeout = new TimeSpan(0, 0, 16);
                    //start应该放在这
                    connection.Start();
                    using (var session = connection.CreateSession())
                    {
                        using (
                            var producer =
                                session.CreateProducer(session.GetDestination(destTopicName,//topic名称
                                    DestinationType.Topic)))
                        {
                            //start不能放在这
                            //connection.Start();
                            IBytesMessage message = session.CreateBytesMessage(msg);
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


        #region 批量发送


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgList"></param>
        public static void SendQueueMessage(List<string> msgList, Action<Exception> exceptionHandler = null)
        {
            if (msgList == null || msgList.Count == 0)
            {
                return;
            }

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    //超时16s
                    connection.RequestTimeout = new TimeSpan(0, 0, 16);
                    //start应该放在这
                    connection.Start();
                    using (var session = connection.CreateSession())
                    {
                        using (
                            var producer =
                                session.CreateProducer(session.GetDestination(
                                    destQueueName, //queue的名称 
                                    DestinationType.Queue)))
                        {
                            foreach (string msg in msgList)
                            {
                                //start不能放在这
                                //connection.Start();
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
        /// <param name="msgList"></param>
        public static void SendTopicMessage(List<string> msgList, Action<Exception> exceptionHandler = null)
        {
            if (msgList == null || msgList.Count == 0)
            {
                return;
            }

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    //超时16s
                    connection.RequestTimeout = new TimeSpan(0, 0, 16);
                    //start应该放在这
                    connection.Start();
                    using (var session = connection.CreateSession())
                    {
                        using (
                            var producer =
                                session.CreateProducer(session.GetDestination(destTopicName,//topic名称
                                    DestinationType.Topic)))
                        {
                            foreach (string msg in msgList)
                            {
                                //start不能放在这
                                //connection.Start();
                                ITextMessage message = session.CreateTextMessage(msg);
                                ////The timestamp of when the message was pubished in UTC time. If the publisher disables setting the timestamp on the message, the time will be set to the start of the UNIX epoc (1970-01-01 00:00:00).
                                //message.NMSTimestamp = DateTime.UtcNow;
                                ////The amount of time for which this message is valid.
                                //message.NMSTimeToLive = TimeSpan.FromMinutes(20);
                                ////是否持久消息
                                //message.NMSDeliveryMode = MsgDeliveryMode.Persistent;

                                producer.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal,
                                    TimeSpan.FromMinutes(30));
                            }
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
        /// <param name="msgList"></param>
        public static void SendQueueMessage(List<byte[]> msgList, Action<Exception> exceptionHandler = null)
        {
            if (msgList == null || msgList.Count == 0)
            {
                return;
            }

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    //超时16s
                    connection.RequestTimeout = new TimeSpan(0, 0, 16);
                    //start应该放在这
                    connection.Start();
                    using (var session = connection.CreateSession())
                    {
                        using (
                            var producer =
                                session.CreateProducer(session.GetDestination(
                                    destQueueName, //queue的名称 
                                    DestinationType.Queue)))
                        {
                            foreach (byte[] msg in msgList)
                            {
                                //start不能放在这
                                //connection.Start();
                                IBytesMessage message = session.CreateBytesMessage(msg);
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
        /// <param name="msgList"></param>
        public static void SendTopicMessage(List<byte[]> msgList, Action<Exception> exceptionHandler = null)
        {
            if (msgList == null || msgList.Count == 0)
            {
                return;
            }

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    //超时16s
                    connection.RequestTimeout = new TimeSpan(0, 0, 16);
                    //start应该放在这
                    connection.Start();
                    using (var session = connection.CreateSession())
                    {
                        using (
                            var producer =
                                session.CreateProducer(session.GetDestination(destTopicName,//topic名称
                                    DestinationType.Topic)))
                        {
                            foreach (byte[] msg in msgList)
                            {
                                //start不能放在这
                                //connection.Start();
                                IBytesMessage message = session.CreateBytesMessage(msg);
                                ////The timestamp of when the message was pubished in UTC time. If the publisher disables setting the timestamp on the message, the time will be set to the start of the UNIX epoc (1970-01-01 00:00:00).
                                //message.NMSTimestamp = DateTime.UtcNow;
                                ////The amount of time for which this message is valid.
                                //message.NMSTimeToLive = TimeSpan.FromMinutes(20);
                                ////是否持久消息
                                //message.NMSDeliveryMode = MsgDeliveryMode.Persistent;

                                producer.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal,
                                    TimeSpan.FromMinutes(30));
                            }
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



        #endregion

    }

}

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Com.Utility.Commons;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq
{
    /// <summary>
    /// MQ产生者
    /// </summary>
    public class MQProducer
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
                        producer.Send(session.CreateTextMessage(msg));
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
                        producer.Send(session.CreateTextMessage(msg));
                    }
                }
            }
        }

    }

}

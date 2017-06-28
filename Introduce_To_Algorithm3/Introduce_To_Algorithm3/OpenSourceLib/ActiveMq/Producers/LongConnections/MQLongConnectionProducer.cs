using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq.Producers.LongConnections
{
    /// <summary>
    /// MQ长时间连接的发送器
    /// </summary>
    public class MQLongConnectionProducer
    {
        /// <summary>
        /// MQ地址
        /// </summary>
        private const string MQUri = "MQUri";
        /// <summary>
        /// 创建连接工厂
        /// </summary>
        private static volatile IConnectionFactory factory = new ConnectionFactory(ConfigUtils.GetString(MQUri));

        /// <summary>
        /// 没有容量上限的消息队列
        /// </summary>
        private static BlockingCollection<MessageItem> msgQueue = new BlockingCollection<MessageItem>();

        /// <summary>
        /// 表示连接
        /// </summary>
        private static volatile IConnection connection = null;

        /// <summary>
        /// 表示会话
        /// </summary>
        private static volatile ISession session = null;

        /// <summary>
        /// 消息产生者
        /// </summary>
        private static volatile IMessageProducer producer = null;

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 最近一次通信时间
        /// </summary>
        private static DateTime lastCommunicateTime = DateTime.Now.AddDays(-1);

        /// <summary>
        /// 是否可以发送消息
        /// </summary>
        private static volatile bool isRunning = false;

        /// <summary>
        /// 消息发送线程
        /// </summary>
        private static volatile Thread sendThread = null;



        public static bool Start()
        {
            try
            {

            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// 初始化发送线程
        /// </summary>
        /// <returns></returns>
        public static void InitSendThread()
        {
            lock (locker)
            {
                if (isRunning)
                {
                    return;
                }
                isRunning = true;
            }

            //消息发送线程
            sendThread = new Thread(() =>
            {
                while (isRunning)
                {
                    try
                    {

                    }
                    catch (Exception e)
                    {

                    }
                }
            });
            sendThread.Start();
        }


    }
}

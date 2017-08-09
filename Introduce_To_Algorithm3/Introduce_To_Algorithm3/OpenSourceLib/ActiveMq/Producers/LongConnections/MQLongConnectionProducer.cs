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
using Introduce_To_Algorithm3.Common.Utils.strings;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq.Producers.LongConnections
{
    /// <summary>
    /// MQ长时间连接的发送器
    /// 
    /// 外部调用 
    /// MQLongConnectionProducer.InitSendThread
    ///  MQLongConnectionProducer.CloseSendThread
    /// </summary>
    public class MQLongConnectionProducer
    {
        /// <summary>
        /// MQ地址  如：failover:(tcp://192.168.163.213:61616)
        /// </summary>
        private static readonly string MqUri = ConfigUtils.GetString("MQUri");

        /// <summary>
        /// 使用Topic还是Queue接收消息,1是使用Topic，0使用Queue接收消息
        /// </summary>
        private static readonly bool IsTopic = StringUtils.EqualsEx("1", ConfigUtils.GetString("IsTopic"));


        /// <summary>
        /// 用于接收消息的Topic或者Queue的名称
        /// </summary>
        private static readonly string TopicOrQueueName = ConfigUtils.GetString("TopicOrQueueName");

        /// <summary>
        /// 创建连接工厂
        /// </summary>
        private static volatile IConnectionFactory factory = new ConnectionFactory(MqUri);

        /// <summary>
        /// 没有容量上限的消息队列
        /// </summary>
        private static BlockingCollection<MessageItem> msgQueue = new BlockingCollection<MessageItem>();

        /// <summary>
        /// 消息最大容量
        /// </summary>
        private const int MaxQueueCapacity = 1000;

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
        /// MQ连接是否存活
        /// </summary>
        private static volatile bool isAlive = false;

        /// <summary>
        /// 消息发送线程
        /// </summary>
        private static volatile Thread sendThread = null;

        /// <summary>
        /// 消息发送之后的处理
        /// </summary>
        private static volatile Action<MessageItem> actionAfterMsgSended = null;

        #region Producer相关
        /// <summary>
        /// 开启连接
        /// </summary>
        /// <returns></returns>
        public static bool StartProducer()
        {
            try
            {
                lock (locker)
                {
                    if (connection != null)
                    {
                        CloseProducer();
                    }
                }


                NLogHelper.Debug("开始初始化MQ Producer");
                //通过工厂构建连接
                connection = factory.CreateConnection();
                //设置唯一的客户端id
                connection.ClientId = Guid.NewGuid().ToString("N");
                //错误事件
                connection.ExceptionListener += ConnectionOnExceptionListener;
                //连接中断事件
                connection.ConnectionInterruptedListener += ConnectionOnConnectionInterruptedListener;
                //连接重建事件
                connection.ConnectionResumedListener += ConnectionOnConnectionResumedListener;
                //超时时间
                connection.RequestTimeout = new TimeSpan(0,0,30);

                if (!connection.IsStarted)
                {
                    connection.Start();
                }
                //创建会话
                session = connection.CreateSession();
                session.RequestTimeout = new TimeSpan(0,0,30);
                //创建生产者
                producer = session.CreateProducer(session.GetDestination(TopicOrQueueName,
                    IsTopic ? DestinationType.Topic : DestinationType.Queue));

                //最近一次通信时间
                lastCommunicateTime = DateTime.Now;
                
                lock (locker)
                {
                    isAlive = true;
                }
                
                NLogHelper.Debug($"MQ Producer初始化成功,{(IsTopic?"主题":"队列")}的名字为{TopicOrQueueName}");
                return true;
            }
            catch (Exception e)
            {

                lock (locker)
                {
                    isAlive = false;
                }

                NLogHelper.Error($"MQ Producer初始化失败:{e}");

                return false;
            }
        }

        /// <summary>
        /// 关闭Producer
        /// </summary>
        public static void CloseProducer()
        {
            try
            {
                NLogHelper.Debug($"开始关闭MQ Producer:{MqUri}");
                if (producer != null)
                {
                    producer.Close();
                    producer = null;
                }
            }
            catch (Exception e)
            {
                NLogHelper.Error($"开始关闭MQ Producer失败:{e}");
            }

            try
            {
                if (session != null)
                {
                    session.Close();
                    session = null;
                }
            }
            catch (Exception e)
            {
                NLogHelper.Error($"关闭MQ Session失败:{e}");
            }

            try
            {
                if (connection != null)
                {
                    connection.Stop();
                    connection.Close();
                    connection = null;
                }
            }
            catch (Exception e)
            {
                NLogHelper.Error($"关闭MQ连接connection失败:{e}");
            }

            lock (locker)
            {
                isAlive = false;
            }
        }


        /// <summary>
        /// MQ是否存活
        /// </summary>
        /// <returns></returns>
        public static bool IsAlive()
        {
            lock (locker)
            {
                return isAlive;
            }

            //最好的方式，安装通信时间判断

            //if (!isAlive)
            //{
            //    return false;
            //}

            //if ((DateTime.Now - lastUpdateTime).TotalSeconds > 15*60)
            //{
            //    return false;
            //}

            //return true;
        }


        #endregion

        #region 事件

        /// <summary>
        /// 连接重建事件
        /// </summary>
        private static void ConnectionOnConnectionResumedListener()
        {
            lock (locker)
            {
                isAlive = true;
            }
            NLogHelper.Warn("ConnectionOnConnectionResumedListener连接恢复");
        }

        /// <summary>
        /// 连接中断事件
        /// </summary>
        private static void ConnectionOnConnectionInterruptedListener()
        {
            lock (locker)
            {
                isAlive = false;
            }

            NLogHelper.Error("ConnectionInterruptedListener连接发生异常连接断开");
        }

        /// <summary>
        /// 错误发生事件
        /// </summary>
        /// <param name="exception"></param>
        private static void ConnectionOnExceptionListener(Exception exception)
        {
            lock (locker)
            {
                isAlive = false;
            }
            NLogHelper.Error("connection_ExceptionListener连接发生异常：" + exception);
        }

        #endregion

        #region 添加消息相关

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="message"></param>
        public static void AddMessage(String message)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return;
            }

            MessageItem msgItem = null;
            #region 丢弃超出容量的消息
            if (msgQueue.Count > MaxQueueCapacity)
            {
                msgQueue.TryTake(out msgItem);
                NLogHelper.Error($"消息{msgItem?.Message}被丢弃");
                msgQueue.TryTake(out msgItem);
                NLogHelper.Error($"消息{msgItem?.Message}被丢弃");
                msgQueue.TryTake(out msgItem);
                NLogHelper.Error($"消息{msgItem?.Message}被丢弃");
                msgQueue.TryTake(out msgItem);
                NLogHelper.Error($"消息{msgItem?.Message}被丢弃");
                msgQueue.TryTake(out msgItem);
                NLogHelper.Error($"消息{msgItem?.Message}被丢弃");
            }
            #endregion

            msgItem = new MessageItem(message);
            msgQueue.Add(msgItem);

        }

        #endregion

        #region 发送线程相关

        /// <summary>
        /// 初始化发送线程
        /// </summary>
        /// <param name="actionAfterSend">消息发送之后的处理</param>
        /// <returns></returns>
        public static void InitSendThread(Action<MessageItem> actionAfterSend = null)
        {
            lock (locker)
            {
                if (isRunning)
                {
                    return;
                }
                isRunning = true;
            }

            actionAfterMsgSended = actionAfterSend;

            //消息发送线程
            sendThread = new Thread(() =>
            {
                Action<MessageItem> tempActionAfterMsgSended = actionAfterMsgSended;
                while (isRunning)
                {
                    try
                    {
                        #region  具体的消息发送
                        
                        if (!isAlive)
                        {
                            //mq连接不活
                            Thread.Sleep(20);
                            continue;
                        }

                        MessageItem msg = null;
                        if (msgQueue.TryTake(out msg, 713))
                        {
                            if (msg != null && !String.IsNullOrEmpty(msg.Message))
                            {
                                //MAP消息
                                //IMapMessage mapMessage = session.CreateMapMessage();//MAP消息
                                //字节消息
                                //IBytesMessage byteMsg = session.CreateBytesMessage(new byte[] { });//字节消息
                                

                                //文本消息
                                ITextMessage txtMsg = session.CreateTextMessage(msg.Message);
                                //The timestamp of when the message was pubished in UTC time. If the publisher disables setting the timestamp on the message, the time will be set to the start of the UNIX epoc (1970-01-01 00:00:00).
                                txtMsg.NMSTimestamp = DateTime.UtcNow;
                                //The amount of time for which this message is valid.
                                txtMsg.NMSTimeToLive = TimeSpan.FromMinutes(30);
                                
                                //是否持久消息
                                txtMsg.NMSDeliveryMode = MsgDeliveryMode.Persistent;

                                //可以自己添加消息头属性
                                //txtMsg.Properties.SetString(key, value);

                                producer.Send(txtMsg);

                                if (tempActionAfterMsgSended != null)
                                {
                                    try
                                    {
                                        tempActionAfterMsgSended(msg);
                                    }
                                    catch (Exception ex)
                                    {
                                        NLogHelper.Error($"发送消息之后的处理失败:{ex}");
                                    }
                                }
                            }
                        }
                        
                        #endregion
                    }
                    catch (Exception e)
                    {
                        NLogHelper.Error($"发送消息失败:{e}");
                        Thread.Sleep(10);
                    }
                }
            });
            sendThread.Start();
        }

        /// <summary>
        /// 关闭发送线程
        /// </summary>
        public static void CloseSendThread()
        {
            isRunning = false;
        }

        #endregion
    }
}

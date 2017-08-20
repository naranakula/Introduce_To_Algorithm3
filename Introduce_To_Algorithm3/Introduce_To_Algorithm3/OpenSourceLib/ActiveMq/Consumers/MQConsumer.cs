using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Com.Utility.Commons;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.strings;
using Introduce_To_Algorithm3.OpenSourceLib.AutoMapper;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq
{
    /// <summary>
    /// MQ消费者
    ///     /// 外部调用
    /// MQMessageHandler.Init();
    /// MQMonitorTimer.Start();
    /// </summary>
    public static class MQConsumer
    {
        /*
         * 消息最大可以2G 如果超过100kb会认为是大消息，进行分包传输
        Message：从字面上就可以看出是被发送的消息。它有下面几种类型：
        StreamMessage：Java 数据流消息，用标准流操作来顺序的填充和读取。
        MapMessage：一个Map类型的消息；名称为 string 类型，而值为 Java 的基本类型。
        TextMessage：普通字符串消息，包含一个String。
        ObjectMessage：对象消息，包含一个可序列化的Java 对象
        BytesMessage：二进制数组消息，包含一个byte[]。
        XMLMessage:  一个XML类型的消息。
         */

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
        /// 连接
        /// </summary>
        //private static volatile Connection connection = null;
        private static volatile IConnection connection = null;

        /// <summary>
        /// 会话
        /// </summary>
        private static volatile ISession session = null;

        /// <summary>
        /// 是否连接是活的
        /// </summary>
        private static volatile bool isAlive = false;

        /// <summary>
        /// 消费者
        /// </summary>
        private static volatile IMessageConsumer consumer = null;

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 最近一次通信时间
        /// </summary>
        private static DateTime lastUpdateTime = DateTime.Now.AddDays(-1);

        /// <summary>
        /// 初始化消费者
        /// </summary>
        /// <returns></returns>
        public static bool InitConsumer()
        {
            try
            {
                lock (locker)
                {
                    CloseConsumer();
                }

                NLogHelper.Info("开始初始化MQConsumer");
                
                //通过工厂构建连接
                //connection = (Connection)factory.CreateConnection();
                connection = factory.CreateConnection();

                //默认是不支持事务，自动确认的
                //connection.AcknowledgementMode = AcknowledgementMode.AutoAcknowledge;
                //设置唯一的客户端ID
                connection.ClientId = Guid.NewGuid().ToString("N");
                connection.ExceptionListener += connection_ExceptionListener;
                connection.ConnectionInterruptedListener += connection_ConnectionInterruptedListener;
                connection.ConnectionResumedListener += ConnectionOnConnectionResumedListener;
                //connection.RedeliveryPolicy;//重发策略
                //超时16s
                connection.RequestTimeout = new TimeSpan(0, 0, 30);
                
                if (!connection.IsStarted)
                {
                    //启动连接
                    connection.Start();
                }
                //创建回话
                session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                session.RequestTimeout = new TimeSpan(0, 0, 30);
                NLogHelper.Info("建立名为{0}的{1}连接".FormatWith(TopicOrQueueName, IsTopic ? "Topic" : "Queue"));
                //创建消费者  如果是主题，非持久订阅，断线后消息会丢失，即使消息是持久的且没有过期
                consumer = session.CreateConsumer(session.GetDestination(TopicOrQueueName,(IsTopic?DestinationType.Topic:DestinationType.Queue)));

                //创建持久订阅
                //session.CreateDurableConsumer
                //注册监听事件
                consumer.Listener += consumer_Listener;
                
                //最近一次通信时间
                lastUpdateTime = DateTime.Now;

                NLogHelper.Info("MQ初始化成功");
                lock (locker)
                {
                    isAlive = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                lock (locker)
                {
                    isAlive = false;
                }
                
                NLogHelper.Error("MQ初始化失败：" + ex);
                return false;
            }
        }

        /// <summary>
        /// An asynchronous listener that is notified when a Fault tolerant connection has been resumed.
        /// 连接恢复
        /// </summary>
        private static void ConnectionOnConnectionResumedListener()
        {
            //连接恢复
            lock (locker)
            {
                isAlive = true;
            }
            NLogHelper.Warn("ConnectionOnConnectionResumedListener连接恢复");
        }

        /// <summary>
        /// 消费者监听
        /// 经测试该函数是在前一个回调完成之后执行的,即回调是单线程执行的,需要自己实现多线程
        /// 消息保证是按顺序到达的，该方法不能抛异常，否则导致消息重发
        /// </summary>
        /// <param name="message"></param>
        private static void consumer_Listener(IMessage message)
        {
            //lock (locker)
            //{
            //    isAlive = true;
            //}
            

            try
            {
                lastUpdateTime = DateTime.Now;

                #region 获取消息

                //MAP消息
                //IMapMessage mapMessage = session.CreateMapMessage();//MAP消息
                //字节消息
                //IBytesMessage byteMsg = session.CreateBytesMessage(new byte[] { });//字节消息
                ITextMessage txtMsg = message as ITextMessage;
              
                if (txtMsg == null || string.IsNullOrWhiteSpace(txtMsg.Text))
                {
                    NLogHelper.Warn("接收到空消息");
                    return;
                }
                #endregion

                //原样保存消息
                string msg = txtMsg.Text;//txtMsg.Text.Trim();
                NLogHelper.Info("接收到消息：" + msg);

                #region 开始处理消息

                //注：经测试该函数是在前一个回调完成之后执行的,即回调是单线程执行的,需要自己实现多线程
                MQMessageHandler.AddToQueue(msg);
                #endregion

            }
            catch (Exception ex)
            {
                NLogHelper.Error("解析处理MQ消息失败：" + ex);
            }
        }

        /// <summary>
        /// 连接断开
        /// </summary>
        private static void connection_ConnectionInterruptedListener()
        {
            //可以恢复的连接断开
            lock (locker)
            {
                isAlive = false;
            }
            
            NLogHelper.Error("ConnectionInterruptedListener连接发生异常连接断开");
        }

        /// <summary>
        /// 连接异常
        /// </summary>
        /// <param name="exception"></param>
        private static void connection_ExceptionListener(Exception exception)
        {
            //不可恢复的连接断开
            lock (locker)
            {
                isAlive = false;
            }
            NLogHelper.Error("connection_ExceptionListener连接发生异常：" + exception);
        }


        /// <summary>
        /// 关闭消费者
        /// </summary>
        public static void CloseConsumer()
        {
            try
            {
                if (consumer != null)
                {
                    NLogHelper.Info("开始关闭MQ Consumer:" + MqUri);
                    consumer.Dispose();
                    consumer.Close();
                    consumer = null;
                }

            }
            catch (Exception ex)
            {
                NLogHelper.Error("关闭MQ Consumer失败：" + ex);
            }

            try
            {

                if (session != null)
                {
                    session.Close();
                    session = null;
                }
            }
            catch (Exception ex)
            {

                NLogHelper.Error("关闭MQ session失败：" + ex);
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
            catch (Exception ex)
            {
                NLogHelper.Error("关闭MQ连接connection失败：" + ex);
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
        public static Boolean IsAlive()
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

    }

   
}

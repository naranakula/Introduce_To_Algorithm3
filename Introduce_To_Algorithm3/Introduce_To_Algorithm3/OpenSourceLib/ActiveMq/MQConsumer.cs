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
using Introduce_To_Algorithm3.OpenSourceLib.AutoMapper;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq
{
    /// <summary>
    /// MQ消费者
    /// </summary>
    public static class MQConsumer
    {
        /// <summary>
        /// MQ地址  如：failover:(tcp://192.168.163.213:61616)
        /// </summary>
        private const string MQUri = "MQUri";

        /// <summary>
        /// 使用Topic还是Queue接收消息,1是使用Topic，0使用Queue接收消息
        /// </summary>
        private const string IsTopicStr = "IsTopic";

        /// <summary>
        /// 用于接收消息的Topic或者Queue的名称
        /// </summary>
        private const string TopicOrQueueNameStr = "TopicOrQueueName";

        /// <summary>
        /// 创建连接工厂
        /// </summary>
        private static IConnectionFactory factory = new ConnectionFactory(ConfigUtils.GetString(MQUri));

        /// <summary>
        /// 连接
        /// </summary>
        private static IConnection connection = null;

        /// <summary>
        /// 会话
        /// </summary>
        private static ISession session = null;

        /// <summary>
        /// 是否连接是活的
        /// </summary>
        private static volatile bool isAlive = false;

        /// <summary>
        /// 消费者
        /// </summary>
        private static IMessageConsumer consumer = null;

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
                if (connection != null)
                {
                    CloseConsumer();
                }

                NLogHelper.Info("开始初始化MQConsumer");
                //通过工厂构建连接
                connection = factory.CreateConnection();
                connection.ExceptionListener += connection_ExceptionListener;
                connection.ConnectionInterruptedListener += connection_ConnectionInterruptedListener;
                connection.ConnectionResumedListener += ConnectionOnConnectionResumedListener;
                //超时16s
                connection.RequestTimeout = new TimeSpan(0, 0, 30);
                if (!connection.IsStarted)
                {
                    //启动连接
                    connection.Start();
                }
                //创建回话
                session = connection.CreateSession();
                session.RequestTimeout = new TimeSpan(0, 0, 30);
                bool isTopic = StringUtils.EqualsEx("1", ConfigUtils.GetString(IsTopicStr, "1"));
                NLogHelper.Info("建立名为{0}的{1}连接".FormatWith(ConfigUtils.GetString(TopicOrQueueNameStr), isTopic ? "Topic" : "Queue"));
                //创建消费者
                consumer = session.CreateConsumer(session.GetDestination(ConfigUtils.GetString(TopicOrQueueNameStr),isTopic?DestinationType.Topic:DestinationType.Queue));
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
        /// 链接回复
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
        /// 消费者监听
        /// 经测试该函数是在前一个回调完成之后执行的,即回调是单线程执行的,需要自己实现多线程
        /// </summary>
        /// <param name="message"></param>
        private static void consumer_Listener(IMessage message)
        {
            //lock (locker)
            //{
            //    isAlive = true;
            //}

            lastUpdateTime = DateTime.Now;

            try
            {
                #region 获取消息
                ITextMessage txtMsg = message as ITextMessage;
              
                if (txtMsg == null || string.IsNullOrWhiteSpace(txtMsg.Text))
                {
                    NLogHelper.Warn("接收到空消息");
                    return;
                }
                #endregion

                string msg = txtMsg.Text.Trim();
                NLogHelper.Info("接收到消息：" + msg);

                #region 开始处理消息

                //注：经测试该函数是在前一个回调完成之后执行的,即回调是单线程执行的,需要自己实现多线程

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

                NLogHelper.Info("开始关闭MQ:" + ConfigUtils.GetString(MQUri));

                if (consumer != null)
                {
                    consumer.Close();
                }
                consumer = null;

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
                }
                session = null;
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
                }
                connection = null;
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

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
        /// 是否活动
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

                Log4netHelper.Info("开始初始化MQConsumer");
                //通过工厂构建连接
                connection = factory.CreateConnection();
                connection.ExceptionListener += connection_ExceptionListener;
                connection.ConnectionInterruptedListener += connection_ConnectionInterruptedListener;

                //创建回话
                session = connection.CreateSession();
                bool isTopic = StringUtils.EqualsEx("1", ConfigUtils.GetString("TopicOrQueue", "1"));
                Log4netHelper.Info("建立名为{0}的{1}连接".FormatWith(ConfigUtils.GetString("NaomsConsumerTopic"), isTopic ? "Topic" : "Queue"));
                //创建消费者
                consumer = session.CreateConsumer(session.GetDestination(ConfigUtils.GetString("NaomsConsumerTopic"),isTopic?DestinationType.Topic:DestinationType.Queue));
                //注册监听事件
                consumer.Listener += consumer_Listener;
                if (!connection.IsStarted)
                {
                    //启动连接
                    connection.Start();
                }
                Log4netHelper.Info("MQ初始化成功");
                isAlive = true;
                return true;
            }
            catch (Exception ex)
            {
                isAlive = false;
                Log4netHelper.Error("MQ初始化失败：" + ex);
                return false;
            }
        }

        /// <summary>
        /// 消费者监听
        /// 经测试该函数是在前一个回调完成之后执行的,即回调是单线程执行的,需要自己实现多线程
        /// </summary>
        /// <param name="message"></param>
        private static void consumer_Listener(IMessage message)
        {
            isAlive = true;
            try
            {
                #region 获取消息
                ITextMessage txtMsg = message as ITextMessage;
              
                if (string.IsNullOrWhiteSpace(txtMsg.Text))
                {
                    return;
                }
                #endregion
               
            }
            catch (Exception ex)
            {
                Log4netHelper.Error("解析处理MQ消息失败：" + ex);
            }
        }

        /// <summary>
        /// 连接断开
        /// </summary>
        private static void connection_ConnectionInterruptedListener()
        {
            isAlive = false;
            Log4netHelper.Error("ConnectionInterruptedListener连接发生异常连接断开");
        }

        /// <summary>
        /// 连接异常
        /// </summary>
        /// <param name="exception"></param>
        private static void connection_ExceptionListener(Exception exception)
        {
            isAlive = false;
            Log4netHelper.Error("connection_ExceptionListener连接发生异常：" + exception);
        }


        /// <summary>
        /// 关闭消费者
        /// </summary>
        public static void CloseConsumer()
        {
            try
            {

                Log4netHelper.Info("开始关闭MQ:" + ConfigUtils.GetString(MQUri));

                if (consumer != null)
                {
                    consumer.Dispose();
                    consumer.Close();
                }
                consumer = null;

                if (session != null)
                {
                    session.Dispose();
                    session.Close();
                }
                session = null;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error("关闭MQ Session失败：" + ex);
            }

            try
            {
                if (connection != null)
                {
                    connection.Stop();
                    connection.Dispose();
                    connection.Close();
                }
                connection = null;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error("关闭MQ连接失败：" + ex);
            }

            isAlive = false;
        }

        /// <summary>
        /// MQ是否存活
        /// </summary>
        /// <returns></returns>
        public static Boolean IsAlive()
        {
            return isAlive;
        }

    }

   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.Producers
{
    /// <summary>
    /// 长连接生产者
    /// 
    /// LongRunRabbitProducer.Start()
    /// 
    /// 
    /// LongRunRabbitProducer.Stop()
    /// 
    /// </summary>
    public static class LongRunRabbitProducer
    {

        // There are four building blocks you really care about in AMQP: virtual hosts, exchanges, queues and bindings. A virtual host holds a bundle of exchanges, queues and bindings.
        //Why would you want multiple virtual hosts? Easy. A username in RabbitMQ grants you access to a virtual host…in its entirety. So the only way to keep group A from accessing group B’s exchanges/queues/bindings/etc. is to create a virtual host for A and one for B. 
        //Virtualhost是虚拟主机,类似于命名空间
        //Rabbitmq的默认用户guest/guest，具有管理员权限 //guest拥有管理员权限，但只能本地访问,已测试
        //即使AutomaticRecoveryEnable为true，CreateConnection超时仍会抛出异常，与activemq不同
        private static readonly ConnectionFactory SFactory = new ConnectionFactory() { UserName = "admin", Password = "admin", VirtualHost = ConnectionFactory.DefaultVHost, HostName = "192.168.163.12", Port = 5672, AutomaticRecoveryEnabled = true/*自动重连，指的是自动重连，为true时CreateConnectin仍会抛出异常*/, NetworkRecoveryInterval = TimeSpan.FromMilliseconds(5753)/*自动重连的时间间隔默认5s*/, RequestedConnectionTimeout = ConnectionFactory.DefaultConnectionTimeout, SocketReadTimeout = ConnectionFactory.DefaultConnectionTimeout, SocketWriteTimeout = ConnectionFactory.DefaultConnectionTimeout/*以上三个值就是不设置时使用的默认的值,30s*/, RequestedHeartbeat = ConnectionFactory.DefaultHeartbeat/*心跳检测，默认是60s*/};

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object SLocker = new object();

        /// <summary>
        /// 连接 多线程安全
        /// 表示一个TCP连接
        /// Each IConnection instance is, in the current implementation, backed by a single background thread that reads from the socket and dispatches the resulting events to the application
        /// </summary>
        private static volatile IConnection _connection = null;


        /// <summary>
        /// 底层的mq是否存活
        /// </summary>
        private static volatile bool _isAlive = false;

        /// <summary>
        /// 底层的mq是否存活
        /// </summary>
        /// <returns></returns>
        public static bool IsAlive()
        {
            lock (SLocker)
            {
                return _isAlive;
            }
        }


        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool Start(Action<Exception> exceptionHandler = null)
        {
            try
            {

                Stop(exceptionHandler);

                //创建并打开连接
                _connection = SFactory.CreateConnection();

                //定义连接事件
                //连接阻塞的回调
                _connection.ConnectionBlocked += ConnectionOnConnectionBlocked;
                //连接未阻塞的回调
                _connection.ConnectionUnblocked += ConnectionOnConnectionUnblocked;
                //连接回调异常
                _connection.CallbackException += ConnectionOnCallbackException;
                //连接恢复错误
                _connection.ConnectionRecoveryError += ConnectionOnConnectionRecoveryError;
                //连接恢复成功
                _connection.RecoverySucceeded += ConnectionOnRecoverySucceeded;
                //连接关闭
                _connection.ConnectionShutdown += ConnectionOnConnectionShutdown;

                lock (SLocker)
                {
                    _isAlive = true;
                }

                return true;
            }
            catch (Exception e)
            {
                lock (SLocker)
                {
                    _isAlive = false;
                }
                exceptionHandler?.Invoke(e);

                return false;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool SendMessage(RMqSendMessage msg,Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (IModel channel = _connection.CreateModel())
                {
                    var properties = channel.CreateBasicProperties();
                    //持久
                    properties.DeliveryMode = 2;
                    //超时时间，单位毫秒
                    properties.Expiration = "7200000";
                    channel.BasicPublish(exchange: msg.ExchangeName, routingKey: msg.RoutingKey, basicProperties: properties, body: msg.ContentBytes);
                }


                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }
        

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="exceptionHandler"></param>
        public static void Stop(Action<Exception> exceptionHandler = null)
        {
            lock (SLocker)
            {
                _isAlive = false;
            }


            if (_connection != null)
            {
                try
                {
                    _connection.Close();
                }
                catch (Exception e)
                {
                    exceptionHandler?.Invoke(e);
                }

                _connection = null;
            }

        }

        #region 连接事件回调

        /// <summary>
        /// 连接阻塞
        /// 
        /// 被限流
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connectionBlockedEventArgs"></param>
        private static void ConnectionOnConnectionBlocked(object sender, ConnectionBlockedEventArgs connectionBlockedEventArgs)
        {
            NLogHelper.Warn($"OnConnectionBlocked:{connectionBlockedEventArgs?.Reason}");
        }

        /// <summary>
        /// 连接关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="shutdownEventArgs"></param>
        private static void ConnectionOnConnectionShutdown(object sender, ShutdownEventArgs shutdownEventArgs)
        {

            lock (SLocker)
            {
                _isAlive = false;
            }

            NLogHelper.Warn($"OnConnectionShutdown");
        }

        /// <summary>
        /// 连接恢复成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void ConnectionOnRecoverySucceeded(object sender, EventArgs eventArgs)
        {

            lock (SLocker)
            {
                _isAlive = true;
            }
            NLogHelper.Warn($"OnRecoverySucceeded");
        }

        /// <summary>
        /// 连接恢复失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connectionRecoveryErrorEventArgs"></param>
        private static void ConnectionOnConnectionRecoveryError(object sender, ConnectionRecoveryErrorEventArgs connectionRecoveryErrorEventArgs)
        {

            lock (SLocker)
            {
                _isAlive = false;
            }

            NLogHelper.Warn($"OnConnectionRecoveryError:{connectionRecoveryErrorEventArgs?.Exception}");
        }
        /// <summary>
        /// 连接回调错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackExceptionEventArgs"></param>
        private static void ConnectionOnCallbackException(object sender, CallbackExceptionEventArgs callbackExceptionEventArgs)
        {
            NLogHelper.Warn($"OnCallbackException:{callbackExceptionEventArgs?.Exception}");
        }
        /// <summary>
        /// 连接畅通
        /// 限流恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void ConnectionOnConnectionUnblocked(object sender, EventArgs eventArgs)
        {
            NLogHelper.Warn($"OnConnectionUnblocked");
        }

        #endregion



    }
}

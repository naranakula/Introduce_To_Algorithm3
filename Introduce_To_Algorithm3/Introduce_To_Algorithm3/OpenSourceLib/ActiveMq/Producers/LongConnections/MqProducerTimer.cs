using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq.Producers.LongConnections
{
    /// <summary>
    ///   MQLongConnectionProducer.InitSendThread
    /// MqProducerTimer.Start();//开启接收消息
    /// 
    /// 关闭
    /// MqProducerTimer.Stop();
    ///  MQLongConnectionProducer.CloseSendThread
    /// </summary>
    public static class MqProducerTimer
    {
        /// <summary>
        /// 底层的timer
        /// </summary>
        private static volatile Timer mqTimer = null;

        /// <summary>
        /// 定时器是否正在运行
        /// </summary>
        private static bool isRunning = false;


        /// <summary>
        /// 是否第一次启动mQ
        /// </summary>
        private static volatile bool _isFirstTimeToStartMq = true;


        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// 通过定时器开启MQ，不要直接开启mQ
        /// </summary>
        public static void Start()
        {
            if (mqTimer == null)
            {
                mqTimer = new Timer(new TimerCallback(MQTimerCallBack), null, 700, 16130);
            }
        }

        /// <summary>
        /// MQ回调
        /// </summary>
        /// <param name="state"></param>
        private static void MQTimerCallBack(object state)
        {
            lock (locker)
            {
                if (isRunning)
                {
                    return;
                }
                isRunning = true;
            }

            try
            {
                if (!MQLongConnectionProducer.IsAlive())
                {
                    if (_isFirstTimeToStartMq)
                    {
                        _isFirstTimeToStartMq = false;

                        NLogHelper.Debug("首次尝试启动MQProducer");
                        MQLongConnectionProducer.StartProducer();
                    }
                    else
                    {
                        NLogHelper.Warn("重启MQProducer");
                        MQLongConnectionProducer.StartProducer();
                    }

                }

            }
            catch (Exception ex)
            {
                NLogHelper.Error("MQProducer定时器异常:" + ex);
            }
            finally
            {
                lock (locker)
                {
                    isRunning = false;
                }
            }
        }

        /// <summary>
        /// 关闭定时器
        /// </summary>
        public static void Stop()
        {
            if (mqTimer != null)
            {
                mqTimer.Dispose();
                mqTimer = null;
            }

            //关闭MQ
            MQLongConnectionProducer.CloseProducer();
        }
    }
}

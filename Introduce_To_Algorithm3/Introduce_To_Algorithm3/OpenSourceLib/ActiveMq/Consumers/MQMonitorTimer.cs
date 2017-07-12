using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq
{
    /// <summary>
    /// MQ监视的Consumer
    /// 外部调用
    /// MQMessageHandler.Init();//初始化消息处理
    /// MQMonitorTimer.Start();//开启接收消息
    /// 
    /// 关闭
    /// MQMonitorTimer.Stop();
    /// MessageHandler.Stop();
    /// </summary>
    public static class MQMonitorTimer
    {
        /// <summary>
        /// 底层的timer
        /// </summary>
        private static Timer mqTimer = null;

        /// <summary>
        /// 定时器是否正在运行
        /// </summary>
        private static bool isRunning = false;

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 通过定时器开启MQ，不要直接开启mQ
        /// </summary>
        public static void Start()
        {
            if (mqTimer == null)
            {
                mqTimer = new Timer(new TimerCallback(MQTimerCallBack),null,500,16130);
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
                if (!MQConsumer.IsAlive())
                {
                    NLogHelper.Warn("重启 MQConsumer");
                    MQConsumer.InitConsumer();
                }

            }
            catch (Exception ex)
            {
                NLogHelper.Error("MQ定时器异常："+ex);
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
            MQConsumer.CloseConsumer();
        }
    }

}

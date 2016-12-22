using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.ActiveMq;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.SocketClients
{
    /// <summary>
    /// 监控socket的Timer
    /// </summary>
    public static class SocketMonitorTimer
    {
        /// <summary>
        /// 底层的socket监视定时器
        /// </summary>
        private static Timer socketTimer = null;

        /// <summary>
        /// 定时器是否正在运行
        /// </summary>
        private static bool isRunning = false;

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 通过定时器开启socket，不要直接开启socket
        /// </summary>
        public static void Start()
        {
            if (socketTimer == null)
            {
                //每16秒判断一次是否需要重连
                socketTimer = new Timer(new TimerCallback(SocketTimerCallBack), null, 800, 16000);
            }
        }

        /// <summary>
        /// socket回调
        /// </summary>
        /// <param name="state"></param>
        private static void SocketTimerCallBack(object state)
        {
            lock (locker)
            {
                if (isRunning)
                {
                    return;
                }
            }

            try
            {
                lock (locker)
                {
                    isRunning = true;
                }

                if (!SocketClientProxy.IsConnected)
                {
                    NLogHelper.Warn("重启socket客户端");
                    SocketClientProxy.Start();
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error("socket监控定时器异常：" + ex);
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
            if (socketTimer != null)
            {
                socketTimer.Dispose();
                socketTimer = null;
            }

            //关闭 发送 接收 多线程
            SocketClientProxy.IsRunning = false;
            SocketClientProxy.Stop();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.threads
{
    public class TimerUtils
    {
        /// <summary>
        /// 立刻启动定时服务
        /// 注：Timer没有start方法，创建了一个实例就立刻执行
        /// 注：定时任务是按照时间间隔严格执行的。即使之前的定时任务没有完成，到达时间也启动新的定时任务。
        /// 这意味着定时任务不一定在同一个线程执行
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="delayTime">以毫秒为单位，第一次启动的延迟时间</param>
        /// <param name="period">调用的周期，毫秒为单位</param>
        /// <param name="state">传递的参数</param>
        /// <returns></returns>
        public static Timer StartTimer(TimerCallback callback, int delayTime, int period, object state = null)
        {
            Timer timer = new Timer(callback,state,delayTime,period);
            return timer;
        }

        /// <summary>
        /// 释放定时器
        /// </summary>
        /// <param name="timer"></param>
        public static void CloseTimer(Timer timer)
        {
            if (timer != null)
            {
                timer.Dispose();
            }
        }
    }
}

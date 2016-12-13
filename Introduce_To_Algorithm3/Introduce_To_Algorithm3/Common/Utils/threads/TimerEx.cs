using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils.threads
{
    public class TimerEx : IDisposable
    {
        /// <summary>
        /// 定时器
        /// </summary>
        private Timer _timer = null;

        /// <summary>
        /// 锁
        /// </summary>
        private object _locker = new object();

        /// <summary>
        /// 是否回调在运行
        /// </summary>
        private bool _isRunning = false;

        /// <summary>
        /// 底层的回调
        /// </summary>
        private readonly Action actionCallback=null;
        
        /// <summary>
        /// 构造函数
        /// 初始化完成后，回调已经开始执行
        /// </summary>
        /// <param name="actionCallback">定时器回调</param>
        /// <param name="dueTime">指定第一次开始指定的时间，单位毫秒，0表示立刻开始执行</param>
        /// <param name="period">指定执行的时间周期，单位毫秒</param>
        public TimerEx(Action actionCallback, int period = 1000, int dueTime=0)
        {
            this.actionCallback = actionCallback;
            //实际上初始化完成后，回调已经开始执行
            _timer = new Timer(new TimerCallback(TimerCallback), null, dueTime, period);
        }
        
        /// <summary>
        /// 定时器回调函数
        /// </summary>
        /// <param name="state"></param>
        private void TimerCallback(object state)
        {
            lock (_locker)
            {
                if (_isRunning)
                {
                    //回调正在运行
                    return;
                }
                _isRunning = true;
            }

            try
            {
                if (actionCallback != null)
                {
                    actionCallback();
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error("定时器回调函数发生异常：" + ex);
            }
            finally
            {
                lock (_locker)
                {
                    _isRunning = false;
                }
            }
        }
        
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}

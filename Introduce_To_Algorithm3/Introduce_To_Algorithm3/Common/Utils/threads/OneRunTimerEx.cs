using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils.threads
{
    /// <summary>
    /// 定时器拓展
    /// 初始化完成后，回调已经开始执行
    /// 同时只能有一个回调执行
    /// </summary>
    public class OneRunTimerEx : IDisposable
    {
        #region 私有属性

        /// <summary>
        /// 定时器
        /// </summary>
        private volatile Timer _timer = null;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// 是否回调在运行
        /// </summary>
        private bool _isRunning = false;
        
        /// <summary>
        /// 底层的回调
        /// </summary>
        private readonly Action _actionCallback=null;

        /// <summary>
        /// 异常处理
        /// </summary>
        private readonly Action<Exception> _exceptionHandler = null;

        #endregion

        /// <summary>
        /// 构造函数
        /// 初始化完成后，回调已经开始执行
        /// </summary>
        /// <param name="actionCallback">定时器回调</param>
        /// <param name="dueTime">指定第一次开始执行前的等待时间，单位毫秒，0表示立刻开始执行</param>
        /// <param name="period">指定执行的时间周期，单位毫秒</param>
        /// <param name="state"> An object containing information to be used by the callback method, or null.</param>
        /// <param name="exceptionHandler">异常处理</param>
        public OneRunTimerEx(Action actionCallback,int dueTime = 0, int period = 1000, object state =null,Action<Exception> exceptionHandler = null)
        {
            this._actionCallback = actionCallback;
            this._exceptionHandler = exceptionHandler;
            //实际上初始化完成后，回调已经开始执行
            _timer = new Timer(new TimerCallback(TimerCallback), state, dueTime, period);
        }
        
        /// <summary>
        /// 定时器回调函数
        /// </summary>
        /// <param name="state"></param>
        private void TimerCallback(object state)
        {
            Action actionInTimer = null;

            lock (_locker)
            {
                if (_isRunning)
                {
                    //回调正在运行
                    return;
                }
                _isRunning = true;
                actionInTimer = this._actionCallback;
            }

            try
            {
                if (actionInTimer != null)
                {
                    actionInTimer();
                }
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Invoke(ex);
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
                try
                {
                    _timer.Dispose();
                }
                catch { }
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
                try
                {
                    _timer.Dispose();
                }
                catch { }
                _timer = null;
            }
        }
    }
}

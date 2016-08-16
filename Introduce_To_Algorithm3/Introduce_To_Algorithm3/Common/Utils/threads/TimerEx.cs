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
        private readonly Action<object> actionCallback=null;
        
        /// <summary>
        /// 是否回调在运行
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (_locker)
                {
                    return _isRunning;
                }
            }
            private set
            {
                lock (_locker)
                {
                    _isRunning = value;
                }
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public TimerEx()
        {
            this.actionCallback = CallBack;
            //实际上初始化完成后，回调已经开始执行
            _timer = new Timer(new TimerCallback(TimerCallback), null, 1000, 9000);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="actionCallback"></param>
        public TimerEx(Action<Object> actionCallback)
        {
            this.actionCallback = actionCallback;
            //实际上初始化完成后，回调已经开始执行
            _timer = new Timer(new TimerCallback(TimerCallback), null, 1000, 9000);
        }
        
        /// <summary>
        /// 定时器回调函数
        /// </summary>
        /// <param name="state"></param>
        private void TimerCallback(object state)
        {
            lock (_locker)
            {
                if (IsRunning)
                {
                    //回调正在运行
                    return;
                }
                IsRunning = true;
            }

            try
            {
                actionCallback(state);
            }
            catch (Exception ex)
            {
                NLogHelper.Error("定时器回调函数发生异常：" + ex);
            }
            finally
            {
                IsRunning = false;
            }
        }

        /// <summary>
        /// 实际的回调函数
        /// </summary>
        /// <param name="state"></param>
        private static void CallBack(object state)
        {

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (_timer != null)
            {
                _timer.Dispose();
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
            }
        }
    }
}

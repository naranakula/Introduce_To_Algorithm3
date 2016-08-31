using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 同一时刻只能运行一个方法
    /// 
    /// 锁应用于类实例级别
    /// </summary>
    public class OneRunAtSameTime
    {
        /// <summary>
        /// 是否正在运行
        /// </summary>
        private bool isRunning = false;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// 同一时刻只能运行一个方法
        /// </summary>
        /// <param name="action"></param>
        public void Run(Action action)
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
                action();
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
        /// 同一时刻只能运行一个方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        public void Run<T>(Action<T> action,T obj)
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
                action(obj);
            }
            finally
            {
                isRunning = false;
            }
        }

        /// <summary>
        /// 同一时刻只能运行一个方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler">异常处理</param>
        public void RunSafe(Action action,Action<Exception> exceptionHandler = null)
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
                action();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
            finally
            {
                isRunning = false;
            }
        }

        /// <summary>
        /// 同一时刻只能运行一个方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        /// <param name="exceptionHandler">异常处理</param>
        public void RunSafe<T>(Action<T> action, T obj,Action<Exception> exceptionHandler)
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
                action(obj);
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
            finally
            {
                isRunning = false;
            }
        }
    }
}

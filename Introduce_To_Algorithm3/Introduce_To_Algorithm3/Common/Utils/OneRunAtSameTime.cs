using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 同一时刻只能运行一个方法
    /// </summary>
    public static class OneRunAtSameTime
    {
        /// <summary>
        /// 是否正在运行
        /// </summary>
        private static bool isRunning = false;

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 同一时刻只能运行一个方法
        /// </summary>
        /// <param name="action"></param>
        public static void Run(Action action)
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
        public static void Run<T>(Action<T> action,T obj)
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
        public static void RunNoException(Action action)
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
            catch (Exception)
            {

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
        public static void RunNoException<T>(Action<T> action, T obj)
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
            catch (Exception )
            {
                
            }
            finally
            {
                isRunning = false;
            }
        }
    }
}

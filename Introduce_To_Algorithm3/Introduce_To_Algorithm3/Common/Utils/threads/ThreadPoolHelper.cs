using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 线程帮助类
    /// 注：线程池线程都是后台线程。每个线程都使用默认堆栈大小，以默认的优先级运行，并处于多线程单元中。
    /// 每个进程都有且只有一个线程池。
    /// </summary>
    public class ThreadPoolHelper
    {
        #region 获取线程池的工作线程数

        /// <summary>
        /// 获取线程池的工作线程数
        /// </summary>
        public static int ActiveThreadNumByThreadPool
        {
            get
            {
                //定义最大的线程数
                int maxNum;
                //定义可用的线程数
                int availableNum;
                //线程池中异步 I/O 线程的最大数目。
                int ioNum;

                //获取最大的线程数
                ThreadPool.GetMaxThreads(out maxNum, out ioNum);
                //获取可用的线程数
                ThreadPool.GetAvailableThreads(out availableNum, out ioNum);

                //返回活动的线程数
                return maxNum - availableNum;
            }
        }

        #endregion

        #region 获取线程池中的可用线程数

        /// <summary>
        /// 获取线程池中的可用工作线程数
        /// </summary>
        public static int AvaliableThreadNumByThreadPool
        {
            get
            {
                //定义可用的线程数
                int availableNum;
                //线程池中异步 I/O 线程的最大数目。
                int ioNum;

                //获取可用的线程数
                ThreadPool.GetAvailableThreads(out availableNum, out ioNum);

                //返回可用的工作线程数
                return availableNum;
            }
        }
        #endregion

        #region 获取线程池中的最大工作线程数

        /// <summary>
        /// 获取线程池中的最大工作线程数
        /// </summary>
        public static int MaxThreadNumByThreadPool
        {
            get
            {
                //定义最大的线程数
                int maxNum;
                //线程池中异步 I/O 线程的最大数目。
                int ioNum;

                //获取最大的线程数
                ThreadPool.GetMaxThreads(out maxNum, out ioNum);

                //获取最大的工作线程数
                return maxNum;
            }
        }

        #endregion

        #region 获取线程池中维护的空闲辅助线程的最小数目

        /// <summary>
        /// 获取线程池中维护的空闲辅助线程的最小数目
        /// </summary>
        public static int MinThreadNumByThreadPool
        {
            get
            {
                //定义最大的线程数
                int minNum;
                //线程池中异步 I/O 线程的最大数目。
                int ioNum;

                //获取最小的线程数
                ThreadPool.GetMinThreads(out minNum, out ioNum);

                //获取最小的工作线程数
                return minNum;
            }
        }

        #endregion

        #region 设置线程池中的最大默认工作线程数=cpu数*25

        /// <summary>
        /// 设置线程池中的最大工作线程数
        /// </summary>
        public static void SetMaxThreadNumByThreadPool()
        {

            //定义最大的线程数
            int maxNum;
            //线程池中异步 I/O 线程的最大数目。
            int ioNum;

            //获取最大的线程数
            ThreadPool.GetMaxThreads(out maxNum, out ioNum);

            //设置最大的线程数 = 处理器数目*25
            ThreadPool.SetMaxThreads(Environment.ProcessorCount*25, ioNum);
        }

        #endregion

        #region 设置线程池中维护的空闲辅助线程的默认最小数目=cpu数*1

        /// <summary>
        /// 设置线程池中维护的空闲辅助线程的最小数目
        /// </summary>
        public static void SetMinThreadNumByThreadPool()
        {
            //定义最小的线程数
            int minNum;
            //线程池中异步 I/O 线程的最大数目。
            int ioNum;

            //获取最小的线程数
            ThreadPool.GetMinThreads(out minNum, out ioNum);

            //设置最小的线程数 = 处理器个数*1
            ThreadPool.SetMinThreads(Environment.ProcessorCount*1, ioNum);
        }

        #endregion

        #region 设置线程池的最大工作线程数和空闲辅助线程的最小数目

        /// <summary>
        /// 设置线程池的最大工作线程数和空闲辅助线程的最小数目
        /// </summary>
        /// <param name="minNum">初始空闲辅助线程的最小数目</param>
        /// <param name="maxNum">线程池的最大工作线程数</param>
        public static void SetThreadNumByThreadPool(int minNum,int maxNum)
        {
            //检测参数的有效性
            if(maxNum <minNum || minNum<=0)
            {
                throw new Exception("入参无效");
            }

            //定义线程数
            int threadNum;
            //线程池异步I/O线程的最大数目
            int ioNum;

            //获取最小的线程数
            ThreadPool.GetMinThreads(out threadNum,out ioNum);
            //设置最小线程数
            ThreadPool.SetMinThreads(minNum, ioNum);

            //获取最大的线程数
            ThreadPool.GetMaxThreads(out threadNum, out ioNum);
            //设置最大的线程数
            ThreadPool.SetMaxThreads(maxNum, ioNum);
        }
        #endregion

        #region 调用线程池的工作线程执行任务

        /// <summary>
        /// 执行线程池的工作线程执行任务，如果调度成功，返回true
        /// </summary>
        /// <param name="callback">public delegate void WaitCallback(object state);</param>
        /// <returns></returns>
        public static bool ExecThreadPool(WaitCallback callback)
        {
            try
            {
                //未传递参数。
                return ThreadPool.QueueUserWorkItem(callback);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 执行线程池的工作线程执行任务，如果调度成功，返回true
        /// </summary>
        /// <param name="callback">public delegate void WaitCallback(object state);</param>
        /// <param name="state">传入方法的参数</param>
        /// <returns></returns>
        public static bool ExecThreadPool(WaitCallback callback, object state)
        {
            try
            {
                //未传递参数。
                return ThreadPool.QueueUserWorkItem(callback, state);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 获取当前线程的名称

        /// <summary>
        /// 获取当前线程的名称
        /// </summary>
        public static string ThreadName
        {
            get { return Thread.CurrentThread.Name; }
        }

        /// <summary>
        /// 获取当前线程的唯一标识符
        /// </summary>
        public static int ThreadId
        {
            get { return Thread.CurrentThread.ManagedThreadId; }
        }

        #endregion

    }
}

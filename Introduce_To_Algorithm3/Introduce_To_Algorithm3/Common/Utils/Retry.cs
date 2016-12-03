using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// a model to do retry job
    /// </summary>
    public class Retry
    {
        /// <summary>
        /// 默认的重试次数3
        /// </summary>
        private const int _defaultTryCount = 3;

        /// <summary>
        /// get the default try count
        /// </summary>
        public static int DefaultTryCount
        {
            get { return _defaultTryCount; }
        }

        /// <summary>
        /// 默认的重试时间5s
        /// </summary>
        private static readonly TimeSpan _defaultInterval = new TimeSpan(0, 0, 5);

        /// <summary>
        /// get the default interval
        /// </summary>
        public static TimeSpan DefaultInterval
        {
            get { return _defaultInterval; }
        }

        /// <summary>
        /// get the actual try count
        /// </summary>
        public int TryCount { get; private set; }

        /// <summary>
        /// get the actual try interval
        /// 如果为null，表示重试时没有时间间隔
        /// </summary>
        public TimeSpan? TryInterval { get; private set; }

        /// <summary>
        /// construct with 3 trycount and 5 seconds interval
        /// </summary>
        public Retry():this(_defaultTryCount,_defaultInterval)
        {
        }

        /// <summary>
        /// construct retry
        /// </summary>
        /// <param name="tryCount">
        /// run at most reyCount times, include the first run
        /// >=1, 函数至多运行tryCount次数（如tryCount=1，表示运行1次）
        /// </param>
        /// <param name="tryInterval">如果为null，表示重试时没有时间间隔</param>
        public Retry(int tryCount, TimeSpan? tryInterval = null)
        {
            if (tryCount <= 0)
            {
                throw new ArgumentOutOfRangeException("tryCount must be greater than zero");
            }

            TryCount = tryCount;
            TryInterval = tryInterval;
        }

        /// <summary>
        /// invoke action, when fails call errorNotify to notify users if it is not null
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler">错误处理，最终出现错误时会通知</param>
        public void Invoke(Action action, Action<RetryException> exceptionHandler = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action is null");
            }

            int tryCount = 0;
            RetryException retryException = null;
            while(true)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    tryCount++;

                    #region 异常处理

                    if (retryException == null)
                    {
                        retryException = new RetryException();
                    }
                    retryException.Enqueue(ex);

                    if (exceptionHandler != null && tryCount>=TryCount)
                    {
                        exceptionHandler(retryException);
                    }
                    #endregion

                    if (tryCount >= TryCount)
                    {
                        return;
                    }

                    if (TryInterval != null)
                    {
                        try
                        {
                            Thread.Sleep(TryInterval.Value);
                        }
                        catch (Exception)
                        {
                            //ignore
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// invoke action, when fails call errorNotify to notify users if it is not null
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        /// <param name="exceptionHandler">错误处理，最终出现错误时会通知</param>
        public void Invoke<T>(Action<T> action,T obj, Action<RetryException> exceptionHandler = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action is null");
            }

            int tryCount = 0;
            RetryException retryException = null;
            while (true)
            {
                try
                {
                    action(obj);
                    return;
                }
                catch (Exception ex)
                {
                    tryCount++;

                    #region 异常处理

                    if (retryException == null)
                    {
                        retryException = new RetryException();
                    }
                    retryException.Enqueue(ex);

                    if (exceptionHandler != null && tryCount >= TryCount)
                    {
                        exceptionHandler(retryException);
                    }
                    #endregion

                    if (tryCount >= TryCount)
                    {
                        return;
                    }

                    if (TryInterval != null)
                    {
                        try
                        {
                            Thread.Sleep(TryInterval.Value);
                        }
                        catch (Exception)
                        {
                            //ignore
                        }
                    }
                }
            }
        }

        /// <summary>
        /// invoke func, when fails call errorNotify to notify users if it is not null
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="exceptionHandler">错误处理，最终出现错误时会通知</param>
        /// <returns></returns>
        public TResult Invoke<TResult>(Func<TResult> func, Action<RetryException> exceptionHandler = null)
        {
            if (func == null)
            {
                throw new ArgumentNullException("action is null");
            }

            int tryCount = 0;
            RetryException retryException = null;
            while (true)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    tryCount++;

                    #region 异常处理

                    if (retryException == null)
                    {
                        retryException = new RetryException();
                    }
                    retryException.Enqueue(ex);

                    if (exceptionHandler != null && tryCount >= TryCount)
                    {
                        exceptionHandler(retryException);
                    }
                    #endregion

                    if (tryCount >= TryCount)
                    {
                        return default(TResult);
                    }

                    if (TryInterval != null)
                    {
                        try
                        {
                            Thread.Sleep(TryInterval.Value);
                        }
                        catch (Exception)
                        {
                            //ignore
                        }
                    }
                }
            }
        }

        /// <summary>
        /// invoke func, when fails call errorNotify to notify users if it is not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="obj"></param>
        /// <param name="exceptionHandler">错误处理，最终出现错误时会通知</param>
        /// <returns></returns>
        public TResult Invoke<T,TResult>(Func<T,TResult> func, T obj, Action<RetryException> exceptionHandler = null)
        {
            if (func == null)
            {
                throw new ArgumentNullException("action is null");
            }

            int tryCount = 0;
            RetryException retryException = null;
            while (true)
            {
                try
                {
                    return func(obj);
                }
                catch (Exception ex)
                {
                    tryCount++;

                    #region 异常处理

                    if (retryException == null)
                    {
                        retryException = new RetryException();
                    }
                    retryException.Enqueue(ex);

                    if (exceptionHandler != null && tryCount >= TryCount)
                    {
                        exceptionHandler(retryException);
                    }
                    #endregion

                    if (tryCount >= TryCount)
                    {
                        return default(TResult);
                    }

                    if (TryInterval != null)
                    {
                        try
                        {
                            Thread.Sleep(TryInterval.Value);
                        }
                        catch (Exception)
                        {
                            //ignore
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// 重试异常
    /// </summary>
    public sealed class RetryException : Exception
    {
        /// <summary>
        /// gets the number of attempts so far
        /// </summary>
        public int RetryCount { get { return exceptionList.Count; } }

        /// <summary>
        /// get exception that caused the notification, if any
        /// </summary>
        private List<Exception> exceptionList { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public RetryException()
        {
            exceptionList = new List<Exception>();
        }

        /// <summary>
        /// enque a exception
        /// </summary>
        /// <param name="ex"></param>
        public void Enqueue(Exception ex)
        {
            exceptionList.Add(ex);
        }

        /// <summary>
        /// 覆盖ToString实现
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < exceptionList.Count; i++)
            {
                sb.AppendLine("第" + (i + 1) + "次retry异常：" + exceptionList[i]);
            }
            return sb.ToString();
        }
    }


}

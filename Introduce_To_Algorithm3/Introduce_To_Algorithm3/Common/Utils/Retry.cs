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
        private const int _defaultTryCount = 3;

        /// <summary>
        /// get the default try count
        /// </summary>
        public static int DefaultTryCount
        {
            get { return _defaultTryCount; }
        }

        private static readonly TimeSpan _defaultInterval = new TimeSpan(0, 0, 60);

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
        /// </summary>
        public TimeSpan TryInterval { get; private set; }

        /// <summary>
        /// construct with 3 trycount and 1 minute interval
        /// </summary>
        public Retry():this(_defaultTryCount,_defaultInterval)
        {
        }

        /// <summary>
        /// construct retry
        /// </summary>
        /// <param name="tryCount">run at most reyCount times, include the first run</param>
        /// <param name="tryInterval"></param>
        public Retry(int tryCount, TimeSpan tryInterval)
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
        /// <param name="errorNotify"></param>
        public void Invoke(Action action, Action<NotifyEventArgs> errorNotify = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action is null");
            }

            int tryCount = 0;
            NotifyEventArgs eventArgs = null;
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

                    #region error notify

                    if (eventArgs == null)
                    {
                        eventArgs = new NotifyEventArgs();
                    }
                    eventArgs.Enqueue(ex);

                    if (errorNotify != null && tryCount>=TryCount)
                    {
                        try { errorNotify(eventArgs); }
                        catch { }
                    }
                    #endregion

                    if (tryCount >= TryCount)
                    {
                        throw new RetryTimeOutException(eventArgs);
                    }

                    Thread.Sleep(TryInterval);
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
        /// <param name="errorNotify"></param>
        public void Invoke<T>(Action<T> action,T obj, Action<NotifyEventArgs> errorNotify=null)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action is null");
            }

            int tryCount = 0;
            NotifyEventArgs eventArgs = null;
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

                    #region error notify

                    if (eventArgs == null)
                    {
                        eventArgs = new NotifyEventArgs();
                    }
                    eventArgs.Enqueue(ex);

                    if (errorNotify != null && tryCount >= TryCount)
                    {
                        try { errorNotify(eventArgs); }
                        catch { }
                    }
                    #endregion

                    if (tryCount >= TryCount)
                    {
                        throw new RetryTimeOutException(eventArgs);
                    }

                    Thread.Sleep(TryInterval);
                }
            }
        }

        /// <summary>
        /// invoke func, when fails call errorNotify to notify users if it is not null
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="errorNotify"></param>
        /// <returns></returns>
        public TResult Invoke<TResult>(Func<TResult> func, Action<NotifyEventArgs> errorNotify=null)
        {
            if (func == null)
            {
                throw new ArgumentNullException("action is null");
            }

            int tryCount = 0;
            NotifyEventArgs eventArgs = null;
            while (true)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    tryCount++;

                    #region error notify

                    if (eventArgs == null)
                    {
                        eventArgs = new NotifyEventArgs();
                    }
                    eventArgs.Enqueue(ex);

                    if (errorNotify != null && tryCount >= TryCount)
                    {
                        try { errorNotify(eventArgs); }
                        catch { }
                    }
                    #endregion

                    if (tryCount >= TryCount)
                    {
                        throw new RetryTimeOutException(eventArgs);
                    }

                    Thread.Sleep(TryInterval);
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
        /// <param name="errorNotify"></param>
        /// <returns></returns>
        public TResult Invoke<T,TResult>(Func<T,TResult> func, T obj,Action<NotifyEventArgs> errorNotify=null)
        {
            if (func == null)
            {
                throw new ArgumentNullException("action is null");
            }

            int tryCount = 0;
            NotifyEventArgs eventArgs = null;
            while (true)
            {
                try
                {
                    return func(obj);
                }
                catch (Exception ex)
                {
                    tryCount++;

                    #region error notify

                    if (eventArgs == null)
                    {
                        eventArgs = new NotifyEventArgs();
                    }
                    eventArgs.Enqueue(ex);

                    if (errorNotify != null && tryCount >= TryCount)
                    {
                        try { errorNotify(eventArgs); }
                        catch { }
                    }
                    #endregion

                    if (tryCount >= TryCount)
                    {
                        throw new RetryTimeOutException(eventArgs);
                    }

                    Thread.Sleep(TryInterval);
                }
            }
        }
    }


    public sealed class NotifyEventArgs : EventArgs
    {
        /// <summary>
        /// gets the number of attempts so far
        /// </summary>
        public int AttemptCount { get { return ExceptionQueue.Count; } }

        /// <summary>
        /// get exception that caused the notification, if any
        /// </summary>
        public Queue<Exception> ExceptionQueue { get; private set; }

        /// <summary>
        /// constructor
        /// </summary>
        public NotifyEventArgs()
        {
            ExceptionQueue =new Queue<Exception>();
        }

        /// <summary>
        /// enque a exception
        /// </summary>
        /// <param name="ex"></param>
        public void Enqueue(Exception ex)
        {
            ExceptionQueue.Enqueue(ex);
        }
    }


    public class RetryTimeOutException : TimeoutException
    {
        public NotifyEventArgs NotifyEventArgs { get; private set; }

        public RetryTimeOutException(NotifyEventArgs args)
        {
            NotifyEventArgs = args;
        }
    }
}

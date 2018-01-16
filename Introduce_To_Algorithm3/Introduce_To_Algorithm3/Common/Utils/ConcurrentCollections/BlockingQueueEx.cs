using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils.ConcurrentCollections
{
    /// <summary>
    /// 阻塞队列
    /// BlockingQueueEx ex = new BlockingQueueEx(..)//创建完已经启动
    /// ex.Stop()//停止
    /// </summary>
    public class BlockingQueueEx<T> where T : class
    {
        /// <summary>
        /// 底层是ConcurrentQueue
        /// 该实例没有上限
        /// </summary>
        private readonly BlockingCollection<T> _blockingQueue = new BlockingCollection<T>();

        /// <summary>
        /// queue中剩余未处理的数据量
        /// </summary>
        public int UnHandleCount
        {
            get
            {
                try
                {
                    return _blockingQueue.Count;
                }
                catch 
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// 是否程序运行，当退出时设置为false
        /// </summary>
        private volatile bool _isRunning = false;

        /// <summary>
        /// 是否程序运行，当退出时设置为false
        /// </summary>
        private bool IsRunning
        {
            get
            {
                lock (_locker)
                {
                    return _isRunning;
                }
            }
        }

        /// <summary>
        /// 数据处理
        /// </summary>
        private readonly Action<T> _dataHandler = null;

        /// <summary>
        /// 异常处理
        /// </summary>
        private readonly Action<Exception> _exceptionHandler = null;

        /// <summary>
        /// 底层线程
        /// </summary>
        private readonly Thread _thread = null;

        /// <summary>
        /// 队列中消息的最大数量，超过该数量，之前的消息将被丢弃
        /// </summary>
        private readonly int _maxNumDataInQueue = 2048;

        /// <summary>
        /// 构造函数  构造完即开启处理线程
        /// </summary>
        /// <param name="dataHandler">数据处理</param>
        /// <param name="exceptionHandler">数据处理异常后的处理</param>
        /// <param name="maxNumberDataInQueue">队列中消息的最大数量，超过该数量，之前的消息将被丢弃 最小是100,如果小于100,将会赋值为100</param>
        public BlockingQueueEx(Action<T> dataHandler = null, Action<Exception> exceptionHandler = null, int maxNumberDataInQueue = 2048)
        {

            lock (_locker)
            {
                _isRunning = true;
                this._dataHandler = dataHandler;
                this._exceptionHandler = exceptionHandler;

                //队列中消息的最大数量，超过该数量，之前的消息将被丢弃 最小是100,如果小于100,将会赋值为100
                if (maxNumberDataInQueue < 100)
                {
                    this._maxNumDataInQueue = 100;
                }
                else
                {
                    this._maxNumDataInQueue = maxNumberDataInQueue;
                }
            }
            _thread = new Thread(() =>
            {
                T item = null;
                Action<T> dataHandlerInThread = null;
                Action<Exception> exceptionHandlerInThread = null;

                lock (_locker)
                {
                    dataHandlerInThread = this._dataHandler;
                    exceptionHandlerInThread = this._exceptionHandler;
                }


                while (IsRunning)
                {
                    try
                    {
                        if (_blockingQueue.TryTake(out item, 513))
                        {
                            dataHandlerInThread?.Invoke(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionHandlerInThread?.Invoke(ex);
                    }
                }

                NLogHelper.Trace($"Message处理循环退出");
            });

            //后台线程
            _thread.IsBackground = true;
            _thread.Priority = ThreadPriority.Normal;
            _thread.Start();
        }


        /// <summary>
        /// 添加一个数据项
        /// </summary>
        /// <param name="item">要处理的数据项，如果为null，则什么也不做</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="abandonAction">当queue中数量超过QueueLimit时,T被抛弃时的毁掉</param>
        public void Add(T item, Action<T> abandonAction = null, Action<Exception> exceptionHandler = null)
        {
            try
            {
                if (item == null)
                {
                    return;
                }

                int curCount = _blockingQueue.Count;

                #region 超出limit 抛弃旧元素
                if (curCount > _maxNumDataInQueue)
                {
                    T delT = null;
                    //TryTake立刻返回
                    //If the collection is empty, this method immediately returns false.
                    if (_blockingQueue.TryTake(out delT))
                    {
                        abandonAction?.Invoke(delT);
                    }
                    if (_blockingQueue.TryTake(out delT))
                    {
                        abandonAction?.Invoke(delT);
                    }
                    if (_blockingQueue.TryTake(out delT))
                    {
                        abandonAction?.Invoke(delT);
                    }
                    if (_blockingQueue.TryTake(out delT))
                    {
                        abandonAction?.Invoke(delT);
                    }
                    if (_blockingQueue.TryTake(out delT))
                    {
                        abandonAction?.Invoke(delT);
                    }
                    if (_blockingQueue.TryTake(out delT))
                    {
                        abandonAction?.Invoke(delT);
                    }
                    if (_blockingQueue.TryTake(out delT))
                    {
                        abandonAction?.Invoke(delT);
                    }
                    if (_blockingQueue.TryTake(out delT))
                    {
                        abandonAction?.Invoke(delT);
                    }
                }
                #endregion

                _blockingQueue.Add(item);
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
            }
        }

        ///// <summary>
        ///// 移除一个数据项，移除之前会阻塞
        ///// 返回移除的数据项
        ///// </summary>
        ///// <returns></returns>
        //public T Take()
        //{
        //    return _blockingQueue.Take();
        //}

        ///// <summary>
        ///// 尝试移除一个数据项，成功移除返回true
        ///// 
        ///// </summary>
        ///// <param name="item"></param>
        ///// <param name="milliseconds">等待的毫秒数，或为 Infinite (-1)，表示无限期等待。</param>
        ///// <returns></returns>
        //public bool TryTake(out T item,int milliseconds)
        //{
        //    return _blockingQueue.TryTake(out item, milliseconds);
        //}

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            //if (_blockingQueue != null)
            //{
            //    //没有必要dispose,因为程序退出
            //    //_blockingQueue.Dispose();
            //}
        }




    }

}

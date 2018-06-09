using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.ConcurrentCollections
{
    /// <summary>
    /// 阻塞队列
    /// BlockingQueueEx ex = new BlockingQueueEx(..)//创建完已经启动
    /// ex.Stop()//停止
    /// </summary>
    public class BlockingQueueEx<T>:IDisposable where T : class
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
        private readonly Action<T> _singleDataHandler = null;

        /// <summary>
        /// 数据列表处理
        /// </summary>
        private readonly Action<List<T>> _dataListHandler = null;

        /// <summary>
        /// 列表处理的阀值,当队列长度>=该值时,允许启用列表处理
        /// </summary>
        private const int ThresholdCanBeListHandled = 8;

        /// <summary>
        /// 最大允许的单次处理批次数量
        /// </summary>
        private readonly int _maxBatchSize;

        /// <summary>
        /// 是否启用列表处理 根据是否提供列表处理函数判断
        /// </summary>
        private readonly bool _enableListHandler;

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
        private readonly int _maxNumDataInQueue;

        /// <summary>
        /// 是否需要多线程性能优化
        /// </summary>
        private readonly bool _isNeedOptimize;

        /// <summary>
        /// 构造函数  构造完即开启处理线程
        /// </summary>
        /// <param name="singleDataHandler">单个数据处理</param>
        /// <param name="dataListHandler">如果数据较多，列表处理，如果为null，则不启用列表处理</param>
        /// <param name="exceptionHandler">数据处理异常后的处理</param>
        /// <param name="maxNumberDataInQueue">队列中消息的最大数量，超过该数量，之前的消息将被丢弃 最小是100,如果小于100,将会赋值为100</param>
        /// <param name="isNeedOptimize">是否需要多线程优化消息处理,通常不需要,使用优化是非常危险的行为</param>
        /// <param name="maxBatchSize">最大允许的单次处理批次数量</param>
        public BlockingQueueEx(Action<T> singleDataHandler = null,Action<List<T>> dataListHandler = null,Action<Exception> exceptionHandler = null, int maxNumberDataInQueue = 4096,bool isNeedOptimize = false,int maxBatchSize = 29)
        {

            lock (_locker)
            {
                this._isRunning = true;
                this._singleDataHandler = singleDataHandler;
                this._exceptionHandler = exceptionHandler;
                this._isNeedOptimize = isNeedOptimize;
                this._dataListHandler = dataListHandler;
                //列表处理函数不为null,启用列表处理
                this._enableListHandler = dataListHandler != null;
                this._maxBatchSize = maxBatchSize>ThresholdCanBeListHandled?maxBatchSize:ThresholdCanBeListHandled;

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
                Action<List<T>> dataListHandlerInThread = null;
                bool enableListHandlerInThread = false;
                int maxBatchSizeInThread = ThresholdCanBeListHandled;
                bool isNeedOptimizeInThread = false;//是否需要多线程优化处理，一般不需要

                lock (_locker)
                {
                    dataHandlerInThread = this._singleDataHandler;
                    exceptionHandlerInThread = this._exceptionHandler;
                    dataListHandlerInThread = this._dataListHandler;
                    enableListHandlerInThread = this._enableListHandler;
                    maxBatchSizeInThread = this._maxBatchSize;
                    isNeedOptimizeInThread = this._isNeedOptimize;
                }


                while (_isRunning)
                {
                    try
                    {
                        #region 数据处理

                        if (enableListHandlerInThread)
                        {

                            //当前队列数据量
                            int currentCount = _blockingQueue.Count;

                            if (currentCount < ThresholdCanBeListHandled)
                            {
                                #region 单个处理  不需要list处理

                                if (_blockingQueue.TryTake(out item, 419))
                                {
                                    if (isNeedOptimizeInThread)
                                    {
                                        //需要优化性能
                                        T tempItem = item;
                                        Task.Factory.StartNew(() =>
                                        {
                                            try
                                            {
                                                dataHandlerInThread?.Invoke(tempItem);
                                            }
                                            catch (Exception ex)
                                            {
                                                exceptionHandlerInThread?.Invoke(ex);
                                            }
                                        });
                                    }
                                    else
                                    {
                                        dataHandlerInThread?.Invoke(item);
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                #region 数据较多,列表处理

                                //单次处理的数据量
                                int dataListSize = ThresholdCanBeListHandled;

                                if (currentCount > ThresholdCanBeListHandled * 2)
                                {
                                    dataListSize = ThresholdCanBeListHandled * 2;
                                }

                                //最大单次处理的批次
                                if (currentCount > maxBatchSizeInThread)
                                {
                                    dataListSize = maxBatchSizeInThread;
                                }


                                List<T> dataList = new List<T>(dataListSize);
                                for (int i = 0; i < dataListSize; i++)
                                {
                                    if (_blockingQueue.TryTake(out item))
                                    {
                                        //有数据立刻返回
                                        dataList.Add(item);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                if (isNeedOptimizeInThread)
                                {
                                    //需要优化性能
                                    Task.Factory.StartNew(() =>
                                    {
                                        try
                                        {
                                            dataListHandlerInThread?.Invoke(dataList);
                                        }
                                        catch (Exception ex)
                                        {
                                            exceptionHandlerInThread?.Invoke(ex);
                                        }
                                    });
                                }
                                else
                                {
                                    dataListHandlerInThread?.Invoke(dataList);
                                }

                                #endregion
                            }
                        }
                        else
                        {

                            #region 单个处理  不需要list处理

                            if (_blockingQueue.TryTake(out item, 419))
                            {
                                if (isNeedOptimizeInThread)
                                {
                                    //需要优化性能
                                    T tempItem = item;
                                    Task.Factory.StartNew(() =>
                                    {
                                        try
                                        {
                                            dataHandlerInThread?.Invoke(tempItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            exceptionHandlerInThread?.Invoke(ex);
                                        }
                                    });
                                }
                                else
                                {
                                    dataHandlerInThread?.Invoke(item);
                                }
                            }

                            #endregion
                        }

                        #endregion
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
        /// 本方法是多线程安全的
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

                    for (int i = 0; i < 5; i++)
                    {
                        if (_blockingQueue.TryTake(out delT))
                        {
                            abandonAction?.Invoke(delT);
                        }
                        else
                        {
                            break;
                        }
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
            DateTime now = DateTime.Now;

            int i = 0;
            const int sleepMs = 7;//休眠时间毫秒
            while (UnHandleCount > 0)
            {
                Thread.Sleep(sleepMs);

                i++;
                if ((DateTime.Now - now).TotalSeconds > 1.03 || i*sleepMs>2000)
                {
                    NLogHelper.Warn($"BlockingQueue存在约{UnHandleCount}条数据未处理完成");
                    break;
                }
            }

            _isRunning = false;
            //if (_blockingQueue != null)
            //{
            //    //没有必要dispose,因为程序退出
            //    //_blockingQueue.Dispose();
            //}
        }

        /// <summary>
        /// Dispose接口
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }

}

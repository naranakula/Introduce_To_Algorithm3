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
    /// 
    /// </summary>
    public class BlockingQueueEx<T> where T:class
    {
        /// <summary>
        /// 底层是ConcurrentQueue
        /// 该实例没有上限
        /// </summary>
        private readonly BlockingCollection<T> _blockingQueue = new BlockingCollection<T>();

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// 是否程序运行，当退出时设置为false
        /// </summary>
        private volatile bool _isRunning = false;

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
        /// 构造函数  构造完即开启处理线程
        /// </summary>
        /// <param name="dataHandler">数据处理</param>
        /// <param name="exceptionHandler">数据处理异常后的处理</param>
        public BlockingQueueEx(Action<T> dataHandler = null,Action<Exception> exceptionHandler = null)
        {
            
            lock (_locker)
            {
                _isRunning = true;
                this._dataHandler = dataHandler;
                this._exceptionHandler = exceptionHandler;
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


                while (_isRunning)
                {
                    try
                    {
                        if (_blockingQueue.TryTake(out item, 513))
                        {
                            dataHandlerInThread?.Invoke(item);
                        }
                    }
                    catch(Exception ex)
                    {
                        exceptionHandlerInThread?.Invoke(ex);
                    }
                }
            });

            //后台线程
            _thread.IsBackground = true;

            _thread.Start();
        }


        /// <summary>
        /// 添加一个数据项
        /// </summary>
        /// <param name="item">要处理的数据项，如果为null，则什么也不做</param>
        /// <param name="exceptionHandler">异常处理</param>
        public void Add(T item,Action<Exception> exceptionHandler = null)
        {
            try
            {
                if (item == null)
                {
                    return;
                }

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

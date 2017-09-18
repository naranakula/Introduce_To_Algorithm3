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
        private BlockingCollection<T> _blockingQueue = new BlockingCollection<T>();

        /// <summary>
        /// 是否正在运行
        /// </summary>
        private volatile bool _isRunning = false;

        /// <summary>
        /// 数据处理
        /// </summary>
        private volatile Action<T> _dataHandler = null;


        /// <summary>
        /// 异常处理
        /// </summary>
        private volatile Action<Exception> _exceptionHandler = null;

        private Thread _thread = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataHandler">数据处理</param>
        public BlockingQueueEx(Action<T> dataHandler = null,Action<Exception> exceptionHandler = null)
        {
            _isRunning = true;
            this._dataHandler = dataHandler;
            this._exceptionHandler = exceptionHandler;
            _thread = new Thread(() =>
            {
                T item = null;
                while (_isRunning)
                {
                    try
                    {
                        if (_blockingQueue.TryTake(out item, 713))
                        {
                            _dataHandler?.Invoke(item);
                        }
                    }
                    catch(Exception ex)
                    {
                        Thread.Sleep(10);
                        _exceptionHandler?.Invoke(ex);
                    }
                }
            });
            _thread.Start();
        }


        /// <summary>
        /// 添加一个数据项
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            _blockingQueue.Add(item);
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
            if (_blockingQueue != null)
            {
                _blockingQueue.Dispose();
            }
            _blockingQueue = null;
        }




    }
}

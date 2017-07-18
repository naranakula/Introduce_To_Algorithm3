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
    public class BlockingQueueEx<T>:IDisposable where T:class
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

        private Thread _thread = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataHandler">数据处理</param>
        public BlockingQueueEx(Action<T> dataHandler = null,Action<Exception> exceptionHandler = null)
        {
            _isRunning = true;
            this._dataHandler = dataHandler;
            _thread = new Thread(() =>
            {
                T item = null;
                while (_isRunning)
                {
                    try
                    {
                        if (_blockingQueue.TryTake(out item, 1111))
                        {
                            if(_dataHandler != null)
                            {
                                _dataHandler(item);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Thread.Sleep(1);
                        if(exceptionHandler != null)
                        {
                            exceptionHandler(ex);
                        }
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

        /// <summary>
        /// 移除一个数据项，移除之前会阻塞
        /// 返回移除的数据项
        /// </summary>
        /// <returns></returns>
        public T Take()
        {
            return _blockingQueue.Take();
        }

        /// <summary>
        /// 尝试移除一个数据项，成功移除返回true
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="milliseconds">等待的毫秒数，或为 Infinite (-1)，表示无限期等待。</param>
        /// <returns></returns>
        public bool TryTake(out T item,int milliseconds)
        {
            return _blockingQueue.TryTake(out item, milliseconds);
        }

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
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                Stop();
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~BlockingQueueEx() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion




    }
}

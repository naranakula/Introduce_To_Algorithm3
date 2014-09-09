using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 同步类，主要演示各种同步技术
    /// </summary>
    public class SynchronizationHelper
    {
        /// <summary>
        /// 使用lock关键字，锁定线程，底层实际上使用的是Monitor类
        /// </summary>
        /// <param name="action"></param>
        public void Lock(Action action)
        {
            lock (this)
            {
                action();
            }
        }

        /// <summary>
        /// Monitor类
        /// </summary>
        /// <param name="action"></param>
        public void MonitorEx(Action action)
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(this, ref lockTaken);
                action();
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(this);
                }
            }
        }

        #region Interlocked提供加减交换的原子操作

        #endregion


        #region SpinLock 空转锁
        //该对象应该声明在外面
        SpinLock spinLock = new SpinLock();
        /// <summary>
        /// 空转锁，等待锁的线程并不放弃cpu，而是空转。
        /// </summary>
        /// <param name="action"></param>
        public void Spinlock(Action action)
        {
            bool lockTaken = false;
            try
            {
                spinLock.Enter(ref lockTaken);
                action();
            }
            finally
            {
                if(lockTaken)
                {
                    spinLock.Exit();
                }
            }
        }
        #endregion

        #region Mutex空转锁

        /// <summary>
        /// 创建互斥体，创建线程不拥有该信号,本地mutex，不能在进程间使用
        /// </summary>
        private Mutex mutex = new Mutex(false);

        /// <summary>
        /// Mutex实例
        /// </summary>
        /// <param name="action"></param>
        public void Mutexx(Action action)
        {
            bool lockAcquired = false;
            try
            {
                lockAcquired = mutex.WaitOne();
                action();
            }
            finally
            {
                if(lockAcquired)
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        /// <summary>
        /// 打开一个命名的跨进程的mutex
        /// </summary>
        /// <param name="mutexName"></param>
        /// <param name="mutex"></param>
        /// <returns>如果新建一个，返回false；如果打开现有的，返回true</returns>
        public bool OpenIfExist(string mutexName,out Mutex mutex)
        {
            try
            {
                mutex = Mutex.OpenExisting(mutexName);
                return true;
            }
            catch
            {
                mutex = new Mutex(false,mutexName);
                return false;
            }
        }


        #endregion

        #region 读写锁，轻量级的版本带slim结尾

        /// <summary>
        /// 读写锁
        /// </summary>
        private ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 读
        /// </summary>
        /// <param name="action"></param>
        public void Read(Action action)
        {
            try
            {
                readerWriterLock.EnterReadLock();
                action();
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        /// <summary>
        /// 读
        /// </summary>
        /// <param name="action"></param>
        public void ReadUpgrate(Action action)
        {
            try
            {
                readerWriterLock.EnterUpgradeableReadLock();
                action();
            }
            finally
            {
                readerWriterLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// 写
        /// </summary>
        /// <param name="action"></param>
        public void Write(Action action)
        {
            try
            {
                readerWriterLock.EnterWriteLock();
                action();
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        #endregion

        #region 使用System.Collections.Concurrent并行集合
        #endregion


        #region Barrier 多个线程分阶段执行
        #endregion

        #region CountDownEventClass多个线程分阶段执行
        #endregion

        #region ManualResetEventSlim 单信号  AutoResetEvent
        #endregion

        #region SemaphoreSlim多个线程分阶段执行
        #endregion

        #region BlockingCollection做生产者消费者模型
        #endregion
    }

    /// <summary>
    /// 声明式同步类，同步所有的属性和方法
    /// 需要加[Synchronization]声明，并继承自ContextBoundObject
    /// </summary>
    [Synchronization]
    public class SyncClass:ContextBoundObject
    {
        
    }

}

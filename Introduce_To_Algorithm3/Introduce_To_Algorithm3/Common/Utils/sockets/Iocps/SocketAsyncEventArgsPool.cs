using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// A collection of reusable SocketAsyncEventArgs objects
    /// 非线程安全
    /// </summary>
    public class SocketAsyncEventArgsPool
    {
        /// <summary>
        /// 底层的stack
        /// </summary>
        private Stack<SocketAsyncEventArgs> m_pool;

        /// <summary>
        /// Initializes the object pool to the specified size
        /// </summary>
        /// <param name="capacity">池的初始容量</param>
        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        /// Add a SocketAsyncEventArgs to the pool
        /// </summary>
        /// <param name="item">不能为null</param>
        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Item can't be null");
            }

            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }

        /// <summary>
        /// Pop 
        /// Remove a SocketAsyncEventArgs instance from the pool and return the object removed from the pool
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Pop()
        {
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }

        /// <summary>
        /// The number of SocketAsyncEventArgs instances in the pool
        /// </summary>
        public int Count
        {
            get { return m_pool.Count; }
        }

    }
}

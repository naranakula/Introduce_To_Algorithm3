using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// this class creates a single large buffer which can be divided up and assigned to SocketAsyncEventArgs objects for use with each socket I/O operation.
    /// This enables buffers to be easily resued and guards against fragmenting heap memory
    /// The operations exposed on the BufferManager class are not thread safe
    /// BufferManager中每段buffer长度是固定不变的
    /// </summary>
    public class BufferManager
    {
        /// <summary>
        /// the total number of bytes controlled by the buffer pool
        /// </summary>
        private int m_numBytes;

        /// <summary>
        /// the underlying byte array maintained by the buffer manager
        /// </summary>
        private byte[] m_buffer;

        /// <summary>
        /// 存放可以使用的buffer池的起始位置
        /// </summary>
        private Stack<int> m_freeIndexPool;

        /// <summary>
        /// 标记可用的buffer池的开始第一个位置
        /// </summary>
        private int m_currentIndex;

        /// <summary>
        /// 每段buffer的大小
        /// 每段buffer长度是一样的
        /// </summary>
        private int m_bufferSize;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="totalBytes"></param>
        /// <param name="bufferSize"></param>
        public BufferManager(int totalBytes, int bufferSize)
        {
            m_numBytes = totalBytes;
            m_currentIndex = 0;
            m_bufferSize = bufferSize;
            m_freeIndexPool = new Stack<int>();
        }

        /// <summary>
        /// Allocates buffer space used by the buffer pool
        /// </summary>
        public void InitBuffer()
        {
            //Create one big large buffer and divide that out to each SocketAsyncEventArgs object
            m_buffer = new byte[m_numBytes];
        }

        /// <summary>
        /// assign a buffer from the buffer pool to the specified SocketAsyncEventArgs object
        /// </summary>
        /// <param name="args"></param>
        /// <returns>true, if the buffer was successfully set, else false</returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer,m_freeIndexPool.Pop(),m_bufferSize);
            }
            else
            {
                if ((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    //剩余的字节数不够m_bufferSize
                    return false;
                }

                //新分配一个buffer
                args.SetBuffer(m_buffer,m_currentIndex,m_bufferSize);
                //记录下一个buffer的起始位置
                m_currentIndex += m_bufferSize;
            }

            return true;
        }

        /// <summary>
        /// Removes the buffer from a SocketAsyncEventArgs object
        /// This frees the buffer back to the buffer pool
        /// </summary>
        /// <param name="args"></param>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
            args.SetBuffer(null,0,0);
        }
    }
}

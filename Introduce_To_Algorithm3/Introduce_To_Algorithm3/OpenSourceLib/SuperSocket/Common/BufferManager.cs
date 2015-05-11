using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.SuperSocket.Common
{
    /// <summary>
    /// This class create a single large buffer which can be divided up and used for socket I/O operation.
    /// This enables buffers to be easily reused and guards against fragmenting heap memory.
    /// 
    /// The operations exposed on the BufferManager class are not thread safe.
    /// </summary>
    public class BufferManager
    {
        /// <summary>
        /// The total number of bytes controlled by the buffer pool
        /// </summary>
        private int m_numBytes;

        /// <summary>
        /// the underlying byte array maintained by the buffer manager
        /// </summary>
        private byte[] m_buffer;
    }
}

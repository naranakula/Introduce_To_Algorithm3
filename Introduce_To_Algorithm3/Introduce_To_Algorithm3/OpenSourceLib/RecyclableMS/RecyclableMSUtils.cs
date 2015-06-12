using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IO;

namespace Introduce_To_Algorithm3.OpenSourceLib.RecyclableMS
{
    /// <summary>
    /// RecyclableMemoryStream帮助类
    /// RecyclableMemoryStream在以下方面做了优化：
    /// 1）通过Pooled buffers缓解大型对象的Heap分配
    /// 2）更少的GC
    /// 3）避免固定大小的对象池的内存泄漏
    /// </summary>
    public static class RecyclableMSUtils
    {
        /// <summary>
        ///  RecyclableMemoryStreamManager should be declared once and it will live for the entire process–this is the pool. 
        /// it's thread safe
        /// </summary>
        private static RecyclableMemoryStreamManager _manager = new RecyclableMemoryStreamManager();

        static RecyclableMSUtils()
        {
            int blockSize = 1024;
            int largeBufferMultiple = 1024 * 1024;
            int maxBufferSize = 16 * largeBufferMultiple;

            var manager = new RecyclableMemoryStreamManager(blockSize,
                                                            largeBufferMultiple,
                                                            maxBufferSize);

            manager.GenerateCallStacks = true;
            manager.AggressiveBufferReturn = true;
            manager.MaximumFreeLargePoolBytes = maxBufferSize * 4;
            manager.MaximumFreeSmallPoolBytes = 100 * blockSize;

            _manager = manager;
        }

        public static byte[] GetBuffer()
        {
            return _manager.GetStream().GetBuffer();
        }

        public static byte[] GetBuffer(string tag)
        {
            return _manager.GetStream(tag).GetBuffer();
        }

        public static byte[] GetBuffer(string tag,byte[] buffer)
        {
            return _manager.GetStream(tag,buffer,0,buffer.Length).GetBuffer();
        }
    }
}

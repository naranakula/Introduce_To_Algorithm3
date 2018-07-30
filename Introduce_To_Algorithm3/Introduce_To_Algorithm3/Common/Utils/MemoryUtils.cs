using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public static class MemoryUtils
    {

        //定义内存的信息结构
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            public uint dwLength;
            //A number between 0 and 100 that specifies the approximate percentage of physical memory that is in use (0 indicates no memory use and 100 indicates full memory use).
            public uint dwMemoryLoad;
             
            //The amount of actual physical memory, in bytes.
            public uint dwTotalPhys;
            //The amount of physical memory currently available, in bytes.
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }

        /// <summary>
        /// 如果有多个内存条，只能获取一个内存条信息
        /// </summary>
        /// <param name="meminfo"></param>
        [DllImport("kernel32")]
        private static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);


        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <param name="memoryInfo"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool TryGetMemoryInfo(out MEMORY_INFO memoryInfo,Action<Exception> exceptionHandler = null)
        {
            memoryInfo = new MEMORY_INFO();
            try
            {
                

                GlobalMemoryStatus(ref memoryInfo);

                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }



    }
    

}

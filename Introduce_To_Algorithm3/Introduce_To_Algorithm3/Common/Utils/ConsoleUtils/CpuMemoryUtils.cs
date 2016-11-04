using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// Cpu Memory工具
    /// </summary>
    public class CpuMemoryUtils
    {
        #region 内存相关
        /// <summary>
        /// 获取系统内存信息
        /// </summary>
        /// <param name="meminfo"></param>
        [DllImport("kernel32")]
        private static extern void GlobalMemoryStatus(ref MemoryInfo meminfo);

        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <returns></returns>
        public static MemoryInfo GetCurrentMemoryInfo()
        {
            MemoryInfo memoryInfo = new MemoryInfo();
            //获取内存信息
            GlobalMemoryStatus(ref  memoryInfo);
            return memoryInfo;
        }
        #endregion

        #region Cpu相关

        /// <summary>
        /// 创建cpu性能计数器
        /// </summary>
        private static PerformanceCounter cpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        /// <summary>
        /// 计算当前cpu值
        /// </summary>
        /// <returns></returns>
        public static double GetNextCpuUsedPercent()
        {
            //这里获取的是当前值，应该取最近几次监控的平均值
            //0 - 100之间的数据
            return cpuLoad.NextValue();
        }

        #endregion
    }



    /// <summary>
    /// 定义内存的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryInfo
    {

        public uint dwLength;
        /// <summary>
        /// percent of memory in use
        /// </summary>
        public uint dwMemoryLoad;
        /// <summary>
        /// bytes of physical memory
        /// </summary>
        public uint dwTotalPhys;
        /// <summary>
        /// free physical memory bytes
        /// </summary>
        public uint dwAvailPhys;
        /// <summary>
        /// 交换文件总大小
        /// </summary>
        public uint dwTotalPageFile;
        /// <summary>
        /// 可用交换文件大小
        /// </summary>
        public uint dwAvailPageFile;
        /// <summary>
        /// 总虚拟内存大小
        /// </summary>
        public uint dwTotalVirtual;
        /// <summary>
        /// 可用虚拟内存大小
        /// </summary>
        public uint dwAvailVirtual;
    }
}

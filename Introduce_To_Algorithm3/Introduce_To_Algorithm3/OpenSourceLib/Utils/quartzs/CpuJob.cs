using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;
using Quartz;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs
{

    /// <summary>
    /// 获取CPU状态
    /// 建议执行周期100ms
    /// </summary>
    public class CpuJob:IJob
    {

        /// <summary>
        /// 一次只能有一个函数被执行
        /// </summary>
        private static volatile OneRunAtSameTime OneRun = new OneRunAtSameTime();

        /// <summary>
        /// 使用的cpu
        /// </summary>
        private static ConcurrentQueue<double> cpuUsedQueue = new ConcurrentQueue<double>();

        /// <summary>
        /// cpu性能计数器
        /// </summary>
        private static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        /// <summary>
        /// 计算当前cpu使用的百分比 0-100的浮点数
        /// 取最近500ms的平均值
        /// </summary>
        private static double _cpuUsedPercent = 0;

        /// <summary>
        /// 计算当前cpu使用的百分比 0-100的浮点数
        /// </summary>
        public static double CpuUsedPercent
        {
            get
            {
                lock (OneRun)
                {
                    return _cpuUsedPercent;
                }
            }
        }


        public void Execute(IJobExecutionContext context)
        {
            OneRun.RunSafe(() =>
            {
                //计算当前cpu使用的百分比 0-100的浮点数
                double dcpuUsed = cpuCounter.NextValue();
                cpuUsedQueue.Enqueue(dcpuUsed);

                //这里5表示取最近5个采集的平均值
                double d;
                while (cpuUsedQueue.Count > 5)
                {
                    cpuUsedQueue.TryDequeue(out d);
                }

                d = 0;
                double[] arr = cpuUsedQueue.ToArray();

                foreach(var arrItem in arr)
                {
                    d += arrItem;
                }

                if (arr.Length > 0)
                {
                    lock (OneRun)
                    {
                        _cpuUsedPercent = d/arr.Length;
                    }
                }
            });
        }
    }
}

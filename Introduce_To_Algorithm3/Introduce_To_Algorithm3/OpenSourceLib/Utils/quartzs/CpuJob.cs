using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;
using Quartz;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs
{

    /// <summary>
    /// 获取CPU状态
    /// 建议执行周期每3-5分钟
    /// </summary>
    public class CpuJob:IJob
    {

        /// <summary>
        /// cpu性能计数器
        /// </summary>
        private static volatile PerformanceCounter _cpuCounter = null;



        public void Execute(IJobExecutionContext context)
        {
            lock (_locker)
            {
                if (_isRunning)
                {
                    return;
                }
                _isRunning = true;
            }

            string jobName = "";
            try
            {
                PerformanceCounter curCpuCounter = null;
                lock (_locker)
                {

                    if (_cpuCounter == null)
                    {
                        _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    }
                    curCpuCounter = _cpuCounter;
                }


                jobName = GetType().Name;

                if (curCpuCounter == null)
                {
                    return;
                }

                
                const int sampleCount = 5;
                const int sleepMilliseconds = 137;
                double cpuUsed = 0;
                for (int i = 0; i < sampleCount+1; i++)
                {
                    if (i == 0)
                    {
                        //第一次不统计
                        curCpuCounter.NextValue();
                        Thread.Sleep(sleepMilliseconds);
                        continue;
                    }
                    //计算当前cpu使用的百分比 0-100的浮点数
                    cpuUsed += curCpuCounter.NextValue();
                    Thread.Sleep(sleepMilliseconds);
                }

                cpuUsed = cpuUsed / sampleCount;

                if (cpuUsed > MaxPercentCpuCanUse)
                {
                    NLogHelper.Warn($"cpu使用超过{cpuUsed.ToString("F1", CultureInfo.CurrentCulture)}%");
                    ErrorCode = 1;
                    ErrorReason = $"该机器占用cpu较大,需人工检查";
                }
                else
                {
                    ErrorCode = 0;
                    ErrorReason = string.Empty;
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error($"执行任务{jobName}异常:" + ex);
            }
            finally
            {
                lock (_locker)
                {
                    _isRunning = false;
                }
            }
        }


        #region 辅助属性
        

        /// <summary>
        /// 当前进程所在机器最大可以使用的cpu比例  (0,100)之间的整数
        /// </summary>
        private static readonly int MaxPercentCpuCanUse = ConfigUtils.GetInteger("MaxPercentDiskCanUse", 93);

        #region 对外暴露数据

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly Object DataLocker = new Object();

        /// <summary>
        /// 错误码，0表示正常,其它表示错误
        /// </summary>
        private static int _errorCode = 0;

        /// <summary>
        /// 错误码，0表示正常,其它表示错误
        /// </summary>
        public static int ErrorCode
        {
            get
            {
                lock (DataLocker)
                {
                    return _errorCode;
                }
            }
            private set
            {
                lock (DataLocker)
                {
                    _errorCode = value;
                }
            }
        }

        /// <summary>
        /// 错误描述
        /// </summary>
        private static string _errorReason = "";

        /// <summary>
        /// 错误描述
        /// </summary>
        public static string ErrorReason
        {
            get
            {
                lock (DataLocker)
                {
                    return _errorReason;
                }
            }
            private set
            {
                lock (DataLocker)
                {
                    _errorReason = value;
                }
            }
        }

        #endregion

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly Object _locker = new Object();

        /// <summary>
        /// 回调函数是否正在执行
        /// </summary>
        private static bool _isRunning = false;

        #endregion

    }


}

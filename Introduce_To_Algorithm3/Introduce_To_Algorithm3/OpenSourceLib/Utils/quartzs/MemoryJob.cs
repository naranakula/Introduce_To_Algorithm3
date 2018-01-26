using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;
using Quartz;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs
{
    /// <summary>
    /// 监控当前进程占用的内存，如果超过指定内存，则对外提供异常信息
    /// 可每5分钟左右执行一次
    /// </summary>
    public class MemoryJob:IJob
    {


        #region IJob实现

        /// <summary>
        /// 注：该方法将定期按时执行，
        /// 意味着如果下一个周期到来，而上一次执行未完成，该方法开启一个新线程执行
        /// 
        /// 在方法内部使用try/catch捕获所有异常
        /// </summary>
        /// <param name="context"></param>
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

            string jobName = string.Empty;
            try
            {
                jobName = GetType().Name;

                //获取当前进程 dispose不会关闭程序，但是会释放资源
                using (Process currentProcess = Process.GetCurrentProcess())
                {
                    //win98或win me不支持 获取当前进程分配的物理内存
                    long workset64 = currentProcess.WorkingSet64;
                    double worksetInMb = workset64 / 1024.0 / 1024.0;

                    if (worksetInMb > MaxMemoryCanUseInMb)
                    {
                        NLogHelper.Warn(
                            $"进程{currentProcess.ProcessName}占用内存较大,约{worksetInMb.ToString("F1", CultureInfo.CurrentCulture)}Mb");
                        ErrorCode = 1;
                        ErrorReason = $"进程{currentProcess.ProcessName}占用内存较大,建议检查或者重启该程序";
                    }
                    else
                    {
                        ErrorCode = 0;
                        ErrorReason = string.Empty;
                    }
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



        #endregion



        #region 辅助属性

        /// <summary>
        /// 当前进程最大允许使用的内存数量
        /// 单位MB
        /// </summary>
        private static readonly int MaxMemoryCanUseInMb = ConfigUtils.GetInteger("MaxMemoryCanUseInMb", 1024);

        #region 对外暴露数据

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly Object DataLocker = new Object();

        /// <summary>
        /// 错误码，0表示正常,其它表示错误
        /// </summary>
        private static int _errorCode=0;

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

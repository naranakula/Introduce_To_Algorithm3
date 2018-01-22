using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.strings;
using Quartz;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs
{
    /// <summary>
    /// 监控当前程序所在的硬盘，如果硬盘剩余空间小于一定比例报警
    /// 可每5分钟左右执行一次
    /// </summary>
    public class DiskJob:IJob
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

                DriveInfo driveInfo = null;
                //获取当前目录
                DirectoryInfo curDir = new DirectoryInfo(Directory.GetCurrentDirectory());

                foreach (DriveInfo item in DriveInfo.GetDrives())
                {
                    if (StringUtils.EqualsEx(curDir.Root.Name, item.RootDirectory.Name))
                    {
                        //查找所在的磁盘
                        driveInfo = item;
                        break;
                    }
                }

                if (driveInfo == null)
                {
                    ErrorReason = "";
                    ErrorCode = 0;
                }
                else
                {
                    double usedPercent =(driveInfo.TotalSize - driveInfo.AvailableFreeSpace)*1.0 / driveInfo.TotalSize;

                    if (usedPercent > MaxPercentDiskCanUse)
                    {
                        NLogHelper.Warn($"{driveInfo.Name}盘使用超过{(usedPercent*100).ToString("F1",CultureInfo.CurrentCulture)}%");
                        ErrorReason = $"{driveInfo.Name}盘剩余空间较紧张,建议及时处理";
                        ErrorCode = 1;
                    }
                    else
                    {
                        ErrorReason = "";
                        ErrorCode = 0;
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
        /// 当前进程所在磁盘最大可以使用的比例  (0,1)之间的数
        /// </summary>
        private static readonly double MaxPercentDiskCanUse = ConfigUtils.GetDouble("MaxPercentDiskCanUse", 0.85);

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

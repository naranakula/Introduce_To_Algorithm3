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
    /// 清理日志的job
    /// 建议的执行时间
    /// 0 17 4 ? * 1,2,4,6     每个星期天星期一星期三星期五4点17分执行
    /// 0 37 3 ? * 2,5     每个星期一星期四3点37分执行
    /// 0 17 4 * * ?     每天4点17分执行
    /// 0 17 4 1 * ?     每月1号4点17分执行  建议使用这个
    /// </summary>
    public class CleanLogJob:IJob
    {
        #region 回调Job
        /// <summary>
        /// 清理日志job
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            lock (locker)
            {
                if (_isRunning)
                {
                    return;
                }
                _isRunning = true;
            }

            try
            {
                CleanLog();
            }
            catch (Exception ex)
            {
                NLogHelper.Error("清理日志失败："+ex);
            }
            finally
            {
                lock (locker)
                {
                    _isRunning = false;
                }
            }
        }

        /// <summary>
        /// 清理日志
        /// </summary>
        public void CleanLog()
        {
            NLogHelper.Info("开始清理日志");
            DirectoryInfo dirInfo = new DirectoryInfo(LogDir);
            if (!dirInfo.Exists)
            {
                NLogHelper.Warn($"未找到{LogDir}日志目录，无法清理");
                return;
            }


            DateTime expireTime = DateTime.Now.Subtract(new TimeSpan(KeepDays, 0, 0, 0));//过期时间

            #region 根据当前硬盘减少保存时间
            try
            {
                DriveInfo driveInfo = null;

                foreach (DriveInfo item in DriveInfo.GetDrives())
                {
                    if (StringUtils.EqualsEx(dirInfo.Root.Name, item.Name))
                    {
                        //查找logs所在的磁盘
                        driveInfo = item;
                        break;
                    }
                }

                //如果磁盘空间不足，保留更少的天数
                if (driveInfo != null && driveInfo.AvailableFreeSpace < driveInfo.TotalSize * DriveAvailableLimit)
                {
                    //当到达硬盘利用极限时，保存的天数
                    expireTime = DateTime.Now.Subtract(new TimeSpan(KeepDaysWhenAvailableLimit, 0, 0, 0));
                }
            }
            catch(Exception ex)
            {
                //ignore
                NLogHelper.Warn($"根据硬盘当前容量判断过期时间失败:{ex}");
            }
            #endregion

            NLogHelper.Info($"清理{expireTime.ToString("yyyyMMdd HH:mm:ss",CultureInfo.CurrentCulture)}之前的日志");

            //清理文件
            try
            {
                CleanFiles(LogDir, SearchPatternArr, expireTime, currentDepth:0);
            }
            catch (Exception ex)
            {
                NLogHelper.Warn($"清理文件失败：{ex}");
            }

            try
            {
                //清理空目录
                CleanEmptyDirectory(LogDir,  currentDepth:0);
            }
            catch (Exception ex)
            {
                NLogHelper.Warn($"清理空目录失败:{ex}");
            }

            NLogHelper.Info("清理日志完成");
        }

        /// <summary>
        /// 清理文件及空目录
        /// </summary>
        /// <param name="topDir">清理的顶层目录，该目录不会被删除</param>
        /// <param name="fileSearchPatternArr">
        /// 文件搜索模式 *所有文件 *.*有拓展名的文件
        /// *：表示0个或多个字符
        /// ?：表示0个或1个字符
        /// 采用*.log|*.txt格式，多个项通过|隔开，该功能是通过自己写代码实现的
        /// </param>
        /// <param name="expireTime">文件过期时间</param>
        /// <param name="currentDepth">当前递归深度</param>
        private void CleanFiles(string topDir, string[] fileSearchPatternArr, DateTime expireTime,int currentDepth)
        {
            if (currentDepth > MaxRecursiveDepth)
            {
                NLogHelper.Warn($"日志文件夹深度太大或者存在快捷方式导致无穷递归");
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(topDir);
            if (!dirInfo.Exists)
            {
                NLogHelper.Warn($"未找到{topDir}目录，无法清理");
                return;
            }


            //If you choose AllDirectories and the directory structure contains a link that creates a loop, the search operation enters an infinite loop.
            //FileInfo[] fileInfos = dirInfo.GetFiles(fileSearchPattern, SearchOption.AllDirectories);
            
            HashSet<string> fileInfos = new HashSet<string>();
            foreach (string searchPatternItem in fileSearchPatternArr)
            {
                //仅搜索当前目录，不递归
                dirInfo.GetFiles(searchPatternItem, SearchOption.TopDirectoryOnly).Select(r => r.FullName).ToList().ForEach(r => fileInfos.Add(r.Trim()));
            }

            //删除过期文件
            foreach (string fileName in fileInfos)
            {

                FileInfo fileInfo = new FileInfo(fileName);
                //文件过期
                if (fileInfo.Exists && fileInfo.LastWriteTime < expireTime)
                {
                    try
                    {
                        fileInfo.Delete();
                        NLogHelper.Trace("删除文件:" + fileInfo.FullName);
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Warn($"删除文件{fileInfo.FullName}失败:{ex}");
                    }
                }
            }

            //递归子文件夹
            DirectoryInfo[] subDirs = dirInfo.GetDirectories();//返回当前目录的子目录。如果没有子目录，则此方法返回一个空数组。 此方法不是递归的。
            foreach (var subDir in subDirs)
            {
                CleanFiles(subDir.FullName,fileSearchPatternArr,expireTime,currentDepth+1);
            }
        }

        /// <summary>
        /// 清除空目录
        /// </summary>
        /// <param name="topDir">顶级目录</param>
        /// <param name="currentDepth">当前递归深度</param>
        private void CleanEmptyDirectory(string topDir,  int currentDepth)
        {
            if (currentDepth > MaxRecursiveDepth)
            {
                NLogHelper.Warn($"日志文件夹存在快捷方式导致无穷递归");
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(topDir);
            if (!dirInfo.Exists)
            {
                NLogHelper.Warn($"未找到{topDir}目录，无法清理");
                return;
            }


            //递归子文件夹
            DirectoryInfo[] subDirs = dirInfo.GetDirectories();//返回当前目录的子目录。如果没有子目录，则此方法返回一个空数组。 此方法不是递归的。
            //非空子目录集合
            List<DirectoryInfo> nonEmptyDirList = new List<DirectoryInfo>();


            DateTime now = DateTime.Now;
            //删除空目录
            foreach (var currentDir in subDirs)
            {
                //文件夹为空, 并且空目录至少2天没有使用
                if (currentDir.Exists && currentDir.GetFileSystemInfos().Length == 0 && (now - currentDir.LastWriteTime).TotalDays > 2)
                {
                    try
                    {
                        currentDir.Delete();
                        NLogHelper.Trace("删除目录：" + currentDir.FullName);
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Warn($"删除目录{currentDir.FullName}失败:{ex}");
                    }
                }

                //获取存在子目录的目录
                if (currentDir.Exists && currentDir.GetDirectories().Length != 0)
                {
                    nonEmptyDirList.Add(currentDir);
                }
            }
            
            //递归子目录
            foreach (var subDir in nonEmptyDirList)
            {
                CleanEmptyDirectory(subDir.FullName,currentDepth+1);
            }

        }

        #endregion


        #region 辅助属性

        /// <summary>
        /// 要清理日志的目录
        /// </summary>
        private const string LogDir = "logs";

        /// <summary>
        /// 保存多少天日志，与创建时间比较
        /// </summary>
        private const int KeepDays = 60;

        /// <summary>
        /// 过滤什么样的文件
        /// 
        /// 采用*.log|*.txt格式，多个项通过|隔开，该功能是通过自己写代码实现的
        /// </summary>
        //private const string FilePattern = "*.log";

        /// <summary>
        /// 避免每次递归创建
        /// </summary>
        private static readonly string[] SearchPatternArr = new string[]{"*.log"};//FilePattern.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => r.Trim()).ToArray();

        /// <summary>
        /// 硬盘空间极限可用值。
        /// 当硬盘可用空间小于当前值时，清理日志,只保存KeepDaysWhenAvailableLimit天，否则保存KeepDays天
        /// </summary>
        private const double DriveAvailableLimit = 0.33;

        /// <summary>
        /// 当到达硬盘利用极限时，保存的天数  与创建时间比较
        /// </summary>
        private const int KeepDaysWhenAvailableLimit = 30;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly Object locker = new Object();

        /// <summary>
        /// 回调函数是否正在执行
        /// </summary>
        private static bool _isRunning = false;

        /// <summary>
        /// 最大递归深度，  防止文件夹快捷方式造成的无穷递归
        /// </summary>
        private const int MaxRecursiveDepth = 32;

        #endregion


    }

}

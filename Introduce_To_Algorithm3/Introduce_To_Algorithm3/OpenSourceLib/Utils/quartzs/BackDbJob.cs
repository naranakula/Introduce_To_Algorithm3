using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils.sqls;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2;
using Quartz;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs
{
    /// <summary>
    /// 备份数据库的job
    /// 建议的执行时间
    /// 0 37 4 ? * 1,4,6     每个星期天星期三星期五4点37分执行
    /// 0 37 4 * * ?     每天4点37分执行
    /// </summary>
    public class BackDbJob:IJob
    {
        #region 回调job

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
                if (!Directory.Exists(BackUpDir))
                {
                    Directory.CreateDirectory(BackUpDir);
                }

                //数据库名
                string dbName = EfDbContext.DbName;

                //要备份的数据库名
                string dbBackName = dbName + "_" + DateTime.Now.ToString("yyyy-MM-dd")+".bak";
                //数据库全路径
                string dbFullPath = Path.Combine(BackUpDir, dbBackName);

                if (File.Exists(dbFullPath))
                {
                    //已经存在
                    return;
                }

                string backupSql = string.Format(BackUpSql, dbName, dbFullPath);

                //不能在事务调用
                //路径中的目录必须存在，否则抛异常
                EfDbContext.ExecuteRawNonQuery(backupSql);

            }
            catch (Exception ex)
            {
                Log4netHelper.Error("备份数据库失败：" + ex);
            }
            finally
            {
                lock (locker)
                {
                    _isRunning = false;
                }
            }
        }
        #endregion


        #region 辅助属性

        /// <summary>
        /// backup database 数据库名 to disk='D:/Test.bak'
        /// backup database 数据库名 to disk='存储位置'
        /// </summary>
        public const string BackUpSql = @"backup database {0} to disk='{1}'";

        /// <summary>
        /// 备份的目录
        /// 如果数据库和程序运行在一台机器，没有问题，
        /// 否则，此目录应该为数据库机器上的共享目录
        /// </summary>
        public const string BackUpDir = @"C:\DbBackup";

        /// <summary>
        /// 锁
        /// </summary>
        private static Object locker = new Object();

        /// <summary>
        /// 回调函数是否正在执行
        /// </summary>
        private static bool _isRunning = false;

        #endregion

    }
}

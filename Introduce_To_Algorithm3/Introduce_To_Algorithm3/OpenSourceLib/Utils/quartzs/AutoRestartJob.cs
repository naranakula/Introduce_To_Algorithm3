using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppRestartLib;
using Quartz;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs
{

    /// <summary>
    /// 重启任务
    /// </summary>
    public class AutoRestartJob : IJob
    {


        public void Execute(IJobExecutionContext context)
        {

            lock (Slocker)
            {
                if (_isRunning)
                {
                    return;
                }

                _isRunning = true;
                _isRestarting = true;
            }
            string jobName = "";
            try
            {
                jobName = GetType().Name;

                #region 延时重启

                Random random = new Random();
                //每次休眠时间
                const int sleepInterval = 100;
                int totalSleepMs = random.Next(11000, 19000);

                //i<1000是为了避免循环次数过大
                for (int i = 0; i * sleepInterval < totalSleepMs && i < 1000; i++)
                {
                    try
                    {
                        Thread.Sleep(sleepInterval);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                #endregion


                //重启
                RestartHelper.Restart(failAction: failReason =>
                {
                    NLogHelper.Error($"自动重启失败:{failReason?.Description},exception={failReason?.Exception}");
                }, successAction: () =>
                 {
                    //成功处理
                    // TODO

                    NLogHelper.Warn($"自动重启成功");
                 }, logAction: str =>
                 {
                     NLogHelper.Warn($"自动重启日志:{str}");
                 });


            }
            catch (Exception e)
            {
                NLogHelper.Error($"执行任务{jobName}失败:{e}");
            }
            finally
            {
                lock (Slocker)
                {
                    _isRunning = false;
                    _isRestarting = false;
                }
            }

        }




        #region 属性

        /// <summary>
        /// 是否正在运行
        /// </summary>
        private static volatile bool _isRunning = false;


        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Slocker = new object();

        /// <summary>
        /// 是否正在重启
        /// </summary>
        private static volatile bool _isRestarting = false;

        /// <summary>
        /// 是否正在重启
        /// </summary>
        public static bool IsRestarting
        {
            get
            {
                lock (Slocker)
                {
                    return _isRestarting;
                }
            }
        }


        #endregion


    }


}

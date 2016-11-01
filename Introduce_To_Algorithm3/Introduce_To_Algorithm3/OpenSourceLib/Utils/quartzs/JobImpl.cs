using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs
{
    /// <summary>
    /// 一个job实现
    /// </summary>
    public class JobImpl:IJob
    {
        #region IJob实现
        /// <summary>
        /// 注：该方法将定期按时执行，
        /// 意味着如果下一个周期到来，而上一次执行未完成，该方法开启一个新线程执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;//获取job的数据
            
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId+"   "+DateTime.Now.ToString());

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

            }
            catch (Exception ex)
            {
                Log4netHelper.Error("清理日志失败：" + ex);
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
        /// 每次job执行均创建一个job实例
        /// </summary>
        public JobImpl()
        {
            Console.WriteLine("创建一个job实例");
        }


        #region 辅助属性

       

        /// <summary>
        /// 锁
        /// </summary>
        private static Object locker = new Object();

        /// <summary>
        /// 回调函数是否正在执行
        /// </summary>
        private static bool _isRunning = false;

        #endregion

        #endregion
    }
}

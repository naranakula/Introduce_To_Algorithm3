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
    /// Each (and every) time the scheduler executes the job, it creates a new instance of the class before calling its Execute(..) method. 
    /// </summary>
    public class JobImpl:IJob
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
            JobDataMap dataMap = context.JobDetail.JobDataMap;//获取job的数据
            
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId+"   "+DateTime.Now);

            lock (_locker)
            {
                if (_isRunning)
                {
                    return;
                }
                _isRunning = true;
            }

            string jobName = GetType().Name;
            try
            {
                NLogHelper.Info($"开始执行任务{jobName}");
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

        /// <summary>
        /// 每次job执行均创建一个job实例
        /// 构造函数必须无参数
        /// </summary>
        public JobImpl()
        {
            //每次job执行均创建一个job实例
            Console.WriteLine("创建一个job实例");
        }


        #region 辅助属性
        
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly Object _locker = new Object();

        /// <summary>
        /// 回调函数是否正在执行
        /// </summary>
        private static bool _isRunning = false;

        #endregion

        #endregion

    }
}

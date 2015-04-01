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
        /// 意味着如果下一个周期到来，而上一次执行未完成，该方法仍然在一个新线程中执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId+"   "+DateTime.Now.ToString());
        }

        /// <summary>
        /// 每次job执行均创建一个job实例
        /// </summary>
        public JobImpl()
        {
            Console.WriteLine("创建一个job实例");
        }
        #endregion
    }
}

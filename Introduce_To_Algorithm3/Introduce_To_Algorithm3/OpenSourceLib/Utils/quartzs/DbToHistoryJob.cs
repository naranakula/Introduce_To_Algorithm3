using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs
{
    /// <summary>
    /// 数据转历史操作
    /// </summary>
    public class DbToHistoryJob:IJob
    {
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
                //如果数据库中表数据较多，不要一次性转历史，每次转1000条
                const int maxCountPerConvert = 100;//每次转历史的最大条数

            }
            catch (Exception ex)
            {
                Log4netHelper.Error("执行数据库转历史任务：" + ex);
            }
            finally
            {
                lock (locker)
                {
                    _isRunning = false;
                }
            }
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

    }
}

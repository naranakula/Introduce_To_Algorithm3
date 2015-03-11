using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// Quartz任务调度类
    /// </summary>
    public class QuartzHelper
    {

        #region 单例模式

        private static QuartzHelper _instance;

        private QuartzHelper()
        {
            _schedulerFactory = new StdSchedulerFactory();
        }

        /// <summary>
        /// 获取一个实例
        /// </summary>
        /// <returns></returns>
        public static QuartzHelper GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (_instance == null)
            {
                _instance = new QuartzHelper();
            }

            return _instance;
        }
        #endregion


        #region Private Member

        /// <summary>
        /// 调度器工厂
        /// </summary>
        private ISchedulerFactory _schedulerFactory;

        /// <summary>
        /// 调度器
        /// </summary>
        private IScheduler _scheduler;

        #endregion
    }
}

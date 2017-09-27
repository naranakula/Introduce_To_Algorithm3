using System;
using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs
{
    /// <summary>
    /// Quartz任务调度类
    /// 支持Quartz 2.x  不支持3.x
    /// Cron表达式含义查询网站：
    /// http://cronexpressiondescriptor.azurewebsites.net
    /// 
    /// 根据现实情况修改quartz.config中的配置文件中quartz最多的线程数
    /// 
    /// ScheduleJob然后在Start， 最后shutdown ,如果不调用ShutDown,进程不会退出
    /// </summary>
    public class QuartzHelper
    {

        #region 单例模式

        private static volatile QuartzHelper _instance;

        private QuartzHelper()
        {
            // 默认使用的是RAMJobStore，即信息保存在内存中
            //AdoJobStore 信息使用ADO.NET保存在数据库中 

            //配置可以使用quartz.config，也可以使用NameValueCollection 以下是默认配置
            NameValueCollection collection = new NameValueCollection();
            collection.Add("quartz.scheduler.instanceName", "DefaultQuartzScheduler");
            collection.Add("quartz.threadPool.threadCount", "10");
            collection.Add("quartz.jobStore.misfireThreshold", "60000");
            collection.Add("quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz");


            _schedulerFactory = new StdSchedulerFactory(collection);
            _scheduler = _schedulerFactory.GetScheduler();
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
        private volatile ISchedulerFactory _schedulerFactory;

        /// <summary>
        /// 调度器
        /// </summary>
        private volatile IScheduler _scheduler;

        #endregion

        #region Public Method

        /// <summary>
        /// 启动调度器
        /// </summary>
        public void Start()
        {
            _scheduler.Start();
            //_scheduler.StartDelayed(new TimeSpan(0,0,2));
        }

        /// <summary>
        /// Halts the Quartz.IScheduler's firing of Quartz.ITriggers, and cleans up all resources associated with the Scheduler.
        /// </summary>
        /// <param name="waitForJobsToComplete">if true the scheduler will not allow this method to return until all currently executing jobs have completed.</param>
        public void Shutdown(bool waitForJobsToComplete = false)
        {
            _scheduler.Shutdown(waitForJobsToComplete);
        }

        /// <summary>
        /// 获取scheduler
        /// </summary>
        /// <returns></returns>
        public IScheduler GetScheduler()
        {
            return _scheduler;
        }

        /// <summary>
        /// 获取指定名称的scheduler
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IScheduler GetScheduler(String name)
        {
            return _schedulerFactory.GetScheduler(name);
        }

        /// <summary>
        /// 获取所有的job
        /// </summary>
        /// <returns></returns>
        public Quartz.Collection.ISet<JobKey> GetAllJobs()
        {
            var matcher = GroupMatcher<JobKey>.AnyGroup();
            return _scheduler.GetJobKeys(matcher);
        }

        /// <summary>
        /// 获取job信息
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public IJobDetail GetJobByKey(string jobName)
        {
            return _scheduler.GetJobDetail(new JobKey(jobName));
        }

        /// <summary>
        /// 添加一个调度任务
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <param name="trigger"></param>
        public void ScheduleJob(IJobDetail jobDetail, ITrigger trigger)
        {
            _scheduler.ScheduleJob(jobDetail, trigger);
        }

        /// <summary>
        /// 创建一个job
        /// job将会被认为是非持久的，使用了默认的组
        /// </summary>
        /// <param name="jobName">job名字，必须唯一</param>
        /// <param name="job"></param>
        /// <param name="data"></param>
        public static  IJobDetail CreateJob(String jobName, IJob job, JobDataMap data = null)
        {
            /*
Durability - if a job is non - durable, it is automatically deleted from the scheduler once there are no longer any active triggers associated with it.In other words, non - durable jobs have a life span bounded by the existence of its triggers.
RequestsRecovery - if a job “requests recovery”, and it is executing during the time of a ‘hard shutdown’ of the scheduler(i.e.the process it is running within crashes, or the machine is shut off), then it is re - executed when the scheduler is started again.In this case, the JobExecutionContext.Recovering property will return true.*/
            //Job 不持久 ， 不要求恢复， 使用默认的SchedulerConstants.DefaultGroup
            //Set the property JobDetail.Durable = true - which instructs Quartz not to delete the Job when it becomes an “orphan” (when the Job not longer has a Trigger referencing it).
            // if set to true, job will request recovery.  automatically re-executed after a scheduler fails.
            IJobDetail jobDetail = new JobDetailImpl(jobName,null,job.GetType(),false,false);
            
            if (data != null)
            {
                foreach (var keyValuePair in data)
                {
                    jobDetail.JobDataMap.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            //添加job的关联数据
            return jobDetail;
        }

        /// <summary>
        /// 创建一个job
        /// job将会被认为是非长久的，使用了默认的组
        /// </summary>
        /// <param name="jobName">job名字，必须唯一</param>
        /// <param name="jobtype"></param>
        /// <param name="data"></param>
        public static IJobDetail CreateJob(String jobName, Type jobtype,JobDataMap data = null)
        {
            /*
Durability - if a job is non - durable, it is automatically deleted from the scheduler once there are no longer any active triggers associated with it.In other words, non - durable jobs have a life span bounded by the existence of its triggers.
RequestsRecovery - if a job “requests recovery”, and it is executing during the time of a ‘hard shutdown’ of the scheduler(i.e.the process it is running within crashes, or the machine is shut off), then it is re - executed when the scheduler is started again.In this case, the JobExecutionContext.Recovering property will return true.*/

           //Job 不持久 ， 不要求恢复， 使用默认的SchedulerConstants.DefaultGroup
           //Set the property JobDetail.Durable = true - which instructs Quartz not to delete the Job when it becomes an “orphan” (when the Job not longer has a Trigger referencing it).
           // if set to true, job will request recovery.   automatically re-executed after a scheduler fails.
           IJobDetail jobDetail = new JobDetailImpl(jobName, null, jobtype, false, false);
            if (data != null)
            {
                foreach (var keyValuePair in data)
                {
                    jobDetail.JobDataMap.Add(keyValuePair.Key,keyValuePair.Value);
                }
            }
            //设置其他信息
            //jobDetail.JobDataMap.Add();
            //添加job的关联数据
            return jobDetail;
        }

        /// <summary>
        /// 暂停一个job
        /// </summary>
        /// <param name="jobName"></param>
        public void Pause(String jobName)
        {
            _scheduler.PauseJob(new JobKey(jobName));
        }

        /// <summary>
        /// Resume一个Job
        /// </summary>
        /// <param name="jobName"></param>
        public void Resume(string jobName)
        {
            _scheduler.ResumeJob(new JobKey(jobName));
        }

        /// <summary>
        /// 删除一个job
        /// </summary>
        /// <param name="jobName"></param>
        public void Delete(string jobName)
        {
            _scheduler.DeleteJob(new JobKey(jobName));
        }

        /// <summary>
        /// 创建一个无限执行的触发器，放入默认的组
        /// trigger将定期按时执行，
        /// 意味着如果下一个周期到来，而上一次周期执行未完成，job仍然在一个新线程中执行
        /// </summary>
        /// <param name="triggerName">触发器的名字，必须唯一</param>
        /// <param name="offsetSeconds">多长时间后触发器执行 即第一次job执行  单位为秒,0表示立刻执行</param>
        /// <param name="periodSeconds">第一次之后每次触发器的执行周期，单位为秒,范围可以超过59</param>
        /// <returns></returns>
        public static ITrigger CreateSimpleTrigger(string triggerName,int offsetSeconds,int periodSeconds)
        {
            //ITrigger trigger = new SimpleTriggerImpl(triggerName,DateTimeOffset.UtcNow.AddSeconds(offsetSeconds),null,SimpleTriggerImpl.RepeatIndefinitely,new TimeSpan(0,0,0,periodSeconds));

            //return trigger;

            if (offsetSeconds <= 0)
            {
                ITrigger trigger =
                    TriggerBuilder.Create()
                        .WithIdentity(triggerName)
                        .StartNow()
                        .WithSimpleSchedule(builder => builder.WithIntervalInSeconds(periodSeconds).RepeatForever())
                        .Build();
                return trigger;
            }
            else
            {
                ITrigger trigger =
                TriggerBuilder.Create()
                    .WithIdentity(triggerName)
                    .StartAt(DateTimeOffset.UtcNow.AddSeconds(offsetSeconds))
                    .WithSimpleSchedule(builder => builder.WithIntervalInSeconds(periodSeconds).RepeatForever())
                    .Build();
                return trigger;
            }
        }

        /// <summary>
        /// 创建一个无限执行的触发器, 放入默认的组
        /// trigger将定期按时执行，
        /// 意味着如果下一个周期到来，而上一次周期执行未完成，job仍然在一个新线程中执行
        /// </summary>
        /// <param name="triggerName">触发器的名字，必须唯一</param>
        /// <param name="offsetSeconds">多长时间后触发器执行 即第一次job执行  单位为秒,0表示立刻执行</param>
        /// <param name="periodSpan">第一次之后每次触发器的执行周期</param>
        /// <returns></returns>
        public static ITrigger CreateSimpleTrigger(string triggerName, int offsetSeconds, TimeSpan periodSpan)
        {
            //ITrigger trigger = new SimpleTriggerImpl(triggerName, DateTimeOffset.UtcNow.AddSeconds(offsetSeconds), null, SimpleTriggerImpl.RepeatIndefinitely, periodSpan);

            if (offsetSeconds <= 0)
            {
                ITrigger trigger =
                    TriggerBuilder.Create()
                        .WithIdentity(triggerName)
                        .StartNow()
                        .WithSimpleSchedule(builder => builder.WithInterval(periodSpan).RepeatForever())
                        .Build();
                return trigger;
            }
            else
            {
                ITrigger trigger =
                TriggerBuilder.Create()
                    .WithIdentity(triggerName)
                    .StartAt(DateTimeOffset.UtcNow.AddSeconds(offsetSeconds))
                    .WithSimpleSchedule(builder => builder.WithInterval(periodSpan).RepeatForever())
                    .Build();
                return trigger;
            }
        }

        /// <summary>
        /// 创建一个无限执行的触发器, 放入默认的组
        /// trigger将定期按时执行，
        /// 意味着如果下一个周期到来，而上一次周期执行未完成，job仍然在一个新线程中执行
        /// </summary>
        /// <param name="triggerName">触发器的名字，必须唯一</param>
        /// <param name="offsetSpan">多长时间后触发器执行 即第一次job执行 经过测试</param>
        /// <param name="periodSpan">第一次之后每次 触发器的执行周期</param>
        /// <returns></returns>
        public static ITrigger CreateSimpleTrigger(string triggerName, TimeSpan offsetSpan, TimeSpan periodSpan)
        {
            ITrigger trigger = new SimpleTriggerImpl(triggerName, DateTimeOffset.UtcNow.Add(offsetSpan), null, SimpleTriggerImpl.RepeatIndefinitely, periodSpan);
            return trigger;
        }

        /// <summary>
        /// 创建CronTrigger，立刻执行
        /// trigger将定期按时执行，
        /// 意味着如果下一个周期到来，而上一次周期执行未完成，job仍然在一个新线程中执行
        /// 
        /// Cron表达式由6个(或者7个)空格分隔的时间元素组成，按顺序依次是  
        /// 元素               取值       特殊字符
        /// 秒                (0-59)      ,-*/
        /// 分钟              (0-59)      ,-*/
        /// 小时              (0-23)      ,-*/
        /// day of Month      (1-31）     ,-*?/LW   需要考虑月实际的天数
        /// 月                (1-12)      ,-*/
        /// day of week       (1-7)       ,-*?/L#   1表示星期天 7表示星期六
        /// 年  可选     empty,1970-2099  ,-*/
        /// 
        /// 
        /// *   表示选择所有可能的值，“*”在子表达式（秒）里表示每秒的含义
        /// ?   字符可以用在day-of-month及day-of-week域中，它用来表示“没有指定值”。这对于需要指定一个或者两个域的值而不需要对其他域进行设置来说相当有用。
        /// -   指定范围，如在小时域中，  10-12表示10,11,12点
        /// ,   指定额外的值，如在小时域中，  10,12,12表示10,12,13点
        /// /   指定增量，   在分钟域中，  5/15表示从5分钟开始每隔15分钟执行，即5 20,35,50分钟执行
        /// 
        /// 
        /// 
        /// 0 0 12 * * ?    表示每天12点执行
        /// 0/5 * * * * ?   表示每5秒执行一次
        /// 0 0/5 * * * ?   表示每5分钟执行一次
        /// 0 0 4 5 * ?     每月5日 4 点执行
        /// 0 17 4 ? * 1,3,5     每个星期天 星期二 星期四 4点 17分执行
        /// </summary>
        /// <param name="triggerName">triggerName,必须唯一</param>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        public static ITrigger CreateCronTrigger(String triggerName,String cronExpression)
        {
            ITrigger trigger = new CronTriggerImpl(triggerName,null,cronExpression);
            return trigger;
        }


        /// <summary>
        /// 创建CronTrigger
        /// trigger将定期按时执行，
        /// 意味着如果下一个周期到来，而上一次周期执行未完成，job仍然在一个新线程中执行
        /// 
        /// Cron表达式由6个(或者7个)空格分隔的时间元素组成，按顺序依次是  
        /// 元素               取值       特殊字符
        /// 秒                (0-59)      ,-*/
        /// 分钟              (0-59)      ,-*/
        /// 小时              (0-23)      ,-*/
        /// day of Month      (1-31）     ,-*?/LW   需要考虑月实际的天数
        /// 月                (1-12)      ,-*/
        /// day of week       (1-7)       ,-*?/L#   1表示星期天 7表示星期六
        /// 年  可选     empty,1970-2099  ,-*/
        /// 
        /// 
        /// *   表示选择所有可能的值，“*”在子表达式（秒）里表示每秒的含义
        /// ?   字符可以用在day-of-month及day-of-week域中，它用来表示“没有指定值”。这对于需要指定一个或者两个域的值而不需要对其他域进行设置来说相当有用。
        /// -   指定范围，如在小时域中，  10-12表示10,11,12点
        /// ,   指定额外的值，如在小时域中，  10,12,12表示10,12,13点
        /// /   指定增量，   在分钟域中，  5/15表示从5分钟开始每隔15分钟执行，即5 20,35,50分钟执行
        /// 
        /// 
        /// 
        /// 0 0 12 * * ?    表示每天12点执行
        /// 0/5 * * * * ?   表示每5秒执行一次
        /// 0 0/5 * * * ?   表示每5分钟执行一次
        /// 0 0 4 5 * ?     每月5日 4 点执行
        /// 0 0 4 ? * 1     每个星期天4点执行
        /// </summary>
        /// <param name="triggerName">triggerName,必须唯一</param>
        /// <param name="startTime">trigger起始执行的时间</param>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        public static ITrigger CreateCronTrigger(String triggerName,DateTimeOffset startTime, String cronExpression)
        {
            ITrigger trigger =
                TriggerBuilder.Create()
                    .StartAt(startTime)
                    .WithIdentity(triggerName)
                    .WithCronSchedule(cronExpression)
                    .Build();
            return trigger;
        }

        /// <summary>
        /// 创建CronTrigger
        /// trigger将定期按时执行，
        /// 意味着如果下一个周期到来，而上一次周期执行未完成，job仍然在一个新线程中执行
        /// 
        /// Cron表达式由6个(或者7个)空格分隔的时间元素组成，按顺序依次是  
        /// 元素               取值       特殊字符
        /// 秒                (0-59)      ,-*/
        /// 分钟              (0-59)      ,-*/
        /// 小时              (0-23)      ,-*/
        /// day of Month      (1-31）     ,-*?/LW   需要考虑月实际的天数
        /// 月                (1-12)      ,-*/
        /// day of week       (1-7)       ,-*?/L#   1表示星期天 7表示星期六
        /// 年  可选     empty,1970-2099  ,-*/
        /// 
        /// 
        /// *   表示选择所有可能的值，“*”在子表达式（秒）里表示每秒的含义
        /// ?   字符可以用在day-of-month及day-of-week域中，它用来表示“没有指定值”。这对于需要指定一个或者两个域的值而不需要对其他域进行设置来说相当有用。
        /// -   指定范围，如在小时域中，  10-12表示10,11,12点
        /// ,   指定额外的值，如在小时域中，  10,12,12表示10,12,13点
        /// /   指定增量，   在分钟域中，  5/15表示从5分钟开始每隔15分钟执行，即5 20,35,50分钟执行
        /// 
        /// 
        /// 
        /// 0 0 12 * * ?    表示每天12点执行
        /// 0/5 * * * * ?   表示每5秒执行一次
        /// 0 0/5 * * * ?   表示每5分钟执行一次
        /// 0 0 4 5 * ?     每月5日 4 点执行
        /// 0 0 4 ? * 1     每个星期天4点执行
        /// </summary>
        /// <param name="triggerName">triggerName,必须唯一</param>
        /// <param name="offsetSeconds">trigger起始执行的时间,以秒为单位</param>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        public static ITrigger CreateCronTrigger(String triggerName, int offsetSeconds, String cronExpression)
        {
            ITrigger trigger =
                TriggerBuilder.Create()
                    .StartAt(DateTimeOffset.UtcNow.AddSeconds(offsetSeconds))
                    .WithIdentity(triggerName)
                    .WithCronSchedule(cronExpression)
                    .Build();
            return trigger;
        }
        #endregion
    }


}

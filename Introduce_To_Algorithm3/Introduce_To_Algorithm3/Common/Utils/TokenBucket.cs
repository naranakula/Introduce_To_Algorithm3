using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 使用Token Bucket算法来做限流处理
    /// 使用Token Bucket 算法，每次接口调用消耗Bucket中一个Token，Bucket中Token有最大上限，每分钟定时向Bucket中添加若干个Token，最大不可超过上限。
    /// 该类的实例方法是多线程安全的
    /// </summary>
    public class TokenBucket
    {
        /// <summary>
        /// 每次评估的时间周期 单位毫秒  默认1分钟
        /// </summary>
        private readonly int estimatePeriodInMillisecond;

        /// <summary>
        /// 桶的最大容量
        /// </summary>
        private readonly int bucketLimit;

        /// <summary>
        /// 每次注水的量
        /// </summary>
        private readonly int appendStep;

        /// <summary>
        /// 锁
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// 当前桶的容量
        /// </summary>
        private int curBucketCount;

        /// <summary>
        /// 上一次注水时间
        /// </summary>
        private DateTime? lastAppendTime;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="estimatePeriodInMillisecond">每次评估的时间周期 单位毫秒  默认1分钟</param>
        /// <param name="bucketLimit">桶的最大容量 默认100，即每分钟可以执行最多100次</param>
        /// <param name="appendStep">每次注水的量，即接口平均每分钟执行40次</param>
        public TokenBucket(int estimatePeriodInMillisecond = 60*1000,int bucketLimit=100,int appendStep = 40)
        {
            this.estimatePeriodInMillisecond = estimatePeriodInMillisecond;
            this.bucketLimit = bucketLimit;
            this.appendStep = appendStep;
            curBucketCount = 0;
        }


        /// <summary>
        /// 限流运行action方法，返回是否运行action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool Run(Action action,Action<Exception> exceptionHandler = null)
        {

            bool canRunAction = false;//是否可以调用Action
            #region token bucket算法
            lock (locker)
            {
                DateTime now = DateTime.Now;
                //注水
                if(lastAppendTime==null || (now - lastAppendTime.Value).TotalMilliseconds > estimatePeriodInMillisecond)
                {
                    //需要注水
                    lastAppendTime = now;
                    //计算当前水量
                    int curCount = curBucketCount + appendStep;
                    if (curCount > bucketLimit)
                    {
                        curCount = bucketLimit;
                    }
                    curBucketCount = curCount;
                }

                //检测是否可以调用action
                if (curBucketCount > 0)
                {
                    curBucketCount--;
                    canRunAction = true;
                }
                else
                {
                    curBucketCount = 0;
                    canRunAction = false;
                }
            }
            #endregion


            if (canRunAction)
            {
                try
                {
                    if(action != null)
                    {
                        action();
                    }
                }
                catch(Exception ex)
                {
                    if (exceptionHandler != null)
                    {
                        exceptionHandler(ex);
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
            
        }

        
    }


}

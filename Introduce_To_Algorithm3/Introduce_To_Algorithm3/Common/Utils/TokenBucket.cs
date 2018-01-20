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
        private readonly int _estimatePeriodInMillisecond;

        /// <summary>
        /// 桶的最大容量
        /// </summary>
        private readonly int _bucketLimit;

        /// <summary>
        /// 每次注水的量
        /// </summary>
        private readonly int _appendStep;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// 当前桶的容量
        /// </summary>
        private int _curBucketCount;

        /// <summary>
        /// 上一次注水时间
        /// </summary>
        private DateTime? _lastAppendTime;

        #region 静态实例

        /// <summary>
        /// 静态锁
        /// </summary>
        private static readonly object sLocker = new object();


        private static TokenBucket _sTokenBucket = null;

        /// <summary>
        /// 一个线程安全的TokenBucket静态实例
        /// 每分钟增加10，最大30
        /// </summary>
        public static TokenBucket Instance
        {
            get
            {
                lock (sLocker)
                {
                    if (_sTokenBucket != null)
                    {
                        return _sTokenBucket;
                    }

                    _sTokenBucket = new TokenBucket(estimatePeriodInMillisecond:60000,bucketLimit:30,appendStep:10);
                    return _sTokenBucket;
                }
            }
        }


        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="estimatePeriodInMillisecond">每次评估的时间周期 单位毫秒  默认1分钟</param>
        /// <param name="bucketLimit">桶的最大容量 默认100，即每分钟可以执行最多60次</param>
        /// <param name="appendStep">每次注水的量，即接口平均每分钟执行20次</param>
        public TokenBucket(int estimatePeriodInMillisecond = 60*1000,int bucketLimit=60,int appendStep = 20)
        {
            this._estimatePeriodInMillisecond = estimatePeriodInMillisecond;
            this._bucketLimit = bucketLimit;
            this._appendStep = appendStep;
            _curBucketCount = 0;
        }


        /// <summary>
        /// 限流运行action方法，返回是否运行action
        /// </summary>
        /// <param name="action">action为null，什么也不做,只返回是否需要运行action</param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool Run(Action action,Action<Exception> exceptionHandler = null)
        {

            bool canRunAction = false;//是否可以调用Action

            #region token bucket算法
            lock (_locker)
            {
                DateTime now = DateTime.Now;
                //注水
                if(_lastAppendTime==null || (now - _lastAppendTime.Value).TotalMilliseconds > _estimatePeriodInMillisecond)
                {
                    //需要注水
                    _lastAppendTime = now;
                    //计算当前水量
                    int curCount = _curBucketCount + _appendStep;
                    if (curCount > _bucketLimit)
                    {
                        curCount = _bucketLimit;
                    }
                    _curBucketCount = curCount;
                }

                //检测是否可以调用action
                if (_curBucketCount > 0)
                {
                    _curBucketCount--;
                    canRunAction = true;
                }
                else
                {
                    _curBucketCount = 0;
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

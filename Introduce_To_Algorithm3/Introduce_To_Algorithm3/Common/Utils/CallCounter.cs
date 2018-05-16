using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 函数调用计数
    /// 
    /// 基本思想:统计一段时间内函数被调用的次数
    /// 
    /// </summary>
    public static class CallCounter
    {
        /// <summary>
        /// 最近调用次数
        /// </summary>
        private static volatile int _recentCount = 0;

        /// <summary>
        /// 上一次最近调用次数
        /// </summary>
        public static volatile int _previousCount = 0;

        /// <summary>
        /// 评估时间间隔 1分钟
        /// </summary>
        public const int AssessTimeInMillisecond = 61 * 1000;

        /// <summary>
        /// 上一次评估时间
        /// </summary>
        private static DateTime _lastAssessTime = DateTime.Now;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object SLocker = new object();

        /// <summary>
        /// int 溢出之前的检查
        /// </summary>
        private const int MaxIntBeforeStackOverflow = int.MaxValue - 10000000;

        /// <summary>
        /// 计数加1
        /// </summary>
        /// <param name="needLock">是否调用需要加锁</param>
        public static void Tick(bool needLock = false)
        {
            try
            {
                if (needLock)
                {
                    DateTime now = DateTime.Now;

                    lock (SLocker)
                    {
                        //long 可以认为不会溢出
                        _recentCount++;

                        //避免调整系统时间引起的bug
                        double diffTotalMs = (now - _lastAssessTime).TotalMilliseconds;

                        if (diffTotalMs > AssessTimeInMillisecond)
                        {
                            //重置当前次数
                            _lastAssessTime = now;
                            //将recent赋给prev，并将recent设为0
                            _previousCount = _recentCount;
                            _recentCount = 0;
                        }
                        else if (diffTotalMs < -1000)
                        {
                            _lastAssessTime = now;
                        }

                        //避免溢出
                        if (_recentCount > MaxIntBeforeStackOverflow)
                        {
                            //理论上永远不会溢出，仅仅是预防措施，尽量减少lock
                            _recentCount = int.MaxValue / 2;
                        }
                    }
                }
                else
                {
                    DateTime now = DateTime.Now;

                    //long 可以认为不会溢出
                    _recentCount++;

                    //避免调整系统时间引起的bug
                    double diffTotalMs = (now - _lastAssessTime).TotalMilliseconds;

                    if (diffTotalMs > AssessTimeInMillisecond)
                    {
                        //重置当前次数
                        _lastAssessTime = now;
                        //将recent赋给prev，并将recent设为0
                        _previousCount = _recentCount;
                        _recentCount = 0;
                    }
                    else if (diffTotalMs < -1000)
                    {
                        _lastAssessTime = now;
                    }

                    //避免溢出
                    if (_recentCount > MaxIntBeforeStackOverflow)
                    {
                        //理论上永远不会溢出，仅仅是预防措施，尽量减少lock
                        _recentCount = int.MaxValue / 2;
                    }
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// 获取最近调用次数
        /// </summary>
        /// <param name="needLock">是否需要加锁</param>
        /// <returns></returns>
        public static int RecentCount(bool needLock = false)
        {
            if (needLock)
            {
                lock (SLocker)
                {
                    return _previousCount;
                }
            }
            else
            {
                return _previousCount;
            }
        }




    }
}

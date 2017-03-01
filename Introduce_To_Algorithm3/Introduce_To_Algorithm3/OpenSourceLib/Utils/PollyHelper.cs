using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// 通过Nuget安装Polly Install-Package Polly
    /// polly allows developers to express transient exception handling policies such as Retry, Retry Forever, Wait and Retry or Circuit Breaker in a fluent manner.
    /// 下面每个函数都有异步的版本
    /// All Polly policies are fully thread-safe. You can safely re-use policies at multiple call sites, and execute through policies concurrently on different threads.
    /// Polly policies都是多线程安全的。Policies的ExecuteAndCapture不会抛出异常。而是返回PolicyResult。ExceptionType表示策略是否处理该异常 FinalException：最后的异常 OutcomeType：策略是否执行成功
    /// Polly的ExecuteAndCapture是不会抛出异常的，Execute会抛出
    /// 
    /// Polly的Execute是这样处理的：
    /// 抛出未处理异常，不会retry
    /// 抛出可处理异常，根据限定进行retry
    /// Each call to .Execute(…) (or similar) through a retry policy maintains its own private state. A retry policy can therefore be re-used safely in a multi-threaded environment.  The internal operation of the retry policy is thread-safe, but this does not magically make delegates you execute through the policy thread-safe: if the delegates you execute through the policy are not thread-safe, they remain not thread-safe.
    /// </summary>
    public static class PollyHelper
    {

        #region 静态构造函数

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static PollyHelper()
        {
            //Policy是多线程安全的，可以只维护一个实例
        }

        #endregion


        #region 重试一次 没有时间间隔 即最多运行两次(经过测试，的确最多执行2次)

        /// <summary>
        /// 重试一次没有时间间隔 不会抛出异常
        /// </summary>
        /// <param name="action">要同步执行的action</param>

        public static PolicyResult RetryOnce(Action action)
        {
            PolicyResult result = Policy.Handle<Exception>().Retry().ExecuteAndCapture(action);

            return result;
        }

        /// <summary>
        /// 重试一次没有时间间隔 返回func执行的结果
        /// </summary>
        /// <param name="func">要同步执行的func</param>

        public static PolicyResult<T> RetryOnce<T>(Func<T> func)
        {
            PolicyResult<T> result = Policy.Handle<Exception>().Retry().ExecuteAndCapture(func);

            return result;
        }

        #endregion

        #region 重试n次 没有时间间隔 最多执行n+1次（经过了测试）

        /// <summary>
        /// 重试多次没有时间间隔，返回true表示最终执行成功 false表示执行失败
        /// </summary>
        /// <param name="retryCount">大于等于0,表示重试的次数，实际最多执行retryCount+1次</param>
        /// <param name="action">需要执行的业务逻辑</param>
        /// <param name="retryCallback">每次重试前的回调</param>
        /// <returns></returns>
        public static PolicyResult Retry(Action action, int retryCount=1, Action<Exception, int> retryCallback = null)
        {
            PolicyResult result = Policy.Handle<Exception>().Retry(retryCount, ((exception, count) =>
            {
                /*
                 * 如果action没有抛出异常，此回调不执行，否则执行 count从1开始，在调用action之前执行 exception表示上次的异常
                 */
                retryCallback?.Invoke(exception, count);
            })).ExecuteAndCapture(action);

            return result;
        }

        /// <summary>
        /// 重试多次没有时间间隔 返回func执行的结果
        /// </summary>
        /// <param name="retryCount">大于等于0,表示重试的次数，实际最多执行retryCount+1次</param>
        /// <param name="func">需要执行的业务逻辑</param>
        /// <param name="retryCallback">每次重试前的回调</param>
        /// <returns>The result of executing the policy. Will be default(TResult) is the policy failed</returns>
        public static PolicyResult<T> Retry<T>(Func<T> func, int retryCount=1, Action<Exception, int> retryCallback = null)
        {
            PolicyResult<T> result = Policy.Handle<Exception>().Retry(retryCount, ((exception, count) =>
            {
                /*
                 * 如果func没有抛出异常，此回调不执行，否则执行 count从1开始，在调用func之前执行 exception表示上次的异常
                 * 
                 */
                retryCallback?.Invoke(exception, count);
            })).ExecuteAndCapture(func);

            return result;
        }

        #endregion
        
        #region Retry forever

        /// <summary>
        /// 无限重试,直至action执行成功
        /// </summary>
        /// <param name="action">实际的业务逻辑</param>
        /// <param name="retryCallback">每次重试之前的回调</param>
        /// <returns></returns>
        public static PolicyResult RetryForver(Action action,Action<Exception> retryCallback = null)
        {
            PolicyResult result =  Policy.Handle<Exception>().RetryForever(exception =>
            {
                //重试之前的回调
                retryCallback?.Invoke(exception);
            }).ExecuteAndCapture(action);

            return result;
        }

        /// <summary>
        /// 无限重试,直至func执行成功
        /// </summary>
        /// <param name="func">实际的业务逻辑</param>
        /// <param name="retryCallback">每次重试之前的回调</param>
        /// <returns></returns>
        public static PolicyResult<T> RetryForver<T>(Func<T> func, Action<Exception> retryCallback = null)
        {
            PolicyResult<T> result = Policy.Handle<Exception>().RetryForever(exception =>
            {
                //重试之前的回调
                retryCallback?.Invoke(exception);
            }).ExecuteAndCapture(func);

            return result;
        }

        #endregion

        #region Retry and Wait

        /// <summary>
        /// 重试并且每次重试之前等待
        /// </summary>
        /// <param name="action">要执行的业务逻辑</param>
        /// <param name="retryCount">重试的次数，总的执行次数=重试次数+1</param>
        /// <param name="sleepDurationProvider">每次重试之前的休眠时间提供者，如果为null，使用默认的的指数实现 1，2，4</param>
        /// <param name="retryCallback">每次重试之前的回调</param>
        /// <returns></returns>
        public static PolicyResult RetryAndWait(Action action, int retryCount = 2,
            Func<int, TimeSpan> sleepDurationProvider = null, Action<Exception, TimeSpan, int> retryCallback = null)
        {
            PolicyResult result = Policy.Handle<Exception>().WaitAndRetry(retryCount, currentCount =>
            {
                if (sleepDurationProvider != null)
                {
                    return sleepDurationProvider(currentCount);
                }
                else
                {
                    const int MaxPow = 7;//2^6 =64s 2^7=128
                    //i从1开始计数
                    double secondsToSleep = Math.Pow(2, currentCount - 1 > MaxPow ? MaxPow : currentCount - 1);//避免overflow

                    return TimeSpan.FromSeconds(secondsToSleep);
                }
            }, (exception, span, count, context) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔 count表示重试次数，从1开始计数
                retryCallback?.Invoke(exception, span, count);
            }).ExecuteAndCapture(action);
            return result;
        }

        /// <summary>
        /// 重试并且每次重试之前等待
        /// </summary>
        /// <param name="func">要执行的业务逻辑</param>
        /// <param name="retryCount">重试的次数，总的执行次数=重试次数+1</param>
        /// <param name="sleepDurationProvider">每次重试之前的休眠时间提供者，如果为null，使用默认的的指数实现1，2，4</param>
        /// <param name="retryCallback">每次重试之前的回调</param>
        /// <returns></returns>
        public static PolicyResult<T> RetryAndWait<T>(Func<T> func, int retryCount = 2,
            Func<int, TimeSpan> sleepDurationProvider = null, Action<Exception, TimeSpan, int> retryCallback = null)
        {

            PolicyResult<T> result = Policy.Handle<Exception>().WaitAndRetry(retryCount, currentCount =>
            {
                if (sleepDurationProvider != null)
                {
                    return sleepDurationProvider(currentCount);
                }
                else
                {
                    const int MaxPow = 7;//2^6 =64s 2^7=128
                    //i从1开始计数
                    double secondsToSleep = Math.Pow(2, currentCount - 1 > MaxPow ? MaxPow : currentCount - 1);//避免overflow

                    return TimeSpan.FromSeconds(secondsToSleep);
                }
            }, (exception, span, count, context) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔 count表示重试次数，从1开始计数
                retryCallback?.Invoke(exception, span, count);
            }).ExecuteAndCapture(func);
            return result;
        }


        /// <summary>
        /// 重试次数和sleepDurations的个数相同，总的执行次数为重试次数+1
        /// </summary>
        /// <param name="sleepDurations">每次重试之前的间隔，不能为null，建议使用指数形式如1，2，4，8</param>
        /// <param name="action">要执行的业务逻辑action</param>
        /// <param name="retryCallback"></param>
        /// <returns>执行成功，返回true，否则返回false</returns>
        public static PolicyResult RetryAndWaitImplicitly(Action action, IEnumerable<TimeSpan> sleepDurations,Action<Exception,TimeSpan,int> retryCallback = null)
        {
            PolicyResult result = Policy.Handle<Exception>().WaitAndRetry(sleepDurations, (exception, span,count,context) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔 count表示重试次数，从1开始计数
                retryCallback?.Invoke(exception,span,count);
            }).ExecuteAndCapture(action);

            return result;
        }


        /// <summary>
        /// 重试次数和sleepDurations的个数相同 总的执行次数为重试次数+1
        /// </summary>
        /// <param name="sleepDurations">每次重试之前的间隔 不能为null</param>
        /// <param name="func">要执行的业务逻辑func</param>
        /// <returns>The result of executing the policy. Will be default(TResult) is the policy failed</returns>
        public static PolicyResult<T> RetryAndWaitImplicitly<T>(Func<T> func,IEnumerable<TimeSpan> sleepDurations, Action<Exception, TimeSpan, int> retryCallback = null)
        {
            PolicyResult<T> result = Policy.Handle<Exception>().WaitAndRetry(sleepDurations, (exception, span, count, context) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔 count表示重试次数，从1开始计数
                retryCallback?.Invoke(exception, span, count);
            }).ExecuteAndCapture(func);

            return result;
        }

        #endregion

        #region WaitAndRetryForever

        /// <summary>
        /// 无限重试带等待
        /// </summary>
        /// <param name="action">执行的业务逻辑</param>
        /// <param name="sleepDurationProvider">每次重试之前的等待时间</param>
        /// <param name="retryCallback">每次重试之前的回调</param>
        /// <returns></returns>
        public static PolicyResult RetryAndWaitForever(Action action,Func<int, TimeSpan> sleepDurationProvider = null,Action < Exception, TimeSpan> retryCallback=null)
        {
            PolicyResult result = Policy.Handle<Exception>().WaitAndRetryForever(currentCount =>
            {
                //提供重试的时间间隔 (currentCount表示重试次数,从1开始计数
                if (sleepDurationProvider != null)
                {
                    return sleepDurationProvider(currentCount);
                }
                else
                {
                    const int MaxPow = 7;//2^6 =64s 2^7=128
                    //i从1开始计数
                    double secondsToSleep = Math.Pow(2, currentCount - 1 > MaxPow ? MaxPow : currentCount - 1);//避免overflow

                    return TimeSpan.FromSeconds(secondsToSleep);
                }
            }, (exception, span) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔
                retryCallback?.Invoke(exception,span);
            }).ExecuteAndCapture(action);

            return result;
        }

        /// <summary>
        /// 无限重试带等待
        /// </summary>
        /// <param name="func"></param>
        /// <param name="sleepDurationProvider">每次重试之前的等待时间</param>
        /// <param name="retryCallback">每次重试之前的回调</param>
        public static PolicyResult<T> RetryAndWaitForever<T>(Func<T> func, Func<int, TimeSpan> sleepDurationProvider = null, Action<Exception, TimeSpan> retryCallback = null)
        {
            PolicyResult<T> result = Policy.Handle<Exception>().WaitAndRetryForever(currentCount =>
            {
                //提供重试的时间间隔 (currentCount表示重试次数,从1开始计数
                if (sleepDurationProvider != null)
                {
                    return sleepDurationProvider(currentCount);
                }
                else
                {
                    const int MaxPow = 7;//2^6 =64s 2^7=128
                    //i从1开始计数
                    double secondsToSleep = Math.Pow(2, currentCount - 1 > MaxPow ? MaxPow : currentCount - 1);//避免overflow

                    return TimeSpan.FromSeconds(secondsToSleep);
                }
            }, (exception, span) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔
                //在每次重试之前执行，span表示当前重试的时间间隔
                retryCallback?.Invoke(exception, span);
            }).ExecuteAndCapture(func);

            return result;
        }

        #endregion

        #region Circuit Breaker 断路器

        /**
         * 断路器有三种状态，Close Open HalfOpen
         * 断路器初始状态Closed, 如果连续发生指定次数的错误，断路器进入Open状态，经过指定的durationOfBreak（此段时间是failfast的），进入HalfOpen状态，下一次执行如果成功，进入Closed状态，执行失败，进入Open状态
         * 
         * 断路器只执行1次，没有重试功能
         * 
         * 断路器在多次Execute执行间维护状态，断路器是多线程安全的，内部使用锁来保证状态，但是action的执行不是在锁中的。锁仅仅是维护内部状态的。
         * 
         */
        /// <summary>
        /// 内部维护的断路器
        /// </summary>
        private static volatile CircuitBreakerPolicy _circuitBreaker = null;

        //断路器在制定数量的错误以后，断路一段时间后重启
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CircuitBreakerPolicy CircuitBreaker()
        {
            CircuitBreakerPolicy policy = Policy.Handle<Exception>().CircuitBreaker(10,new TimeSpan(0,0,60),(exception, span) => {
                //onbreak  断路时执行
            },() => {
                //onreset  闭路之后执行
            },() =>
            {
                //onhalfopen The action to call when the circuit transitions to Polly.CircuitBreaker.CircuitState.HalfOpen state, ready to try action executions again.
            });

            //policy.ExecuteAndCapture();//执行action或func
            
            return policy;

/*
CircuitState.Closed - Normal operation. Execution of actions allowed.
CircuitState.Open - The automated controller has opened the circuit. Execution of actions blocked.
CircuitState.HalfOpen - Recovering from open state, after the automated break duration has expired. Execution of actions permitted. Success of subsequent action/s controls onward transition to Open or Closed state.
CircuitState.Isolated - Circuit held manually in an open state. Execution of actions blocked.
*/
        }

        /// <summary>
        /// 断路器
        /// </summary>
        /// <returns></returns>
        public static CircuitBreakerPolicy AdvancedCircuitBreaker()
        {
            CircuitBreakerPolicy policy = Policy.Handle<Exception>().AdvancedCircuitBreaker(
         0.5, // Break on >=50% actions result in handled exceptions...失败比例达到上限时断路
        TimeSpan.FromSeconds(10), // ... over any 10 second period 统计失败比例的时间段
        8, // ... provided at least 8 actions in the 10 second period. 统计时间段内需执行最少的action，才会认为该时间段有效
         TimeSpan.FromSeconds(30), // Break for 30 seconds. 断路的时间，之后电路重置
        (exception, span) =>
        {
            //onbreak  断路时执行
        }, () =>
        {
            //onreset  闭路之后执行
        }, () =>
        {
            //onhalfopen The action to call when the circuit transitions to Polly.CircuitBreaker.CircuitState.HalfOpen state, ready to try action executions again.
        }
                );

            //policy.ExecuteAndCapture();//执行action或func
            return policy;
        }

        #endregion

    }
}

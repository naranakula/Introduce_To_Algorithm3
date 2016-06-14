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
    /// polly allows developers to express transient exception handling policies such as Retry, Retry Forever, Wait and Retry or Circuit Breaker in a fluent manner.
    /// 下面每个函数都有异步的版本
    /// </summary>
    public static class PollyHelper
    {

        #region Retry

        #region 重试一次 即最多运行两次

        /// <summary>
        /// 重试一次没有时间间隔 返回true表示最终执行成功 false表示执行失败
        /// </summary>
        /// <param name="action">要同步执行的action</param>
        
        public static bool RetryOnce(Action action)
        {
            PolicyResult result = Policy.Handle<Exception>().Retry().ExecuteAndCapture(action);

            if (result.Outcome == OutcomeType.Successful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 重试一次没有时间间隔 返回func执行的结果
        /// </summary>
        /// <param name="func">要同步执行的func</param>

        public static T RetryOnce<T>(Func<T> func)
        {
            PolicyResult<T> result = Policy.Handle<Exception>().Retry().ExecuteAndCapture(func);

            if (result.Outcome == OutcomeType.Successful)
            {
                return result.Result;
            }
            else
            {
                return default(T);
            }
        }

        #endregion

        #region 重试n次

        /// <summary>
        /// 重试多次没有时间间隔，返回true表示最终执行成功 false表示执行失败
        /// </summary>
        /// <param name="retryCount">大于等于0,表示重试的次数，实际最多执行retryCount+1次</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool Retry(int retryCount, Action action)
        {
            PolicyResult result = Policy.Handle<Exception>().Retry(retryCount, ((exception, count) =>
            {
                /*
                 * 如果action没有抛出异常，此回调不执行，否则执行 count从1开始，在调用action之前执行 exception表示上次的异常
                 */
            })).ExecuteAndCapture(action);

            if (result.Outcome == OutcomeType.Successful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 重试多次没有时间间隔 返回func执行的结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tryCount"></param>
        /// <param name="func"></param>
        /// <returns>The result of executing the policy. Will be default(TResult) is the policy failed</returns>
        public static T Retry<T>(int tryCount, Func<T> func)
        {
            PolicyResult<T> result = Policy.Handle<Exception>().Retry(tryCount, ((exception, count) =>
            {
                /*
                 * 如果func没有抛出异常，此回调不执行，否则执行 count从1开始，在调用func之前执行 exception表示上次的异常
                 * 
                 */
            })).ExecuteAndCapture(func);

            if (result.Outcome == OutcomeType.Successful)
            {
                return result.Result;
            }
            else
            {
                return default(T);
            }
        }

        #endregion

        #endregion

        #region Retry forever

        /// <summary>
        /// 无限重试,直至action执行成功
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool RetryForver(Action action)
        {
            PolicyResult result =  Policy.Handle<Exception>().RetryForever(exception =>
            {
                //重试之前的回调
            }).ExecuteAndCapture(action);

            if (result.Outcome == OutcomeType.Successful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 无限重试,直至func执行成功
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T RetryForver<T>(Func<T> func)
        {
            PolicyResult<T> result = Policy.Handle<Exception>().RetryForever(exception =>
            {
                //重试之前的回调
            }).ExecuteAndCapture(func);

            if (result.Outcome == OutcomeType.Successful)
            {
                return result.Result;
            }
            else
            {
                return default(T);
            }
        }

        #endregion

        #region Retry and Wait

        /// <summary>
        /// 重试次数和sleepDurations的个数相同
        /// </summary>
        /// <param name="sleepDurations">每次重试之前的间隔</param>
        /// <param name="action">要执行的action</param>
        /// <returns>执行成功，返回true，否则返回false</returns>
        public static bool WaitAnRetry(IEnumerable<TimeSpan> sleepDurations,Action action)
        {
            PolicyResult result = Policy.Handle<Exception>().WaitAndRetry(sleepDurations, (exception, span) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔
            }).ExecuteAndCapture(action);

            return result.Outcome == OutcomeType.Successful;
        }


        /// <summary>
        /// 重试次数和sleepDurations的个数相同
        /// </summary>
        /// <param name="sleepDurations">每次重试之前的间隔</param>
        /// <param name="func">要执行的func</param>
        /// <returns>The result of executing the policy. Will be default(TResult) is the policy failed</returns>
        public static T WaitAnRetry<T>(IEnumerable<TimeSpan> sleepDurations, Func<T> func)
        {
            PolicyResult<T> result = Policy.Handle<Exception>().WaitAndRetry(sleepDurations, (exception, span) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔
            }).ExecuteAndCapture(func);

            return result.Result;
        }

        #endregion

        #region WaitAndRetryForever

        /// <summary>
        /// 无限重试带等待
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool WaitAndRetryForever(Action action)
        {
            PolicyResult result = Policy.Handle<Exception>().WaitAndRetryForever(retryAttempt =>
            {
                //提供重试的时间间隔 retryAttempt表示重试次数,从1开始计数
                int seconds = retryAttempt*2 ;
                if (seconds > 100)
                {
                    seconds = 100;
                    //最多等待100s
                }
                return new TimeSpan(0, 0, 0, seconds, 0);
            }, (exception, span) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔
            }).ExecuteAndCapture(action);

            return result.Outcome == OutcomeType.Successful;
        }

        /// <summary>
        /// 无限重试带等待
        /// </summary>
        /// <param name="func"></param>
        /// <returns>The result of executing the policy. Will be default(TResult) is the policy failed</returns>
        public static T WaitAndRetryForever<T>(Func<T> func)
        {
            PolicyResult<T> result = Policy.Handle<Exception>().WaitAndRetryForever(retryAttempt =>
            {
                //提供重试的时间间隔 retryAttempt表示重试次数,从1开始计数
                int seconds = retryAttempt * 2;
                if (seconds > 100)
                {
                    seconds = 100;
                    //最多等待100s
                }
                return new TimeSpan(0, 0, 0, seconds, 0);
            }, (exception, span) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔
            }).ExecuteAndCapture(func);

            return result.Result;
        }

        #endregion

        #region Circuit Breaker 断路器

        //断路器在制定数量的错误以后，断路一段时间后重启

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

            
            return policy;
        }

        #endregion

    }
}

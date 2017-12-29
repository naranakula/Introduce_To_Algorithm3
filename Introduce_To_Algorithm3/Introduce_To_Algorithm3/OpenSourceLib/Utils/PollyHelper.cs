using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.NoOp;
using Polly.Timeout;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// 通过Nuget安装Polly Install-Package Polly
    /// 策略均可以考虑重用
    /// polly allows developers to express transient exception handling policies such as Retry, Retry Forever, Wait and Retry or Circuit Breaker in a fluent manner.
    /// 下面每个函数都有异步的版本
    /// All Polly policies are fully thread-safe. You can safely re-use policies at multiple call sites, and execute through policies concurrently on different threads.
    /// Polly policies都是多线程安全的。Policies的ExecuteAndCapture不会抛出异常。而是返回PolicyResult。ExceptionType表示策略是否处理该异常 FinalException：最后的异常 OutcomeType：策略是否执行成功
    /// Polly的ExecuteAndCapture是不会抛出异常的，Execute会抛出
    /// 
    /// Polly的Execute是这样处理的：
    /// 抛出未处理异常，不会retry
    /// 抛出可处理异常，根据限定进行retry
    /// 
    /// Execute是会抛出异常的  ExecuteAndCapture不抛出异常
    /// Each call to .Execute(…) (or similar) through a retry policy maintains its own private state. A retry policy can therefore be re-used safely in a multi-threaded environment.  The internal operation of the retry policy is thread-safe, but this does not magically make delegates you execute through the policy thread-safe: if the delegates you execute through the policy are not thread-safe, they remain not thread-safe.
    /// </summary>
    public static class PollyHelper
    {

        #region 安全执行一次 NoOp 没有策略 仅仅执行业务逻辑

        /// <summary>
        /// 该策略是多线程安全的
        /// 策略均可以考虑重用
        /// </summary>
        private static readonly NoOpPolicy noOpPolicy = Policy.NoOp();

        /// <summary>
        /// 安全的执行业务逻辑
        /// 该方法不会抛出异常，只执行一次
        /// </summary>
        /// <param name="action">执行的业务逻辑</param>
        /// <returns></returns>
        public static PolicyResult RunSafe(Action action)
        {
            //retryCount必须大于等于0  策略可以考虑重用
            return Policy.Handle<Exception>().Retry(0).ExecuteAndCapture(action);
            //下面是等价的
            //return Policy.NoOp().ExecuteAndCapture(action);
        }

        /// <summary>
        /// 执行业务逻辑
        /// 该方法不会抛出异常，只执行一次
        /// </summary>
        /// <param name="func">执行的业务逻辑</param>
        /// <returns></returns>
        public static PolicyResult<T> RunSafe<T>(Func<T> func)
        {
            //retryCount必须大于等于0
            return Policy.Handle<Exception>().Retry(0).ExecuteAndCapture(func);
            //return Policy.NoOp().ExecuteAndCapture(func);
        }

        #endregion

        #region 重试

        #region 重试一次 没有时间间隔 即最多运行两次(经过测试，的确最多执行2次)

        /// <summary>
        /// 重试一次没有时间间隔 不会抛出异常
        /// </summary>
        /// <param name="action">要同步执行的action</param>

        public static PolicyResult RetryOnce(Action action)
        {
            //Retry重试一次
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
        public static PolicyResult Retry(Action action, int retryCount = 1, Action<Exception, int> retryCallback = null)
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
        public static PolicyResult<T> Retry<T>(Func<T> func, int retryCount = 1, Action<Exception, int> retryCallback = null)
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
        public static PolicyResult RetryForver(Action action, Action<Exception> retryCallback = null)
        {
            PolicyResult result = Policy.Handle<Exception>().RetryForever(exception =>
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
        public static PolicyResult RetryAndWait(Action action, int retryCount = 1,
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
                    const int maxPow = 7;//2^6 =64s 2^7=128
                    //currentCount从1开始计数
                    double secondsToSleep = Math.Pow(2, currentCount - 1 > maxPow ? maxPow : currentCount - 1);//避免overflow

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
        public static PolicyResult<T> RetryAndWait<T>(Func<T> func, int retryCount = 1,
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
                    const int maxPow = 7;//2^6 =64s 2^7=128
                    //i从1开始计数
                    double secondsToSleep = Math.Pow(2, currentCount - 1 > maxPow ? maxPow : currentCount - 1);//避免overflow

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
        public static PolicyResult RetryAndWaitImplicitly(Action action, IEnumerable<TimeSpan> sleepDurations, Action<Exception, TimeSpan, int> retryCallback = null)
        {
            PolicyResult result = Policy.Handle<Exception>().WaitAndRetry(sleepDurations, (exception, span, count, context) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔 count表示重试次数，从1开始计数
                retryCallback?.Invoke(exception, span, count);
            }).ExecuteAndCapture(action);

            return result;
        }


        /// <summary>
        /// 重试次数和sleepDurations的个数相同 总的执行次数为重试次数+1
        /// </summary>
        /// <param name="sleepDurations">每次重试之前的间隔 不能为null</param>
        /// <param name="func">要执行的业务逻辑func</param>
        /// <param name="retryCallback"></param>
        /// <returns>The result of executing the policy. Will be default(TResult) is the policy failed</returns>
        public static PolicyResult<T> RetryAndWaitImplicitly<T>(Func<T> func, IEnumerable<TimeSpan> sleepDurations, Action<Exception, TimeSpan, int> retryCallback = null)
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
        public static PolicyResult RetryAndWaitForever(Action action, Func<int, TimeSpan> sleepDurationProvider = null, Action<Exception, TimeSpan> retryCallback = null)
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
                    const int maxPow = 7;//2^6 =64s 2^7=128
                    //i从1开始计数
                    double secondsToSleep = Math.Pow(2, currentCount - 1 > maxPow ? maxPow : currentCount - 1);//避免overflow

                    return TimeSpan.FromSeconds(secondsToSleep);
                }
            }, (exception, span) =>
            {
                //在每次重试之前执行，span表示当前重试的时间间隔
                retryCallback?.Invoke(exception, span);
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
                    const int maxPow = 7;//2^6 =64s 2^7=128
                    //i从1开始计数
                    double secondsToSleep = Math.Pow(2, currentCount - 1 > maxPow ? maxPow : currentCount - 1);//避免overflow

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

        #endregion

        #region 连续多次错误后failfast一段时间 Circuit Breaker 断路器

        #region 普通断路器

        /*
         * 断路器有三种状态，Close Open HalfOpen
         * 断路器初始状态Closed, 如果连续发生指定次数的错误，断路器进入Open状态，经过指定的durationOfBreak（此段时间是failfast的），进入HalfOpen状态，下一次执行如果成功，进入Closed状态，执行失败，进入Open状态
         * 
         * 断路器只执行1次，没有重试功能
         * 
         * 断路器在多次Execute执行间维护状态，断路器是多线程安全的，内部使用锁来保证状态，但是action的执行不是在锁中的。锁仅仅是维护内部状态的。
         * 
         */

        /// <summary>
        /// 断路器类
        /// 多线程支持
        /// 根据连续异常次数判断是否断路
        /// </summary>
        public class CircuitBreakerEx
        {
            /// <summary>
            /// 底层的断路器
            /// </summary>
            private readonly CircuitBreakerPolicy _circuitBreaker = null;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="consecutiveExceptionsAllowedBeforeBreaking">在多少连续异常后断路器断开</param>
            /// <param name="durationOfBreak">断路器断开的时间间隔，之后进入HalfOpen状态</param>
            /// <param name="onBreak"></param>
            /// <param name="onReset"></param>
            /// <param name="onHalfOpen"></param>
            public CircuitBreakerEx(TimeSpan durationOfBreak, int consecutiveExceptionsAllowedBeforeBreaking = 5, Action<Exception, TimeSpan> onBreak = null, Action onReset = null, Action onHalfOpen = null)
            {
                //初始化断路器
                _circuitBreaker = Policy.Handle<Exception>()
                    .CircuitBreaker(consecutiveExceptionsAllowedBeforeBreaking, durationOfBreak,
                        (exception, timeSpan) =>
                        {
                            //onbreak  断路时执行  timespan:短路时间
                            onBreak?.Invoke(exception, timeSpan);
                        }, () =>
                        {
                            //  //onreset  闭路之后执行 即正常工作时执行
                            onReset?.Invoke();
                        }, () =>
                        {
                            //onhalfopen The action to call when the circuit transitions to Polly.CircuitBreaker.CircuitState.HalfOpen state, ready to try action executions again.
                            onHalfOpen?.Invoke();
                        });
            }

            /// <summary>
            /// 获取断路器状态
            /// </summary>
            public CircuitState CircuitState { get { return _circuitBreaker.CircuitState; } }

            /// <summary>
            /// 同步执行并返回结果
            /// </summary>
            /// <param name="action">要执行的业务逻辑</param>
            /// <returns></returns>
            public PolicyResult ExecuteAndCapture(Action action)
            {
                return _circuitBreaker.ExecuteAndCapture(action);
            }

            /// <summary>
            /// 同步执行并返回结果
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="func"></param>
            /// <returns></returns>
            public PolicyResult<T> ExecuteAndCapture<T>(Func<T> func)
            {
                return _circuitBreaker.ExecuteAndCapture(func);
            }

        }

        #endregion

        #region 先进断路器  在采样时间内错误超过一定比例(在样本超过一定次数的统计情况下)

        //在采样时间内错误超过一定比例(在样本超过一定次数的统计情况下)
        //     The circuit will break if, within any timeslice of duration samplingDuration,
        //     the proportion of actions resulting in a handled exception exceeds failureThreshold,
        //     provided also that the number of actions through the circuit in the timeslice
        //     is at least minimumThroughput.
        //     The circuit will stay broken for the durationOfBreak. Any attempt to execute
        //     this policy while the circuit is broken, will immediately throw a Polly.CircuitBreaker.BrokenCircuitException
        //     containing the exception that broke the circuit.
        //     If the first action after the break duration period results in a handled exception,
        //     the circuit will break again for another durationOfBreak; if no exception is
        //     thrown, the circuit will reset.


        /// <summary>
        /// 先进断路器类
        /// 多线程支持
        /// 根据时间切片内调用失败的比例判断是否需要断路
        /// </summary>
        public class AdvancedCircuitBreakerEx
        {
            /// <summary>
            /// 底层的断路器
            /// </summary>
            private readonly CircuitBreakerPolicy _circuitBreaker = null;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="failureThreshold">调用失败的比例 取值[0，1] 如0.3表示30%的调用失败 </param>
            /// <param name="samplingDuration">采样时间</param>
            /// <param name="minimumThroughput"> 统计时间段内需执行最少的action，才会认为该时间段有效</param>
            /// <param name="durationOfBreak">断路时间，之后的调用如果成功，断路器重置，如果失败，重新进入断路状态</param>
            /// <param name="onBreak">onbreak  断路时执行</param>
            /// <param name="onReset">onreset  闭路之后执行</param>
            /// <param name="onHalfOpen">断路时间过后，进入HalfOpen状态，之后的调用如果成功，断路器重置，如果失败，重新进入断路状态</param>
            public AdvancedCircuitBreakerEx(double failureThreshold, TimeSpan samplingDuration, int minimumThroughput,
                TimeSpan durationOfBreak, Action<Exception, TimeSpan> onBreak = null, Action onReset = null, Action onHalfOpen = null)
            {
                _circuitBreaker = Policy.Handle<Exception>()
                    .AdvancedCircuitBreaker(failureThreshold, samplingDuration, minimumThroughput, durationOfBreak,
                        (exception, timeSpan) =>
                        {
                            //onbreak  断路时执行 
                            onBreak?.Invoke(exception, timeSpan);
                        }, () =>
                        {
                            //  //onreset  闭路之后执行 即正常工作时执行
                            onReset?.Invoke();
                        }, () =>
                        {
                            //onhalfopen The action to call when the circuit transitions to Polly.CircuitBreaker.CircuitState.HalfOpen state, ready to try action executions again.
                            onHalfOpen?.Invoke();
                        });
            }
            
            /// <summary>
            /// 获取断路器状态
            /// </summary>
            public CircuitState CircuitState { get { return _circuitBreaker.CircuitState; } }

            /// <summary>
            /// 同步执行并返回结果
            /// </summary>
            /// <param name="action">要执行的业务逻辑</param>
            /// <returns></returns>
            public PolicyResult ExecuteAndCapture(Action action)
            {
                return _circuitBreaker.ExecuteAndCapture(action);
            }

            /// <summary>
            /// 同步执行并返回结果
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="func"></param>
            /// <returns></returns>
            public PolicyResult<T> ExecuteAndCapture<T>(Func<T> func)
            {
                return _circuitBreaker.ExecuteAndCapture(func);
            }

        }

        #endregion

        #endregion
        
        #region Fallback 回退策略 如果执行失败，执行替代action或者返回替代结果

        //FallbackPolicy是多线程安全的和可重用的

        /// <summary>
        /// FallbackPolicy是多线程安全的和可重用的
        /// </summary>
        /// <param name="action">要执行的业务逻辑</param>
        /// <param name="fallbackAction">业务逻辑执行失败后，执行的挽救代码</param>
        /// <returns></returns>
        public static PolicyResult Fallback(Action action, Action fallbackAction = null)
        {
            FallbackPolicy policy = Policy.Handle<Exception>().Fallback(() =>
            {
                fallbackAction?.Invoke();
            });

            return policy.ExecuteAndCapture(action);
        }


        /// <summary>
        /// FallbackPolicy是多线程安全的和可重用的
        /// </summary>
        /// <param name="func">要执行的业务逻辑</param>
        /// <param name="fallbackAction">业务逻辑执行失败后，执行的挽救代码</param>
        /// <returns></returns>
        public static PolicyResult<T> Fallback<T>(Func<T> func, Action fallbackAction = null)
        {
            FallbackPolicy policy = Policy.Handle<Exception>().Fallback(() =>
            {
                fallbackAction?.Invoke();
            });

            return policy.ExecuteAndCapture(func);
        }

        #endregion
        
        #region Timeout policy 确保操作不会超过指定的时间

        // TimeoutPolicy是多线程安全和可重用的
        //     Builds a Polly.Policy that will wait for a delegate to complete for a specified
        //     period of time. A Polly.Timeout.TimeoutRejectedException will be thrown if the
        //     delegate does not complete within the configured timeout.
        /// <summary>
        /// 调用action with timeout，不会抛出异常
        /// </summary>
        /// <param name="action">调用的业务逻辑，action在task中执行(即和调用线程不是一个)</param>
        /// <param name="timeoutSpan">多少时间后过期</param>
        /// <param name="onTimeout">过期后的回调，过期后Polly不会杀死task，task仍然执行</param>
        /// <returns></returns>
        public static PolicyResult RunWithTimeOut(Action action, TimeSpan timeoutSpan, Action<Context, TimeSpan, Task> onTimeout = null)
        {
            //TimeoutStrategy.Pessimistic recognises that there are cases where you need to execute delegates which have no in-built timeout, and do not honor cancellation.Policy使用Task执行user delegate
            //在悲观模式下，Polly不会杀死底层的线程，而是将task传递到onTimeout回调中
            TimeoutPolicy policy = Policy.Timeout(timeoutSpan, TimeoutStrategy.Pessimistic,
                (context, timeSpan, task) =>
                {
                    if (onTimeout != null)
                    {
                        onTimeout(context, timeSpan, task);
                    }
                    else
                    {
                        //过期后Polly不会杀死task，task仍然执行，需要在这里进行处理
                    }
                });

            return policy.ExecuteAndCapture(action);
        }


        /// <summary>
        /// 调用func with timeout，不会抛出异常
        /// </summary>
        /// <param name="func">调用的业务逻辑，func在task中执行(即和调用线程不是一个)</param>
        /// <param name="timeoutSpan">多少时间后过期</param>
        /// <param name="onTimeout">过期后的回调，过期后Polly不会杀死task，task仍然执行</param>
        /// <returns></returns>
        public static PolicyResult<T> RunWithTimeOut<T>(Func<T> func, TimeSpan timeoutSpan, Action<Context, TimeSpan, Task> onTimeout = null)
        {
            //TimeoutStrategy.Pessimistic recognises that there are cases where you need to execute delegates which have no in-built timeout, and do not honor cancellation.Policy使用Task执行user delegate
            //在悲观模式下，Polly不会杀死底层的线程，而是将task传递到onTimeout回调中
            TimeoutPolicy policy = Policy.Timeout(timeoutSpan, TimeoutStrategy.Pessimistic,
                (context, timeSpan, task) =>
                {
                    if (onTimeout != null)
                    {
                        onTimeout(context, timeSpan, task);
                    }
                    else
                    {
                        //过期后Polly不会杀死task，task仍然执行，需要在这里进行处理
                    }
                });

            return policy.ExecuteAndCapture(func);
        }


        #endregion

        #region Bulkhead 隔水舱 限制最大允许并行执行的actions 不要因为局部错误而导致整艘船沉没 One fault shouldn't bring down the whole ship!

        //Bulkhead通过限制最大允许并行执行的actions,避免消耗过多的资源，
        //如果并行数超过限制，将action放到queue中，如果超过maxQueuingActions，抛弃action，直接抛出异常
        //The policy itself does not place calls onto threads; it assumes upstream systems have already placed calls into threads, but limits their parallelization of execution.
        //Bulkhead本身不会创建多线程，而是假设上游已经在多线程中使用
        //Bulkhead是多线程安全和可重用的

        /// <summary>
        /// 隔水舱
        /// </summary>
        public class BulkheadEx
        {
            #region 私有的变量

            /// <summary>
            /// 底层隔水舱实例
            /// </summary>
            private readonly BulkheadPolicy _bulkhead;

            #endregion

            #region 构造函数

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="maxParallelization">此Policy可以最多并行执行的action 大于0</param>
            /// <param name="maxQueuingActions">The maxmimum number of actions that may be queuing, waiting for an execution slot. 大于等于0</param>
            /// <param name="onBulkheadRejected">当过多的action超出限制时执行的回调操作</param>
            public BulkheadEx(int maxParallelization, int maxQueuingActions, Action<Context> onBulkheadRejected = null)
            {
                _bulkhead = Policy.Bulkhead(maxParallelization, maxQueuingActions, context =>
                {
                    onBulkheadRejected?.Invoke(context);
                });
            }

            #endregion

            #region 公有函数

            /// <summary>
            /// 获取舱内可用的空间
            /// </summary>
            public int BulkheadAvailableCount
            {
                get { return _bulkhead.BulkheadAvailableCount; }
            }


            /// <summary>
            /// 使用blukhead执行业务逻辑
            /// </summary>
            /// <param name="action"></param>
            /// <returns></returns>
            public PolicyResult ExecuteAndCapture(Action action)
            {
                return _bulkhead.ExecuteAndCapture(action);
            }

            /// <summary>
            /// 使用blukhead执行业务逻辑
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="func"></param>
            /// <returns></returns>
            public PolicyResult<T> ExecuteAndCapture<T>(Func<T> func)
            {
                return _bulkhead.ExecuteAndCapture(func);
            }

            #endregion

        }

        #endregion

        #region Cache 

        #endregion

        #region Policy Wrap

        //fallback.Wrap(breaker).Wrap(retry).Execute(action);//retry最先执行
        //it is thread-safe
        //it can be reused across multiple call sites

        //PolicyWrap policyWrap = fallback.Wrap(cache).Wrap(retry).Wrap(breaker).Wrap(timeout).Wrap(bulkhead);
        #endregion

    }
}

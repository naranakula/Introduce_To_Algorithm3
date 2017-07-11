using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils
{

    /// <summary>
    /// 确保应用程序退出
    /// </summary>
    public static class AppExitEx
    {

        /// <summary>
        /// 确保程序安全退出
        /// </summary>
        /// <param name="normalExitAction">正常退出执行的动作</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="maxWaitMilliSecondBeforeExit">退出之前最长的等待时间，单位毫秒,即normalExitAction可能执行的最长时间，超出该时间说明正常退出指令写的有问题，程序强制杀死</param>
        public static void EnsureExit(Action normalExitAction = null,Action<Exception> exceptionHandler = null,int maxWaitMilliSecondBeforeExit = 1500) 
        {
            try
            {
                #region 强制退出

                //Task创建的是后台线程
                Task.Factory.StartNew(() =>
                {
                    //5s后强制退出
                    if (maxWaitMilliSecondBeforeExit > 0)
                    {
                        try
                        {
                            Thread.Sleep(maxWaitMilliSecondBeforeExit);
                        }
                        catch
                        {
                            //ignore
                        }
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            NLogHelper.Warn($"程序第{i+1}次尝试强制退出");
                            Environment.Exit(0);
                        }
                        catch
                        {
                            //ignore
                            try
                            {
                                Thread.Sleep(10);
                            }
                            catch { }
                        }
                    }
                });

                #endregion

                if (normalExitAction != null)
                {
                    normalExitAction();
                }

            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
                else
                {
                    NLogHelper.Error("程序退出时异常："+ex);
                }
            }
        }

        /// <summary>
        /// 强制退出
        /// </summary>
        /// <param name="maxWaitMilliSecondBeforeExit">退出之前最长的等待时间，单位毫秒, 小于等于0表示不等待</param>
        public static void SafeForceExit(int maxWaitMilliSecondBeforeExit = 0)
        {
            if (maxWaitMilliSecondBeforeExit > 0)
            {
                try
                {
                    Thread.Sleep(maxWaitMilliSecondBeforeExit);
                }
                catch
                {
                    //ignore
                }
            }


            for (int i = 0; i < 11; i++)
            {
                try
                {
                    Environment.Exit(0);
                }
                catch
                {
                    //ignore
                    try
                    {
                        Thread.Sleep(10);
                    }
                    catch { }
                }
            }
        }

    }

}

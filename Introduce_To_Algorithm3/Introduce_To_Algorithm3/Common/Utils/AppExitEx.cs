using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public static class AppExitEx
    {
        /// <summary>
        /// 确保程序安全退出
        /// </summary>
        /// <param name="normalExitAction">正常退出执行的动作</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="maxWaitMilliSecondBeforeExit">退出之前最长的等待时间，单位毫秒,即normalExitAction可能执行的最长时间，超出该时间说明正常退出指令写的有问题，程序强制杀死</param>
        public static void EnsureExit(Action normalExitAction = null,Action<Exception> exceptionHandler = null,int maxWaitMilliSecondBeforeExit = 5000) 
        {
            try
            {
                #region 强制退出
                //Task创建的是后台线程
                Task.Factory.StartNew(() =>
                {
                    //5s后强制退出
                    Thread.Sleep(maxWaitMilliSecondBeforeExit);
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            Environment.Exit(0);
                            Thread.Sleep(10);
                        }
                        catch (Exception)
                        {
                            //ignore
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
            }
        }

    }
}

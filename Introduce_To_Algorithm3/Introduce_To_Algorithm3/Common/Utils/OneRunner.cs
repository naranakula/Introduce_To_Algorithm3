﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using MessageBox = System.Windows.Forms.MessageBox;


namespace Common
{
    /// <summary>
    /// 更加安全合理的方式是使用 Mutex
    /// 只有一个实例运行
    /// </summary>
    public static class OneRunner
    {


        #region Mutex单实例实现

        /// <summary>
        /// 应用程序的ID
        /// 每个程序使用不同的id
        /// 建议在配置文件中配置
        /// </summary>
        private static readonly string AppId = ConfigUtils.GetString("AppId", "dd771b7a02e746b388ffad5adf202fc5");//@"dd771b7a02e746b388ffad5adf202fc5";//ConfigUtils.GetString("AppId");

        /// <summary>
        /// 用于测试单实例的Mutex
        /// </summary>
        private static volatile Mutex _oneRunMutex = null;

        /// <summary>
        /// 是否之前已经运行了实例
        /// 此方法只能程序运行时调用一次
        /// </summary>
        /// <returns></returns>
        public static bool IsAlreadyRun(Action<Exception> exceptionHandler = null)
        {
            
            try
            {
                if (_oneRunMutex != null)
                {
                    throw new ArgumentException("这个方法仅能调用一次");
                }

                bool createdNew = false;
                //注意退出时释放Mutex
                _oneRunMutex = new Mutex(true, AppId, out createdNew);

                if (!createdNew)
                {
                    //已经运行了一个实例
                    //此时不需要释放Mutex，因为你没有拥有锁
                    return true;
                }
                else
                {
                    //第一次运行，程序退出前释放锁，实际上不释放也没关系，因为退出程序会自动释放所有资源  包括锁
                    return false;
                }
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);

                //从严格判断的角度上讲，发生异常判读为已经启动
                return true;
            }
        }

        /// <summary>
        /// 通用的已经运行一个实例的执行动作
        /// </summary>
        public static void GeneralAlreadyRunAction(Action<Exception> exceptionHandler = null)
        {
            try
            {
                #region 强制退出

                int maxWaitMilliSecondBeforeExit = 10000;
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

                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            NLogHelper.Warn($"程序自动第{i + 1}次尝试强制退出");
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

                MessageBox.Show($"已经启动一个实例，请点击确定退出本实例（{maxWaitMilliSecondBeforeExit / 1000}秒后自动退出）", "已经启动一个实例",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        NLogHelper.Warn($"程序强制第{i + 1}次尝试强制退出");
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
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
                else
                {
                    NLogHelper.Error("程序退出时异常：" + ex);
                }
            }
        }



        /// <summary>
        /// 释放锁
        /// 放到程序退出的最后一行代码
        /// </summary>
        public static void ReleaseMutex()
        {
            if (_oneRunMutex != null)
            {
                try
                {
                    //调用时有可能没有获取到锁，抛出异常
                    _oneRunMutex.ReleaseMutex();
                }
                catch
                {
                    // ignored
                }
                try
                {
                    _oneRunMutex.Dispose();
                }
                catch
                {
                    // ignored
                }
                _oneRunMutex = null;
            }
        }


        #endregion



        #region 辅助方法



        /// <summary>
        /// 通用的已经运行一个实例的执行动作
        /// </summary>
        public static void GeneralExitAction(string title, string content, Action<Exception> exceptionHandler = null, int maxWaitMilliSecondBeforeExit = 30000)
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
                        const int sleepPeriod = 100;//每次休眠100毫秒
                        for (int i = 0; i * sleepPeriod < maxWaitMilliSecondBeforeExit; i++)
                        {
                            try
                            {
                                Thread.Sleep(sleepPeriod);
                            }
                            catch
                            {
                                //ignore
                            }
                        }
                    }

                    for (int i = 0; i < 11; i++)
                    {
                        try
                        {
                            NLogHelper.Warn($"程序自动第{i + 1}次尝试强制退出");
                            Environment.Exit(0);
                        }
                        catch
                        {
                            //ignore
                            try
                            {
                                Thread.Sleep(10);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }
                });

                #endregion

                System.Windows.MessageBox.Show(content, title, MessageBoxButton.OK, MessageBoxImage.Warning);

                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        NLogHelper.Warn($"程序强制第{i + 1}次尝试强制退出");
                        Environment.Exit(0);
                    }
                    catch
                    {
                        //ignore
                        try
                        {
                            Thread.Sleep(10);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
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
                    NLogHelper.Error("程序退出时异常：" + ex);
                }
            }
        }




        #endregion



        /// <summary>
        /// IsOnlyOneProcessRunning
        /// 当发生异常时返回false
        /// </summary>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns></returns>
        public static bool IsOnlyOneProcessRunning(Action<Exception> exceptionHandler = null)
        {
            try
            {
                Process[] runningProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
                return runningProcesses.Length <= 1;
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return false;
            }
        }

        /// <summary>
        /// Run only one instance,If not, exit.
        /// </summary>
        /// <param name="waitSecondBeforeExit"></param>
        public static void RunOneProcessOrelseExit(int waitSecondBeforeExit = 5)
        {
            Process[] runningProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (runningProcesses.Length > 1)
            {
                for (int i = waitSecondBeforeExit; i > 0; i--)
                {
                    if (i == 1)
                    {
                        Console.WriteLine("{0} already on machine {1}. Exiting in {2} seconds ...",
                                      Process.GetCurrentProcess().ProcessName, Environment.MachineName, i);
                    }
                    else
                    {
                        Console.Write("{0} already on machine {1}. Exiting in {2} seconds ...\r",
                                      Process.GetCurrentProcess().ProcessName, Environment.MachineName, i);
                    }
                    Thread.Sleep(1000);
                }
                System.Environment.Exit(-1);
            }
        }

        /// <summary>
        /// 将制定进程界面置前
        /// </summary>
        /// <param name="instance"></param>
        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL); //置窗口为正常状态

            SetForegroundWindow(instance.MainWindowHandle);
        }



        #region 调用系统api

        [DllImport("User32.dll ")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll ")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int WS_SHOWNORMAL = 1;

        #endregion

    }


}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Common
{
    /// <summary>
    /// 更加安全合理的方式是使用 Mutex
    /// </summary>
    public static class OneRunner
    {
        #region Mutex单实例实现

        /// <summary>
        /// 应用程序的ID
        /// 每个程序使用不同的id
        /// 建议在配置文件中配置
        /// </summary>
        private const string APP_ID = @"dd771b7a02e746b388ffad5adf202fc5";//ConfigUtils.GetString("AppId");

        /// <summary>
        /// 用于测试单实例的Mutex
        /// </summary>
        private static volatile Mutex OneRunMutex = null;

        /// <summary>
        /// 是否之前已经运行了实例
        /// 此方法只能程序运行时调用一次
        /// </summary>
        /// <returns></returns>
        public static bool IsAlreadyRun()
        {
            if (OneRunMutex != null)
            {
                throw  new ArgumentException("这个方法仅能调用一次");
            }

            bool createdNew = false;
            //注意退出时释放Mutex
            OneRunMutex = new Mutex(true,APP_ID,out createdNew);
            
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

        /// <summary>
        /// 释放锁
        /// 放到程序退出的最后一行代码
        /// </summary>
        public static void ReleaseMutex()
        {
            if (OneRunMutex != null)
            {
                OneRunMutex.ReleaseMutex();
                OneRunMutex = null;
            }
        }


        #endregion


        /// <summary>
        /// IsOnlyOneProcessRunning
        /// </summary>
        /// <returns></returns>
        public static bool IsOnlyOneProcessRunning()
        {
            Process[] runningProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            return runningProcesses.Length == 1;
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

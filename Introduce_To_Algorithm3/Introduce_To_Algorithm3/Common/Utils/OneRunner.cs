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
    public class OneRunner
    {
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

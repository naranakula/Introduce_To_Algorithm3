using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utils
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
    }
}

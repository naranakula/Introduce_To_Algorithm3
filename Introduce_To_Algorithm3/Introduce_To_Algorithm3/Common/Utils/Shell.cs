using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// shell utilities
    /// </summary>
    public static class Shell
    {
        /// <summary>
        /// run cmd using strcmd and strArgs
        /// </summary>
        /// <param name="strCmd"></param>
        /// <param name="strArgs"></param>
        /// <param name="strOutput"></param>
        /// <returns></returns>
        public static int RunConsole(string strCmd, string strArgs, out string strOutput)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = strCmd;
            p.StartInfo.Arguments = strArgs;

            StringBuilder sbStdErr = new StringBuilder();
            p.ErrorDataReceived += (object sender, DataReceivedEventArgs args) => { sbStdErr.Append(args.Data); };
            p.Start();
            p.BeginErrorReadLine();
            strOutput = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            strOutput += sbStdErr.ToString();
            int returnCode = p.ExitCode;
            p.Close();
            return returnCode;
        }

        /// <summary>
        /// run a command, wait for its return code 
        /// </summary>
        /// <param name="strCmd"></param>
        /// <param name="strArgs"></param>
        /// <returns></returns>
        public static int Run(string strCmd, string strArgs)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.Arguments = strArgs;
            psi.CreateNoWindow = true;
            psi.FileName = strCmd;
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            using (Process p = Process.Start(psi))
            {
                p.WaitForExit();
                return p.ExitCode;
            }
        }

        /// <summary>
        /// run a command
        /// </summary>
        /// <param name="strCmd"></param>
        /// <param name="strArgs"></param>
        /// <returns></returns>
        public static int RunConsole(string strCmd, string strArgs)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.Arguments = strArgs;
            psi.FileName = strCmd;
            psi.UseShellExecute = false;
            using (Process p = Process.Start(psi))
            {
                p.WaitForExit();
                return p.ExitCode;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public class PcFunctions
    {

        #region Window API
        /// <summary>
        /// 广播，消息被寄送到系统的所有顶层窗口
        /// </summary>
        private  const int HWND_BROADCAST = 0xFFFF;
        /// <summary>
        /// 系统命令
        /// </summary>
        private const int WM_SYSCOMMAND = 0x0112;
        /// <summary>
        /// 显示器相关
        /// </summary>
        private const int SC_MONITORPOWER = 0xF170;
        /// <summary>
        /// 锁定工作台的显示，效果等同于开始按钮的锁定和CTRL+ALT+DEL
        /// 该方法异步执行
        /// </summary>
        [DllImport("user32")]
        public static extern void LockWorkStation();
        /// <summary>
        /// 该函数将一个消息放入（寄送）到与指定窗口创建的线程相联系消息队列里，不等待线程处理消息就返回，是异步消息模式。
        /// </summary>
        /// <param name="hWnd">
        /// 窗口程序接收消息的窗口和句柄 。可以取特定含义的两个值：
        /// HWND_BROADCAST：消息被寄送到系统的所有顶层窗口
        /// NULL：此函数的操作和调用参数dwThread设置为当前线程的标识符PostThreadMessage函数一样
        /// </param>
        /// <param name="hMsg">指定被寄送的消息。</param>
        /// <param name="wParam">指定附加的消息特定的信息</param>
        /// <param name="lParam">指定附加的消息特定的信息</param>
        /// <returns>如果函数调用成功，返回非零，函数调用返回值为零</returns>
        [DllImport("user32.dll")]
        private static extern int PostMessage(int hWnd, int hMsg, int wParam, int lParam);

        ///// <summary>
        ///// 待机
        ///// </summary>
        ///// <param name="hiberate"></param>
        ///// <param name="forceCritical"></param>
        ///// <param name="disableWakeEvent"></param>
        ///// <returns></returns>
        //[DllImport("Powrprof.dll",CharSet = CharSet.Auto,ExactSpelling = true)]
        //public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        /// <summary>
        /// 退出windows, 关闭计算机
        /// </summary>
        /// <param name="flag">            
        /// 关闭系统的参数。
        ///0 Log off用户退出
        ///4 Forced Log off 强制用户退出
        ///1 Shutdown 关闭操作系统
        ///5 Forced Shutdown 强制关闭操作系统
        ///2 Reboot 重启
        ///6 Forced Reboot 强制重启
        ///8 Power Off - Shuts down the computer and turns off the power (if supported by the computer in question).
        ///12 Forced Power Off
        /// </param>
        /// <param name="reversed"> A means to extend Win32Shutdown. Currently, the Reserved parameter is ignored.</param>
        private static void ExitWin(int flag, int reversed)
        {
            ManagementBaseObject mboShutDown = null;
            //初始化一个公共信息模型的管理类，初始化到当前机器上的所有windows的系统
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();//绑定到管理对象
            //启用用户特权，否则不能关闭
            mcWin32.Scope.Options.EnablePrivileges = true;

            //获取Win32Shutdown方法的参数，该方法提供了提供了操作系统支持的所有关闭系统的参数
            ManagementBaseObject mboShutDownParams = mcWin32.GetMethodParameters("Win32Shutdown");

            //设置参数的值
            //关闭系统的参数。
            //0 Log off用户退出
            //4 Forced Log off 强制用户退出
            //1 Shutdown 关闭操作系统
            //5 Forced Shutdown 强制关闭操作系统
            //2 Reboot 重启
            //6 Forced Reboot 强制重启
            //8 Power Off - Shuts down the computer and turns off the power (if supported by the computer in question).
            //12 Forced Power Off
            mboShutDownParams["Flags"] = flag;
            //Reserved :  A means to extend Win32Shutdown. Currently, the Reserved parameter is ignored.
            mboShutDownParams["Reserved"] = reversed;

            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutDown = manObj.InvokeMethod("Win32Shutdown", mboShutDownParams, null);
            }
        }

        #endregion

        #region 主功能区
        /// <summary>
        /// 关闭计算机
        /// </summary>
        public static void ShutDownComputer()
        {
            try
            {
                //关闭操作系统
                ExitWin(1, 0);
            }
            catch (Exception ex)
            {
                Log4netHelper.Error("关闭操作系统失败",ex);
                try
                {
                    //强制关闭操作系统
                    ExitWin(5, 0);
                }
                catch (Exception ex2)
                {
                    Log4netHelper.Error("强制关闭操作系统失败", ex2);
                }
            }

            MonitorOff();
        }

        /// <summary>
        /// 关闭显示器
        /// 注：仅仅关闭显示器，系统仍然正常运行
        /// </summary>
        public static void MonitorOff()
        {
            PostMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
        }

        /// <summary>
        /// 打开显示器
        /// </summary>
        public static void MonitorOn()
        {
            // Turn on monitor
            PostMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, -1);
        }

        /// <summary>
        /// 重启计算机
        /// </summary>
        public static void RestartComputer()
        {
            try
            {
                //重启计算机
                ExitWin(2,0);
            }
            catch (Exception ex)
            {
                Log4netHelper.Error("重启计算机失败，尝试强制重启："+ex);
                try
                {
                    //强制重启
                    ExitWin(6,0);
                }
                catch (Exception ex2)
                {
                    Log4netHelper.Error("强制重启失败：" + ex2);
                }
            }

            MonitorOff();
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public class PInvokeHelper
    {
        /// <summary>
        /// 创建指定窗口的线程设置到前台，并且激活该窗口。
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern IntPtr SetForegroundWindow(IntPtr hwnd);
        /// <summary>
        /// 显示隐藏窗口
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>

        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        /// <summary>
        /// 根据标题查找窗口句柄
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);
        /// <summary>
        /// 最小化窗口，即使拥有窗口的线程被挂起也会最小化。在从其他线程最小化窗口时才使用这个参数。
        /// </summary>
        public const int SW_FORCEMINIMIZE = 11;

        /// <summary>
        /// 隐藏窗口并激活其他窗口。
        /// </summary>
        public const int SW_HIDE = 0;
        /// <summary>
        /// 窗口原来的位置以原来的尺寸激活和显示窗口。
        /// </summary>
        public const int SW_SHOW = 5;
        /// <summary>
        /// 最大化指定的窗口
        /// </summary>
        public const int SW_MAXIMIZE = 3;
        /// <summary>
        /// 最小化指定的窗口并且激活在Z序中的下一个顶层窗口。
        /// </summary>
        public const int SW_MINIMIZE = 6;

        /// <summary>
        /// 激活窗口并将其最小化
        /// </summary>
        public const int SW_SHOWMINIMIZED = 2;
        /// <summary>
        /// 激活窗口并将其最大化。
        /// </summary>
        public const int SW_SHOWMAXIMIZED = 3;
        /// <summary>
        /// 激活窗口并将其最小化。
        /// </summary>
        public const int SW_SHOWNOACTIVATE = 4;

    }
}

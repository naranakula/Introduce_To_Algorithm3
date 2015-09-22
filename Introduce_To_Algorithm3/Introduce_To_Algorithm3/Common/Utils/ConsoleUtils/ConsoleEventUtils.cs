using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 接收控制台事件，主要是关闭事件,
    /// 该钩子函数定义到Main方法同一级别
    /// </summary>
    public static class ConsoleEventUtils
    {
        /// <summary>
        /// 释放与调用线程关联的控制台
        /// </summary>
        /// <returns>调用成功，返回true</returns>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        /// <summary>
        /// 为调用进程分配一个新的控制台。
        /// 一个进程只能拥有一个控制台关联。
        /// 在使用完控制台程序之后一定要记得调用FreeConsole函数释放该控制台，否则会造成内存泄露。
        /// </summary>
        /// <returns>调用成功，返回true</returns>
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handlerRoutine"></param>
        /// <param name="add">true，表示添加；false表示删除</param>
        /// <returns>函数成功返回true,失败返回false</returns>
        [DllImport("kernel32.dll",CharSet = CharSet.Auto)]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine,bool add);

        /// <summary>
        /// 返回false，表示程序关闭；true，不会关闭
        /// </summary>
        /// <param name="ctrlType"></param>
        /// <returns></returns>
        public delegate bool ConsoleCtrlDelegate(int ctrlType);

        /// <summary>
        /// 用户关闭Console时，系统发送的消息
        /// </summary>
        private const int CTRL_CLOSE_EVENT = 2;

        /// <summary>
        /// Ctrl+C,系统发送的消息
        /// </summary>
        private const int CTRL_C_EVENT = 0;

        /// <summary>
        /// Ctrl+Break,系统发送的消息
        /// </summary>
        private const int CTRL_BREAK_EVENT = 1;

        /// <summary>
        /// 用户退出(注销),系统发送的消息
        /// </summary>
        private const int CTRL_LOGOFF_EVENT = 5;

        /// <summary>
        /// 系统关闭，系统发送的消息
        /// </summary>
        private const int CTRL_SHUTDOWN_EVENT = 6;

        /// <summary>
        /// 安装消息处理钩子
        /// </summary>
        /// <returns>true,表示安装成功，false，表示安装失败</returns>
        public static Boolean RegisterHandler()
        {
            ConsoleCtrlDelegate consoleCtrlDelegate = new ConsoleCtrlDelegate(HandleRoutine);
            bool bRet = SetConsoleCtrlHandler(consoleCtrlDelegate, true);

            return bRet;
        }

        /// <summary>
        /// 返回false，表示程序关闭；true，不会关闭
        /// </summary>
        /// <param name="ctrlType"></param>
        /// <returns></returns>
        private static bool HandleRoutine(int ctrlType)
        {
            switch (ctrlType)
            {
                case  CTRL_C_EVENT:
                case CTRL_BREAK_EVENT:
                    return true;
              
                case CTRL_CLOSE_EVENT:
                case  CTRL_LOGOFF_EVENT:
                case CTRL_SHUTDOWN_EVENT:
                    break;
            }
            
            //返回false，忽略处理，让系统进行默认的操作
            //返回true,阻止系统的操作
            return false;
        }
    }
}

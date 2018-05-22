﻿using System;
using System.Runtime.InteropServices;

namespace Introduce_To_Algorithm3.Common.Utils.ConsoleUtils
{
    public static class ConsoleUtils
    {
        /// <summary>
        /// 释放与调用线程关联的控制台
        /// </summary>
        /// <returns>调用成功，返回true</returns>
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        /// <summary>
        /// 为调用进程分配一个新的控制台。
        /// 一个进程只能拥有一个控制台关联。
        /// 在使用完控制台程序之后一定要记得调用FreeConsole函数释放该控制台，否则会造成内存泄露。
        /// </summary>
        /// <returns>调用成功，返回true</returns>
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();


        /// <summary>
        /// 分配控制台
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool SafeAllocConsole(Action<Exception> exceptionHandler = null)
        {
            try
            {
                return AllocConsole();
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        /// <summary>
        /// 释放控制台
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool SafeFreeConsole(Action<Exception> exceptionHandler = null)
        {
            try
            {
                return FreeConsole();
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }
    }
}

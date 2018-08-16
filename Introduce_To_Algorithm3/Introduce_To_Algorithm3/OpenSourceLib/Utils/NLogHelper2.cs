using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    public static class NLogHelper2
    {

        /// <summary>
        /// 私有的logger类
        /// Logger类是多线程安全的
        /// loggers are thread-safe
        /// 实际使用中在对应的类中创建一个_logger，这样就可以使用${callsite}了
        /// 使用不同name的logger，根据名称记录不同的日志：如邮件，数据库日志
        /// </summary>
        private static readonly Logger Slogger = LogManager.GetCurrentClassLogger();



        /// <summary>
        /// 记录Trace日志
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                Slogger.Trace(message);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 记录Debug日志
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                Slogger.Debug(message);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 记录Info信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                Slogger.Info(message);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 记录Warn日志
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                Slogger.Warn(message);
                
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        /// 记录Error日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                Slogger.Error(message);
                
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 记录Fetal日志
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                Slogger.Fatal(message);
            }
            catch
            {
                // ignored
            }
        }


    }
}

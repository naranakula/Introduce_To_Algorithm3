using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using MathNet.Numerics;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// log4net日志
    /// 通过log4net配置来支持多线程
    /// 支持fatal\ error\ warn\ info\ debug日志
    /// 
    /// 在高性能条件下：建立一个队列和线程专门负责写日志，其它线程只需将要写的日志放到队列中即可
    /// </summary>
    public static class Log4netHelper
    {
        /// <summary>
        /// 日志名
        /// </summary>
        private const string LOGGERNAME = "CustomLog";

        #region 静态初始化

        /// <summary>
        /// 自动加载log4net.config
        /// </summary>
        static Log4netHelper()
        {
            //获取应用程序的目录，并查找log4net.config文件
            string dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            FileInfo file = new FileInfo(Path.Combine(dir, "log4net.config"));
            if (file.Exists)
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(file);
            }
        }

        #endregion

        #region fatal
        /// <summary>
        /// 写Fatal日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Fatal(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Fatal(msg);
        }
        /// <summary>
        /// 写Fatal日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Fatal(object msg)
        {
            if (msg == null)
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Fatal(msg);
        }

        /// <summary>
        /// 写Fatal日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="args">格式化参数</param>
        public static void Fatal(string msg, string args)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Fatal(string.Format(msg, args));
        }

        /// <summary>
        /// 写Fatal日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="ex">The exception to log, including its stack trace.</param>
        public static void Fatal(string msg, Exception exception)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Fatal(msg, exception);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void FatalFormat(string format, params object[] args)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).FatalFormat(format, args);
        }

        #endregion


        #region error
        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Error(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Error(msg);
        }
        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Error(object msg)
        {
            if (msg == null)
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Error(msg);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="args">格式化参数</param>
        public static void Error(string msg,string args)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Error(string.Format(msg,args));
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="ex">The exception to log, including its stack trace.</param>
        public static void Error(string msg, Exception exception)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Error(msg, exception);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(string format, params object[] args)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).ErrorFormat(format, args);
        }

        #endregion

        #region warn

        /// <summary>
        /// 写warn日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Warn(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Warn(msg);
        }

        /// <summary>
        /// 写warn日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exception"></param>
        public static void Warn(string msg, Exception exception)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Warn(msg, exception);
        }


        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Warn(object msg)
        {
            if (msg == null)
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Warn(msg);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="args">格式化参数</param>
        public static void Warn(string msg, string args)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Warn(string.Format(msg, args));
        }


        /// <summary>
        /// 写warn日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WarnFormat(string format, params object[] args)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).WarnFormat(format, args);
        }

        #endregion

        #region  info
        /// <summary>
        /// 写info日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Info(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Info(msg);
        }

        /// <summary>
        /// 写info日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="ex">The exception to log, including its stack trace.</param>
        public static void Info(string msg, Exception exception)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Info(msg, exception);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Info(object msg)
        {
            if (msg == null)
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Info(msg);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="args">格式化参数</param>
        public static void Info(string msg, string args)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Info(string.Format(msg, args));
        }


        /// <summary>
        /// 写info日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void InfoFormat(string format, params object[] args)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).InfoFormat(format, args);
        }

        #endregion

        #region Debug

        /// <summary>
        /// 写debug日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Debug(msg);
        }

        /// <summary>
        /// 写debug日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exception"></param>
        public static void Debug(string msg, Exception exception)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Debug(msg, exception);
        }

        /// <summary>
        /// 写debug日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void DebugFormat(string format, params object[] args)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).DebugFormat(format, args);
        }


        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Debug(object msg)
        {
            if (msg == null)
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Debug(msg);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="args">格式化参数</param>
        public static void Debug(string msg, string args)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            log4net.LogManager.GetLogger(LOGGERNAME).Debug(string.Format(msg, args));
        }


        #endregion

        /// <summary>
        /// 格式化数据
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

    }
}

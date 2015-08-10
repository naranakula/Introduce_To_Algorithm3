using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// log4net日志
    /// 通过log4net配置来支持多线程
    /// 支持error\ warn\ info\ debug日志
    /// </summary>
    public static class Log4netHelper
    {
        /// <summary>
        /// 日志名
        /// </summary>
        private const string LOGGERNAME = "CmluLog";


        #region 静态初始化

        /// <summary>
        /// 自动加载log4net.config
        /// </summary>
        static Log4netHelper()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));
        }

        #endregion


        #region error
        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Error(string msg)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).Error(msg);
        }
        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Error(object msg)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).Error(msg);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="args">格式化参数</param>
        public static void Error(string msg,string args)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).Error(string.Format(msg,args));
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="ex">The exception to log, including its stack trace.</param>
        public static void Error(string msg, Exception exception)
        {
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
            log4net.LogManager.GetLogger(LOGGERNAME).Warn(msg);
        }

        /// <summary>
        /// 写warn日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exception"></param>
        public static void Warn(string msg, Exception exception)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).Warn(msg, exception);
        }


        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Warn(object msg)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).Warn(msg);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="args">格式化参数</param>
        public static void Warn(string msg, string args)
        {
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
            log4net.LogManager.GetLogger(LOGGERNAME).Info(msg);
        }

        /// <summary>
        /// 写info日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="ex">The exception to log, including its stack trace.</param>
        public static void Info(string msg, Exception exception)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).Info(msg, exception);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Info(object msg)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).Info(msg);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="args">格式化参数</param>
        public static void Info(string msg, string args)
        {
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
            log4net.LogManager.GetLogger(LOGGERNAME).Debug(msg);
        }

        /// <summary>
        /// 写debug日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exception"></param>
        public static void Debug(string msg, Exception exception)
        {
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
            log4net.LogManager.GetLogger(LOGGERNAME).Debug(msg);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="args">格式化参数</param>
        public static void Debug(string msg, string args)
        {
            log4net.LogManager.GetLogger(LOGGERNAME).Debug(string.Format(msg, args));
        }


        #endregion
    }
}

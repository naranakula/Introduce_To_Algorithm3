using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils.files;
using log4net;
using log4net.Core;
using log4net.Repository;
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
        
        /// <summary>
        /// 是否初始化
        /// </summary>
        private static volatile bool  isInited = false;

        /// <summary>
        /// 日志
        /// </summary>
        private static volatile ILog logger = null;

        #region 静态初始化

        static Log4netHelper()
        {
            Init();
        }

        /// <summary>
        /// 自动加载log4net.config
        /// </summary>
        public static bool Init(Action<Exception> exceptionHandler = null)
        {
            try
            {
                if (isInited)
                {
                    return true;
                }
                
                //获取应用程序的目录，并查找log4net.config文件
                //string dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string configFile = DirectoryHold.ResolveFile("log4net.config");
                ILoggerRepository repository = LogManager.CreateRepository("CmluRepository");//该方法如果已经存在同名Repository，则抛出异常
                if (!string.IsNullOrWhiteSpace(configFile))
                {
                    FileInfo file = new FileInfo(configFile);
                    log4net.Config.XmlConfigurator.ConfigureAndWatch(repository,file);
                    logger = log4net.LogManager.GetLogger(repository.Name, LOGGERNAME);
                    isInited = true;
                    return true;
                }
                else
                {
                    throw new Exception("未找到log4net.config配置文件");
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return false;
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
            try
            {
                logger.Fatal(msg);
            }
            catch { }
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
            try
            {
                logger.Fatal(msg);
            }
            catch { }
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
            try
            {
                logger.Fatal(string.Format(msg, args));
            }
            catch
            {
                
            }
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
            try
            {
                logger.Fatal(msg, exception);
            }
            catch { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void FatalFormat(string format, params object[] args)
        {
            try
            {
                logger.FatalFormat(format, args);
            }
            catch { }
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
            try
            {
                logger.Error(msg);
            }
            catch { }
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
            try
            {
                logger.Error(msg);
            }
            catch { }
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

            try
            {
                logger.Error(string.Format(msg, args));
            }
            catch { }
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

            try
            {
                logger.Error(msg, exception);
            }
            catch { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(string format, params object[] args)
        {
            try
            {
                logger.ErrorFormat(format, args);
            }
            catch { }
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

            try
            {
                logger.Warn(msg);
            }
            catch { }
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

            try
            {
                logger.Warn(msg, exception);
            }
            catch { }
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

            try
            {
                logger.Warn(msg);
            }
            catch { }
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

            try
            {
                logger.Warn(string.Format(msg, args));
            }
            catch { }
        }


        /// <summary>
        /// 写warn日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WarnFormat(string format, params object[] args)
        {
            try
            {
                logger.WarnFormat(format, args);
            }
            catch { }
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

            try
            {
                logger.Info(msg);
            }
            catch { }
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
            try
            {
                logger.Info(msg, exception);
            }
            catch { }
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

            try
            {
                logger.Info(msg);
            }
            catch { }
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

            try
            {
                logger.Info(string.Format(msg, args));
            }
            catch { }
        }


        /// <summary>
        /// 写info日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void InfoFormat(string format, params object[] args)
        {
            try
            {
                logger.InfoFormat(format, args);
            }
            catch { }
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

            try
            {
                logger.Debug(msg);
            }
            catch { }
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

            try
            {
                logger.Debug(msg, exception);
            }
            catch { }
        }

        /// <summary>
        /// 写debug日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void DebugFormat(string format, params object[] args)
        {
            try
            {
                logger.DebugFormat(format, args);
            }
            catch { }
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

            try
            {
                logger.Debug(msg);
            }
            catch { }
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

            try
            {
                logger.Debug(string.Format(msg, args));
            }
            catch { }
        }


        #endregion


        /// <summary>
        /// 是否Fatal Enabled
        /// </summary>
        /// <returns></returns>
        public static bool IsFatalEnabled()
        {
            try
            {
                return logger.IsFatalEnabled;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 是否Error Enabled
        /// </summary>
        /// <returns></returns>
        public static bool IsErrorEnabled()
        {
            try
            {
                return logger.IsErrorEnabled;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否Warn Enabled
        /// </summary>
        /// <returns></returns>
        public static bool IsWarnEnabled()
        {
            try
            {
                return logger.IsWarnEnabled;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 是否Info Enabled
        /// </summary>
        /// <returns></returns>
        public static bool IsInfoEnabled()
        {
            try
            {
                return logger.IsInfoEnabled;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 是否Debug Enabled
        /// </summary>
        /// <returns></returns>
        public static bool IsDebugEnabled()
        {
            try
            {
                return logger.IsDebugEnabled;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 格式化数据
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(string format, params object[] args)
        {
            return string.Format(format, args);
        }

    }

}

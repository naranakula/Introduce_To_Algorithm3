using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils.ConcurrentCollections;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// Nlog异步辅助类
    /// </summary>
    public static class NLogAsyncHelper
    {

        /// <summary>
        /// 私有的logger类
        /// Logger类是多线程安全的
        /// loggers are thread-safe
        /// 实际使用中在对应的类中创建一个_logger，这样就可以使用${callsite}了
        /// 使用不同name的logger，根据名称记录不同的日志：如邮件，数据库日志
        /// </summary>
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
                AsyncActionUtils.AddAction(() =>
                {
                    _logger.Trace(message);
                });
            }
            catch (Exception)
            {

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
                AsyncActionUtils.AddAction(() =>
                {
                    _logger.Debug(message);
                });
            }
            catch (Exception)
            {

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
                AsyncActionUtils.AddAction(() =>
                {
                    _logger.Info(message);
                });
            }
            catch (Exception)
            {
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
                AsyncActionUtils.AddAction(() =>
                {
                    _logger.Warn(message);
                });
            }
            catch (Exception)
            {

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
                AsyncActionUtils.AddAction(() =>
                {
                    _logger.Error(message);
                });
            }
            catch (Exception)
            {

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
                AsyncActionUtils.AddAction(() =>
                {
                    _logger.Fatal(message);
                });
            }
            catch (Exception)
            {

            }
        }

        #region 各级别日志是否可用

        /// <summary>
        /// Trace级别日志是否开启
        /// </summary>
        public static bool IsTraceEnabled
        {
            get
            {
                try
                {
                    return _logger.IsTraceEnabled;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Debug级别日志是否开启
        /// </summary>
        public static bool IsDebugEnabled
        {
            get
            {
                try
                {
                    return _logger.IsDebugEnabled;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Info级别日志是否开启
        /// </summary>
        public static bool IsInfoEnabled
        {
            get
            {
                try
                {
                    return _logger.IsInfoEnabled;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Warn级别日志是否开启
        /// </summary>
        public static bool IsWarnEnabled
        {
            get
            {
                try
                {
                    return _logger.IsWarnEnabled;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Error级别日志是否开启
        /// </summary>
        public static bool IsErrorEnabled
        {
            get
            {
                try
                {
                    return _logger.IsErrorEnabled;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Fatal级别日志是否开启
        /// </summary>
        public static bool IsFatalEnabled
        {
            get
            {
                try
                {
                    return _logger.IsFatalEnabled;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion


    }
}

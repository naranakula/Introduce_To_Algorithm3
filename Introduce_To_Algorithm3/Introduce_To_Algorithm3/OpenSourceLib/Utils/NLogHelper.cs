using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.ConcurrentCollections;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite;
using Introduce_To_Algorithm3.Common.Utils.strings;
using Introduce_To_Algorithm3.Models;
using NLog;
using Logger = NLog.Logger;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{


    /// <summary>
    /// 根据github，nlog比log4net更好
    /// nlog自动查找程序目录的NLog.config来配置程序
    /// 
    /// 基本原则：Trace和Debug不包含在上线产品中， Info及以上包含在上线产品中
    /// 
    /// 受限访问或者受限日志：在使用中，服务器端会记录大量日志，这时需要限制每分钟接口可以访问的次数。使用Token Bucket 算法，每次接口调用消耗Bucket中一个Token，Bucket中Token有最大上限，每分钟定时向Bucket中添加若干个Token，最大不可超过上限。
    /// </summary>
    public static class NLogHelper
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
        /// 数据库日志，支持error和fatal
        /// </summary>
        private static readonly BlockingQueueEx<CommonModel> SblockingQueueEx = new BlockingQueueEx<CommonModel>(singleDataHandler: item
            =>
        {
            if (item == null || item.CommonModelObject == null)
            {
                return;
            }

            #region 日志

            if (StringUtils.EqualsEx(CommonModelType.LogType, item.CommonModelType))
            {
                var logItem = item.CommonModelObject as LogItem;
                if (logItem == null)
                {
                    return;
                }

                SqliteCodeFirstContext.ActionSafe(context =>
                {
                    context.LogItems.Add(logItem);
                    context.SaveChanges();
                });
            }

            #endregion
        });

        
        ///// <summary>
        ///// 静态构造函数
        ///// </summary>
        //static NLogHelper()
        //{
        //    //对_logger做定制的初始化
        //}
        //private static Logger _logger = LogManager.GetLogger("fileandconsole");

        ///// <summary>
        ///// 获取指定名字的日志
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static void SetLoggerName(string name)
        //{
        //    _logger = LogManager.GetLogger(name);
        //}

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
                Slogger.Debug(message);
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// 记录Info信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isEnableDbLog"></param>
        public static void Info(string message, bool isEnableDbLog = false)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                Slogger.Info(message);

                if (isEnableDbLog)
                {
                    LogItem item = new LogItem()
                    {
                        Id = GuidUtils.GetGuid32(),
                        LogSource = "",
                        LogType = "info",
                        CreateTime = DateTime.Now,
                        LogContent = message
                    };
                    CommonModel model = new CommonModel()
                    {
                        CommonModelObject = item,
                        CommonModelType = CommonModelType.LogType
                    };
                    SblockingQueueEx.Add(model);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 记录Warn日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isEnableDbLog"></param>
        public static void Warn(string message, bool isEnableDbLog = false)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                Slogger.Warn(message);

                if (isEnableDbLog)
                {
                    LogItem item = new LogItem()
                    {
                        Id = GuidUtils.GetGuid32(),
                        LogSource = "",
                        LogType = "warn",
                        CreateTime = DateTime.Now,
                        LogContent = message
                    };
                    CommonModel model = new CommonModel()
                    {
                        CommonModelObject = item,
                        CommonModelType = CommonModelType.LogType
                    };
                    SblockingQueueEx.Add(model);
                }
            }
            catch (Exception)
            {

            }

        }


        /// <summary>
        /// 记录Error日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isEnableDbLog"></param>
        public static void Error(string message, bool isEnableDbLog = false)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                Slogger.Error(message);

                if (isEnableDbLog)
                {
                    LogItem item = new LogItem()
                    {
                        Id = GuidUtils.GetGuid32(),
                        LogSource = "",
                        LogType = "error",
                        CreateTime = DateTime.Now,
                        LogContent = message
                    };
                    CommonModel model = new CommonModel()
                    {
                        CommonModelObject = item,
                        CommonModelType = CommonModelType.LogType
                    };
                    SblockingQueueEx.Add(model);
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 记录Fetal日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isEnableDbLog"></param>
        public static void Fatal(string message,bool isEnableDbLog = false)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                Slogger.Fatal(message);

                if (isEnableDbLog)
                {
                    LogItem item = new LogItem()
                    {
                        Id = GuidUtils.GetGuid32(),
                        LogSource = "",
                        LogType = "fatal",
                        CreateTime = DateTime.Now,
                        LogContent = message
                    };
                    CommonModel model = new CommonModel()
                    {
                        CommonModelObject = item,
                        CommonModelType = CommonModelType.LogType
                    };
                    SblockingQueueEx.Add(model);
                }
            }
            catch (Exception)
            {

            }
        }


        #region 队列相关


        /// <summary>
        /// 添加处理消息
        /// </summary>
        /// <param name="model"></param>
        public static void AddMessage(CommonModel model)
        {
            SblockingQueueEx.Add(model);
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            SblockingQueueEx.Stop();
        }

        #endregion

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
                    return Slogger.IsTraceEnabled;
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
                    return Slogger.IsDebugEnabled;
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
                    return Slogger.IsInfoEnabled;
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
                    return Slogger.IsWarnEnabled;
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
                    return Slogger.IsErrorEnabled;
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
                    return Slogger.IsFatalEnabled;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion

        ///// <summary>
        ///// 格式化数据
        ///// </summary>
        ///// <param name="format"></param>
        ///// <param name="args"></param>
        ///// <returns></returns>
        //public static string FormatWith(this string format, params object[] args)
        //{
        //    return string.Format(format, args);
        //}


    }


}

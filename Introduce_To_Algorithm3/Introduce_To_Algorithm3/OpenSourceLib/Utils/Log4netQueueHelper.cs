using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// log4net日志
    /// 通过log4net配置来支持多线程
    /// 支持fatal\ Debug\ warn\ info\ debug日志
    /// 
    /// 在高性能条件下：建立一个队列和线程专门负责写日志，其它线程只需将要写的日志放到队列中即可
    /// </summary>
    public static class Log4netQueueHelper
    {
        /// <summary>
        /// 日志名
        /// </summary>
        private const string LOGGERNAME = "CustomLog";

        /// <summary>
        /// 消息队列
        /// </summary>
        private static volatile ConcurrentQueue<Message> msgQueue;

        /// <summary>
        /// 后台线程
        /// </summary>
        private static Thread backGroundThread;

        #region 静态初始化

        /// <summary>
        /// 自动加载log4net.config
        /// </summary>
        static Log4netQueueHelper()
        {
            //获取应用程序的目录，并查找log4net.config文件
            string dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            FileInfo file = new FileInfo(Path.Combine(dir, "log4net.config"));
            if (file.Exists)
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(file);
            }

            //建立消息队列
            msgQueue = new ConcurrentQueue<Message>();

            //建立后台线程
            backGroundThread = new Thread(ThreadCallback);
            backGroundThread.Name = "LogThread";
            //设置为后台线程，这样前台线程结束后，自动结束
            backGroundThread.IsBackground = true;
            backGroundThread.Start();
        }

        /// <summary>
        /// 线程回调
        /// </summary>
        private static void ThreadCallback()
        {
            //循环计数
            int loopCount = 0;
            while (true)
            {
                Message msgItem;
                if (msgQueue.TryDequeue(out msgItem))
                {
                    if (msgItem == null || string.IsNullOrWhiteSpace(msgItem.Msg))
                    {
                        return;
                    }
                    try
                    {

                        ILog logger = log4net.LogManager.GetLogger(LOGGERNAME);
                        switch (msgItem.MsgType)
                        {
                            case MessageType.Fatal:
                                logger.Fatal(msgItem.Msg);
                                break;
                            case MessageType.Error:
                                logger.Error(msgItem.Msg);
                                break;
                            case MessageType.Warn:
                                logger.Warn(msgItem.Msg);
                                break;
                            case MessageType.Info:
                                logger.Info(msgItem.Msg);
                                break;
                            case MessageType.Debug:
                                logger.Debug(msgItem.Msg);
                                break;

                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                else
                {
                    //做点事
                    loopCount++;

                    if (loopCount > 10000000)
                    {
                        loopCount = 1;
                    }

                    //每20次休息50秒
                    if (loopCount % 20 == 0)
                    {
                        Thread.Sleep(50);
                    }
                }
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
            Message msgItem = new Message()
            {
                Msg = msg,
                MsgType = MessageType.Fatal
            };
            Enqueue(msgItem);
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
            Message msgItem = new Message()
            {
                Msg = msg.ToString(),
                MsgType = MessageType.Fatal
            };
            Enqueue(msgItem);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void FatalFormat(string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = string.Format(format, args),
                MsgType = MessageType.Fatal
            };
            Enqueue(msgItem);
        }

        #endregion

        #region Error
        /// <summary>
        /// 写Error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Error(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = msg,
                MsgType = MessageType.Error
            };
            Enqueue(msgItem);
        }
        /// <summary>
        /// 写Error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Error(object msg)
        {
            if (msg == null)
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = msg.ToString(),
                MsgType = MessageType.Error
            };
            Enqueue(msgItem);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = string.Format(format, args),
                MsgType = MessageType.Error
            };
            Enqueue(msgItem);
        }

        #endregion

        #region warn

        /// <summary>
        /// 写Warn日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Warn(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = msg,
                MsgType = MessageType.Warn
            };
            Enqueue(msgItem);
        }
        /// <summary>
        /// 写Warn日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Warn(object msg)
        {
            if (msg == null)
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = msg.ToString(),
                MsgType = MessageType.Warn
            };
            Enqueue(msgItem);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WarnFormat(string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = string.Format(format, args),
                MsgType = MessageType.Warn
            };
            Enqueue(msgItem);
        }

        #endregion

        #region  info
        /// <summary>
        /// 写Info日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Info(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = msg,
                MsgType = MessageType.Info
            };
            Enqueue(msgItem);
        }
        /// <summary>
        /// 写Info日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Info(object msg)
        {
            if (msg == null)
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = msg.ToString(),
                MsgType = MessageType.Info
            };
            Enqueue(msgItem);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void InfoFormat(string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = string.Format(format, args),
                MsgType = MessageType.Info
            };
            Enqueue(msgItem);
        }

        #endregion

        #region Debug
        /// <summary>
        /// 写Debug日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Debug(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = msg,
                MsgType = MessageType.Debug
            };
            Enqueue(msgItem);
        }
        /// <summary>
        /// 写Debug日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Debug(object msg)
        {
            if (msg == null)
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = msg.ToString(),
                MsgType = MessageType.Debug
            };
            Enqueue(msgItem);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void DebugFormat(string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return;
            }

            Message msgItem = new Message()
            {
                Msg = string.Format(format, args),
                MsgType = MessageType.Debug
            };
            Enqueue(msgItem);
        }

        #endregion

        /// <summary>
        /// 辅助入队方法
        /// </summary>
        /// <param name="msgItem"></param>
        private static void Enqueue(Message msgItem)
        {
            if (msgItem == null)
            {
                return;
            }

            //最多2w条消息，防止爆队
            if (msgQueue.Count > 20000)
            {
                Message tempItem;
                msgQueue.TryDequeue(out tempItem);
                msgQueue.TryDequeue(out tempItem);
                msgQueue.TryDequeue(out tempItem);
                msgQueue.TryDequeue(out tempItem);
                msgQueue.TryDequeue(out tempItem);
                msgQueue.TryDequeue(out tempItem);
            }

            msgQueue.Enqueue(msgItem);
        }

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

        /// <summary>
        /// 私有辅助消息类
        /// </summary>
        private class Message
        {
            /// <summary>
            /// 消息类型
            /// </summary>
            public MessageType MsgType { get; set; }
            /// <summary>
            /// 消息类型
            /// </summary>
            public string Msg { get; set; }
        }

        /// <summary>
        /// 私有辅助消息类型类
        /// </summary>
        private enum MessageType
        {
            Fatal,//致命
            Error,//错误
            Warn,//警告
            Info,//信息
            Debug//调试
        }

    }
}

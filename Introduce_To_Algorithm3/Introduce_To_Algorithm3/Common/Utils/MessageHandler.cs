﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// MQ的消息处理
    ///     /// 外部调用
    /// MQMessageHandler.Init();
    /// MQMonitorTimer.Start();
    /// 
    /// 被BlockingQueueEx<T>替代
    /// </summary>
    [Obsolete("被BlockQueueEx替代")]
    public class MessageHandler
    {
        /// <summary>
        /// 线程安全集合提供阻塞和限制功能
        /// </summary>
        private static BlockingCollection<String> messageQueue = new BlockingCollection<string>();

        /// <summary>
        /// 是否正在运行，用来判断程序是否关闭
        /// </summary>
        private static volatile bool isRunning = true;

        /// <summary>
        /// 消息处理线程
        /// </summary>
        private static volatile Thread messageHandlerThread = null;

        /// <summary>
        /// 添加到消息队列
        /// </summary>
        /// <param name="message"></param>
        public static void AddToQueue(String message)
        {
            messageQueue.Add(message);
        }



        /// <summary>
        /// 初始化消息处理线程
        /// </summary>
        public static void Init()
        {
            lock (messageQueue)
            {

                if (messageHandlerThread != null)
                {
                    return;
                }

                //设置为运行
                isRunning = true;
            }



            messageHandlerThread = new Thread(() =>
            {
                while (isRunning)
                {
                    try
                    {
                        String message = null;
                        //等待1111ms获取消息
                        if (messageQueue.TryTake(out message, 1111))
                        {
                            #region 处理消息message

                            try
                            {
                                HandleMessage(message);
                            }
                            catch (Exception ex)
                            {
                                NLogHelper.Error("HandleMessage处理消息失败,异常信息:" + ex);
                            }

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Error("处理消息失败,异常信息:" + ex);
                        try
                        {
                            Thread.Sleep(5);
                        }
                        catch (Exception)
                        {
                            //ignore
                        }
                    }
                }
            });

            
            messageHandlerThread.Start();

            NLogHelper.Info("初始化消息处理线程成功");

            return;
        }

        /// <summary>
        /// 实际的消息处理
        /// </summary>
        /// <param name="message"></param>
        private static void HandleMessage(String message)
        {
            #region 实际消息处理

            if (String.IsNullOrWhiteSpace(message))
            {
                NLogHelper.Debug("消息为空，不做处理");
                return;
            }

            NLogHelper.Debug($"开始处理消息:{message}");

            //TODO: 实际的消息处理代码


            #endregion
        }


        /// <summary>
        /// 停止消息处理线程
        /// </summary>
        public static void Stop()
        {
            lock (messageQueue)
            {
                isRunning = false;
            }
        }

    }
}

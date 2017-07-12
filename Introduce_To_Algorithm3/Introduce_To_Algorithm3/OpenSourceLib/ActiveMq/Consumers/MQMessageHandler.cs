using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq
{
    /// <summary>
    /// MQ的消息处理
    ///     /// 外部调用
    /// MQMessageHandler.Init();
    /// MQMonitorTimer.Start();
    /// </summary>
    public class MQMessageHandler
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
                        //等待317ms获取消息
                        if (messageQueue.TryTake(out message, 317))
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
        public static void HandleMessage(String message)
        {
            #region 实际消息处理

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

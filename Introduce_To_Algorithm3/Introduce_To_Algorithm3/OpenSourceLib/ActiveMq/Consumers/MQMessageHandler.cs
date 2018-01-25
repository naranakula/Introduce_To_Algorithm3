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
        /// 线程安全集合提供阻塞和限制功能  FIFO  无上限集合
        /// </summary>
        private static readonly BlockingCollection<CommonMessage> MessageQueue = new BlockingCollection<CommonMessage>();

        /// <summary>
        /// 是否正在运行，用来判断程序是否关闭
        /// </summary>
        private static volatile bool _isRunning = true;

        /// <summary>
        /// 消息处理线程
        /// </summary>
        private static volatile Thread _messageHandlerThread = null;

        /// <summary>
        /// 队列中允许最大的数量的消息
        /// </summary>
        private const int MaxNumberDataInQueue = 4096;

        /// <summary>
        /// 批量处理的最大批次
        /// </summary>
        private const int MaxBatchSize = 32;

        /// <summary>
        /// 列表处理的阀值,当队列长度>=该值时,允许启用列表处理
        /// </summary>
        private const int ThresholdCanBeListHandled = 8;


        /// <summary>
        /// 添加到消息队列
        /// </summary>
        /// <param name="message"></param>
        public static void AddToQueue(CommonMessage message)
        {
            try
            {
                if (message == null)
                {
                    return;
                }

                #region 避免queue内存溢出

                int curCount = MessageQueue.Count;

                if (curCount > MaxNumberDataInQueue)
                {
                    CommonMessage delMsg = null;

                    for (int i = 0; i < 5; i++)
                    {
                        if (MessageQueue.TryTake(out delMsg))
                        {
                            NLogHelper.Warn($"丢弃消息:{delMsg}");
                        }
                        else
                        {
                            break;
                        }
                    }

                }

                #endregion

                MessageQueue.Add(message);
            }
            catch
            {
                //没有必要异常处理
            }
        }



        /// <summary>
        /// 初始化消息处理线程
        /// </summary>
        public static void Init()
        {
            lock (MessageQueue)
            {

                if (_messageHandlerThread != null)
                {
                    return;
                }

                //设置为运行
                _isRunning = true;
            }



            _messageHandlerThread = new Thread(() =>
            {
                while (_isRunning)
                {
                    try
                    {
                        int curCount = MessageQueue.Count;

                        if (curCount > ThresholdCanBeListHandled)
                        {
                            int batchSize = ThresholdCanBeListHandled;
                            if (curCount > MaxBatchSize)
                            {
                                batchSize = MaxBatchSize;
                            }

                            List<CommonMessage> msgList = new List<CommonMessage>(batchSize);

                            CommonMessage message = null;
                            for (int i = 0; i < batchSize; i++)
                            {
                                if(MessageQueue.TryTake(out message))
                                {
                                    msgList.Add(message);
                                }
                                else
                                {
                                    break;
                                }
                            }


                            try
                            {
                                HandleListMessage(msgList);
                            }
                            catch (Exception ex)
                            {
                                NLogHelper.Error("HandleListMessage处理消息失败,异常信息:" + ex);
                            }
                        }
                        else
                        {
                            CommonMessage message = null;
                            //等待517ms获取消息
                            if (MessageQueue.TryTake(out message, 517))
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
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Error("处理消息失败,异常信息:" + ex);

                    }
                }
            });

            //后台线程
            _messageHandlerThread.IsBackground = true;
            //消息优先级
            _messageHandlerThread.Priority = ThreadPriority.Normal;

            _messageHandlerThread.Start();

            NLogHelper.Info("初始化消息处理线程成功");

            return;
        }

        /// <summary>
        /// 处理列表消息
        /// </summary>
        /// <param name="msgList"></param>
        private static void HandleListMessage(List<CommonMessage> msgList)
        {
            if(msgList == null || msgList.Count == 0)
            {
                return;
            }

            #region 实际消息处理

            #endregion
        }

        /// <summary>
        /// 实际的消息处理
        /// </summary>
        /// <param name="message"></param>
        public static void HandleMessage(CommonMessage message)
        {
            if (message == null)
            {
                return;
            }

            #region 实际消息处理

            #endregion
        }


        /// <summary>
        /// 停止消息处理线程
        /// </summary>
        public static void Stop()
        {
            lock (MessageQueue)
            {
                _isRunning = false;
            }

            //没有必要dispose MessageQueue
        }

    }


}

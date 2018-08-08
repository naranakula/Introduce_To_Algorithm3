using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils.ConcurrentCollections;
using Introduce_To_Algorithm3.Common.Utils.strings;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.Producers
{


    /// <summary>
    /// 消息处理器
    /// 使用方式
    /// 
    /// Add()
    /// Add()
    /// ....
    /// 
    /// 
    /// Stop()
    /// 
    /// </summary>
    public static class RMqProducerMessageHandler
    { 
        /// <summary>
        /// 底层消息处理
        /// 构造时初始化底层线程
        /// </summary>
        private static readonly BlockingQueueEx<RMqSendMessage> BlockingQueue = new BlockingQueueEx<RMqSendMessage>(singleDataHandler: SingleDataHandler, dataListHandler: DataListHandler, exceptionHandler: ExceptionHandler,isNeedOptimize:false);

        /// <summary>
        /// 列表发送
        /// </summary>
        /// <param name="msgList"></param>
        private static void DataListHandler(List<RMqSendMessage> msgList)
        {
            if (LongRunRabbitProducer.IsAlive())
            {
                bool result = LongRunRabbitProducer.SendMessage(msgList, ex =>
                {
                    NLogHelper.Error($"{msgList.Count}条消息处理失败:{ex}");
                });

                if (result)
                {

                }
                else
                {

                }

            }
            else
            {
                NLogHelper.Error($"mq连接异常，{msgList.Count}条消息发送失败");
            }
        }

        /// <summary>
        /// 添加待处理的消息
        /// </summary>
        /// <param name="item"></param>
        public static void Add(RMqSendMessage item)
        {
            if (item == null)
            {
                return;
            }

            BlockingQueue.Add(item, abandonAction: AbandonMessageHandler);
        }

        /// <summary>
        /// 消息丢弃
        /// </summary>
        /// <param name="obj"></param>
        private static void AbandonMessageHandler(RMqSendMessage obj)
        {
            NLogHelper.Warn($"存在消息丢弃,消息id={obj.Id}");
        }

        /// <summary>
        /// 单个消息处理
        /// </summary>
        /// <param name="message"></param>
        private static void SingleDataHandler(RMqSendMessage message)
        {
            if (LongRunRabbitProducer.IsAlive())
            {
                bool result = LongRunRabbitProducer.SendMessage(message, ex =>
                {
                    NLogHelper.Error($"id={message.Id}的消息处理失败:{ex}");
                });

                if (result)
                {

                }
                else
                {
                    
                }

            }
            else
            {
                NLogHelper.Error($"mq连接异常，消息id={message.Id}的消息发送失败");
            }
        }

       

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="ex"></param>
        private static void ExceptionHandler(Exception ex)
        {
            NLogHelper.Error($"消息处理异常:{ex}");
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            BlockingQueue.Stop();
        }
    }
}

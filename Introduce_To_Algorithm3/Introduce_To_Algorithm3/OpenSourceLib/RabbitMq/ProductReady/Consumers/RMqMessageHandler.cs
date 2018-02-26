using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils.ConcurrentCollections;
using Introduce_To_Algorithm3.Common.Utils.strings;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.Consumers
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
    public static class RMqMessageHandler
    {


        /// <summary>
        /// 底层消息处理
        /// 构造时初始化底层线程
        /// </summary>
        private static readonly BlockingQueueEx<RMqMessage> _blockingQueue = new BlockingQueueEx<RMqMessage>(singleDataHandler:SingleDataHandler,dataListHandler:ListDataHandler,exceptionHandler:ExceptionHandler);

        /// <summary>
        /// 添加待处理的消息
        /// </summary>
        /// <param name="item"></param>
        public static void Add(RMqMessage item)
        {
            if (item == null)
            {
                return;
            }

            _blockingQueue.Add(item,abandonAction:AbandonMessageHandler);
        }

        /// <summary>
        /// 消息丢弃
        /// </summary>
        /// <param name="obj"></param>
        private static void AbandonMessageHandler(RMqMessage obj)
        {
            NLogHelper.Warn($"存在消息丢弃,base64={StringUtils.ToBase64String(obj.ContentBytes)}");
        }

        /// <summary>
        /// 单个消息处理
        /// </summary>
        /// <param name="message"></param>
        private static void SingleDataHandler(RMqMessage message)
        {
        }

        /// <summary>
        /// 列表消息处理
        /// </summary>
        /// <param name="msgList"></param>
        private static void ListDataHandler(List<RMqMessage> msgList)
        {
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
            _blockingQueue.Stop();
        }
    }


}

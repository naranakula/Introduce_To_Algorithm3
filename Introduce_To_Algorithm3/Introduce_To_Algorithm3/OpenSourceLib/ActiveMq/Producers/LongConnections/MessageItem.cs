using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq.Producers.LongConnections
{
    /// <summary>
    /// 消息数据项
    /// </summary>
    public class MessageItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public MessageItem(string message)
        {
            this.Message = message;
            CreateTime = DateTime.Now;
            TryCount = 0;
        }


        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 消息处理尝试的次数
        /// </summary>
        public int TryCount { get; set; }
    }
}

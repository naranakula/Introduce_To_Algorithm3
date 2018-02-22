using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.Consumers
{
    /// <summary>
    /// 消息类
    /// </summary>
    public class RMqMessage
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public byte[] ContentBytes { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}

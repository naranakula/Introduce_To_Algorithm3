using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.SocketClients
{
    /// <summary>
    /// 接收或者发送的消息
    /// </summary>
    public class MessageItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageItem()
        {
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
    }
}

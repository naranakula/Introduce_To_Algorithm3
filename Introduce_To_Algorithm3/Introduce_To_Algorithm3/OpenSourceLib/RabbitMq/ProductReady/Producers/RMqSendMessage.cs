using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.Producers
{
    /// <summary>
    /// 发送的消息
    /// </summary>
    public class RMqSendMessage
    {

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();

        private string _id;
        private byte[] _contentBytes;
        private string _exchangeName;
        private string _routingKey;
        private DateTime _createTime;

        /// <summary>
        /// 消息Id
        /// </summary>
        public string Id
        {
            get { lock (_locker) { return _id; } }
            set { lock (_locker) { _id = value; } }
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public byte[] ContentBytes
        {
            get { lock (_locker) { return _contentBytes; } }
            set { lock (_locker) { _contentBytes = value; } }
        }

        /// <summary>
        /// 发送的exchange交换器的名字
        /// </summary>
        public string ExchangeName
        {
            get { lock (_locker) { return _exchangeName; } }
            set { lock (_locker) { _exchangeName = value; } }
        }

        /// <summary>
        /// 发送的路由键
        /// </summary>
        public string RoutingKey
        {
            get { lock (_locker) { return _routingKey; } }
            set { lock (_locker) { _routingKey = value; } }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { lock (_locker) { return _createTime; } }
            set { lock (_locker) { _createTime = value; } }
        }
    }
}

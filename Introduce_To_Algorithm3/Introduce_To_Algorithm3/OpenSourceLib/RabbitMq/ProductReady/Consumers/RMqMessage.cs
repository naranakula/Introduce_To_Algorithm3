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
        private string _id;
        private byte[] _contentBytes;
        private DateTime _createTime;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();

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
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { lock (_locker) { return _createTime; } }
            set { lock (_locker) { _createTime = value; } }
        }
    }
}

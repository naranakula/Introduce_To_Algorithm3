using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetCommsConsole.NetComms.DPSBase;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// Contains information required to send, receive and correctly rebuild any objects sent via NetworkComms.Net.
    /// Any data sent via NetworkCommsDotNet is always preceded by a packetHeader.
    /// 每个消息都有一个消息头
    /// </summary>
    public sealed class PacketHeader : IExplicitlySerialize
    {

        /// <summary>
        /// 数据包头部配置
        /// </summary>
        private Dictionary<PacketHeaderLongItems, long> longItems;

        /// <summary>
        /// 数据包头部配置
        /// </summary>
        private Dictionary<PacketHeaderStringItems, string> stringItems;

        /// <summary>
        /// 构造函数
        /// </summary>
        private PacketHeader() { }


        #region 接口实现

        public void Serialize(Stream outputStream)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream inputStream)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

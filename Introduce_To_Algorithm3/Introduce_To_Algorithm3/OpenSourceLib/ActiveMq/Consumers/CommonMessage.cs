using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq
{
    /// <summary>
    /// 通用消息
    /// </summary>
    public class CommonMessage
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public CommonMessageType MessageType { get; set; }

        /// <summary>
        /// 文本消息
        /// </summary>
        public string TextMessage { get; set; }

        /// <summary>
        /// 字节数组消息
        /// </summary>
        public byte[] BytesMessage { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// ToString覆盖
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (MessageType == CommonMessageType.Text)
            {
                return TextMessage == null ? string.Empty : TextMessage;
            }
            else if (MessageType == CommonMessageType.Bytes)
            {
                return "BytesMessage";
            }
            else
            {
                return "";
            }
        }
    }


    public enum CommonMessageType
    {
        Text,//文本
        Bytes//字节数组
    }

}

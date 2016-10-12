using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCommsConsole.NetComms.Tools;

namespace NetCommsConsole.NetComms
{
    /// <summary>
    /// Contains options and flags for sending and receiving data such as serialisation method, data processors, encryption etc.
    /// Several static constructors are provided to help create SendReceiveOptions in the most common formats.
    /// </summary>
    public class SendReceiveOptions
    {
        #region 私有变量

        /// <summary>
        /// Gets the options that should be passed to the <see cref="DPSBase.DataSerializer"/> and <see cref="DPSBase.DataProcessor"/>s on object serialization and deserialization
        /// </summary>
        private Dictionary<string, string> options;

        #region 常量
        /// <summary>
        /// 常量字符串 接收确认的常量
        /// </summary>
        private const string ReceiveConfirmationRequiredStr = "ReceiveConfirmationRequired";

        #endregion

        #endregion

        #region 公有变量

        /// <summary>
        /// if true, and packets send with with <see cref="SendReceiveOptions"/> will forced to trigger a receive confirmation
        /// </summary>
        public bool ReceiveConfirmationRequired
        {
            get { return options.ContainsKey(ReceiveConfirmationRequiredStr); }
            set
            {
                if (value)
                {
                    options[ReceiveConfirmationRequiredStr] = string.Empty;
                }
                else
                {
                    options.Remove(ReceiveConfirmationRequiredStr);
                }
            }
        }

        /// <summary>
        /// IncludePacketConstructionTime的字符串常量
        /// </summary>
        private const string IncludePacketConstructionTimeStr = "IncludePacketConstructionTime";

        /// <summary>
        /// If true any packets sent with this options will include the packet creation time in the header
        /// </summary>
        public bool IncludePacketConstructionTime
        {
            get { return options.ContainsKey(IncludePacketConstructionTimeStr); }
            set
            {
                if (value)
                {
                    options[IncludePacketConstructionTimeStr] = string.Empty;
                }
                else
                {
                    options.Remove(IncludePacketConstructionTimeStr);
                }
            }
        }

        /// <summary>
        /// UseNestedPacketType的常量字符串
        /// </summary>
        private const string UseNestedPacketTypeStr = "UseNestedPacketType";

        /// <summary>
        /// If true any packets sent with this <see cref="SendReceiveOptions"/> will be nested which can be used to obscure(隐藏) the actual packet type.
        /// </summary>
        public bool UseNestedPacket
        {
            get { return options.ContainsKey(UseNestedPacketTypeStr); }
            set
            {
                if (value)
                {
                    options[UseNestedPacketTypeStr] = "";
                }
                else
                {
                    options.Remove(UseNestedPacketTypeStr);
                }
            }
        }

        /// <summary>
        /// ReceiveHandlePriority的常量字符串
        /// </summary>
        private const string ReceiveHandlePriorityStr = "ReceiveHandlePriority";

        /// <summary>
        /// 接收处理优先级
        /// Incoming packets are handled using a flexible QueueItemPriority. Reserved internal packet types and packets marked with QueueItemPriority.Highest are not enqueued but handled in real Time by thread handling the incoming data. You are free to specify the queue item priorit for packer handlers using this SendReceiveOptions by setting this value as desired. CAUTION: Only use QueueItemPriority.Highest Sparingly.
        /// </summary>
        public QueueItemPriority ReceiveHandlePriority
        {
            get
            {
                if (options.ContainsKey(ReceiveHandlePriorityStr))
                {
                    return (QueueItemPriority) Enum.Parse(typeof (QueueItemPriority), options[ReceiveHandlePriorityStr]);
                }
                else
                {
                    return QueueItemPriority.Normal;
                }
            }
            set { options[ReceiveHandlePriorityStr] = Enum.GetName(typeof (QueueItemPriority), value); }
        }


        


        /// <summary>
        /// Gets the options that should be passed to the <see cref="DPSBase.DataSerializer"/> and <see cref="DPSBase.DataProcessor"/>s on object serialization and deserialization
        /// </summary>
        public Dictionary<string, string> Options
        {
            get { return options; }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms.Tools
{
    /// <summary>
    /// Wrapper used to track the SendReceiveOptions for different packet types
    /// </summary>
    public class PacketTypeUnwrapper
    {
        /// <summary>
        /// 数据包类型
        /// </summary>
        private string packetTypeStr;

        /// <summary>
        /// The SendReceiveOptions associated with this unwrapper.
        /// </summary>
        public SendReceiveOptions Options { get; private set; }

        /// <summary>
        /// Initialise a new PacketTypeUnwrapper
        /// </summary>
        /// <param name="packetTypeStr">The packet type of this unwrapper</param>
        /// <param name="options">The SendReceiveOptions to use with this unwrapper</param>
        public PacketTypeUnwrapper(string packetTypeStr, SendReceiveOptions options)
        {
            this.packetTypeStr = packetTypeStr;
            this.Options = options;
        }
    }
}

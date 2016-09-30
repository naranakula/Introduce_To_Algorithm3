using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms
{
    /// <summary>
    /// 连接类型
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// 无定义
        /// </summary>
        Undefined,

        /// <summary>
        /// A TCP connection type. TCPConnection
        /// </summary>
        TCP,

        /// <summary>
        /// A UDP connection type.UDPConnection
        /// </summary>
        UDP,

        /// <summary>
        /// A Bluetooth RFCOMM connection. BluetoothConnection 
        /// </summary>
        Bluetooth
    }
}

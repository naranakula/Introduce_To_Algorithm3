using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms
{
    /// <summary>
    /// 连接状态
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// 连接状态未定义，连接的初始状态
        /// </summary>
        Undefined,

        /// <summary>
        /// 连接正在建立
        /// </summary>
        Establishing,

        /// <summary>
        /// 连接建立
        /// </summary>
        Established,

        /// <summary>
        /// 连接关闭
        /// </summary>
        Shutdown
    }
}

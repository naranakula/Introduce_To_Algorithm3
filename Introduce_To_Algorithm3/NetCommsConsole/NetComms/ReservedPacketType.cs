using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms
{
    /// <summary>
    /// 预留的数据包类型
    /// </summary>
    internal enum ReservedPacketType
    {
        Confirmation,//确认
        CheckSumFailResend,//校验码错误
        AliveTestPacket,//检查连接是否存活
        ConnectionSetup,//连接建立
        Unmanaged,//未管理
        NestedPacket
    }
}

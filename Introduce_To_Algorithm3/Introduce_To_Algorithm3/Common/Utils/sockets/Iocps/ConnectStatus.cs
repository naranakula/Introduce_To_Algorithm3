using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// 连接状态
    /// </summary>
    public enum ConnectStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        SUCCESS = 0,

        /// <summary>
        /// 超时
        /// </summary>
        FAIL_TIME_OUT,

        /// <summary>
        /// Failed due to connection already exist
        /// </summary>
        FAIL_ALREADY_CONNECTED,

        /// <summary>
        /// Failed due to unknown error
        /// </summary>
        FAIL_SOCKET_ERROR
    }
}

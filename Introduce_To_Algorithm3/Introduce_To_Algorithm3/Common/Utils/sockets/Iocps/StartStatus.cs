using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// Server start status
    /// </summary>
    public enum StartStatus
    {
        /// <summary>
        /// Success
        /// </summary>
        SUCCESS=0,

        /// <summary>
        /// Failed due to server already started
        /// 服务器已经开启了
        /// </summary>
        FAIL_ALREADY_STARTED,

        /// <summary>
        /// Failed due to socket error
        /// </summary>
        FAIL_SOCKET_ERROR
    }
}

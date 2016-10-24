using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// Send status
    /// </summary>
    public enum SendStatus:uint
    {
        /// <summary>
        /// Success
        /// </summary>
        SUCCESS = 0,

        /// <summary>
        /// Failed due to socket error
        /// </summary>
        FAIL_SOCKET_ERROR,

        /// <summary>
        /// Failed due to no connection exists
        /// </summary>
        FAIL_NOT_CONNECTED,

        /// <summary>
        /// Failed due to invalid packet
        /// </summary>
        FAIL_INVALID_PACKET,

        /// <summary>
        /// Failed due to connection closing
        /// </summary>
        FAIL_CONNECTION_CLOSING
    }
}

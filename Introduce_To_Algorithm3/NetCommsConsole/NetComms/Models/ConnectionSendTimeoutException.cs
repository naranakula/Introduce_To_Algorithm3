using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// A connection send has timed out
    /// </summary>
    [Serializable]
    public class ConnectionSendTimeoutException:CommsException
    {
        /// <summary>
        /// Create a new instance of ConnectionSendTimeoutException
        /// </summary>
        public ConnectionSendTimeoutException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of ConnectionSendTimeoutException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public ConnectionSendTimeoutException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of ConnectionSendTimeoutException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public ConnectionSendTimeoutException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ConnectionSendTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// an error occurred while trying to establish a connection
    /// </summary>
    [Serializable]
    public class ConnectionShutdownException:CommsException
    {
        /// <summary>
        /// Create a new instance of ConnectionShutdownException
        /// </summary>
        public ConnectionShutdownException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of ConnectionShutdownException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public ConnectionShutdownException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of ConnectionShutdownException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public ConnectionShutdownException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ConnectionShutdownException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}

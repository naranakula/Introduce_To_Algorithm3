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
    public class ConnectionSetupException:CommsException
    {
        /// <summary>
        /// Create a new instance of ConnectionSetupException
        /// </summary>
        public ConnectionSetupException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of ConnectionSetupException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public ConnectionSetupException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of ConnectionSetupException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public ConnectionSetupException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ConnectionSetupException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

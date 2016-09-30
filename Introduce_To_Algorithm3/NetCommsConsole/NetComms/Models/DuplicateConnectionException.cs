using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// A possible duplicate connection has been detected.
    /// </summary>
    [Serializable]
    public class DuplicateConnectionException:CommsException
    {
        /// <summary>
        /// Create a new instance of DuplicateConnectionException
        /// </summary>
        public DuplicateConnectionException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of DuplicateConnectionException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public DuplicateConnectionException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of DuplicateConnectionException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public DuplicateConnectionException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DuplicateConnectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

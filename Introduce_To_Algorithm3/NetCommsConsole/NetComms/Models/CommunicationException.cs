using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// An error occurred while during communication which does not fall under other exception cases
    /// </summary>
    [Serializable]
    public class CommunicationException:CommsException
    {

        /// <summary>
        /// Create a new instance of CommunicationException
        /// </summary>
        public CommunicationException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of CommunicationException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public CommunicationException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of CommunicationException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public CommunicationException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CommunicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// A timeout has occurred while waiting for a confirmation packet to be received. Check for errors and or consider increasing NetworkComms.PacketConfirmationTimeoutMS
    /// </summary>
    public class ConfirmationTimeoutException:CommsException
    {
        /// <summary>
        /// Create a new instance of ConfirmationTimeoutException
        /// </summary>
        public ConfirmationTimeoutException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of ConfirmationTimeoutException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public ConfirmationTimeoutException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of ConfirmationTimeoutException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public ConfirmationTimeoutException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ConfirmationTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

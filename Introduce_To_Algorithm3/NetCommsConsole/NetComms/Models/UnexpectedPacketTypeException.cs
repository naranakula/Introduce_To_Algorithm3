using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// An unexpected incoming packetType has been received. Consider setting NetworkComms.IgnoreUnknownPacketTypes to true to prevent this exception.
    /// </summary>
    [Serializable]
    public class UnexpectedPacketTypeException:CommsException
    {
        /// <summary>
        /// Create a new instance of UnexpectedPacketTypeException
        /// </summary>
        public UnexpectedPacketTypeException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of UnexpectedPacketTypeException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public UnexpectedPacketTypeException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of UnexpectedPacketTypeException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public UnexpectedPacketTypeException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnexpectedPacketTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// An error occurred during a packetType data handler execution.
    /// </summary>
    [Serializable]
    public class PacketHandlerException:CommsException
    {
        /// <summary>
        /// Create a new instance of PacketHandlerException
        /// </summary>
        public PacketHandlerException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of PacketHandlerException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public PacketHandlerException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of PacketHandlerException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public PacketHandlerException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PacketHandlerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

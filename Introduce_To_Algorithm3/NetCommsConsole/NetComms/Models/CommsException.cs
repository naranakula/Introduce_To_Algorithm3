using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// Base Exception.
    /// All connecion related exceptions can be caught in a single catch block by using catch (<see cref="CommsException"/>)(<see cref="CommsException"/>)
    /// </summary>
    [Serializable]
    public abstract class CommsException:Exception
    {
        /// <summary>
        /// Create a new instance of CommsException
        /// </summary>
        public CommsException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of CommsException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public CommsException(string msg)
            : base(msg)
        {

        }

        /// <summary>
        /// Create a new instance of CommsException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public CommsException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CommsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

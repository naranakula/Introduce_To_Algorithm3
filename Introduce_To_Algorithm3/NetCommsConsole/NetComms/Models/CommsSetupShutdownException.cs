using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// An error occurred while trying to setup or shutdown NetworkComms.Net
    /// </summary>
    [Serializable]
    public class CommsSetupShutdownException:CommsException
    {
        /// <summary>
        /// Create a new instance of CommsSetupShutdownException
        /// </summary>
        public CommsSetupShutdownException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of CommsSetupShutdownException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public CommsSetupShutdownException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of CommsSetupShutdownException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public CommsSetupShutdownException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CommsSetupShutdownException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

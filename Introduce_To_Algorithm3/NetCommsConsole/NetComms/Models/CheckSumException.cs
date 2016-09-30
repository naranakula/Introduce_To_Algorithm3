using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    ///  A checksum error has occurred. NetworkComms.EnablePacketCheckSumValidation must be set to true for this exception to be thrown.
    /// </summary>
    [Serializable]
    public class CheckSumException:CommsException
    {
        /// <summary>
        /// Create a new instance of CheckSumException
        /// </summary>
        public CheckSumException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of CheckSumException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public CheckSumException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of CheckSumException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public CheckSumException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CheckSumException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

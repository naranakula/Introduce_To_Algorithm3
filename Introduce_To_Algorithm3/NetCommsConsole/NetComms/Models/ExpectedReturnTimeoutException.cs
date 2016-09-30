using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    ///  A timeout has occurred while waiting for an expected return object. Check for errors and or consider increasing the provided return timeout value.
    /// </summary>
    [Serializable]
    public class ExpectedReturnTimeoutException:CommsException
    {
        /// <summary>
        /// Create a new instance of ExpectedReturnTimeoutException
        /// </summary>
        public ExpectedReturnTimeoutException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of ExpectedReturnTimeoutException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public ExpectedReturnTimeoutException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of ExpectedReturnTimeoutException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public ExpectedReturnTimeoutException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ExpectedReturnTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}

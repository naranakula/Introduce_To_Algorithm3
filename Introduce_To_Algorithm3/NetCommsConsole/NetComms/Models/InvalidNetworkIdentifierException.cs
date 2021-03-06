﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// An invalid network identifier has been provided
    /// </summary>
    [Serializable]
    public class InvalidNetworkIdentifierException:CommsException
    {

        /// <summary>
        /// Create a new instance of InvalidNetworkIdentifierException
        /// </summary>
        public InvalidNetworkIdentifierException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of InvalidNetworkIdentifierException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public InvalidNetworkIdentifierException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of InvalidNetworkIdentifierException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public InvalidNetworkIdentifierException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidNetworkIdentifierException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

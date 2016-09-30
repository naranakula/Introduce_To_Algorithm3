using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// An error occurred while trying to serialise/compress or deserialise/uncompress an object.
    /// </summary>
    [Serializable]
    public class SerialisationException:CommsException
    {
        /// <summary>
        /// Create a new instance of SerialisationException
        /// </summary>
        public SerialisationException()
            : base()
        {
        }

        /// <summary>
        /// Create a new instance of SerialisationException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public SerialisationException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Create a new instance of SerialisationException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        /// <param name="innerException">An associated inner exception</param>
        public SerialisationException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }


        /// <summary>
        /// Constructor required by the runtime and by .NET programming conventions
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SerialisationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

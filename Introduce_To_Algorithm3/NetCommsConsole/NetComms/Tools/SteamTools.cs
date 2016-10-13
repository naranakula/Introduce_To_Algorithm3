using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms.Tools
{
    /// <summary>
    /// Wrapper class for writing to streams with timeouts. used primarily to prevent strem write deadlocks.
    /// </summary>
    public static class SteamTools
    {

        /// <summary>
        /// write the provided sendBuffer to the destination stream in chunks of writeBufferSize. throws exception is any write takes longer than timeoutPerByteWriteMS
        /// </summary>
        /// <param name="sendBuffer">buffer containing data to write</param>
        /// <param name="inputStart">the start position in sendBuffer</param>
        /// <param name="bufferLength">the number of bytes to write</param>
        /// <param name="destinationStream">the destination stream</param>
        /// <param name="writeBufferSize">the size in bytes of each successive write</param>
        /// <param name="timeoutMSPerKBWrite">the maximum time to allow for write to complete per KB</param>
        /// <param name="minTimeoutMS">the minimum time to allow for any sized write</param>
        /// <returns></returns>
        public static double Write(byte[] sendBuffer, int inputStart, int bufferLength,
            ThreadSafeStream destinationStream, int writeBufferSize, double timeoutMSPerKBWrite, int minTimeoutMS)
        {
            throw new Exception();
        }

    }
}

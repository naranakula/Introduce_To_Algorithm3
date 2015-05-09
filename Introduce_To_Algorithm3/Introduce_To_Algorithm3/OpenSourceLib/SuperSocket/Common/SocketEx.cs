using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.SuperSocket.Common
{
    /// <summary>
    /// Socket extension class
    /// </summary>
    public static class SocketEx
    {
        /// <summary>
        /// Close the socket safely.
        /// </summary>
        /// <param name="socket">The socket.</param>
        public static void SafeClose(this Socket socket)
        {
            if (socket == null)
            {
                return;
            }

            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
                
            }

            try
            {
                socket.Close();
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// sends the data
        /// </summary>
        /// <param name="socket">the socket</param>
        /// <param name="data">the data</param>
        public static void SendData(this Socket socket, byte[] data)
        {
            socket.Send(data, 0, data.Length, SocketFlags.None);
        }

        /// <summary>
        /// Sends the data
        /// </summary>
        /// <param name="socket">the client</param>
        /// <param name="data">the data</param>
        /// <param name="offset">the offset</param>
        /// <param name="length">the length</param>
        public static void SendData(this Socket socket, byte[] data, int offset, int length)
        {
            socket.Send(data, offset, length, SocketFlags.None);
        }

    }
}

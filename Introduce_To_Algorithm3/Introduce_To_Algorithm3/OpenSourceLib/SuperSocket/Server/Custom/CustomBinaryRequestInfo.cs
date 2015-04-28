using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;

namespace Introduce_To_Algorithm3.OpenSourceLib.SuperSocket.Server.Custom
{
    /// <summary>
    ///   Binary type request information
    /// </summary>
    public class CustomBinaryRequestInfo:BinaryRequestInfo
    {
        public CustomBinaryRequestInfo(string key, byte[] body) : base(key, body)
        {
        }
    }
}

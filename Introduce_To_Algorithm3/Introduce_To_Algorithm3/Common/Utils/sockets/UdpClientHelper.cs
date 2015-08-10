using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets
{
    public class UdpClientHelper
    {
        public void Test()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8001);
            UdpClient client = new UdpClient(endPoint);
            //client.Send();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace Introduce_To_Algorithm3.OpenSourceLib.NetMqs.ReqRep
{
    public class NetClient
    {
        public static void Start()
        {
            using (var client = new RequestSocket())
            {
                client.Connect("tcp://localhost:5555");
                while (true)
                {
                    string strIn = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(strIn))
                    {
                        continue;
                    }

                    client.SendFrame(strIn);
                    var message = client.ReceiveFrameString();
                    Console.WriteLine("接收到："+message);
                }
            }
        }
    }
}

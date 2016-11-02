using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace Introduce_To_Algorithm3.OpenSourceLib.NetMqs.ReqRep
{
    public class NetMQRepServer
    {
        /// <summary>
        /// 请求响应模式
        /// 一个请求必须对应一个回应
        /// </summary>
        public static void Start()
        {
            using (var server = new ResponseSocket())
            {
                server.Bind("tcp://*:5555");
                while (true)
                {
                    var message = server.ReceiveFrameString();
                    Console.WriteLine("接收到："+message);
                    //经过测试，发送到了特定的客户端中，而不是群发
                    //Response socket always reply to the last client that sends a message.
                    server.SendFrame(DateTime.Now.ToString());
                }
            }
        }
    }
}

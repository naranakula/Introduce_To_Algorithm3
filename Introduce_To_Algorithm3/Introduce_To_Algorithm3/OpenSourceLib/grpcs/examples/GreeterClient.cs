using com.cmlu.services;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.grpcs.examples
{
    /// <summary>
    /// grpc的客户端，需要使用nuget添加grpc
    /// </summary>
    public static class GreeterClient
    {
        public static void TestMain(string[] args)
        {
            //var channelCredentials = new SslCredentials(File.ReadAllText("roots.pem"));
            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            var client = new Greeter.GreeterClient(channel);

            for (int i = 0; i < 10; i++)
            {
                var reply = client.SayHello(new Request() { Request_ = "Hello" });
                Console.WriteLine(reply.Response_);
            }
            //关闭信道
            channel.ShutdownAsync().Wait();
            
        }
    }
}

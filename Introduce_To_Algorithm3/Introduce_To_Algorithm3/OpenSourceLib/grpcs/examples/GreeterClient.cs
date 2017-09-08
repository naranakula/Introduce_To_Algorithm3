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
        public static async void TestMain(string[] args)
        {
            //var channelCredentials = new SslCredentials(File.ReadAllText("roots.pem"));
            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            var client = new Greeter.GreeterClient(channel);

            #region Unary RPC

            for (int i = 0; i < 10; i++)
            {
                //同步调用，只用unary rpc有同步调用，其它三种全是异步
                var reply = client.SayHello(new Request() { Request_ = "Hello" });
                Console.WriteLine(reply.Response_);
                //异步调用
                Response response = await client.SayHelloAsync(new Request());
            }

            #endregion

            #region Server streaming RPC

            //同步调用，大的数据分多次传输
            using (var serverStreamCall = client.SayHelloStreamServer(new Request() { Request_ = "" }))
            {
                var responseStream = serverStreamCall.ResponseStream;
                while (await responseStream.MoveNext())
                {
                    //读取当前返回值
                    Response response = responseStream.Current;
                    //TODO：处理响应
                }
            }

            #endregion

            #region Client streaming RPC

            using (var call = client.SayHelloStreamClient())
            {
                List<Request> list = new List<Request>();
                list.Add(new Request() { Request_ = "1"});
                list.Add(new Request() { Request_ = "2" });
                list.Add(new Request() { Request_ = "3" });
                foreach (var request in list)
                {
                    //发送多个消息
                    await call.RequestStream.WriteAsync(request);
                }

                //通知服务器写完成，没有更多的写操作
                await call.RequestStream.CompleteAsync();

                //读取响应
                Response response = await call.ResponseAsync;
            }

            #endregion

            #region Bi-directional streaming 

            using (var call = client.SayHelloStreamBidirectional())
            {
                //读和写可以分开线程执行，但读本身写本身不能多线程
                //在新线程开启读任务
                var readTask = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        //读取一个响应
                        var response = call.ResponseStream.Current;
                        //TODO: 处理响应
                    }
                });

                List<Request> list = new List<Request>();
                list.Add(new Request() { Request_ = "1" });
                list.Add(new Request() { Request_ = "2" });
                list.Add(new Request() { Request_ = "3" });
                foreach (var request in list)
                {
                    //发送多个消息
                    await call.RequestStream.WriteAsync(request);
                }

                //通知服务器写完成，没有更多的写操作
                await call.RequestStream.CompleteAsync();
                
                //等待读线程结束
                await readTask;
            }

            #endregion


            //关闭信道
            channel.ShutdownAsync().Wait(5000);

        }
    }
}

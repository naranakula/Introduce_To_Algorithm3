using com.cmlu.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using System.Net;
using System.Threading;

namespace Introduce_To_Algorithm3.OpenSourceLib.grpcs.examples
{

    //需要使用Nuget添加Grpc和Grpc.Tools
    /// <summary>
    /// GRPC服务器端接口实现
    /// 
    /// Unary只能传输少量数据，传输大数据使用stream流式接口,使用了不小于5GB的流量做了测试了7天7夜
    /// 服务实现类只创建一个实例的，即Greeter.BindService(new GreeterServiceImpl())时创建的实例,已验证
    /// 
    /// 服务器端尽量不要抛出异常，而是通过try catch，设置result_code result_desc来反映异常
    /// </summary>
    public class GreeterServiceImpl:Greeter.GreeterBase
    {
        public GreeterServiceImpl()
        {
            Console.WriteLine("创建了一个服务器实现实例");
        }

        /// <summary>
        ///Unary RPC  覆盖基类方法
        /// 一般不加async
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<Response> SayHello(Request request, ServerCallContext context)
        {
            //不要调用基类的方法，基类仅仅抛出异常
            //base.SayHello(request, context);


            Console.WriteLine(request.Request_);
            
            return Task.FromResult(new Response() {Response_ = "Hello at " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss")});


            //return Task.Factory.StartNew(()=> {
            //    return new Response() { Response_ = "Hello at " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") };
            //});
        }

        /// <summary>
        /// Server streaming RPC
        /// 可能需要手动加async
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task SayHelloStreamServer(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            //不要调用基类的方法，基类仅仅抛出异常
            //return base.SayHelloStreamServer(request, responseStream, context);


            for (int i = 0; i < 1000 * 1000 * 1000; i++)
            {
                //测试1000M=1g大数据流式传输  使用了约5GB的流量做了测试
                var response =new Response() { Response_ = i.ToString() };
                await responseStream.WriteAsync(response);
                if (i % 1000 == 0)
                {
                    Thread.Sleep(200);
                }
            }

            /*
            List<Response> list = new List<Response>();
            for (int i = 0; i < 1000 * 100 * 10000; i++)
            {
                //测试1000M=1g大数据流式传输,考虑到数据太多，一次性读到内存是不可行的
                list.Add(new Response() { Response_ = i.ToString() });
            }
            foreach (var response in list)
            {
                //服务器端返回多个结果
                await responseStream.WriteAsync(response);
            }
            */
        }

        /// <summary>
        /// Client streaming RPC
        /// 可能需要手动加async
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Response> SayHelloStreamClient(IAsyncStreamReader<Request> requestStream, ServerCallContext context)
        {
            //不要调用基类的方法，基类仅仅抛出异常
            //return base.SayHelloStreamClient(requestStream, context);

            while (await requestStream.MoveNext())
            {
                //获取客户端的多个输入，不一定全部读取客户端的输入
                Request curRequest = requestStream.Current;
                //TODO: 处理输入
            }

            //不需要Task.FromResult
            return new Response() {Response_ = "hello"};
        }

        /// <summary>
        /// bidirectional streaming RPC
        /// 可能需要手动加async
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task SayHelloStreamBidirectional(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Response> responseStream,
            ServerCallContext context)
        {
            //不要调用基类的方法，基类仅仅抛出异常
            //return base.SayHelloStreamBidirectional(requestStream, responseStream, context);

            while (await requestStream.MoveNext())
            {
                //获取客户端的多个输入
                Request curRequest = requestStream.Current;
                //TODO: 处理输入,产生输出


                List<Response> list = new List<Response>();
                list.Add(new Response() { Response_ = "1" });
                list.Add(new Response() { Response_ = "2" });
                list.Add(new Response() { Response_ = "3" });

                foreach (var response in list)
                {
                    //服务器端返回多个结果
                    await responseStream.WriteAsync(response);
                }



            }
        }
    }

    /// <summary>
    /// 启动并运行grpc
    /// </summary>
    public class GreeterServer
    {
        /// <summary>
        /// 绑定的端口
        /// </summary>
        const int Port = 50051;


        /// <summary>
        /// Main方法
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {

            var options = new List<ChannelOption>()
            {
                new ChannelOption(ChannelOptions.MaxSendMessageLength,32*1024*1024),//最大可以发送的消息长度
                new ChannelOption(ChannelOptions.MaxReceiveMessageLength,16*1024*1024),//最大允许接收的消息长度
                new ChannelOption(ChannelOptions.MaxConcurrentStreams,1024),//最大允许的并发连接
                new ChannelOption(ChannelOptions.SoReuseport,1),//重用端口
            };

            //Server可以服务多个services，绑定多个端口
            Server server = new Server(options)
            {
                //可以注册多个service
                Services = { Greeter.BindService(new GreeterServiceImpl()), },
                //可以注册多个端口
                //0.0.0.0监听在本机的所有IP地址
                Ports = { new ServerPort(IPAddress.Any.ToString()/*0.0.0.0*/, Port, ServerCredentials.Insecure/*没有安全验证*/) },

            };
            
            //启动后后面的代码继续执行
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");

            Console.ReadLine();

            //关闭服务
            server.ShutdownAsync().Wait(8000);
        }
    }

}

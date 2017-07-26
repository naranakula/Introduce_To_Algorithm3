using com.cmlu.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using System.Net;

namespace Introduce_To_Algorithm3.OpenSourceLib.grpcs.examples
{

    //需要使用Nuget添加Grpc和Grpc.Tools
    /// <summary>
    /// GRPC服务器端接口实现
    /// </summary>
    public class GreeterServiceImpl:Greeter.GreeterBase
    {
        /// <summary>
        /// 覆盖基类方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<Response> SayHello(Request request, ServerCallContext context)
        {
            Console.WriteLine(request.Request_);
            return Task.Factory.StartNew(()=> {
                return new Response() { Response_ = "Hello at " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") };
            });
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
            //Server可以服务多个services，绑定多个端口
            Server server = new Server
            {
                //可以注册多个service
                Services = { Greeter.BindService(new GreeterServiceImpl()) },
                //可以注册多个端口
                //0.0.0.0监听在本机的所有IP地址
                Ports = { new ServerPort(IPAddress.Any.ToString()/*0.0.0.0*/, Port, ServerCredentials.Insecure/*没有安全验证*/) }
            };
            
            //启动后后面的代码继续执行
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");

            Console.ReadLine();

            //关闭服务
            server.ShutdownAsync().Wait(5000);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace Introduce_To_Algorithm3.OpenSourceLib.grpcs
{
    /// <summary>
    /// Grpc服务器
    /// 
    /// GRPC的C#版本底层通过pinvoke调用的grpc c-based的版本
    /// 服务实现类只创建一个实例的，即Greeter.BindService(new GreeterServiceImpl())时创建的实例,已验证
    /// examples下有完整例子
    /// </summary>
    public class GrpcServer
    {
        /// <summary>
        /// 服务列表
        /// Greeter.BindService(new GreeterServiceImpl())
        /// </summary>
        private List<ServerServiceDefinition> serviceList = null;

        /// <summary>
        /// 服务器端口
        /// </summary>
        private int serverPort;

        /// <summary>
        /// 底层服务
        /// </summary>
        private Server server = null;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="services">服务定义 如:（单个） Greeter.BindService(new GreeterServiceImpl())</param>
        /// <param name="serverPort"></param>
        public GrpcServer(List<ServerServiceDefinition> services,int serverPort)
        {
            this.serverPort = serverPort;
            this.serviceList = services;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns>是否启动成功</returns>
        public bool Start(Action<Exception> exceptionHandler = null)
        {
            try
            {
                var options = new List<ChannelOption>()
                {
                    //Grpc不适合处理大量的数据，处理的数据级别是MB。如果需要传输大的消息，使用stream流式消息多次传输
                    new ChannelOption(ChannelOptions.MaxSendMessageLength,32*1024*1024),//最大可以发送的消息长度
                    new ChannelOption(ChannelOptions.MaxReceiveMessageLength,16*1024*1024),//最大允许接收的消息长度
                    new ChannelOption(ChannelOptions.MaxConcurrentStreams,1024),//最大允许的并发连接
                    new ChannelOption(ChannelOptions.SoReuseport,1),//重用端口
                };

                ////Server可以服务多个services，绑定多个端口
                //Server server = new Server(options)
                //{
                //    //可以注册多个service
                //    Services = { Greeter.BindService(new GreeterServiceImpl()), },
                //    //可以注册多个端口
                //    //0.0.0.0监听在本机的所有IP地址
                //    Ports = { new ServerPort(IPAddress.Any.ToString()/*0.0.0.0*/, Port, ServerCredentials.Insecure/*没有安全验证*/) },

                //};

                //Server可以定义多个服务，绑定多个端口
                server = new Server(options);

                //添加多个服务
                foreach (var serviceItem in serviceList)
                {
                    server.Services.Add(serviceItem);
                }

                //添加多个监听端口
                //0.0.0.0表示监听本机所有ip地址， 没有安全验证
                string ipAny = IPAddress.Any.ToString();
                server.Ports.Add(ipAny, serverPort, ServerCredentials.Insecure);

                server.Start();

                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool  Stop(Action<Exception> exceptionHandler = null)
        {
            if (server == null)
            {
                return true;
            }

            for (int i = 0; i < 2; i++)
            {
                try
                {
                    server.ShutdownAsync().Wait(5500);
                    return true;
                }
                catch (Exception ex)
                {
                    if (i == 1)
                    {
                        exceptionHandler?.Invoke(ex);
                    }
                }
            }

            return false;

        }


    }
}

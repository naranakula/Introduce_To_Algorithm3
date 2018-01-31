using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using com.cmlu.services;
using Grpc.Core;
using Introduce_To_Algorithm3.OpenSourceLib.grpcs.examples;

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
        private readonly List<ServerServiceDefinition> _serviceList = null;

        /// <summary>
        /// 服务器端口
        /// </summary>
        private readonly int _serverPort;

        /// <summary>
        /// 服务器监听端口
        /// </summary>
        public int ServerPort
        {
            get { return _serverPort; }
        }


        /// <summary>
        /// 底层服务
        /// </summary>
        private volatile Server _server = null;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="services">服务定义 如:（单个） Greeter.BindService(new GreeterServiceImpl())</param>
        /// <param name="serverPort"></param>
        public GrpcServer(List<ServerServiceDefinition> services,int serverPort)
        {
            this._serverPort = serverPort;
            this._serviceList = services;
        }

        /// <summary>
        /// 启动服务,没有加密
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
                    //最大允许的Maximum number of concurrent incoming streams to allow on a http2 connection
                    //单个http2连接允许的最大数量的stream
                    new ChannelOption(ChannelOptions.MaxConcurrentStreams,63),//单个连接最大允许的并发流
                    new ChannelOption(ChannelOptions.SoReuseport,1),//重用端口
                    //HTTP/2 默认的 window size 是 64 KB，实际这个值太小了，在 TiKV 里面我们直接设置成 1 GB。
                    //TCP两端都有缓冲区来保存接收的数据，如果满了，那么在缓冲区清空之前不能接收更多的数据
                    //发送也有缓冲区
                    //The official definition of the window size is "the amount of octets that can be transmitted without receiving an acknowledgement from the other side". window size 官方定义是：在未收到对方确认报文时，发送端能发送的字节（八字节）数；​
                    //目前C#没有该设置项,建议设成1M或者512K
                    //new ChannelOption(ChannelOptions.)
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
                _server = new Server(options);

                //添加多个服务
                foreach (var serviceItem in _serviceList)
                {
                    _server.Services.Add(serviceItem);
                }

                
                //添加多个监听端口
                //0.0.0.0表示监听本机所有ip地址， 没有安全验证
                string ipAny = IPAddress.Any.ToString();
                _server.Ports.Add(ipAny, _serverPort, ServerCredentials.Insecure);
                
                _server.Start();

                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        /// <summary>
        /// 使用ssl加密启动服务
        /// 建议使用RSA  密钥长度可以为1024，2048 或者 4096 bits
        /// 可以使用sslgen.cmd每次生成不同的密钥
        /// </summary>
        /// <param name="serverCertFile">共用证书</param>
        /// <param name="privateKeyFile">私钥</param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool StartWithSsl(string serverCertFile="server.crt",string privateKeyFile = "server.key",Action<Exception> exceptionHandler = null)
        {
            try
            {
                
                var options = new List<ChannelOption>()
                {
                    //Grpc不适合处理大量的数据，处理的数据级别是MB。如果需要传输大的消息，使用stream流式消息多次传输
                    new ChannelOption(ChannelOptions.MaxSendMessageLength,32*1024*1024),//最大可以发送的消息长度
                    new ChannelOption(ChannelOptions.MaxReceiveMessageLength,16*1024*1024),//最大允许接收的消息长度
                    //最大允许的Maximum number of concurrent incoming streams to allow on a http2 connection
                    //单个http2连接允许的最大数量的stream
                    new ChannelOption(ChannelOptions.MaxConcurrentStreams,100),
                    new ChannelOption(ChannelOptions.SoReuseport,1),//重用端口，默认值就是1
                    //HTTP/2 默认的 window size 是 64 KB，实际这个值太小了，在 TiKV 里面我们直接设置成 1 GB。
                    //TCP两端都有缓冲区来保存接收的数据，如果满了，那么在缓冲区清空之前不能接收更多的数据
                    //发送也有缓冲区
                    //The official definition of the window size is "the amount of octets that can be transmitted without receiving an acknowledgement from the other side". window size 官方定义是：在未收到对方确认报文时，发送端能发送的字节（八字节）数；​
                    //目前C#没有该设置项,建议设成1M或者512K
                    //new ChannelOption(ChannelOptions.)
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
                _server = new Server(options);

                //添加多个服务
                foreach (var serviceItem in _serviceList)
                {
                    _server.Services.Add(serviceItem);
                }

                #region ssl加密

                //证书链PEM encoded certificate chain  包含公钥 有效期 签发者等信息，但不包含私钥
                string serverCert = File.ReadAllText(serverCertFile, Encoding.UTF8);
                //私钥PEM encoded private key  不能公开，只有服务器上有
                string privateKey = File.ReadAllText(privateKeyFile, Encoding.UTF8);


                var keyPair = new KeyCertificatePair(serverCert, privateKey);

                var sslCredentials = new SslServerCredentials(new List<KeyCertificatePair>(){keyPair});

                #endregion

                //添加多个监听端口
                //0.0.0.0表示监听本机所有ip地址， 没有安全验证
                string ipAny = IPAddress.Any.ToString();
                //ssl证书
                _server.Ports.Add(ipAny, _serverPort, sslCredentials);

                _server.Start();

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
            if (_server == null)
            {
                return true;
            }

            try
            {
                _server.ShutdownAsync().Wait(9543);
                return true;
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
            }

            _server = null;

            return false;

        }


        #region 测试 已测试


        public static void TestMain(String[] args)
        {
            GrpcServer server = new GrpcServer(new List<ServerServiceDefinition>(){ Greeter.BindService(new GreeterServiceImpl()) }, 5142);
            //server.StartWithSsl();
            server.Start();
            Console.ReadLine();
            Console.ReadLine();
        }

        #endregion


    }
}

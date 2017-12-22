using Grpc.Core;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cmlu.services;

namespace Introduce_To_Algorithm3.OpenSourceLib.grpcs
{
    /// <summary>
    /// https://grpc.io/
    /// A high performance, open-source universal RPC framework
    /// Works across languages and platforms
    /// Start quickly and scale Install runtime and dev environments with a single line and also scale to millions of RPCs per second with the framework
    /// Bi-directional streaming and fully integrated pluggable authentication with http/2 based transport
    /// with pluggable support for load balancing, tracing, health checking and authentication. 
    /// 可以连接手机(android ios)和浏览器(js)
    /// 
    /// 
    /// grpc首先定义service，指定方法的参数和返回值。
    /// grpc默认使用protocol buffer.
    /// grpc依赖.net4.5，
    /// 
    /// grpc必须有且仅有一个输入一个输出，并且建议每个方法专有自己的输入输出定义(当然这不是必须的，自己写的严格遵守)
    /// gRPC service methods have exactly one input message and exactly one output message. Typically, these messages are used as input and output to only one method.
    /// GRPC的C#版本底层通过pinvoke调用的grpc c-based的版本
    /// 服务器代码参照GrpcServer
    /// </summary>
    public static class GrpcClientHelper
    {
        /*
gRPC支持四种服务方法:grpc服务都是一个输入参数一个输出返回值的
google.protobuf.Empty用来表示无输入
1) Unary RPC 客户端发送一次请求，服务器端返回一次响应。定义如下：
rpc SayHello(HelloRequest) returns (HelloResponse){ }
//客户端调用，然后服务器端发送metadata或者等待客户端request message（依赖于程序配置）,服务器端创建响应并和status信息和optional tailing metadata一起返回，如果状态OK，客户端获取response
2)Server streaming RPC 客户端发送一次请求，服务器端返回一系列流式消息，客户端读取流直到没有更多消息
stream关键字
rpc LotsOfReplies(HelloRequest) returns (stream HelloResponse){ }
//客户端发送请求，服务端发送所有的响应和status code和optional trailing meta.客户端接收所有服务器端响应。客户端要接收完所有的消息。
3)Client streaming RPC 客户端发送一系列流式消息，客户端发送完后等待服务器端返回结果
rpc LotsOfGreetings(stream HelloRequest) returns (HelloResponse) {
}
//客户端发送stream of requests，服务器端发送一个响应，服务器端不一定要接收完所有的客户端请求。
4)Bidirectional streaming RPC  双向流。客户端和服务器都可以读写流式消息。两个流可以独立操作。类似于socket。
客户端和服务器可以以任意的顺序读写。每个流中消息的顺序是保持的。类似于socket
rpc BidiHello(stream HelloRequest) returns (stream HelloResponse){ }
//
//
客户端可以指定timeout或deadline,
客户端和服务器端都可以发送元数据metadata,metadata是一系列键值对。


//pb的C#编译器 建立.net4.5工程 使用 nuget下载Google.Protobuf 和 google.protobuf.Tools  Tools包含了protoc.exe
//grpc.tools增加了对grpc service的支持, google.protobuf.Tools支持原生的pb文件
//执行命令 protoc -I=$SRC_DIR --csharp_out=$DST_DIR $SRC_DIR/addressbook.proto
//-I指定查找import的目录，默认是程序当前目录，--sharp_out指定C#代码输出目录
使用.proto文件定义服务，pb编译器生成客户端和服务器端代码。


         */

        /*
         * 
         * 

发现一个问题，如果长时间存在异常job，发生如下错误：
System.Net.Sockets.SocketException (0x80004005): 由于系统缓冲区空间不足或队列已满，不能执行套接字上的操作。
An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full.

http://blog.csdn.net/s_nuclear/article/details/25898597

问题原因：

服务器上部署有  WebService、处理服务：WebService有客户端轮询调用；处理服务也轮询数据库，进行相关处理后上传文件到SFTP上。

netstat -ano|findstr WAIT
服务器上修改这两个值 regedit
修改两个注册表：
HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\Tcpip\Parameters\MaxUserPort  如果没有，则手动创建 DWord（32位）  ”数值数据“改为十进制65531 或者认为适当的值。
此值表示 用户最大可以使用的端口数量，默认为5000。

HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\Tcpip\Parameters\TcpTimedWaitDelay 如果没有，则手动创建 DWord（32位）  ”数值数据“改为十进制30 或者你认为适当的值。
此值表示一个关闭后的端口等待多久之后可以重新使用，默认为120秒，也就是2分钟才可以重新使用。
重启系统


一般部署方案:
两个loadbalancer部署在不同的机器上，每个lb管理相同的server


开发:
2017年10月20日 13:06:10
    增加后备模式，即先考虑负载均衡如果负载均衡失败再固定推荐


         * 
         */



        /*
         * grpc支持两种加密认证方式
         * 1）ssl\TLS，认证和加密整个数据
         * 
         * 2)token-based authentication with google,需要连google
         * 
         * 
         * 认证可以在channel级别，也可以每次调用认证。
         * 两种证书类型：
         * Channel credentials, which are attached to a Channel, such as SSL credentials.
         * Call credentials, which are attached to a call
         * 
         * https://grpc.io/docs/guides/auth.html
         */


        /*
         
service HelloService {
  rpc SayHello (HelloRequest) returns (HelloResponse);
}

message HelloRequest {
  string greeting = 1;
}

message HelloResponse {
  string reply = 1;
} 


         */


        /*
         * When a message is parsed, if the encoded message does not contain a particular singular element, the corresponding field in the parsed object is set to the default value for that field. These defaults are type-specific:

For strings, the default value is the empty string.
For bytes, the default value is empty bytes.
For bools, the default value is false.
For numeric types, the default value is zero.
For enums, the default value is the first defined enum value, which must be 0.
For message fields, the field is not set. Its exact value is language-dependent. See the generated code guide for details.
The default value for repeated fields is empty (generally an empty list in the appropriate language).

            Note that for scalar message fields, once a message is parsed there's no way of telling whether a field was explicitly set to the default value (for example whether a boolean was set to false) or just not set at all: you should bear this in mind when defining your message types. For example, don't have a boolean that switches on some behaviour when set to false if you don't want that behaviour to also happen by default. Also note that if a scalar message field is set to its default, the value will not be serialized on the wire.
            very enum definition must contain a constant that maps to zero as its first element. This is because:

There must be a zero value, so that we can use 0 as a numeric default value.
The zero value needs to be the first element, for compatibility with the proto2 semantics where the first enum value is always the default.


         */

        /*
         * 国际化
         * 方法名只能是ASCII码的。
         * status detail/message 可以是unicode string
         * metadata key/value 是unicode string
         * 
         */



        #region 定义常量

        /// <summary>
        /// 服务地址 ip:port
        /// </summary>
        //public static readonly string ServiceAddress = "127.0.0.1:50051";

        #endregion



        ///// <summary>
        ///// 安全调用grpc
        ///// </summary>
        ///// <param name="action">执行的操作，构建client并调用</param>
        ///// <param name="exceptionHandler">异常处理</param>
        //public static bool SafeInvoke(Action<Channel> action, Action<Exception> exceptionHandler = null) 
        //{
        //    Channel channel = null;

        //    try
        //    {
        //        var options = new List<ChannelOption>()
        //        {
        //            new ChannelOption(ChannelOptions.MaxSendMessageLength,16*1024*1024),//最大可以发送的消息长度
        //            new ChannelOption(ChannelOptions.MaxReceiveMessageLength,32*1024*1024),//最大允许接收的消息长度
        //        };
        //        //不使用加密
        //        channel = new Channel(ServiceAddress, ChannelCredentials.Insecure,options);


        //        if(action != null)
        //        {
        //            action(channel);
        //            //构建client
        //            //var client = new Greeter.GreeterClient(channel);
        //            //客户端调用时指定deadline,如果不指定表示不超时，deadline使用utc时间
        //            //调用client,可以多次调用
        //            //var reply = client.SayHello(new Request() { Request_ = "Hello" });
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        if(exceptionHandler != null)
        //        {
        //            exceptionHandler(ex);
        //        }

        //        return false;
        //    }
        //    finally
        //    {
        //        if (channel != null)
        //        {
        //            for (int i = 0; i < 2; i++)
        //            {
        //                try
        //                {
        //                    channel.ShutdownAsync().Wait(6000);
        //                    //安全关闭退出
        //                    break;
        //                }
        //                catch (Exception ex)
        //                {
        //                    NLogHelper.Error($"第{i + 1}次关闭Channel失败:{ex}");
        //                }
        //            }
        //        }

        //    }


        //}

        ///
        /// 避免每次创建options
        private static readonly List<ChannelOption> GrpcOptions = new List<ChannelOption>(capacity: 3)
        {
            ////Grpc不适合处理大量的数据，处理的数据级别是MB。如果需要传输大的消息，使用stream流式消息多次传输
            new ChannelOption(ChannelOptions.MaxSendMessageLength,8*1024*1024),//最大可以发送的消息长度
            new ChannelOption(ChannelOptions.MaxReceiveMessageLength,32*1024*1024),//最大允许接收的消息长度
            new ChannelOption(ChannelOptions.SoReuseport,1),//重用端口，默认值就是1
        };


        /// <summary>
        /// 安全调用grpc
        /// Grpc不适合处理大量的数据，处理的数据级别是MB。如果需要传输大的消息，使用stream流式消息多次传输
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="action">执行的操作，构建client并调用</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="timeoutSeconds">过期时间  单位秒 该参数实际上没有用</param>
        public static bool SafeInvoke(string ip,int port,Action<Channel> action, Action<Exception> exceptionHandler = null/*,int timeoutSeconds = 16*/)
        {
            if (action == null)
            {
                return true;
            }

            Channel channel = null;

            try
            {
                //var options = new List<ChannelOption>(capacity:3)
                //{
                //    ////Grpc不适合处理大量的数据，处理的数据级别是MB。如果需要传输大的消息，使用stream流式消息多次传输
                //    new ChannelOption(ChannelOptions.MaxSendMessageLength,8*1024*1024),//最大可以发送的消息长度
                //    new ChannelOption(ChannelOptions.MaxReceiveMessageLength,32*1024*1024),//最大允许接收的消息长度
                //    new ChannelOption(ChannelOptions.SoReuseport,1),//重用端口，默认值就是1
                //};
                //不使用加密
                //channel = new Channel(string.Format("{0}:{1}",ip,port), ChannelCredentials.Insecure);
                channel = new Channel(ip, port, ChannelCredentials.Insecure,GrpcOptions);

                //if (action != null)
                {
                    action(channel);


                    /*
                     *
                     *
                    //构建client, 客户端不需要关闭
                    var client = new Greeter.GreeterClient(channel);
                    //客户端调用时指定deadline,如果不指定表示不超时
                    //调用client,可以重用channel,多次调用方法
                    //deadLine必须使用UTC时间
                    var reply = client.SayHello(new Request() {Request_ = "Hello"},
                        deadline: DateTime.UtcNow.AddSeconds(timeoutSeconds));
                    *
                    *
                    */
                }

                return true;
            }
            catch (RpcException rpcEx)
            {
                //判断是否正常
                StatusCode statusCode = rpcEx.Status.StatusCode;
                string detail = rpcEx.Status.Detail;

                if (statusCode == StatusCode.Unavailable)
                {
                    //服务不可用，通常是因为网络原因
                }


                if (statusCode == StatusCode.DeadlineExceeded)
                {
                    //超时
                }
                
                //与网络相关的异常只有这两个


                exceptionHandler?.Invoke(rpcEx);
                return false;
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return false;
            }
            finally
            {
                if (channel != null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            channel.ShutdownAsync().Wait(9000);
                            //channel.ShutdownAsync().Wait();
                            //安全关闭退出
                            break;
                        }
                        catch (Exception ex)
                        {
                            NLogHelper.Error($"第{i + 1}次关闭Channel失败:{ex}");
                        }
                    }

                    //channel = null;
                }
            }
        }

        /// <summary>
        /// 安全调用采用了反射可能影响性能,不推荐使用
        /// 不能实现channel级别的重用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="action"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool SafeInvoke<T>(string ip, int port, Action<T> action,
            Action<Exception> exceptionHandler = null/*, int timeoutSeconds = 16*/) where T : ClientBase<T>
        {
            if (action == null)
            {
                return true;
            }

            Channel channel = null;

            try
            {
                //var options = new List<ChannelOption>(capacity:3)
                //{
                //    ////Grpc不适合处理大量的数据，处理的数据级别是MB。如果需要传输大的消息，使用stream流式消息多次传输
                //    new ChannelOption(ChannelOptions.MaxSendMessageLength,8*1024*1024),//最大可以发送的消息长度
                //    new ChannelOption(ChannelOptions.MaxReceiveMessageLength,32*1024*1024),//最大允许接收的消息长度
                //    new ChannelOption(ChannelOptions.SoReuseport,1),//重用端口，默认值就是1
                //};
                //不使用加密
                //channel = new Channel(string.Format("{0}:{1}",ip,port), ChannelCredentials.Insecure);
                channel = new Channel(ip, port, ChannelCredentials.Insecure, GrpcOptions);

                //if (action != null)
                {
                    T stub = Activator.CreateInstance(typeof(T), channel) as T;
                    
                    action(stub);


                    /*
                     *
                     *
                    //构建client, 客户端不需要关闭
                    var client = new Greeter.GreeterClient(channel);
                    //客户端调用时指定deadline,如果不指定表示不超时
                    //调用client,可以重用channel,多次调用方法
                    //deadLine必须使用UTC时间
                    var reply = client.SayHello(new Request() {Request_ = "Hello"},
                        deadline: DateTime.UtcNow.AddSeconds(timeoutSeconds));
                    *
                    *
                    */
                }

                return true;
            }
            catch (RpcException rpcEx)
            {
                //判断是否正常
                StatusCode statusCode = rpcEx.Status.StatusCode;
                string detail = rpcEx.Status.Detail;

                if (statusCode == StatusCode.Unavailable)
                {
                    //服务不可用，通常是因为网络原因
                }


                if (statusCode == StatusCode.DeadlineExceeded)
                {
                    //超时
                }


                exceptionHandler?.Invoke(rpcEx);
                return false;
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return false;
            }
            finally
            {
                if (channel != null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            channel.ShutdownAsync().Wait(9000);
                            //channel.ShutdownAsync().Wait();
                            //安全关闭退出
                            break;
                        }
                        catch (Exception ex)
                        {
                            NLogHelper.Error($"第{i + 1}次关闭Channel失败:{ex}");
                        }
                    }

                    //channel = null;
                }
            }
        }


        /// <summary>
        /// 安全调用grpc
        /// Grpc不适合处理大量的数据，处理的数据级别是MB。如果需要传输大的消息，使用stream流式消息多次传输
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="action">执行的操作，构建client并调用</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="timeoutSeconds">过期时间  单位秒</param>
        public static bool SafeInvokeWithSsl(string ip, int port, Action<Channel> action, Action<Exception> exceptionHandler = null, int timeoutSeconds = 16)
        {
            if (action == null)
            {
                return true;
            }

            Channel channel = null;

            try
            {
                //共用证书，不要每次读取物理文件，应该读取到内存中
                //与服务器端的server.crt是同一个 PEM encoded root certificate
                var cacert = File.ReadAllText("server.crt");

                //证书
                var ssl = new SslCredentials(cacert);

                var options = new List<ChannelOption>()
                {
                    ////Grpc不适合处理大量的数据，处理的数据级别是MB。如果需要传输大的消息，使用stream流式消息多次传输
                    new ChannelOption(ChannelOptions.MaxSendMessageLength,8*1024*1024),//最大可以发送的消息长度
                    new ChannelOption(ChannelOptions.MaxReceiveMessageLength,32*1024*1024),//最大允许接收的消息长度
                    new ChannelOption(ChannelOptions.SoReuseport,1),//重用端口，默认值就是1
                    new ChannelOption(ChannelOptions.SslTargetNameOverride,"root"),//表示使用自签证书，需要注意下其中“ChannelOptions.SslTargetNameOverride”这部分是必须的，因为我们是自己生成的证书，所以域名是root
                };
                //不使用加密
                //channel = new Channel(string.Format("{0}:{1}",ip,port), ChannelCredentials.Insecure);
                channel = new Channel(ip, port, ssl, options);

                //if (action != null)
                {
                    action(channel);


                    /*
                     *
                     *
                    //构建client, 客户端不需要关闭
                    var client = new Greeter.GreeterClient(channel);
                    //客户端调用时指定deadline,如果不指定表示不超时
                    //调用client,可以重用channel,多次调用方法
                    //deadLine必须使用UTC时间
                    var reply = client.SayHello(new Request() {Request_ = "Hello"},
                        deadline: DateTime.UtcNow.AddSeconds(timeoutSeconds));
                    *
                    *
                    */
                }

                return true;
            }
            catch (RpcException rpcEx)
            {
                //判断是否正常
                StatusCode statusCode = rpcEx.Status.StatusCode;
                string detail = rpcEx.Status.Detail;

                if (statusCode == StatusCode.Unavailable)
                {
                    //服务不可用，通常是因为网络原因
                }

                if (statusCode == StatusCode.DeadlineExceeded)
                {
                    //超时
                }


                exceptionHandler?.Invoke(rpcEx);
                return false;
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return false;
            }
            finally
            {
                if (channel != null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            channel.ShutdownAsync().Wait(9000);
                            //channel.ShutdownAsync().Wait();
                            //安全关闭退出
                            break;
                        }
                        catch (Exception ex)
                        {
                            NLogHelper.Error($"第{i + 1}次关闭Channel失败:{ex}");
                        }
                    }

                    //channel = null;
                }
            }
        }


        #region ServerCallContext相关


        /// <summary>
        /// 从调用上下文中获取客户端IP，
        /// 如果获取失败，返回String.Empty或者null
        /// 兼容ipv4和ipv6
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static string GetClientIp(ServerCallContext context, Action<Exception> exceptionHandler = null)
        {
            try
            {
                string clientPeer = context?.Peer?.Trim();//例如:  ipv4:192.168.163.166:64205   //例如:  ipv4:192.168.163.166:64205  ipv6:fe80::d11:b6b6:1588:a2c2:1221
                if (string.IsNullOrEmpty(clientPeer))
                {
                    return string.Empty;
                }
                // / test.Greeter / SayHello

                int firstIndex = clientPeer.IndexOf(":",StringComparison.CurrentCulture);
                int lastIndex = clientPeer.LastIndexOf(":", StringComparison.CurrentCulture);

                if (firstIndex >= 0 && lastIndex > 0 && lastIndex > firstIndex)
                {
                    return clientPeer.Substring(firstIndex + 1, lastIndex - firstIndex - 1).Trim();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return string.Empty;
            }
        }


        /// <summary>
        /// 从调用上下文中获取客户端端口，
        /// 如果获取失败，返回-1
        /// 兼容ipv4和ipv6
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static int GetClientPort(ServerCallContext context, Action<Exception> exceptionHandler = null)
        {
            try
            {
                string clientPeer = context?.Peer;//例如:  ipv4:192.168.163.166:64205
                if (string.IsNullOrWhiteSpace(clientPeer))
                {
                    return -1;
                }
                

                string[] arr = clientPeer.Trim().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length >= 1)
                {
                    //>=1说明至少有一项
                    string portStr = arr[arr.Length-1].Trim();
                    int port = -1;
                    if (int.TryParse(portStr, out port))
                    {
                        return port;
                    }
                    return -1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return -1;
            }
        }


        /// <summary>
        /// 从调用上下文中获取客户端调用方法，
        /// 如果获取失败，返回String.Empty或者null
        /// // / test.Greeter / SayHello
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static string GetClientCallMethod(ServerCallContext context, Action<Exception> exceptionHandler = null)
        {
            try
            {
                string method = context?.Method;
                // / test.Greeter / SayHello
                if (string.IsNullOrEmpty(method))
                {
                    return string.Empty;
                }
                else
                {
                    return method;
                }
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return string.Empty;
            }
        }



        #endregion





        #region 测试  已测试

        public static void TestMain(string[] args)
        {

            GrpcClientHelper.SafeInvoke("127.0.0.1", 50051, channel =>
            {
                Greeter.GreeterClient client = new Greeter.GreeterClient(channel);
                var result = client.SayHello(new Request(), deadline: DateTime.UtcNow.AddSeconds(12));
                Console.WriteLine(result.Response_);
            });


            GrpcClientHelper.SafeInvoke<Greeter.GreeterClient>("127.0.0.1", 50051, client =>
            {
                var result = client.SayHello(new Request(),deadline:DateTime.UtcNow.AddSeconds(12));
                Console.WriteLine(result.Response_);
            });



            GrpcClientHelper.SafeInvokeWithSsl("192.168.163.159", 5142, channel =>
            {
                var client = new Greeter.GreeterClient(channel);
                Request request = new Request();
                request.Request_ = "request at" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                Response response = client.SayHello(request, deadline: DateTime.UtcNow.AddSeconds(36));
                Console.WriteLine(response.Response_);
            });
        }

        #endregion 


    }
}
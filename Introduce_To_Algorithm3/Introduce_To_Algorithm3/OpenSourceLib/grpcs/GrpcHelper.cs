﻿using Grpc.Core;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// grpc依赖.net4.5
    /// 
    /// grpc好像必须一个输入一个输出
    /// </summary>
    public static class GrpcHelper
    {
        /*
gRPC支持四种服务方法:grpc服务都是一个输入一个输出的
google.protobuf.Empty用来表示无输入
1) Unary RPC 客户端发送一次请求，服务器端返回一次响应。定义如下：
rpc SayHello(HelloRequest) returns (HelloResponse){ }
//客户端调用，然后服务器端发送metadata或者等待客户端request message（依赖于程序配置）,服务器端创建响应并和status信息和optional tailing metadata一起返回，如果状态OK，客户端获取response
2)Server streaming RPC 客户端发送一次请求，服务器端返回一系列流式消息，客户端读取流指导没有更多消息
stream关键字
rpc LotsOfReplies(HelloRequest) returns (stream HelloResponse){ }
//客户端发送请求，服务端发送所有的响应和status code和optional trailing meta.客户端接收所有服务器端响应。
3)Client streaming RPC 客户端发送一系列流式消息，客户端发送完后等待服务器端返回结果
rpc LotsOfGreetings(stream HelloRequest) returns (HelloResponse) {
}
//客户端发送stream of requests，服务器端发送一个响应，服务器端不一定要接受完所有的客户端请求。
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
         * grpc支持两种加密认证方式
         * 1）ssl\TLS，认证和加密整个数据
         * 
         * 2)token-based authentication with google,需要连google
         * 
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

        #region 定义常量

        /// <summary>
        /// 服务地址
        /// </summary>
        public static readonly string ServiceAddress = "127.0.0.1:50051";

        #endregion


       
        /// <summary>
        /// 安全调用grpc
        /// </summary>
        /// <param name="action">执行的操作，构建client并调用</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static void SafeInvoke(Action<Channel> action, Action<Exception> exceptionHandler = null) 
        {
            Channel channel = null;

            try
            {
                //不使用加密
                channel = new Channel(ServiceAddress, ChannelCredentials.Insecure);
                

                if(action != null)
                {
                    action(channel);
                    //构建client
                    //var client = new Greeter.GreeterClient(channel);
                    //调用client,可以多次调用
                    //var reply = client.SayHello(new Request() { Request_ = "Hello" });
                }

            }
            catch (Exception ex)
            {
                if(exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
            finally
            {
                if (channel != null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            channel.ShutdownAsync().Wait();
                            //安全关闭退出
                            break;
                        }
                        catch (Exception ex)
                        {
                            NLogHelper.Error($"第{i + 1}次关闭Channel失败:{ex}");
                        }
                    }
                }

            }


        }
    }
}
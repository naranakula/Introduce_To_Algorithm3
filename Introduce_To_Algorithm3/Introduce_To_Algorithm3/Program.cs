using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.files;
using Introduce_To_Algorithm3.Common.Utils.Serial;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite;
using Introduce_To_Algorithm3.OpenSourceLib.Dapper;
using Introduce_To_Algorithm3.OpenSourceLib.FTP;
using Introduce_To_Algorithm3.OpenSourceLib.grpcs.examples;
using Introduce_To_Algorithm3.OpenSourceLib.NetMqs.ZeroMqs;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using com.cmlu.services;
using Introduce_To_Algorithm3.OpenSourceLib.grpcs;
using Introduce_To_Algorithm3.OpenSourceLib.grpcs.protos;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2;
using Introduce_To_Algorithm3.OpenSourceLib.AutoMapper;
using Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //测试拦截器可以使用
            //GrpcClientHelper.SafeInvoke("127.0.0.1", 50051, channel =>
            //{
            //    CallInvoker callInvoker = channel.Intercept(new ClientInterceptor());
            //    Greeter.GreeterClient client = new Greeter.GreeterClient(callInvoker);
            //    var respone = client.SayHello(new Request());
            //    respone = client.SayHello(new Request());
            //    respone = client.SayHello(new Request());
            //    respone = client.SayHello(new Request());
            //    respone = client.SayHello(new Request());
            //    Console.WriteLine(respone.Response_);
            //});

            //红黑树
            SortedDictionary<string,string> rbTree = new SortedDictionary<string, string>();
            DirectoryHold.ResetCurrentDir();
            NLogHelper.Info(GuidUtils.GetGuid());
            NLogHelper.Trace("Trace");
            NLogHelper.Debug("Debug");
            NLogHelper.Info("Info");
            NLogHelper.Warn("Warn");
            NLogHelper.Error("Error");
            NLogHelper.Fatal("Fatal");
        }
    }

    /// <summary>
    /// 客户端拦截
    /// </summary>
    public class ClientInterceptor : Interceptor
    {

        public ClientInterceptor()
        {
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            Console.WriteLine("inter");
            return continuation(request, context);
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            Console.WriteLine("inter");
            return continuation(request, context);
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            Console.WriteLine("inter");
            return continuation(request, context);
        }

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            Console.WriteLine("inter");
            return continuation(context);
        }

        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            Console.WriteLine("inter");
            return continuation(context);
        }



        
    }


    /// <summary>
    /// 服务器端拦截
    /// </summary>
    public class ServerInterceptor : Interceptor
    {

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            Console.WriteLine("UnaryServerHandler interceptor");
            return continuation(request, context);
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            Console.WriteLine("interceptor");
            return continuation(requestStream, context);
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            Console.WriteLine("interceptor");
            return continuation(request, responseStream, context);
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            Console.WriteLine("interceptor");
            return continuation(requestStream, responseStream, context);
        }
    }


}


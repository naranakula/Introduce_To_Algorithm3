// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: test.proto
// </auto-generated>
#pragma warning disable 1591
#region Designer generated code

using System;
using System.Threading;
using System.Threading.Tasks;
using grpc = global::Grpc.Core;

namespace com.cmlu.services {
  public static partial class Greeter
  {
    static readonly string __ServiceName = "test.Greeter";

    static readonly grpc::Marshaller<global::com.cmlu.services.Request> __Marshaller_Request = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::com.cmlu.services.Request.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::com.cmlu.services.Response> __Marshaller_Response = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::com.cmlu.services.Response.Parser.ParseFrom);

    static readonly grpc::Method<global::com.cmlu.services.Request, global::com.cmlu.services.Response> __Method_SayHello = new grpc::Method<global::com.cmlu.services.Request, global::com.cmlu.services.Response>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SayHello",
        __Marshaller_Request,
        __Marshaller_Response);

    static readonly grpc::Method<global::com.cmlu.services.Request, global::com.cmlu.services.Response> __Method_SayHelloStreamServer = new grpc::Method<global::com.cmlu.services.Request, global::com.cmlu.services.Response>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "SayHelloStreamServer",
        __Marshaller_Request,
        __Marshaller_Response);

    static readonly grpc::Method<global::com.cmlu.services.Request, global::com.cmlu.services.Response> __Method_SayHelloStreamClient = new grpc::Method<global::com.cmlu.services.Request, global::com.cmlu.services.Response>(
        grpc::MethodType.ClientStreaming,
        __ServiceName,
        "SayHelloStreamClient",
        __Marshaller_Request,
        __Marshaller_Response);

    static readonly grpc::Method<global::com.cmlu.services.Request, global::com.cmlu.services.Response> __Method_SayHelloStreamBidirectional = new grpc::Method<global::com.cmlu.services.Request, global::com.cmlu.services.Response>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "SayHelloStreamBidirectional",
        __Marshaller_Request,
        __Marshaller_Response);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::com.cmlu.services.TestReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Greeter</summary>
    public abstract partial class GreeterBase
    {
      /// <summary>
      ///Unary RPC
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::com.cmlu.services.Response> SayHello(global::com.cmlu.services.Request request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      ///Server streaming RPC
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="responseStream">Used for sending responses back to the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>A task indicating completion of the handler.</returns>
      public virtual global::System.Threading.Tasks.Task SayHelloStreamServer(global::com.cmlu.services.Request request, grpc::IServerStreamWriter<global::com.cmlu.services.Response> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      ///Client streaming RPC
      /// </summary>
      /// <param name="requestStream">Used for reading requests from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::com.cmlu.services.Response> SayHelloStreamClient(grpc::IAsyncStreamReader<global::com.cmlu.services.Request> requestStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      ///bidirectional streaming RPC
      /// </summary>
      /// <param name="requestStream">Used for reading requests from the client.</param>
      /// <param name="responseStream">Used for sending responses back to the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>A task indicating completion of the handler.</returns>
      public virtual global::System.Threading.Tasks.Task SayHelloStreamBidirectional(grpc::IAsyncStreamReader<global::com.cmlu.services.Request> requestStream, grpc::IServerStreamWriter<global::com.cmlu.services.Response> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Greeter</summary>
    public partial class GreeterClient : grpc::ClientBase<GreeterClient>
    {
      /// <summary>Creates a new client for Greeter</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public GreeterClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Greeter that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public GreeterClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected GreeterClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected GreeterClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      ///Unary RPC
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::com.cmlu.services.Response SayHello(global::com.cmlu.services.Request request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SayHello(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///Unary RPC
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::com.cmlu.services.Response SayHello(global::com.cmlu.services.Request request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SayHello, null, options, request);
      }
      /// <summary>
      ///Unary RPC
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::com.cmlu.services.Response> SayHelloAsync(global::com.cmlu.services.Request request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SayHelloAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///Unary RPC
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::com.cmlu.services.Response> SayHelloAsync(global::com.cmlu.services.Request request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SayHello, null, options, request);
      }
      /// <summary>
      ///Server streaming RPC
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncServerStreamingCall<global::com.cmlu.services.Response> SayHelloStreamServer(global::com.cmlu.services.Request request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SayHelloStreamServer(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///Server streaming RPC
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncServerStreamingCall<global::com.cmlu.services.Response> SayHelloStreamServer(global::com.cmlu.services.Request request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_SayHelloStreamServer, null, options, request);
      }
      /// <summary>
      ///Client streaming RPC
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncClientStreamingCall<global::com.cmlu.services.Request, global::com.cmlu.services.Response> SayHelloStreamClient(grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SayHelloStreamClient(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///Client streaming RPC
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncClientStreamingCall<global::com.cmlu.services.Request, global::com.cmlu.services.Response> SayHelloStreamClient(grpc::CallOptions options)
      {
        return CallInvoker.AsyncClientStreamingCall(__Method_SayHelloStreamClient, null, options);
      }
      /// <summary>
      ///bidirectional streaming RPC
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncDuplexStreamingCall<global::com.cmlu.services.Request, global::com.cmlu.services.Response> SayHelloStreamBidirectional(grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SayHelloStreamBidirectional(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///bidirectional streaming RPC
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncDuplexStreamingCall<global::com.cmlu.services.Request, global::com.cmlu.services.Response> SayHelloStreamBidirectional(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_SayHelloStreamBidirectional, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override GreeterClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new GreeterClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(GreeterBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SayHello, serviceImpl.SayHello)
          .AddMethod(__Method_SayHelloStreamServer, serviceImpl.SayHelloStreamServer)
          .AddMethod(__Method_SayHelloStreamClient, serviceImpl.SayHelloStreamClient)
          .AddMethod(__Method_SayHelloStreamBidirectional, serviceImpl.SayHelloStreamBidirectional).Build();
    }

  }
}
#endregion

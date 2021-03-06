// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: ConfigService.proto
// </auto-generated>
#pragma warning disable 1591
#region Designer generated code

using System;
using System.Threading;
using System.Threading.Tasks;
using grpc = global::Grpc.Core;

namespace com.common.cmlu.services {
  /// <summary>
  ///定义获取配置接口
  /// </summary>
  public static partial class ConfigService
  {
    static readonly string __ServiceName = "com.common.cmlu.services.ConfigService";

    static readonly grpc::Marshaller<global::com.common.cmlu.services.GetConfigRequest> __Marshaller_GetConfigRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::com.common.cmlu.services.GetConfigRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::com.common.cmlu.services.GetConfigResponse> __Marshaller_GetConfigResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::com.common.cmlu.services.GetConfigResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::com.common.cmlu.services.GetConfigRequest, global::com.common.cmlu.services.GetConfigResponse> __Method_GetConfig = new grpc::Method<global::com.common.cmlu.services.GetConfigRequest, global::com.common.cmlu.services.GetConfigResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetConfig",
        __Marshaller_GetConfigRequest,
        __Marshaller_GetConfigResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::com.common.cmlu.services.ConfigServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of ConfigService</summary>
    public abstract partial class ConfigServiceBase
    {
      /// <summary>
      ///获取配置信息
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::com.common.cmlu.services.GetConfigResponse> GetConfig(global::com.common.cmlu.services.GetConfigRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for ConfigService</summary>
    public partial class ConfigServiceClient : grpc::ClientBase<ConfigServiceClient>
    {
      /// <summary>Creates a new client for ConfigService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public ConfigServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for ConfigService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public ConfigServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected ConfigServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected ConfigServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      ///获取配置信息
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::com.common.cmlu.services.GetConfigResponse GetConfig(global::com.common.cmlu.services.GetConfigRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetConfig(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///获取配置信息
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::com.common.cmlu.services.GetConfigResponse GetConfig(global::com.common.cmlu.services.GetConfigRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetConfig, null, options, request);
      }
      /// <summary>
      ///获取配置信息
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::com.common.cmlu.services.GetConfigResponse> GetConfigAsync(global::com.common.cmlu.services.GetConfigRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetConfigAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///获取配置信息
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::com.common.cmlu.services.GetConfigResponse> GetConfigAsync(global::com.common.cmlu.services.GetConfigRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetConfig, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override ConfigServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ConfigServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(ConfigServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetConfig, serviceImpl.GetConfig).Build();
    }

  }
}
#endregion

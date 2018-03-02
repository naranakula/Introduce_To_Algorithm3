using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Introduce_To_Algorithm3.OpenSourceLib.grpcs
{

    /// <summary>
    /// 服务器拦截器
    /// Interceptor是抽象类，提供了默认实现，只需要覆盖想要实现的方法即可
    /// </summary>
    public class CommonServerInterceptor:Interceptor
    {

        #region 属性

        /// <summary>
        /// 在拦截调用之前的调用
        /// 第一个参数:调用的方法全名
        /// 第二个参数是客户端IP
        /// </summary>
        private readonly Action<string,string> _beforeAction = null;

        /// <summary>
        /// 在拦截调用之后的调用
        /// 第一个参数:调用的方法全名
        /// 第二个参数:调用的客户端IP
        /// 第三个参数:是否调用成功
        /// 第四个参数:调用失败的异常信息
        /// 第五个参数:调用耗费的时间
        /// </summary>
        private readonly Action<string,string, bool, Exception, TimeSpan> _afterAction = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommonServerInterceptor()
        {

        }

        /// <summary>
        /// 构造函数
        /// 
        /// </summary>
        /// <param name="beforeAction">在之前回调</param>
        /// <param name="afterAction">在之后回调</param>
        public CommonServerInterceptor(Action<string,string> beforeAction, Action<string,string, bool, Exception, TimeSpan> afterAction = null)
        {
            this._beforeAction = beforeAction;
            this._afterAction = afterAction;
        }


        #endregion;


        /// <summary>
        /// 一元调用
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            //调用的方法名
            string methodName = GrpcClientHelper.GetClientCallMethod(context);
            //客户端的IP
            string clientIp = GrpcClientHelper.GetClientIp(context);
            //开始调用时间
            DateTime startTime = DateTime.Now;

            try
            {
                this._beforeAction?.Invoke(methodName,clientIp);
            }
            catch {}

            try
            {
                Task<TResponse> responseTask = continuation(request, context);

                //调用使用时间
                TimeSpan usedTimeSpan = DateTime.Now - startTime;
                try
                {
                    _afterAction?.Invoke(methodName,clientIp, true, null, usedTimeSpan);
                }
                catch { }


                return responseTask;
            }
            catch (Exception ex)
            {
                //调用使用时间
                TimeSpan usedTimeSpan = DateTime.Now - startTime;
                try
                {
                    _afterAction?.Invoke(methodName, clientIp, false, ex, usedTimeSpan);
                }
                catch { }

                throw ex;
            }
        }

        /// <summary>
        /// 客户端Streaming
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestStream"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(requestStream, context);
        }

        /// <summary>
        /// 服务器Streaming
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(request, responseStream, context);
        }

        /// <summary>
        /// 双工调用
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(requestStream, responseStream, context);
        }

    }
}

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
    /// 通用的Grpc客户端拦截器
    /// Interceptor是抽象类，提供了默认实现，只需要覆盖想要实现的方法即可
    /// </summary>
    public class CommonClientInterceptor:Interceptor
    {

        #region 属性

        /// <summary>
        /// 在拦截调用之前的调用
        /// 第一个参数:调用的方法全名
        /// </summary>
        private readonly Action<string> _beforeAction = null;

        /// <summary>
        /// 在拦截调用之后的调用
        /// 第一个参数:调用的方法全名
        /// 第二个参数:是否调用成功
        /// 第三个参数:调用失败的异常信息
        /// 第四个参数:调用耗费的时间
        /// </summary>
        private readonly Action<string,bool,Exception,TimeSpan> _afterAction = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommonClientInterceptor()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// 
        /// </summary>
        /// <param name="beforeAction">在之前回调</param>
        /// <param name="afterAction">在之后回调</param>
        public CommonClientInterceptor(Action<string> beforeAction,
            Action<string, bool,Exception, TimeSpan> afterAction = null)
        {
            this._beforeAction = beforeAction;
            this._afterAction = afterAction;
        }


        #endregion;


        /// <summary>
        /// 阻塞的一元调用
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            //调用的方法全名
            string fullMethodName = context.Method?.FullName;

            //调用开始时间
            DateTime startTime = DateTime.Now;
            try
            {
                _beforeAction?.Invoke(fullMethodName);
            }
            catch { }

            try
            {
                TResponse resultResponse = continuation(request, context);

                //调用使用时间
                TimeSpan usedTimeSpan = DateTime.Now - startTime;
                try
                {
                    _afterAction?.Invoke(fullMethodName, true, null, usedTimeSpan);
                }
                catch { }

                return resultResponse;
            }
            catch (Exception ex)
            {
                //调用使用时间
                TimeSpan usedTimeSpan = DateTime.Now - startTime;
                try
                {
                    _afterAction?.Invoke(fullMethodName, false, ex, usedTimeSpan);
                }
                catch { }

                throw ex;
            }
        }

        /// <summary>
        /// 非阻塞的一元调用
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            return continuation(request, context);
        }

        /// <summary>
        /// 异步的服务器Streaming
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            return continuation(request, context);
        }

        /// <summary>
        /// 异步的客户端streaming
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            return continuation(context);
        }

        /// <summary>
        /// 异步的双工调用
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            return continuation(context);
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Introduce_To_Algorithm3.Models;

namespace Introduce_To_Algorithm3.OpenSourceLib.grpcs
{


    /// <summary>
    /// 利用channel的自动重连和线程安全做longRunchannel
    /// 
    /// LongRunChannel longChannel = new LongRunChannel(...);
    /// longChannel.Start()
    /// 
    /// 一系列的SafeInvoke
    /// SafeInvoke
    /// 
    /// longChannel.Stop()
    /// 
    /// </summary>
    public class LongRunChannel : IDisposable
    {

        #region 属性

        /// <summary>
        /// 服务器IP
        /// </summary>
        private readonly string _serverIp;

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIp
        {
            get { return _serverIp; }
        }

        /// <summary>
        /// 服务器端口
        /// </summary>
        private readonly int _serverPort;

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort
        {
            get { return _serverPort; }
        }

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();


        /// <summary>
        /// 通道  抽象一个tcp连接
        /// </summary>
        private volatile Channel _channel = null;

        /// <summary>
        /// 避免每次创建options
        /// </summary>
        private static readonly List<ChannelOption> GrpcOptions = new List<ChannelOption>(capacity: 4)
        {
            ////Grpc不适合处理大量的数据，处理的数据级别是MB。如果需要传输大的消息，使用stream流式消息多次传输
            new ChannelOption(ChannelOptions.MaxSendMessageLength,16*1024*1024),//最大可以发送的消息长度
            new ChannelOption(ChannelOptions.MaxReceiveMessageLength,32*1024*1024),//最大允许接收的消息长度
            new ChannelOption(ChannelOptions.SoReuseport,1),//重用端口，默认值就是1
            new ChannelOption(ChannelOptions.MaxConcurrentStreams,63),//单个连接最大允许的并发流
        };


        #region 重启相关

        /// <summary>
        /// 如果Channel进入ShutDown状态，重建Channel的最小时间间隔 毫秒
        /// </summary>
        private readonly int _minReEstablishChannelTimeIntervalInMilliseconds;

        /// <summary>
        /// 上一次重建Channel时间
        /// </summary>
        private DateTime _lastRebuildChannelTime = DateTime.Now;

        /// <summary>
        /// 如果上一次重建间隔时间超过此分钟数，必须重建，不管是不是正常
        /// </summary>
        private readonly int _maxIntervalMinutesToRebuild;

        /// <summary>
        /// 是否人工停止，人工停止后，将不再重建连接
        /// </summary>
        private volatile bool _isStop = false;

        /// <summary>
        /// 连续grpc的错误
        /// </summary>
        private volatile int _continuousGrpcError = 0;

        /// <summary>
        /// 最大连续的grpc的错误次数
        /// </summary>
        private readonly int _maxContinuousGrpcErrorCount;

        #endregion

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        public LongRunChannel(string serverIp, int serverPort)
        {
            if (string.IsNullOrWhiteSpace(serverIp))
            {
                throw new ArgumentException("server ip can't be empty");
            }


            lock (_locker)
            {
                this._serverIp = serverIp.Trim();
                this._serverPort = serverPort;
                this._isStop = false;

                Random rand = new Random();
                _maxContinuousGrpcErrorCount = rand.Next(13, 23);
                _maxIntervalMinutesToRebuild = rand.Next(24 * 60, 25 * 67);
                _minReEstablishChannelTimeIntervalInMilliseconds = rand.Next(2500, 4100);
            }

        }

        #endregion

        #region 启动&停止

        /// <summary>
        /// 获取通道状态
        /// </summary>
        private ChannelState ChannelState
        {
            get
            {
                lock (_locker)
                {
                    Channel tempChannel = _channel;
                    if (tempChannel == null)
                    {
                        return ChannelState.Shutdown;
                    }
                    //grpc错误次数超出限制,重建grpc channel
                    else if (_continuousGrpcError > _maxContinuousGrpcErrorCount)
                    {
                        return ChannelState.Shutdown;
                    }
                    //超过1天没有错误，重建grpc channel
                    else if ((DateTime.Now - _lastRebuildChannelTime).TotalMinutes > _maxIntervalMinutesToRebuild)
                    {
                        return ChannelState.Shutdown;
                    }
                    else
                    {
                        //通道状态，grpc底层已经实现了锁和异常处理(看过源代码)
                        return tempChannel.State;
                    }
                }
            }
        }


        /// <summary>
        /// 启动channel
        /// 经测试即使服务器不存在，仍然可以Start成功，new Channel只是创建channel，并没有实际上连接服务器
        /// 经测试如果服务器不存在也能启动成功，此时channel state是Idle，此时网络并没有实际连接
        /// 理论上不可能返回false
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool Start(Action<Exception> exceptionHandler = null)
        {
            try
            {
                //内部停止
                //如果已经启动则停止

                #region 关闭
                Channel tempChannel = _channel;
                if (tempChannel != null)
                {
                    //延迟关闭，保证上一个channel一段时间可用
                    Task.Factory.StartNew(() =>
                    {
                        Channel innerTempChannel = tempChannel;
                        
                        try
                        {
                            Thread.Sleep(2017);
                        }
                        catch
                        {
                            // ignored
                        }

                        //延迟关闭，保证上一个一段时间可用
                        try
                        {
                            innerTempChannel.ShutdownAsync().Wait(millisecondsTimeout: 9000);
                        }
                        catch
                        {
                            // ignored
                        }
                    });
                }
                #endregion

                //经测试如果服务器不存在也能启动成功，此时channel state是Idle，此时网络并没有实际连接
                _channel = new Channel(_serverIp, _serverPort, ChannelCredentials.Insecure, GrpcOptions);
                lock (_locker)
                {
                    _continuousGrpcError = 0;
                    _lastRebuildChannelTime = DateTime.Now;
                }
                return true;
            }
            catch (Exception ex)
            {
                lock (_locker)
                {
                    _channel = null;
                }

                if (exceptionHandler != null)
                {
                    try
                    {
                        exceptionHandler(ex);
                    }
                    catch
                    {
                        // ignored
                    }
                }
                return false;
            }
        }

        ///// <summary>
        ///// 内部停止
        ///// </summary>
        //private void InnerStop(int millisecondsTimeout = 9000, Action<Exception> exceptionHandler = null)
        //{
        //    Channel tempChannel = _channel;
        //    if (tempChannel != null)
        //    {
        //        try
        //        {
        //            tempChannel.ShutdownAsync().Wait(millisecondsTimeout: millisecondsTimeout);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (exceptionHandler != null)
        //            {
        //                try
        //                {
        //                    exceptionHandler(ex);
        //                }
        //                catch
        //                {
        //                    // ignored
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            _channel = null;
        //        }
        //    }
        //}


        /// <summary>
        /// 停止
        /// </summary>
        public void Stop(int millisecondsTimeout = 9000, Action<Exception> exceptionHandler = null)
        {
            //经过考虑，放在InnerStop前面更合适
            lock (_locker)
            {
                _isStop = true;
                _continuousGrpcError = 0;
            }


            Channel tempChannel = _channel;
            if (tempChannel != null)
            {
                try
                {
                    tempChannel.ShutdownAsync().Wait(millisecondsTimeout: millisecondsTimeout);
                }
                catch (Exception ex)
                {
                    if (exceptionHandler != null)
                    {
                        try
                        {
                            exceptionHandler(ex);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                finally
                {
                    _channel = null;
                }
            }
        }



        #endregion

        #region SafeInvoke

        /// <summary>
        /// 基于channel的多线程安全和自动重连
        /// 调用成功返回true，发生异常返回false
        /// 多线程安全
        /// </summary>
        /// <param name="channelAction"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool SafeInvoke(Action<Channel> channelAction, Action<Exception> exceptionHandler = null)
        {

            try
            {
                Channel tempChannel = _channel;

                lock (_locker)
                {
                    //获取通道状态
                    ChannelState state = this.ChannelState;

                    #region ShutDown重新建立连接

                    if (state == ChannelState.Shutdown)
                    {
                        //按照grpc文档，理论上start之后永远不会进入该状态
                        //已经人为停止，不需要重建
                        if (_isStop)
                        {
                            throw new CommonException(errorCode: 1, errorReason: "channel已经被停止");
                        }



                        //重建至少间隔这么多的时间
                        if ((DateTime.Now - _lastRebuildChannelTime).TotalMilliseconds >
                            _minReEstablishChannelTimeIntervalInMilliseconds)
                        {
                            //重建Channel,包含了关闭上一个连接，重建放在锁了，避免某些多线程异常
                            Start(exceptionHandler);
                            //因为重建需要耗时，所以重新调用Now
                            _lastRebuildChannelTime = DateTime.Now;

                            //重置channel
                            tempChannel = _channel;
                        }

                    }


                    #endregion

                }


                if (tempChannel == null)
                {
                    throw new CommonException(errorCode:2,errorReason:"channel为null");
                }


                channelAction(tempChannel);

                /*
                 * 代码示例
                 //构建client, 客户端不需要关闭
                 var client = new Greeter.GreeterClient(channel);
                 //客户端调用时指定deadline,如果不指定表示不超时
                 //调用client,可以重用channel,多次调用方法
                 //deadLine必须使用UTC时间
                 var reply = client.SayHello(new Request() {Request_ = "Hello"}, deadline: DateTime.UtcNow.AddSeconds(timeoutSeconds));
                 */
                //重置连续错误为0
                _continuousGrpcError = 0;
                return true;
            }
            catch (RpcException rpcException)
            {
                //连续错误+1
                _continuousGrpcError++;
                exceptionHandler?.Invoke(rpcException);
                return false;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        #endregion

        #region IDispose接口

        /// <summary>
        /// IDispose接口
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        #endregion

    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// SafeInvoke
    /// 
    /// longChannel.Stop()
    /// 
    /// </summary>
    public class LongRunChannel:IDisposable
    {

        #region 属性

        /// <summary>
        /// 服务器IP
        /// </summary>
        private readonly string _serverIp;

        /// <summary>
        /// 服务器端口
        /// </summary>
        private readonly int _serverPort;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();


        /// <summary>
        /// 通道
        /// </summary>
        private volatile Channel _channel = null;

        /// <summary>
        /// 避免每次创建options
        /// </summary>
        private static readonly List<ChannelOption> GrpcOptions = new List<ChannelOption>(capacity: 3)
        {
            ////Grpc不适合处理大量的数据，处理的数据级别是MB。如果需要传输大的消息，使用stream流式消息多次传输
            new ChannelOption(ChannelOptions.MaxSendMessageLength,8*1024*1024),//最大可以发送的消息长度
            new ChannelOption(ChannelOptions.MaxReceiveMessageLength,32*1024*1024),//最大允许接收的消息长度
            //new ChannelOption(ChannelOptions.SoReuseport,1),//重用端口，默认值就是1
            new ChannelOption(ChannelOptions.MaxConcurrentStreams,63),//单个连接最大允许的并发流
        };


        #region 重启相关

        /// <summary>
        /// 如果Channel进入ShutDown状态，重建Channel的最小时间间隔 毫秒
        /// </summary>
        private const int MinReEstablishChannelTimeIntervalInMilliseconds = 5113;

        /// <summary>
        /// 上一次重建Channel时间
        /// </summary>
        private DateTime _lastRebuildChannelTime = DateTime.Now.AddDays(-1);
        
        /// <summary>
        /// 是否人工停止，人工停止后，将不再重建连接
        /// </summary>
        private volatile bool _isStop = false;

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
            this._serverIp = serverIp;
            this._serverPort = serverPort;
            this._isStop = false;
        }

        #endregion
        
        #region 启动&停止

        /// <summary>
        /// 获取通道状态
        /// </summary>
        public ChannelState ChannelState
        {
            get
            {
                Channel tempChannel = _channel;
                if (tempChannel == null)
                {
                    return ChannelState.Shutdown;
                }

                //通道状态，底层已经实现了锁和异常处理
                return tempChannel.State;
            }
        }


        /// <summary>
        /// 启动channel
        /// 经测试即使服务器不存在，仍然可以Start成功，new Channel只是创建channel，并没有实际上连接服务器
        /// 
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool Start(Action<Exception> exceptionHandler = null)
        {
            try
            {
                //内部停止
                InnerStop();

                //如果已经启动则停止
                _channel = new Channel(_serverIp,_serverPort,ChannelCredentials.Insecure,GrpcOptions);
                return true;
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return false;
            }
        }

        /// <summary>
        /// 内部停止
        /// </summary>
        private void InnerStop(int millisecondsTimeout=9000,Action<Exception> exceptionHandler = null)
        {
            Channel tempChannel = _channel;
            if (tempChannel != null)
            {
                try
                {
                    tempChannel.ShutdownAsync().Wait(millisecondsTimeout: millisecondsTimeout);
                }
                catch (Exception ex)
                {
                    exceptionHandler?.Invoke(ex);
                }

                _channel = null;
            }
        }


        /// <summary>
        /// 停止
        /// </summary>
        public void Stop(int millisecondsTimeout = 9000, Action<Exception> exceptionHandler = null)
        {
            InnerStop(millisecondsTimeout,exceptionHandler);

            _isStop = true;
        }



        #endregion

        #region SafeInvoke

        /// <summary>
        /// 基于channel的多线程安全和自动重连
        /// </summary>
        /// <param name="channelAction"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool SafeInvoke(Action<Channel> channelAction, Action<Exception> exceptionHandler = null)
        {
            Channel tempChannel = _channel;

            try
            {
                //获取通道状态
                ChannelState state = this.ChannelState;

                #region ShutDown重新建立连接
                if (state == ChannelState.Shutdown)
                {
                    //已经人为停止，不需要重建
                    if (_isStop)
                    {
                        throw new CommonException(exceptionCode:1,exceptionDesc:"channel已经被停止");
                    }
                    
                    //是否需要重建
                    bool isNeedToReBuild = false;

                    lock (_locker)
                    {
                        if ((DateTime.Now - _lastRebuildChannelTime).TotalMilliseconds >
                            MinReEstablishChannelTimeIntervalInMilliseconds)
                        {
                            isNeedToReBuild = true;

                            //重建Channel,重建放在锁了，避免某些多线程异常
                            Start();
                            //因为重建需要耗时，所以重新调用Now
                            _lastRebuildChannelTime = DateTime.Now;
                        }
                    }

                    if (isNeedToReBuild)
                    {
                        //重建后，重新初始化状态
                        state = this.ChannelState;
                        tempChannel = _channel;
                        
                        if (state == ChannelState.Shutdown)
                        {
                            throw new CommonException(exceptionCode: 2, exceptionDesc: "Channel状态为ShutDown");
                        }
                    }
                }
                #endregion
                
                channelAction(tempChannel);

                /*
                 //构建client, 客户端不需要关闭
                 var client = new Greeter.GreeterClient(channel);
                 //客户端调用时指定deadline,如果不指定表示不超时
                 //调用client,可以重用channel,多次调用方法
                 //deadLine必须使用UTC时间
                 var reply = client.SayHello(new Request() {Request_ = "Hello"}, deadline: DateTime.UtcNow.AddSeconds(timeoutSeconds));
                 */

                return true;
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

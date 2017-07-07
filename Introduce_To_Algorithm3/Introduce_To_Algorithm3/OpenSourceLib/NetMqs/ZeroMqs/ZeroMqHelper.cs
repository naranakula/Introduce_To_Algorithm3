using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using ZeroMQ;
using ZeroMQ.lib;

namespace Introduce_To_Algorithm3.OpenSourceLib.NetMqs.ZeroMqs
{
    /// <summary>
    /// 建议使用NetMQ
    /// ZeroMQ strings are length-specified bytes and are sent on the wire without a trailing null. 
    /// 
    /// ZeroMQ特性：
    /// 1、异步IO，无锁数据结果
    /// 2、自动重连，组件无启动顺序ZeroMQ will automatically reconnect if the network connection is broken
    /// 3、自动queue message，当queue over-full时，自动丢弃消息或者阻塞发送者（依赖于使用方式）
    /// 4、自动处理消息边界 It delivers whole messages exactly as they were sent
    /// 5、It handles network errors intelligently, by retrying automatically in cases where it makes sense.
    /// 
    /// 
    /// 注意事项：
    /// 1、zeromq每个进程应该只创建一个ZContext实例。Technically, the context is the container for all sockets in a single process
    /// 2、不要在多线程中使用ZSocket。
    /// 3、frame是长度指定的数据，长度可以从0到内存上限。
    /// 
    /// 消息事项：
    /// 1、可以发送长度为0的消息
    /// 2、如果有多部消息zeromq保障完整发送
    /// 3、消息是由IO线程发送的，消息的长度受限于内存。
    /// </summary>
    public static class ZeroMqHelper
    {
        /// <summary>
        /// volatile 表示每次读取数据，而不是读取寄存器缓存  线程在每次使用变量的时候，都会读取变量修改后的最新值
        /// 保证共享变量可见性，不保证原子操作  lock 既保证共享变量可见性，又保证原子操作
        /// </summary>
        private static volatile bool isRunning = true;

        /// <summary>
        /// zeromq每个进程应该只创建一个ZContext实例。
        /// ZContext是多线程安全的  ZSocket不是多线程安全的
        /// </summary>
        private static volatile ZContext zContext = new ZContext();


        /// <summary>
        /// 获取zmq的版本
        /// </summary>
        /// <returns></returns>
        public static string GetZmqVersion()
        {
            int major, minor, patch;
            zmq.version(out major, out minor, out patch);
            return $"{major}.{minor}.{patch}";
        }


        #region REQ_REP 请求响应

        /// <summary>
        /// 响应  服务器端
        /// </summary>
        /// <param name="port"></param>
        /// <param name="messageHandler">不能为null，否则抛出异常,MessageHandler不能抛出异常</param>
        public static void ReqServer(int port, Func<String, String> messageHandler)
        {
            //ZContext实例
            using (ZContext context = new ZContext())
            {
                //ZSocket实例 响应
                using (ZSocket responder = new ZSocket(context, ZSocketType.REP))
                {
                    ZError zError = null;
                    bool result = responder.Bind($"tcp://*:{port}", out zError);

                    while (isRunning)
                    {
                        //接收消息
                        using (ZFrame request = responder.ReceiveFrame())
                        {
                            //Response socket always reply to the last client that sends a message.
                            String msg = request.ReadString(Encoding.UTF8);
                            String resp = messageHandler(msg);
                            responder.Send(new ZFrame(resp, Encoding.UTF8));
                        }

                    }
                }
            }
        }


        /// <summary>
        /// 发送消息客户端
        /// </summary>
        /// <param name="endPoint">端点地址：格式  tcp://127.0.0.1:5555  </param>
        /// <param name="message">消息内容</param>
        /// <param name="responseHandler">响应处理</param>
        public static void RepClient(string endPoint, string message, Action<string> responseHandler = null)
        {
            using (var context = new ZContext())
            {
                using (var requester = new ZSocket(context, ZSocketType.REQ))
                {
                    requester.Connect(endPoint);


                    //发送消息
                    requester.Send(new ZFrame(message, Encoding.UTF8));

                    using (ZFrame reply = requester.ReceiveFrame())
                    {
                        String response = reply.ReadString(Encoding.UTF8);
                        if (responseHandler != null)
                        {
                            responseHandler(response);
                        }
                    }
                }
            }
        }


        #endregion

        #region Pub 服务器单向的向客户端推送消息

        /**
         * 
         * Pub sub所有的客户端都能收到消息
         * 
         */

        /// <summary>
        /// 推送消息服务器
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="messageProducer">消息产生器,可以借助BlockingCollection</param>
        public static void PubServer(int port, Func<String> messageProducer)
        {
            using (var context = new ZContext())
            {
                using (var publisher = new ZSocket(context, ZSocketType.PUB))
                {
                    string address = $"tcp://*:{port}";
                    publisher.Bind(address);//绑定地址

                    while (isRunning)
                    {
                        string message = messageProducer();
                        using (var frame = new ZFrame(message, Encoding.UTF8))
                        {
                            publisher.Send(frame);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="address">端点地址：格式  tcp://127.0.0.1:5555 </param>
        /// <param name="messageHandler"></param>
        public static void SubClient(string address, Action<String> messageHandler = null)
        {
            using (var context = new ZContext())
            {
                using (var subscriber = new ZSocket(context, ZSocketType.SUB))
                {
                    subscriber.Connect(address);
                    //订阅带有指定前缀的消息,空字符串表示订阅所有的消息
                    subscriber.Subscribe("12");
                    while (isRunning)
                    {
                        using (var frame = subscriber.ReceiveFrame())
                        {
                            //返回的消息包含前缀
                            string message = frame.ReadString(Encoding.UTF8);
                            messageHandler?.Invoke(message);
                        }
                    }
                }
            }
        }

        #endregion

        #region Push_Pull

        /*
         * 模式一：Push服务器端绑定端口，发送消息；多个pull客户端连接服务器，接收信息，一条信息只能发送到一个客户端。主要用于当消费者能力不足时，提供多个消费者。
         * 
         * 模式二：公平队列，Pull作为服务器端绑定端口，接收消息，多个Push客户端发送消息到pull队列中处理
         * 
         */
         
        /// <summary>
        /// Push服务器端
        /// </summary>
        /// <param name="port"></param>
        /// <param name="messageProducer"></param>
        public static void PushServer(int port, Func<String> messageProducer)
        {
            using (var context = new ZContext())
            {
                using (var sender = new ZSocket(context, ZSocketType.PUSH))
                {
                    sender.Bind($"tcp://*:{port}");

                    while (isRunning)
                    {
                        String message = messageProducer();
                        sender.Send(new ZFrame(message, Encoding.UTF8));
                    }
                }
            }
        }
        
        /// <summary>
        /// Pull客户端，消费者  一条信息只能发送到一个客户端
        /// </summary>
        /// <param name="address">端点地址：格式  tcp://127.0.0.1:5555 </param>
        /// <param name="messageHandler"></param>
        public static void PullClient(string address, Action<String> messageHandler = null)
        {
            using (var context = new ZContext())
            {
                using (var receiver = new ZSocket(context, ZSocketType.PULL))
                {
                    receiver.Connect(address);
                    while (isRunning)
                    {
                        using (var frame = receiver.ReceiveFrame())
                        {
                            //返回的消息包含前缀
                            string message = frame.ReadString(Encoding.UTF8);
                            messageHandler?.Invoke(message);
                        }
                    }
                }
            }
        }


        #region 模式二  公平队列

        //公平队列，Pull作为服务器端绑定端口，接收消息，多个Push客户端发送消息到pull队列中处理

        /// <summary>
        /// Push客户端
        /// </summary>
        /// <param name="address">端点地址：格式  tcp://127.0.0.1:5555</param>
        /// <param name="messageProducer"></param>
        public static void PushClient(string address, Func<String> messageProducer)
        {
            using (var context = new ZContext())
            {
                using (var sender = new ZSocket(context, ZSocketType.PUSH))
                {
                    sender.Connect(address);

                    while (isRunning)
                    {
                        string message = messageProducer();
                        sender.Send(new ZFrame(message, Encoding.UTF8));
                    }
                }
            }
        }


        /// <summary>
        /// Pull服务器
        /// </summary>
        /// <param name="port"></param>
        /// <param name="messageHandler"></param>
        public static void PullServer(int port, Action<string> messageHandler)
        {
            using (var context = new ZContext())
            {
                using (var receiver = new ZSocket(context, ZSocketType.PULL))
                {
                    receiver.Bind($"tcp://*:{port}");
                    while (isRunning)
                    {
                        using (var frame = receiver.ReceiveFrame())
                        {
                            //返回的消息包含前缀
                            string message = frame.ReadString(Encoding.UTF8);
                            messageHandler?.Invoke(message);
                        }
                    }
                }
            }
        }


        #endregion

        #endregion

        #region Router_Dealer 路由分发


        /*
         * 路由分发模式：多个客户端A  多个服务端B  中间路由分发器C  C作为服务端， A连接C的路由端  B连接C的分发端。
         * 要求请求和响应是无状态的 除非使用memcached redis
         * 
         */

        /// <summary>
        /// 对应客户端A
        /// </summary>
        /// <param name="address">中间路由分发器地址 端口是routeport tcp://127.0.0.1:5559</param>
        /// <param name="message"></param>
        /// <param name="messageHandler"></param>
        public static void ReqClient(string address, string message,Action<string> messageHandler)
        {
            using (var requester = new ZSocket(zContext, ZSocketType.REQ))
            {
                requester.Connect(address);
                requester.Send(new ZFrame(message, Encoding.UTF8));
                using (ZFrame reply = requester.ReceiveFrame())
                {
                    string resp = reply.ReadString(Encoding.UTF8);
                    messageHandler?.Invoke(resp);
                }
            }
        }

        /// <summary>
        /// 工作服务器
        /// </summary>
        /// <param name="address">中介地址 端口对应dealerport tcp://127.0.0.1:5559</param>
        /// <param name="messageHandler"></param>
        public static void WorkServer(string address, Func<string,string> messageHandler)
        {
            using (ZSocket responder = new ZSocket(zContext, ZSocketType.REP))
            {
                responder.Connect(address);
                while (isRunning)
                {
                    using (ZFrame request = responder.ReceiveFrame())
                    {
                        //读取消息
                        string message = request.ReadString(Encoding.UTF8);
                        //处理消息，产生处理结果
                        string response = messageHandler(message);
                        //发送处理结果
                        responder.Send(new ZFrame(response, Encoding.UTF8));
                    }
                }
            }
        }


        /// <summary>
        /// 中间路由分发器
        /// </summary>
        /// <param name="routerPort">路由绑定端口</param>
        /// <param name="dealerPort">分发绑定端口</param>
        public static void RouterDealer(int routerPort, int dealerPort)
        {
            using (var router = new ZSocket(zContext, ZSocketType.ROUTER))
            {
                using (var dealer = new ZSocket(zContext, ZSocketType.DEALER))
                {
                    router.Bind($"tcp://*:{routerPort}");
                    dealer.Bind($"tcp://*:{dealerPort}");

                    //初始化 Poll set
                    ZPollItem poll = ZPollItem.CreateReceiver();

                    //路由
                    //switch messages between sockets
                    ZError error = null;
                    ZMessage message = null;
                    DateTime lastErrorTime = DateTime.Now.AddDays(-1);
                    int logIntervalSeconds = 19;
                    long errorCount = 0;//错误次数
                    while (isRunning)
                    {
                        if (router.PollIn(poll, out message, out error, TimeSpan.FromMilliseconds(32)))
                        {
                            //转发消息
                            dealer.Send(message);
                        }
                        else
                        {
                            #region 处理错误

                            if (error!= null && error.Number != ZError.EAGAIN.Number)
                            {
                                //Eagain表示重试
                                //记录日志
                                errorCount++;
                                if (errorCount > 1000000)
                                {
                                    errorCount = 1;
                                }
                                if (errorCount % 10 == 0)
                                {
                                    Thread.Sleep(10);
                                }

                                if ((DateTime.Now - lastErrorTime).TotalSeconds > logIntervalSeconds)
                                {
                                    lastErrorTime = DateTime.Now;
                                    NLogHelper.Error($"Router接收消息失败，错误码={error}");
                                }
                            }

                            #endregion
                        }

                        if (dealer.PollIn(poll, out message, out error, TimeSpan.FromMilliseconds(32)))
                        {
                            router.Send(message);
                        }
                        else
                        {
                            #region 处理错误

                            if (error != null && error.Number != ZError.EAGAIN.Number)
                            {
                                //Eagain表示重试
                                //记录日志
                                errorCount++;
                                if (errorCount > 1000000)
                                {
                                    errorCount = 1;
                                }
                                if (errorCount % 10 == 0)
                                {
                                    Thread.Sleep(10);
                                }

                                if ((DateTime.Now - lastErrorTime).TotalSeconds > logIntervalSeconds)
                                {
                                    lastErrorTime = DateTime.Now;
                                    NLogHelper.Error($"Router接收消息失败，错误码={error}");
                                }
                            }
                            #endregion
                        }
                        
                    }
                    

                }
            }
        }



        #endregion


    }
}

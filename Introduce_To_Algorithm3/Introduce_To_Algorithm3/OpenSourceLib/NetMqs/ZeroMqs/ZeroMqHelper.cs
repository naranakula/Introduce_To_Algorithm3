using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;
using ZeroMQ.lib;

namespace Introduce_To_Algorithm3.OpenSourceLib.NetMqs.ZeroMqs
{
    /// <summary>
    /// 建议使用NetMQ
    /// ZeroMQ strings are length-specified bytes and are sent on the wire without a trailing null. 
    /// </summary>
    public static class ZeroMqHelper
    {
        /// <summary>
        /// volatile 表示每次读取数据，而不是读取寄存器缓存  线程在每次使用变量的时候，都会读取变量修改后的最新值
        /// 保证共享变量可见性，不保证原子操作  lock 既保证共享变量可见性，又保证原子操作
        /// </summary>
        private static volatile bool isRunning = true;


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
        public static void RepClient(string endPoint, String message,Action<string> responseHandler = null)
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


        /// <summary>
        /// 推送消息服务器
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="messageProducer">消息产生器,可以借助BlockingCollection</param>
        public static void PushServer(int port, Func<String> messageProducer)
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


    }
}

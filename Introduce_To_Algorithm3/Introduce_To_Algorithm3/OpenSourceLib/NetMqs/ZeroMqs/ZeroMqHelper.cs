using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace Introduce_To_Algorithm3.OpenSourceLib.NetMqs.ZeroMqs
{
    /// <summary>
    /// 建议使用NetMQ
    /// </summary>
    public static class ZeroMqHelper
    {
        /// <summary>
        /// volatile 表示每次读取数据，而不是读取寄存器缓存  线程在每次使用变量的时候，都会读取变量修改后的最新值
        /// 保证共享变量可见性，不保证原子操作  lock 既保证共享变量可见性，又保证原子操作
        /// </summary>
        private static volatile bool isRunning = true;


        #region REQ_REP 请求响应

        /// <summary>
        /// 请求响应
        /// </summary>
        /// <param name="port"></param>
        /// <param name="messageHandler">不能为null，否则抛出异常</param>
        public static void Rep(int port, Func<String, String> messageHandler)
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
                            String msg = request.ReadString(Encoding.UTF8);
                            String resp = messageHandler(msg);
                            responder.Send(new ZFrame(resp, Encoding.UTF8));
                        }

                    }



                }
            }
        }

        #endregion
    }
}

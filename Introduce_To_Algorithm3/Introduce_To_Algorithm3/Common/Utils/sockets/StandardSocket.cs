using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils.sockets
{
    public class StandardSocket : IDisposable
    {
        /// <summary>
        /// 底层连接
        /// </summary>
        private volatile Socket socket;

        /// <summary>
        /// 远程地址端口
        /// </summary>
        private int port;

        /// <summary>
        /// 远程地址IP
        /// </summary>
        private string ip;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        private volatile bool isRunning;

        /// <summary>
        /// 超时时间,以毫秒为单位
        /// </summary>
        private int timeout = 3000;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning { get { return isRunning; } }

        /// <summary>
        /// 重试两次，间隔100ms
        /// </summary>
        private Retry retry = new Retry(2, new TimeSpan(0, 0, 0, 0, 100));

        /// <summary>
        /// 构造函数
        /// </summary>
        public StandardSocket()
        {
            ip = ConfigurationManager.AppSettings["RemoteIp"];
            port = int.Parse(ConfigurationManager.AppSettings["RemotePort"]);
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            if (socket != null)
            {
                //开始前如果已经存在连接，先释放以前的连接，然后重新建立新连接
                Stop();
            }
            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = socket.ReceiveTimeout = timeout;
                socket.Connect(ipEndPoint);
                isRunning = true;
                Log4netHelper.Info(string.Format("建立到{0}:{1}连接成功", ip, port));
            }
            catch (Exception ex)
            {
                isRunning = false;
                Log4netHelper.Error(string.Format("建立到{0}:{1}连接失败，exception={2}", ip, port, ex));
            }

            return isRunning;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buffer"></param>
        public bool Send(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                //没有数据发送，默认是发送成功的
                return true;
            }
            //如果连接没有建立，新建立连接，防止连接异常的问题。
            if (!isRunning)
            {
                Start();
            }

            try
            {
                //在阻塞情况下，send会返回整数正好等于要发送的数据数据,
                //If space is not available at the sending socket to hold the message to be transmitted, and the socket file descriptor does not have O_NONBLOCK（非阻塞状态） set, send() shall block until space is available. If space is not available at the sending socket to hold the message to be transmitted, and the socket file descriptor does have O_NONBLOCK set, send() shall fail.
                retry.Invoke(() => socket.Send(buffer, 0, buffer.Length, SocketFlags.None));
                return true;
            }
            catch (Exception ex)
            {
                //发送错误，可能是断线
                isRunning = false;
                Log4netHelper.Error(string.Format("发送数据发生错误：{0}", ex));
                return false;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="encoding"></param>
        public bool Send(string msg, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(msg);
            return Send(buffer);
        }

       
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Stop()
        {
            if (socket == null)
            {
                isRunning = false;
                return;
            }

            try
            {
                //This ensures that all data is sent and received on the connected socket before it is closed.
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                Log4netHelper.Info(string.Format("关闭{0}:{1}成功", ip, port));
            }
            catch (Exception ex)
            {
                socket.Close();
                Log4netHelper.Error(string.Format("关闭{0}:{1}时发生异常：{2}。", ip, port, ex));
            }
            finally
            {
                socket = null;
                isRunning = false;
            }
        }

        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// 获取底层连接
        /// </summary>
        /// <returns></returns>
        public Socket GetSocket()
        {
            return socket;
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        public void Receive()
        {
            if (!isRunning || socket == null)
            {
                Thread.Sleep(100);
                return;
            }
            
            if (socket.Poll(200000, SelectMode.SelectRead))
            {
                //512k
                byte[] buffer = new byte[1024 * 512];
                int count = socket.Receive(buffer, buffer.Length, SocketFlags.None);
                if (count == 0)
                {
                    isRunning = false;
                }
                else
                {
                    Encoding encoding = Encoding.GetEncoding("UTF-8");
                    Log4netHelper.Debug(string.Format("接收到数据：{0}", encoding.GetString(buffer, 0, count)));
                }
            }
        }
    }
}

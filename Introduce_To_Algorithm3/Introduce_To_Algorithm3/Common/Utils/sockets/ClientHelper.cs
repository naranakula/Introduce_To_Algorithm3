using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Com.Utility.Commons
{
    /// <summary>
    /// 客户端发送
    /// </summary>
    public class ClientHelper
    {
        /// <summary>
        /// 短连接实现
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        /// <param name="sendTimeout">发送超时时间 单位毫秒</param>
        /// <param name="receiveTimeout">接收超时时间 单位毫秒</param>
        /// <param name="sendContent">发送内容</param>
        /// <param name="sendEncoding">发送时采用的编码</param>
        /// <param name="recvEncoding">接收时采用的编码</param>
        /// <returns></returns>
        public static string ShortSend(string ip,int port,int sendTimeout,int receiveTimeout,string sendContent,Encoding sendEncoding,Encoding recvEncoding)
        {
            //发送内容不能为空
            if(string.IsNullOrEmpty(sendContent))
            {
                return string.Empty;
            }

            Socket socket = null;
            try
            {
                //表示ip地址和端口号
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip),port);
                //构建socket
                socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                socket.SendTimeout = sendTimeout;
                socket.ReceiveTimeout = receiveTimeout;
                //连接到服务器端
                socket.Connect(endPoint);
                //发送数据包
                socket.Send(sendEncoding.GetBytes(sendContent));

                //存放服务器端发回的字符
                byte[] recvBytes = new byte[1024];
                //表示接收到的字节数
                int ibytes;
                //接收到的字符串
                string recvStr = string.Empty;
                do
                {
                    //短连接实现，当服务器端socket.Close()时，ibytes为0
                    ibytes = socket.Receive(recvBytes, recvBytes.Length, SocketFlags.None);
                    recvStr += recvEncoding.GetString(recvBytes, 0, ibytes);
                    Thread.Sleep(1);
                } while (ibytes != 0);

                return recvStr;
            }
            catch (Exception ex)
            {
                throw new Exception("发送报文错误",ex);
            }
            finally
            {
                if(socket != null)
                {
                    socket.Close();
                }
            }
        }

        /// <summary>
        /// 长连接实现
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        /// <param name="sendTimeout">发送超时时间 单位毫秒</param>
        /// <param name="receiveTimeout">接收超时时间 单位毫秒</param>
        /// <param name="sendContent">发送内容</param>
        /// <param name="sendEncoding">发送时采用的编码</param>
        /// <param name="recvEncoding">接收时采用的编码</param>
        /// <returns></returns>
        public static string LongSend(string ip,int port,int sendTimeout,int receiveTimeout,string sendContent,Encoding sendEncoding,Encoding recvEncoding)
        {
            //发送内容不能为空
            if (string.IsNullOrEmpty(sendContent))
            {
                return string.Empty;
            }

            Socket socket = null;
            try
            {
                //表示ip地址和端口号
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                //构建socket
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = sendTimeout;
                socket.ReceiveTimeout = receiveTimeout;
                //连接到服务器端
                socket.Connect(endPoint);
                //发送数据包
                socket.Send(sendEncoding.GetBytes(sendContent));

                //存放服务器端发回的字符
                //byte[] recvBytes = new byte[1024];
                //接收到的字符串
                string recvStr = string.Empty;
                int i = 0;
                while (!socket.Poll(1000000, SelectMode.SelectRead))
                {
                    if(++i > 10)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                
                while (socket.Connected && socket.Available > 0)
                {
                    byte[] recvBytes = new byte[socket.Available];
                    //长连接实现，当服务器端socket.Close()时，ibytes（接收到的字节数）为0
                    int ibytes = socket.Receive(recvBytes, recvBytes.Length, SocketFlags.None);
                    recvStr += recvEncoding.GetString(recvBytes, 0, ibytes);
                    Thread.Sleep(1);
                }
                

                return recvStr;
            }
            catch (Exception ex)
            {
                throw new Exception("发送报文错误", ex);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
        }

        /// <summary>
        /// 短连接实现
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        /// <param name="sendTimeout">发送超时时间 单位毫秒</param>
        /// <param name="receiveTimeout">接收超时时间 单位毫秒</param>
        /// <param name="sendContent">发送内容</param>
        /// <param name="sendEncoding">发送时采用的编码</param>
        /// <param name="recvEncoding">接收时采用的编码</param>
        /// <returns></returns>
        public static string ShortSendSafe(string ip, int port, int sendTimeout, int receiveTimeout, string sendContent, Encoding sendEncoding, Encoding recvEncoding)
        {
            //发送内容不能为空
            if (string.IsNullOrEmpty(sendContent))
            {
                return string.Empty;
            }

            Socket socket = null;
            try
            {
                //表示ip地址和端口号
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                //构建socket
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = sendTimeout;
                socket.ReceiveTimeout = receiveTimeout;
                //连接到服务器端
                socket.Connect(endPoint);
                //发送数据包
                socket.Send(sendEncoding.GetBytes(sendContent));

                //存放服务器端发回的字符
                byte[] recvBytes = new byte[1024];
                //表示接收到的字节数
                int ibytes;
                //接收到的字符串
                string recvStr = string.Empty;
                List<Byte> list = new List<byte>();
                do
                {
                    //短连接实现，当服务器端socket.Close()时，ibytes为0
                    ibytes = socket.Receive(recvBytes, recvBytes.Length, SocketFlags.None);
                    for (int i = 0; i < ibytes;i++ )
                    {
                        list.Add(recvBytes[i]);
                    }
                        //recvStr += recvEncoding.GetString(recvBytes, 0, ibytes);
                    Thread.Sleep(1);
                } while (ibytes != 0);
                recvStr = recvEncoding.GetString(list.ToArray(), 0, list.Count);
                return recvStr;
            }
            catch (Exception ex)
            {
                throw new Exception("发送报文错误", ex);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
        }

        /// <summary>
        /// 长连接实现
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        /// <param name="sendTimeout">发送超时时间 单位毫秒</param>
        /// <param name="receiveTimeout">接收超时时间 单位毫秒</param>
        /// <param name="sendContent">发送内容</param>
        /// <param name="sendEncoding">发送时采用的编码</param>
        /// <param name="recvEncoding">接收时采用的编码</param>
        /// <returns></returns>
        public static string LongSendSafe(string ip, int port, int sendTimeout, int receiveTimeout, string sendContent, Encoding sendEncoding, Encoding recvEncoding)
        {
            //发送内容不能为空
            if (string.IsNullOrEmpty(sendContent))
            {
                return string.Empty;
            }

            Socket socket = null;
            try
            {
                //表示ip地址和端口号
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                //构建socket
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = sendTimeout;
                socket.ReceiveTimeout = receiveTimeout;
                //连接到服务器端
                socket.Connect(endPoint);
                //发送数据包
                socket.Send(sendEncoding.GetBytes(sendContent));

                //存放服务器端发回的字符
                //byte[] recvBytes = new byte[1024];
                //接收到的字符串
                string recvStr = string.Empty;
                int i = 0;
                while (!socket.Poll(1000000, SelectMode.SelectRead))
                {
                    if (++i > 10)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                List<byte> list = new List<byte>();
                while (socket.Connected && socket.Available > 0)
                {
                    byte[] recvBytes = new byte[socket.Available];
                    //长连接实现，当服务器端socket.Close()时，ibytes（接收到的字节数）为0
                    int ibytes = socket.Receive(recvBytes, recvBytes.Length, SocketFlags.None);
                    for (int j = 0; j < ibytes;j++ )
                    {
                        list.Add(recvBytes[j]);
                    }
                    Thread.Sleep(1);
                }
                recvStr = recvEncoding.GetString(list.ToArray(), 0, list.Count);

                return recvStr;
            }
            catch (Exception ex)
            {
                throw new Exception("发送报文错误", ex);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
        }
    }
}

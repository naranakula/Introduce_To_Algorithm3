using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils.sockets
{
    public class TcpClientEx
    {
        #region private member

        /// <summary>
        /// a tcp client for client
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// 需要保持一个引用，以便最后关闭
        /// 关闭TcpClient，不会关闭NetworkStream
        /// </summary>
        private NetworkStream networkStream;

        /// <summary>
        /// the ip you want to connect
        /// </summary>
        private string ip;

        /// <summary>
        /// the port you eant to connect
        /// </summary>
        private int port;

        /// <summary>
        /// set the buffer size = 4096
        /// </summary>
        private const int bufferSize =64 * 64;
        
        /// <summary>
        /// the tcp client is conected or not
        /// </summary>
        private bool isConnected;

        #endregion

        #region constructor

        /// <summary>
        /// set the ip & port using constructor
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public TcpClientEx(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        #endregion

        #region connect

        /// <summary>
        /// connect to remote
        /// </summary>
        public void Connect()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            tcpClient = new TcpClient();
            tcpClient.ReceiveTimeout = 5000;
            tcpClient.SendTimeout = 5000;
            tcpClient.Connect(ipEndPoint);
            networkStream = tcpClient.GetStream();
            isConnected = true;
        }

        /// <summary>
        /// connect to remote without throw any exception
        /// </summary>
        /// <returns>true, if successfully connected without exception; false, otherwise</returns>
        public bool ConnectSafe()
        {
            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                tcpClient = new TcpClient();
                tcpClient.Connect(ipEndPoint);
                networkStream = tcpClient.GetStream();
                isConnected = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region gettcpclient


        public TcpClient GetTcpClient()
        {
            return tcpClient;
        }

        #endregion

        #region close

        /// <summary>
        /// close the connection
        /// </summary>
        public void Close()
        {
            tcpClient.GetStream().Flush();
            tcpClient.GetStream().Close();//You must close the NetworkStream when you are through sending and receiving data.//建议先关闭stream，在关闭client  关闭NetworkStream会隐含的关闭Socket close the NetworkStream that will implicitly close the underlying socket. Closing a TcpClient does not free the resources of its NetworkStream.
            tcpClient.Close();
            //tcpClient.GetStream().Close();//You must close the NetworkStream when you are through sending and receiving data.//你必须保持一个NetStream引用，不能再这里调用GetStream()，因为client已经关闭，会抛异常。先关闭client，在关闭stream
            //Closing the tcpClient instance does not close the network stream
            //因此不建议用TcpClient，建议使用Socket
            isConnected = false;
        }

        /// <summary>
        /// close the connection without exception
        /// </summary>
        /// <returns>true, if successfully closed without any exception; false, otherwise</returns>
        public bool CloseSafe()
        {
            try
            {
                tcpClient.GetStream().Flush();
                tcpClient.GetStream().Close();//You must close the NetworkStream when you are through sending and receiving data.//建议先关闭stream，在关闭client  关闭NetworkStream会隐含的关闭Socket close the NetworkStream that will implicitly close the underlying socket. Closing a TcpClient does not free the resources of its NetworkStream.
                tcpClient.Close();
               
                isConnected = false;
                //Closing the tcpClient instance does not close the network stream
                //因此不建议用TcpClient，建议使用Socket
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region isconnected

        public bool IsConnected()
        {
            return isConnected;
        }

        #endregion

        #region send

        /// <summary>
        /// 发送消息，如果发送失败抛出异常抛出异常
        /// </summary>
        /// <param name="msg"></param>
        public void Send(string msg,Encoding encoding)
        {
            if (msg == null)
            {
                msg = string.Empty;
            }

            byte[] bytes = encoding.GetBytes(msg);
            tcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 发送消息，如果发送失败也不抛出异常
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SendSafe(string msg,Encoding encoding)
        {
            try
            {
                Send(msg,encoding);
                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex.ToString());
                return false;
            }
        }

        #endregion

        #region receive

        /// <summary>
        /// 终止符
        /// </summary>
        private byte[] terminator = Encoding.UTF8.GetBytes("##<eof>##");


        #region buffer

        /// <summary>
        /// the buffer to receive
        /// </summary>
        private byte[] buffer = new byte[64*1024];

        /// <summary>
        /// 接收到数据的offset
        /// </summary>
        private int offset;

        #endregion
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <returns>true, 如果解析了包；false,如果解析失败</returns>
        public Tuple<bool, string> receive(Encoding encoding)
        {
            try
            {
                offset = 0;
                NetworkStream stream = tcpClient.GetStream();
                Tuple<bool, int> tuple = null;
                while (true)
                {
                    int count = stream.Read(buffer, offset, buffer.Length - offset);

                    offset += count;
                    tuple = search();
                    if (tuple.Item1)
                    {
                        //找到了完整包
                        break;
                    }
                    else if (offset >= buffer.Length)
                    {
                        //buffer满了，为了防止内存占用过多和攻击，放弃解析本包
                        break;
                    }
                }

                if (tuple.Item1)
                {
                    return new Tuple<bool, string>(true, encoding.GetString(buffer, 0, tuple.Item2));
                }
                else
                {
                    return new Tuple<bool, string>(false, string.Empty);
                }
            }
            catch (Exception ex)
            {
                //this.lastException = ex;
                Log4netHelper.Error(ex.ToString());
                return new Tuple<bool, string>(false, string.Empty);
            }
        }


        public bool IsAnyDataToRead()
        {
            return networkStream.DataAvailable;// 如果远程主机处于关机状态或关闭了连接，DataAvailable 可能会引发 SocketException。
        }

        /// <summary>
        /// 在接收到的buffer中查找终止符
        /// </summary>
        /// <returns>true,如果找到，其结束位置为第二个参数；false如果未找到，第二个参数没有意义</returns>
        private Tuple<bool, int> search()
        {
            for (int i = 0; i < offset; i++)
            {
                if (buffer[i] == terminator[0])
                {
                    bool isSame = true;

                    for (int j = 0; j < terminator.Length; j++)
                    {
                        if ((i + j) >= offset)
                        {
                            //超出边界，不可能相同，返回false
                            return new Tuple<bool, int>(false, 0);
                        }
                        //对应位置不相等,i不是要查找的位置，退出循环
                        if (buffer[i + j] != terminator[j])
                        {
                            isSame = false;
                            break;
                        }
                    }
                    //找到索引位置
                    if (isSame)
                    {
                        return new Tuple<bool, int>(true, i);
                    }
                }
            }

            return new Tuple<bool, int>(false, 0);
        }


        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.HPSocket
{
    /// <summary>
    /// 表示服务器端的一个连接
    /// </summary>
    public class HPServerSocket
    {
        #region private member
        /// <summary>
        /// 底层的连接
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// 接收到的数据
        /// </summary>
        private List<byte> _receivedList;

        /// <summary>
        /// buffer缓存
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// 是否连接
        /// </summary>
        private bool isConnected;

        /// <summary>
        /// 远程的ip
        /// </summary>
        private string remoteIp;

        /// <summary>
        /// 远程的端口
        /// </summary>
        private int remotePort;
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="socket"></param>
        public HPServerSocket(Socket socket)
        {
            this._socket = socket;
            buffer = new byte[512];
            isConnected = true;
            try
            {
                isConnected = true;

                IPEndPoint endPoint = (IPEndPoint)socket.RemoteEndPoint;
                remoteIp = endPoint.Address.ToString();
                remotePort = endPoint.Port;
            }
            catch (Exception ex)
            {
                remoteIp = string.Empty;
                isConnected = false;
            }
        }

        #endregion

        #region Public Member

        /// <summary>
        /// 返回底层连接
        /// </summary>
        public Socket Socket
        {
            get
            {
                return _socket;
            }
        }

        /// <summary>
        /// 获取底层的buffer
        /// </summary>
        public byte[] Buffer
        {
            get { return buffer; }
        }

        /// <summary>
        /// 返回远程ip
        /// </summary>
        public string RemoteIp
        {
            get
            {
                return remoteIp;
            }
        }

        /// <summary>
        /// 远程端口
        /// </summary>
        public int RemotePort
        {
            get
            {
                return remotePort;
            }
        }

        #endregion
    }
}

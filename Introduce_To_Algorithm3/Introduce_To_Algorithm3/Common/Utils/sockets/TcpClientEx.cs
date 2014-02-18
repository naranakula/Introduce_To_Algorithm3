using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
            tcpClient.Connect(ipEndPoint);
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
            tcpClient.Close();
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
                tcpClient.Close();
                isConnected = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}

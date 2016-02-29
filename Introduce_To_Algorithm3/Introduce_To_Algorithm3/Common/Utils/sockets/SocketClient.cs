using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Introduce_To_Algorithm3.Common.Utils.sockets
{
    /// <summary>
    /// a helper class to do with socket
    /// </summary>
    public static class SocketClient
    {
        /// <summary>
        /// is a ip valid
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsValidIp(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                return false;
            }

            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            return Regex.IsMatch(ip, pattern);
        }

        /// <summary>
        /// is a port valid
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool IsValidPort(int port)
        {
            //port under 500 are saved for windows
            const int MINPORT = 501;
            const int MAXPORT = 65535;

            return port >= MINPORT && port <= MAXPORT;
        }

        /// <summary>
        /// check whether the iport is valid
        /// </summary>
        /// <param name="iport"></param>
        /// <param name="oport"></param>
        /// <returns></returns>
        public static bool IsValidPort(string iport, out int oport)
        {
            oport = -1;
            if (string.IsNullOrWhiteSpace(iport))
            {
                return false;
            }

            if (int.TryParse(iport, out oport))
            {
                return IsValidPort(oport);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// convert a string to ip address
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPAddress StringToIPAddress(this string ip)
        {
            return IPAddress.Parse(ip);
        }

        /// <summary>
        /// get the local machine name
        /// </summary>
        public static string LocalHostName
        {
            get { return Dns.GetHostName(); }
        }

        /// <summary>
        /// get the LAN ip
        /// </summary>
        public static string LANIP
        {
            get
            {
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                if (addressList == null || addressList.Length < 1)
                {
                    return "";
                }

                return addressList[0].ToString();
            }
        }

        /// <summary>
        /// get wan ip
        /// </summary>
        public static string WANIP
        {
            get
            {
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                if (addressList == null || addressList.Length < 2)
                {
                    return "";
                }

                return addressList[1].ToString();
            }
        }

        /// <summary>
        /// get the remote endpoint ip
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <returns></returns>
        public static string GetClientIP(Socket clientSocket)
        {
            IPEndPoint remotePoint = (IPEndPoint)clientSocket.RemoteEndPoint;
            return remotePoint.Address.ToString() + ":" + remotePoint.Port;
        }

        /// <summary>
        /// create a ipendpoint instance
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IPEndPoint CreateIPEndPoint(string ip, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            return new IPEndPoint(ipAddress, port);
        }

        /// <summary>
        /// create a universal ipendpoint instance
        /// 创建一个万能的IPEndPoint
        /// IPaddress.Any监测所有活动的网络接口
        /// 端口0：表示所有可用端口
        /// creates an IPEndpoint using any available IP address on the local computer, and any available port number.
        /// </summary>
        /// <returns></returns>
        public static IPEndPoint CreateUniversalIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Any, 0);
        }

        /// <summary>
        /// create a tcplistener instance
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static TcpListener CreateTcpListener(int port)
        {
            IPAddress ipAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            return new TcpListener(localEndPoint);
        }

        /// <summary>
        /// create a tcp socket instance
        /// </summary>
        /// <returns></returns>
        public static Socket CreateTcpSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// create a udp socket object
        /// </summary>
        /// <returns></returns>
        public static Socket CreateUdpSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        /// <summary>
        /// get the local endpoint of tcplistener
        /// </summary>
        /// <param name="tcpListener"></param>
        /// <returns></returns>
        public static IPEndPoint GetLocalPoint(TcpListener tcpListener)
        {
            return (IPEndPoint)tcpListener.LocalEndpoint;
        }

        /// <summary>
        /// get the local point ip
        /// </summary>
        /// <param name="tcpListener"></param>
        /// <returns></returns>
        public static string GetLocalPoint_IP(TcpListener tcpListener)
        {
            IPEndPoint localEndPoint = (IPEndPoint)tcpListener.LocalEndpoint;
            return localEndPoint.Address.ToString();
        }

        /// <summary>
        /// get the the port of local point
        /// </summary>
        /// <param name="tcpListener"></param>
        /// <returns></returns>
        public static int GetLocalPoint_Port(TcpListener tcpListener)
        {
            IPEndPoint localEndPoint = (IPEndPoint)tcpListener.LocalEndpoint;
            return localEndPoint.Port;
        }

        /// <summary>
        /// get <see cref="IPEndPoint"/> from a <see cref="Socket"/>
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static IPEndPoint GetLocalPoint(Socket socket)
        {
            return (IPEndPoint)socket.LocalEndPoint;
        }

        /// <summary>
        /// get local ip from a socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static string GetLocalPoint_IP(Socket socket)
        {
            IPEndPoint ipEndPoint = (IPEndPoint)socket.LocalEndPoint;
            return ipEndPoint.ToString();
        }

        /// <summary>
        /// get the local port from a socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static int GetLocalPoint_Port(Socket socket)
        {
            IPEndPoint ipEndPoint = (IPEndPoint)socket.LocalEndPoint;
            return ipEndPoint.Port;
        }

        /// <summary>
        /// bind the endpoint to socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="endPoint"></param>
        public static void BindEndPoint(Socket socket, IPEndPoint endPoint)
        {
            if (!socket.IsBound)
            {
                socket.Bind(endPoint);
            }
        }

        /// <summary>
        /// bind the endpoint
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="port"></param>
        public static void BindEndPoint(Socket socket, int port)
        {
            //create a ipendpoint
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

            if (!socket.IsBound)
            {
                socket.Bind(endPoint);
            }
        }

        /// <summary>
        /// start listen to special port
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="port"></param>
        public static void StartListen(Socket socket, int port)
        {
            StartListen(socket, port, 100);
        }

        /// <summary>
        /// start listen to special port and max length of the pending connection queue
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="port"></param>
        /// <param name="maxConn"></param>
        public static void StartListen(Socket socket, int port, int maxConn)
        {
            //create a local IPEndPoint
            IPEndPoint localPoint = new IPEndPoint(IPAddress.Any, port);

            if (!socket.IsBound)
            {
                socket.Bind(localPoint);
            }
            //The maximum length of the pending connections queue. 
            socket.Listen(maxConn);
        }


        /// <summary>
        /// start to listen all the ip of local machine at special ip 
        /// </summary>
        /// <param name="port">the port we want to listen</param>
        /// <param name="backlog">The maximum length of the pending connections queue</param>
        public static void StartListen(int port, int backlog)
        {
            //create a tcp socket
            Socket socket =  new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //create a local point
            IPEndPoint localPoint = new IPEndPoint(IPAddress.Any,port);

            if (!socket.IsBound)
            {
                socket.Bind(localPoint);
            }

            //The maximum length of the pending connections queue
            socket.Listen(backlog);
        }

        /// <summary>
        /// start to listen all the ip of local machine at special ip
        /// use 100 as the maximum length of the pending connections queue
        /// </summary>
        /// <param name="port"></param>
        public static void StartListen(int port)
        {
            StartListen(port, 100);
        }

        /// <summary>
        /// start listen to special ip and port
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="maxConn"></param>
        public static void StartListen(Socket socket, string ip, int port, int maxConn)
        {
            //create a localIPEndPoint
            IPEndPoint localPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            //bound socket to special ip
            if (!socket.IsBound)
            {
                socket.Bind(localPoint);
            }

            //the maximum length of the pending connections queue
            socket.Listen(maxConn);
        }

        /// <summary>
        /// start listen to special ip and port
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="maxConn"></param>
        public static void StartListen(string ip, int port, int maxConn)
        {
            //create a tcp socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            StartListen(socket, ip, port, maxConn);
        }

        /// <summary>
        /// connect to a server based on tcp
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool Connect(Socket socket, string ip, int port)
        {
            try
            {
                //connect to server
                socket.Connect(IPAddress.Parse(ip), port);

                //check the connection and wait 
                return socket.Poll(-1, SelectMode.SelectWrite);

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// send msg using socket synchronization
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        public static void Send(Socket socket, byte[] buffer)
        {
            socket.Send(buffer,0, buffer.Length, SocketFlags.None);
        }

        /// <summary>
        /// send msg using socket synchronization
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        /// <param name="encoding"></param>
        public static void Send(Socket socket, string msg, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(msg);
            //返回The number of bytes sent to the System.Net.Sockets.Socket.
            //返回实际发送的数目，可能没完全发送完，需要一个循环继续发送数据
            socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        /// <summary>
        /// receive message from socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <returns>the number of bytes we are received</returns>
        public static int Receive(Socket socket, byte[] buffer)
        {
            return socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
        }

#region receive

        /// <summary>
        /// receive msg
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="encoding"></param>
        public static string Receive(Socket socket, Encoding encoding)
        {
            if (socket == null || encoding == null)
            {
                throw new ArgumentNullException();
            }
            
            //pool for 1 seconds
            // true if data is available for reading; -or- true if the
            //     connection has been closed, reset, or terminated; otherwise, returns false.
            //如果socket有数据可读或者socket已经关闭，返回true
            if (!socket.Poll(1000 * 1000, SelectMode.SelectRead))
            {
                return null;
            }
            const int BUFFERSIZE = 256;

            Byte[] recvBytes = new byte[BUFFERSIZE];
            //recvBytes = new byte[socket.Available];
            int i = socket.Receive(recvBytes, recvBytes.Length, SocketFlags.None);
            if (i <= 0)
            {
                return null;
            }
            else
            {
                return encoding.GetString(recvBytes, 0, i);
            }
        }
#endregion

        /// <summary>
        /// close socket
        /// </summary>
        /// <param name="socket"></param>
        public static void Close(Socket socket)
        {
            if (socket == null)
            {
                return;
            }

            try
            {
                //This ensures that all data is sent and received on the connected socket before it is closed.
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                socket.Close();
            }

        }

    }
}

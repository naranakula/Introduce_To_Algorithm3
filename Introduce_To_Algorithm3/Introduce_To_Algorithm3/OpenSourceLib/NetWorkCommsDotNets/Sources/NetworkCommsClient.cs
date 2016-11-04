

using System;
using System.Threading;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;

namespace Introduce_To_Algorithm3.OpenSourceLib.NetWorkCommsDotNets.Sources
{
    /// <summary>
    /// NetworkComms.Net的客户端
    /// </summary>
    public static class NetworkCommsClient
    {
        /// <summary>
        /// 消息类型,用于标志那种类型的消息，值可以自定义
        /// </summary>
        public const string PacketType = "CmluMessage";


        /// <summary>
        /// 服务器Ip
        /// </summary>
        private static readonly string serverIp = "192.168.163.87";//初始化从配置文件中读取

        /// <summary>
        /// 服务器端口
        /// </summary>
        private static readonly int serverPort = 8193;//初始化从配置文件中读取

        ///// <summary>
        ///// 连接信息
        ///// </summary>
        //private static volatile ConnectionInfo connectionInfo = null;

        /// <summary>
        /// 是否进行了初始化
        /// </summary>
        private static volatile bool isInited = false;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            if (isInited == true)
            {
                return;
            }
            //Add an incoming packet handler using default SendReceiveOptions.
            // Multiple  handlers for the same packet type will be executed in the order they are added.
            //注册接收到消息的处理器   //PacketType用于标志哪种类型的消息，客户端和服务器端协议商定，标志消息类型，可以自定义
            //事件是在多线程中回调的，不能实现确定回调的线程
            NetworkComms.AppendGlobalIncomingPacketHandler<string>(PacketType, PacketHandlerAction);

            //客户端建立连接事件
            //事件是在多线程中回调的，不能实现确定回调的线程
            NetworkComms.AppendGlobalConnectionEstablishHandler(ConnectionEstablishDelegate);
            //关闭连接事件
            //事件是在多线程中回调的，不能实现确定回调的线程
            NetworkComms.AppendGlobalConnectionCloseHandler(ConnectionShutdownDelegate);
            //未处理的信息包处理
            //事件是在多线程中回调的，不能实现确定回调的线程
            NetworkComms.AppendGlobalIncomingUnmanagedPacketHandler(UnmanagedPacketHandlerDelgatePointer);


            isInited = true;
        }

        /// <summary>
        /// 未处理的信息包
        /// 事件是在多线程中回调的，不能实现确定回调的线程
        /// </summary>
        /// <param name="packetHeader"></param>
        /// <param name="connection"></param>
        /// <param name="incomingObject"></param>
        private static void UnmanagedPacketHandlerDelgatePointer(PacketHeader packetHeader, Connection connection, byte[] incomingObject)
        {
            Console.WriteLine("接收到未处理的信息包,thread id ="+Thread.CurrentThread.ManagedThreadId);
            #region 接收到未处理的信息包



            #endregion
        }

        /// <summary>
        /// 连接关闭事件
        /// 事件是在多线程中回调的，不能实现确定回调的线程
        /// </summary>
        /// <param name="connection"></param>
        private static void ConnectionShutdownDelegate(Connection connection)
        {
            Console.WriteLine("连接关闭：" + connection.ConnectionInfo.LocalEndPoint+",thread id ="+Thread.CurrentThread.ManagedThreadId);
            #region 连接关闭事件





            #endregion
        }

        /// <summary>
        /// 连接建立事件
        /// 事件是在多线程中回调的，不能实现确定回调的线程
        /// </summary>
        /// <param name="connection"></param>
        private static void ConnectionEstablishDelegate(Connection connection)
        {
            Console.WriteLine("连接建立：" + connection.ConnectionInfo.LocalEndPoint+",thread id ="+Thread.CurrentThread.ManagedThreadId);
            #region 连接建立事件


            #endregion
        }



        /// <summary>
        /// Delegate for handling incoming packets. 
        /// 处理接收到的数据包  
        /// 事件是在多线程中回调的，不能实现确定回调的线程
        /// </summary>
        /// <param name="packetHeader">消息包头部</param>
        /// <param name="connection">底层的连接</param>
        /// <param name="incomingObject">接收到的消息</param>
        private static void PacketHandlerAction(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            //解析数据包处理，并发送
            Console.WriteLine("客户端接收：" + incomingObject + ",thread id =" + Thread.CurrentThread.ManagedThreadId);

            #region 消息处理事件
            


            #endregion
        }

        /// <summary>
        /// 发送消息, 如果连接存在，则重用现有连接，如果不存在，创建连接，如果连接已经关闭，重新创建连接。
        /// 总之客户端确保采用一切措施使得发送能够成功，即使客户端先开启或者服务器中途重启
        /// 
        /// 
        /// 如果由于网络或者服务器关闭等原因发送不成功，则抛出异常
        /// </summary>
        /// <param name="messageToSend">消息不能为空</param>
        public static void Send(string messageToSend)
        {
            if (string.IsNullOrWhiteSpace(messageToSend))
            {
                return;
            }

            //if (connectionInfo == null)
            //{
            //    connectionInfo = new ConnectionInfo(serverIp,serverPort);
            //}

            ////如果连接存在，重用已经存在的连接，否则创建一个新连接
            //Connection tcpConnection = TCPConnection.GetConnection(connectionInfo);

            //tcpConnection.SendObject(MessageType,  messageToSend);

            //PacketType用于标志哪种类型的消息，客户端和服务器端协议商定，标志消息类型，可以自定义
            //如果连接未建立，建立连接，如果连接已经建立，重用现有连接，如果连接关闭，则新建连接
            NetworkComms.SendObject(PacketType,serverIp,serverPort,messageToSend);
            
        }

        /// <summary>
        /// 关闭程序时，调用释放所有的资源
        /// Shutdown all connections, threads and execute OnCommsShutdown event.
        /// should be called on application close.
        /// </summary>
        public static void ShutDown()
        {
            NetworkComms.Shutdown();
        }
    }
}

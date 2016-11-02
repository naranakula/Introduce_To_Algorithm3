using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Introduce_To_Algorithm3.OpenSourceLib.NetWorkCommsDotNets.Sources
{
    /// <summary>
    /// NetworkComms.Net的服务器端
    /// </summary>
    public static class NetworkCommsServer
    {
        /// <summary>
        /// 消息类型  数据包类型，用于标志那种类型的消息，值可以自定义
        /// </summary>
        public const string PacketType = "CmluMessage";

        /// <summary>
        /// 监听的服务器端口
        /// </summary>
        private static readonly int serverPort = 8193;

        /// <summary>
        /// 开启监听
        /// </summary>
        public static void Start()
        {
            //Add an incoming packet handler using default SendReceiveOptions.
            // Multiple  handlers for the same packet type will be executed in the order they are added.
            //注册接收到消息的处理器
            //PacketType用于标志哪种类型的消息，客户端和服务器端协议商定，标志消息类型，可以自定义
            //事件是在多线程中回调的，不能实现确定回调的线程
            NetworkComms.AppendGlobalIncomingPacketHandler<string>(PacketType, PacketHandlerAction);
            //连接建立监听 事件是在多线程中回调的，不能实现确定回调的线程
            NetworkComms.AppendGlobalConnectionEstablishHandler(ConnectionEstablishDelegate);
            //连接关闭监听 事件是在多线程中回调的，不能实现确定回调的线程
            NetworkComms.AppendGlobalConnectionCloseHandler(ConnectionShutdownDelegate);
            //未处理的信息包处理 事件是在多线程中回调的，不能实现确定回调的线程
            NetworkComms.AppendGlobalIncomingUnmanagedPacketHandler(UnmanagedPacketHandlerDelgatePointer);

            //在serverPort上开始监听消息,并返回监听的列表
            List<ConnectionListenerBase> listenerList = Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, serverPort));
            Console.WriteLine("服务器监听下列地址：");
            foreach (var listenerItem in listenerList)
            {
                Console.WriteLine(listenerItem.LocalListenEndPoint);
            }

        }

        /// <summary>
        /// 未处理的信息包  事件是在多线程中回调的，不能实现确定回调的线程
        /// </summary>
        /// <param name="packetHeader"></param>
        /// <param name="connection"></param>
        /// <param name="incomingObject"></param>
        private static void UnmanagedPacketHandlerDelgatePointer(PacketHeader packetHeader, Connection connection, byte[] incomingObject)
        {
            Console.WriteLine("接收到未处理的信息包,thread id =" + Thread.CurrentThread.ManagedThreadId);
            #region 接收到未处理的信息包

            #endregion
        }

        /// <summary>
        /// 连接关闭事件  事件是在多线程中回调的，不能实现确定回调的线程
        /// </summary>
        /// <param name="connection"></param>
        private static void ConnectionShutdownDelegate(Connection connection)
        {
            Console.WriteLine("连接关闭：" + connection.ConnectionInfo.RemoteEndPoint + ",thread id =" + Thread.CurrentThread.ManagedThreadId);
            #region 连接关闭事件
            #endregion
        }

        /// <summary>
        /// 连接建立事件  事件是在多线程中回调的，不能实现确定回调的线程
        /// </summary>
        /// <param name="connection"></param>
        private static void ConnectionEstablishDelegate(Connection connection)
        {
            Console.WriteLine("连接建立：" + connection.ConnectionInfo.RemoteEndPoint + ",thread id =" + Thread.CurrentThread.ManagedThreadId);
            #region 连接建立事件
            #endregion
        }

        /// <summary>
        /// Delegate for handling incoming packets. 
        /// 处理接收到的数据包   事件是在多线程中回调的，不能实现确定回调的线程
        /// </summary>
        /// <param name="packetHeader">消息包头部</param>
        /// <param name="connection">底层的连接</param>
        /// <param name="incomingObject">接收到的消息</param>
        private static void PacketHandlerAction(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            //解析数据包处理，并发送

            #region 消息处理事件

            #endregion

            Random rand = new Random();
            Console.WriteLine("接收到：" + incomingObject + ",thread id =" + Thread.CurrentThread.ManagedThreadId);
            foreach (var existCon in GetExistingConnection())
            {
                Console.WriteLine(existCon.ConnectionInfo.RemoteEndPoint);
            }
            if (rand.Next(1, 4) == 1)
            {
                Console.WriteLine("发送");
                connection.SendObject(PacketType,"接收到："+incomingObject);
                if (rand.Next(1, 3) == 1)
                {
                    Console.WriteLine("关闭");
                    NetworkComms.CloseAllConnections(ConnectionType.TCP);
                }
            }
        }

        /// <summary>
        /// 返回当前程序监听的EndPoint列表
        /// </summary>
        /// <returns></returns>
        public static List<EndPoint> GetExistingLocalListenEndPoints()
        {
            return Connection.ExistingLocalListenEndPoints(ConnectionType.TCP);
        }

        /// <summary>
        /// 获取当前的连接
        /// </summary>
        /// <returns></returns>
        public static List<Connection> GetExistingConnection()
        {
            // Use ApplicationLayerProtocolStatus.NetworkCommsDotNet.Connections.ApplicationLayerProtocolStatus.Undefined
            //     to match all status types 第二个参数匹配所有的类型
            return NetworkComms.GetExistingConnection(ConnectionType.TCP,ApplicationLayerProtocolStatus.Undefined);
        }

        /// <summary>
        /// 关闭所有的连接
        /// </summary>
        public static void CloseAllConnection()
        {
            NetworkComms.CloseAllConnections(ConnectionType.TCP);
        }

        /// <summary>
        /// Returns a ConnectionInfo array containing information for all connections
        /// 获取现存所有的连接，不包括已经关闭的连接
        /// </summary>
        /// <returns></returns>
        public static List<ConnectionInfo> GetAllConnectionInfo()
        {
            return NetworkComms.AllConnectionInfo(false);
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

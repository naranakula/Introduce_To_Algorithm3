using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// 有新连接的代理
    /// </summary>
    /// <param name="socket"></param>
    public delegate void OnSocketNewConnectionDelegate(INetworkSocket socket);

    /// <summary>
    /// 接收到数据包的代理
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="receivedPacket"></param>
    public delegate void OnSocketReceiveDelegate(INetworkSocket socket, Packet receivedPacket);

    /// <summary>
    /// socket发送代理
    /// </summary>
    /// <param name="socket">socket连接</param>
    /// <param name="status">发送状态</param>
    /// <param name="sendPacket">发送的数据包</param>
    public delegate void OnSocketSentDelegate(INetworkSocket socket, SendStatus status, Packet sendPacket);

    /// <summary>
    /// Socket断开连接代理
    /// </summary>
    /// <param name="socket"></param>
    public delegate void OnSocketDisconnectedDelegate(INetworkSocket socket);


    /// <summary>
    /// Socket interface
    /// </summary>
    public interface INetworkSocket
    {
        /// <summary>
        /// 断开连接
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 是否连接还活着 true if the connection is alive, otherwise false
        /// </summary>
        bool IsConnectionAlive { get; }

        /// <summary>
        /// Send given packet to the client
        /// 发送数据包
        /// </summary>
        /// <param name="packet"></param>
        void Send(Packet packet);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">要发送的数据包</param>
        /// <param name="offset">数据包的起始位置</param>
        /// <param name="dataSize">要发送的数据量</param>
        void Send(byte[] data, int offset, int dataSize);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        void Send(byte[] data);

        /// <summary>
        /// 广播数据包
        /// </summary>
        /// <param name="packet"></param>
        void Broadcast(Packet packet);

        /// <summary>
        /// 广播数据包
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="offset">数据包的起始位置</param>
        /// <param name="dataSize">数据包的数据量</param>
        void Broadcast(byte[] data, int offset, int dataSize);

        /// <summary>
        /// 广播数据
        /// </summary>
        /// <param name="data"></param>
        void Broadcast(byte[] data);

        /// <summary>
        /// IP信息
        /// </summary>
        IPInfo IPInfo { get; }

        /// <summary>
        /// return the server managing this socket
        /// </summary>
        INetworkServer Server { get; }

        /// <summary>
        /// No delay flag
        /// </summary>
        bool NoDelay { get; set; }

        /// <summary>
        /// Callback object
        /// </summary>
        INetworkSocketCallback CallBackObj { get; set; }

        /// <summary>
        /// OnNewConnected event
        /// 新连接事件
        /// </summary>
        OnSocketNewConnectionDelegate OnNewConnection { get; set; }

        /// <summary>
        /// OnReceived event
        /// 接收到数据包事件
        /// </summary>
        OnSocketReceiveDelegate OnReceived { get; set; }

        /// <summary>
        /// OnSent event
        /// 发送数据包事件
        /// </summary>
        OnSocketSentDelegate OnSent { get; set; }

        /// <summary>
        /// OnDisconnect event
        /// 连接断开事件
        /// </summary>
        OnSocketDisconnectedDelegate OnDisconnect { get; set; }
    }

}

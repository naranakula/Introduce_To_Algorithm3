using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.HP_Socket
{
    /// <summary>
    /// 通信组件服务状态，可以通过通信组件的GetState()方法来获取组件的当前服务状态
    /// </summary>
    public enum ServiceState
    {
        /// <summary>
        /// 正在启动
        /// </summary>
        Starting=0,

        /// <summary>
        /// 已经启动
        /// </summary>
        Started = 1,

        /// <summary>
        /// 正在停止
        /// </summary>
        Stoping = 2,

        /// <summary>
        /// 已经启动
        /// </summary>
        Stoped = 3,
    }

    /// <summary>
    /// Socket 操作类型，应用程序的OnError()事件中通过该参数标志是哪种操作导致的错误
    /// </summary>
    public enum SocketOperation
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Accept
        /// </summary>
        Accept = 1,

        /// <summary>
        /// Connect
        /// </summary>
        Connect = 2,

        /// <summary>
        /// Send
        /// </summary>
        Send = 3,

        /// <summary>
        /// Receive
        /// </summary>
        Receive = 4,
    }

    /// <summary>
    /// 事件通知处理结果，事件通知的返回值，不同的返回值影响通信组件的后续行为
    /// </summary>
    public enum HandleResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 0,

        /// <summary>
        /// 忽略
        /// </summary>
        Ignore = 1,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 2,
    }

    /// <summary>
    /// 名称：操作结果代码
    /// 描述：组件 Start()  / Stop()方法执行失败时，可以通过GetLastError()获取错误代码
    /// </summary>
    public enum SocketError
    {
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 0,

        /// <summary>
        /// 当前状态不允许操作
        /// </summary>
        IllegalState = 1,

        /// <summary>
        /// 非法参数
        /// </summary>
        InvalidParam = 2,

        /// <summary>
        /// 创建 SOCKET 失败
        /// </summary>
        SocketCreate = 3,

        /// <summary>
        /// 绑定 SOCKET 失败
        /// </summary>
        SocketBind = 4,

        /// <summary>
        /// 设置 SOCKET 失败
        /// </summary>
        SocketPrepare = 5,

        /// <summary>
        /// 监听 SOCKET 失败
        /// </summary>
        SocketListen = 6,

        /// <summary>
        /// 创建完成端口失败
        /// </summary>
        CPCreate = 7,

        /// <summary>
        /// 创建工作线程失败
        /// </summary>
        WorkerThreadCreate = 8,

        /// <summary>
        /// 创建监听线程失败
        /// </summary>
        DetectThreadCreate = 9,

        /// <summary>
        /// 绑定完成端口失败
        /// </summary>
        SocketAttachToCP = 10,

        /// <summary>
        /// 连接服务器失败
        /// </summary>
        ConnectServer = 11,

        /// <summary>
        /// 网络错误
        /// </summary>
        Network = 12,

        /// <summary>
        /// 数据处理错误
        /// </summary>
        DataProc = 13,

        /// <summary>
        /// 数据发送错误
        /// </summary>
        DataSend = 14,
    }

    /// <summary>
    /// 数据抓取结果，数据抓取操作的返回值
    /// </summary>
    public enum FetchResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 0,

        /// <summary>
        /// 抓取长度过长
        /// </summary>
        LengthTooLong = 1,

        /// <summary>
        /// 找不到 ConnID 对应的数据
        /// </summary>
        DataNotFound = 2,
    }

    /// <summary>
    /// 发送策略
    /// </summary>
    public enum SendPolicy
    {
        /// <summary>
        /// 打包模式(默认)
        /// 
        /// 尽量把多个发送操作的数据组合在一起发送，增加传输效率
        /// </summary>
        Pack = 0,

        /// <summary>
        /// 安全模式
        /// 尽量把多个发送操作的数据组合在一起发送，并尽量避免缓冲区溢出
        /// </summary>
        Safe = 1,

        /// <summary>
        /// 直接模式
        /// 对每一个发送操作都直接投递，适用于负载不高但要求实时性较高的场合
        /// </summary>
        Direct = 2,
    }

    /// <summary>
    /// 接受策略
    /// </summary>
    public enum RecvPolicy
    {
        /// <summary>
        /// 串行模式（模式）
        /// 对于单个连接，顺序触发OnReceive 和OnClose / OnError 事件。降低应用程序处理的复杂度，增强安全性；但同时损失一些并发性能。
        /// 对于HP-Socket的所有组件（IServer / IAgent / IClient），当连接触发了OnClose或OnError事件时，均表示连接已被关闭。并且OnClose或OnError事件只会触发一次，也就是说：如果触发了OnError事件则不会再触发OnClose事件或第二个OnError事件。
        /// </summary>
        Serial = 0,

        /// <summary>
        /// 并行策略
        /// 对于单个连接，同时收到OnReceive 和OnClose / OnError 事件时，会在不同的通信线程中同时触发这些事件，使并发性能得到提升，但应用程序需要考虑在OnReceive 的事件处理代码中，某些公共数据可能被 OnClose /OnError 的事件处理代码修改或释放的情形，程序代码逻辑会变得复
        /// </summary>
        Parallel = 1,
    }


    /****************************************************/
    /************** sockaddr结构体,udp服务器时OnAccept最后个参数可转化 **************/
    [StructLayout(LayoutKind.Sequential)]
    public struct in_addr
    {
        public ulong S_addr;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct sockaddr_in
    {
        public short sin_family;
        public ushort sin_port;
        public in_addr sin_addr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] sLibNamesin_zero;
    }

    /****************************************************/

    [StructLayout(LayoutKind.Sequential)]
    public struct WSABUF
    {
        public int Length;
        public IntPtr Buffer;
    }

    /// <summary>
    /// 支持Unicode
    /// </summary>
    public class HPSocketSdk
    {
        /// <summary>
        /// HPSocket的C文件路径
        /// </summary>
        private const string SOCKET_DLL_PATH = "HPSocket4C_U.dll";

        /*****************************************************************************************************/
        /******************************************** 公共类、接口 ********************************************/
        /*****************************************************************************************************/



        /****************************************************/
        /************** HPSocket4C.dll 回调函数 **************/
        /* Agent & Server & Clent */
        public delegate HandleResult OnSend(IntPtr connId, IntPtr pData, int length);
        public delegate HandleResult OnReceive(IntPtr connId, IntPtr pData, int length);
        public delegate HandleResult OnPullReceive(IntPtr connId, int length);
        public delegate HandleResult OnClose(IntPtr connId);
        public delegate HandleResult OnError(IntPtr connId, SocketOperation enOperation, int errorCode);

        /* Agent & Server */
        public delegate HandleResult OnShutdown();

        /* Agent & Client */
        public delegate HandleResult OnPrepareConnect(IntPtr connId /* IntPtr pClient */, uint socket);
        public delegate HandleResult OnConnect(IntPtr connId /* IntPtr pClient */);

        /* Server */
        public delegate HandleResult OnPrepareListen(IntPtr soListen);
        public delegate HandleResult OnAccept(IntPtr connId, IntPtr pClient);

        /****************************************************/
        /************** HPSocket4C.dll 导出函数 **************/

        /// <summary>
        /// 创建 TcpServer 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpServer(IntPtr pListener);

        /// <summary>
        /// 创建 TcpClient 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpClient(IntPtr pListener);

        /// <summary>
        /// 创建 TcpAgent 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpAgent(IntPtr pListener);

        /// <summary>
        /// 创建 TcpPullServer 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpPullServer(IntPtr pListener);

        /// <summary>
        /// 创建 TcpPullClient 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpPullClient(IntPtr pListener);

        /// <summary>
        /// 创建 TcpPullAgent 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpPullAgent(IntPtr pListener);

        /// <summary>
        /// 创建 UdpServer 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_UdpServer(IntPtr pListener);

        /// <summary>
        /// 创建 UdpClient 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_UdpClient(IntPtr pListener);


        /// <summary>
        /// 销毁 TcpServer 对象
        /// </summary>
        /// <param name="pServer"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpServer(IntPtr pServer);

        /// <summary>
        /// 销毁 TcpClient 对象
        /// </summary>
        /// <param name="pClient"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpClient(IntPtr pClient);

        /// <summary>
        /// 销毁 TcpAgent 对象
        /// </summary>
        /// <param name="pAgent"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpAgent(IntPtr pAgent);

        /// <summary>
        /// 销毁 TcpPullServer 对象
        /// </summary>
        /// <param name="pClient"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpPullServer(IntPtr pServer);

        /// <summary>
        /// 销毁 TcpPullClient 对象
        /// </summary>
        /// <param name="pClient"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpPullClient(IntPtr pClient);

        /// <summary>
        /// 销毁 TcpPullAgent 对象
        /// </summary>
        /// <param name="pAgent"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpPullAgent(IntPtr pAgent);

        /// <summary>
        /// 销毁 UdpServer 对象
        /// </summary>
        /// <param name="pServer"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_UdpServer(IntPtr pServer);

        /// <summary>
        /// 销毁 UdpClient 对象
        /// </summary>
        /// <param name="pClient"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_UdpClient(IntPtr pClient);


        /// <summary>
        /// 创建 TcpServerListener 对象
        /// </summary>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpServerListener();

        /// <summary>
        /// 创建 TcpClientListener 对象
        /// </summary>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpClientListener();

        /// <summary>
        /// 创建 TcpAgentListener 对象
        /// </summary>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpAgentListener();

        /// <summary>
        /// 创建 TcpPullServerListener 对象
        /// </summary>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpPullServerListener();

        /// <summary>
        /// 创建 TcpPullClientListener 对象
        /// </summary>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpPullClientListener();

        /// <summary>
        /// 创建 TcpPullAgentListener 对象
        /// </summary>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_TcpPullAgentListener();

        /// <summary>
        /// 创建 UdpServerListener 对象
        /// </summary>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_UdpServerListener();

        /// <summary>
        /// 创建 UdpClientListener 对象
        /// </summary>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr Create_HP_UdpClientListener();


        /// <summary>
        /// 销毁 TcpServerListener 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpServerListener(IntPtr pListener);

        /// <summary>
        /// 销毁 TcpClientListener 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpClientListener(IntPtr pListener);

        /// <summary>
        /// 销毁 TcpAgentListener 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpAgentListener(IntPtr pListener);


        /// <summary>
        /// 销毁 TcpPullServerListener 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpPullServerListener(IntPtr pListener);

        /// <summary>
        /// 销毁 TcpPullClientListener 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpPullClientListener(IntPtr pListener);

        /// <summary>
        /// 销毁 TcpPullAgentListener 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_TcpPullAgentListener(IntPtr pListener);

        /// <summary>
        /// 销毁 UdpServerListener 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_UdpServerListener(IntPtr pListener);

        /// <summary>
        /// 销毁 UdpClientListener 对象
        /// </summary>
        /// <param name="pListener"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void Destroy_HP_UdpClientListener(IntPtr pListener);

        /**********************************************************************************/
        /***************************** Server 回调函数设置方法 *****************************/

        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Server_OnPrepareListen(IntPtr pListener, OnPrepareListen fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Server_OnAccept(IntPtr pListener, OnAccept fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Server_OnSend(IntPtr pListener, OnSend fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Server_OnReceive(IntPtr pListener, OnReceive fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Server_OnPullReceive(IntPtr pListener, OnPullReceive fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Server_OnClose(IntPtr pListener, OnClose fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Server_OnError(IntPtr pListener, OnError fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Server_OnShutdown(IntPtr pListener, OnShutdown fn);

        /**********************************************************************************/
        /***************************** Client 回调函数设置方法 *****************************/

        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Client_OnPrepareConnect(IntPtr pListener, OnPrepareConnect fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Client_OnConnect(IntPtr pListener, OnConnect fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Client_OnSend(IntPtr pListener, OnSend fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Client_OnReceive(IntPtr pListener, OnReceive fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Client_OnPullReceive(IntPtr pListener, OnPullReceive fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Client_OnClose(IntPtr pListener, OnClose fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Client_OnError(IntPtr pListener, OnError fn);

        /**********************************************************************************/
        /****************************** Agent 回调函数设置方法 *****************************/

        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Agent_OnPrepareConnect(IntPtr pListener, OnPrepareConnect fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Agent_OnConnect(IntPtr pListener, OnConnect fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Agent_OnSend(IntPtr pListener, OnSend fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Agent_OnReceive(IntPtr pListener, OnReceive fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Agent_OnPullReceive(IntPtr pListener, OnPullReceive fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Agent_OnClose(IntPtr pListener, OnClose fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Agent_OnError(IntPtr pListener, OnError fn);
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Set_FN_Agent_OnShutdown(IntPtr pListener, OnShutdown fn);

        /**************************************************************************/
        /***************************** Server 操作方法 *****************************/

        /// <summary>
        /// 名称：启动通信组件
        /// 描述：启动服务器通信组件，启动完成后可开始接受客户端连接并收发数据
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="bindAddress">监听地址</param>
        /// <param name="uPort">监听接口</param>
        /// <returns>失败，可通过 GetLastError() 获取错误代码</returns>
        [DllImport(SOCKET_DLL_PATH, CharSet = CharSet.Unicode)]
        public static extern bool HP_Server_Start(IntPtr pServer, String bindAddress, ushort uPort);

        /// <summary>
        /// 关闭服务端通信组件，关闭完成后断开所有客户端连接并释放所有资源
        /// </summary>
        /// <param name="pServer"></param>
        /// <returns>失败，可通过 GetLastError() 获取错误代码</returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_Stop(IntPtr pServer);

        /// <summary>
        /// 用户通过该方法向指定客户端发送数据
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId">连接 ID</param>
        /// <param name="pBuffer">发送数据长度</param>
        /// <param name="length">发送数据长度</param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool HP_Server_Send(IntPtr pServer, IntPtr connId, byte[] pBuffer, int length);

        /// <summary>
        /// 用户通过该方法向指定客户端发送数据
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId">连接 ID</param>
        /// <param name="pBuffer">发送数据长度</param>
        /// <param name="length">发送数据长度</param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH, SetLastError = true)]
        public static extern bool HP_Server_Send(IntPtr pServer, IntPtr connId, IntPtr pBuffer, int length);

        /// <summary>
        /// 用户通过该方法向指定客户端发送数据
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId"></param>
        /// <param name="pBuffer"></param>
        /// <param name="length"></param>
        /// <param name="iOffset">针对pBuffer的偏移</param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool HP_Server_SendPart(IntPtr pServer, IntPtr connId, byte[] pBuffer, int length, int iOffset);

        /// <summary>
        /// 用户通过该方法向指定客户端发送数据
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId"></param>
        /// <param name="pBuffer"></param>
        /// <param name="length"></param>
        /// <param name="iOffset">针对pBuffer的偏移</param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH, SetLastError = true)]
        public static extern bool HP_Server_SendPart(IntPtr pServer, IntPtr connId, IntPtr pBuffer, int length, int iOffset);

        /// <summary>
        /// 发送多组数据
        /// 向指定连接发送多组数据
        /// TCP - 顺序发送所有数据包 
        /// UDP - 把所有数据包组合成一个数据包发送（数据包的总长度不能大于设置的 UDP 包最大长度） 
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId">连接 ID</param>
        /// <param name="pBuffers">发送缓冲区数组</param>
        /// <param name="iCount">发送缓冲区数目</param>
        /// <returns>TRUE.成功,FALSE.失败，可通过 Windows API 函数 ::GetLastError() 获取 Windows 错误代码</returns>
        [DllImport(SOCKET_DLL_PATH, SetLastError = true)]
        public static extern bool HP_Server_SendPackets(IntPtr pServer, IntPtr connId, WSABUF[] pBuffers, int iCount);


        /// <summary>
        /// 断开与某个客户端的连接
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId">连接 ID</param>
        /// <param name="bForce">是否强制断开连接</param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_Disconnect(IntPtr pServer, IntPtr connId, bool bForce);

        /// <summary>
        /// 断开超过指定时长的连接
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="dwPeriod">时长（毫秒）</param>
        /// <param name="bForce">是否强制断开连接</param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_DisconnectLongConnections(IntPtr pServer, uint dwPeriod, bool bForce);

        /// <summary>
        /// 断开超过指定时长的静默连接
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="dwPeriod">时长（毫秒）</param>
        /// <param name="bForce">是否强制断开连接</param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_DisconnectSilenceConnections(IntPtr pServer, uint dwPeriod, bool bForce);

        /******************************************************************************/
        /***************************** Server 属性访问方法 *****************************/

        /// <summary>
        /// 设置数据发送策略
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="enSendPolicy"></param>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Server_SetSendPolicy(IntPtr pServer, SendPolicy enSendPolicy);

        /// <summary>
        /// 获取数据发送策略
        /// </summary>
        /// <param name="pServer"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern SendPolicy HP_Server_GetSendPolicy(IntPtr pServer);

        /// <summary>
        /// 设置数据接受策略
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="enSendPolicy"></param>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern void HP_Server_SetRecvPolicy(IntPtr pServer, RecvPolicy enSendPolicy);

        /// <summary>
        /// 获取数据接收策略
        /// </summary>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern RecvPolicy HP_Server_GetRecvPolicy(IntPtr pServer);

        /// <summary>
        /// 设置连接的附加数据
        /// 是否为连接绑定附加数据或者绑定什么样的数据，均由应用程序只身决定
        /// 如果组件的接收策略为RP_PARALLEL，由于OnReceive事件与Onclose / OnError事件可能同时触发，请慎用这种绑定方法
        /// 
        /// 1）在OnAccept() / OnConnect() 事件中调用SetConnectionExtra(dwConnID, pExtra) 把Connection ID和应用层数据进行绑定。
        /// 2) 在OnReceive() / OnSend() 事件中调用GetConnectionExtra(dwConnID, ppExtra) 取出与Connection ID绑定的应用层数据，执行相应业务逻辑处理。
        /// 3) 在OnClose() / OnError() 事件中取消Connection ID和应用层数据的绑定，清除应用层数据并释放资源。
        /// 
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId">连接 ID</param>
        /// <param name="pExtra"></param>
        /// <returns>若返回 false 失败则为（无效的连接 ID）</returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_SetConnectionExtra(IntPtr pServer, IntPtr connId, IntPtr pExtra);

        /// <summary>
        /// 获取连接的附加数据
        /// 是否为连接绑定附加数据或者绑定什么样的数据，均由应用程序只身决定
        /// 如果组件的接收策略为RP_PARALLEL，由于OnReceive事件与Onclose / OnError事件可能同时触发，请慎用这种绑定方法
        /// 
        /// 1）在OnAccept() / OnConnect() 事件中调用SetConnectionExtra(dwConnID, pExtra) 把Connection ID和应用层数据进行绑定。
        /// 2) 在OnReceive() / OnSend() 事件中调用GetConnectionExtra(dwConnID, ppExtra) 取出与Connection ID绑定的应用层数据，执行相应业务逻辑处理。
        /// 3) 在OnClose() / OnError() 事件中取消Connection ID和应用层数据的绑定，清除应用层数据并释放资源。
        /// 
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId">连接 ID</param>
        /// <param name="pExtra">数据指针</param>
        /// <returns>若返回 false 失败则为（无效的连接 ID）</returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_GetConnectionExtra(IntPtr pServer, IntPtr connId, ref IntPtr pExtra);

        /// <summary>
        /// 检查通信组件是否已启动
        /// </summary>
        /// <param name="pServer"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_HasStarted(IntPtr pServer);

        /// <summary>
        /// 查看通信组件当前状态
        /// </summary>
        /// <param name="pServer"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern ServiceState HP_Server_GetState(IntPtr pServer);

        /// <summary>
        /// 获取最近一次失败操作的错误代码
        /// </summary>
        /// <param name="pServer"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern SocketError HP_Server_GetLastError(IntPtr pServer);

        /// <summary>
        /// 获取最近一次失败操作的错误描述
        /// </summary>
        /// <param name="pServer"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern IntPtr HP_Server_GetLastErrorDesc(IntPtr pServer);


        /// <summary>
        /// 获取连接中未发出数据的长度
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId"></param>
        /// <param name="piPending"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_GetPendingDataLength(IntPtr pServer, IntPtr connId, ref int piPending);

        /// <summary>
        /// 获取客户端连接数
        /// </summary>
        /// <param name="pServer"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern uint HP_Server_GetConnectionCount(IntPtr pServer);

        /// <summary>
        /// 获取所有连接的 CONNID
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="pIDs"></param>
        /// <param name="pdwCount"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_GetAllConnectionIDs(IntPtr pServer, IntPtr[] pIDs, ref uint pdwCount);

        /// <summary>
        /// 获取某个客户端连接时长（毫秒）
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId"></param>
        /// <param name="pdwPeriod"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_GetConnectPeriod(IntPtr pServer, IntPtr connId, ref uint pdwPeriod);

        /// <summary>
        /// 获取某个连接静默时间（毫秒）
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="connId"></param>
        /// <param name="pdwPeriod"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_GetSilencePeriod(IntPtr pServer, IntPtr connId, ref uint pdwPeriod);

        /// <summary>
        /// 获取监听 Socket 的地址信息
        /// </summary>
        /// <param name="pServer"></param>
        /// <param name="lpszAddress"></param>
        /// <param name="piAddressLen"></param>
        /// <param name="pusPort"></param>
        /// <returns></returns>
        [DllImport(SOCKET_DLL_PATH)]
        public static extern bool HP_Server_GetListenAddress(IntPtr pServer, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpszAddress, ref int piAddressLen, ref ushort pusPort);








    }

}

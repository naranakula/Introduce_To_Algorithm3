using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.SocketServers
{
    /// <summary>
    /// https://github.com/juhgiyo/EpServerEngine.cs
    /// https://github.com/vans163/Thixi
    /// https://github.com/kpro1999/SlimIOCP 
    /// </summary>
    public class IocpServer:IDisposable
    {
        #region 字段 & 属性

        /// <summary>
        /// 最大允许的客户端数
        /// </summary>
        private const int MaxClient = 100;

        /// <summary>
        /// 底层的socket服务器
        /// </summary>
        private Socket serverSocket;

        /// <summary>
        /// 当前的连接客户端数
        /// </summary>
        private volatile int currentClientCount;

        /// <summary>
        /// 用于每个I/O Socket操作的缓冲区大小
        /// </summary>
        private const int IOBufferSize = 8*1024;


        //private readonly BufferManager 

        #endregion
        
        #region IDisposable接口

        /// <summary>
        /// IDisposable接口
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }


}


//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading; 
//using LjlxIm.Model;

//namespace LjlxIm.Business
//{
//    /// <summary>  
//    /// IOCP SOCKET服务器  
//    /// </summary>  
//    public class IocpServer : IDisposable
//    {
//        const int OpsToPreAlloc = 2;
//        #region Fields 

//        /// <summary>
//        /// 数据包输入代理
//        /// </summary> 
//        public delegate void BinaryInputHandler(SocketAsyncEventArgsUserToken socketAsyncEventArgsUser, Message msg);

//        /// <summary>  
//        /// 服务器程序允许的最大客户端连接数  
//        /// </summary>  
//        private readonly int _maxClient;

//        /// <summary>  
//        /// 监听Socket，用于接受客户端的连接请求  
//        /// </summary>  
//        private Socket _serverSock;

//        /// <summary>  
//        /// 当前的连接的客户端数  
//        /// </summary>  
//        private int _clientCount;

//        /// <summary>  
//        /// 用于每个I/O Socket操作的缓冲区大小  
//        /// </summary>  
//        private int _bufferSize = 1024;

//        /// <summary>  
//        /// 信号量  
//        /// </summary>  
//        readonly Semaphore _maxAcceptedClients;

//        /// <summary>  
//        /// 缓冲区管理  
//        /// </summary>  
//        readonly BufferManager _bufferManager;

//        private bool _disposed;

//        #endregion

//        #region Properties  

//        /// <summary>
//        /// 数据输入处理
//        /// </summary>
//        public BinaryInputHandler BinaryInput { get; set; }


//        /// <summary>  
//        /// 服务器是否正在运行  
//        /// </summary>  
//        public bool IsRunning { get; private set; }
//        /// <summary>  
//        /// 监听的IP地址  
//        /// </summary>  
//        public IPAddress Address { get; }
//        /// <summary>  
//        /// 监听的端口  
//        /// </summary>  
//        public int Port { get; }
//        /// <summary>  
//        /// 通信使用的编码  
//        /// </summary>  
//        public Encoding Encoding { get; set; }

//        #endregion

//        #region Ctors   
//        /// <summary>  
//        /// 异步Socket TCP服务器  
//        /// </summary>  
//        /// <param name="localIpAddress">监听的IP地址</param>  
//        /// <param name="listenPort">监听的端口</param>  
//        /// <param name="maxClient">最大客户端数量</param>  
//        public IocpServer(IPAddress localIpAddress, int listenPort, int maxClient)
//        {
//            Address = localIpAddress;
//            Port = listenPort;
//            Encoding = Encoding.Default;

//            _maxClient = maxClient;

//            _serverSock = new Socket(localIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

//            _bufferManager = new BufferManager(_bufferSize * _maxClient * OpsToPreAlloc, _bufferSize);

//            _maxAcceptedClients = new Semaphore(_maxClient, _maxClient);
//        }

//        #endregion


//        #region 初始化  

//        /// <summary>  
//        /// 初始化函数  
//        /// </summary>  
//        public void Init()
//        {
//            _bufferManager.Inint(); 
//            for (var i = 0; i < _maxClient; i++)
//            { 
//                var readWriteEventArg = new SocketAsyncEventArgsUserToken();

//                readWriteEventArg.SocketAsyncEventArgs.Completed += OnIoCompleted;
//                readWriteEventArg.IocpServer = this;

//                _bufferManager.SetBuffer(readWriteEventArg.SocketAsyncEventArgs);

//                SocketAsyncEventArgsPool.Push(readWriteEventArg);
//            }

//            Console.WriteLine("初始化完毕...");
//        }

//        #endregion

//        #region Start  
//        /// <summary>  
//        /// 启动  
//        /// </summary>  
//        public void Start()
//        {
//            if (!IsRunning)
//            {
//                Init();
//                IsRunning = true;
//                IPEndPoint localEndPoint = new IPEndPoint(Address, Port);
//                // 创建监听socket  
//                _serverSock = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

//                if (localEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
//                {
//                    // 配置监听socket为 dual-mode (IPv4 & IPv6)   
//                    // 27 is equivalent to IPV6_V6ONLY socket option in the winsock snippet below,  
//                    _serverSock.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
//                    _serverSock.Bind(new IPEndPoint(IPAddress.IPv6Any, localEndPoint.Port));
//                }
//                else
//                {
//                    _serverSock.Bind(localEndPoint);
//                }
//                // 开始监听  
//                _serverSock.Listen(_maxClient);
//                // 在监听Socket上投递一个接受请求。  
//                StartAccept(null);
//            }
//        }
//        #endregion

//        #region Stop   
//        /// <summary>  
//        /// 停止服务  
//        /// </summary>  
//        public void Stop()
//        {
//            if (!IsRunning) return;
//            IsRunning = false;
//            _serverSock.Close();
//        }

//        #endregion


//        #region Accept  

//        /// <summary>  
//        /// 从客户端开始接受一个连接操作  
//        /// </summary>  
//        private void StartAccept(SocketAsyncEventArgs asyniar)
//        {
//            if (asyniar == null)
//            {
//                asyniar = new SocketAsyncEventArgs(); 
//                asyniar.Completed += OnAcceptCompleted;
//            }
//            else
//            {
//                asyniar.AcceptSocket = null; 
//            }

//            _maxAcceptedClients.WaitOne();
//            if (!_serverSock.AcceptAsync(asyniar))
//            {
//                ProcessAccept(asyniar);
//            }
//        }


//        /// <summary>
//        /// 获取时间戳
//        /// </summary>
//        /// <returns></returns>
//        public static long DateUnix()
//        {
//            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
//        }

//        /// <summary>  
//        /// accept 操作完成时回调函数  
//        /// </summary>  
//        /// <param name="sender">Object who raised the event.</param>   
//        /// <param name="asyncEventArgs"></param>  
//        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs asyncEventArgs)
//        { 
//            ProcessAccept(asyncEventArgs);
//        }

//        /// <summary>  
//        /// 监听Socket接受处理  
//        /// </summary>  
//        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>  
//        private void ProcessAccept(SocketAsyncEventArgs e)
//        {
//            try
//            { 
//                if (e.SocketError == SocketError.Success)
//                {
//                    var s = e.AcceptSocket;//和客户端关联的socket  
//                    if (s.Connected)
//                    {
//                        try
//                        { 
//                            Interlocked.Increment(ref _clientCount);//原子操作加1  
//                            SocketAsyncEventArgsUserToken asyniar = SocketAsyncEventArgsPool.Pop();
//                            asyniar.SocketAsyncEventArgs.AcceptSocket = s; 
//                            asyniar.SocketAsyncEventArgs.UserToken = asyniar;

//                            Log4Debug($"客户 {s.RemoteEndPoint} 连入, 共有 {_clientCount} 个连接。");

//                            AsyncSocketUserTokenList.Add(asyniar); 

//                            if (!s.ReceiveAsync(asyniar.SocketAsyncEventArgs))//投递接收请求  
//                            {
//                                ProcessReceive(asyniar.SocketAsyncEventArgs);
//                            }
//                        }
//                        catch (SocketException ex)
//                        {
//                            Log4Debug($"接收客户 {s.RemoteEndPoint} 数据出错, 异常信息： {ex} 。");
//                        }
//                        //投递下一个接受请求  
//                        StartAccept(e);
//                    }
//                }
//            }
//            catch (Exception)
//            { 
//                return;
//            } 
//        }

//        #endregion

//        #region 发送数据  

//        /// <summary>  
//        /// 异步的发送数据  
//        /// </summary>  
//        /// <param name="e"></param>  
//        /// <param name="data"></param>  
//        public void Send(SocketAsyncEventArgs e, byte[] data)
//        {
//            try
//            {
//                var userToken = e.UserToken as SocketAsyncEventArgsUserToken;
//                if (userToken == null) return;

//                if (e.SocketError == SocketError.Success)
//                {
//                    Socket s = e.AcceptSocket;//和客户端关联的socket  
//                    if (s.Connected)
//                    {
//                        Array.Copy(data, 0, e.Buffer, 0, data.Length);//设置发送数据  

//                        //e.SetBuffer(data, 0, data.Length); //设置发送数据  
//                        if (!s.SendAsync(e))//投递发送请求，这个函数有可能同步发送出去，这时返回false，并且不会引发SocketAsyncEventArgs.Completed事件  
//                        {
//                            // 同步发送时处理发送完成事件  
//                            ProcessSend(e);
//                        }
//                        else
//                        {
//                            CloseClientSocket(userToken);
//                        }
//                    }
//                }
//            }
//            catch (Exception)
//            { 
//                return;
//            } 
//        }

//        /// <summary>  
//        /// 同步的使用socket发送数据  
//        /// </summary>  
//        /// <param name="socket"></param>  
//        /// <param name="buffer"></param>  
//        /// <param name="offset"></param>  
//        /// <param name="size"></param>  
//        /// <param name="timeout"></param>  
//        public void Send(Socket socket, byte[] buffer, int offset, int size, int timeout)
//        {
//            socket.SendTimeout = 0;
//            var startTickCount = Environment.TickCount;
//            var sent = 0; // how many bytes is already sent  
//            do
//            {
//                if (Environment.TickCount > startTickCount + timeout)
//                {
//                    //throw new Exception("Timeout.");  
//                }
//                try
//                {
//                    sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
//                }
//                catch (SocketException ex)
//                {
//                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
//                    ex.SocketErrorCode == SocketError.IOPending ||
//                    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
//                    {
//                        // socket buffer is probably full, wait and try again  
//                        Thread.Sleep(30);
//                    }
//                }
//            } while (sent < size);
//        }


//        /// <summary>  
//        /// 发送完成时处理函数  
//        /// </summary>  
//        /// <param name="e">与发送完成操作相关联的SocketAsyncEventArg对象</param>  
//        private void ProcessSend(SocketAsyncEventArgs e)
//        {
//            try
//            {
//                var userToken = e.UserToken as SocketAsyncEventArgsUserToken; 
//                if (userToken == null) return;

//                if (e.SocketError == SocketError.Success)
//                {
//                    // var s = (Socket)e.SocketAsyncEventArgsUserToken; 
//                }
//                else
//                {
//                    CloseClientSocket(userToken);
//                }
//            }
//            catch (Exception)
//            { 
//                return;
//            } 
//        }

//        #endregion

//        #region 接收数据   
//        /// <summary>  
//        ///接收完成时处理函数  
//        /// </summary>  
//        /// <param name="e">与接收完成操作相关联的SocketAsyncEventArg对象</param>  
//        private void ProcessReceive(SocketAsyncEventArgs e)
//        {
//            try
//            {
//                var userToken = e.UserToken as SocketAsyncEventArgsUserToken;

//                if (userToken == null || e.AcceptSocket == null) return;
//                if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)   // 检查远程主机是否关闭连接  
//                {
//                    Socket s = e.AcceptSocket;
//                    //判断所有需接收的数据是否已经完成  
//                    if (s.Available == 0)
//                    {
//                        byte[] data = new byte[e.BytesTransferred];
//                        Array.Copy(e.Buffer, e.Offset, data, 0, data.Length);//从e.Buffer块中复制数据出来，保证它可重用  

//                        var info = Encoding.Default.GetString(data);
//                        Log4Debug($"收到 {s.RemoteEndPoint}");

//                        var user = (SocketAsyncEventArgsUserToken)e.UserToken;
//                        user.BuffStream.Write(data, 0, data.Length); //插入数据包缓冲池 

//                        Message message;
//                        while (user.BuffStream.Read(out message))
//                        {
//                            BinaryInput?.BeginInvoke(user, message, RecevieCallBack, BinaryInput);
//                        }
//                    }

//                    if (!s.ReceiveAsync(e))//为接收下一段数据，投递接收请求，这个函数有可能同步完成，这时返回false，并且不会引发SocketAsyncEventArgs.Completed事件  
//                    {
//                        //同步接收时处理接收完成事件  
//                        ProcessReceive(e);
//                    } 
//                }
//                else
//                {
//                    CloseClientSocket(userToken);
//                }
//            }
//            catch (Exception)
//            {
//                var userToken = e.UserToken as SocketAsyncEventArgsUserToken; 
//                CloseClientSocket(userToken);
//                return;
//            } 
//        }
//        void RecevieCallBack(IAsyncResult result)
//        {
//            BinaryInput.EndInvoke(result);
//        }
//        #endregion

//        #region 回调函数  

//        /// <summary>  
//        /// 当Socket上的发送或接收请求被完成时，调用此函数  
//        /// </summary>  
//        /// <param name="sender">激发事件的对象</param>  
//        /// <param name="asyncEventArgse">与发送或接收完成操作相关联的SocketAsyncEventArg对象</param>  
//        private void OnIoCompleted(object sender, SocketAsyncEventArgs asyncEventArgse)
//        { 
//            switch (asyncEventArgse.LastOperation)
//            {
//                case SocketAsyncOperation.Accept:
//                    ProcessAccept(asyncEventArgse);
//                    break;
//                case SocketAsyncOperation.Receive:
//                    ProcessReceive(asyncEventArgse);
//                    break;
//                default:
//                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
//            }
//        }

//        #endregion

//        #region Close  
//        /// <summary>  
//        /// 关闭socket连接  
//        /// </summary>  
//        /// <param name="e">SocketAsyncEventArg associated with the completed send/receive operation.</param>  
//        public void CloseClientSocket(SocketAsyncEventArgsUserToken e)
//        {
//            try
//            {
//                if (e.SocketAsyncEventArgs.AcceptSocket!=null)
//                { 
//                    var s = e.SocketAsyncEventArgs.AcceptSocket; 
//                    //  Log4Debug($"客户 {s.RemoteEndPoint} 断开连接!");

//                    CloseClientSocket(s, e); 
//                } 
//            }
//            catch (Exception)
//            { 
//                return;
//            } 
//        }

//        /// <summary>  
//        /// 关闭socket连接  
//        /// </summary>  
//        /// <param name="s"></param>  
//        /// <param name="e"></param>  
//        private void CloseClientSocket(Socket s, SocketAsyncEventArgsUserToken e)
//        { 
//            try
//            {
//                s.Shutdown(SocketShutdown.Send);
//                AsyncSocketUserTokenList.Remove(e);
//            }
//            catch (Exception)
//            {
//                // Throw if client has closed, so it is not necessary to catch.  
//            }
//            finally
//            {
//                s.Close();
//                e.SocketAsyncEventArgs.AcceptSocket = null;
//            }
//            Interlocked.Decrement(ref _clientCount);


//            _maxAcceptedClients.Release();

//            SocketAsyncEventArgsPool.Push(e);//SocketAsyncEventArg 对象被释放，压入可重用队列。   
//        }
//        #endregion

//        #region Dispose  
//        /// <summary>  
//        /// Performs application-defined tasks associated with freeing,   
//        /// releasing, or resetting unmanaged resources.  
//        /// </summary>  
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        /// <summary>  
//        /// Releases unmanaged and - optionally - managed resources  
//        /// </summary>  
//        /// <param name="disposing"><c>true</c> to release   
//        /// both managed and unmanaged resources; <c>false</c>   
//        /// to release only unmanaged resources.</param>  
//        protected virtual void Dispose(bool disposing)
//        {
//            if (_disposed) return;
//            if (disposing)
//            {
//                try
//                {
//                    Stop();
//                    // ReSharper disable once RedundantCheckBeforeAssignment
//                    if (_serverSock != null)
//                    {
//                        _serverSock = null;
//                    }
//                }
//                catch (SocketException)
//                {
//                    //TODO 事件  
//                }
//            }
//            _disposed = true;
//        }
//        #endregion

//        public void Log4Debug(string msg)
//        {
//            Console.WriteLine("notice:" + msg);
//        }

//    }
//}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.HPSocket;

namespace Introduce_To_Algorithm3.EasySocket
{
    /// <summary>
    /// 高性能服务器
    /// 采用异步实现
    /// </summary>
    public class HPServer:IDisposable
    {
        #region Private Member

        /// <summary>
        /// 服务器是否正在运行
        /// </summary>
        private volatile bool isRunning;

        /// <summary>
        /// 底层服务器实例
        /// </summary>
        private TcpListener tcpListener;

        /// <summary>
        /// 服务器绑定的端口
        /// </summary>
        private int port;

        /// <summary>
        /// 最大同时连接队列长度
        /// </summary>
        private int backlog;

        /// <summary>
        /// 服务器接受客户端连接使用的线程
        /// 该线程是前台线程
        /// </summary>
        private Thread serverThread;

        /// <summary>
        /// 接收处理线程
        /// </summary>
        private Thread receiveThread;

        /// <summary>
        /// 同步事件，确保子线程启动
        /// </summary>
        private ManualResetEvent resetEvent;

        /// <summary>
        /// 连接队列
        /// 存放新连接的队列
        /// </summary>
        private ConcurrentQueue<HPServerSocket> conQueue;

        /// <summary>
        /// 并发的包
        /// </summary>
        private ConcurrentBag<HPServerSocket> conBag; 
        #endregion

        #region 构造函数

        /// <summary>
        /// 建议使用1024-5000的端口
        /// </summary>
        /// <param name="port"></param>
        /// <param name="backlog"></param>
        public HPServer(int port,int backlog)
        {
            this.port = port;
            this.backlog = backlog;
            //连接队列
            conQueue = new ConcurrentQueue<HPServerSocket>();
            conBag = new ConcurrentBag<HPServerSocket>();
        }

        #endregion

        #region Start

        /// <summary>
        /// 启动服务器，并返回是否启动成功
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                isRunning = false;
                serverThread = new Thread(Listen){Name = "ServerThread",IsBackground = false};
                serverThread.Start();

                //完成创建服务器
                resetEvent.WaitOne();

                receiveThread = new Thread(ReceiveProxy){Name = "ReceiceThread",IsBackground = false};
                receiveThread.Start();
            }
            catch (SocketException sex)
            {
                isRunning = false;
            }
            catch (Exception ex)
            {
                isRunning = false;
            }
            finally
            {
                //释放掉锁资源
                resetEvent.Close();
                resetEvent = null;
            }

            return isRunning;
        }

        /// <summary>
        /// 接收处理
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveProxy(object obj)
        {
            while (isRunning)
            {
                HPServerSocket curSocket = null;
                try
                {
                    if (conQueue.TryDequeue(out curSocket))
                    {
                        //获取到内容
                        curSocket.Socket.BeginReceive(curSocket.Buffer, 0, curSocket.Buffer.Length, SocketFlags.None, new AsyncCallback(ReadProxy), curSocket);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (SocketException sex)
                {

                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        /// <summary>
        /// 从socket中读取数据
        /// </summary>
        /// <param name="ar"></param>
        private void ReadProxy(IAsyncResult ar)
        {
            HPServerSocket serverSocket = ar.AsyncState as HPServerSocket;
            if (serverSocket == null)
            {
                return;
            }

            try
            {
                int bytesRead = serverSocket.Socket.EndReceive(ar);

                if (bytesRead == 0)
                {
                    //客户端已经关闭
                }
                else
                {
                    //读取到了数据

                    //处理数据

                    //重新接收数据
                    serverSocket.Socket.BeginReceive(serverSocket.Buffer, 0, serverSocket.Buffer.Length,SocketFlags.None, new AsyncCallback(ReceiveProxy), serverSocket);
                }
            }
            catch (SocketException sex)
            {
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// 监听是否有新连接接入
        /// </summary>
        private void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any,port);
                tcpListener.Server.SendTimeout = 5000;
                tcpListener.Server.ReceiveTimeout = 5000;
                tcpListener.Start(backlog);
                isRunning = true;
                //通知主线程创建完毕
                resetEvent.Set();

                //休眠
                Thread.Sleep(200);

                while (isRunning)
                {
                    try
                    {

                        if (!tcpListener.Pending())
                        {
                            Thread.Sleep(10);
                            continue;
                        }

                        //AcceptSocket是阻塞的，直到有连接来到
                        Socket socket = tcpListener.AcceptSocket();

                        #region 开始处理连接

                        HPServerSocket item = new HPServerSocket(socket);
                        conQueue.Enqueue(item);
                        conBag.Add(item);
                        #endregion

                    }
                    catch (SocketException sex)
                    {
                        
                    }
                    catch (Exception)
                    {

                    }
                }


            }
            catch (Exception ex)
            {
                isRunning = false;
                //通知主线程创建完毕
                resetEvent.Set();
            }
        }

        #endregion

        #region Stop

        /// <summary>
        /// 实现IDisposable接口
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void Stop()
        {
            isRunning = false;

            Thread.Sleep(100);
        }

        #endregion
    }
}

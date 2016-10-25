﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// Implements the connection logic for the socket server
    /// </summary>
    public class Server
    {
        /// <summary>
        /// the maximum number of connections is designed to handle
        /// </summary>
        private int m_maxNumConnections;

        /// <summary>
        /// Buffer size to use for each socket I/O operation
        /// </summary>
        private int m_receiveBufferSize;

        /// <summary>
        /// represents a large reusable set of buffers for all socket operations
        /// </summary>
        private BufferManager m_bufferManager;

        /// <summary>
        /// read, write (don't alloc buffer space for accept
        /// </summary>
        private const int opsToPreAlloc = 2;

        /// <summary>
        /// The maximum length of the pending connections queue.
        /// </summary>
        private int backlog = 128;

        /// <summary>
        /// the socket used to listen for incoming connection request
        /// </summary>
        private Socket listenSocket;

        /// <summary>
        /// pool of reusable SocketAsyncEventArgs objects for write, read and accept socket operations
        /// </summary>
        private SocketAsyncEventArgsPool m_readWritePool;

        /// <summary>
        /// 连接的客户端数量
        /// </summary>
        private int m_numConnectedSockets;

        /// <summary>
        /// 信号量锁
        /// </summary>
        private SemaphoreSlim m_maxNumberAcceptedClients;

        /// <summary>
        /// Create an uninitialized server instance
        /// To start the server listening for connection requests
        /// call the Init method, then Start method
        /// </summary>
        /// <param name="maxNumConnections">The maximum number of connections is designed to handle 最大的连接数</param>
        /// <param name="receiveBufferSize">buffer size to use for each I/O operation</param>
        public Server(int maxNumConnections, int receiveBufferSize)
        {
            m_numConnectedSockets = 0;
            m_maxNumConnections = maxNumConnections;
            m_receiveBufferSize = receiveBufferSize;

            //allocate buffers such that the maximum number of socket can hava one outstanding read and write posted to the socket simultaneously
            m_bufferManager = new BufferManager(receiveBufferSize * maxNumConnections * opsToPreAlloc, receiveBufferSize);
            m_readWritePool = new SocketAsyncEventArgsPool(maxNumConnections);
            //初始化未命名的信号量,指定初始入口数和最大并发入口数。
            m_maxNumberAcceptedClients = new SemaphoreSlim(maxNumConnections, maxNumConnections);
        }

        /// <summary>
        /// Init the server by preallocating reusable buffers and context objects
        /// </summary>
        public void Init()
        {
            //Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds against memory fragmentation
            m_bufferManager.InitBuffer();

            //Preallocate pool of SocketAsyncEventArgs object
            SocketAsyncEventArgs readWriteEventArg;

            for (int i = 0; i < m_maxNumConnections; i++)
            {
                //预先分配SocketAsyncEventArgs
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                readWriteEventArg.UserToken = new AsyncUserToken();
                //assign a byte buffer from the buffer pool to the SocketAsyncEventArg object
                m_bufferManager.SetBuffer(readWriteEventArg);

                //add SocketAsyncEventArg to the pool
                m_readWritePool.Push(readWriteEventArg);
            }
        }

        /// <summary>
        /// Start the server such that it is listening for incoming connection requests.
        /// 
        /// </summary>
        /// <param name="localEndPoint">the endpoint which the server will listening to</param>
        public void Start(IPEndPoint localEndPoint)
        {
            //Create the socket which listens for incoming connections
            listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(localEndPoint);

            //Start the server with a listen backlog of 100 connections
            listenSocket.Listen(backlog);

            //accept on the listening socket
            StartAccept(null);
        }

        /// <summary>
        /// Begins an operation to accept a connection request from the client
        /// </summary>
        /// <param name="acceptEventArgs">the context object ot use when issuing the accept operation on the server's listening socket</param>
        private void StartAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs == null)
            {
                acceptEventArgs = new SocketAsyncEventArgs();
                acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                //socket must be cleared since the context object is being reused
                acceptEventArgs.AcceptSocket = null;
            }

            m_maxNumberAcceptedClients.Wait();

            //如果 I/O 操作挂起，将返回 true。操作完成时，将引发 e 参数的 SocketAsyncEventArgs.Completed 事件。
            //如果 I/O 操作同步完成，将返回 false。将不会引发 e 参数的 SocketAsyncEventArgs.Completed 事件，并且可以在方法调用返回后立即检查作为参数传递的 e 对象以检索操作的结果。
            bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArgs);
            if (!willRaiseEvent)
            {
                //同步返回了
                ProcessAccept(acceptEventArgs);
            }
        }

        /// <summary>
        /// 处理Accept 到新连接
        /// </summary>
        /// <param name="acceptEventArgs"></param>
        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            Interlocked.Increment(ref m_numConnectedSockets);

            //开始接受新的连接
            StartAccept(acceptEventArgs);


            //Get the Socket for the accepted client connection and put it into the ReadEventArg Object user token
            SocketAsyncEventArgs readEventArgs = m_readWritePool.Pop();
            ((AsyncUserToken)readEventArgs.UserToken).Socket = acceptEventArgs.AcceptSocket;

            //as soon as the client is connected, post a receive to the connection
            bool willRaiseEvent = acceptEventArgs.AcceptSocket.ReceiveAsync(readEventArgs);

            if (!willRaiseEvent)
            {
                ProcessReceive(readEventArgs);
            }

        }

        /// <summary>
        /// 处理接收
        /// </summary>
        /// <param name="readEventArgs"></param>
        private void ProcessReceive(SocketAsyncEventArgs readEventArgs)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 处理发送
        /// </summary>
        /// <param name="socketAsyncEventArgs"></param>
        private void ProcessSend(SocketAsyncEventArgs socketAsyncEventArgs)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 接收到连接的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        /// <summary>
        /// 异步IO操作完成的回调
        /// this method is called whenever a receive or send operation is completed on a socket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            //determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

    }
}

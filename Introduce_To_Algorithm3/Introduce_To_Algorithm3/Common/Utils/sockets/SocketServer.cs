using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Introduce_To_Algorithm3.Common.Utils.sockets
{
    public class SocketServer
    {
        #region private member

        /// <summary>
        /// whether the server is runing
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// the listener to watch the connection
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// the port we listen to
        /// </summary>
        private readonly int port;

        /// <summary>
        /// a secondary thread
        /// </summary>
        private Thread thread;

        /// <summary>
        /// the last exception happened
        /// </summary>
        private Exception _lastEx;

        /// <summary>
        /// action when server get an connection.
        /// </summary>
        private WaitCallback action;

        #endregion

        #region public member

        /// <summary>
        /// check the run status of server
        /// </summary>
        public bool IsAlive { get { return thread != null && thread.IsAlive; } }

        /// <summary>
        /// get the server ip
        /// </summary>
        public string ServerIP
        {
            get
            {
                if (listener != null)
                {
                    return SocketClient.GetLocalPoint_IP(listener);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// return the port we listen to
        /// </summary>
        public int Port { get { return port; } }

        /// <summary>
        /// get the last exception
        /// </summary>
        public Exception LastException { get { return _lastEx; } }

        #endregion


        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="port"></param>
        public SocketServer(int port)
        {
            this.port = port;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="port"></param>
        /// <param name="callback"></param>
        public SocketServer(int port, WaitCallback callback)
        {
            this.port = port;
            this.action = callback;
        }

        #endregion


        #region start server

        /// <summary>
        /// start a connection
        /// </summary>
        public void Start()
        {
            try
            {
                isRunning = true;
                thread = new Thread(Listen);
                thread.Start();
            }
            catch (Exception ex)
            {
                _lastEx = ex;
            }
        }

        /// <summary>
        /// a delegate to 
        /// </summary>
        private void Listen()
        {
            //construct a listener
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start(1024);

            while (isRunning)
            {
                try
                {
                    if (!listener.Pending())
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    //accept the pending connection
                    Socket client = listener.AcceptSocket();
                   // TcpClient client = listener.AcceptTcpClient();

                    if (action != null)
                    {
                        ThreadPoolHelper.ExecThreadPool(action, client);
                    }

                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    //RequestHandle requestHandle = new RequestHandle(client);
                    //ThreadPoolHelper.ExecThreadPool(ThreadProc, requestHandle);
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    _lastEx = ex;
                }
            }
        }

        ///// <summary>
        ///// the actual class to do the request
        ///// </summary>
        ///// <param name="stateInfo"></param>
        //private static void ThreadProc(Object stateInfo)
        //{
        //    
        //    Thread.CurrentThread.IsBackground = true;

        //    RequestHandle requestHandle = stateInfo as RequestHandle;

        //    if (requestHandle != null)
        //    {
        //        requestHandle.Handle();
        //    }
        //}

        #endregion

        #region shut down server

        /// <summary>
        /// shut down the server
        /// </summary>
        public void Stop()
        {
            try
            {
                isRunning = false;
                if (listener != null)
                {
                    listener.Stop();
                }

                if (thread != null)
                {
                    thread.Join(1000);
                }
            }
            catch (Exception ex)
            {
                _lastEx = ex;
            }
        }

        #endregion
    }
}

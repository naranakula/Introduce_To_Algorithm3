using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets
{
    /// <summary>
    /// a tcp Listener for server
    /// </summary>
    public class TcpListenerEx
    {
        #region private member

        /// <summary>
        /// the listener is waiting for client or not.
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// the listener for server
        /// </summary>
        private TcpListener tcpListener;

        /// <summary>
        /// the port for listener
        /// </summary>
        private int port;

        /// <summary>
        /// The maximum length of the pending connections queue.
        /// </summary>
        private const int backlog = 1024;

        /// <summary>
        /// waitcallback when receive a connection
        /// </summary>
        private Action<Socket> waitCallback;

        /// <summary>
        /// a thread for receive a connection
        /// </summary>
        private Thread tcpThread;

        /// <summary>
        /// a flag to sleep
        /// </summary>
        private int sleepFlag;
        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="port"></param>
        /// <param name="func">the action should close the connection</param>
        public TcpListenerEx(int port, Action<Socket> action)
        {
            this.port = port;
            this.waitCallback = action;
        }

        #endregion

        #region start

        /// <summary>
        /// start listenning for incoming connections
        /// </summary>
        private void Listen()
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start(backlog);
            while (isRunning)
            {
                try
                {
                    if (!tcpListener.Pending())
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    sleepFlag++;
                    Socket socket = tcpListener.AcceptSocket();
                    ThreadPool.QueueUserWorkItem((obj) => waitCallback((Socket)obj), socket);

                    if (sleepFlag % 100 == 0)
                    {
                        Thread.Sleep(10);
                    }

                    if (sleepFlag > 100000000)
                    {
                        sleepFlag = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// start listenning for incoming connections
        /// </summary>
        public void Start()
        {
            isRunning = true;
            tcpThread = new Thread(Listen);
            tcpThread.Start();
            Thread.Sleep(10);
        }

        /// <summary>
        /// start listenning for incoming connections without throw exception
        /// </summary>
        /// <returns></returns>
        public bool StartSafe()
        {
            try
            {
                Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region stop

        /// <summary>
        /// stop the listener
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            tcpThread.Join(1000);
            tcpListener.Stop();
        }

        /// <summary>
        /// stop the listener safely without throwing exception
        /// </summary>
        /// <returns></returns>
        public bool StopSafe()
        {
            try
            {
                Stop();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}

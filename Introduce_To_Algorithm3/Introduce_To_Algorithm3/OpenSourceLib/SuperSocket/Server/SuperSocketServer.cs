using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;

namespace Introduce_To_Algorithm3.OpenSourceLib
{
    /// <summary>
    /// SuperSocket的服务器类
    /// </summary>
    public class SuperSocketServer
    {
        #region private member
        /// <summary>
        /// 底层的app server类
        /// </summary>
        private AppServer _appServer;

        /// <summary>
        /// 通信接口
        /// </summary>
        private int _port;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SuperSocketServer(int port)
        {
            this._port = port;
            _appServer = new AppServer();
        }

        #endregion

        #region Public Method

        /// <summary>
        /// setup with port
        /// </summary>
        /// <returns></returns>
        public bool Setup()
        {
            _appServer.NewSessionConnected += NewSessionConnected;
            _appServer.NewRequestReceived += NewRequestReceived;
            _appServer.SessionClosed += SessionClosed;
            return _appServer.Setup(_port);
        }

        /// <summary>
        /// start appServer
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            return _appServer.Start();
        }

        /// <summary>
        /// stop server
        /// </summary>
        public void Stop()
        {
            _appServer.Stop();
        }

        #endregion


        #region Callback


        /// <summary>
        /// occurs a full request message received
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        private void NewRequestReceived(AppSession session, global::SuperSocket.SocketBase.Protocol.StringRequestInfo requestInfo)
        {
        }

        /// <summary>
        /// oocurs after a new session connected
        /// </summary>
        /// <param name="session"></param>
        private void NewSessionConnected(AppSession session)
        {
        }


        /// <summary>
        /// occurs when session closed
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        private void SessionClosed(AppSession session, CloseReason value)
        {
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.HP_Socket
{
    public class TcpClientEvent
    {
        public delegate HandleResult OnPrepareConnectEventHandler(TcpClient sender, uint socket);
        public delegate HandleResult OnConnectEventHandler(TcpClient sender);
        public delegate HandleResult OnSendEventHandler(TcpClient sender, IntPtr pData, int length);
        public delegate HandleResult OnReceiveEventHandler(TcpClient sender, IntPtr pData, int length);
        public delegate HandleResult OnCloseEventHandler(TcpClient sender);
        public delegate HandleResult OnErrorEventHandler(TcpClient sender, SocketOperation enOperation, int errorCode);
    }

    public class TcpClient
    {
        protected IntPtr _pClient = IntPtr.Zero;

        protected IntPtr pClient
        {
            get
            {
                return _pClient;
            }

            set
            {
                _pClient = value;
            }
        }

        protected IntPtr pListener = IntPtr.Zero;

        protected bool IsCreate = false;

        /****************************************************/
        /// <summary>
        /// 准备连接了事件
        /// </summary>
        public event TcpClientEvent.OnPrepareConnectEventHandler OnPrepareConnect;
        /// <summary>
        /// 连接事件
        /// </summary>
        public event TcpClientEvent.OnConnectEventHandler OnConnect;
        /// <summary>
        /// 数据发送事件
        /// </summary>
        public event TcpClientEvent.OnSendEventHandler OnSend;
        /// <summary>
        /// 数据到达事件
        /// </summary>
        public event TcpClientEvent.OnReceiveEventHandler OnReceive;
        /// <summary>
        /// 连接关闭事件
        /// </summary>
        public event TcpClientEvent.OnCloseEventHandler OnClose;
        /// <summary>
        /// 连接发生错误事件
        /// </summary>
        public event TcpClientEvent.OnErrorEventHandler OnError;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TcpClient()
        {
            CreateListener();
        }

        ~TcpClient()
        {
            Destroy();
        }

        /// <summary>
        /// 释放TcpServer和TcpServerListener
        /// </summary>
        public virtual void Destroy()
        {
            Stop();

            if (pClient != IntPtr.Zero)
            {
                HPSocketSdk.Destroy_HP_TcpClient(pClient);
                pClient = IntPtr.Zero;
            }
            if (pListener != IntPtr.Zero)
            {
                HPSocketSdk.Destroy_HP_TcpClientListener(pListener);
                pListener = IntPtr.Zero;
            }

            IsCreate = false;
        }
        /// <summary>
        /// 停止通讯组件
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            if (IsStarted == false)
            {
                return false;
            }
            return HPSocketSdk.HP_Client_Stop(pClient);
        }

        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStarted
        {
            get
            {
                if (pClient == IntPtr.Zero)
                {
                    return false;
                }
                return HPSocketSdk.HP_Client_HasStarted(pClient);
            }
        }

        /// <summary>
        /// 创建socket监听&服务组件
        /// </summary>
        /// <param name="isUseDefaultCallback">是否使用tcpserver类默认回调函数</param>
        /// <returns></returns>
        protected  virtual  bool CreateListener()
        {
            if (IsCreate == true || pListener != IntPtr.Zero || pClient != IntPtr.Zero)
            {
                return false;
            }


            pListener = HPSocketSdk.Create_HP_TcpClientListener();
            if (pListener == IntPtr.Zero)
            {
                return false;
            }

            pClient = HPSocketSdk.Create_HP_TcpClient(pListener);
            if (pClient == IntPtr.Zero)
            {
                return false;
            }

            IsCreate = true;

            return true;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.FTP.SERVER
{
    public class FTPServer
    {
        private TcpListener FTPListener;
        /// <summary>
        /// 客户端连接数
        /// </summary>
        public ArrayList FTPClients = new ArrayList();

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning
        {
            get { return FTPListener != null; }
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                Stop();

                FTPListener = new TcpListener(IPAddress.Any,21);

                return true;
            }
            catch (Exception ex)
            {
                Stop();
                return false;
            }
        }

        /// <summary>
        /// 停止ftp
        /// </summary>
        public void Stop()
        {
            if (FTPListener != null)
            {
                FTPListener.Stop();
            }

            FTPListener = null;
        }
    }
}

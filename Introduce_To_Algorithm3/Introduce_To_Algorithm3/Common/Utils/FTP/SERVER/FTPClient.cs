using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.FTP.SERVER
{
    public class FTPClient
    {
        #region Construction

        public DateTime ConnectedTime;
        public DateTime LastInteraction;

        /// <summary>
        /// Used inside PORT method
        /// </summary>
        private IPEndPoint ClientEndPoint = null;


        public string SessioonID
        {
            get { return ConnectedTime.Ticks.ToString(); }
        }

        private Socket ClientSocket;

        public FTPUser ConnectedUser;


        public string EndPoint
        {
            get { return ClientSocket.RemoteEndPoint.ToString(); }
        }


        public bool IsConnected
        {
            get
            {
                if (ClientSocket == null || !ClientSocket.Connected)
                {
                    Disconnect();
                    return false;
                }
                
                return true;
            }
        }

        private bool DataTransferEnabled = false;
        private TcpListener DataListener = null;

        private string Rename_FilePath;
        byte[] BufferData = new byte[512];


        public FTPClient(Socket clientSocket)
        {
            this.ClientSocket = clientSocket;
            ConnectedTime = DateTime.Now;
            ConnectedUser = new FTPUser();
        }

        private void Disconnect()
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}

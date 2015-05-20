using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace Com.Utility.Commons
{
	/// <summary>
	/// FTP Client
	/// </summary>
	public class FtpClient
	{
		#region ���캯��
		/// <summary>
		/// ȱʡ���캯��
		/// </summary>
		public FtpClient()
		{
			remoteHost  = "";
			remotePath  = "";
			remoteUser  = "";
			remotePass  = "";
			remotePort  = 21;
			isConnected     = false;
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="remoteHost"></param>
		/// <param name="remotePath"></param>
		/// <param name="remoteUser"></param>
		/// <param name="remotePass"></param>
		/// <param name="remotePort"></param>
		public FtpClient( string remoteHost, string remotePath, string remoteUser, string remotePass, int remotePort )
		{
			this.remoteHost  = remoteHost;
            this.remotePath = remotePath;
            this.remoteUser = remoteUser;
            this.remotePass = remotePass;
            this.remotePort = remotePort;
			Connect();
		}
		#endregion

		#region ��½
		/// <summary>
		/// FTP������IP��ַ
		/// </summary>
		private string remoteHost;
		public string RemoteHost
		{
			get
			{
				return remoteHost;
			}
			set
			{
				remoteHost = value;
			}
		}
		/// <summary>
		/// FTP�������˿�
		/// </summary>
		private int remotePort;
		public int RemotePort
		{
			get
			{
				return remotePort;
			}
			set
			{
				remotePort = value;
			}
		}
		/// <summary>
		/// ��ǰ������Ŀ¼
		/// </summary>
		private string remotePath;
		public string RemotePath
		{
			get
			{
				return remotePath;
			}
			set
			{
				remotePath = value;
			}
		}
		/// <summary>
		/// ��¼�û��˺�
		/// </summary>
		private string remoteUser;
		public string RemoteUser
		{
			set
			{
				remoteUser = value;
			}
		}
		/// <summary>
		/// �û���¼����
		/// </summary>
		private string remotePass;
		public string RemotePass
		{
			set
			{
				remotePass = value;
			}
		}

		/// <summary>
		/// �Ƿ��¼
		/// </summary>
		private Boolean isConnected;
		public bool Connected
		{
			get
			{
				return isConnected;
			}
		}
		#endregion

		#region ����
		/// <summary>
		/// �������� 
		/// </summary>
		public void Connect()
		{
			try
			{
                socketControl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(RemoteHost), remotePort);
                // ����
                try
                {
                    socketControl.Connect(ep);
                }
                catch (Exception)
                {
                    throw new IOException("Couldn't connect to remote server");
                }

                // ��ȡӦ����
                ReadReply();
                if (iReplyCode != 220)
                {
                    DisConnect();
                    throw new IOException(reply.Substring(4));
                }

                // ��½
                SendCommand("USER " + remoteUser);
                if (!(iReplyCode == 331 || iReplyCode == 230))
                {
                    CloseSocketConnect();//�ر�����
                    throw new IOException(reply.Substring(4));
                }
                if (iReplyCode != 230)
                {
                    SendCommand("PASS " + remotePass);
                    if (!(iReplyCode == 230 || iReplyCode == 202))
                    {
                        CloseSocketConnect();//�ر�����
                        throw new IOException(reply.Substring(4));
                    }
                }
                isConnected = true;

                // �л���Ŀ¼
                ChDir(remotePath);
			}
            catch
            {
                
            }
		}
    

		/// <summary>
		/// �ر�����
		/// </summary>
		public void DisConnect()
		{
			if( socketControl != null )
			{
				SendCommand("QUIT");
			}
			CloseSocketConnect();
		}

		#endregion

		#region ����ģʽ

		/// <summary>
		/// ����ģʽ:���������͡�ASCII����
		/// </summary>
		public enum TransferType {Binary,ASCII};

		/// <summary>
		/// ���ô���ģʽ
		/// </summary>
		/// <param name="ttType">����ģʽ</param>
		public void SetTransferType(TransferType ttType)
		{
			if(ttType == TransferType.Binary)
			{
				SendCommand("TYPE I");//binary���ʹ���
			}
			else
			{
				SendCommand("TYPE A");//ASCII���ʹ���
			}
			if (iReplyCode != 200)
			{
				throw new IOException(reply.Substring(4));
			}
			else
			{
				trType = ttType;
			}
		}


		/// <summary>
		/// ��ô���ģʽ
		/// </summary>
		/// <returns>����ģʽ</returns>
		public TransferType GetTransferType()
		{
			return trType;
		}
    
		#endregion

		#region �ļ�����
		/// <summary>
		/// ����ļ��б�
		/// </summary>
		/// <param name="mask">�ļ�����ƥ���ַ���</param>
		/// <returns></returns>
		public string[] Dir(string mask)
		{
			// ��������
			if(!isConnected)
			{
				Connect();
			}

			//���������������ӵ�socket
			Socket socketData = CreateDataSocket();
   
			//��������
			SendCommand("NLST " + mask);

			//����Ӧ�����
			if(!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226))
            {
                if( reply.IndexOf("No such file or directory") > 0 )
                    return null;
				throw new IOException(reply.Substring(4));
			}

			//��ý��
			strMsg = "";
			while(true)
			{
                System.Threading.Thread.Sleep(2000);
				int iBytes = socketData.Receive(buffer, buffer.Length, 0);
			    strMsg += Encoding.GetEncoding("gb2312").GetString(buffer, 0, iBytes);
				if(iBytes < buffer.Length)
				{
					break;
				}
			}
			char[] seperator = {'\n'};
			string[] strsFileList = strMsg.Split(seperator);

            for (int i = 0; strsFileList != null && i < strsFileList.Length; i++)
            {
                strsFileList[i] = strsFileList[i].Replace("\r", "");
            }
                socketData.Close();//����socket�ر�ʱҲ���з�����
			if(iReplyCode != 226)
			{
				ReadReply();
                if (iReplyCode == 550)
                {
                    return null;
                }
				if(iReplyCode != 226)
				{
					throw new IOException(reply.Substring(4));
				}
			}
			return strsFileList;
		}
    

		/// <summary>
		/// ��ȡ�ļ���С
		/// </summary>
		/// <param name="fileName">�ļ���</param>
		/// <returns>�ļ���С</returns>
		private long GetFileSize(string fileName)
		{
			if(!isConnected)
			{
				Connect();
			}
			SendCommand("SIZE " + Path.GetFileName(fileName));
			long lSize=0;
			if(iReplyCode == 213)
			{
				lSize = Int64.Parse(reply.Substring(4));
			}
			else
			{
				throw new IOException(reply.Substring(4));
			}
			return lSize;
		}


		/// <summary>
		/// ɾ��
		/// </summary>
		/// <param name="fileName">��ɾ���ļ���</param>
		public void Delete(string fileName)
		{
			if(!isConnected)
			{
				Connect();
			}
			SendCommand("DELE "+fileName);
			if(iReplyCode != 250)
			{
				throw new IOException(reply.Substring(4));
			}
		}
    

		/// <summary>
		/// ������(������ļ����������ļ�����,�����������ļ�)
		/// </summary>
		/// <param name="oldFileName">���ļ���</param>
		/// <param name="newFileName">���ļ���</param>
		public void Rename(string oldFileName,string newFileName)
		{
			if(!isConnected)
			{
				Connect();
			}
			SendCommand("RNFR "+oldFileName);
			if(iReplyCode != 350)
			{
				throw new IOException(reply.Substring(4));
			}
			//  ������ļ�����ԭ���ļ�����,������ԭ���ļ�
			SendCommand("RNTO "+newFileName);
			if(iReplyCode != 250)
			{
				throw new IOException(reply.Substring(4));
			}
		}
		#endregion

		#region �ϴ�������
		/// <summary>
		/// ����һ���ļ�
		/// </summary>
		/// <param name="fileNameMask">�ļ�����ƥ���ַ���</param>
		/// <param name="folder">����Ŀ¼(������\����)</param>
		public void Get(string fileNameMask,string folder)
		{
			if(!isConnected)
			{
				Connect();
			}
			string[] strFiles = Dir(fileNameMask);
			foreach(string strFile in strFiles)
			{
				if(!strFile.Equals(""))//һ����˵strFiles�����һ��Ԫ�ؿ����ǿ��ַ���
				{
					Get(strFile,folder,strFile);
				}
			}
            if (isConnected)
            {
                DisConnect();
            }

            if (File.Exists(fileNameMask))
            {
                File.Delete(fileNameMask);
            }
		}

        public bool FileExists(string fileNameMask)
        {
            try
            {
                if (!isConnected)
                {
                    Connect();
                }
                string[] strFiles = Dir(fileNameMask);
                if (strFiles != null && strFiles.Length > 0)
                {
                    foreach (string fileName in strFiles)
                    {
                        if (!string.IsNullOrEmpty(fileName) &&
                            fileName.IndexOf(fileNameMask) > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }         
        }
    

		/// <summary>
		/// ����һ���ļ�
		/// </summary>
		/// <param name="remoteFileName">Ҫ���ص��ļ���</param>
		/// <param name="folder">����Ŀ¼(������\����)</param>
		/// <param name="localFileName">�����ڱ���ʱ���ļ���</param>
		public void Get(string remoteFileName,string folder,string localFileName)
		{
		    try
		    {
                remoteFileName = remoteFileName.Replace("\r", "");
                localFileName = localFileName.Replace("\r", "");
                if (!isConnected)
                {
                    Connect();
                }
                SetTransferType(TransferType.ASCII);
                if (localFileName.Equals(""))
                {
                    localFileName = remoteFileName;
                }

                FileStream output = new
                    FileStream(folder + "\\" + localFileName, FileMode.Create);
                Socket socketData = CreateDataSocket();

                SendCommand("RETR " + remoteFileName);

                if (!(iReplyCode == 150 || iReplyCode == 125
                    || iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(reply.Substring(4));
                }
                while (true)
                {
                    int iBytes = socketData.Receive(buffer, buffer.Length, 0);
                    output.Write(buffer, 0, iBytes);
                    if (iBytes <= 0)
                    {
                        break;
                    }
                }
                output.Close();
                if (socketData.Connected)
                {
                    socketData.Close();
                }
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    ReadReply();
                    if (!(iReplyCode == 226 || iReplyCode == 250))
                    {
                        throw new IOException(reply.Substring(4));
                    }
                }
                if (File.Exists(localFileName))
                {
                    File.Delete(localFileName);
                }
		    }
		    catch (Exception ex )
		    {
                Console.WriteLine( ex.Message + ex.StackTrace);
		        return ;
		    }
		}
    

		/// <summary>
		/// �ϴ�һ���ļ�
		/// </summary>
		/// <param name="folder">����Ŀ¼(������\����)</param>
		/// <param name="fileNameMask">�ļ���ƥ���ַ�(���԰���*��?)</param>
		public void Put(string folder,string fileNameMask)
		{
			string[] strFiles = Directory.GetFiles(folder,fileNameMask);
			foreach(string strFile in strFiles)
			{
				//strFile���������ļ���(����·��)
				Put(strFile);
			}
		}
    

		/// <summary>
		/// �ϴ�һ���ļ�
		/// </summary>
		/// <param name="fileName">�����ļ���</param>
		public void Put(string fileName)
		{
			if(!isConnected)
			{
				Connect();
			}
			Socket socketData = CreateDataSocket();
			SendCommand("STOR "+Path.GetFileName(fileName));
			if( !(iReplyCode == 125 || iReplyCode == 150) )
			{
				throw new IOException(reply.Substring(4));
			}
			FileStream input = new 
				FileStream(fileName,FileMode.Open);
			int iBytes = 0;
			while ((iBytes = input.Read(buffer,0,buffer.Length)) > 0)
			{
				socketData.Send(buffer, iBytes, 0);
			}
			input.Close();
			if (socketData.Connected)
			{
				socketData.Close();
			}
			if(!(iReplyCode == 226 || iReplyCode == 250))
			{
				ReadReply();
				if(!(iReplyCode == 226 || iReplyCode == 250))
				{
					throw new IOException(reply.Substring(4));
				}
			}
		}
    
		#endregion

		#region Ŀ¼����
		/// <summary>
		/// ����Ŀ¼
		/// </summary>
		/// <param name="dirName">Ŀ¼��</param>
		public void MkDir(string dirName)
		{
			if(!isConnected)
			{
				Connect();
			}
			SendCommand("MKD "+dirName);
			if(iReplyCode != 257)
			{
				throw new IOException(reply.Substring(4));
			}
		}
    
 
		/// <summary>
		/// ɾ��Ŀ¼
		/// </summary>
		/// <param name="dirName">Ŀ¼��</param>
		public void RmDir(string dirName)
		{
			if(!isConnected)
			{
				Connect();
			}
			SendCommand("RMD "+dirName);
			if(iReplyCode != 250)
			{
				throw new IOException(reply.Substring(4));
			}
		}
    
 
		/// <summary>
		/// �ı�Ŀ¼
		/// </summary>
		/// <param name="dirName">�µĹ���Ŀ¼��</param>
		public void ChDir(string dirName)
		{
			if(dirName.Equals(".") || dirName.Equals(""))
			{
				return;
			}
			if(!isConnected)
			{
				Connect();
			}
			SendCommand("CWD "+dirName);
			if(iReplyCode != 250)
			{
				throw new IOException(reply.Substring(4));
			}
			this.remotePath = dirName;
		}
    
		#endregion

		#region �ڲ�����
		/// <summary>
		/// ���������ص�Ӧ����Ϣ(����Ӧ����)
		/// </summary>
		private string strMsg;
		/// <summary>
		/// ���������ص�Ӧ����Ϣ(����Ӧ����)
		/// </summary>
		private string reply;
		/// <summary>
		/// ���������ص�Ӧ����
		/// </summary>
		private int iReplyCode;
		/// <summary>
		/// ���п������ӵ�socket
		/// </summary>
		private Socket socketControl;
		/// <summary>
		/// ����ģʽ
		/// </summary>
		private TransferType trType;
		/// <summary>
		/// ���պͷ������ݵĻ�����
		/// </summary>
		private static int BLOCK_SIZE = 512;
		Byte[] buffer = new Byte[BLOCK_SIZE];
		/// <summary>
		/// ���뷽ʽ
		/// </summary>
		Encoding ASCII = Encoding.ASCII;
		#endregion

		#region �ڲ�����
		/// <summary>
		/// ��һ��Ӧ���ַ�����¼��strReply��strMsg
		/// Ӧ�����¼��iReplyCode
		/// </summary>
		private void ReadReply()
		{
			strMsg = "";
			reply = ReadLine();
			iReplyCode = Int32.Parse(reply.Substring(0,3));
		}

		/// <summary>
		/// ���������������ӵ�socket
		/// </summary>
		/// <returns>��������socket</returns>
		private Socket CreateDataSocket()
		{
			SendCommand("PASV");
			if(iReplyCode != 227)
			{
				throw new IOException(reply.Substring(4));
			}
			int index1 = reply.IndexOf('(');
			int index2 = reply.IndexOf(')');
			string ipData = 
				reply.Substring(index1+1,index2-index1-1);
			int[] parts = new int[6];
			int len = ipData.Length;
			int partCount = 0;
			string buf="";
			for (int i = 0; i < len && partCount <= 6; i++)
			{
				char ch = Char.Parse(ipData.Substring(i,1));
				if (Char.IsDigit(ch))
					buf+=ch;
				else if (ch != ',')
				{
					throw new IOException("Malformed PASV strReply: " + 
						reply);
				}
				if (ch == ',' || i+1 == len)
				{
					try
					{
						parts[partCount++] = Int32.Parse(buf);
						buf="";
					}
					catch (Exception)
					{
						throw new IOException("Malformed PASV strReply: " + 
							reply);
					}
				}
			}
			string ipAddress = parts[0] + "."+ parts[1]+ "." +
				parts[2] + "." + parts[3];
			int port = (parts[4] << 8) + parts[5];
			Socket s = new 
				Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
			IPEndPoint ep = new 
				IPEndPoint(IPAddress.Parse(ipAddress), port);
			try
			{
				s.Connect(ep);
			}
			catch(Exception)
			{
				throw new IOException("Can't connect to remote server");
			}
			return s;
		}


		/// <summary>
		/// �ر�socket����(���ڵ�¼��ǰ)
		/// </summary>
		private void CloseSocketConnect()
		{
			if(socketControl!=null)
			{
				socketControl.Close();
				socketControl = null;
			}
			isConnected = false;
		}
 
		/// <summary>
		/// ��ȡSocket���ص������ַ���
		/// </summary>
		/// <returns>����Ӧ������ַ�����</returns>
		private string ReadLine()
		{
			while(true)
			{
				int iBytes = socketControl.Receive(buffer, buffer.Length, 0);
                strMsg += Encoding.GetEncoding("gb2312").GetString(buffer, 0, iBytes);
				if(iBytes < buffer.Length)
				{
					break;
				}
			}
			char[] seperator = {'\n'};
			string[] mess = strMsg.Split(seperator);
			this.strMsg = this.strMsg.Length > 2 ? mess[mess.Length-2] : mess[0];

			if(!strMsg.Substring(3,1).Equals(" "))//�����ַ�����ȷ������Ӧ����(��220��ͷ,�����һ�ո�,�ٽ��ʺ��ַ���)
			{
				return ReadLine();
			}
			return strMsg;
		}


		/// <summary>
		/// ���������ȡӦ��������һ��Ӧ���ַ���
		/// </summary>
		/// <param name="command">����</param>
		private void SendCommand(String command)
		{
			Byte[] cmdBytes = Encoding.ASCII.GetBytes((command+"\r\n").ToCharArray());
			socketControl.Send(cmdBytes, cmdBytes.Length, 0);
			ReadReply();
		}

		#endregion
	}
}

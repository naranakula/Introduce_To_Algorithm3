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
		#region 构造函数
		/// <summary>
		/// 缺省构造函数
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
		/// 构造函数
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

		#region 登陆
		/// <summary>
		/// FTP服务器IP地址
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
		/// FTP服务器端口
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
		/// 当前服务器目录
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
		/// 登录用户账号
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
		/// 用户登录密码
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
		/// 是否登录
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

		#region 链接
		/// <summary>
		/// 建立连接 
		/// </summary>
		public void Connect()
		{
			try
			{
                socketControl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(RemoteHost), remotePort);
                // 链接
                try
                {
                    socketControl.Connect(ep);
                }
                catch (Exception)
                {
                    throw new IOException("Couldn't connect to remote server");
                }

                // 获取应答码
                ReadReply();
                if (iReplyCode != 220)
                {
                    DisConnect();
                    throw new IOException(reply.Substring(4));
                }

                // 登陆
                SendCommand("USER " + remoteUser);
                if (!(iReplyCode == 331 || iReplyCode == 230))
                {
                    CloseSocketConnect();//关闭连接
                    throw new IOException(reply.Substring(4));
                }
                if (iReplyCode != 230)
                {
                    SendCommand("PASS " + remotePass);
                    if (!(iReplyCode == 230 || iReplyCode == 202))
                    {
                        CloseSocketConnect();//关闭连接
                        throw new IOException(reply.Substring(4));
                    }
                }
                isConnected = true;

                // 切换到目录
                ChDir(remotePath);
			}
            catch
            {
                
            }
		}
    

		/// <summary>
		/// 关闭连接
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

		#region 传输模式

		/// <summary>
		/// 传输模式:二进制类型、ASCII类型
		/// </summary>
		public enum TransferType {Binary,ASCII};

		/// <summary>
		/// 设置传输模式
		/// </summary>
		/// <param name="ttType">传输模式</param>
		public void SetTransferType(TransferType ttType)
		{
			if(ttType == TransferType.Binary)
			{
				SendCommand("TYPE I");//binary类型传输
			}
			else
			{
				SendCommand("TYPE A");//ASCII类型传输
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
		/// 获得传输模式
		/// </summary>
		/// <returns>传输模式</returns>
		public TransferType GetTransferType()
		{
			return trType;
		}
    
		#endregion

		#region 文件操作
		/// <summary>
		/// 获得文件列表
		/// </summary>
		/// <param name="mask">文件名的匹配字符串</param>
		/// <returns></returns>
		public string[] Dir(string mask)
		{
			// 建立链接
			if(!isConnected)
			{
				Connect();
			}

			//建立进行数据连接的socket
			Socket socketData = CreateDataSocket();
   
			//传送命令
			SendCommand("NLST " + mask);

			//分析应答代码
			if(!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226))
            {
                if( reply.IndexOf("No such file or directory") > 0 )
                    return null;
				throw new IOException(reply.Substring(4));
			}

			//获得结果
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
                socketData.Close();//数据socket关闭时也会有返回码
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
		/// 获取文件大小
		/// </summary>
		/// <param name="fileName">文件名</param>
		/// <returns>文件大小</returns>
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
		/// 删除
		/// </summary>
		/// <param name="fileName">待删除文件名</param>
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
		/// 重命名(如果新文件名与已有文件重名,将覆盖已有文件)
		/// </summary>
		/// <param name="oldFileName">旧文件名</param>
		/// <param name="newFileName">新文件名</param>
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
			//  如果新文件名与原有文件重名,将覆盖原有文件
			SendCommand("RNTO "+newFileName);
			if(iReplyCode != 250)
			{
				throw new IOException(reply.Substring(4));
			}
		}
		#endregion

		#region 上传和下载
		/// <summary>
		/// 下载一批文件
		/// </summary>
		/// <param name="fileNameMask">文件名的匹配字符串</param>
		/// <param name="folder">本地目录(不得以\结束)</param>
		public void Get(string fileNameMask,string folder)
		{
			if(!isConnected)
			{
				Connect();
			}
			string[] strFiles = Dir(fileNameMask);
			foreach(string strFile in strFiles)
			{
				if(!strFile.Equals(""))//一般来说strFiles的最后一个元素可能是空字符串
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
		/// 下载一个文件
		/// </summary>
		/// <param name="remoteFileName">要下载的文件名</param>
		/// <param name="folder">本地目录(不得以\结束)</param>
		/// <param name="localFileName">保存在本地时的文件名</param>
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
		/// 上传一批文件
		/// </summary>
		/// <param name="folder">本地目录(不得以\结束)</param>
		/// <param name="fileNameMask">文件名匹配字符(可以包含*和?)</param>
		public void Put(string folder,string fileNameMask)
		{
			string[] strFiles = Directory.GetFiles(folder,fileNameMask);
			foreach(string strFile in strFiles)
			{
				//strFile是完整的文件名(包含路径)
				Put(strFile);
			}
		}
    

		/// <summary>
		/// 上传一个文件
		/// </summary>
		/// <param name="fileName">本地文件名</param>
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

		#region 目录操作
		/// <summary>
		/// 创建目录
		/// </summary>
		/// <param name="dirName">目录名</param>
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
		/// 删除目录
		/// </summary>
		/// <param name="dirName">目录名</param>
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
		/// 改变目录
		/// </summary>
		/// <param name="dirName">新的工作目录名</param>
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

		#region 内部变量
		/// <summary>
		/// 服务器返回的应答信息(包含应答码)
		/// </summary>
		private string strMsg;
		/// <summary>
		/// 服务器返回的应答信息(包含应答码)
		/// </summary>
		private string reply;
		/// <summary>
		/// 服务器返回的应答码
		/// </summary>
		private int iReplyCode;
		/// <summary>
		/// 进行控制连接的socket
		/// </summary>
		private Socket socketControl;
		/// <summary>
		/// 传输模式
		/// </summary>
		private TransferType trType;
		/// <summary>
		/// 接收和发送数据的缓冲区
		/// </summary>
		private static int BLOCK_SIZE = 512;
		Byte[] buffer = new Byte[BLOCK_SIZE];
		/// <summary>
		/// 编码方式
		/// </summary>
		Encoding ASCII = Encoding.ASCII;
		#endregion

		#region 内部函数
		/// <summary>
		/// 将一行应答字符串记录在strReply和strMsg
		/// 应答码记录在iReplyCode
		/// </summary>
		private void ReadReply()
		{
			strMsg = "";
			reply = ReadLine();
			iReplyCode = Int32.Parse(reply.Substring(0,3));
		}

		/// <summary>
		/// 建立进行数据连接的socket
		/// </summary>
		/// <returns>数据连接socket</returns>
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
		/// 关闭socket连接(用于登录以前)
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
		/// 读取Socket返回的所有字符串
		/// </summary>
		/// <returns>包含应答码的字符串行</returns>
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

			if(!strMsg.Substring(3,1).Equals(" "))//返回字符串正确的是以应答码(如220开头,后面接一空格,再接问候字符串)
			{
				return ReadLine();
			}
			return strMsg;
		}


		/// <summary>
		/// 发送命令并获取应答码和最后一行应答字符串
		/// </summary>
		/// <param name="command">命令</param>
		private void SendCommand(String command)
		{
			Byte[] cmdBytes = Encoding.ASCII.GetBytes((command+"\r\n").ToCharArray());
			socketControl.Send(cmdBytes, cmdBytes.Length, 0);
			ReadReply();
		}

		#endregion
	}
}

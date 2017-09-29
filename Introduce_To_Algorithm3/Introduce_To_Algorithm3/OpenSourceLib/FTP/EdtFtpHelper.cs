using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.FtpClient;
using System.Text;
using System.Threading.Tasks;
using EnterpriseDT.Net.Ftp;

namespace Introduce_To_Algorithm3.OpenSourceLib.FTP
{


    /// <summary>
    /// 建议先看FluentFTP
    /// FluentFtp更好
    /// 
    /// 使用edtFTPnet/Free进行ftp操作
    /// Nuget的版本很旧，从官网下载
    /// 
    /// http://enterprisedt.com/products/edtftpnet/
    /// https://www.limilabs.com
    /// 
    /// FTP使用两个端口，一个数据端口(20) 一个命令端口21
    /// edtFtpNet默认使用被动模式
    /// 
    /// 主动模式：客户端连接ftp服务器的21端口，建立命令通道，然后客户端本地在另外一个端口监听等待连接，利用命令通道将端口号告诉服务器，服务器使用20端口主动连接客户端，建立数据通道。
    /// 被动模式：建立命令通道与主动模式相同。客户端发送PASV指令告诉服务器采用被动模式。服务器选择端口（>1024的无特权端口）进行监听，并告诉客户端该端口，客户端选择一个端口与服务器提供的端口建立数据通道。
    /// 
    /// 建立采用被动模式，并且服务器ftp程序开通防火墙例外
    /// 采用哪个模式是由客户端决定的
    /// </summary>
    public class EdtFtpHelper
    {
        /// <summary>
        /// ftp服务器地址
        /// </summary>
        private readonly string _serverIp;

        /// <summary>
        /// ftp服务器端口
        /// </summary>
        private readonly int _serverPort;

        /// <summary>
        /// 服务器用户名
        /// </summary>
        private readonly string _userName;

        /// <summary>
        /// 密码
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// 初始化ftp配置
        /// </summary>
        /// <param name="ftpIp"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="serverPort"></param>
        public EdtFtpHelper(string ftpIp, string userName, string password,int serverPort)
        {
            _serverIp = ftpIp;
            _userName = userName;
            _password = password;
            _serverPort = serverPort;
        }

        /// <summary>
        /// 执行一个ftp操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler"></param>
        public void SafeAction(Action<FTPConnection> action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (FTPConnection connection = new FTPConnection())
                {
                    connection.ServerAddress = _serverIp;
                    connection.UserName = _userName;
                    connection.Password = _password;
                    connection.ServerPort = _serverPort;
                    //默认使用的就是PASV被动模式
                    connection.ConnectMode = FTPConnectMode.PASV;
                    //设置传输类型Binary,默认就是Binary
                    connection.TransferType = FTPTransferType.BINARY;
                    //不用设置命令编码，采用默认值，注意传输文件名使用ASCII值
                    //只使用0-127的文件名不会出错。
                    connection.Timeout = 6000;//单位毫秒
                    connection.Connect();

                    if (action != null)
                    {
                        action(connection);
                    }

                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
        }


        /// <summary>
        /// 上传一个文件，如果文件已经存在，通常的行为是覆盖，这依赖于服务器设置
        /// </summary>
        /// <param name="localFileName"></param>
        /// <param name="remoteFileName"></param>
        /// <param name="exceptionHandler">异常处理</param>
        public void UploadFile(string localFileName, string remoteFileName, Action<Exception> exceptionHandler = null)
        {
            SafeAction(connection =>
            {
                string dirName = Path.GetDirectoryName(remoteFileName);

                if (!string.IsNullOrWhiteSpace(dirName))
                {
                    if (!connection.DirectoryExists(dirName))
                    {
                        connection.CreateDirectory(dirName);
                    }
                }

                connection.UploadFile(localFileName, remoteFileName);
            }, exceptionHandler);
        }

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="localFileName"></param>
        /// <param name="remoteFileName">确保远程文件存在</param>
        /// <param name="exceptionHandler">异常处理</param>
        public void DownloadFile(string localFileName, string remoteFileName, Action<Exception> exceptionHandler = null)
        {
            SafeAction(connection =>
            {
                string dirName = Path.GetDirectoryName(localFileName);
                if (!string.IsNullOrWhiteSpace(dirName))
                {
                    if (!Directory.Exists(dirName))
                    {
                        //创建目录及子目录
                        Directory.CreateDirectory(dirName);
                    }
                }
                connection.DownloadFile(localFileName, remoteFileName);
            }, exceptionHandler);
        }

        /// <summary>
        /// 如果目录不存在，创建目录
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="connection"></param>
        private void MakeSureDirExist(string dirName, FTPConnection connection)
        {
            if (!connection.DirectoryExists(dirName))
            {
                connection.CreateDirectory(dirName);
            }
        }

        /// <summary>
        /// 删除目录，确保目录存在
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="connection"></param>
        private void DeleteDir(string dirName, FTPConnection connection)
        {
            connection.DeleteDirectory(dirName);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private bool IsFileExists(string fileName, FTPConnection connection)
        {
            return connection.Exists(fileName);
        }

        /// <summary>
        /// 返回当前工作目录的文件
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private string[] GetFiles(FTPConnection connection)
        {
            return connection.GetFiles();
        }

        /// <summary>
        /// 返回指定目录的文件
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private string[] GetFiles(string dirName, FTPConnection connection)
        {
            return connection.GetFiles(dirName);
        }

        /// <summary>
        /// 返回当前工作目录的文件
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private FTPFile[] GetFileInfos(FTPConnection connection)
        {
            return connection.GetFileInfos();
        }

        /// <summary>
        /// 返回指定目录的文件
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private FTPFile[] GetFileInfos(string dirName, FTPConnection connection)
        {
            return connection.GetFileInfos(dirName);
        }

        /// <summary>
        /// 获取文件上一次的写时间
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private DateTime GetLastWriteTime(string remoteFile, FTPConnection connection)
        {
            return connection.GetLastWriteTime(remoteFile);
        }

        /// <summary>
        /// 获取文件的大小，字节单位
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private long GetFileSize(string remoteFile, FTPConnection connection)
        {
            return connection.GetSize(remoteFile);
        }
        
        /// <summary>
        /// 删除文件，删除前确保文件存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private bool DeleteFile(string fileName, FTPConnection connection)
        {
            return connection.DeleteFile(fileName);
        }

        /// <summary>
        /// 上传一个文件，如果文件已经存在，通常的行为是覆盖，这依赖于服务器设置
        /// </summary>
        /// <param name="localFileName"></param>
        /// <param name="remoteFileName"></param>
        /// <param name="connection"></param>
        private void UploadFile(string localFileName, string remoteFileName, FTPConnection connection)
        {
            connection.UploadFile(localFileName, remoteFileName);
        }

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="localFileName"></param>
        /// <param name="remoteFileName"></param>
        /// <param name="connection"></param>
        private void DownloadFile(string localFileName, string remoteFileName, FTPConnection connection)
        {
            connection.DownloadFile(localFileName, remoteFileName);
        }
    }


}

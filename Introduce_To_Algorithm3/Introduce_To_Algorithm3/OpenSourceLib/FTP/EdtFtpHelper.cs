using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.FtpClient;
using System.Text;
using System.Threading.Tasks;
using EnterpriseDT.Net.Ftp;

namespace Introduce_To_Algorithm3.OpenSourceLib.FTP
{
    /// <summary>
    /// 使用edtFTPnet/Free进行ftp操作
    /// Nuget的版本很旧，从官网下载
    /// </summary>
    public static class EdtFtpHelper
    {
        /// <summary>
        /// ftp服务器地址
        /// </summary>
        private static string _serverIp = "192.168.163.218";

        /// <summary>
        /// 服务器姓名
        /// </summary>
        private static string _userName = "cmlu";

        /// <summary>
        /// 密码
        /// </summary>
        private static string _password = "558276344";

        /// <summary>
        /// 初始化ftp配置
        /// </summary>
        /// <param name="ftpIp"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static void Init(string ftpIp, string userName, string password)
        {
            _serverIp = ftpIp;
            _userName = userName;
            _password = password;
        }
        
        /// <summary>
        /// 执行一个ftp操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler"></param>
        public static void SafeAction(Action<FTPConnection> action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (FTPConnection connection = new FTPConnection())
                {
                    connection.ServerAddress = _serverIp;
                    connection.UserName = _userName;
                    connection.Password = _password;
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
        /// 如果目录不存在，创建目录
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="connection"></param>
        private static void MakeSureDirExist(string dirName,FTPConnection connection)
        {
            if (!connection.DirectoryExists(dirName))
            {
                connection.CreateDirectory(dirName);
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="connection"></param>
        private static void DeleteDir(string dirName, FTPConnection connection)
        {
            connection.DeleteDirectory(dirName);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static bool IsFileExists(string fileName, FTPConnection connection)
        {
            return connection.Exists(fileName);
        }

        /// <summary>
        /// 返回当前工作目录的文件
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static string[] GetFiles(FTPConnection connection)
        {
            return connection.GetFiles();
        }

        /// <summary>
        /// 返回指定目录的文件
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static string[] GetFiles(string dirName,FTPConnection connection)
        {
            return connection.GetFiles(dirName);
        }

        /// <summary>
        /// 返回当前工作目录的文件
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static FTPFile[] GetFileInfos(FTPConnection connection)
        {
            return connection.GetFileInfos();
        }

        /// <summary>
        /// 返回指定目录的文件
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static FTPFile[] GetFileInfos(string dirName, FTPConnection connection)
        {
            return connection.GetFileInfos(dirName);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static bool DeleteFile(string fileName, FTPConnection connection)
        {
            return connection.DeleteFile(fileName);
        }

        /// <summary>
        /// 上传一个文件，如果文件已经存在，通常的行为是覆盖，这依赖于服务器设置
        /// </summary>
        /// <param name="localFileName"></param>
        /// <param name="remoteFileName"></param>
        /// <param name="connection"></param>
        private static void UploadFile(string localFileName, string remoteFileName, FTPConnection connection)
        {
            connection.UploadFile(localFileName,remoteFileName);
        }

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="localFileName"></param>
        /// <param name="remoteFileName"></param>
        /// <param name="connection"></param>
        private static void DownloadFile(string localFileName, string remoteFileName, FTPConnection connection)
        {
            connection.DownloadFile(localFileName, remoteFileName);
        }
    }
}

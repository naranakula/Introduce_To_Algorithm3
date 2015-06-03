using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.FtpClient;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.FTPUtils
{
    /// <summary>
    /// 使用System.Net.FtpClient做的ftp代理
    /// </summary>
    public class SystemNetFtpClientImpl
    {
        #region Member

        /// <summary>
        /// the ftp port to connect 
        /// </summary>
        private int port = 21;

        /// <summary>
        /// the ftp server to connect to
        /// </summary>
        private string host = "localhost";

        /// <summary>
        /// 用户名
        /// </summary>
        private string userName;

        /// <summary>
        /// 密码
        /// </summary>
        private string password;

        /// <summary>
        /// 客户端连接
        /// </summary>
        private FtpClient _ftpClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="host">主机名</param>
        /// <param name="port">端口</param>
        private SystemNetFtpClientImpl(string userName, string password, string host="localhost", int port=21)
        {
            this.password = password;
            this.userName = userName;
            this.host = host;
            this.port = port;
        }

        /// <summary>
        /// 服务器
        /// </summary>
        private static SystemNetFtpClientImpl _instance;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static SystemNetFtpClientImpl Init(string userName, string password, string host = "localhost", int port = 21)
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (_instance == null)
            {
                _instance = new SystemNetFtpClientImpl(userName,password,host,port);
            }

            return _instance;
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static SystemNetFtpClientImpl GetInstance()
        {
            if (_instance == null)
            {
                throw new Exception("need to init first");
            }

            return _instance;
        }

        #endregion

        #region Connect

        public void Connect()
        {
            if (_ftpClient != null && !_ftpClient.IsDisposed)
            {
                _ftpClient.Dispose();
            }

            _ftpClient = new FtpClient()
            {
                Host = host,
                Port = port,
                Credentials = new NetworkCredential(userName,password)
            };
            _ftpClient.Encoding = Encoding.UTF8;
            _ftpClient.Connect();
        }

        #endregion

        #region CreateDirectory

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dir"></param>
        public void CreateDirectory(string dir)
        {
            _ftpClient.CreateDirectory(dir,true);
        }

        #endregion

        #region DeleteDirectory

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir"></param>
        public void DeleteDirectory(string dir)
        {
            _ftpClient.DeleteDirectory(dir,true,FtpListOption.AllFiles|FtpListOption.ForceList);
        }


        #endregion

        #region DeleteFile

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteFile(string fileName)
        {
            _ftpClient.DeleteFile(fileName);
        }

        #endregion

        #region DirectoryExists

        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool DirectoryExists(string dir)
        {
            return _ftpClient.DirectoryExists(dir);
        }

        #endregion

        #region FileExists

        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool FileExists(string fileName)
        {
            return _ftpClient.FileExists(fileName);
        }

        #endregion

        #region Read&Write

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="fielName"></param>
        /// <returns></returns>
        public void ReadFile(string fileName)
        {
            using (Stream stream = _ftpClient.OpenRead(fileName))
            {
                //读取字符
            }
        }

        /// <summary>
        /// 写文件,如果文件存在则覆盖
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void WriteFile(string fileName)
        {
            using (Stream stream = _ftpClient.OpenWrite(fileName))
            {
            }
        }

        #endregion

        #region GetListing

        /// <summary>
        /// 获取当前目录下的文件或者文件夹信息，不能递归获取
        /// </summary>
        public void GetListing()
        {
            FtpListItem[] itemList = _ftpClient.GetListing(_ftpClient.GetWorkingDirectory());
            foreach (var item in itemList)
            {
                Console.WriteLine(item);
            }
        }

        #endregion

        #region GetWorkingDirectory

        /// <summary>
        /// 返回当前工作目录
        /// </summary>
        public void GetWorkingDirectory()
        {
            Console.WriteLine(_ftpClient.GetWorkingDirectory());
        }

        #endregion

        #region Close

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (_ftpClient != null)
            {
                _ftpClient.Dispose();
                _ftpClient = null;
            }
        }

        #endregion
    }
}

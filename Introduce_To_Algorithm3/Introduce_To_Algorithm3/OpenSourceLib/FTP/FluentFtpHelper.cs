using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;

namespace Introduce_To_Algorithm3.OpenSourceLib.FTP
{
    /// <summary>
    /// FluentFtp 比 EdtFtp好
    /// 
    /// 地址:https://github.com/robinrodricks/FluentFTP
    /// 
    /// 
    /// FluentFtp is fully managed ftp and ftps library written in C#
    /// 
    /// </summary>
    public class FluentFtpHelper
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
        public FluentFtpHelper(string ftpIp, string userName, string password, int serverPort=21)
        {
            _serverIp = ftpIp;
            _userName = userName;
            _password = password;
            _serverPort = serverPort;
        }


        /// <summary>
        /// 执行ftp操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler"></param>
        public void SafeAction(Action<FtpClient> action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (FtpClient ftpClient = new FtpClient(this._serverIp, this._serverPort, this._userName,
                    this._password))
                {
                    //ftpClient.ConnectTimeout; 默认是15s
                     //ftpClient.ReadTimeout; 默认是15s
                    ftpClient.Connect();

                    action(ftpClient);

                    //// upload a file
                    //client.UploadFile(@"C:\MyVideo.mp4", "/htdocs/big.txt");

                    //// rename the uploaded file
                    //client.Rename("/htdocs/big.txt", "/htdocs/big2.txt");

                    //// download the file again
                    //client.DownloadFile(@"C:\MyVideo_2.mp4", "/htdocs/big2.txt");

                    //// delete the file
                    //client.DeleteFile("/htdocs/big2.txt");

                    //// delete a folder recursively
                    //client.DeleteDirectory("/htdocs/extras/");

                    //// check if a file exists
                    //if (client.FileExists("/htdocs/big2.txt")) { }

                    //// check if a folder exists
                    //if (client.DirectoryExists("/htdocs/extras/")) { }

                    //// upload a file 
                    //client.UploadFile(@"C:\MyVideo.mp4", "/htdocs/big.txt", FtpExists.Overwrite, false, FtpVerify.Retry);


                }

            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
            }
            
        }



    }
}

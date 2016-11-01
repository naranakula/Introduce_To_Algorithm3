using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// 根据自己的实际情况来定义.主要作用就是存储客户端的信息.
    /// </summary>
    public class AsyncUserToken
    {
        /// <summary>  
        /// 客户端IP地址  
        /// </summary>  
        public IPAddress IPAddress { get; set; }

        /// <summary>  
        /// 远程地址  
        /// </summary>  
        public EndPoint Remote { get; set; }

        /// <summary>  
        /// 通信SOKET  
        /// </summary>  
        public Socket Socket { get; set; }

        /// <summary>  
        /// 连接时间  
        /// </summary>  
        public DateTime ConnectTime { get; set; }

        /// <summary>  
        /// 所属用户信息  
        /// </summary>  
        //public UserInfo UserInfo { get; set; }


        /// <summary>  
        /// 数据缓存区  数据接收缓冲区
        /// </summary>  
        public List<byte> RecvBuffer { get; set; }


        public AsyncUserToken()
        {
            this.RecvBuffer = new List<byte>();
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        public void ParsePacket()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.grpcs
{


    /// <summary>
    /// 利用channel的自动重连和线程安全做longRunchannel
    /// </summary>
    public class LongRunChannel
    {
        #region 属性

        /// <summary>
        /// 服务器IP
        /// </summary>
        private readonly string _serverIp;

        /// <summary>
        /// 服务器端口
        /// </summary>
        private readonly int _serverPort;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        public LongRunChannel(string serverIp, int serverPort)
        {
            this._serverIp = serverIp;
            this._serverPort = serverPort;
        }
    }

    
}

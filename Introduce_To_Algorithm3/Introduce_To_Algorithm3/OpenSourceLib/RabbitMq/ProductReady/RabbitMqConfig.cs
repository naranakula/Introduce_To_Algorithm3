using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady
{

    /// <summary>
    /// RabbitMq配置
    /// </summary>
    public class RabbitMqConfig
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hostIp">服务器名</param>
        /// <param name="port">端口</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="virtualHost">虚拟主机</param>
        public RabbitMqConfig(string hostIp, int port, string userName, string password, string virtualHost = "/")
        {
            this.HostIp = hostIp;
            this.Port = port;
            this.UserName = userName;
            this.Password = password;
            this.VirtualHost = virtualHost;
        }

        #endregion



        private volatile string _hostIp;

        /// <summary>
        /// 主机IP
        /// </summary>
        public string HostIp { get { return _hostIp; } private set { _hostIp = value; } }

        /// <summary>
        /// 端口
        /// </summary>
        private volatile int _port;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get { return _port; } private set { _port = value; } }


        private volatile string _userName;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get { return _userName; } private set { _userName = value; } }


        private volatile string _password;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get { return _password; } private set { _password = value; } }


        private volatile string _virtualHost = "/";

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get { return _virtualHost; } private set { _virtualHost = value; } }


        /// <summary>
        /// 覆盖ToString
        /// 可以唯一标识一个配置
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{HostIp}_{Port}_{UserName}_{Password}_{VirtualHost}";
        }
    }

}

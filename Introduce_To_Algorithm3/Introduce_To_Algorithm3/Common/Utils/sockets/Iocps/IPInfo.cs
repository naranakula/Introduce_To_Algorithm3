using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// IP地址信息
    /// </summary>
    public sealed class IPInfo
    {
        /// <summary>
        /// IP地址
        /// </summary>
        private string m_ipAddress;

        /// <summary>
        /// IP端点
        /// </summary>
        private IPEndPoint m_ipEndPoint;

        /// <summary>
        /// IP端点类型
        /// </summary>
        private IPEndPointType m_ipEndPointType;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">ip地址</param>
        /// <param name="ipEndPoint">端点</param>
        /// <param name="ipEndPointType">端点类型， 本地还是远程</param>
        public IPInfo(string ipAddress, IPEndPoint ipEndPoint, IPEndPointType ipEndPointType)
        {
            this.m_ipAddress = ipAddress;
            this.m_ipEndPoint = ipEndPoint;
            this.m_ipEndPointType = ipEndPointType;
        }

        /// <summary>
        /// ip地址的字符串
        /// </summary>
        public String IPAddress
        {
            get { return m_ipAddress;}
        }

        /// <summary>
        /// IP端
        /// </summary>
        public IPEndPoint IPEndPoint
        {
            get
            {
                return m_ipEndPoint;
            }
        }

        /// <summary>
        /// IP端点类型
        /// </summary>
        public IPEndPointType IPEndPointType
        {
            get
            {
                return m_ipEndPointType;
            }
        }


    }
}

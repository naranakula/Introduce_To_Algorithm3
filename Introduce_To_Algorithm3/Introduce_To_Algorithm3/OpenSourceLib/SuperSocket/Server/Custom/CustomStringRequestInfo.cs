using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;

namespace Introduce_To_Algorithm3.OpenSourceLib.SuperSocket.Server.Custom
{
    /// <summary>
    /// RequestInfo表示来自客户端的请求，每个来自客户端的请求都应该能够实例化为RequestInfo类型
    /// </summary>
    public class CustomStringRequestInfo:IRequestInfo
    {
        /// <summary>
        /// 实现RequestInfo接口
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 自定义属性
        /// </summary>
        public string Name { get; set; }
    }
}

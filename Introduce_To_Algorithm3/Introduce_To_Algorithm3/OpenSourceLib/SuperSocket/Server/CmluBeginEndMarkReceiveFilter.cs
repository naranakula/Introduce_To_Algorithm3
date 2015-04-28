using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;

namespace Introduce_To_Algorithm3.OpenSourceLib.SuperSocket.Server
{
    /// <summary>
    /// 带起止符的请求协议
    /// 此类协议的每个请求有固定的开始结束标记
    /// </summary>
    public class CmluBeginEndMarkReceiveFilter:BeginEndMarkReceiveFilter<StringRequestInfo>
    {
        /// <summary>
        /// 协议的开始
        /// </summary>
        public static  readonly  byte[] BeginMark = Encoding.UTF8.GetBytes("<#Begin#>");

        /// <summary>
        /// 协议的结束字符
        /// </summary>
        public static readonly byte[] EndMark = Encoding.UTF8.GetBytes("<#End#>");

        public CmluBeginEndMarkReceiveFilter() : base(BeginMark, EndMark)
        {
        }

        protected override StringRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {
            throw new NotImplementedException();
        }
    }
}

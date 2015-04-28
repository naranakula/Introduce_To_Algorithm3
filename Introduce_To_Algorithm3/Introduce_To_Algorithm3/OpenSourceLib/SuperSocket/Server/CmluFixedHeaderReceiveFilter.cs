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
    /// 固定头部并且头部包含内容长度的协议
    /// 该协议有两部分组成：第一部分为头部，头部包含了请求体长度等基础信息，第二部分为请求体
    /// </summary>
    public class CmluFixedHeaderReceiveFilter:FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
        /// <summary>
        /// 定义了头的长度
        /// </summary>
        public CmluFixedHeaderReceiveFilter() : base(12)
        {
        }

        /// <summary>
        /// Gets the body length from header.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            //可以自行定义如何解析头，并且返回头中提的长度
            throw new NotImplementedException();
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            throw new NotImplementedException();
        }
    }
}

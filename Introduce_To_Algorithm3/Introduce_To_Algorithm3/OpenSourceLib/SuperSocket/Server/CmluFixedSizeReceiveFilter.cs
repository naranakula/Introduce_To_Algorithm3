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
    /// 固定请求大小的协议实现
    /// </summary>
    public class CmluFixedSizeReceiveFilter:FixedSizeReceiveFilter<StringRequestInfo>
    {
        public CmluFixedSizeReceiveFilter()
            : base(12)
        {
        }

        protected override StringRequestInfo ProcessMatchedRequest(byte[] buffer, int offset, int length, bool toBeCopied)
        {
            throw new NotImplementedException();
        }
    }
}

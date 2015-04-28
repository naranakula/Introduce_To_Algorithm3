using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;

namespace Introduce_To_Algorithm3.OpenSourceLib.SuperSocket.Server.Custom
{
    /// <summary>
    /// 接收过滤器，将接收到的二进制数据转换为RequestInfo
    /// </summary>
    public class CustomReceiveFilter<TRequestInfo>: IReceiveFilter<TRequestInfo> where TRequestInfo:IRequestInfo
    {
        /// <summary>
        /// Filters received data of the specific session into request info
        /// </summary>
        /// <param name="readBuffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="toBeCopied"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        public TRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reset this instance to intial state
        /// </summary>
        public void Reset()
        {
            throw new NotImplementedException();
        }

        public int LeftBufferSize
        {
            get { throw new NotImplementedException(); }
        }

        public IReceiveFilter<TRequestInfo> NextReceiveFilter
        {
            get { throw new NotImplementedException(); }
        }

        public FilterState State
        {
            get { throw new NotImplementedException(); }
        }
    }
}

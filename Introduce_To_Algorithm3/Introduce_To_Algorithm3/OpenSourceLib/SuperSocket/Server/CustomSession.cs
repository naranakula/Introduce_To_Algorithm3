using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Introduce_To_Algorithm3.OpenSourceLib
{
    /// <summary>
    /// AppSession 代表一个和客户端的逻辑连接，基于连接的操作如发送数据到客户端，接收客户端数据，关闭连接等在该类中定义
    /// </summary>
    public class CustomSession:AppSession<CustomSession>
    {
        /// <summary>
        /// Called when [session started].
        /// </summary>
        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
        }

        /// <summary>
        ///  Called when [session closed].
        /// </summary>
        /// <param name="reason">close reason</param>
        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);
        }

        /// <summary>
        ///  Handles the exceptional error, it only handles application error.
        /// </summary>
        /// <param name="e">错误原因</param>
        protected override void HandleException(Exception e)
        {
            base.HandleException(e);
        }

        /// <summary>
        /// Handles the unknown request.
        /// </summary>
        /// <param name="requestInfo"></param>
        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            base.HandleUnknownRequest(requestInfo);
        }

      
    }
}

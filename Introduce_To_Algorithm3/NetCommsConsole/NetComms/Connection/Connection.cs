using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetCommsConsole.NetComms.Connections
{
    /// <summary>
    /// 代表一个实际的连接
    /// </summary>
    public abstract class Connection
    {
        /// <summary>
        /// Connection information related to this connection.
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; protected set; }

        /// <summary>
        /// A manual reset event which can be used to handle connection setup and establish.
        /// </summary>
        protected ManualResetEvent connectionSetupWait = new ManualResetEvent(false);

        /// <summary>
        /// A manual reset event which can be used to handle connection setup and establish.
        /// </summary>
        protected ManualResetEvent connectionEstablishWait = new ManualResetEvent(false);

        /// <summary>
        /// A boolean used to signal a connection setup exception
        /// </summary>
        protected bool connectionSetupException = false;

        /// <summary>
        /// If <see cref="connectionSetupException"/> is true provides additional exception information.
        /// </summary>
        protected string connectionSteupExceptionStr = string.Empty;

        /// <summary>
        /// Create a new Connection object
        /// </summary>
        /// <param name="connectionInfo">ConnectionInfo corresponding to the new connection</param>
        /// <param name="defaultSendReceiveOptions">The sendReceiveOptions which should be used as connection defaults</param>
        protected Connection(ConnectionInfo connectionInfo, SendReceiveOptions defaultSendReceiveOptions)
        {
            
        }
    }
}

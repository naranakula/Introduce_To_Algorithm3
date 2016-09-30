using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NetCommsConsole.NetComms.Connections
{
    /// <summary>
    /// A base class that the listener of each connection type inherits from.
    /// This allows NetworkComms.Net to manage listeners at general connection level
    /// </summary>
    public abstract class ConnectionListenerBase
    {
        #region public properties

        /// <summary>
        /// The send receive options associated with this listener
        /// </summary>

        public SendReceiveOptions ListenerDefaultSendReceiveOptions { get; protected set; }

        /// <summary>
        /// 连接类型
        /// </summary>
        public ConnectionType ConnectionType { get; protected set; }

        /// <summary>
        /// The application layer protocol status for this listener
        /// </summary>
        public ApplicationLayerProtocolStatus ApplicationLayerProtocol;

        /// <summary>
        /// True if this listener is listening
        /// </summary>
        public bool IsListening { get; protected set; }

        /// <summary>
        /// True if this listener will be advertised via peer discovery
        /// </summary>
        public bool IsDiscoverable { get; protected set; }

        /// <summary>
        /// The local endpoint that this listener is associated with
        /// </summary>
        public EndPoint LocalListenEndPoint { get; protected set; }

        #endregion

        #region private properties

        /// <summary>
        /// Thread safety lock which is used when accessing <see cref="incomingPacketHandlers"/>  and <see cref="incomingPacketUnwrappers"/>
        /// </summary>
        private object delegateLocker = new object();

 


        #endregion


    }
}

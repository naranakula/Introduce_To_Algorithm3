using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using NetCommsConsole.NetComms.Connections;
using NetCommsConsole.NetComms.DPSBase;
using NetCommsConsole.NetComms.Models;
using NetCommsConsole.NetComms.Tools;

namespace NetCommsConsole.NetComms
{
    /// <summary>
    /// 连接信息
    /// </summary>
    public class ConnectionInfo:IEquatable<ConnectionInfo>,IExplicitlySerialize
    {

        #region public member

        /// <summary>
        /// 连接的类型
        /// </summary>
        public ConnectionType ConnectionType { get; internal set; }

        /// <summary>
        /// True if the RemotePoint is Connectable
        /// </summary>
        public bool IsConnectable { get; private set; }

        /// <summary>
        /// 连接创建时间
        /// </summary>
        public DateTime ConnectionCreationTime { get; protected set; }

        /// <summary>
        /// True, is connection was originally established by remote
        /// 是否是服务器端
        /// </summary>
        public bool ServerSide { get; internal set; }

        /// <summary>
        /// If the connection is  <see cref="ServerSide"/> references the listener that was used.
        /// </summary>
        public ConnectionListenerBase ConnectionListener { get; internal set; }

        /// <summary>
        /// The DateTime corresponding to the creation time of this connection object
        /// </summary>
        public DateTime ConnectionEstablishedTime { get; private set; }

        /// <summary>
        /// The <see cref="EndPoint"/> corresponding to the local end of the connection
        /// </summary>
        public EndPoint LocalEndPoint { get; private set; }

        /// <summary>
        /// The <see cref="EndPoint"/> coressponding to the local end of the connection
        /// </summary>
        public EndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// Describes the current state of the connection
        /// </summary>
        public ConnectionState ConnectionState { get; private set; }

        /// <summary>
        /// Returns the networkIdentifier of this peer as a ShortGuid. If the NetworkIdentifier has not yet been set returns ShortGuid.Empty.
        /// 使用ShortGuid没有必要，将来替换为Guid
        /// </summary>
        public ShortGuid NetworkIdentifier
        {
            get
            {
                if (string.IsNullOrEmpty(NetworkIdentifierStr)) return ShortGuid.Empty;
                else return new ShortGuid(NetworkIdentifierStr);
            }
        }


        /// <summary>
        /// The DateTime corresponding to the time data was sent or received
        /// </summary>
        public DateTime LastTrafficTime
        {
            get
            {
                lock (internalLocker)
                    return lastTrafficTime;
            }
            protected set
            {
                lock (internalLocker)
                    lastTrafficTime = value;
            }
        }

        /// <summary>
        /// If enabled NetworkComms.Net uses a custom application layer protocol to provide 
        /// useful features such as inline serialisation, transparent packet transmission, 
        /// remote peer information etc. Default: ApplicationLayerProtocolStatus.Enabled
        /// </summary>
        public ApplicationLayerProtocolStatus ApplicationLayerProtocol { get; private set; }

        #endregion

        #region Private Member

        /// <summary>
        /// 唯一的id表示 of the end point
        /// </summary>
        private string NetworkIdentifierStr;

        /// <summary>
        /// 本地endpoint ip地址
        /// </summary>
        private string localEndPointAddressStr;

        /// <summary>
        /// 本地endpoint端口
        /// </summary>
        private int localEndPointPort;
        
        private bool hashCodeCacheSet = false;

        private int hashCodeCache;

        /// <summary>
        /// 最近一次通信时间
        /// </summary>
        private DateTime lastTrafficTime;
        
        /// <summary>
        /// 锁
        /// </summary>
        private object internalLocker = new object();
        #endregion

        #region Internal Usages

        /// <summary>
        ///  The localEndPoint cast as <see cref="IPEndPoint"/>.
        /// </summary>
        internal IPEndPoint LocalIPEndPoint
        {
            get
            {
                try
                {
                    return (IPEndPoint)LocalEndPoint;
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException("Unable to cast LocalEndPoint to IPEndPoint.", ex);
                }
            }
        }

        /// <summary>
        /// The remoteEndPoint cast as <see cref="IPEndPoint"/>.
        /// </summary>
        internal IPEndPoint RemoteIPEndPoint
        {
            get
            {
                try
                {
                    return (IPEndPoint)RemoteEndPoint;
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException("Unable to cast LocalEndPoint to IPEndPoint.", ex);
                }
            }
        }

        /// <summary>
        /// The localEndPoint cast as <see cref="IPEndPoint"/>
        /// </summary>
        internal BluetoothEndPoint LocalBTEndPoint
        {
            get
            {
                try
                {
                    return (BluetoothEndPoint)LocalEndPoint;
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException("Unable to cast LocalEndPoint to BluetoothEndPoint.", ex);
                }
            }
        }

        /// <summary>
        /// The remoteEndPoint cast as <see cref="IPEndPoint"/>.
        /// </summary>
        internal BluetoothEndPoint RemoteBTEndPoint
        {
            get
            {
                try
                {
                    return (BluetoothEndPoint)RemoteEndPoint;
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException("Unable to cast LocalEndPoint to BluetoothEndPoint.", ex);
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor required for deserialization
        /// </summary>
        private ConnectionInfo()
        {
            
        }

        /// <summary>
        /// Create a new ConnectionInfo object pointing at the provided remote <see cref="IPEndPoint"/>.
        /// Uses the custom NetworkComms.Net application layer protocol.
        /// </summary>
        /// <param name="remoteEndPoint">The end point corresponding with the remote target</param>
        public ConnectionInfo(EndPoint remoteEndPoint)
        {
            this.RemoteEndPoint = remoteEndPoint;

            switch (remoteEndPoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    this.LocalEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    break;
                case AddressFamily.InterNetworkV6:
                    this.LocalEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
                    break;

                case (AddressFamily)32:
                    this.LocalEndPoint = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.SerialPort);
                    break;
            }

            this.ConnectionCreationTime = DateTime.Now;
            this.ApplicationLayerProtocol = ApplicationLayerProtocolStatus.Enabled;
        }


        /// <summary>
        /// Create a new ConnectionInfo object pointing at the provided remote <see cref="IPEndPoint"/>
        /// </summary>
        /// <param name="remoteEndPoint">The end point corresponding with the remote target</param>
        /// <param name="applicationLayerProtocol">If enabled NetworkComms.Net uses a custom 
        /// application layer protocol to provide useful features such as inline serialisation, 
        /// transparent packet transmission, remote peer handshake and information etc. We strongly 
        /// recommend you enable the NetworkComms.Net application layer protocol.</param>
        public ConnectionInfo(EndPoint remoteEndPoint, ApplicationLayerProtocolStatus applicationLayerProtocol)
        {
            if (applicationLayerProtocol == ApplicationLayerProtocolStatus.Undefined)
            {
                throw new ArgumentException("A value of ApplicationLayerProtocolStatus.Undefined is invalid when creating instance of ConnectionInfo.", "applicationLayerProtocol");
            }

            this.RemoteEndPoint = remoteEndPoint;

            switch (remoteEndPoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    this.LocalEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    break;
                case AddressFamily.InterNetworkV6:
                    this.LocalEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
                    break;

                case (AddressFamily)32:
                    this.LocalEndPoint = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.SerialPort);
                    break;

            }

            this.ConnectionCreationTime = DateTime.Now;
            this.ApplicationLayerProtocol = applicationLayerProtocol;
        }

        /// <summary>
        /// Create a new ConnectionInfo object pointing at the provided remote ipAddress and port. 
        /// Provided ipAddress and port are parsed in to <see cref="RemoteEndPoint"/>. Uses the 
        /// custom NetworkComms.Net application layer protocol.
        /// </summary>
        /// <param name="remoteIPAddress">IP address of the remote target in string format</param>
        /// <param name="remotePort">The available of the remote target. 
        /// Valid ports are 1 through 65535. </param>
        public ConnectionInfo(string remoteIPAddress, int remotePort)
        {
            IPAddress ipAddress;
            if (!IPAddress.TryParse(remoteIPAddress, out ipAddress))
            {
                throw new ArgumentException("Provided remoteIPAddress string was not successfully parsed.", "remoteIPAddress");
            }

            this.RemoteEndPoint = new IPEndPoint(ipAddress, remotePort);

            switch (this.RemoteEndPoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    this.LocalEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    break;
                case AddressFamily.InterNetworkV6:
                    this.LocalEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
                    break;
                case (AddressFamily)32:
                    this.LocalEndPoint = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.SerialPort);
                    break;

            }

            this.ConnectionCreationTime = DateTime.Now;
            this.ApplicationLayerProtocol = ApplicationLayerProtocolStatus.Enabled;
        }


        /// <summary>
        /// Create a new ConnectionInfo object pointing at the provided remote ipAddress and port. 
        /// Provided ipAddress and port are parsed in to <see cref="RemoteEndPoint"/>.
        /// </summary>
        /// <param name="remoteIPAddress">IP address of the remote target in string format, e.g. "192.168.0.1"</param>
        /// <param name="remotePort">The available port of the remote target. 
        /// Valid ports are 1 through 65535. Port numbers less than 256 are reserved for well-known services (like HTTP on port 80) and port numbers less than 1024 generally require admin access</param>
        /// <param name="applicationLayerProtocol">If enabled NetworkComms.Net uses a custom 
        /// application layer protocol to provide useful features such as inline serialisation, 
        /// transparent packet transmission, remote peer handshake and information etc. We strongly 
        /// recommend you enable the NetworkComms.Net application layer protocol.</param>
        public ConnectionInfo(string remoteIPAddress, int remotePort, ApplicationLayerProtocolStatus applicationLayerProtocol)
        {
            if (applicationLayerProtocol == ApplicationLayerProtocolStatus.Undefined)
                throw new ArgumentException("A value of ApplicationLayerProtocolStatus.Undefined is invalid when creating instance of ConnectionInfo.", "applicationLayerProtocol");

            IPAddress ipAddress;
            if (!IPAddress.TryParse(remoteIPAddress, out ipAddress))
                throw new ArgumentException("Provided remoteIPAddress string was not successfully parsed.", "remoteIPAddress");

            this.RemoteEndPoint = new IPEndPoint(ipAddress, remotePort);

            switch (this.RemoteEndPoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    this.LocalEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    break;
                case AddressFamily.InterNetworkV6:
                    this.LocalEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
                    break;

                case (AddressFamily)32:
                    this.LocalEndPoint = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.SerialPort);
                    break;

            }

            this.ConnectionCreationTime = DateTime.Now;
            this.ApplicationLayerProtocol = applicationLayerProtocol;
        }

        /// <summary>
        /// Create a connectionInfo object which can be used to inform a remote peer of local connectivity.
        /// Uses the custom NetworkComms.Net application layer protocol.
        /// </summary>
        /// <param name="connectionType">The type of connection</param>
        /// <param name="localNetworkIdentifier">The local network identifier</param>
        /// <param name="localEndPoint">The localEndPoint which should be referenced remotely</param>
        /// <param name="isConnectable">True if connectable on provided localEndPoint</param>
        public ConnectionInfo(ConnectionType connectionType, ShortGuid localNetworkIdentifier, EndPoint localEndPoint,
            bool isConnectable)
        {
            if (localEndPoint == null)
            {
                throw new ArgumentNullException("localEndPoint", "localEndPoint may not be null");
            }

            this.ConnectionType = connectionType;
            this.NetworkIdentifierStr = localNetworkIdentifier.ToString();
            switch (localEndPoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    this.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    break;
                case AddressFamily.InterNetworkV6:
                    this.RemoteEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
                    break;
                case (AddressFamily)32:
                    this.RemoteEndPoint = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.SerialPort);
                    break;
            }

            this.LocalEndPoint = localEndPoint;
            this.IsConnectable = isConnectable;
            this.ApplicationLayerProtocol = ApplicationLayerProtocolStatus.Enabled;

        }

        /// <summary>
        /// Create a connectionInfo object which can be used to inform a remote peer of local connectivity
        /// </summary>
        /// <param name="connectionType">The type of connection</param>
        /// <param name="localNetworkIdentifier">The local network identifier</param>
        /// <param name="localEndPoint">The localEndPoint which should be referenced remotely</param>
        /// <param name="isConnectable">True if connectable on provided localEndPoint</param>
        /// <param name="applicationLayerProtocol">If enabled NetworkComms.Net uses a custom 
        /// application layer protocol to provide useful features such as inline serialisation, 
        /// transparent packet transmission, remote peer handshake and information etc. We strongly 
        /// recommend you enable the NetworkComms.Net application layer protocol.</param>
        public ConnectionInfo(ConnectionType connectionType, ShortGuid localNetworkIdentifier, EndPoint localEndPoint,
            bool isConnectable, ApplicationLayerProtocolStatus applicationLayerProtocol)
        {
            if (localEndPoint == null)
                throw new ArgumentNullException("localEndPoint", "localEndPoint may not be null");

            if (applicationLayerProtocol == ApplicationLayerProtocolStatus.Undefined)
                throw new ArgumentException("A value of ApplicationLayerProtocolStatus.Undefined is invalid when creating instance of ConnectionInfo.", "applicationLayerProtocol");

            this.ConnectionType = connectionType;
            this.NetworkIdentifierStr = localNetworkIdentifier.ToString();
            this.LocalEndPoint = localEndPoint;

            switch (localEndPoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    this.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    break;
                case AddressFamily.InterNetworkV6:
                    this.RemoteEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
                    break;

                case (AddressFamily)32:
                    this.RemoteEndPoint = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.SerialPort);
                    break;
            }

            this.IsConnectable = isConnectable;
            this.ApplicationLayerProtocol = applicationLayerProtocol;
        }

        /// <summary>
        /// Create a connectionInfo object for a new connection
        /// </summary>
        /// <param name="connectionType">The type of connection</param>
        /// <param name="remoteEndPoint">The remoteEndPoint of this connection</param>
        /// <param name="localEndPoint">The localEndpoint of this connection</param>
        /// <param name="applicationLayerProtocol">If enabled NetworkComms.Net uses a custom 
        /// application layer protocol to provide useful features such as inline serialisation, 
        /// transparent packet transmission, remote peer handshake and information etc. We strongly 
        /// recommend you enable the NetworkComms.Net application layer protocol.</param>
        /// <param name="connectionListener">The listener associated with this connection if server side</param>
        internal ConnectionInfo(ConnectionType connectionType, EndPoint remoteEndPoint, EndPoint localEndPoint,
            ApplicationLayerProtocolStatus applicationLayerProtocol = ApplicationLayerProtocolStatus.Enabled,
            ConnectionListenerBase connectionListener = null)
        {
            if (localEndPoint == null)
                throw new ArgumentNullException("localEndPoint", "localEndPoint may not be null");

            if (remoteEndPoint == null)
                throw new ArgumentNullException("remoteEndPoint", "remoteEndPoint may not be null");

            if (applicationLayerProtocol == ApplicationLayerProtocolStatus.Undefined)
                throw new ArgumentException("A value of ApplicationLayerProtocolStatus.Undefined is invalid when creating instance of ConnectionInfo.", "applicationLayerProtocol");

            this.ServerSide = (connectionListener != null);
            this.ConnectionListener = connectionListener;
            this.ConnectionType = connectionType;
            this.RemoteEndPoint = remoteEndPoint;
            this.LocalEndPoint = localEndPoint;
            this.ConnectionCreationTime = DateTime.Now;
            this.ApplicationLayerProtocol = applicationLayerProtocol;
        }


        #endregion

        #region Methods

        /// <summary>
        /// Marks the connection as establishing
        /// </summary>
        internal void NoteStartConnectionEstablish()
        {
            lock (internalLocker)
            {
                if (ConnectionState == ConnectionState.Shutdown) throw new ConnectionSetupException("Unable to mark as establishing as connection has already shutdown.");

                if (ConnectionState == ConnectionState.Establishing) throw new ConnectionSetupException("Connection already marked as establishing");
                else ConnectionState = ConnectionState.Establishing;
            }
        }

        #endregion

        #region IEquatable
        public bool Equals(ConnectionInfo other)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IExplicitlySerialize接口

        public void Serialize(Stream outputStream)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream inputStream)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}

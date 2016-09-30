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
        
        /// <summary>
        /// 是否缓存了hashCache
        /// </summary>
        private bool hashCodeCacheSet = false;

        /// <summary>
        /// 缓存的hashCache
        /// </summary>
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

        /// <summary>
        /// Set this connectionInfo as established
        /// </summary>
        internal void NoteCompleteConnectionEstablish()
        {
            lock (internalLocker)
            {
                if (ConnectionState == ConnectionState.Shutdown) throw new ConnectionSetupException("Unable to mark as established as connection has already shutdown.");

                if (ConnectionState != ConnectionState.Establishing) throw new ConnectionSetupException("Connection should be marked as establishing before calling CompleteConnectionEstablish");

                if (ConnectionState == ConnectionState.Established) throw new ConnectionSetupException("Connection already marked as established.");

                ConnectionState = ConnectionState.Established;
                ConnectionEstablishedTime = DateTime.Now;

                //The below only really applied to TCP connections
                //We only expect a remote network identifier for managed connections
                //if (ApplicationLayerProtocol == ApplicationLayerProtocolStatus.Enabled && NetworkIdentifier == ShortGuid.Empty)
                //    throw new ConnectionSetupException("Remote network identifier should have been set by this point.");
            }
        }

        /// <summary>
        /// Note this connection as shutdown
        /// </summary>
        internal void NoteConnectionShutdown()
        {
            lock (internalLocker)
                ConnectionState = ConnectionState.Shutdown;
        }

        /// <summary>
        /// Update the localEndPoint information for this connection
        /// </summary>
        /// <param name="localEndPoint"></param>
        internal void UpdateLocalEndPointInfo(EndPoint localEndPoint)
        {
            if (localEndPoint == null)
                throw new ArgumentNullException("localEndPoint", "localEndPoint may not be null.");

            lock (internalLocker)
            {
                hashCodeCacheSet = false;
                this.LocalEndPoint = localEndPoint;
            }
        }

        /// <summary>
        /// During a connection handshake we might be provided with more update information regarding endpoints, connectability and identifiers
        /// </summary>
        /// <param name="handShakeInfo"><see cref="ConnectionInfo"/> provided by remoteEndPoint during connection handshake.</param>
        /// <param name="remoteEndPoint">The correct remoteEndPoint of this connection.</param>
        internal void UpdateInfoAfterRemoteHandshake(ConnectionInfo handshakeInfo, EndPoint remoteEndPoint)
        {
            lock (internalLocker)
            {
                NetworkIdentifierStr = handshakeInfo.NetworkIdentifier.ToString();
                RemoteEndPoint = remoteEndPoint;

                IsConnectable = handshakeInfo.IsConnectable;
            }
        }

        /// <summary>
        /// Updates the last traffic time for this connection
        /// </summary>
        internal void UpdateLastTrafficTime()
        {
            lock (internalLocker)
                lastTrafficTime = DateTime.Now;
        }

        /// <summary>
        /// Replaces the current networkIdentifier with that provided
        /// </summary>
        /// <param name="networkIdentifier">The new networkIdentifier for this connectionInfo</param>
        public void ResetNetworkIdentifer(ShortGuid networkIdentifier)
        {
            NetworkIdentifierStr = networkIdentifier.ToString();
        }


        /// <summary>
        /// A connectionInfo object may be used across multiple connection sessions, i.e. due to a possible timeout. 
        /// This method resets the state of the connectionInfo object so that it may be reused.
        /// </summary>
        internal void ResetConnectionInfo()
        {
            lock (internalLocker)
            {
                ConnectionState = ConnectionState.Undefined;
            }
        }


        #endregion

        #region IEquatable
        /// <summary>
        /// Compares this <see cref="ConnectionInfo"/> object with obj and returns true if obj is ConnectionInfo and both 
        /// the <see cref="NetworkIdentifier"/> and <see cref="RemoteEndPoint"/> match.
        /// </summary>
        /// <param name="obj">The object to test of equality</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            lock (internalLocker)
            {
                var other = obj as ConnectionInfo;
                if (((object)other) == null)
                    return false;
                else
                    return this == other;
            }
        }

        /// <summary>
        /// Compares this <see cref="ConnectionInfo"/> object with other and returns true if both the <see cref="NetworkIdentifier"/> 
        /// and <see cref="RemoteEndPoint"/> match.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ConnectionInfo other)
        {
            lock (internalLocker)
                return this == other;
        }

        /// <summary>
        /// Returns left.Equals(right)
        /// </summary>
        /// <param name="left">Left connectionInfo</param>
        /// <param name="right">Right connectionInfo</param>
        /// <returns>True if both are equal, otherwise false</returns>
        public static bool operator ==(ConnectionInfo left, ConnectionInfo right)
        {
            if (((object)left) == ((object)right)) return true;
            else if (((object)left) == null || ((object)right) == null) return false;
            else
            {
                if (left.RemoteEndPoint != null && right.RemoteEndPoint != null && left.LocalEndPoint != null && right.LocalEndPoint != null)
                    return (left.NetworkIdentifier.ToString() == right.NetworkIdentifier.ToString() && left.RemoteEndPoint.Equals(right.RemoteEndPoint) && left.LocalEndPoint.Equals(right.LocalEndPoint) && left.ApplicationLayerProtocol == right.ApplicationLayerProtocol);
                if (left.RemoteEndPoint != null && right.RemoteEndPoint != null)
                    return (left.NetworkIdentifier.ToString() == right.NetworkIdentifier.ToString() && left.RemoteEndPoint.Equals(right.RemoteEndPoint) && left.ApplicationLayerProtocol == right.ApplicationLayerProtocol);
                else if (left.LocalEndPoint != null && right.LocalEndPoint != null)
                    return (left.NetworkIdentifier.ToString() == right.NetworkIdentifier.ToString() && left.LocalEndPoint.Equals(right.LocalEndPoint) && left.ApplicationLayerProtocol == right.ApplicationLayerProtocol);
                else
                    return (left.NetworkIdentifier.ToString() == right.NetworkIdentifier.ToString() && left.ApplicationLayerProtocol == right.ApplicationLayerProtocol);
            }
        }

        /// <summary>
        /// Returns !left.Equals(right)
        /// </summary>
        /// <param name="left">Left connectionInfo</param>
        /// <param name="right">Right connectionInfo</param>
        /// <returns>True if both are different, otherwise false</returns>
        public static bool operator !=(ConnectionInfo left, ConnectionInfo right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns NetworkIdentifier.GetHashCode() ^ RemoteEndPoint.GetHashCode();
        /// </summary>
        /// <returns>The hashcode for this connection info</returns>
        public override int GetHashCode()
        {
            lock (internalLocker)
            {
                if (!hashCodeCacheSet)
                {
                    if (RemoteEndPoint != null & LocalEndPoint != null)
                        hashCodeCache = NetworkIdentifier.GetHashCode() ^ LocalEndPoint.GetHashCode() ^ RemoteEndPoint.GetHashCode() ^ (ApplicationLayerProtocol == ApplicationLayerProtocolStatus.Enabled ? 1 << 31 : 0);
                    if (RemoteEndPoint != null)
                        hashCodeCache = NetworkIdentifier.GetHashCode() ^ RemoteEndPoint.GetHashCode() ^ (ApplicationLayerProtocol == ApplicationLayerProtocolStatus.Enabled ? 1 << 31 : 0);
                    else if (LocalEndPoint != null)
                        hashCodeCache = NetworkIdentifier.GetHashCode() ^ LocalEndPoint.GetHashCode() ^ (ApplicationLayerProtocol == ApplicationLayerProtocolStatus.Enabled ? 1 << 31 : 0);
                    else
                        hashCodeCache = NetworkIdentifier.GetHashCode() ^ (ApplicationLayerProtocol == ApplicationLayerProtocolStatus.Enabled ? 1 << 31 : 0);

                    hashCodeCacheSet = true;
                }

                return hashCodeCache;
            }
        }


        /// <summary>
        /// Returns a string containing suitable information about this connection
        /// </summary>
        /// <returns>A string containing suitable information about this connection</returns>
        public override string ToString()
        {
            //Add a useful connection state identifier
            string connectionStateIdentifier;
            switch (ConnectionState)
            {
                case ConnectionState.Undefined:
                    connectionStateIdentifier = "U";
                    break;
                case ConnectionState.Establishing:
                    connectionStateIdentifier = "I";
                    break;
                case ConnectionState.Established:
                    connectionStateIdentifier = "E";
                    break;
                case ConnectionState.Shutdown:
                    connectionStateIdentifier = "S";
                    break;
                default:
                    throw new Exception("Unexpected connection state.");
            }

            string returnString = "[" + ConnectionType.ToString() + "-" + (ApplicationLayerProtocol == ApplicationLayerProtocolStatus.Enabled ? "E" : "D") + "-" + connectionStateIdentifier + "] ";

            if (RemoteEndPoint != null && LocalEndPoint != null)
                returnString += LocalEndPoint.ToString() + " -> " + RemoteEndPoint.ToString();
            else if (RemoteEndPoint != null)
                returnString += "Local -> " + RemoteEndPoint.ToString();
            else if (LocalEndPoint != null)
                returnString += LocalEndPoint.ToString() + " " + (IsConnectable ? "Connectable" : "NotConnectable");

            if (NetworkIdentifier != ShortGuid.Empty)
                returnString += " (" + NetworkIdentifier + ")";

            return returnString.Trim();
        }


        #endregion

        #region IExplicitlySerialize接口

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="outputStream"></param>
        public void Serialize(Stream outputStream)
        {
            List<byte[]> data = new List<byte[]>();

            lock (internalLocker)
            {
                if (LocalEndPoint as IPEndPoint != null)
                {
                    localEndPointAddressStr = LocalIPEndPoint.Address.ToString();
                    localEndPointPort = LocalIPEndPoint.Port;
                }

                if (LocalEndPoint as InTheHand.Net.BluetoothEndPoint != null)
                {
                    localEndPointAddressStr = LocalBTEndPoint.Address.ToString();
                    localEndPointPort = LocalBTEndPoint.Port;
                }

                //大端 还是 小端 有机器决定 
                byte[] conTypeData = BitConverter.GetBytes((int) ConnectionType);
                data.Add(conTypeData);

                byte[] netIDData = Encoding.UTF8.GetBytes(NetworkIdentifierStr);
                byte[] netIdLengthData = BitConverter.GetBytes(netIDData.Length);


            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="inputStream"></param>
        public void Deserialize(Stream inputStream)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}

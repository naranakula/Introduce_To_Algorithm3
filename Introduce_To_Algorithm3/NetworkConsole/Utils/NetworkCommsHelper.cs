using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;
using SharpZipLibCompressor;

namespace NetworkConsole.Utils
{
    /// <summary>
    /// networkcomms帮助类
    /// </summary>
    public static class NetworkCommsHelper
    {
        /// <summary>
        /// 获取连接信息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ConnectionInfo GetConnectionInfo(string ip, int port)
        {
            ConnectionInfo connectionInfo = new ConnectionInfo(ip, port);
            return connectionInfo;
        }

        /// <summary>
        /// 获取一个tcp连接，如果不存在建立一个新连接，否则重用现有连接
        /// </summary>
        /// <param name="connectionInfo"></param>
        /// <returns></returns>
        public static TCPConnection GetConnection(ConnectionInfo connectionInfo)
        {
            TCPConnection conn = TCPConnection.GetConnection(connectionInfo);
            
            return conn;
        }


        #region DataSerializer & DataProcessor

        /// <summary>
        /// 获取protobuf serializer
        /// </summary>
        /// <returns></returns>
        public static DataSerializer GetProtobufSerializer()
        {
            DataSerializer serializer = DPSManager.GetDataSerializer<ProtobufSerializer>();
            return serializer;
        }

        /// <summary>
        /// 获取GZipProcessor
        /// </summary>
        /// <returns></returns>
        public static DataProcessor GetGZipProcessor()
        {
            DataProcessor processor = DPSManager.GetDataProcessor<SharpZipLibGzipCompressor>();
            return processor;
        }


        #endregion

    }
}

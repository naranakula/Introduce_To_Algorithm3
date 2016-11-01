

using NetworkCommsDotNet;

namespace Introduce_To_Algorithm3.OpenSourceLib.NetWorkCommsDotNets.Sources
{
    /// <summary>
    /// NetworkComms.Net的客户端
    /// </summary>
    public static class NetworkCommsClient
    {
        /// <summary>
        /// 服务器Ip
        /// </summary>
        private static readonly string serverIp = "192.168.163.51/";//初始化从配置文件中读取

        /// <summary>
        /// 服务器端口
        /// </summary>
        private static readonly int serverPort = 8991;//初始化从配置文件中读取

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息不能为空</param>
        public static void Send(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }
            
            //NetworkComms.SendObject()
        }
    }
}

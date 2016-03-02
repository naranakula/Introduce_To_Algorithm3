
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets
{
    /// <summary>
    /// Socket拓展类
    /// </summary>
    public static class SocketEx
    {
        /// <summary>
        /// Close the socket safely
        /// </summary>
        /// <param name="socket"></param>
        public static void SafeClose(this Socket socket)
        {
            if (socket == null)
            {
                return;
            }

            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
            }

            try
            {
                socket.Close();
            }
            catch
            {
            }
            finally
            {
                socket = null;
            }
        }

        /// <summary>
        /// 确定一个或多个套接字的状态。
        /// Select 是一种静态方法，它可确定一个或多个 Socket 实例的状态。必须先将一个或多个套接字放入 IList 中，然后才能使用 Select 方法。通过调用 Select（将 IList 作为 checkRead 参数,其它设置为null），可检查是否具有可读性。若要检查套接字是否具有可写性，请使用 checkWrite 参数，其它设置为null。若要检测错误条件，请使用 checkError，其它设置为NULL。在调用 Select 之后，IList 中将仅填充那些满足条件的套接字。
        /// </summary>
        /// <param name="checkRead">要检查可读性的 Socket 实例的 IList。</param>
        /// <param name="checkWrite">一个 Socket 实例的 IList，用于检查可写性。</param>
        /// <param name="checkError">要检查错误的 Socket 实例的 IList。</param>
        /// <param name="microSeconds">超时值（以毫秒为单位）。A -1 值指示超时值为无限大。</param>
        public static void Select(List<Socket> checkRead,List<Socket> checkWrite, List<Socket> checkError, int microSeconds)
        {
            Socket.Select(checkRead,checkWrite,checkError,microSeconds);
        }


        #region Options

        /// <summary>
        /// 在多播中需要设置TTl值（Time to live），每一个ip数据报文中都包含一个TTL，每当有路由器转发该报文时，TTL减1，知道减为0时，生命周期结束，报文即时没有到达目的地，也立即宣布死亡。
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ttl"></param>
        public static void SetSocketTtl(Socket socket, int ttl)
        {
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive,ttl);
        }

        /// <summary>
        /// 注册到某个多播地址
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="address"></param>
        public static void JoinMulticastGroup(Socket socket, string address)
        {
            IPAddress ipAddress = IPAddress.Parse(address);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ipAddress,IPAddress.Any));
        }


        /// <summary>
        /// 注销到某个多播地址
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="address"></param>
        public static void DropMulticastGroup(Socket socket, string address)
        {
            IPAddress ipAddress = IPAddress.Parse(address);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, new MulticastOption(ipAddress, IPAddress.Any));
        }

        #endregion

    }
}

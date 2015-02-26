using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 使用System.BitConverter将基础数据类型与字节数组相互转换。
    /// </summary>
    public static class BitConverterUtils
    {
        /// <summary>
        /// 指示机器字节码是否为小端模式
        /// 注：本机器为true
        /// </summary>
        public static bool IsLittleEndian
        {
            get { return BitConverter.IsLittleEndian; }
        }

        /// <summary>
        /// 由主机序转换为网络序
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] HostToNetworkOrder(int i)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
        }

        /// <summary>
        /// 由主机序转换为网络序
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] HostToNetworkOrder(short i)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
        }
        /// <summary>
        /// 由主机序转换为网络序
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] HostToNetworkOrder(long i)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
        }

        /// <summary>
        /// 有网络序转换为主机序
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int NetworkToHostOrder(byte[] bytes)
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes,0));
        }
    }
}

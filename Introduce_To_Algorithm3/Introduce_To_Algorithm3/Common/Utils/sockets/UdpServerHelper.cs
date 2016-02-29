using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils.sockets
{
    //IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
    //Console.WriteLine(addresses[1]);
    //IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 1235);
    //UdpClient client = new UdpClient();
    //while (true)
    //{
    //    Console.WriteLine("输入:");
    //    string s = Console.ReadLine();
    //    byte[] buffer = Encoding.UTF8.GetBytes(s);
    //    client.Send(buffer, buffer.Length, endPoint);
    //    IPEndPoint  point = new IPEndPoint(IPAddress.Any, 0);
    //    byte[] buffer2 = client.Receive(ref point);
    //    Console.WriteLine(point);
    //    Console.WriteLine("接收到:"+Encoding.UTF8.GetString(buffer2));
    //}


    //IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 1235);
    //UdpClient client = new UdpClient(endPoint);

    //while (true)
    //{
    //    IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
    //    byte[] buffer = client.Receive(ref point);
    //    Console.WriteLine("接收到:"+Encoding.UTF8.GetString(buffer));
    //    client.Send(buffer, buffer.Length, point);
    //}
    //经过Reflector，Receive中的IpEndPoint应该是Out，而不应该是ref
    //如果是IPV4  ，内部使用 IPEndPoint Any = new IPEndPoint(IPAddress.Any, 0);
    //如果是IPV6 ,  内部使用 IPEndPoint IPv6Any = new IPEndPoint(IPAddress.IPv6Any, 0);
    /// <summary>
    /// UdpServer帮助类
    /// </summary>
    public class UdpServerHelper:IDisposable
    {
             /// <summary>
        /// 底层通信
        /// </summary>
        private readonly UdpClient _udpClient;

        /// <summary>
        /// 绑定到本地的端口
        /// </summary>
        private readonly int _localPort;

        /// <summary>
        /// 底层编码
        /// </summary>
        private Encoding _encoding;

        /// <summary>
        /// 构造函数
        /// 提供绑定到本地的端口号
        /// </summary>
        /// <param name="localPort">绑定到本地的端口</param>
        /// <param name="encoding">编码方式</param>
        public UdpServerHelper( int localPort,Encoding encoding)
        {
            this._localPort = localPort;
            this._encoding = encoding;
            //服务端使用该IP和端口发送和接收数据
            //an IPEndPoint using the IP address and port number from which you intend to send and receive data.
            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, _localPort));
            //可以采用异步调用
            _udpClient.BeginReceive(new AsyncCallback(AsyncCallback), _udpClient);
        }

        /// <summary>
        /// 异步调用会掉
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncCallback(IAsyncResult ar)
        {
            //Object obj = ar.AsyncState;//获取传递的数据,本例中为UdpClient
            IPEndPoint remoteIpEp = new IPEndPoint(IPAddress.Any, 0);
            //虽然端口指定了0，表示任意端口，但该server只能接收构造函数中绑定的端口中的数据
            //构造函数中指定的端口是唯一的端口用来发送和接收数据的
            byte[] buffer  = _udpClient.EndReceive(ar, ref remoteIpEp);
            if (remoteIpEp != null)
            {
                Log4netHelper.Info(_encoding.GetString(buffer));
                _udpClient.Send(buffer, buffer.Length, remoteIpEp);
            }

            //重新开始异步调用
            _udpClient.BeginReceive(new AsyncCallback(AsyncCallback), _udpClient);
        }

        /// <summary>
        /// 发送数据
        /// 将数据报发送到指定的远程主机，并返回发送的字节数。
        /// 因为指定了IPEndPoint,所以不能调用Connect建立默认连接
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="encoding"></param>
        /// <param name="ipEndPoint"></param>
        public void SendTo(string msg, Encoding encoding,IPEndPoint ipEndPoint)
        {
            byte[] buffer = encoding.GetBytes(msg);
            _udpClient.Send(buffer, buffer.Length,ipEndPoint);
        }

        /// <summary>
        /// 发送数据
        /// 因为指定了IPEndPoint,所以不能调用Connect建立默认连接
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="ipEndPoint"></param>
        public void SendTo(byte[] buffer,IPEndPoint ipEndPoint)
        {
            _udpClient.Send(buffer, buffer.Length,ipEndPoint);
        }

        /// <summary>
        /// 同步接收一个数据报
        /// </summary>
        /// <param name="remoteIpEndPoint">远程主机</param>
        /// <returns></returns>
        public byte[] Receive(ref IPEndPoint remoteIpEndPoint)
        {
            //Receive 方法将阻止，直到数据报从远程主机到达为止。
            //如果在 Connect 方法中指定了默认远程主机，则 Receive 方法将只接受来自该主机的数据报。 其他所有数据报将被丢弃。
            return _udpClient.Receive(ref remoteIpEndPoint);
        }

        /// <summary>
        /// 清理资源函数
        /// </summary>
        public void Dispose()
        {
            if (_udpClient == null)
            {
                return;
            }

            SafeInvoke.Safe(_udpClient.Close);
        }
    }
}

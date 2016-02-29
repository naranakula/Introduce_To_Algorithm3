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
    /// 
    /// </summary>
    public class UdpClientHelper:IDisposable
    {
        /// <summary>
        /// 底层通信
        /// </summary>
        private UdpClient _udpClient;

        /// <summary>
        /// 远程IP
        /// </summary>
        private string _remoteIp;

        /// <summary>
        /// 远程端口
        /// </summary>
        private int _remotePort;

        /// <summary>
        /// 协议
        /// </summary>
        private Encoding _encoding = Encoding.UTF8;

        /// <summary>
        /// 构造函数
        /// 提供链接的远程服务器的IP和端口号
        /// </summary>
        /// <param name="remoteIp"></param>
        /// <param name="remotePort"></param>
        public UdpClientHelper(string remoteIp, int remotePort)
        {
            this._remoteIp = remoteIp;
            this._remotePort = remotePort;
            _udpClient = new UdpClient();
            
        }

        private void AsyncCallback(IAsyncResult ar)
        {
            IPEndPoint remoteIpEndPoint = null;
            byte[] buffer = _udpClient.EndReceive(ar, ref remoteIpEndPoint);
            if (buffer != null)
            {
                Log4netHelper.Info(_encoding.GetString(buffer));
            }
            _udpClient.BeginReceive(new AsyncCallback(AsyncCallback), null);
        }

        /// <summary>
        /// Connect方法将UdpClient与默认的远程机器绑定，从此该UdpClient只能访问该默认主机。Send方法调用时不需指定远程主机，如果指定其它主机将出错
        /// 注:不同于Tcp,Connect并没有实际建立连接,实际上Udp是无连接的
        /// 不要使用connect
        /// </summary>
        public void Connect()
        {
            //  IPaddress.Any监测所有活动的网络接口
            //  端口0：表示所有可用端口
            //  creates an IPEndpoint using any available IP address on the local computer, and any available port number.
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_remoteIp),_remotePort);
            _udpClient.Connect(endPoint);
            //可以采用异步调用
            _udpClient.BeginReceive(new AsyncCallback(AsyncCallback), null);
        }

        /// <summary>
        /// 发送数据
        /// 将数据报发送到在 Connect 方法中建立的远程主机，并返回发送的字节数。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="encoding"></param>
        public void Send(string msg,Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(msg);
            _udpClient.Send(buffer, buffer.Length);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buffer"></param>
        public void Send(byte[] buffer)
        {
            _udpClient.Send(buffer, buffer.Length);
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
        /// 接收一个数据报
        /// 接收和发送使用相同的IPEndPoint。因为服务器使用其绑定的ip和接口用来发送和接收数据
        /// </summary>
        /// <param name="remoteIpEndPoint">远程主机</param>
        /// <returns></returns>
        public byte[] Receive(ref IPEndPoint remoteIpEndPoint)
        {
            //Receive 方法将阻止，直到数据报从远程主机到达为止。
            //如果在 Connect 方法中指定了默认远程主机，则 Receive 方法将只接受来自该主机的数据报。 其他所有数据报将被丢弃。
            //receive从指定的endpoint接受数据
            //实际上经过测试，IPEndPoint可以接收和send指定的不同的IP和端口，仍然能够收到send指定的IPEndPoint回传的数据，并且IPEndpoint会被修改为正确的数据
            //经过测试，这里receive传入的IPEndPoint可以为null，并传回实际的IPEndPoint
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);//0表示任何可用端口 Any表示任何地址,实际使用时应该指定ip地址
            //实际上应该是out，而不应该是ref
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

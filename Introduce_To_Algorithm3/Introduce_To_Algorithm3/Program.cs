using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log4netQueueHelper.Fatal("Fatal");
            Log4netQueueHelper.Error("Error");
            Log4netQueueHelper.Warn("Warn");
            Log4netQueueHelper.Info("Info");
            Log4netQueueHelper.Debug("Debug");
            //IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            //Console.WriteLine(addresses[1]);
            //IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            //UdpClient client = new UdpClient();
            //while (true)
            //{
            //    Console.WriteLine("输入:");
            //    string s = Console.ReadLine();
            //    byte[] buffer = Encoding.UTF8.GetBytes(s);
            //    client.Send(buffer, buffer.Length, endPoint);
            //    IPEndPoint point = null;
            //    byte[] buffer2 = client.Receive(ref point);
            //    Console.WriteLine(point);
            //    Console.WriteLine("接收到:" + Encoding.UTF8.GetString(buffer2));
            //}


            //IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 1233);
            //Console.WriteLine(1233);
            //UdpClient client = new UdpClient(endPoint);

            //while (true)
            //{
            //    IPEndPoint point = new IPEndPoint(IPAddress.Any, 1233);
            //    byte[] buffer = client.Receive(ref point);
            //    Console.WriteLine("接收到:" + Encoding.UTF8.GetString(buffer));
            //    client.Send(buffer, buffer.Length, point);
            //}
            Thread.Sleep(1000000);


            DateTime now = DateTime.Now;
            Console.WriteLine(now);
        }
    }
}

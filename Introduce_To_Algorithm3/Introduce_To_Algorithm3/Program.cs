using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.GraphEx;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Structs;
using Introduce_To_Algorithm3.Common.Utils.sockets;
using System.Net.Sockets;
using System.Threading;
using Introduce_To_Algorithm3.Common.DynamicProgramming;
using System.Net;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            //fwghso
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            var query1 = (from r in list where r.Length > 5 group r by r.Length into g select g).Where(g => g.Count() >= 2);
            var query2 = from r1 in list join r2 in list2 on r1.Length equals r2.Length select new { Item1 = r1,Item2 = r2}; 
        }


        static void MainProxy1(string[] args)
        {
            while (true)
            {
                string s = Console.ReadLine();
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }
                Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iPEndPoint = new IPEndPoint(Dns.GetHostAddresses("mt-dev-07")[0], 19999);
                socket.Connect(iPEndPoint);
                socket.Send(Encoding.Unicode.GetBytes(s));
                byte[] buffer = new byte[512];
                int readNum = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                Console.WriteLine(Encoding.Unicode.GetString(buffer, 0, readNum) + "  >>> " + DateTime.Now);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        static void MainProxy2(string[] args)
        {
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 19999);
            socket.Bind(iPEndPoint);
            socket.Listen(1024);
            while (true)
            {
                if (!socket.Poll(100000, SelectMode.SelectRead))
                {
                    continue;
                }

                Socket client = socket.Accept();

                byte[] buffer = new byte[512];
                int readNum = client.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                string readString = Encoding.Unicode.GetString(buffer, 0, readNum);
                Console.WriteLine(DateTime.Now.ToString() + "  >>>  " + readString);
                client.Send(Encoding.Unicode.GetBytes(DateTime.Now.ToString()));
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                switch (readString.ToLower())
                {
                    case "cls":
                        Console.Clear();
                        break;
                    case "dir":
                        Console.WriteLine(Directory.GetCurrentDirectory());
                        break;
                    case "time":
                        Console.WriteLine(DateTime.Now);
                        break;
                }
                if (readString == "exit")
                {
                    break;
                }
            }
            socket.Close();
        }
    }

}
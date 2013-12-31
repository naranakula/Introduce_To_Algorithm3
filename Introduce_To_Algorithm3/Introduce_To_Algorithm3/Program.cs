using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.GraphEx;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.sockets;
using System.Net.Sockets;
using System.Threading;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {

            float f1 = 1E-10f, f2 = 1E-07f;
            List<Tuple<int,int>> list = new List<Tuple<int,int>>();
            for (int i = 0; i < 11000; i++)
            {
                int slot = Hash(i, 101);
                list.Add(Tuple.Create<int,int>(slot,i));
            }
            var query = (from r in list group r by r.Item1 into g select g).OrderBy(g => g.Count()).ToList();
            Console.WriteLine(query.First().Count() +"            "+query.Last().Count());
            /*
            while (true)
            {
                Socket socket = SocketClient.CreateTcpSocket();
                if (SocketClient.Connect(socket, "172.31.25.34", 32123))
                {
                    Console.Write(">>>:");
                    SocketClient.Send(socket, Console.ReadLine(), Encoding.UTF8);
                    SocketClient.Close(socket);
                }
                else
                {
                    Console.WriteLine("Connect failed");
                    break;
                }
            }*/
            /*
            SocketServer server = new SocketServer(32123,WaitCallBackDelegate);
            server.Start();
            Thread.Sleep(1000);
            Console.WriteLine("server listen "+server.ServerIP+":"+server.Port);
            Console.ReadLine();
            */
        }

        public static int Hash(int key, int slotNum)
        {
            double d = (Math.Sqrt(5) - 1) / 2;
            int slot = (int)(((key * d) - (int)(key * d)) * slotNum);
            return slot;
        }


        public static void WaitCallBackDelegate(object state)
        {
            try
            {
                if (state == null || !(state is Socket))
                {
                    return;
                }

                Socket socket = state as Socket;
                Console.WriteLine(SocketClient.GetClientIP(socket) + "sends");
                Console.WriteLine(SocketClient.Receive(socket, Encoding.UTF8));
                SocketClient.Close(socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

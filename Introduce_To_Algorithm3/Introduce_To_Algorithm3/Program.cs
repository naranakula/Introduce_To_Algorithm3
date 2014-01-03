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

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            long l = CodeCounter.GetCodeLines(@"C:\Users\v-chlu\Documents\GitHub\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3");
            Console.WriteLine(l);
            AVL<int, int> avl = new AVL<int, int>();
            RBT<int, int> rbt = new RBT<int, int>();
            int count = 1000000;
            List<int> list = new List<int>();
            Random rand = new Random(100000);
            for (int i = 0; i < count; i++)
            {
                list.Add(rand.Next());
            }
            for (int i = 0; i < list.Count; i++)
            {
                rbt.Insert(list[i], i);
            }

            for (int i = list.Count-1; i >= 0; i--)
            {
                var node = rbt.Search_(list[rand.Next(list.Count)]);
                list.Remove(node.Key);
                rbt.Delete(node);
            }
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

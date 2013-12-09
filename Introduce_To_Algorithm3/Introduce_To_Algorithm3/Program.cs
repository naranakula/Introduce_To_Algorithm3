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
            
            SocketServer server = new SocketServer(32123,WaitCallBackDelegate);
            server.Start();
            Thread.Sleep(1000);
            Console.WriteLine("server listen "+server.ServerIP+":"+server.Port);
            Console.ReadLine();
            
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

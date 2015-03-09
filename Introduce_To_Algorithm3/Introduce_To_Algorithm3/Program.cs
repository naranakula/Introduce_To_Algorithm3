using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Utility.Commons;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Introduce_To_Algorithm3.Common.Math;
using Introduce_To_Algorithm3.Common.Structs;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.threads;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Socket> list = new List<Socket>();
            for (int i = 0; i < 1020; i++)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.163.163"), 52321);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);
                String terminator = "##<eof>##";
                String msg = "Helo world";

                byte[] arr = Encoding.UTF8.GetBytes(msg + terminator);
                socket.Send(arr);
                arr = Encoding.UTF8.GetBytes(msg + terminator + msg + terminator + terminator);
                socket.Send(arr);
                list.Add(socket);
                //Thread.Sleep(100);
                Console.WriteLine(i);
                Thread.Sleep(100);
            }
            Console.WriteLine("Hello world");
            Console.ReadLine();
        }
    }
}
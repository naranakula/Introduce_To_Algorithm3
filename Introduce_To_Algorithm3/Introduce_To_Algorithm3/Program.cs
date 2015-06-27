using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<List<byte>> list = new List<List<byte>>();
            for (int i = 0; i < 10000; i++)
            {
                List<byte> item = new List<byte>(1024);
                list.Add(item);
            }
            Console.WriteLine("Hello world"+GC.GetTotalMemory(false)/1024/1024);
            Console.ReadLine();
            Console.ReadLine();
            Console.WriteLine(list);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.GraphEx;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Structs;
using Introduce_To_Algorithm3.Common.Utils.sockets;
using System.Net.Sockets;
using System.Threading;
using Introduce_To_Algorithm3.Common.DynamicProgramming;
using System.Net;
using Introduce_To_Algorithm3.Common.Sort;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line == "stop")
                {
                    return;
                }
                TcpClientEx tcpclient = new TcpClientEx("172.31.25.34", 1999);
                tcpclient.Connect();
                var client = tcpclient.GetTcpClient();

                using (var stream = client.GetStream())
                {
                    var buffer = Encoding.Unicode.GetBytes(line);
                    stream.Write(buffer, 0, buffer.Length);
                    buffer = new byte[1024];
                    int count = stream.Read(buffer, 0, buffer.Length);
                    Console.WriteLine(Encoding.Unicode.GetString(buffer,0,count));
                }
                tcpclient.Close();
            }
        }

    }
}
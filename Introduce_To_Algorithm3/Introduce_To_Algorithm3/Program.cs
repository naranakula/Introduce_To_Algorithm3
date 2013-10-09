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

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = CodeCounter.GetCodeLines(@"C:\Users\v-chlu\Documents\GitHub\Introduce_To_Algorithm3\Introduce_To_Algorithm3");
            Console.WriteLine(count);

            SocketServer server = new SocketServer(12321);
            server.Start();

            server.Stop();
        }
    }
}

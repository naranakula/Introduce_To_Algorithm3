using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int count = CodeCounter.GetCodeLines(@"E:\Github\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3");
            Console.WriteLine(count);
        }
    }
}

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
            int count = CodeCounter.GetCodeLines(
                    @"C:\Users\cmlu\Documents\GitHub\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3");
            Console.WriteLine(count);
        }

    }
}

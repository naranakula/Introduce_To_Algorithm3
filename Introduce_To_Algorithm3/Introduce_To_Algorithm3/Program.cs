using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Com.Utility.Commons;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.Serial;
using Introduce_To_Algorithm3.Models;
using Introduce_To_Algorithm3.OpenSourceLib.RabbitMq;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(CodeCounter.GetCodeLines(@"E:\Github\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3"));
        }
    }
}

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
using Introduce_To_Algorithm3.Common.Utils.Serial;
using Introduce_To_Algorithm3.OpenSourceLib.RabbitMq;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                string s = Console.ReadLine();
                if (s == "q")
                {
                    break;
                }
                RabbitMqProducer.Send2(s);
            }
        }
    }
}

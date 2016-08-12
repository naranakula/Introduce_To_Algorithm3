using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Tools;

namespace NetworkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            HostInfo.AllLocalAdaptorNames().ForEach(Console.WriteLine);
            Console.WriteLine("---------------------------");
            NetworkComms.DefaultSendReceiveOptions.Options.Keys.ToList().ForEach(r=>Console.WriteLine(r+":"+NetworkComms.DefaultSendReceiveOptions.Options[r]));
        }
    }
}

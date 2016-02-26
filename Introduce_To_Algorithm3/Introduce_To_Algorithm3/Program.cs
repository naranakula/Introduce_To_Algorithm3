using System;
using System.Configuration;
using System.Linq;
using System.Net;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.Resolve(host);
            Console.WriteLine("Canonical name:"+hostEntry.HostName);

            hostEntry.AddressList.ToList().ForEach(r=>Console.WriteLine(r));
            Console.WriteLine("-------------------");
            hostEntry.Aliases.ToList().ForEach(r=>Console.WriteLine(r));

        }
    }
}

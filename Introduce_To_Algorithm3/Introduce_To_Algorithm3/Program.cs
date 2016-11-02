

using System;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.OpenSourceLib.NetMqs.ReqRep;
using Introduce_To_Algorithm3.OpenSourceLib.NetWorkCommsDotNets.Sources;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {




            NetworkCommsClient.Init();
            while (true)
            {
                string Input = Console.ReadLine();
                try
                {
                    NetworkCommsClient.Send(Input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("发生异常：" + ex);
                }
            }

            NetworkCommsServer.Start();
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}

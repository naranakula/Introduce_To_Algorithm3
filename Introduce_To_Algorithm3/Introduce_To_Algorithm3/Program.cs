

using System;
using System.Threading;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.OpenSourceLib.NetMqs.ReqRep;
using Introduce_To_Algorithm3.OpenSourceLib.NetWorkCommsDotNets.Sources;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for (int i = 0; i < 10000; i++)
            {
                double d = CpuMemoryUtils.GetNextCpuUsedPercent();
                Console.WriteLine(d);
                Thread.Sleep(100);
            }
        }
    }
}

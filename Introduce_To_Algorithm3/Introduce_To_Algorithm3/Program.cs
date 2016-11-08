

using System;
using System.IO;
using System.Linq;
using System.Threading;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.OpenSourceLib.NetMqs.ReqRep;
using Introduce_To_Algorithm3.OpenSourceLib.NetWorkCommsDotNets.Sources;
using Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("UI"+DateTime.Now);
            QuartzHelper.GetInstance().ScheduleJob(QuartzHelper.CreateJob("TestJob",typeof(JobImpl)),QuartzHelper.CreateSimpleTrigger("TestTrigger",1,15));
            QuartzHelper.GetInstance().Start();
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}

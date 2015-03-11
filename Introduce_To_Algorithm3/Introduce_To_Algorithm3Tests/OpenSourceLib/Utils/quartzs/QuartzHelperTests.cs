using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Introduce_To_Algorithm3.OpenSourceLib.Utils.Tests
{
    [TestClass()]
    public class QuartzHelperTests
    {

        [TestMethod()]
        public void StartTest()
        {
            QuartzHelper quartzHelper = QuartzHelper.GetInstance();
            quartzHelper.Start();
            quartzHelper.ScheduleJob(quartzHelper.CreateJob("firstjob", typeof(JobImpl)), quartzHelper.CreateCronTrigger("firstTrigger", 5, "0/5 * * * * ?"));
            Console.ReadLine();
            quartzHelper.Shutdown();
        }

    }
}

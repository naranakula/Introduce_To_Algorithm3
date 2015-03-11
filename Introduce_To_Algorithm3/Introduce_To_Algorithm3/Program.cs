using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Utility.Commons;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Introduce_To_Algorithm3.Common.Math;
using Introduce_To_Algorithm3.Common.Structs;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.threads;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs;
using MathNet.Numerics.LinearAlgebra.Double;
using Quartz;
using Quartz.Impl;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            QuartzHelper quartzHelper = QuartzHelper.GetInstance();
            quartzHelper.Start();
            quartzHelper.ScheduleJob(quartzHelper.CreateJob("firstjob",typeof(JobImpl)), quartzHelper.CreateCronTrigger("firstTrigger",5,"0/5 * * * * ?"));
            Console.ReadLine();
            quartzHelper.Shutdown();
        }
    }
}
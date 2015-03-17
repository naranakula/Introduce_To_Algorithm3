using System;
using System.Collections.Generic;
using System.Configuration;
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
using log4net.Util;
using MathNet.Numerics.LinearAlgebra.Double;
using Quartz;
using Quartz.Impl;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*StartupUtils.SetStartup();
            Console.WriteLine("AppFullName="+FileUtils.GetAppFullName());
            Console.WriteLine("AppName="+FileUtils.GetAppName());
            Console.WriteLine("CurrentDir="+FileUtils.GetCurrentDir());
            Console.WriteLine("File="+new FileInfo("file.xml").FullName);
            Console.WriteLine("AppDir="+FileUtils.GetAppDir());

            Directory.SetCurrentDirectory(FileUtils.GetAppDir());
            Console.WriteLine("CurrentDir=" + FileUtils.GetCurrentDir());
            Console.WriteLine("File=" + new FileInfo("file.xml").FullName);*/
            BeITMemcachedHelper.GetInstance().Setup("192.168.163.204", new string[] {"192.168.163.204"});
            Object obj = BeITMemcachedHelper.GetInstance().Get<String>("192.168.163.204");
            Console.WriteLine(obj);
            Directory.SetCurrentDirectory(FileUtils.GetAppDir());


            Console.WriteLine(CodeCounter.GetCodeLines(@"C:\Users\cmlu\Documents\GitHub\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3"));

            QuartzHelper quartzHelper = QuartzHelper.GetInstance();
            quartzHelper.Start();
            quartzHelper.ScheduleJob(quartzHelper.CreateJob("firstjob", typeof(JobImpl)), quartzHelper.CreateSimpleTrigger("triggerName",0, 4));
            Thread.Sleep(10000);
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
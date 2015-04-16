using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
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
using Introduce_To_Algorithm3.Common.Utils.sqls;
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
            DateTime now = DateTime.Now;
            string dir = now.ToString("yyyy-MM");
            string file = now.ToString("yyyy-MM-dd")+".txt";
            file = Path.Combine(dir, file);
            if (Directory.Exists(dir) && File.Exists(file))
            {
                return;
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!File.Exists(file))
            {
                File.CreateText(file).Close();
            }

            /* string sql = @"SELECT [VALUE]
      ,[REMARK]
      ,[TYPE]
      ,[Relation]
  FROM [Fids].[dbo].[fids_cfg_dict]";
            var dt =SqlHelper.GetInstance(@"User ID=sa;Initial Catalog=Fids;Data Source=CMLU-PC\CMLU;Password=558276344")
                .ExecuteDataTable(sql);
            StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                //string line = string.Format("dicts.Add(new CfgDict {3}" + " Id = Guid.NewGuid(), CreatedTime = DateTime.Now, Description = \"\",Value = \"{0}\", Remark = \"{1}\", Type = \"{2}\" " + "{4});", row["VALUE"], row["REMARK"], row["TYPE"],"{","}");
                string line = row["TYPE"].ToString()+" |";
                list.Add(line);
                sb.Append(line);
            }
            
            File.WriteAllLines(@"D:\io.txt",list.Distinct().ToArray(),Encoding.UTF8);
            */

            //File.WriteAllText(@"D:\io.txt",sb.ToString(),Encoding.UTF8);



            /*
            Console.WriteLine("定时任务测试");
            String jobName = "FirstJob";
            ITrigger trigger = QuartzHelper.GetInstance().CreateSimpleTrigger("FirstTrigger", 1, 5);
            IJobDetail job = QuartzHelper.GetInstance().CreateJob(jobName, typeof (JobImpl));
            Console.WriteLine("调度任务");
            QuartzHelper.GetInstance().ScheduleJob(job,trigger);
            QuartzHelper.GetInstance().Start();
            while (true)
            {
                Console.WriteLine("P：暂停任务；R:恢复任务");
                String line = Console.ReadLine();
                if (line == "P" || line == "p")
                {
                    QuartzHelper.GetInstance().Pause(jobName);
                }

                if (line == "R" || line == "r")
                {
                    QuartzHelper.GetInstance().Resume(jobName);
                }
            }

            Console.WriteLine("输入任意内容删除Job");
            Console.ReadLine();
            QuartzHelper.GetInstance().Delete(jobName);*/
            Console.ReadLine();
        }
    }





}
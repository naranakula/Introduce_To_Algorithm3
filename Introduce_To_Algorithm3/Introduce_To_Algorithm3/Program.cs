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
using Introduce_To_Algorithm3.Common.Utils.sockets;
using Introduce_To_Algorithm3.Common.Utils.sqls;
using Introduce_To_Algorithm3.Common.Utils.threads;
using Introduce_To_Algorithm3.OpenSourceLib;
using Introduce_To_Algorithm3.OpenSourceLib.Cache;
using Introduce_To_Algorithm3.OpenSourceLib.FTPUtils;
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
            SystemNetFtpClientImpl.Init("cmlu", "558276344","192.168.163.230");
            var instance = SystemNetFtpClientImpl.GetInstance();
            instance.Connect();
            instance.CreateDirectory("/test/cmlu");
            if (instance.FileExists("/index.html"))
                instance.DeleteFile("/index.html");
            instance.GetListing();
            instance.WriteFile("/test/cmlu.txt");
            instance.Close();
        }
    }

}
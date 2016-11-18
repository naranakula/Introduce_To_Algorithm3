

using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.files;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2;
using Introduce_To_Algorithm3.OpenSourceLib.SharpZipLib;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string logFile = "update_" + DateTime.Now.ToString("yyyyMM") + ".log";
            Console.WriteLine(logFile);
        }
    }
}

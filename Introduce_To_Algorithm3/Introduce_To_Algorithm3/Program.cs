﻿

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Forms;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.files;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2;
using Introduce_To_Algorithm3.OpenSourceLib.Json;
using Introduce_To_Algorithm3.OpenSourceLib.Rxs;
using Introduce_To_Algorithm3.OpenSourceLib.SharpZipLib;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using Newtonsoft.Json;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NLogHelper.Info(GuidUtils.GetGuid2());
            NLogHelper.Trace("Trace");
            NLogHelper.Debug("Debug");
            NLogHelper.Info("Info");
            NLogHelper.Warn("Warn");
            NLogHelper.Error("Error");
            NLogHelper.Fatal("Fatal");
            string str = JsonHelper.ToJson(1);
            JsonHelper.ParseJsonExample();

            string jsonStr = @"{
      'CPU': 'Intel',
      'Drives': [
      'DVD read/writer',
      '500 gigabyte hard drive'
      ]
      }";

            jsonStr = JsonHelper.FromJsonToXml(jsonStr);
        }
    }
}

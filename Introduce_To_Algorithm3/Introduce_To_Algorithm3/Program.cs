using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.files;
using Introduce_To_Algorithm3.Common.Utils.Serial;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite;
using Introduce_To_Algorithm3.OpenSourceLib.Dapper;
using Introduce_To_Algorithm3.OpenSourceLib.FTP;
using Introduce_To_Algorithm3.OpenSourceLib.grpcs.examples;
using Introduce_To_Algorithm3.OpenSourceLib.NetMqs.ZeroMqs;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using com.cmlu.services;
using Introduce_To_Algorithm3.OpenSourceLib.grpcs;
using Introduce_To_Algorithm3.OpenSourceLib.grpcs.protos;
using Grpc.Core;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var item = CacheHelper.Cache;
            NLogHelper.Info(GuidUtils.GetGuid2());
            DirectoryHold.ResetCurrentDir();
            NLogHelper.Trace("Trace");
            NLogHelper.Debug("Debug");
            NLogHelper.Info("Info");
            NLogHelper.Warn("Warn");
            NLogHelper.Error("Error");
            NLogHelper.Fatal("Fatal");
        }
    }
}


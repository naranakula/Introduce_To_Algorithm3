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
using System.Threading;
using System.Threading.Tasks;
using com.cmlu.services;
using Introduce_To_Algorithm3.OpenSourceLib.grpcs;
using Introduce_To_Algorithm3.OpenSourceLib.grpcs.protos;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2;
using Introduce_To_Algorithm3.OpenSourceLib.AutoMapper;
using Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //红黑树
            SortedDictionary<string,string> rbTree = new SortedDictionary<string, string>();
            DirectoryHold.ResetCurrentDir();
            NLogAsyncHelper.Info(GuidUtils.GetGuid());
            NLogAsyncHelper.Trace("Trace");
            NLogAsyncHelper.Debug("Debug");
            NLogAsyncHelper.Info("Info");
            NLogAsyncHelper.Warn("Warn");
            NLogAsyncHelper.Error("Error");
            NLogAsyncHelper.Fatal("Fatal");
        }
    }
    
}


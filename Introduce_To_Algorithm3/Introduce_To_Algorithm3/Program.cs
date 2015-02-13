using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Com.Utility.Commons;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Introduce_To_Algorithm3.Common.Math;
using Introduce_To_Algorithm3.Common.Structs;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.threads;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DateTime before = DateTime.Now;
            Timer timer = TimerUtils.StartTimer(obj =>
            {
                Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId + "        " + (DateTime.Now - before));
                before = DateTime.Now;
                Thread.Sleep(6000);
            }, 1000, 1000);
            Console.Read();
        }
    }
}
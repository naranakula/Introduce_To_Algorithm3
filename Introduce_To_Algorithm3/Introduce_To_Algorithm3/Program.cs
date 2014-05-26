using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.GraphEx;
using Introduce_To_Algorithm3.Common.Math;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Structs;
using Introduce_To_Algorithm3.Common.Utils.sockets;
using System.Net.Sockets;
using System.Threading;
using Introduce_To_Algorithm3.Common.DynamicProgramming;
using System.Net;
using Introduce_To_Algorithm3.Common.Sort;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        private static void Main(string[] args)
        {
            int n = 50; // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            actual = Fibonacci.FnLinear(n);
            Console.WriteLine(actual + "\t\t" + watch.Elapsed);
            watch.Restart();
            expected = Fibonacci.Fn(n);
            Console.WriteLine(expected + "\t\t" + watch.Elapsed);
        }
    }
}
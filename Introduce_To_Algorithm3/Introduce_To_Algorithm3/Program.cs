using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Introduce_To_Algorithm3.Common.Utils;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(
                CodeCounter.GetCodeLines(
                    @"C:\Users\chlu\Documents\GitHub\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3"));
            //////////////////////
            int i = 0;
            while (true)
            {
                Console.WriteLine("Hello world"+i++);
                System.Threading.Thread.Sleep(100);
                if (i > int.MaxValue - 100)
                {
                    i = 0;
                }
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.GraphEx;
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
            FibonacciHeap<int,int> heap = new FibonacciHeap<int, int>();
            for (int i = 0; i < 1000000; i++)
            {
                heap.Insert(i,i);
            }
            for (int i = 0; i < 1000000; i++)
            {
                var r = heap.ExtractMin();
                if (r.Key != i) 
                    Console.WriteLine(r.Key);
            }
        }
    }
}
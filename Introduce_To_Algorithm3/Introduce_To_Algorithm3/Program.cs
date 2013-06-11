using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.DynamicProgramming;
using Introduce_To_Algorithm3.Common.GreedyAlgorithm;
using Introduce_To_Algorithm3.Common.Math;
using Introduce_To_Algorithm3.Common.Search;
using Introduce_To_Algorithm3.Common.Sort;
using Introduce_To_Algorithm3.Common.Structs;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            FibonacciHeap<int,int> heap = new FibonacciHeap<int, int>();
            for (int i = 0; i < 1; i++)
            {
                heap.Insert(i,i);
            }
            FibonacciHeap<int, int> heap1 = new FibonacciHeap<int, int>();
            for (int i = 2; i < 4; i++)
            {
                heap1.Insert(i, i);
            }

            heap = FibonacciHeap<int, int>.Union(heap, heap1);
        }
    }
}

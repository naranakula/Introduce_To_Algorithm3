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
            BMinusTree<int,int> tree = new BMinusTree<int, int>(37);

            for (int i = 0; i < 200; i++)
            {
                tree.Insert(i, i);
            }
            Console.WriteLine(tree.Heigth);
            for (int i = 0; i < 200000; i++)
            {
                tree.Insert(i, i);
            }
            Console.WriteLine(tree.Heigth);
        }
    }
}

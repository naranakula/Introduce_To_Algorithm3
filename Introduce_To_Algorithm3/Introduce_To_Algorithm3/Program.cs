using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            List<KnapsackItem> items = new List<KnapsackItem>();
            items.Add(new KnapsackItem(){Value = 50,Weight = 40});
            items.Add(new KnapsackItem(){Value = 35,Weight = 35});
            items.Add(new KnapsackItem(){Value = 25,Weight = 25});
            Console.WriteLine(Knapsack.DpKanpsack(items,60));
        }
    }
}

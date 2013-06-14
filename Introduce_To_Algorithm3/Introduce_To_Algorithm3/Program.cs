using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.DynamicProgramming;
using Introduce_To_Algorithm3.Common.Graph;
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
            List<Tuple<int, int>> lists = new List<Tuple<int, int>>();
            lists.Add(new Tuple<int, int>(1,2));
            lists.Add(new Tuple<int, int>(1,3));
            lists.Add(new Tuple<int, int>(2,3));
            lists.Add(new Tuple<int, int>(2,4));
            lists.Add(new Tuple<int, int>(6,5));
            lists.Add(new Tuple<int, int>(5,7));
            lists.Add(new Tuple<int, int>(8,9));
            lists.Add(new Tuple<int, int>(10,10));
            var i = DisjointSet.ConnectedComponent(lists);
        }
    }
}

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
            BMinusTreeNode<int,int> node = new BMinusTreeNode<int, int>(){IsLeaf = true};
            node.KeyValues = new Tuple<int, int>[]
                {
                    new Tuple<int, int>(1,1),
                    new Tuple<int, int>(2,2),
                    new Tuple<int, int>(3,3),
                    new Tuple<int, int>(3,3),
                    new Tuple<int, int>(4,4),
                    new Tuple<int, int>(6,6), 
                };
            BMinusTree<int,int> tree = new BMinusTree<int, int>(4);
            tree.BinarySearch(node, 2);
            
        }
    }
}

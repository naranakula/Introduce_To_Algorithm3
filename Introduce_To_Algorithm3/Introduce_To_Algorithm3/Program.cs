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
            List<Tuple<char,double>> lists = new List<Tuple<char, double>>();
            lists.Add(new Tuple<char, double>('a',0.45));
            lists.Add(new Tuple<char, double>('b',0.13));
            lists.Add(new Tuple<char, double>('c',0.12));
            lists.Add(new Tuple<char, double>('d',0.16));
            lists.Add(new Tuple<char, double>('e',0.09));
            lists.Add(new Tuple<char, double>('f',0.05));

            var huffman = HuffmanTree<char>.Build(lists);
        }
    }
}

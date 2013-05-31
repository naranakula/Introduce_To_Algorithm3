using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.DynamicProgramming;
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
            Console.WriteLine(LCS.Lcs("hellowwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwkwww", "yyyyyyyllwyyyyyyyyy"));
            Console.WriteLine(LCS.LcsUnConsecutiveRecursive("hellowwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwkwww", "yyyyyyylldwyyyyyyyyy"));
            Console.ReadKey();
        }
    }
}

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
            int[] arrs = {0,1,5,8, 9, 10, 17, 17, 20,24, 30};
            List<int> s;
            for (int i = 1; i <= 11; i++)
            {
                Console.WriteLine(CutRod.MemoizedCutRod(arrs,i)+"   "+CutRod.Cut_Rod(arrs,i)+"  "+CutRod.BottomUpCutRod(arrs,i)+"  "+CutRod.ExtendedBottomUpCutRod(arrs,i,out s));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Search;
using Introduce_To_Algorithm3.Common.Sort;
using Introduce_To_Algorithm3.Common.Structs;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 1, -1, 2, 3, -8,7,3,100 ,-10}; // TODO: 初始化为适当的值
            int[] expected = null; // TODO: 初始化为适当的值
            int[] actual;
            int i = OrderStatics<int>.Select(arr, 1);
            Console.WriteLine(i);
        }
    }
}

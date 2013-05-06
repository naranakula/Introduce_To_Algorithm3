using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            int[] arrs = {1, -2, 3, 4, 5};
            HashTableOpenAddressing<string,string> hash = new HashTableOpenAddressing<string, string>();
            arrs.ToList().ForEach(i => hash.Insert(i.ToString(),i.ToString()));
            Console.WriteLine(hash.Search(5.ToString()));
            Console.WriteLine(hash.Search("-2".ToString()));
            hash.Delete("3");
        }
    }
}

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
            BST<int,string> bst = new BST<int, string>();

            arrs.ToList().ForEach(i=>bst.Insert(i,""+i));
            bst.Delete(bst.Search_(5));
            bst.Delete(bst.Search_(3));
        }
    }
}

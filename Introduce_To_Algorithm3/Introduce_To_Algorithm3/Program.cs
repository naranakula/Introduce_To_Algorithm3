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
            int[] arrs = {9,1, 2, 3, 4, 5, 6,0, 7,  9};
            RBT<string,int> rbts = new RBT<string, int>();
            arrs.ToList().ForEach(i=>rbts.Insert(""+i,i));
            rbts.InorderTreeWalk().ForEach(Console.WriteLine);
            for (int i = 0; i < arrs.Length; i++)
            {
                rbts.Delete(rbts.Search_(arrs[i].ToString()));
            }
        }
    }
}

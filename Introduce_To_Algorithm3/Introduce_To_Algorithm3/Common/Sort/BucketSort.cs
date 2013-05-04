using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Sort
{
    public class BucketSort
    {
        /// <summary>
        /// each element between [0,1)
        /// it runs at O(n)
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static double[] Sort(double[] arr)
        {
            if(arr == null || arr.Length <=1)
            {
                return arr;
            }
            List<double>[] lists = new List<double>[arr.Length];
            for (int i = 0; i < lists.Length; i++)
            {
                lists[i] = new List<double>();
            }

            foreach (var d in arr)
            {
               lists[(int)(d*arr.Length)].Add(d);
            }
            for (int i = 0; i < lists.Length; i++)
            {
                lists[i].Sort();
            }

            double[] results = new double[arr.Length];
            int j = 0;
            for (int i = 0; i < lists.Length; i++)
            {
                foreach (var d in lists[i])
                {
                    results[j++] = d;
                }
            }

            return results;
        }
    }
}

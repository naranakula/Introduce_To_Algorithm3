using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Sort
{
    /// <summary>
    /// CountingSort is used to sort n integers in the range 0 to k.
    /// It runs at O(n+k)
    /// </summary>
    public class CountingSort
    {
        /// <summary>
        /// CountingSort is used to sort n integers in the range 0 to k.
        /// It runs at O(n+k) AND stable
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="max">max is the largest one in arr</param>
        /// <returns></returns>
        public static  int[] Sort(int[] arr,int max)
        {
            if(arr == null || arr.Length<=1)
            {
                return arr;
            }
            foreach (var i in arr)
            {
                if(!(i>=0)&&(i<=max))
                {
                    throw new Exception("element in array must between [0,max]");
                }
            }

            int[] counts = new int[max+1];
            foreach (var i in arr)
            {
                counts[i]++;
            }
            for (int i = 1; i < counts.Length; i++)
            {
                counts[i] = counts[i] + counts[i - 1];
            }
            int[] result = new int[arr.Length];

            for(int i=arr.Length-1;i>=0;i--)
            {
                result[counts[arr[i]] - 1] = arr[i];
                counts[arr[i]]--;
            }

            return result;
        }
    }
}

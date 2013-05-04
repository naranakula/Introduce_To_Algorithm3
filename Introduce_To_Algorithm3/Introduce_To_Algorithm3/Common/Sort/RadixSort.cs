using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Sort
{
    /// <summary>
    /// it sort n element which each element is of length - k
    /// </summary>
    public class RadixSort<T> where T:IComparable<T>,IList<T>
    {
        /// <summary>
        /// Given n d-digit numbers in which each digit can take on up to k possible values,RADIX-SORT correctly sorts these numbers in d(n+k) time if the stable sort it uses takes (n+k) time.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="sortProxy"> the sort must stable</param>
        /// <returns></returns>
        public static T[] Sort(T[] arr,StableSort sortProxy)
        {
            if(arr == null || arr.Length <=1)
            {
                return arr;
            }

            int len = arr[0].Count;
            for (int i = 1; i < arr.Length; i++)
            {
                if(arr[i].Count != len)
                {
                    throw new Exception("element must have same length");
                }
            }

            //from low position to high position
            for(int i=0;i<arr.Length;i++)
            {
                // use a stable sort to sort array on digit i
                sortProxy(arr, i);
            }

            return arr;
        }


        public delegate void StableSort(T[] arr, int i);
    }
}

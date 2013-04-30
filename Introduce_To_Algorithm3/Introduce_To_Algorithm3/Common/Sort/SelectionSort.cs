using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Sort
{
    /// <summary>
    /// find the smallest one and exchange with a[0], find the second smallest and exchange with a[1]
    /// it runs at O(n^2)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SelectionSort<T> where T : IComparable<T>
    {
        /// <summary>
        /// Select sort. It sorts in place and stable. it runs at O(n^2)
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static T[] Sort(T[] arr)
        {
            if (arr == null || arr.Length <= 1)
            {
                return arr;
            }

            return Sort(arr, 0, arr.Length - 1);
        }


        /// <summary>
        /// hi must >= lo, and sort arr[lo...hi] in place. Doesn't affect the element out of [lo,hi]
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static T[] Sort(T[] arr, int lo, int hi)
        {
            for(int i=lo;i<hi;i++)
            {
                int minIndex = i;
                for (int j = i + 1; j <= hi; j++)
                {
                    if(arr[minIndex].CompareTo(arr[j])>0)
                    {
                        minIndex = j;
                    }
                }
                SortCommons<T>.Exchange(arr,minIndex,i);
            }
            return arr;
        }
    }
}

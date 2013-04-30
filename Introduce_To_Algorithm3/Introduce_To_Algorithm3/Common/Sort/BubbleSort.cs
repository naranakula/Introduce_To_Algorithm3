using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Sort
{
    /// <summary>
    /// It works by repeatedly swapping adjacent elements that are out of order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BubbleSort<T> where T : IComparable<T>
    {

        /// <summary>
        /// bubble sort. It sorts in place and stable. it runs at O(n^2)
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
            for (int i = hi; i > lo; i--)
            {
                for (int j = lo; j < i; j++)
                {
                    if (arr[j].CompareTo(arr[j + 1]) > 0)
                    {
                        SortCommons<T>.Exchange(arr, j, j + 1);
                    }
                }
            }
            return arr;
        }



    }
}

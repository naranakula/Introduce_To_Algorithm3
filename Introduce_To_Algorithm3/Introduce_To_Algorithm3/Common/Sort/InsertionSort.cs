using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Sort
{
    /// <summary>
    /// Insertion Sort: like play card, you sort card in your hand and put a new one into the already sorted cards.
    /// It sort in place and stable.
    /// run at O(n^2).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InsertionSort<T> where T : IComparable<T>
    {
        /// <summary>
        /// Insertion sort. It sorts in place and stable. it runs at O(n^2)
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
        private static T[] Sort(T[] arr, int lo, int hi)
        {
            for (int i = lo + 1; i <= hi; i++)
            {
                //[lo,i) are aleady sorted,just put i into it
                int j = i-1;
                T key = arr[i];
                while (j >= lo && arr[j].CompareTo(key) > 0)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
            return arr;
        }


    }
}

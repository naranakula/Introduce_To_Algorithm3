using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Sort;

namespace Introduce_To_Algorithm3.Common.Search
{
    public class OrderStatics<T> where T : IComparable<T>
    {
        /// <summary>
        /// return the min
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public T Minimum(T[] arr)
        {
            T min = arr[0];
            for(int i=1;i<arr.Length;i++)
            {
                if(min.CompareTo(arr[i])>0)
                {
                    min = arr[i];
                }
            }
            return min;
        }

        /// <summary>
        /// return the max
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public T Maximum(T[] arr)
        {
            T max = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                if (max.CompareTo(arr[i]) < 0)
                {
                    max = arr[i];
                }
            }
            return max;
        }

        /// <summary>
        /// select the ith minimum one
        /// it runs at O(n)
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static T Select(T[] arr,int i)
        {
            QuickSort<T>.RandomRearrange(arr);
            return Select(arr, 0, arr.Length-1,i-1);
        }

        /// <summary>
        /// slect the i+1 min
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="i">i must in [lo,hi]</param>
        /// <returns></returns>
        public static T Select(T[] arr,int lo,int hi, int i)
        {
            if(lo == hi)
            {
                return arr[lo];
            }
            int q = QuickSort<T>.Partition(arr, lo, hi);
            if(q==i)
            {
                return arr[q];
            }
            
            if(q>i)
            {
                return Select(arr, lo, q-1, i);
            }
            else
            {
                return Select(arr, q+1, hi, i);
            }
        }
    }
}

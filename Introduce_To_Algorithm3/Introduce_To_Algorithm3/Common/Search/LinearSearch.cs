using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Search
{
    public static class LinearSearch<T> where T:IComparable<T>
    {
        /// <summary>
        /// search k in array.
        /// Return nongative if not searched. Return the 0 based position if searched
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static int Search(T[] arr,T key)
        {
            if(arr == null || arr.Length==0)
            {
                return -1;
            }

            return Search(arr, key, 0, arr.Length - 1);
        }


        /// <summary>
        /// search k in array[lo...hi].
        /// Return nongative if not searched. Return the 0 based position if searched
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="key"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static int Search(T[] arr, T key,int lo,int hi)
        {
            if (arr == null || arr.Length == 0)
            {
                return -1;
            }
            int index = -1;
            for (int i = lo; i <= hi; i++)
            {
                if(arr[i].CompareTo(key)==0)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
    }
}

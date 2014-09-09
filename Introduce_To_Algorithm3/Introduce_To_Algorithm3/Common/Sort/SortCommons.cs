using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Sort
{
    public static class SortCommons<T> where T:IComparable<T>
    {
        /// <summary>
        /// exchange two object in array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="ex1"></param>
        /// <param name="ex2"></param>
        public static void Exchange(T[] arr, int ex1, int ex2)
        {
            if (ex1 == ex2)
            {
                return;
            }
            T tmp = arr[ex1];
            arr[ex1] = arr[ex2];
            arr[ex2] = tmp;
        }

        /// <summary>
        /// decide whether a array is sorted
        /// it sorted if the array is empty or just has one element
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static  bool IsSorted(T[] arr)
        {
            if(arr == null || arr.Length <= 1)
            {
                return true;
            }

            return IsSorted(arr, 0, arr.Length - 1);
        }

        /// <summary>
        /// decide whether a array is sorted between [lo,hi]
        /// it sorted if the array is empty or just has one element
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static bool IsSorted(T[] arr,int lo,int hi)
        {
            if (arr == null || arr.Length <= 1)
            {
                return true;
            }

            bool isSorted = true;
            for(int i=lo;i<hi;i++)
            {
                if(arr[i].CompareTo(arr[i+1])>0)
                {
                    isSorted = false;
                    break;
                }
            }
            return isSorted;
        }

    }
}

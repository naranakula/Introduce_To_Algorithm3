using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Sort
{

    
    /// <summary>
    /// Merge Sort:Divde into 2 sub array and sort them recursively, then combine the array into one
    /// Merge sort isn't sort in place and not stable but runs at O(nlgn)
    /// combine:like we have two sorted cards on desk. we pick the first one for each and pick the smaller in our hands 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MergeSort<T> where T:IComparable<T>
    {
        /// <summary>
        /// promote sort by using insertion sort if the subarray is small because the insertion sort is quicker when the length of array is not big.
        /// This is the standard of small length
        /// </summary>
        private const int SMALLARRAYLENGTH = 10;

        /// <summary>
        /// Merge sort isn't sort in place and not stable but runs at O(nlgn)
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static T[] Sort(T[] arr)
        {
            if (arr == null || arr.Length <= 1)
            {
                return arr;
            }
            // we need this because the sort isn't in place. We only need only one auxiliary array for all recursions
            T[] tmp = new T[arr.Length];
            Sort(arr, tmp,0, arr.Length - 1);
            return arr;
        }

        /// <summary>
        /// promote sort by using insertion sort if the subarray is small because the insertion sort is quicker when the length of array is not big.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static T[] SortPromote(T[] arr)
        {
            if (arr == null || arr.Length <= 1)
            {
                return arr;
            }
            // we need this because the sort isn't in place. We only need only one auxiliary array for all recursions
            T[] tmp = new T[arr.Length];
            SortPromote(arr, tmp, 0, arr.Length - 1);
            return arr;
        }

        /// <summary>
        /// hi must >= lo,  Doesn't affect the element out of [lo,hi]
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="tmp"> </param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static void Sort(T[] arr,T[] tmp, int lo, int hi)
        {
            if(lo>=hi)
            {
                return;
            }
            int mid = (lo + hi)/2;
            Sort(arr,tmp, lo, mid);
            Sort(arr,tmp, mid + 1, hi);

            Combine(arr, tmp, lo, mid, hi);
        }


        /// <summary>
        /// hi must >= lo, and sort arr[lo...hi] in place. Doesn't affect the element out of [lo,hi]
        /// promote sort by using insertion sort if the subarray is small because the insertion sort is quicker when the length of array is not big.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="tmp"> </param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static void SortPromote(T[] arr, T[] tmp, int lo, int hi)
        {
            if (hi -lo < SMALLARRAYLENGTH)
            {
                InsertionSort<T>.Sort(arr, lo, hi);
                return;
            }
            int mid = (lo + hi) / 2;
            Sort(arr, tmp, lo, mid);
            Sort(arr, tmp, mid + 1, hi);

            Combine(arr, tmp, lo, mid, hi);
        }


        /// <summary>
        /// combine the [lo,mid] and (mid,hi] into one
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="tmp"> we need this because the sort isn't in place. We only need only one auxiliary array for all recursions </param>
        /// <param name="lo"></param>
        /// <param name="mid"></param>
        /// <param name="hi"></param>
        private static void Combine(T[] arr,T[] tmp,int lo,int mid,int hi)
        {
            int loBegin = lo, hiBegin = mid + 1,i= lo;
            while(i<=hi)
            {
                while (loBegin <= mid && hiBegin<=hi)
                {
                    if(arr[loBegin].CompareTo(arr[hiBegin])<0)
                    {
                        tmp[i++] = arr[loBegin++];
                    }
                    else
                    {
                        tmp[i++] = arr[hiBegin++];
                    }
                }

                if(loBegin>mid)
                {
                    //alreay copyed all the left part
                    tmp[i++] = arr[hiBegin++];
                }
                else
                {
                    tmp[i++] = arr[loBegin++];
                }
            }

            Array.Copy(tmp,lo,arr,lo,hi-lo+1);
        }

    }
}

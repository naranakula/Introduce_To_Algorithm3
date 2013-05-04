using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Sort
{
    /// <summary>
    /// quick sort:
    /// 1)Divide A[p...r] into two part A[p...q-1] and [q+1...r] such that each element of A[p...q-1] is less than or equal to A[q] , which is ,in turn, less than or equal to each element of A[q+1....r]
    /// 2)sort two subarray A[p,q-1] and A[q+1...r] recursively
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuickSort<T> where T : IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// quick sort the arr, it runs at O(nlgn) and not stable(which means the sorted result cannot reserve the equal element position)
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static  T[] Sort(T[] arr)
        {
            if (arr == null || arr.Length <= 1)
            {
                return arr;
            }

            Sort(arr, 0, arr.Length - 1);
            return arr;
        }

        /// <summary>
        /// quick sort the arr, it runs at O(nlgn) and not stable(which means the sorted result cannot reserve the equal element position)
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static  T[] SortPromote(T[] arr)
        {
            if (arr == null || arr.Length <= 1)
            {
                return arr;
            }
            RandomRearrange(arr);
            SortPromote(arr, 0, arr.Length - 1);
            return arr;
        }

        /// <summary>
        /// quick sort the arr, it runs at O(nlgn) and not stable(which means the sorted result cannot reserve the equal element position)
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo">you should guarantee the availabity of the index</param>
        /// <param name="hi"></param>
        public static void Sort(T[] arr, int lo, int hi)
        {
            if (arr == null || arr.Length <= 1 || hi<=lo)
            {
                return;
            }

            sort(arr, lo, hi);
        }

        /// <summary>
        /// quick sort the arr, it runs at O(nlgn) and not stable(which means the sorted result cannot reserve the equal element position)
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo">you should guarantee the availabity of the index</param>
        /// <param name="hi"></param>
        private static void sort(T[] arr, int lo, int hi)
        {
            if (lo < hi)
            {
                int q = Partition(arr, lo, hi);
                sort(arr, lo, q - 1);
                sort(arr, q + 1, hi);
            }
        }

        /// <summary>
        /// quick sort the arr, it runs at O(nlgn) and not stable(which means the sorted result cannot reserve the equal element position)
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo">you should guarantee the availabity of the index</param>
        /// <param name="hi"></param>
        public static void SortPromote(T[] arr, int lo, int hi)
        {
            if (arr == null || arr.Length <= 1 || hi <= lo)
            {
                return;
            }

            sortPromote(arr, lo, hi);
        }

        /// <summary>
        /// quick sort the arr, it runs at O(nlgn) and not stable(which means the sorted result cannot reserve the equal element position)
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo">you should guarantee the availabity of the index</param>
        /// <param name="hi"></param>
        private static void sortPromote(T[] arr, int lo, int hi)
        {
            if (lo < hi)
            {
                if(hi-lo<9)
                {
                    InsertionSort<T>.Sort(arr, lo, hi);
                    return;
                }
                int q, k;
                Partition3(arr, lo, hi,out q,out k);
                sortPromote(arr, lo, q - 1);
                sortPromote(arr, k + 1, hi);
            }
        }

        /// <summary>
        /// Divide A[p...r] into two part A[p...q-1] and [q+1...r] such that each element of A[p...q-1] is less than A[q] , which is ,in turn, less than or equal to each element of A[q+1....r]
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo">you should guarantee the availabity of the index</param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static  int Partition(T[] arr, int lo, int hi)
        {
            T tmp = arr[hi];
            //let hi as pivot. from low postion, find the one lower than the pivot and move it to low position
            int q = lo - 1;
            for (int i = lo; i < hi; i++)
            {
                if(arr[i].CompareTo(tmp)<0)
                {
                    q++;
                    SortCommons<T>.Exchange(arr,q,i);
                }
            }
            SortCommons<T>.Exchange(arr,q+1,hi);
            return q + 1;
        }

        /// <summary>
        /// Divide A[p...r] into two part A[p...q-1] and [q+1...r] such that each element of A[p...q-1] is less than or equal to A[q] , which is ,in turn, less than each element of A[q+1....r]
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo">you should guarantee the availabity of the index</param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static int Partition2(T[] arr, int lo, int hi)
        {
            T tmp = arr[hi];
            //let hi as pivot. from low postion, find the one lower than the pivot and move it to low position
            int q = lo - 1;
            for (int i = lo; i < hi; i++)
            {
                if (arr[i].CompareTo(tmp) <= 0)
                {
                    q++;
                    SortCommons<T>.Exchange(arr, q, i);
                }
            }
            SortCommons<T>.Exchange(arr, q + 1, hi);
            return q + 1;
        }


        /// <summary>
        /// Divide A[p...r] into two part A[p...q-1] and [k+1...r] such that each element of A[p...q-1] is less than A[q...k] ,a[q...k] are equal, which is ,in turn, less than each element of A[q+1....r]
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo">you should guarantee the availabity of the index</param>
        /// <param name="hi"></param>
        /// <param name="q"> </param>
        /// <param name="k"> </param>
        /// <returns></returns>
        public static void Partition3(T[] arr, int lo, int hi,out int q,out int k)
        {
            T tmp = arr[hi];
            //let hi as pivot. from low postion, find the one lower than the pivot and move it to low position
            q = lo - 1;
            k = lo-1;
            for (int i = lo; i < hi; i++)
            {
                if (arr[i].CompareTo(tmp) < 0)
                {
                    q++;
                    k++;
                    SortCommons<T>.Exchange(arr, q, i);
                }
                if(arr[i].CompareTo(tmp) == 0)
                {
                    k++;
                }
            }
            SortCommons<T>.Exchange(arr, q + 1, hi);
            q++;
            k++;
        }



        /// <summary>
        /// Rearrange the array,so the result is random
        /// </summary>
        /// <param name="arr"></param>
        public static  void RandomRearrange(T[] arr)
        {
            if(arr == null || arr.Length <= 1)
            {
                return;
            }
            Random rand = new Random();
            for(int i=0;i<arr.Length-1;i++)
            {
                int index = rand.Next(i, arr.Length);
                SortCommons<T>.Exchange(arr,i,index);
            }
        }
    }
}

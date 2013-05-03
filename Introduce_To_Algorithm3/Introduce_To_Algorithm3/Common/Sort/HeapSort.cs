using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Sort
{
    /// <summary>
    /// Heapsort runs in place and runs at O（nlgn)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class HeapSort<T> where T:IComparable<T>
    {

        /// <summary>
        /// The parent index of i
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static  int Parent(int i)
        {
            return (i - 1) / 2;
        }

        /// <summary>
        /// return the leftchild
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static  int Left(int i)
        {
            return 2 * i + 1;
        }

        /// <summary>
        /// return the right child
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static  int Right(int i)
        {
            return 2 * i + 2;
        }

        /// <summary>
        /// get the index of first Leaf
        /// </summary>
        /// <returns></returns>
        private static  int FirstLeaf(int heapsize)
        {
            return heapsize / 2;
        }


        /// <summary>
        /// lets the value at arr[i] “float down” in the max-heap so that the subtree rooted at index i obeys the max-heap property.
        /// it runs at O(lgn)
        /// </summary>
        /// <param name="arr" />
        /// <param name="i"></param>
        /// <param name="heapsize"> </param>
        private static  void Sink(T[] arr, int i,int heapsize)
        {
            int left = Left(i);
            int right = Right(i);
            int max = i;
            if (left < heapsize && arr[left].CompareTo(arr[max]) > 0)
            {
                max = left;
            }

            if (right < heapsize && arr[right].CompareTo(arr[max]) > 0)
            {
                max = right;
            }

            if (max != i)
            {
                SortCommons<T>.Exchange(arr, max, i);
                Sink(arr,max,heapsize);
            }
        }

        /// <summary>
        /// build a heap in O(n)
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static void BuildMaxHeap(T[] arr)
        {
            for (int i = FirstLeaf(arr.Length) - 1; i >= 0; i--)
            {
                Sink(arr,i,arr.Length);
            }
        }


        /// <summary>
        /// heapsort: it runs in place and runs at n(lgn)
        /// </summary>
        /// <param name="arr"></param>
        public static T[] Sort(T[] arr)
        {
            if (arr == null || arr.Length <= 1)
            {
                return arr;
            }
            BuildMaxHeap(arr);

            int heapsize = arr.Length;
            for(int i=heapsize;i>1;i--)
            {
                heapsize--;
                SortCommons<T>.Exchange(arr,heapsize,0);
                Sink(arr,0,heapsize);
            }
            return arr;
        }
    }
}

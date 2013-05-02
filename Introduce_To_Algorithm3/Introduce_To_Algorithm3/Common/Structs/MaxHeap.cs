using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Sort;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// the value of parent >= all his children
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaxHeap<T> where T:IComparable<T>
    {
        /// <summary>
        /// store the heap
        /// </summary>
        private T[] arr = new T[4];
        /// <summary>
        /// the heap length
        /// </summary>
        private int len;
        /// <summary>
        /// The parent index of i
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int Parent(int i)
        {
            return (i - 1) / 2;
        }

        /// <summary>
        /// return the leftchild
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int Left(int i)
        {
            return 2*i + 1;
        }

        /// <summary>
        /// return the right child
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int Right(int i)
        {
            return 2*i + 2;
        }

        /// <summary>
        /// get the index of first Leaf
        /// </summary>
        /// <returns></returns>
        private int FirstLeaf()
        {
            return len/2;
        }

        /// <summary>
        /// get the capaticity of heap
        /// </summary>
        public int Capaticity
        {
            get { return arr.Length; }
        }

        /// <summary>
        /// get the length of heap
        /// </summary>
        public int Length
        {
            get { return len; }
        }

        /// <summary>
        /// lets the value at arr[i] “float down” in the max-heap so that the subtree rooted at index i obeys the max-heap property.
        /// it runs at O(lgn)
        /// </summary>
        /// <param name="i"></param>
        private void Sink(int i)
        {
            int left = Left(i);
            int right = Right(i);
            int max = i;
            if(left<len && arr[left].CompareTo(arr[i])>0)
            {
                max = left;
            }

            if(right <len && arr[right].CompareTo(arr[i])>0)
            {
                max = right;
            }

            if(max != i)
            {
                SortCommons<T>.Exchange(arr,max,i);
                Sink(max);
            }
        }

        /// <summary>
        /// build a heap in O(n)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static MaxHeap<T> BuildMaxHeap(T[] source)
        {
            MaxHeap<T> heap = new MaxHeap<T>();
            if(source == null || source.Length == 0)
            {
                heap.len = 0;
                return heap;
            }
            if(source.Length == 1)
            {
                heap.arr[0] = source[0];
                heap.len = source.Length;
                return heap;
            }
            heap.len = source.Length;
            heap.arr = source;

            for(int i=heap.FirstLeaf()-1;i>=0;i--)
            {
                heap.Sink(i);
            }
            return heap;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Sort;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// the value of parent <= all his children
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MinHeap<T> where T : IComparable<T>
    {
        /// <summary>
        /// store the heap
        /// </summary>
        private T[] arr = new T[4];
        /// <summary>
        /// the heap length
        /// </summary>
        private int heapSize;
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
            return 2 * i + 1;
        }

        /// <summary>
        /// return the right child
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int Right(int i)
        {
            return 2 * i + 2;
        }

        /// <summary>
        /// get the index of first Leaf
        /// </summary>
        /// <returns></returns>
        public int FirstLeaf()
        {
            return heapSize / 2;
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
            get { return heapSize; }
        }

        /// <summary>
        /// get the data
        /// </summary>
        /// <returns></returns>
        public T[] GetData()
        {
            return arr;
        }

        /// <summary>
        /// lets the value at arr[i] “float down” in the min-heap so that the subtree rooted at index i obeys the min-heap property.
        /// it runs at O(lgn)
        /// </summary>
        /// <param name="i"></param>
        public void Sink(int i)
        {
            int left = Left(i);
            int right = Right(i);
            int max = i;
            if (left < heapSize && arr[left].CompareTo(arr[max]) < 0)
            {
                max = left;
            }

            if (right < heapSize && arr[right].CompareTo(arr[max]) < 0)
            {
                max = right;
            }

            if (max != i)
            {
                SortCommons<T>.Exchange(arr, max, i);
                Sink(max);
            }
        }

        /// <summary>
        /// lets the value at arr[i] swim up so that themaxheap  obeys the min-heap property.
        /// </summary>
        /// <param name="i"></param>
        public void Swimup(int i)
        {
            while (i > 0 && arr[Parent(i)].CompareTo(arr[i]) > 0)
            {

                SortCommons<T>.Exchange(arr, i, Parent(i));
                i = Parent(i);
            }
        }

        /// <summary>
        /// whether the heap is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return (arr == null || heapSize <= 0);
        }

        /// <summary>
        /// build a heap in O(n)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static MinHeap<T> BuildMinHeap(T[] source)
        {
            MinHeap<T> heap = new MinHeap<T>();
            if (source == null || source.Length == 0)
            {
                heap.heapSize = 0;
                return heap;
            }
            if (source.Length == 1)
            {
                heap.arr[0] = source[0];
                heap.heapSize = source.Length;
                return heap;
            }
            heap.heapSize = source.Length;
            heap.arr = source;

            for (int i = heap.FirstLeaf() - 1; i >= 0; i--)
            {
                heap.Sink(i);
            }
            return heap;
        }

        /// <summary>
        /// insert a node to heap
        /// </summary>
        /// <param name="node"></param>
        public void Insert(T node)
        {
            if (arr == null)
            {
                arr = new T[4];
            }

            if (heapSize >= arr.Length)
            {
                Array.Resize(ref arr, arr.Length * 2);
            }

            arr[heapSize] = node;
            Swimup(heapSize);
            heapSize++;
        }

        /// <summary>
        /// peek the max one from heap but not remove
        /// </summary>
        /// <returns>if the heap is empty, throw exceptions</returns>
        public T Peek()
        {
            if (IsEmpty())
            {
                throw new Exception("heap is empty");
            }

            return arr[0];
        }

        /// <summary>
        /// return and remove the max one
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (IsEmpty())
            {
                throw new Exception("heap is empty");
            }

            T tmp = arr[0];

            SortCommons<T>.Exchange(arr, 0, heapSize - 1);
            heapSize--;
            Sink(0);

            if (heapSize != 0 && arr.Length / heapSize > 2 && arr.Length / 2 >= 4)
            {
                Array.Resize(ref arr, arr.Length / 2);
            }

            return tmp;
        }

        /// <summary>
        /// is heap is min heap
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="heapSize">the size of heap.  heapSize&lt;=arr.Length</param>
        /// <returns></returns>
        public static bool IsMinHeap(T[] arr, int heapSize)
        {
            if (heapSize <= 1)
            {
                return true;
            }

            for (int i = 0; i < heapSize; i++)
            {
                int left = 2 * i + 1;
                int right = 2 * i + 2;
                if (left >= heapSize)
                {
                    break;
                }

                if (arr[i].CompareTo(arr[left]) > 0)
                {
                    return false;
                }

                if (right >= heapSize)
                {
                    break;
                }

                if (arr[i].CompareTo(arr[right]) > 0)
                {
                    return false;
                }
            }

            return true;
        }

        public BinaryHeap<T> ConvertTo()
        {
            return new BinaryHeap<T>(this);
        } 
    }
}

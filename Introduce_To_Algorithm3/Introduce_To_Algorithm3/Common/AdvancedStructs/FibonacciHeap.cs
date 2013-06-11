using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    /// <summary>
    /// A fibonacci heap is a collection of rooted tree that are min-heap ordered
    /// we define the potential of Fibonacci heap φ(H) = t(H)+2m(H)
    /// t(H) the number of tree in the root list
    /// m(H) the number of marked nodes in H
    /// </summary>
    public class FibonacciHeap<K, V> where K : IComparable<K>, IEquatable<K>
    {
        #region member
        private FibonacciHeapNode<K, V> minRoot;
        public int Count { get; set; }


        public FibonacciHeap()
        {
            Count = 0;
            minRoot = null;
        }
        #endregion

        #region insert
        /// <summary>
        /// insert
        /// it runs at O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key, V val)
        {
            FibonacciHeapNode<K,V> node = new FibonacciHeapNode<K, V>(key,val);
            if (minRoot == null)
            {
                minRoot = node;
                node.LeftSibling = node.RightSibling = node;
            }
            else
            {
                //insert node to root list
                FibonacciHeapNode<K, V> temp = minRoot.RightSibling;
                node.RightSibling = temp;
                minRoot.RightSibling = node;
                node.LeftSibling = minRoot;
                temp.LeftSibling = node;
                if (node.Key.CompareTo(minRoot.Key) < 0)
                {
                    minRoot = node;
                }
            }
            Count++;
        }
        #endregion

        #region peek
        /// <summary>
        /// peek the min one
        /// it runs at O(1)
        /// </summary>
        /// <returns></returns>
        public Tuple<K, V> Peek()
        {
            if (Count <= 0)
            {
                throw new Exception("New Element in Fibonacci Heap");
            }

            return new Tuple<K, V>(minRoot.Key,minRoot.Value);
        }

        #endregion

        #region union

        /// <summary>
        /// union
        /// It runs at O(1)
        /// </summary>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <returns></returns>
        public static FibonacciHeap<K, V> Union(FibonacciHeap<K, V> h1, FibonacciHeap<K, V> h2)
        {
            if (h1 == null || h1.minRoot == null) return h2;
            if (h2 == null || h2.minRoot == null) return h1;

            FibonacciHeap<K,V> heap = new FibonacciHeap<K, V>();
            heap.minRoot = h1.minRoot;
            if (h1.minRoot.Key.CompareTo(h2.minRoot.Key) > 0)
            {
                heap.minRoot = h2.minRoot;
            }

            //concatenate the root list of H1 and H2
            FibonacciHeapNode<K, V> h1Node = h1.minRoot.RightSibling;
            FibonacciHeapNode<K, V> h2Node = h2.minRoot.RightSibling;
            h1.minRoot.RightSibling = h2.minRoot;
            h2.minRoot.LeftSibling = h1.minRoot;
            h2Node.RightSibling = h1Node;
            h1Node.LeftSibling = h2Node;

            heap.Count = h1.Count + h2.Count;
            return heap;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    /// <summary>
    ///     A fibonacci heap is a collection of rooted tree that are min-heap ordered
    ///     we define the potential of Fibonacci heap φ(H) = t(H)+2m(H)
    ///     t(H) the number of tree in the root list
    ///     m(H) the number of marked nodes in H
    ///     D(n) represents the maximum degree of any node in n-node fibonacci heap
    ///     it's Extractmin and delete runs at O(lgn) insert  minimum union decrease runs at O(1)
    /// </summary>
    public class FibonacciHeap<K, V> where K : IComparable<K>, IEquatable<K>
    {
        #region member

        /// <summary>
        ///     the root of fibonacci heap & it is a min heap
        /// </summary>
        private FibonacciHeapNode<K, V> minRoot;

        /// <summary>
        ///     the number of nodes in heap
        /// </summary>
        public int Count { get; protected set; }


        public FibonacciHeap()
        {
            Count = 0;
            minRoot = null;
        }

        #endregion

        #region insert

        /// <summary>
        ///     insert
        ///     it runs at O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key, V val)
        {
            var node = new FibonacciHeapNode<K, V>(key, val);
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
        ///     peek the min one
        ///     it runs at O(1)
        /// </summary>
        /// <returns></returns>
        public Tuple<K, V> Peek()
        {
            if (Count <= 0 || minRoot == null)
            {
                throw new Exception("New Element in Fibonacci Heap");
            }

            return new Tuple<K, V>(minRoot.Key, minRoot.Value);
        }

        #endregion

        #region getroot

        /// <summary>
        ///     get root
        /// </summary>
        /// <returns></returns>
        public FibonacciHeapNode<K, V> GetRoot()
        {
            return minRoot;
        }

        #endregion

        #region union

        /// <summary>
        ///     union
        ///     It runs at O(1)
        /// </summary>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <returns></returns>
        public static FibonacciHeap<K, V> Union(FibonacciHeap<K, V> h1, FibonacciHeap<K, V> h2)
        {
            if (h1 == null || h1.minRoot == null) return h2;
            if (h2 == null || h2.minRoot == null) return h1;

            var heap = new FibonacciHeap<K, V>();
            heap.minRoot = h1.minRoot;
            if (h1.minRoot.Key.CompareTo(h2.minRoot.Key) > 0)
            {
                heap.minRoot = h2.minRoot;
            }

            //concatenate the root list of H1 and H2
            FibonacciHeapNode<K, V> h1Node = h1.minRoot.RightSibling;
            FibonacciHeapNode<K, V> h2Node = h2.minRoot.RightSibling;
            h1.minRoot.RightSibling = h2Node;
            h2Node.LeftSibling = h1.minRoot;
            h2.minRoot.RightSibling = h1Node;
            h1Node.LeftSibling = h2.minRoot;

            heap.Count = h1.Count + h2.Count;
            return heap;
        }

        /// <summary>
        ///     union
        /// </summary>
        /// <param name="heap"></param>
        public void Union(FibonacciHeap<K, V> heap)
        {
            FibonacciHeap<K, V> h = Union(this, heap);
            Count = h.Count;
            minRoot = h.minRoot;
        }

        #endregion

        #region Extract min

        /// <summary>
        ///     extractMin it runs at O(lgn)
        ///     it's where the delayed work of consolidating trees finally occurs.
        /// </summary>
        /// <returns></returns>
        public FibonacciHeapNode<K, V> ExtractMin()
        {
            if (minRoot == null || Count <= 0)
            {
                throw new Exception("Heap is empty");
            }

            //for each child in minRoot add it in the root list
            List<FibonacciHeapNode<K, V>> list = minRoot.GetChildren();

            minRoot.Child = null;
            minRoot.Degree = 0;
            foreach (var node in list)
            {
                node.Parent = null;
                //insert node to root list
                FibonacciHeapNode<K, V> right = minRoot.RightSibling;
                node.RightSibling = right;
                right.LeftSibling = node;
                minRoot.RightSibling = node;
                node.LeftSibling = minRoot;
            }

            FibonacciHeapNode<K, V> temp = minRoot;
            FibonacciHeapNode<K, V> tempR = minRoot.RightSibling;
            if (temp == tempR)
            {
                //no element in heap
                minRoot = null;
                Count = 0;
                return temp;
            }
            //remove minroot
            FibonacciHeapNode<K, V> tempL = minRoot.LeftSibling;
            tempL.RightSibling = tempR;
            tempR.LeftSibling = tempL;
            //minRoot may not the min one here
            minRoot = tempR;
            Consolidate();
            Count--;
            return temp;
        }

        /// <summary>
        /// </summary>
        private void Consolidate()
        {
            if (minRoot == null) return;

            //if arr[i] = y, then y is currently a root with y.degree = i
            var arr = new FibonacciHeapNode<K, V>[MaxDegree() + 1];
            FibonacciHeapNode<K, V> temp = minRoot;
            var rootList = new List<FibonacciHeapNode<K, V>>();
            rootList.Add(temp);
            while (true)
            {
                temp = temp.RightSibling;
                if (temp == minRoot)
                {
                    break;
                }
            }

            foreach (var fnode in rootList)
            {
                FibonacciHeapNode<K, V> x = fnode;
                int d = fnode.Degree;
                while (arr[d] != null)
                {
                    FibonacciHeapNode<K, V> y = arr[d];
                    if (x.Key.CompareTo(y.Key) > 0)
                    {
                        Exchange(x, y);
                    }

                    HeapLink(y, x);
                    arr[d] = null;
                    d++;
                }
                arr[d] = x;
            }

            minRoot = null;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != null)
                {
                    if (minRoot == null)
                    {
                        minRoot = arr[i];
                        minRoot.LeftSibling = minRoot.RightSibling = minRoot;
                    }
                    else
                    {
                        FibonacciHeapNode<K, V> minR = minRoot;
                        minRoot.RightSibling = arr[i];
                        arr[i].LeftSibling = minRoot;
                        arr[i].RightSibling = minR;
                        minR.LeftSibling = arr[i];
                        if (minRoot.Key.CompareTo(arr[i].Key) > 0)
                        {
                            minRoot = arr[i];
                        }
                    }
                }
            }
        }

        private void HeapLink(FibonacciHeapNode<K, V> y, FibonacciHeapNode<K, V> x)
        {
            FibonacciHeapNode<K, V> temp = y;
            FibonacciHeapNode<K, V> yL = y.LeftSibling, yR = y.RightSibling;
            yL.RightSibling = yR;
            yR.LeftSibling = yL;
            y.Parent = x;
            x.Degree++;
            if (x.Child == null)
            {
                x.Child = y;
                y.LeftSibling = y.RightSibling = y;
            }
            else
            {
                FibonacciHeapNode<K, V> t = x.Child.RightSibling;
                x.Child.RightSibling = y;
                y.LeftSibling = x.Child;
                y.RightSibling = t;
                t.LeftSibling = y;
            }
            y.Mark = false;
        }

        private void Exchange(FibonacciHeapNode<K, V> x, FibonacciHeapNode<K, V> y)
        {
            FibonacciHeapNode<K, V> temp = y;
            y = x;
            x = temp;
        }


        /// <summary>
        ///     get the maximum degree of all nodes in fibonacci Heap
        /// </summary>
        /// <returns></returns>
        public int MaxDegree()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region decrease

        public void DecreaseKey(FibonacciHeapNode<K, V> x, K key)
        {
            if (key.CompareTo(x.Key) > 0)
            {
                throw new Exception("new key is greater than current key");
            }

            if (key.CompareTo(x.Key) == 0)
            {
                return;
            }


            x.Key = key;
            FibonacciHeapNode<K, V> y = x.Parent;
            if (y != null && x.Key.CompareTo(y.Key) < 0)
            {
                Cut(x, y);
                CascadingCut(y);
            }

            if (x.Key.CompareTo(minRoot.Key) < 0)
            {
                minRoot = x;
            }
        }

        private void CascadingCut(FibonacciHeapNode<K, V> node)
        {
            FibonacciHeapNode<K, V> z = node.Parent;
            if (z != null)
            {
                if (node.Mark == false)
                {
                    node.Mark = true;
                }
                else
                {
                    Cut(node, z);
                    CascadingCut(z);
                }
            }
        }

        private void Cut(FibonacciHeapNode<K, V> x, FibonacciHeapNode<K, V> y)
        {
            y.Degree--;

            if (x.LeftSibling == x)
            {
                y.Child = null;
            }
            else
            {
                x.LeftSibling.RightSibling = x.RightSibling;
                x.RightSibling.LeftSibling = x.LeftSibling;
            }

            // add x to the root list
            FibonacciHeapNode<K, V> t = minRoot.RightSibling;
            minRoot.RightSibling = x;
            x.LeftSibling = minRoot;
            x.RightSibling = t;
            t.LeftSibling = x;
            x.Parent = null;
            x.Mark = false;
        }

        #endregion
    }
}

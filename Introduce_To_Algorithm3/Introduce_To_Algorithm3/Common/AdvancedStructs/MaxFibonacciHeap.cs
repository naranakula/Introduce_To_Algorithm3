using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    /// <summary>
    ///  A max fibonacci heap is a collection of rooted tree that are max-heap ordered
    ///     we define the potential of Fibonacci heap φ(H) = t(H)+2m(H)
    ///     t(H) the number of tree in the root list
    ///     m(H) the number of marked nodes in H
    ///     D(n) represents the maximum degree of any node in n-node fibonacci heap
    ///     it's Extractmin and delete runs at O(lgn) insert  minimum union decrease runs at O(1)
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MaxFibonacciHeap<K, V> where K : IComparable<K>, IEquatable<K>
    {
        #region Member

        /// <summary>
        /// the root of max fibonacci heap & it is a max one of the rooted list
        /// </summary>
        private FibonacciHeapNode<K, V> maxRoot;


        /// <summary>
        /// the number of nodes in heap
        /// </summary>
        public int Count { get; protected set; }


        /// <summary>
        /// constructor
        /// </summary>
        public MaxFibonacciHeap()
        {
            Count = 0;
            maxRoot = null;
        }

        #endregion

        #region Insert

        /// <summary>
        /// insert it runs at O(1). add a node to the rooted list
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key, V val)
        {
            Count++;

            var newNode = new FibonacciHeapNode<K, V>(key, val);

            if (maxRoot == null)
            {
                maxRoot = newNode;
                newNode.LeftSibling = newNode.RightSibling = newNode;
            }
            else
            {
                //insert node to the root list
                FibonacciHeapNode<K, V> temp = maxRoot.RightSibling;
                newNode.RightSibling = temp;
                maxRoot.RightSibling = newNode;
                newNode.LeftSibling = maxRoot;
                temp.LeftSibling = newNode;
                if (newNode.Key.CompareTo(maxRoot.Key) > 0)
                {
                    //change the root to biggest one
                    maxRoot = newNode;
                }
            }
        }


        #endregion 

        #region Peek

        /// <summary>
        ///     peek the min one
        ///     it runs at O(1)
        /// </summary>
        /// <returns></returns>
        public Tuple<K, V> Peek()
        {
            if (Count <= 0 || maxRoot == null)
            {
                throw new Exception("New Element in Fibonacci Heap");
            }

            return new Tuple<K, V>(maxRoot.Key, maxRoot.Value);
        }

        #endregion

        #region Getroot

        /// <summary>
        ///     get root
        /// </summary>
        /// <returns></returns>
        public FibonacciHeapNode<K, V> GetRoot()
        {
            return maxRoot;
        }

        #endregion

        #region Union


        /// <summary>
        ///     union
        ///     It runs at O(1)
        /// </summary>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <returns></returns>
        public static MaxFibonacciHeap<K, V> Union(MaxFibonacciHeap<K, V> h1, MaxFibonacciHeap<K, V> h2)
        {
            if (h1 == null || h1.maxRoot == null) return h2;
            if (h2 == null || h2.maxRoot == null) return h1;

            var heap = new MaxFibonacciHeap<K, V>();
            heap.maxRoot = h1.maxRoot;
            if (h1.maxRoot.Key.CompareTo(h2.maxRoot.Key) < 0)
            {
                heap.maxRoot = h2.maxRoot;
            }

            //concatenate the root list of H1 and H2
            FibonacciHeapNode<K, V> h1Node = h1.maxRoot.RightSibling;
            FibonacciHeapNode<K, V> h2Node = h2.maxRoot.RightSibling;
            h1.maxRoot.RightSibling = h2Node;
            h2Node.LeftSibling = h1.maxRoot;
            h2.maxRoot.RightSibling = h1Node;
            h1Node.LeftSibling = h2.maxRoot;

            heap.Count = h1.Count + h2.Count;
            return heap;
        }

        /// <summary>
        ///     union
        /// </summary>
        /// <param name="heap"></param>
        public void Union(MaxFibonacciHeap<K, V> heap)
        {
            MaxFibonacciHeap<K, V> h = Union(this, heap);
            Count = h.Count;
            maxRoot = h.maxRoot;
        }


        #endregion

        #region Extract max

        /// <summary>
        /// extractMax it runs at O(D(n)) amortized time. D(n) is maximum degree which O(lgn)
        /// the max one are removed from heap
        /// it's where the delayed work of consolidating trees finally occurs
        /// </summary>
        /// <returns></returns>
        public FibonacciHeapNode<K, V> ExtractMax()
        {
            if (maxRoot == null || Count <= 0)
            {
                throw new Exception("Heap is empty");
            }

            //for each child in maxroot add it in the root list
            List<FibonacciHeapNode<K, V>> list = maxRoot.GetChildren();

            maxRoot.Child = null;
            maxRoot.Degree = 0;
            foreach (var node in list)
            {
                node.Parent = null;
                //insert node to root list
                FibonacciHeapNode<K, V> right = maxRoot.RightSibling;
                node.RightSibling = right;
                right.LeftSibling = node;
                maxRoot.RightSibling = node;
                node.LeftSibling = maxRoot;
            }

            FibonacciHeapNode<K, V> temp = maxRoot;
            FibonacciHeapNode<K, V> tempR = maxRoot.RightSibling;
            if (temp == tempR)
            {
                //no element in heap
                maxRoot = null;
                Count = 0;
                return temp;
            }

            //remove maxroot
            FibonacciHeapNode<K, V> tempL = maxRoot.LeftSibling;
            tempL.RightSibling = tempR;
            tempR.LeftSibling = tempL;
            //maxRoot may not the max one here
            maxRoot = tempR;
            Consolidate();
            Count--;
            return temp;
        }

        /// <summary>
        /// consolidate the heap until every node in the root list have different degree value
        /// 1)if find two roots x and y in the root list with the same degree, without loss of generality, let x.key  &gt;= y.key
        /// 2)remove y from the root list, and make y a child of x by calling the HeapLink procedure. the procedure increments the attribute x.degree and clear the mark on y
        /// </summary>
        private void Consolidate()
        {
            if (maxRoot == null)
            {
                //actually when it is called, maxRoot can't be null & maxRoot may not actually the max one
                return;
            }

            //if arr[i] = y, then y is currently a root with y.degree = i
            var arr = new FibonacciHeapNode<K, V>[MaxDegree() + 1];
            FibonacciHeapNode<K, V> temp = maxRoot;
            var rootList = new List<FibonacciHeapNode<K, V>>();

            while (true)
            {
                rootList.Add(temp);
                temp = temp.RightSibling;
                if (temp == maxRoot)
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
                    //y must appear before x in the rootList  & x and y have same degree
                    FibonacciHeapNode<K, V> y = arr[d];
                    if (x.Key.CompareTo(y.Key) < 0)
                    {
                        Exchange(x, y);
                    }
                    //now x is bigger
                    HeapLink(y, x);
                    arr[d] = null;
                    d++;
                }
                arr[d] = x;
            }

            maxRoot = null;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != null)
                {
                    if (maxRoot == null)
                    {
                        maxRoot = arr[i];
                        maxRoot.LeftSibling = maxRoot.RightSibling = maxRoot;
                    }
                    else
                    {
                        FibonacciHeapNode<K, V> maxR = maxRoot.RightSibling;
                        maxRoot.RightSibling = arr[i];
                        arr[i].LeftSibling = maxRoot;
                        arr[i].RightSibling = maxR;
                        maxR.LeftSibling = arr[i];
                        if (maxRoot.Key.CompareTo(arr[i].Key) < 0)
                        {
                            maxRoot = arr[i];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// x.key&gt;=y.key  make y a child of x
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
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
        /// get the maximum degree of all nodes in fibonacci Heap
        /// this isn't currently the maximum degree. it's the maximum degree will in the root list.
        /// </summary>
        /// <returns></returns>
        public int MaxDegree()
        {
            return (int)(System.Math.Log(Count, (System.Math.Sqrt(5.0) + 1) / 2.0)) + 2;
        }

        #endregion

        #region IsEmpty

        public bool IsEmpty
        {
            get { return maxRoot == null || Count <= 0; }
        }

        #endregion

    }
}

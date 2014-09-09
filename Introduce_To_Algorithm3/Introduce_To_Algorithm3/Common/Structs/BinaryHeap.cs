using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class BinaryHeap<T> where T:IComparable<T>
    {
        /// <summary>
        /// the root 
        /// </summary>
        public BinaryHeapNode<T> Root { get; protected set; }

        /// <summary>
        /// how many element current in heap
        /// </summary>
        public int Size { get; protected set; }

        /// <summary>
        /// is it maxheap or minheap
        /// </summary>
        public HeapType HeapType { get; protected set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="type"></param>
        public BinaryHeap(T[] arr,HeapType type):this(arr,arr.Length,type)
        {
            
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="heapSize">heapsize must not bigger than arr.Length</param>
        /// <param name="type"></param>
        public BinaryHeap(T[] arr, int heapSize,HeapType type)
        {
            Debug.Assert(heapSize<=arr.Length);

            HeapType = type;

            Root = new BinaryHeapNode<T>(arr[0]);
            BuildHeap(arr,Root,0,heapSize);
        }


        public BinaryHeap(MaxHeap<T> maxHeap):this(maxHeap.GetData(),maxHeap.Length,HeapType.MaxHeap)
        {
            
        }

        public BinaryHeap(MinHeap<T> minHeap) : this(minHeap.GetData(),minHeap.Length,HeapType.MinHeap)
        {
            
        } 

        /// <summary>
        /// constructor heap
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="node"></param>
        /// <param name="i">the index of node in arr</param>
        /// <param name="heapSize"></param>
        private void BuildHeap(T[] arr, BinaryHeapNode<T> node, int i,int heapSize)
        {
            int left = 2*i + 1;
            int right = 2*i + 2;
            if (left >= heapSize)
            {
                return;
            }

            BinaryHeapNode<T> leftNode = new BinaryHeapNode<T>(arr[left],node);
            node.Left = leftNode;
            BuildHeap(arr,leftNode,left,heapSize);

            if (right >= heapSize)
            {
                return;
            }

            BinaryHeapNode<T> rightNode = new BinaryHeapNode<T>(arr[right],node);
            node.Right = rightNode;
            BuildHeap(arr,rightNode,right,heapSize);
        }

    }


    public enum HeapType
    {
        MaxHeap,
        MinHeap
    }

    /// <summary>
    /// binary heap node 
    /// it &lt;T@gt; can have satellite data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryHeapNode<T> where T : IComparable<T>
    {
        #region Member

        public T Key { get; protected set; }

        /// <summary>
        /// parent node
        /// </summary>
        public BinaryHeapNode<T> Parent { get; set; }

        /// <summary>
        /// left child node
        /// </summary>
        public BinaryHeapNode<T> Left { get; set; }

        /// <summary>
        /// right child node
        /// </summary>
        public BinaryHeapNode<T> Right { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        public BinaryHeapNode()
        {
            
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="key"></param>
        public BinaryHeapNode(T key)
        {
            this.Key = key;
        }


        public BinaryHeapNode(T key, BinaryHeapNode<T> parent)
        {
            this.Key = key;
            this.Parent = parent;
        } 
        #endregion


        #region Method

        /// <summary>
        /// is the root of binary heap
        /// </summary>
        /// <returns></returns>
        public bool IsRoot()
        {
            return Parent == null;
        }

        /// <summary>
        /// is it a leaf
        /// </summary>
        /// <returns></returns>
        public bool IsLeaf()
        {
            return Left == null && Right == null;
        }
        #endregion
    }

}

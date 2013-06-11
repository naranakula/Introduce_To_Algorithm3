using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    public class FibonacciHeapNode<K, V> where K : IComparable<K>, IEquatable<K>
    {
        public K Key;
        public V Value;
        public FibonacciHeapNode<K, V> Child;
        public FibonacciHeapNode<K, V> LeftSibling;
        public FibonacciHeapNode<K, V> RightSibling;
        public FibonacciHeapNode<K, V> Parent;


        public FibonacciHeapNode()
        {
        }

        public FibonacciHeapNode(K key, V val)
        {
            Key = key;
            Value = val;
        }

        /// <summary>
        /// the number of children in the child list of this node
        /// </summary>
        public int Degree;
        /// <summary>
        /// indicate whether node has lost a child since the last time it was made the child of another node.Newly created nodes are unmarked, and a node x becomes unmarked whenever it
        /// is made the child of another node.
        /// </summary>
        public bool Mark;

        public List<FibonacciHeapNode<K, V>> GetChildren()
        {
            List<FibonacciHeapNode<K,V>> list = new List<FibonacciHeapNode<K, V>>();
            if (Child == null) return list;

            FibonacciHeapNode<K, V> temp = Child;
            while (true)
            {
                list.Add(temp);
                temp = temp.RightSibling;
                if (temp == Child)
                {
                    break;
                }
            }
            return list;
        }
    }
}

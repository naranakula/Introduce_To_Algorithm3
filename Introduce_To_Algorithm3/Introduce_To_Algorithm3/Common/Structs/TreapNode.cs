using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class TreapNode<K, V> where K : IComparable<K>, IEquatable<K>
    {
        public K Key;
        public V Value;

        public TreapNode()
        {
            Key = default(K);
            Value = default(V);
        }

        public TreapNode(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public TreapNode(K key, V val, double priority)
        {
            Key = key;
            Value = val;
            Priority = priority;
        }

        public TreapNode<K, V> Parent;
        public TreapNode<K, V> Left;
        public TreapNode<K, V> Right;
        /// <summary>
        /// the priority is a random number which used to construct a heap
        /// </summary>
        public double Priority;
    }
}

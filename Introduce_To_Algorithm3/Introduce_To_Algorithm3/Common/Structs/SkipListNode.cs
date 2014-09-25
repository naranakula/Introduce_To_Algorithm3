using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// the skip list node
    /// </summary>
    public class SkipListNode<K, V> : IComparable<SkipListNode<K, V>>, IEquatable<SkipListNode<K, V>> where K:IComparable<K>,IEquatable<K>
    {
        public K Key;
        public V Value;

        public SkipListNode()
        {
        }

        public SkipListNode(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public SkipListNode<K, V> Prev;
        public SkipListNode<K, V> Next;
        public SkipListNode<K, V> Up;
        public SkipListNode<K, V> Down;

        /// <summary>
        /// a label marks it is max
        /// </summary>
        public bool IsMaxNode { get; set; }

        /// <summary>
        /// a label marks it is min
        /// </summary>
        public bool IsMinNode { get; set; }

        /// <summary>
        /// if &gt; other, return 1; &lt; other return -1; == other ,return 0 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(SkipListNode<K, V> other)
        {
            if (IsMaxNode && other.IsMaxNode)
            {
                throw new Exception("can't be both max");
            }

            if (IsMinNode && other.IsMinNode)
            {
                throw new Exception("can't be both min");
            }

            if (IsMaxNode)
            {
                return 1;
            }

            if (IsMinNode)
            {
                return -1;
            }

            return this.Key.CompareTo(other.Key);
        }

        /// <summary>
        /// is equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SkipListNode<K, V> other)
        {
            return CompareTo(other) == 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class SplayNode<K, V> where K : IComparable<K>, IEquatable<K>
    {
        public K Key;
        public V Value;

        public SplayNode()
        {
            Key = default(K);
            Value = default(V);
        }

        public SplayNode(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public SplayNode<K, V> Parent;
        public SplayNode<K, V> Left;
        public SplayNode<K, V> Right;
    }
}

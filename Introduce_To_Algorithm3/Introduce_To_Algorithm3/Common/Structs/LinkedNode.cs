using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// linked node
    /// </summary>
    /// <typeparam name="K">the type of key</typeparam>
    /// <typeparam name="V">the type of value</typeparam>
    public class LinkedNode<K, V> where K : IComparable<K>, IEquatable<K>
    {
        public K Key;
        public V Value;

        public LinkedNode()
        {

        }

        public LinkedNode(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public LinkedNode<K, V> Parent;
        public LinkedNode<K, V> Left;
        public LinkedNode<K, V> Right;
    }


    /// <summary>
    /// linked node
    /// </summary>
    /// <typeparam name="K">the type of key</typeparam>
    public class LinkedNode<K> where K : IComparable<K>, IEquatable<K>
    {
        public K Key;

        public LinkedNode()
        {

        }

        public LinkedNode(K key)
        {
            Key = key;
        }

        public LinkedNode<K> Parent;
        public LinkedNode<K> Left;
        public LinkedNode<K> Right;
    }
}

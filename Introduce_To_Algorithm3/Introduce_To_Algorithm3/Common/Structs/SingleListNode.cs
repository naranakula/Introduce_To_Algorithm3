using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class SingleListNode<T> where T : IComparable<T>
    {
        public T Key;

        public SingleListNode()
        {

        }

        public SingleListNode(T key)
        {
            Key = key;
        }

        public SingleListNode<T> Next;
    }


    /// <summary>
    /// linked node
    /// </summary>
    /// <typeparam name="K">the type of key</typeparam>
    /// <typeparam name="V">the type of value</typeparam>
    public class SingleListNode<K, V> where K : IComparable<K>
    {
        public K Key;
        public V Value;

        public SingleListNode()
        {

        }

        public SingleListNode(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public SingleListNode<K, V> Next;
    }
}

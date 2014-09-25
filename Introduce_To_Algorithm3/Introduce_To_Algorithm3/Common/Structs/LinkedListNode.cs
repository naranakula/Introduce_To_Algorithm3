using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class LinkedListNode<T>where T:IComparable<T>,IEquatable<T>
    {
        public T Key;

        public LinkedListNode()
        {

        }

        public LinkedListNode(T key)
        {
            Key = key;
        }

        public LinkedListNode<T> Prev;
        public LinkedListNode<T> Next;
    }


    /// <summary>
    /// linked node
    /// </summary>
    /// <typeparam name="K">the type of key</typeparam>
    /// <typeparam name="V">the type of value</typeparam>
    public class LinkedListNode<K, V> where K : IComparable<K>,IEquatable<K>
    {
        public K Key;
        public V Value;

        public LinkedListNode()
        {

        }

        public LinkedListNode(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public LinkedListNode<K, V> Prev;
        public LinkedListNode<K, V> Next;
    }


}

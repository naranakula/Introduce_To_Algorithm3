using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public enum Color
    {
        BLACK,
        RED
    }

    /// <summary>
    /// Red Black node
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class RBTreeNode<K, V> where K : IComparable<K>, IEquatable<K>
    {
        public Color Color;
        public RBTreeNode()
        {
        }

        public RBTreeNode(K key, V val)
        {
            this.Key = key;
            this.Value = val;
        }

        public RBTreeNode(K key, V val, Color color)
        {
            this.Color = color;
            this.Key = key;
            this.Value = val;
        }

        public K Key;
        public V Value;

        public RBTreeNode<K, V> Parent;
        public RBTreeNode<K, V> Left;
        public RBTreeNode<K, V> Right;
        /// <summary>
        /// number of his children and himself
        /// </summary>
        public int Size;
    }

    /// <summary>
    /// red black node
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public class RBTreeNode<K> where K : IComparable<K>, IEquatable<K>
    {
        public Color Color;
        public RBTreeNode()
        {
        }

        public RBTreeNode(K key)
        {
            this.Key = key;
        }

        public RBTreeNode(K key, Color color)
        {
            this.Color = color;
            this.Key = key;
        }


        public K Key;

        public RBTreeNode<K> Parent;
        public RBTreeNode<K> Left;
        public RBTreeNode<K> Right;
        /// <summary>
        /// number of his children and himself
        /// </summary>
        public int Size;
    }
}

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
    public class RBTreeNode<K, V> : TreeNode<K, V> where K : IComparable<K>, IEquatable<K>
    {
        public Color Color;
        public RBTreeNode()
        {
        }

        public RBTreeNode(K key, V val)
            : base(key, val)
        {

        }

        public RBTreeNode(K key, V val, Color color)
            : base(key, val)
        {
            this.Color = color;
        }
    }

    /// <summary>
    /// red black node
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public class RBTreeNode<K> : TreeNode<K> where K : IComparable<K>, IEquatable<K>
    {
        public Color Color;
        public RBTreeNode()
        {
        }

        public RBTreeNode(K key)
            : base(key)
        {

        }

        public RBTreeNode(K key, Color color)
            : base(key)
        {
            this.Color = color;
        }
    }
}

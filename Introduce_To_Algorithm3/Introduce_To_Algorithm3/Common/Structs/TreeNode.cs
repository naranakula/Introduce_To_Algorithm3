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
    public class TreeNode<K, V> where K : IComparable<K>, IEquatable<K>
    {
        public K Key;
        public V Value;

        public TreeNode()
        {

        }

        public TreeNode(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public TreeNode<K, V> Parent;
        public TreeNode<K, V> Left;
        public TreeNode<K, V> Right;
        /// <summary>
        /// number of his children and himself
        /// </summary>
        public int Size;
    }


    /// <summary>
    /// linked node
    /// </summary>
    /// <typeparam name="K">the type of key</typeparam>
    public class TreeNode<K> where K : IComparable<K>, IEquatable<K>
    {
        public K Key;

        public TreeNode()
        {

        }

        public TreeNode(K key)
        {
            Key = key;
        }

        public TreeNode<K> Parent;
        public TreeNode<K> Left;
        public TreeNode<K> Right;
        /// <summary>
        /// number of his children and himself
        /// </summary>
        public int Size;
    }
}

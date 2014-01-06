using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// AVL tree node
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class AVLNode<K, V> where K : IComparable<K>, IEquatable<K>
    {
        public K Key;
        public V Value;
        /// <summary>
        /// the height of this node,
        /// if a node has no children, mark its height = 1
        /// </summary>
        public int H;

        /// <summary>
        ///  the balance factor of this node = the height of left trree - the height of right tree
        /// </summary>
        public int BF
        {
            get
            {
                int leftHeight = Left == null ? 0 : Left.H;
                int rightHeight = Right == null ? 0 : Right.H;
                return leftHeight - rightHeight;
            }
        }

        public AVLNode()
        {
            Key = default(K);
            Value = default(V);
        }

        public AVLNode(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public AVLNode<K, V> Parent;
        public AVLNode<K, V> Left;
        public AVLNode<K, V> Right;
    }




    /// <summary>
    /// AVL tree node
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public class AVLNode<K> where K : IComparable<K>, IEquatable<K>
    {
        public K Key;
        /// <summary>
        /// the height of this node
        /// </summary>
        public int H;
        public AVLNode()
        {

        }

        public AVLNode(K key)
        {
            Key = key;
        }

        public AVLNode<K> Parent;
        public AVLNode<K> Left;
        public AVLNode<K> Right;
    }



}

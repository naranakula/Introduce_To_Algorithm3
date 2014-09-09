using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// sometimes we don't know how many children a node can have.
    /// the left-child, right-sibling representation can solve this problem.
    /// x.left-child point to the leftmost child of node x
    /// x.right-sibling points the siblings of x immediately to its right.
    /// if node x has no children, then x.left-child = null
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class UnBoundedTreeNode<K, V> where K : IComparable<K>, IEquatable<K>
    {
        public K Key;
        public V Value;

        public UnBoundedTreeNode()
        {
            Key = default(K);
            Value = default(V);
        }

        public UnBoundedTreeNode(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public UnBoundedTreeNode<K, V> Parent;
        public UnBoundedTreeNode<K, V> LeftChild;
        public UnBoundedTreeNode<K, V> RightSibling;
    }
}

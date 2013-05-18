using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// AVL tree is a binary search tree that is height balanced: for each node x, the heights of the left and right subtrees of x differ by at most 1.
    /// Balance factor: the height of left child minus the height of right tree     avl only has bf : 0 -1 1
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class AVL<K, V> where K : IComparable<K>, IEquatable<K>
    {
        /// <summary>
        /// the root of avl tree
        /// </summary>
        private AVLNode<K, V> root;









    }
}

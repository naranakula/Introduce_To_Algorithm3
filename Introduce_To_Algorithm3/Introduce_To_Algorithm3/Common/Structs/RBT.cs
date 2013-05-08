using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// Red black tree:
    /// 1)every node is red or black
    /// 2)the root is black
    /// 3)each leaf is black
    /// 4)if a node is  red, then both its children are black
    /// 5)For each node, all simple paths from the node to descendant leaves contain the same number of black nodes.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"> </typeparam>
    public class RBT<K,V>:BST<K,V> where K : IComparable<K>, IEquatable<K>
    {
        /// <summary>
        /// Red Black insert
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public override void Insert(K key,V val)
        {
        }
    }
}

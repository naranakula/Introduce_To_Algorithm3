using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    /// <summary>
    /// 1)every non root non leaf node has [ceil(M/2),M] child
    /// 2)root has [2,M] child
    /// 3)every leaf has same height
    /// 4)every non root node has [ceil(M/2)-1,M-1] keys
    /// we call t=Ceil(M/2) as the minimum degree of the B- tree.
    /// m-order B- tree m>=3
    /// it normally used for db
    /// all keys are different
    /// </summary>
    public class BMinusTree<K, V> where K : IComparable<K>, IEquatable<K>
    {
        #region members
        /// <summary>
        /// the minimum degree of B- tree
        /// </summary>
        private int minDegree;

        /// <summary>
        /// the number of keys is between [1,2t-1]
        /// </summary>
        private BMinusTreeNode<K, V> root;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minDegree">the minimum degree of B- tree. it &gt;= 2, which means it the min child can a interval node has</param>
        public BMinusTree(int minDegree)
        {
            this.minDegree = minDegree;
        }


        public BMinusTreeNode<K, V> GetRoot()
        {
            return root;
        }
        #endregion


        #region Search

        /// <summary>
        /// search key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>if found, return the node and the index of key in the node. null, if not found</returns>
        public Tuple<BMinusTreeNode<K, V>, int> Search(K key)
        {
            return Search(root, key);
        }

        /// <summary>
        /// search key
        /// </summary>
        /// <param name="root"></param>
        /// <param name="key"></param>
        /// <returns>if found, return the node and the index of key in the node. null, if not found</returns>
        public Tuple<BMinusTreeNode<K, V>, int> Search(BMinusTreeNode<K, V> root, K key)
        {
            if (root == null || root.N <= 0)
            {
                return null;
            }

            int i = 0;
            while (i < root.N && key.CompareTo(root.KeyValues[i].Item1) > 0)
            {
                i++;
            }

            if (i < root.N && key.CompareTo(root.KeyValues[i].Item1) == 0)
            {
                return new Tuple<BMinusTreeNode<K, V>, int>(root, i);
            }
            else if (root.IsLeaf)
            {
                return null;
            }
            else
            {
                return Search(root.Children[i], key);
            }
        }


        /// <summary>
        /// search key
        /// </summary>
        /// <param name="key">if found, return the node and the index of key in the node. null, if not found</param>
        /// <returns></returns>
        public Tuple<BMinusTreeNode<K, V>, int> BinarySearch(K key)
        {
            return BinarySearch(root, key);
        }


        public Tuple<BMinusTreeNode<K, V>, int> BinarySearch(BMinusTreeNode<K, V> root, K key)
        {
            if (root == null || root.N <= 0)
            {
                return null;
            }

            int i = BinarySearch(root, key, 0, root.KeyValues.Length - 1);
            if (i < root.N && key.CompareTo(root.KeyValues[i].Item1) == 0)
            {
                return new Tuple<BMinusTreeNode<K, V>, int>(root, i);
            }
            else if (root.IsLeaf)
            {
                return null;
            }
            else
            {
                return Search(root.Children[i], key);
            }
        }


        public int BinarySearch(BMinusTreeNode<K, V> node, K key, int begin, int end)
        {
            if (end == begin)
            {
                return key.CompareTo(node.KeyValues[begin].Item1) <= 0 ? begin : begin + 1;
            }
            int mid = (begin + end) / 2;
            if (key.CompareTo(node.KeyValues[mid].Item1) <= 0)
            {
                return BinarySearch(node, key, begin, mid);
            }
            else
            {
                return BinarySearch(node, key, mid + 1, end);
            }
        }
        #endregion

        #region


        private void Create()
        {
            if (root == null)
                root = new BMinusTreeNode<K, V> { IsLeaf = true, N = 0 };
        }

        #endregion
    }
}

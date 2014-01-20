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
    public class BTree<K, V> where K : IComparable<K>, IEquatable<K>
    {
        #region members
        /// <summary>
        /// the minimum degree of B- tree
        /// </summary>
        private int minDegree;

        /// <summary>
        /// the number of keys is between [1,2t-1]
        /// </summary>
        private BTreeNode<K, V> root;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minDegree">the minimum degree of B- tree. it &gt;= 2, which means it the min child can a interval node has</param>
        public BTree(int minDegree)
        {
            Trace.Assert(minDegree >= 2);
            this.minDegree = minDegree;
        }


        public BTreeNode<K, V> GetRoot()
        {
            return root;
        }

        /// <summary>
        /// get the mindegree
        /// </summary>
        public int MinDegree
        {
            get { return minDegree; }
        }
        /// <summary>
        /// get the order of a tree
        /// </summary>
        public int Order
        {
            get { return 2 * minDegree; }
        }

        public int Heigth { get; set; }
        public int Count { get; set; }


        public bool IsEmpty
        {
            get { return Count == 0; }
        }
        #endregion

        #region Search

        /// <summary>
        /// search key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>if found, return the node and the index of key in the node. null, if not found</returns>
        public Tuple<BTreeNode<K, V>, int> Search(K key)
        {
            return Search(root, key);
        }

        /// <summary>
        /// search key
        /// </summary>
        /// <param name="root"></param>
        /// <param name="key"></param>
        /// <returns>if found, return the node and the index of key in the node. null, if not found</returns>
        public Tuple<BTreeNode<K, V>, int> Search(BTreeNode<K, V> root, K key)
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
                return new Tuple<BTreeNode<K, V>, int>(root, i);
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
        public Tuple<BTreeNode<K, V>, int> BinarySearch(K key)
        {
            return BinarySearch(root, key);
        }


        public Tuple<BTreeNode<K, V>, int> BinarySearch(BTreeNode<K, V> root, K key)
        {
            if (root == null || root.N <= 0)
            {
                return null;
            }

            int i = BinarySearch(root, key, 0, root.N - 1);
            if (i < root.N && key.CompareTo(root.KeyValues[i].Item1) == 0)
            {
                return new Tuple<BTreeNode<K, V>, int>(root, i);
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


        public int BinarySearch(BTreeNode<K, V> node, K key, int begin, int end)
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

        #region insert

        /// <summary>
        /// we assure that node.Children[i] is full, but node isn't full
        /// </summary>
        /// <param name="node"></param>
        /// <param name="i"></param>
        private void SplitChild(BTreeNode<K, V> node, int i)
        {
            BTreeNode<K, V> y = node.Children[i];
            BTreeNode<K, V> z = new BTreeNode<K, V>(minDegree);
            z.IsLeaf = y.IsLeaf;
            z.N = minDegree - 1;
            z.Parent = node;
            for (int j = 0; j < minDegree - 1; j++)
            {
                z.KeyValues[j] = y.KeyValues[j + minDegree];
                //for accuracy and gc
                y.KeyValues[j + minDegree] = null;
            }
            if (!y.IsLeaf)
            {
                //copy child point
                for (int j = 0; j < minDegree; j++)
                {
                    z.Children[j] = y.Children[j + minDegree];
                    y.Children[j + minDegree] = null;
                }
            }
            y.N = minDegree - 1;
            for (int j = node.N; j > i; j--)
            {
                node.Children[j + 1] = node.Children[j];
            }
            node.Children[i + 1] = z;
            for (int j = node.N - 1; j > i; j--)
            {
                node.KeyValues[j + 1] = node.KeyValues[j];
            }

            node.KeyValues[i] = y.KeyValues[minDegree - 1];
            y.KeyValues[minDegree - 1] = null;
            node.N++;
        }

        /// <summary>
        /// if it already contains in tree, return true and update the value. if not , return false.
        /// 
        /// it runs at tLgt(N)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool Insert(K key, V val)
        {
            var v = BinarySearch(root, key);
            if (v != null)
            {
                v.Item1.KeyValues[v.Item2] = new Tuple<K, V>(key, val);
                return true;
            }

            Count++;

            if (root == null)
            {
                root = new BTreeNode<K, V>(minDegree);
                root.IsLeaf = true;
                root.N = 1;
                Heigth = 1;
                root.KeyValues[0] = new Tuple<K, V>(key, val);
                return false;
            }

            if (root.N == 2 * minDegree - 1)
            {
                BTreeNode<K, V> node = new BTreeNode<K, V>(minDegree);
                node.Children[0] = root;
                root.Parent = node;
                root = node;
                Heigth++;
                SplitChild(node, 0);
                InsertNonFull(root, key, val);
                return false;
            }
            else
            {
                InsertNonFull(root, key, val);
                return false;
            }
        }

        private void InsertNonFull(BTreeNode<K, V> root, K key, V val)
        {
            //root.N possibly equal 0
            int i = root.N - 1;
            if (root.IsLeaf)
            {
                while (i >= 0 && key.CompareTo(root.KeyValues[i].Item1) < 0)
                {
                    root.KeyValues[i + 1] = root.KeyValues[i];
                    i--;
                }
                root.KeyValues[i + 1] = new Tuple<K, V>(key, val);
                root.N++;
            }
            else
            {
                int index = root.Search(key).Item1;
                BTreeNode<K, V> node = root.Children[index];
                if (node.N == 2 * minDegree - 1)
                {
                    SplitChild(root, index);
                    if (key.CompareTo(root.KeyValues[i].Item1) > 0)
                    {
                        index++;
                    }
                }
                InsertNonFull(root.Children[index], key, val);
            }
        }

        #endregion

        #region delete

        /// <summary>
        /// you need find the item by using search
        /// </summary>
        /// <param name="item"></param>
        public void Delete(Tuple<BTreeNode<K, V>, int> item)
        {
            if (item == null || Count <= 0 || item.Item1 == null)
            {
                Count = 0;
                return;
            }

            Count--;
            BTreeNode<K, V> node = item.Item1;
            int index = item.Item2;

            //case 0: node is leaf and root. this is only one node -- the root node just delete
            if (node.IsLeaf && node == root)
            {
                if (node.N <= 1)
                {
                    root = null;
                    Heigth = 0;
                    Count = 0;
                }
                else
                {
                    int i = index + 1;
                    for (; i < node.N; i++)
                    {
                        node.KeyValues[i - 1] = node.KeyValues[i];
                    }
                    //for gc and accuracy
                    node.KeyValues[i - 1] = null;
                    //actually no need to update child because node is leaf and all its children are null
                    for (i = index + 1; i < node.N + 1; i++)
                    {
                        node.Children[i - 1] = node.Children[i];
                    }
                    node.Children[i - 1] = null;
                    node.N--;
                }
                return;
            }


            //case 1:node is leaf and its key numbers >= t, delete A[i],K[i];
            if (node.IsLeaf && node.N >= minDegree)
            {
                int i = index + 1;
                for (; i < node.N; i++)
                {
                    node.KeyValues[i - 1] = node.KeyValues[i];
                }
                //for gc and accuracy
                node.KeyValues[i - 1] = null;
                //actually no need to update child because node is leaf and all its children are null
                for (i = index + 1; i < node.N + 1; i++)
                {
                    node.Children[i - 1] = node.Children[i];
                }
                node.Children[i - 1] = null;
                node.N--;
                return;
            }

            //case 2:node is lead and its key numbers = t-1,
            if (node.IsLeaf && node.N == minDegree - 1)
            {
                throw new NotImplementedException();
                return;
            }
            throw new NotImplementedException();


        }


        #endregion
    }
}

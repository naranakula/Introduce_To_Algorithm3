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
        /// the number of keys is between [0,2t-1] && the root of B tree is always in main memory.
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

        /// <summary>
        /// get the root of B tree
        /// </summary>
        /// <returns></returns>
        public BTreeNode<K, V> GetRoot()
        {
            return root;
        }

        /// <summary>
        /// get the mindegree = the minimum number of children a non root node must have
        /// </summary>
        public int MinDegree
        {
            get { return minDegree; }
        }

        /// <summary>
        /// get the order of a tree = the maximum number of children a non root node must have
        /// </summary>
        public int Order
        {
            get { return 2 * minDegree; }
        }

        /// <summary>
        /// the height of B tree
        /// we define Heigth = 0 when a tree has no nodes
        /// Heighth = 1 when a tree just has root node.
        /// </summary>
        public int Heigth { get; set; }

        /// <summary>
        /// the number of key value in B tree
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// is btree empty
        /// </summary>
        public bool IsEmpty
        {
            get { return Count == 0; }
        }
        #endregion

        #region search

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
        /// search key in sequence
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
        /// search key return the first node searched just as sequence search
        /// </summary>
        /// <param name="key">if found, return the node and the index of key in the node. null, if not found</param>
        /// <returns></returns>
        public Tuple<BTreeNode<K, V>, int> BinarySearch(K key)
        {
            return BinarySearch(root, key);
        }

        /// <summary>
        /// search key return the first node searched just as sequence search
        /// </summary>
        /// <param name="root"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private Tuple<BTreeNode<K, V>, int> BinarySearch(BTreeNode<K, V> root, K key)
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

        /// <summary>
        /// search key return the first node searched just as sequence search
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private int BinarySearch(BTreeNode<K, V> node, K key, int begin, int end)
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
            //y is full
            BTreeNode<K, V> y = node.Children[i];
            BTreeNode<K, V> z = new BTreeNode<K, V>(minDegree);
            z.IsLeaf = y.IsLeaf;
            z.N = minDegree - 1;
            for (int j = 0; j < minDegree - 1; j++)
            {
                //move the last t-1 key to new created node
                z.KeyValues[j] = y.KeyValues[j + minDegree];
                //for accuracy and gc
                y.KeyValues[j + minDegree] = null;
            }
            if (!y.IsLeaf)
            {
                //if y is leaf, the y's children are null
                //copy t child point
                for (int j = 0; j < minDegree; j++)
                {
                    z.Children[j] = y.Children[j + minDegree];
                    y.Children[j + minDegree] = null;
                }
            }

            y.N = minDegree - 1;

            for (int j = node.N; j > i; j--)
            {
                //move the node
                node.Children[j + 1] = node.Children[j];
            }
            node.Children[i + 1] = z;
            for (int j = node.N - 1; j >= i; j--)
            {
                node.KeyValues[j + 1] = node.KeyValues[j];
            }

            node.KeyValues[i] = y.KeyValues[minDegree - 1];
            y.KeyValues[minDegree - 1] = null;
            node.N++;
        }

        /// <summary>
        /// if it already contains in tree, return true and update the value. if not, insert a new node, return false.
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

            // insert first node
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
            //root.N can be 0
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
                while (i >= 0 && key.CompareTo(root.KeyValues[i].Item1) < 0)
                {
                    i--;
                }
                i++;
                BTreeNode<K, V> node = root.Children[i];
                if (node.N == 2 * minDegree - 1)
                {
                    SplitChild(root, i);
                    if (key.CompareTo(root.KeyValues[i].Item1) >= 0)
                    {
                        i++;
                    }
                }
                InsertNonFull(root.Children[i], key, val);
            }
        }

        #endregion

        #region delete

        /// <summary>
        /// you need find the item by using search
        /// when we delete a key from node x, we design this node x must have at least minDegree keys which is one more then the least a nonroot node can have
        /// 1)if the key  k in node x and x is a leaf,delete the k from x.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if contains key , otherwise false.</returns>
        public bool Delete(K key)
        {
            Tuple<BTreeNode<K,V>, int> searchResult = Search(key);
            if (searchResult == null)
            {
                return false;
            }
            Count--;
            Delete(root, key);
            if (root.N == 0 && root.IsLeaf)
            {
                root = null;
                Heigth--;
            }
            else if (root.N == 0)
            {
                root = root.Children[0];
                Heigth--;
            }
            return true;
        }

        /// <summary>
        /// we guarantee that whenever call delete, the first parameter node must have at least t keys where t is the minimum degree. This condition allow us to delete a key from the tree in one way downward pass without having to back up.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        private void Delete(BTreeNode<K, V> node, K key)
        {
            if (node == null)
            {
                return;
            }

            #region case 1: node is leaf
            if (node.IsLeaf)
            {
                //this works even key is not in the node
                for (int i = 0; i < node.N; i++)
                {
                    if (node.KeyValues[i].Item1.CompareTo(key) == 0)
                    {
                        for (int j = i; j < node.N - 1; j++)
                        {
                            node.KeyValues[i] = node.KeyValues[i + 1];
                        }
                        //for gc
                        node.KeyValues[node.N - 1] = null;
                        node.N--;
                        break;
                    }
                }
                return;
            }
            #endregion

            Tuple<int, bool> tuple = node.Search(key);

            if (tuple.Item2)
            {
                #region case 2: node is a internal node && key is in node

                BTreeNode<K, V> precedeChild = node.Children[tuple.Item1];
                if (precedeChild.N >= minDegree)
                {
                    #region case 2a: the child y thar precede k in node has at least t keys,then find biggest key in y, delete y and replace it in x.
                    Tuple<K,V> max = precedeChild.Maximum();
                    Delete(precedeChild, max.Item1);
                    node.KeyValues[tuple.Item1] = new Tuple<K, V>(max.Item1, max.Item2);
                    return;
                    #endregion
                }
                else
                {
                    #region case 2b:examine the child z follows k in nodex, if z has at least minDegree keys. find the smallest in z, delete it from z, replace k with it.
                    BTreeNode<K, V> successorChild = node.Children[tuple.Item1 + 1];
                    if (successorChild.N >= minDegree)
                    {
                        Tuple<K, V> min = successorChild.Minimum();
                        Delete(successorChild, min.Item1);
                        node.KeyValues[tuple.Item1] = new Tuple<K, V>(min.Item1, min.Item2);
                        return;
                    }
                    #endregion

                    #region case 2c: the preceder and successor child of node have only minDegree-1 keys, merge key and preceder and successor into preceder,now preceder has 2minDegree-1 key. delete key from it.
                    for (int i = tuple.Item1 + 1; i < node.N; i++)
                    {
                        node.Children[i] = node.Children[i + 1];
                    }
                    //for gc and accurate
                    node.Children[node.N] = null;

                    Tuple<K, V> found = node.KeyValues[tuple.Item1];
                    for (int i = tuple.Item1; i < node.N - 1; i++)
                    {
                        node.KeyValues[i] = node.KeyValues[i + 1];
                    }
                    node.KeyValues[node.N - 1] = null;
                    node.N--;

                    //merge precede and successor
                    precedeChild.KeyValues[precedeChild.N] = found;
                    precedeChild.N++;
                    int flag = precedeChild.N;
                    for (int i = 0; i < successorChild.N; i++)
                    {
                        precedeChild.KeyValues[precedeChild.N] = successorChild.KeyValues[i];
                        precedeChild.N++;
                    }

                    for (int i = 0; i <= successorChild.N; i++)
                    {
                        precedeChild.Children[flag] = successorChild.Children[i];
                        flag++;
                    }

                    Delete(precedeChild, key);
                    return;
                    #endregion
                }
                #endregion
            }
            else
            {
                #region case 3: key is not present in internal node x
                BTreeNode<K, V> subTree = node.Children[tuple.Item1];
                if (subTree.N >= minDegree)
                {
                    Delete(subTree, key);
                }
                else
                {
                    BTreeNode<K, V> before = tuple.Item1 == 0 ? null : node.Children[tuple.Item1 - 1];
                    BTreeNode<K, V> after = tuple.Item1 == node.N? null : node.Children[tuple.Item1 + 1];
                    #region case 3a:If subTree has only t - 1 keys but has an immediate sibling with at least t keys, give subTree an extra key by moving a key from x down into subTrr, moving a key from subTree’s immediate left or right sibling up into x, and moving the appropriate child pointer from the sibling into subTree .
                    if (before != null && before.N >= minDegree)
                    {
                        Tuple<K, V> farther = node.KeyValues[tuple.Item1 - 1];
                        Tuple<K, V> leftKeyValue = before.KeyValues[before.N - 1];
                        BTreeNode<K, V> leftChild = before.Children[before.N];
                        before.KeyValues[before.N - 1] = null;
                        before.Children[before.N] = null;
                        before.N--;
                        node.KeyValues[tuple.Item1 - 1] = leftKeyValue;
                        for (int i = subTree.N; i >0; i--)
                        {
                            subTree.KeyValues[i] = subTree.KeyValues[i - 1]; 
                        }
                        subTree.KeyValues[0] = farther;
                        for (int i = subTree.N+1; i > 0; i--)
                        {
                            subTree.Children[i] = subTree.Children[i-1];
                        }
                        subTree.Children[0] = leftChild;
                        subTree.N++;
                        Delete(subTree, key);
                        return;
                    }

                    if (after != null && after.N >= minDegree)
                    {
                        Tuple<K, V> farther = node.KeyValues[tuple.Item1];
                        Tuple<K, V> rightKeyValue = after.KeyValues[0];
                        BTreeNode<K, V> rightChild = after.Children[0];
                        for (int i = 0; i < after.N-1; i++)
                        {
                            after.KeyValues[i] = after.KeyValues[i + 1];
                        }
                        after.KeyValues[after.N - 1] = null;

                        for (int i = 0; i < after.N; i++)
                        {
                            after.Children[i] = after.Children[i + 1];
                        }
                        after.Children[after.N] = null;
                        after.N--;

                        node.KeyValues[tuple.Item1] = rightKeyValue;
                        subTree.KeyValues[subTree.N] = farther;
                        subTree.Children[subTree.N + 1] = rightChild;
                        subTree.N++;
                        Delete(subTree, key);
                        return;
                    }
                    #endregion
                    if (before != null)
                    {
                        Tuple<K, V> farther = node.KeyValues[tuple.Item1 - 1];
                        for (int i = tuple.Item1-1; i < node.N-1; i++)
                        {
                            node.KeyValues[i] = node.KeyValues[i + 1];
                        }
                        node.KeyValues[node.N - 1] = null;

                        before.KeyValues[before.N] = farther;
                        for (int i = 0; i < subTree.N; i++)
                        {
                            before.KeyValues[before.N + 1 + i] = subTree.KeyValues[i];
                        }
                        for (int i = 0; i <= subTree.N; i++)
                        {
                            before.Children[before.N + 1 + i] = subTree.Children[i];
                        }
                        before.N += 1 + subTree.N;
                        for (int i = tuple.Item1; i < node.N; i++)
                        {
                            node.Children[i] = node.Children[i + 1];
                        }
                        node.Children[node.N] = null;
                        node.N--;
                        Delete(before, key);
                    }
                    else if (after != null)
                    {
                        Tuple<K, V> farther = node.KeyValues[tuple.Item1];
                        for (int i = tuple.Item1; i < node.N - 1; i++)
                        {
                            node.KeyValues[i] = node.KeyValues[i + 1];
                        }
                        node.KeyValues[node.N - 1] = null;
                        subTree.KeyValues[subTree.N] = farther;
                        for (int i = 0; i < after.N; i++)
                        {
                            subTree.KeyValues[subTree.N + 1 + i] = after.KeyValues[i];
                        }
                        for (int i = 0; i <= after.N; i++)
                        {
                            subTree.Children[subTree.N + 1 + i] = after.Children[i];
                        }
                        subTree.N += 1 + after.N;
                        for (int i = tuple.Item1+1; i <  node.N; i++)
                        {
                            node.Children[i] = node.Children[i + 1];
                        }
                        node.Children[node.N] = null;
                        node.N--;
                        Delete(subTree, key);
                    }
                }
                #endregion
            }
        }

        #endregion

        #region isBTree

        /// <summary>
        /// is this tree a b tree
        /// </summary>
        /// <returns></returns>
        public bool IsBTree()
        {
            if (root == null)
            {
                return true;
            }

            List<BTreeNode<K, V>> lists = TreeWalk();
            foreach (var bTreeNode in lists)
            {
                if (!IsBTreeNode(bTreeNode))
                {
                    return false;
                }
            }

            List<K> list = InorderTreeWalk();
            if (list.Count != Count)
            {
                return false;
            }

            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].CompareTo(list[i + 1]) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// tree walk all the node
        /// </summary>
        /// <returns></returns>
        public List<BTreeNode<K, V>> TreeWalk()
        {
            List<BTreeNode<K, V>> lists = new List<BTreeNode<K, V>>();
            TreeWalk(root, ref lists);
            return lists;
        }

        /// <summary>
        /// tree walk all the node from root down to child
        /// </summary>
        /// <param name="node"></param>
        /// <param name="lists"></param>
        private void TreeWalk(BTreeNode<K, V> node, ref List<BTreeNode<K, V>> lists)
        {
            if (node == null)
            {
                return;
            }
            lists.Add(node);
            for (int i = 0; i <= node.N; i++)
            {
                TreeWalk(node.Children[i], ref lists);
            }
        }

        /// <summary>
        /// inorder tree walk all the key which will return a sorted list
        /// </summary>
        /// <returns></returns>
        public List<K> InorderTreeWalk()
        {
            List<K> lists = new List<K>();
            InorderTreeWalk(root, ref lists);
            return lists;
        }

        private void InorderTreeWalk(BTreeNode<K, V> node, ref List<K> lists)
        {
            if (node == null)
            {
                return;
            }

            for (int i = 0; i < node.N; i++)
            {
                InorderTreeWalk(node.Children[i], ref lists);
                lists.Add(node.KeyValues[i].Item1);
            }
            InorderTreeWalk(node.Children[node.N], ref  lists);
        }

        /// <summary>
        /// if a node btree node
        /// </summary>
        /// <param name="node"></param>
        private bool IsBTreeNode(BTreeNode<K, V> node)
        {
            if (node == null)
            {
                return true;
            }

            if (root != node)
            {
                if (!(node.N >= minDegree - 1 && node.N <= 2 * minDegree - 1))
                {
                    return false;
                }
            }

            for (int i = 0; i < node.N - 1; i++)
            {
                if (node.KeyValues[i].Item1.CompareTo(node.KeyValues[i + 1].Item1) > 0)
                {
                    return false;
                }
            }

            for (int i = node.N + 1; i < 2 * minDegree; i++)
            {
                if (node.Children[i] != null)
                {
                    throw new Exception();
                }
            }

            for (int i = node.N; i < 2 * minDegree - 1; i++)
            {
                if (node.KeyValues[i] != null)
                {
                    throw new Exception();
                }
            }
            return true;
        }

        #endregion
    }
}

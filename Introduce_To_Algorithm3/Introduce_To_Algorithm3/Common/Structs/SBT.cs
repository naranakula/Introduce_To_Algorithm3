using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// Size balanced Tree matain a size property. which meet
    /// s[right[t]] >= s[left[left[t]]], s[right[left[t]]]
    /// s[left[t]] >= s[left[right[t]]], s[right[right[t]]]
    /// 
    /// which runs at O(lgN)
    /// 
    /// treap>sbt>avl>rbt
    /// 
    /// rotate matains the binary search tree property
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class SBT<K, V> where K : IComparable<K>, IEquatable<K>
    {
        #region fromBST

        protected int count;
        /// <summary>
        /// return the number in tree
        /// </summary>
        public int Count
        {
            get { return count; }
        }
        /// <summary>
        /// root of the tree
        /// </summary>
        protected TreeNode<K, V> root;

        /// <summary>
        /// inorder tree walk, O(n)  return a sorted result
        /// </summary>
        /// <returns></returns>
        public List<Tuple<K, V>> InorderTreeWalk()
        {
            List<Tuple<K, V>> lists = new List<Tuple<K, V>>();
            InorderTreeWalk(lists, root);
            return lists;
        }

        /// <summary>
        /// inorder tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="root"></param>
        private void InorderTreeWalk(List<Tuple<K, V>> lists, TreeNode<K, V> root)
        {
            if (root == null) return;
            InorderTreeWalk(lists, root.Left);
            lists.Add(new Tuple<K, V>(root.Key, root.Value));
            InorderTreeWalk(lists, root.Right);
        }

        /// <summary>
        /// inorder tree walk, O(n)  return a sorted result
        /// </summary>
        /// <returns></returns>
        public List<TreeNode<K, V>> InorderTreeWalk_()
        {
            List<TreeNode<K, V>> lists = new List<TreeNode<K, V>>();
            InorderTreeWalk_(lists, root);
            return lists;
        }

        /// <summary>
        /// inorder tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="root"></param>
        private void InorderTreeWalk_(List<TreeNode<K, V>> lists, TreeNode<K, V> root)
        {
            if (root == null) return;
            InorderTreeWalk_(lists, root.Left);
            lists.Add(root);
            InorderTreeWalk_(lists, root.Right);
        }

        /// <summary>
        /// pre order walk tree O(n)
        /// </summary>
        /// <returns></returns>
        public List<Tuple<K, V>> PreorderTreeWalk()
        {
            List<Tuple<K, V>> lists = new List<Tuple<K, V>>();
            PreorderTreeWalk(lists, root);
            return lists;
        }

        /// <summary>
        /// pre order walk tree
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="root"></param>
        private void PreorderTreeWalk(List<Tuple<K, V>> lists, TreeNode<K, V> root)
        {
            if (root == null) return;
            lists.Add(new Tuple<K, V>(root.Key, root.Value));
            PreorderTreeWalk(lists, root.Left);
            PreorderTreeWalk(lists, root.Right);
        }

        /// <summary>
        /// pre order walk tree O(n)
        /// </summary>
        /// <returns></returns>
        public List<TreeNode<K, V>> PreorderTreeWalk_()
        {
            List<TreeNode<K, V>> lists = new List<TreeNode<K, V>>();
            PreorderTreeWalk_(lists, root);
            return lists;
        }

        /// <summary>
        /// pre order walk tree
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="root"></param>
        private void PreorderTreeWalk_(List<TreeNode<K, V>> lists, TreeNode<K, V> root)
        {
            if (root == null) return;
            lists.Add(root);
            PreorderTreeWalk_(lists, root.Left);
            PreorderTreeWalk_(lists, root.Right);
        }

        /// <summary>
        /// post order walk tree runs at O(n)
        /// </summary>
        /// <returns></returns>
        public List<Tuple<K, V>> PostorderTreeWalk()
        {
            List<Tuple<K, V>> lists = new List<Tuple<K, V>>();
            PostorderTreeWalk(lists, root);
            return lists;
        }

        /// <summary>
        /// post order walk tree
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="treeNode"></param>
        private void PostorderTreeWalk(List<Tuple<K, V>> lists, TreeNode<K, V> root)
        {
            if (root == null) return;
            PostorderTreeWalk(lists, root.Left);
            PostorderTreeWalk(lists, root.Right);
            lists.Add(new Tuple<K, V>(root.Key, root.Value));
        }

        /// <summary>
        /// post order walk tree runs at O(n)
        /// </summary>
        /// <returns></returns>
        public List<TreeNode<K, V>> PostorderTreeWalk_()
        {
            List<TreeNode<K, V>> lists = new List<TreeNode<K, V>>();
            PostorderTreeWalk_(lists, root);
            return lists;
        }

        /// <summary>
        /// post order walk tree
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="treeNode"></param>
        private void PostorderTreeWalk_(List<TreeNode<K, V>> lists, TreeNode<K, V> root)
        {
            if (root == null) return;
            PostorderTreeWalk_(lists, root.Left);
            PostorderTreeWalk_(lists, root.Right);
            lists.Add(root);
        }

        /// <summary>
        /// runs at O(h) h is the high of tree
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<K, V> Search(K key)
        {
            return Search(key, root);
        }

        private Tuple<K, V> Search(K key, TreeNode<K, V> root)
        {
            if (root == null) return null;
            int i = root.Key.CompareTo(key);

            if (i == 0)
            {
                return new Tuple<K, V>(root.Key, root.Value);
            }
            else if (i > 0)
            {
                return Search(key, root.Left);
            }
            else
            {
                return Search(key, root.Right);
            }
        }


        /// <summary>
        /// runs at O(h) h is the high of tree
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TreeNode<K, V> Search_(K key)
        {
            return Search_(key, root);
        }

        private TreeNode<K, V> Search_(K key, TreeNode<K, V> root)
        {
            if (root == null) return null;
            int i = root.Key.CompareTo(key);

            if (i == 0)
            {
                return root;
            }
            else if (i > 0)
            {
                return Search_(key, root.Left);
            }
            else
            {
                return Search_(key, root.Right);
            }
        }


        /// <summary>
        /// return the min one, null if the tree is empty  O(h)
        /// </summary>
        /// <returns></returns>
        public Tuple<K, V> Minimum()
        {
            return Minimum(root);
        }

        /// <summary>
        /// return the min one, null if the tree is empty  O(h)
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private Tuple<K, V> Minimum(TreeNode<K, V> root)
        {
            if (root == null) return null;
            TreeNode<K, V> node = root;

            while (node.Left != null)
            {
                node = node.Left;
            }

            return new Tuple<K, V>(node.Key, node.Value);
        }

        /// <summary>
        /// return the max one, null if the tree is empty  O(h)
        /// </summary>
        /// <returns></returns>
        public Tuple<K, V> Maximum()
        {
            return Maximum(root);
        }

        /// <summary>
        /// return the max one, null if the tree is empty  O(h)
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private Tuple<K, V> Maximum(TreeNode<K, V> root)
        {
            if (root == null) return null;
            TreeNode<K, V> node = root;

            while (node.Right != null)
            {
                node = node.Right;
            }

            return new Tuple<K, V>(node.Key, node.Value);
        }


        /// <summary>
        /// return the min one, null if the tree is empty  O(h)
        /// </summary>
        /// <returns></returns>
        public TreeNode<K, V> Minimum_()
        {
            return Minimum_(root);
        }

        /// <summary>
        /// return the min one, null if the tree is empty  O(h)
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private TreeNode<K, V> Minimum_(TreeNode<K, V> root)
        {
            if (root == null) return null;
            TreeNode<K, V> node = root;

            while (node.Left != null)
            {
                node = node.Left;
            }

            return node;
        }

        /// <summary>
        /// return the max one, null if the tree is empty  O(h)
        /// </summary>
        /// <returns></returns>
        public TreeNode<K, V> Maximum_()
        {
            return Maximum_(root);
        }

        /// <summary>
        /// return the max one, null if the tree is empty  O(h)
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private TreeNode<K, V> Maximum_(TreeNode<K, V> root)
        {
            if (root == null) return null;
            TreeNode<K, V> node = root;

            while (node.Right != null)
            {
                node = node.Right;
            }

            return node;
        }

        /// <summary>
        /// find successor
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public TreeNode<K, V> Successor_(TreeNode<K, V> root)
        {
            if (root == null) return null;

            if (root.Right != null)
            {
                return Minimum_(root.Right);
            }

            TreeNode<K, V> node = root.Parent;
            while (node != null && root == node.Right)
            {
                root = node;
                node = node.Parent;
            }
            return node;
        }

        /// <summary>
        /// find predecessor
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public TreeNode<K, V> Predecessor_(TreeNode<K, V> root)
        {
            if (root == null) return null;

            if (root.Left != null)
            {
                return Maximum_(root.Left);
            }

            TreeNode<K, V> node = root.Parent;
            while (node != null && root == node.Left)
            {
                root = node;
                node = node.Parent;
            }

            return node;
        }

        /// <summary>
        /// find successor
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public Tuple<K, V> Successor(TreeNode<K, V> root)
        {
            if (root == null) return null;

            if (root.Right != null)
            {
                return Minimum(root.Right);
            }

            TreeNode<K, V> node = root.Parent;
            while (node != null && root == node.Right)
            {
                root = node;
                node = node.Parent;
            }
            return node == null ? null : new Tuple<K, V>(node.Key, node.Value);
        }

        /// <summary>
        /// find predecessor
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public Tuple<K, V> Predecessor(TreeNode<K, V> root)
        {
            if (root == null) return null;

            if (root.Left != null)
            {
                return Maximum(root.Left);
            }

            TreeNode<K, V> node = root.Parent;
            while (node != null && root == node.Left)
            {
                root = node;
                node = node.Parent;
            }

            return node == null ? null : new Tuple<K, V>(node.Key, node.Value);
        }

        #endregion

        #region Insert

        /// <summary>
        /// insert to sbt which runs at O(lgn)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key, V val)
        {
            count++;

            TreeNode<K, V> inserted = new TreeNode<K, V>(key, val) { Size = 1 };
            TreeNode<K, V> node = root, parent = null;

            while (node != null)
            {
                node.Size++;
                parent = node;
                if (key.CompareTo(node.Key) < 0)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }

            inserted.Parent = parent;

            if (parent == null)
            {
                root = inserted;
            }
            else if (key.CompareTo(parent.Key) < 0)
            {
                parent.Left = inserted;
            }
            else
            {
                parent.Right = inserted;
            }

            //fix up the insert node, so sbt matains its property
            InsertFixup(inserted);
        }

        /// <summary>
        /// fix up the insert node, so sbt matains its property
        /// </summary>
        /// <param name="inserted"></param>
        private void InsertFixup(TreeNode<K, V> inserted)
        {
            
        }

        #region rotate

        /// <summary>
        /// left rotate
        /// </summary>
        /// <param name="node">node must have non null right node</param>
        private void LeftRotate(TreeNode<K, V> node)
        {
            TreeNode<K, V> right = node.Right;
            node.Right = right.Left;

            if (right.Left != null)
            {
                right.Left.Parent = node;
            }

            right.Parent = node.Parent;

            if (node.Parent == null)
            {
                root = right;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = right;
            }
            else
            {
                node.Parent.Right = right;
            }

            right.Left = node;
            node.Parent = right;

            right.Size = node.Size;
            node.Size = Size(node.Left) + Size(node.Right) + 1;
        }

        /// <summary>
        /// right rotate
        /// </summary>
        /// <param name="node">node must have left child</param>
        private void RightRotate(TreeNode<K, V> node)
        {
            TreeNode<K, V> left = node.Left;
            node.Left = left.Right;

            if (left.Right != null)
            {
                left.Right.Parent = node;
            }

            left.Parent = node.Parent;

            if (node.Parent == null)
            {
                root = left;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = left;
            }
            else
            {
                node.Parent.Right = left;
            }

            left.Right = node;
            node.Parent = left;

            left.Size = node.Size;
            node.Size = Size(node.Left) + Size(node.Right) + 1;
        }

        #endregion

        /// <summary>
        /// find the number of nodes in the tree
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int Size(TreeNode<K, V> node)
        {
            return node == null ? 0 : node.Size;
        }

        #endregion

        #region Delete

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void Delete(TreeNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            count--;
        }

        #endregion
    }
}

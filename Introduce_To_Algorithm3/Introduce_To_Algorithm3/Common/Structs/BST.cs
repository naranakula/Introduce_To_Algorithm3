﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// binary search tree. 
    /// node >= left node   node<=right node
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class BST<K, V> where K : IComparable<K>, IEquatable<K>
    {

        #region Member

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

        #endregion

        #region TreeWalk

        /// <summary>
        /// inorder tree walk, O(n)  return a sorted result
        /// </summary>
        /// <returns></returns>
        public List<Tuple<K,V>> InorderTreeWalk()
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
        private void InorderTreeWalk(List<Tuple<K,V>> lists,TreeNode<K,V> root)
        {
            if (root == null) return;
            InorderTreeWalk(lists, root.Left);
            lists.Add(new Tuple<K, V>(root.Key,root.Value));
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
        public List<Tuple<K,V>> PreorderTreeWalk()
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

        #endregion

        #region Search & Min & Max & Precedessor & Successor

        /// <summary>
        /// runs at O(h) h is the high of tree
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<K, V> Search(K key)
        {
            return Search(key, root);
        }

        private Tuple<K,V> Search(K key,TreeNode<K,V> root)
        {
            if (root == null)
            {
                return null;
            }

            int i = root.Key.CompareTo(key);

            if(i == 0)
            {
                return new Tuple<K, V>(root.Key,root.Value);
            }
            else if(i>0)
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
            if (root == null)
            {
                return null;
            }

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
        public Tuple<K,V> Minimum()
        {
            return Minimum(root);
        }

        /// <summary>
        /// return the min one, null if the tree is empty  O(h)
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private Tuple<K,V> Minimum(TreeNode<K,V> root)
        {
            if (root == null)
            {
                return null;
            }

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
        private Tuple<K, V> Maximum(TreeNode<K,V> root)
        {
            if (root == null)
            {
                return null;
            }

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
        public Tuple<K,V> Successor(TreeNode<K,V> root)
        {
            if (root == null) return null;

            if(root.Right != null)
            {
                return Minimum(root.Right);
            }

            TreeNode<K, V> node = root.Parent;
            while (node != null && root == node.Right)
            {
                root = node;
                node = node.Parent;
            }
            return node == null ? null : new Tuple<K, V>(node.Key,node.Value);
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

            return node==null?null:new Tuple<K, V>(node.Key, node.Value);
        }

        #endregion

        #region Insert & Delete

        /// <summary>
        /// insert 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public virtual void Insert(K key,V val)
        {
            count++;

            if(root == null)
            {
                root = new TreeNode<K, V>(key,val);
                return;
            }

            TreeNode<K, V> tmp = root,parent=null;
            while (tmp != null)
            {
                parent = tmp;
                tmp = tmp.Key.CompareTo(key)>0 ? tmp.Left : tmp.Right;
            }

            TreeNode<K,V> inserted = new TreeNode<K, V>(key,val);
            inserted.Parent = parent;
            if (parent.Key.CompareTo(key) > 0)
            {
                parent.Left = inserted;
            }
            else
            {
                parent.Right = inserted;
            }
        }


        /// <summary>
        /// delete a node
        /// 
        /// If node has no children, then we simply remove it by modifying its parent to replace with NIL as its child.
        /// If node has just one child, then we elevate that child to take ´node's position in the tree  by modifying ´node's parent to replace node by ´node's child.
        /// 
        /// </summary>
        /// <param name="node"></param>
        public virtual void Delete(TreeNode<K,V> node)
        {
            count--;
            if(node.Left == null)
            {
                Transplant(node,node.Right);
            }
            else if(node.Right == null)
            {
                Transplant(node, node.Left);
            }
            else
            {
                TreeNode<K, V> min = Minimum_(node.Right);
                if(min.Parent != node)
                {
                    Transplant(min,min.Right);
                    min.Right = node.Right;
                    min.Right.Parent = min;
                }
                Transplant(node,min);
                min.Left = node.Left;
                min.Left.Parent = min;
            }
        }

        #endregion

        #region Utils

        /// <summary>
        /// if a node is root node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsRoot(TreeNode<K,V> node)
        {
            return node.Parent == null;
        }


        /// <summary>
        /// if a node has left child
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool HasLeft(TreeNode<K, V> node)
        {
            return node.Left != null;
        }


        /// <summary>
        /// if a node has right node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool HasRight(TreeNode<K,V> node)
        {
            return node.Right != null;
        }


        /// <summary>
        /// When TRANSPLANT replaces the subtree rooted at node u with
        /// the subtree rooted at node v , node u’s parent becomes node v’s parent, and u’s parent ends up having v as its appropriate child.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        private void Transplant(TreeNode<K,V> u,TreeNode<K,V> v)
        {
            if(IsRoot(u))
            {
                root = v;
            }
            else if(u == u.Parent.Left)
            {
                u.Parent.Left = v;
            }
            else
            {
                u.Parent.Right = v;
            }

            if(v != null)
            {
                v.Parent = u.Parent;
            }
        }

        #endregion

        #region Order

        /// <summary>
        /// select ith smallest element inorder 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TreeNode<K,V> Select(int i)
        {
            return Select(root, i);
        }

        private TreeNode<K, V> Select(TreeNode<K,V> root,int i)
        {
            int r = 1+(root.Left == null ? 0 : root.Left.Size);
            if(i==r)
            {
                return root;
            }
            else if(i<r)
            {
                return Select(root.Left, i);
            }
            else
            {
                return Select(root.Right, i - r);
            }
        }


        /// <summary>
        /// given a node return the position of x in the linear order by inorder tree walk
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int Rank(TreeNode<K,V> node)
        {
            int i = Size(node.Left) + 1;
            while (node != root)
            {
                if(node == node.Parent.Right)
                {
                    i += Size(node.Parent.Left) + 1;
                }
                node = node.Parent;
            }

            return i;
        }

        /// <summary>
        /// find the number of nodes in the tree
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int Size(TreeNode<K,V> node)
        {
            return node == null ? 0 : node.Size;
        }

        #endregion

        #region Height

        /// <summary>
        /// get the height of rbt tree.  
        /// it runs at O(n)
        /// T(n) = 2T(n/2) + O(1)
        /// define height as follow:
        /// 1)if a node is null, its height is 0
        /// 2) it a node has left or right child, its height is 1.(we treat its left & right children are null)
        /// 3）height(node) = 1+ Max(    height(node.left), height(node.right)  )
        /// </summary>
        /// <returns></returns>
        public int MaxHeight()
        {
            return MaxHeight(root);
        }

        /// <summary>
        /// get the height of rbt tree
        /// define height as follow:
        /// 1)if a node is null, its height is 0
        /// 2) it a node has left or right child, its height is 1.(we treat its left & right children are null)
        /// 3）height(node) = 1+ Max(    height(node.left), height(node.right)  )
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int MaxHeight(TreeNode<K, V> node)
        {
            if (node == null)
            {
                return 0;
            }

            return System.Math.Max(MaxHeight(node.Left), MaxHeight(node.Right)) + 1;
        }

        /// <summary>
        /// get the height of rbt tree.  
        /// it runs at O(n)
        /// T(n) = 2T(n/2) + O(1)
        /// define height as follow:
        /// 1)if a node is null, its height is 0
        /// 2) it a node has left or right child, its height is 1.(we treat its left & right children are null)
        /// 3）height(node) = 1+ Min(    height(node.left), height(node.right)  )
        /// </summary>
        /// <returns></returns>
        public int MinHeight()
        {
            return MinHeight(root);
        }

        /// <summary>
        /// get the height of rbt tree
        /// define height as follow:
        /// 1)if a node is null, its height is 0
        /// 2) it a node has left or right child, its height is 1.(we treat its left & right children are null)
        /// 3）height(node) = 1+ Min(    height(node.left), height(node.right)  )
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int MinHeight(TreeNode<K, V> node)
        {
            if (node == null)
            {
                return 0;
            }

            return System.Math.Min(MinHeight(node.Left), MinHeight(node.Right)) + 1;
        }

        #endregion
    }
}

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
    public class RBT<K,V> where K : IComparable<K>, IEquatable<K>
    {
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
        protected RBTreeNode<K, V> root;


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
        private void InorderTreeWalk(List<Tuple<K, V>> lists, RBTreeNode<K, V> root)
        {
            if (root == null) return;
            InorderTreeWalk(lists, root.Left);
            lists.Add(new Tuple<K, V>(root.Key, root.Value));
            InorderTreeWalk(lists, root.Right);
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
        private void PreorderTreeWalk(List<Tuple<K, V>> lists, RBTreeNode<K, V> root)
        {
            if (root == null) return;
            lists.Add(new Tuple<K, V>(root.Key, root.Value));
            PreorderTreeWalk(lists, root.Left);
            PreorderTreeWalk(lists, root.Right);
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
        private void PostorderTreeWalk(List<Tuple<K, V>> lists, RBTreeNode<K, V> treeNode)
        {
            if (root == null) return;
            PostorderTreeWalk(lists, root.Left);
            PostorderTreeWalk(lists, root.Right);
            lists.Add(new Tuple<K, V>(root.Key, root.Value));
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

        private Tuple<K, V> Search(K key, RBTreeNode<K, V> root)
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
        public RBTreeNode<K, V> Search_(K key)
        {
            return Search_(key, root);
        }

        private RBTreeNode<K, V> Search_(K key, RBTreeNode<K, V> root)
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
        private Tuple<K, V> Minimum(RBTreeNode<K, V> root)
        {
            if (root == null) return null;
            RBTreeNode<K, V> node = root;

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
        private Tuple<K, V> Maximum(RBTreeNode<K, V> root)
        {
            if (root == null) return null;
            RBTreeNode<K, V> node = root;

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
        public RBTreeNode<K, V> Minimum_()
        {
            return Minimum_(root);
        }

        /// <summary>
        /// return the min one, null if the tree is empty  O(h)
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private RBTreeNode<K, V> Minimum_(RBTreeNode<K, V> root)
        {
            if (root == null) return null;
            RBTreeNode<K, V> node = root;

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
        public RBTreeNode<K, V> Maximum_()
        {
            return Maximum_(root);
        }

        /// <summary>
        /// return the max one, null if the tree is empty  O(h)
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private RBTreeNode<K, V> Maximum_(RBTreeNode<K, V> root)
        {
            if (root == null) return null;
            RBTreeNode<K, V> node = root;

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
        public RBTreeNode<K, V> Successor_(RBTreeNode<K, V> root)
        {
            if (root == null) return null;

            if (root.Right != null)
            {
                return Minimum_(root.Right);
            }

            RBTreeNode<K, V> node = root.Parent;
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
        public RBTreeNode<K, V> Predecessor_(RBTreeNode<K, V> root)
        {
            if (root == null) return null;

            if (root.Left != null)
            {
                return Maximum_(root.Left);
            }

            RBTreeNode<K, V> node = root.Parent;
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
        public Tuple<K, V> Successor(RBTreeNode<K, V> root)
        {
            if (root == null) return null;

            if (root.Right != null)
            {
                return Minimum(root.Right);
            }

            RBTreeNode<K, V> node = root.Parent;
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
        public Tuple<K, V> Predecessor(RBTreeNode<K, V> root)
        {
            if (root == null) return null;

            if (root.Left != null)
            {
                return Maximum(root.Left);
            }

            RBTreeNode<K, V> node = root.Parent;
            while (node != null && root == node.Left)
            {
                root = node;
                node = node.Parent;
            }

            return node == null ? null : new Tuple<K, V>(node.Key, node.Value);
        }


        /// <summary>
        /// Red Black insert
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key,V val)
        {
        }


        /// <summary>
        /// left rotate 
        /// </summary>
        /// <param name="x">x must have right child</param>
        private void LeftRotate(RBTreeNode<K,V> x)
        {
            RBTreeNode<K, V> y = x.Right;
            x.Right = y.Left;

            if(y.Left != null)
            {
                y.Left.Parent = x;
            }

            y.Parent = x.Parent;

            if(x.Parent == null)
            {
                root = y;
            }
            else if(x == x.Parent.Left)
            {
                x.Parent.Left = y;
            }
            else
            {
                x.Parent.Right = y;
            }

            y.Left = x;
            x.Parent = y;
        }



        /// <summary>
        /// right rotate 
        /// </summary>
        /// <param name="x">x must have left child</param>
        private void RightRotate(RBTreeNode<K, V> x)
        {
            RBTreeNode<K, V> y = x.Left;
            x.Left = y.Right;

            if (y.Right != null)
            {
                y.Right.Parent = x;
            }

            y.Parent = x.Parent;

            if (x.Parent == null)
            {
                root = y;
            }
            else if (x == x.Parent.Left)
            {
                x.Parent.Left = y;
            }
            else
            {
                x.Parent.Right = y;
            }

            y.Right = x;
            x.Parent = y;
        }


    }
}

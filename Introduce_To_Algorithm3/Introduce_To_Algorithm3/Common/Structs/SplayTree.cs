using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// splay tree implement insert, search, delete at O(lgn).
    /// Splay tree are self - balanced trees with an amortized runtime of n(lgn) for a set of n operations on a tree with n elements.They're not a bad implementation of a dictionary with random access, but where sorted traversal is important.
    /// 
    /// If we want to search a key, we normally will search it again.To reduce the time used to search, the higher frequency a node is searched, the node should nearer to root.when we search a node, we then move the searched node into root, this kind tree called splay tree.
    /// 
    /// 
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class SplayTree<K, V> where K : IComparable<K>, IEquatable<K>
    {
        #region Inorder Tree Walk

        /// <summary>
        /// inoreder tree walk, it runs at O(n) & return a sorted result
        /// </summary>
        /// <returns></returns>
        public List<Tuple<K, V>> InorderTreeWalk()
        {
            List<Tuple<K, V>> lists = new List<Tuple<K, V>>();
            InorderTreeWalk(ref lists, root);
            return lists;
        }

        /// <summary>
        /// inorder tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="root"></param>
        private void InorderTreeWalk(ref List<Tuple<K, V>> lists, SplayNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            InorderTreeWalk(ref lists, node.Left);
            lists.Add(new Tuple<K, V>(node.Key, node.Value));
            InorderTreeWalk(ref lists, node.Right);
        }

        /// <summary>
        /// inorder tree walk, which runs at O(n) and return a sorted list
        /// </summary>
        /// <returns></returns>
        public List<SplayNode<K, V>> InorderTreeWalk_()
        {
            List<SplayNode<K, V>> lists = new List<SplayNode<K, V>>();

            InorderTreeWalk_(ref lists, root);

            return lists;
        }

        static int i = 0;
        /// <summary>
        /// inorder tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        private void InorderTreeWalk_(ref List<SplayNode<K, V>> lists, SplayNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }
            
            InorderTreeWalk_(ref lists, node.Left);
            lists.Add(node);
            InorderTreeWalk_(ref lists, node.Right);
        }
        #endregion

        #region Preorder Tree Walk

        /// <summary>
        /// preorder tree walk, it runs at O(n)
        /// </summary>
        /// <returns></returns>
        public List<Tuple<K, V>> PreorderTreeWalk()
        {
            List<Tuple<K, V>> lists = new List<Tuple<K, V>>();
            PreorderTreeWalk(ref lists, root);
            return lists;
        }

        /// <summary>
        /// Preorder tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="root"></param>
        private void PreorderTreeWalk(ref List<Tuple<K, V>> lists, SplayNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            lists.Add(new Tuple<K, V>(node.Key, node.Value));
            PreorderTreeWalk(ref lists, node.Left);
            PreorderTreeWalk(ref lists, node.Right);
        }

        /// <summary>
        /// Preorder tree walk, which runs at O(n) 
        /// </summary>
        /// <returns></returns>
        public List<SplayNode<K, V>> PreorderTreeWalk_()
        {
            List<SplayNode<K, V>> lists = new List<SplayNode<K, V>>();
            PreorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// Preorder tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        private void PreorderTreeWalk_(ref List<SplayNode<K, V>> lists, SplayNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            lists.Add(node);
            PreorderTreeWalk_(ref lists, node.Left);
            PreorderTreeWalk_(ref lists, node.Right);
        }
        #endregion

        #region Postorder Tree Walk

        /// <summary>
        /// Postorder tree walk, it runs at O(n)
        /// </summary>
        /// <returns></returns>
        public List<Tuple<K, V>> PostorderTreeWalk()
        {
            List<Tuple<K, V>> lists = new List<Tuple<K, V>>();
            PostorderTreeWalk(ref lists, root);
            return lists;
        }

        /// <summary>
        /// Postorder tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="root"></param>
        private void PostorderTreeWalk(ref List<Tuple<K, V>> lists, SplayNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            PostorderTreeWalk(ref lists, node.Left);
            PostorderTreeWalk(ref lists, node.Right);
            lists.Add(new Tuple<K, V>(node.Key, node.Value));
        }

        /// <summary>
        /// Postorder tree walk, which runs at O(n) 
        /// </summary>
        /// <returns></returns>
        public List<SplayNode<K, V>> PostorderTreeWalk_()
        {
            List<SplayNode<K, V>> lists = new List<SplayNode<K, V>>();
            PostorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// Postorder tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        private void PostorderTreeWalk_(ref List<SplayNode<K, V>> lists, SplayNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            PostorderTreeWalk_(ref lists, node.Left);
            PostorderTreeWalk_(ref lists, node.Right);
            lists.Add(node);
        }
        #endregion

        #region Minimum

        /// <summary>
        /// return the min one, null if the tree is empty which runs at O(n)
        /// </summary>
        /// <returns></returns>
        public Tuple<K, V> Minimum()
        {
            return Minimum(root);
        }

        /// <summary>
        /// return the minimum one
        /// </summary>
        /// <returns></returns>
        private Tuple<K, V> Minimum(SplayNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            while (node.Left != null)
            {
                node = node.Left;
            }

            return new Tuple<K, V>(node.Key, node.Value);
        }

        /// <summary>
        /// return the min one, null if the tree is empty which runs at O(n)
        /// </summary>
        /// <returns></returns>
        public SplayNode<K, V> Minimum_()
        {
            return Minimum_(root);
        }

        /// <summary>
        /// return the minimum one
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private SplayNode<K, V> Minimum_(SplayNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            while (node.Left != null)
            {
                node = node.Left;
            }

            return node;
        }

        #endregion

        #region Maximum

        /// <summary>
        /// return the max one, null if the tree is empty which runs at O(n)
        /// </summary>
        /// <returns></returns>
        public Tuple<K, V> Maximum()
        {
            return Maximum(root);
        }

        /// <summary>
        /// return the Maximum one
        /// </summary>
        /// <returns></returns>
        private Tuple<K, V> Maximum(SplayNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            while (node.Right != null)
            {
                node = node.Right;
            }

            return new Tuple<K, V>(node.Key, node.Value);
        }

        /// <summary>
        /// return the max one, null if the tree is empty which runs at O(n)
        /// </summary>
        /// <returns></returns>
        public SplayNode<K, V> Maximum_()
        {
            return Maximum_(root);
        }

        /// <summary>
        /// return the Maximum one
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private SplayNode<K, V> Maximum_(SplayNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            while (node.Right != null)
            {
                node = node.Right;
            }

            return node;
        }

        #endregion

        #region Member
        /// <summary>
        /// the root of splay tree
        /// </summary>
        private SplayNode<K, V> root;

        /// <summary>
        /// the number of data in this tree
        /// </summary>
        private int count;

        /// <summary>
        /// the number of data in this tree
        /// </summary>
        public int Count { get { return count; } }
        #endregion

        #region Constructor
        /// <summary>
        /// constructor, create an empty tree.
        /// </summary>
        public SplayTree()
        {
        }
        #endregion

        #region LEFT & RIGHT rotate

        /// <summary>
        /// left rotate 
        /// </summary>
        /// <param name="node">node must have non null right node</param>
        private void LeftRotate(SplayNode<K, V> node)
        {
            SplayNode<K, V> right = node.Right;
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
        }

        /// <summary>
        /// right rotate
        /// </summary>
        /// <param name="node">node must have non null left child</param>
        private void RightRotate(SplayNode<K, V> node)
        {
            SplayNode<K, V> left = node.Left;
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
        }
        #endregion

        #region BOTTOM-UP splay
        /// <summary>
        /// splay function move node to the root
        /// 0)if node's parent is null, then node is root, no need to do anything
        /// 1)if node has parent but doesn't have grand parent, if node is left child, then right rotate node's parent. if node is right child, then left rotate node's parent. After this, node move to root. no need to do something else.
        /// 2)if node has both parent (named P) and grand parent(named G). node and P are both left(right) child, then right(left) rotate P-G && P-Node.  node is left(right) child & parent is right(left) child, then right(left) rotate P-Node && left(right) rotate G-Node.
        /// </summary>
        /// <param name="node"></param>
        private void BottomUpSplay(SplayNode<K, V> node)
        {
            if (node.Parent == null)
            {
                //no need to do anything
                return;
            }

            if (node.Parent.Parent == null)
            {
                //node has parent, but node.parent doesn;t have parent
                if (node == node.Parent.Left)
                {
                    RightRotate(node.Parent);
                    //also no need to do anything
                }
                else
                {
                    LeftRotate(node.Parent);
                    //also no need to do anything
                }
            }
            else
            {
                if (node == node.Parent.Left)
                {
                    if (node.Parent == node.Parent.Parent.Left)
                    {
                        RightRotate(node.Parent.Parent);
                        RightRotate(node.Parent);
                        //recursive call
                        BottomUpSplay(node);
                    }
                    else
                    {
                        RightRotate(node.Parent);
                        LeftRotate(node.Parent);
                        BottomUpSplay(node);
                    }
                }
                else
                {
                    if (node.Parent == node.Parent.Parent.Right)
                    {
                        LeftRotate(node.Parent.Parent);
                        LeftRotate(node.Parent);
                        BottomUpSplay(node);
                    }
                    else
                    {
                        LeftRotate(node.Parent);
                        RightRotate(node.Parent);
                        BottomUpSplay(node);
                    }
                }
            }
        }
        #endregion

        #region Search

        /// <summary>
        /// Search a node whose key is key, if found the node, then splay node.
        /// if can not found node, then return null & splay the most recently visited node
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SplayNode<K, V> Search_(K key)
        {
            if (root == null)
            {
                return null;
            }

            SplayNode<K, V> node = root, lastVisited = null;

            while (node != null)
            {
                lastVisited = node;
                int i = node.Key.CompareTo(key);
                if (i == 0)
                {
                    BottomUpSplay(node);
                    // now node is moved to the root
                    return node;
                }
                else if (i > 0)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }

            // can not find the key, splay the most recently visited node, then return null
            BottomUpSplay(lastVisited);
            return null;

        }

        /// <summary>
        /// Search a node whose key is key, if found the node, then splay node.
        /// if can not found node, then return null & splay the most recently visited node
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<K, V> Search(K key)
        {
            SplayNode<K, V> node = Search_(key);
            return node == null ? null : new Tuple<K, V>(node.Key, node.Value);
        }

        #endregion

        #region Insert

        /// <summary>
        /// insert a new node to splay tree
        /// insert the new node to the splay tree, then splay the new node.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key, V val)
        {
            count++;
            SplayNode<K, V> inserted = new SplayNode<K, V>(key, val);
            if (root == null)
            {
                //insert the first node
                root = inserted;
                return;
            }

            SplayNode<K, V> node = root, lastVisited = null;
            bool isRightChild = false;
            while (node != null)
            {
                lastVisited = node;
                int i = node.Key.CompareTo(key);
                if (i > 0)
                {
                    node = node.Left;
                    isRightChild = false;
                }
                else
                {
                    node = node.Right;
                    isRightChild = true;
                }
            }

            inserted.Parent = lastVisited;
            if (isRightChild)
            {
                lastVisited.Right = inserted;
            }
            else
            {
                lastVisited.Left = inserted;
            }

            BottomUpSplay(inserted);
        }

        #endregion

        #region Delete

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Delete(K key)
        {
        }

        #endregion

        #region Split

        /// <summary>
        /// split the splay tree into two part:T1, T2
        /// 
        /// 1)the key in T1 <= key
        /// 2)the key in T2 > key
        /// </summary>
        /// <param name="key"></param>
        public void Split(K key)
        {
        }

        #endregion


        #region Join
        #endregion


        #region IsBST

        /// <summary>
        /// determine whether a node is binary search tree node 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsBSTNode(SplayNode<K, V> node)
        {
            if (node == null)
            {
                return true;
            }

            if (node.Left != null && node.Left.Key.CompareTo(node.Key) > 0)
            {
                return false;
            }

            if (node.Right != null && node.Right.Key.CompareTo(node.Key) < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// determine wheter if this tree is binary search tree
        /// </summary>
        /// <returns></returns>
        public bool IsBST()
        {
            List<SplayNode<K, V>> lists = InorderTreeWalk_();

            if (lists.Count != count)
            {
                throw new Exception("the node number of this tree doesn't match the Count property");
            }

            for (int i = 0; i < lists.Count; i++)
            {
                if (!IsBSTNode(lists[i]))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}

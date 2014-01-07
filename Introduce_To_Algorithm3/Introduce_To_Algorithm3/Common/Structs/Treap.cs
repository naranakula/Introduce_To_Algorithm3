using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// we assign node.priority which is a random number chosen independently for each node
    /// The nodes of the treap are ordered so that the keys obey the binary-search-tree property and the priorities obey the min-heap order property
    /// if v is a left child of u, then v.key &lt;= u.key
    /// if v is a right child of u, then v.key  &gt;= u.key
    /// if v is a child of u, the v.priority &gt;= u.priority
    /// 
    /// Then the resulting treap is the tree that would have been formed as if the nodes had been inserted into a normal binary search tree in the order given by their priorities. i.e., x.priority &lt; y.priority means that we had inserted x before y
    /// 
    /// This treap's random generator use current time, so the keys must not generate from the same generator
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class Treap<K, V> where K : IComparable<K>, IEquatable<K>
    {
        #region Count

        /// <summary>
        /// the root of this tree
        /// </summary>
        private TreapNode<K, V> root;

        /// <summary>
        /// the number of node in treep
        /// </summary>
        private int count;

        public Treap() { }

        /// <summary>
        /// the number of nodes in the tree
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        #endregion

        #region TreeWalk

        /// <summary>
        /// inorder tree walk
        /// runs at O(n) and will return a sorted result
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
        /// <param name="node"></param>
        public void InorderTreeWalk(ref List<Tuple<K, V>> lists, TreapNode<K, V> node)
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
        /// inorder tree walk
        /// which runs at O(n)
        /// </summary>
        /// <returns></returns>
        public List<TreapNode<K, V>> InorderTreeWalk_()
        {
            List<TreapNode<K, V>> lists = new List<TreapNode<K, V>>();
            InorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// inorder tree walk
        /// which runs at O(n) 
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        public void InorderTreeWalk_(ref List<TreapNode<K, V>> lists, TreapNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            InorderTreeWalk_(ref lists, node.Left);
            lists.Add(node);
            InorderTreeWalk_(ref lists, node.Right);
        }


        /// <summary>
        /// pre order walk tree which runs at O(n)
        /// </summary>
        /// <returns></returns>
        public List<Tuple<K, V>> PreorderTreeWalk()
        {
            List<Tuple<K, V>> lists = new List<Tuple<K, V>>();
            PreorderTreeWalk(ref lists, root);
            return lists;
        }

        /// <summary>
        /// pre order walk tree
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        public void PreorderTreeWalk(ref List<Tuple<K, V>> lists, TreapNode<K, V> node)
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
        /// preorder tree walk
        /// which runs at O(n)
        /// </summary>
        /// <returns></returns>
        public List<TreapNode<K, V>> PreorderTreeWalk_()
        {
            List<TreapNode<K, V>> lists = new List<TreapNode<K, V>>();
            PreorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// preorder tree walk
        /// which runs at O(lgn) & which is the height of tree
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        public void PreorderTreeWalk_(ref List<TreapNode<K, V>> lists, TreapNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            lists.Add(node);
            PreorderTreeWalk_(ref lists, node.Left);
            PreorderTreeWalk_(ref lists, node.Right);
        }

        /// <summary>
        /// post order walk tree which runs at O(n)
        /// </summary>
        /// <returns></returns>
        public List<Tuple<K, V>> PostorderTreeWalk()
        {
            List<Tuple<K, V>> lists = new List<Tuple<K, V>>();
            PostorderTreeWalk(ref lists, root);
            return lists;
        }

        /// <summary>
        /// post order walk tree
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        private void PostorderTreeWalk(ref List<Tuple<K, V>> lists, TreapNode<K, V> node)
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
        /// post order tree walk 
        /// which runs at O(n) 
        /// </summary>
        /// <returns></returns>
        public List<TreapNode<K, V>> PostorderTreeWalk_()
        {
            List<TreapNode<K, V>> lists = new List<TreapNode<K, V>>();
            PostorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// post order tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        public void PostorderTreeWalk_(ref List<TreapNode<K, V>> lists, TreapNode<K, V> node)
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

        #region Search

        /// <summary>
        /// search a key runs at O(lgn)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<K, V> Search(K key)
        {
            return Search(key, root);
        }

        /// <summary>
        /// search key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private Tuple<K, V> Search(K key, TreapNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }
            int i = node.Key.CompareTo(key);

            if (i == 0)
            {
                return new Tuple<K, V>(node.Key, node.Value);
            }
            else if (i > 0)
            {
                return Search(key, node.Left);
            }
            else
            {
                return Search(key, node.Right);
            }
        }

        /// <summary>
        /// runs at O(lgn) which is the high of the tree
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TreapNode<K, V> Search_(K key)
        {
            return Search_(key, root);
        }

        /// <summary>
        /// runs at O(lgn) which is the high of the tree
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private TreapNode<K, V> Search_(K key, TreapNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            int i = node.Key.CompareTo(key);

            if (i == 0)
            {
                return node;
            }
            else if (i > 0)
            {
                return Search_(key, node.Left);
            }
            else
            {
                return Search_(key, node.Right);
            }
        }

        #endregion

        #region Min max

        /// <summary>
        /// return the min one , null if the tree is empty
        /// runs at O(lgn) which is the height of tree
        /// </summary>
        /// <returns></returns>
        public Tuple<K, V> Minimum()
        {
            return Minimum(root);
        }

        /// <summary>
        /// return the min one, null if the tree is empty
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Tuple<K, V> Minimum(TreapNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            TreapNode<K, V> result = node;

            while (result.Left != null)
            {
                result = result.Left;
            }

            return new Tuple<K, V>(result.Key, result.Value);
        }

        /// <summary>
        /// return the max one, null if the tree is empty.
        /// which runs at O(lgn).
        /// </summary>
        /// <returns></returns>
        public Tuple<K, V> Maximum()
        {
            return Maximum(root);
        }

        /// <summary>
        /// return the max one, null if the tree is empty
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Tuple<K, V> Maximum(TreapNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            TreapNode<K, V> result = node;

            while (result.Right != null)
            {
                result = result.Right;
            }

            return new Tuple<K, V>(result.Key, result.Value);
        }

        /// <summary>
        /// return the min one , null if the tree is empty
        /// runs at O(lgn) which is the height of tree
        /// </summary>
        /// <returns></returns>
        public TreapNode<K, V> Minimum_()
        {
            return Minimum_(root);
        }

        /// <summary>
        /// return the min one, null if the tree is empty
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private TreapNode<K, V> Minimum_(TreapNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            TreapNode<K, V> result = node;

            while (result.Left != null)
            {
                result = result.Left;
            }

            return result;
        }

        /// <summary>
        /// return the max one, null if the tree is empty.
        /// which runs at O(lgn).
        /// </summary>
        /// <returns></returns>
        public TreapNode<K, V> Maximum_()
        {
            return Maximum_(root);
        }

        /// <summary>
        /// return the max one, null if the tree is empty
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private TreapNode<K, V> Maximum_(TreapNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            TreapNode<K, V> result = node;

            while (result.Right != null)
            {
                result = result.Right;
            }

            return result;
        }

        #endregion

        #region Successor  predecessor

        /// <summary>
        /// find successor which runs at O(lgn)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreapNode<K, V> Successor_(TreapNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            if (node.Right != null)
            {
                return Minimum_(node.Right);
            }

            TreapNode<K, V> parent = node.Parent;

            while (parent != null && node == parent.Right)
            {
                node = parent;
                parent = parent.Parent;
            }

            return parent;
        }

        /// <summary>
        /// find predecessor which runs at O(lgn)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreapNode<K, V> Predecessor_(TreapNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            if (node.Left != null)
            {
                return Maximum_(node.Left);
            }

            TreapNode<K, V> parent = node.Parent;
            while (parent != null && node == parent.Left)
            {
                node = parent;
                parent = parent.Parent;
            }

            return parent;

        }

        /// <summary>
        /// find successor which runs at O(lgn)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Tuple<K, V> Successor(TreapNode<K, V> node)
        {
            TreapNode<K, V> result = Successor_(node);
            return result == null ? null : new Tuple<K, V>(result.Key, result.Value);
        }

        /// <summary>
        /// find predecessor which runs at O(lgn)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Tuple<K, V> Predecessor(TreapNode<K, V> node)
        {
            TreapNode<K, V> result = Predecessor_(node);
            return result == null ? null : new Tuple<K, V>(result.Key, result.Value);
        }

        #endregion

        #region Random

        /// <summary>
        /// the random generator to generate a priority
        /// </summary>
        private Random rand = new Random();

        #endregion

        #region Insert

        /// <summary>
        /// insert a new node to treep which runs at O(lgn)
        /// insert a new node to its position. if heap matains, do nothing; it heap doesn't matains, find the first node offend heap & do the following things:
        /// 1) if the new inserted node is the left child of its parent, right rotate its parent
        /// 2) if the new inserted node it the right child of its parent,left rotate its parent.
        /// 
        /// Remeber left rotate & right rotate always matains the tree 
        /// 
        /// the tree are build as if the key arrived at the priority squence from small to large
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Insert(K key, V value)
        {
            TreapNode<K, V> inserted = new TreapNode<K, V>(key, value, rand.NextDouble());
            count++;

            if (root == null)
            {
                root = inserted;
                return;
            }
            else
            {
                Insert(root, root/*the value of this parameter doesn't matter*/, inserted);
            }
            
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="current"></param>
        /// <param name="parent"></param>
        /// <param name="inserted"></param>
        private void Insert(TreapNode<K, V> current, TreapNode<K,V> parent, TreapNode<K, V> inserted)
        {
            if(current == null)
            {
                if (parent.Key.CompareTo(inserted.Key) > 0)
                {
                    parent.Left = inserted;
                    inserted.Parent = parent;
                }
                else
                {
                    parent.Right = inserted;
                    inserted.Parent = parent;
                }
                return;
            }
            int i = current.Key.CompareTo(inserted.Key);
            if (i > 0)
            {
                Insert(current.Left, current, inserted);
                if (current.Priority > current.Left.Priority)
                {
                    RightRotate(current);
                }
            }
            else
            {
                Insert(current.Right, current, inserted);
                if (current.Priority > current.Right.Priority)
                {
                    LeftRotate(current);
                }
            }
        }

        /// <summary>
        /// right rotate matains the tree property
        /// </summary>
        /// <param name="parent"></param>
        private void RightRotate(TreapNode<K, V> parent)
        {
            TreapNode<K, V> child = parent.Left;
            parent.Left = child.Right;

            if (child.Right != null)
            {
                child.Right.Parent = parent;
            }

            child.Parent = parent.Parent;

            if (parent.Parent == null)
            {
                root = child;
            }
            else if (parent == parent.Parent.Left)
            {
                parent.Parent.Left = child;
            }
            else
            {
                parent.Parent.Right = child;
            }

            child.Right = parent;
            parent.Parent = child;
        }

        /// <summary>
        /// left rotate matains the tree property
        /// </summary>
        /// <param name="parent"></param>
        private void LeftRotate(TreapNode<K, V> parent)
        {
            TreapNode<K, V> child = parent.Right;
            parent.Right = child.Left;

            if (child.Left != null)
            {
                child.Left.Parent = parent;
            }

            child.Parent = parent.Parent;

            if (parent.Parent == null)
            {
                root = child;
            }
            else if (parent == parent.Parent.Left)
            {
                parent.Parent.Left = child;
            }
            else
            {
                parent.Parent.Right = child;
            }

            child.Left = parent;
            parent.Parent = child;
        }

        #endregion

        #region Delete

        /// <summary>
        /// delete a node from treap which runs at O(lgn)
        /// 1) find the minPriority(leftChild, rightChild),then left rorate or right rotate according to the min is right or left child
        /// 2) when the node reach to the leaf,delete it.
        /// </summary>
        /// <param name="node"></param>
        public void Delete(TreapNode<K, V> node)
        {
            count--;

            TreapNode<K, V> del = node;
            while (del.Left != null || del.Right != null)
            {
                bool needToLeftRotate = (del.Left == null) || (del.Right != null && del.Right.Priority < del.Left.Priority);//minPriority(leftChild, rightChild) = rightChild

                if (needToLeftRotate)
                {
                    LeftRotate(del);
                }
                else
                {
                    RightRotate(del);
                }

            }

            if (del.Parent == null)
            {
                //delete a root
                root = null;
            }
            else if (del == del.Parent.Left)
            {
                del.Parent.Left = null;
            }
            else
            {
                del.Parent.Right = null;
            }

        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// AVL tree is a binary search tree that is height balanced: for each node x, the heights of the left and right subtrees of x differ by at most 1.
    /// Balance factor: the height of left child minus the height of right tree only has value : 0, -1, 1
    /// 
    /// AVL tree works O(lgn) even in worse case.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class AVL<K, V> where K : IComparable<K>, IEquatable<K>
    {
        /// <summary>
        /// the number of node in avl tree
        /// </summary>
        private int count;
        /// <summary>
        /// the root of avl tree
        /// </summary>
        private AVLNode<K, V> root;


        public AVL() { }

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

        /// <summary>
        /// inorder tree walk, 
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
        /// <param name="root"></param>
        private void InorderTreeWalk(ref List<Tuple<K, V>> lists, AVLNode<K, V> node)
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
        /// inorder tree walk.
        /// which runs at O(n) 
        /// </summary>
        /// <returns></returns>
        public List<AVLNode<K, V>> InorderTreeWalk_()
        {
            List<AVLNode<K, V>> lists = new List<AVLNode<K, V>>();
            InorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// inorder tree walk
        /// which runs at O(n) 
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        public void InorderTreeWalk_(ref List<AVLNode<K, V>> lists, AVLNode<K, V> node)
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
        private void PreorderTreeWalk(ref List<Tuple<K, V>> lists, AVLNode<K, V> node)
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
        public List<AVLNode<K, V>> PreorderTreeWalk_()
        {
            List<AVLNode<K, V>> lists = new List<AVLNode<K, V>>();
            PreorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// preorder tree walk
        /// which runs at O(lgn) & which is the height of tree
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        public void PreorderTreeWalk_(ref List<AVLNode<K, V>> lists, AVLNode<K, V> node)
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
        private void PostorderTreeWalk(ref List<Tuple<K, V>> lists, AVLNode<K, V> node)
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
        public List<AVLNode<K, V>> PostorderTreeWalk_()
        {
            List<AVLNode<K, V>> lists = new List<AVLNode<K, V>>();
            PostorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// post order tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        public void PostorderTreeWalk_(ref List<AVLNode<K, V>> lists, AVLNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            PostorderTreeWalk_(ref lists, node.Left);
            PostorderTreeWalk_(ref lists, node.Right);
            lists.Add(node);
        }

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
        private Tuple<K, V> Search(K key, AVLNode<K, V> node)
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
        public AVLNode<K, V> Search_(K key)
        {
            return Search_(key, root);
        }

        /// <summary>
        /// runs at O(lgn) which is the high of the tree
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private AVLNode<K, V> Search_(K key, AVLNode<K, V> node)
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
        private Tuple<K, V> Minimum(AVLNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            AVLNode<K, V> result = node;

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
        private Tuple<K, V> Maximum(AVLNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            AVLNode<K, V> result = node;

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
        public AVLNode<K, V> Minimum_()
        {
            return Minimum_(root);
        }

        /// <summary>
        /// return the min one, null if the tree is empty
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private AVLNode<K, V> Minimum_(AVLNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            AVLNode<K, V> result = node;

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
        public AVLNode<K, V> Maximum_()
        {
            return Maximum_(root);
        }

        /// <summary>
        /// return the max one, null if the tree is empty
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private AVLNode<K, V> Maximum_(AVLNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            AVLNode<K, V> result = node;

            while (result.Right != null)
            {
                result = result.Right;
            }

            return result;
        }

        /// <summary>
        /// find successor
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public AVLNode<K, V> Successor_(AVLNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            if (node.Right != null)
            {
                return Minimum_(node.Right);
            }

            AVLNode<K, V> parent = node.Parent;

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
        public AVLNode<K, V> Predecessor_(AVLNode<K, V> node)
        {
            if (node == null)
            {
                return null;
            }

            if (node.Left != null)
            {
                return Maximum_(node.Left);
            }

            AVLNode<K, V> parent = node.Parent;
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
        public Tuple<K, V> Successor(AVLNode<K, V> node)
        {
            AVLNode<K, V> result = Successor_(node);
            return result == null ? null : new Tuple<K, V>(result.Key, result.Value);
        }

        /// <summary>
        /// find predecessor which runs at O(lgn)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Tuple<K, V> Predecessor(AVLNode<K, V> node)
        {
            AVLNode<K, V> result = Predecessor_(node);
            return result == null ? null : new Tuple<K, V>(result.Key, result.Value);
        }

        #region insert & delete
        /// <summary>
        /// insert a key value pair into avl tree
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Insert(K key, V value)
        {
            //the node need to insert into avl tree
            AVLNode<K, V> inserted = new AVLNode<K, V>(key, value);
            //the size of avl tree increase 1
            count++;
            //the parent of inserted
            AVLNode<K, V> parent = root, node = null;

            while (parent != null)
            {
                node = parent;
                if (key.CompareTo(parent.Key) < 0)
                {
                    parent = parent.Left;
                }
                else
                {
                    //if equal, we insert the new node to right side
                    parent = parent.Right;
                }
            }

            inserted.Parent = node;

            if (node == null)
            {
                //first time to insert a node to a  tree
                root = inserted;
            }
            else if (inserted.Key.CompareTo(node.Key) < 0)
            {
                node.Left = inserted;
            }
            else
            {
                node.Right = inserted;
            }

            //if a node has node child, we mark it height = 1
            inserted.H = 1;

            AVLNode<K, V> rParent = node;
            bool needToFixup = false;
            while (rParent != null)
            {
                int oldH = rParent.H;
                rParent.H = System.Math.Max(rParent.Left == null ? 0 : rParent.Left.H, rParent.Right == null ? 0 : rParent.Right.H) + 1;
                if (System.Math.Abs(rParent.H) >= 2)
                {
                    needToFixup = true;
                }
                if (oldH == rParent.H)
                {
                    // no need to do it more
                    break;
                }
                rParent = rParent.Parent;
            }

            //insert fix up
            if (needToFixup)
            {
                InsertFixup(inserted);
            }
        }

        /// <summary>
        /// Fixup avl tree after insert
        /// </summary>
        /// <param name="inserted">the new node which recently inserted into avl tree</param>
        private void InsertFixup(AVLNode<K, V> inserted)
        {
            if (inserted == null)
            {
                return;
            }

            AVLNode<K, V> parent = inserted.Parent;
            AVLNode<K, V> current = inserted;

            while (parent != null)
            {
                if (parent.BF == 2)
                {
                    //LL
                    if (current.BF == 1)
                    {
                        RightRotate(parent);
                    }
                    //LR
                    if (current.BF == -1)
                    {
                        LeftRotate(current);
                        RightRotate(parent);
                    }
                }

                if (parent.BF == -2)
                {
                    //RR
                    if (current.BF == -1)
                    {
                        LeftRotate(parent);
                    }
                    //RL
                    if (current.BF == 1)
                    {
                        RightRotate(current);
                        LeftRotate(parent);
                    }
                }

                current = parent;
                parent = parent.Parent;
            }
        }


        /// <summary>
        /// delete a node from avl tree
        /// to delete a node you should find it by using key
        /// </summary>
        /// <param name="node"></param>
        public void Delete(AVLNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }
            count--;

            //delete the node then fixup it
            AVLNode<K, V> parent = node.Parent;
            if (node.Left == null && node.Right == null)
            {
                Transplant(node, node.Right);
            }
            else if (node.Left == null)
            {
                parent = node.Right;
                Transplant(node, node.Right);
            }
            else if (node.Right == null)
            {
                parent = node.Left;
                Transplant(node, node.Left);
            }
            else
            {
                AVLNode<K, V> min = Minimum_(node.Right);
                if (min.Parent != node)
                {
                    parent = min.Right != null ? min.Right : (min.Parent.Right==null?min.Parent:min.Parent.Right);
                    Transplant(min, min.Right);
                    min.Right = node.Right;
                    min.Right.Parent = min;
                }
                else
                {
                    parent = node.Left;
                }
                Transplant(node, min);
                min.Left = node.Left;
                min.Left.Parent = min;
            }

            //parent is the deepest node get affected by delete 
            AVLNode<K, V> rParent = parent;
            while (rParent != null)
            {
                rParent.H = System.Math.Max(rParent.Left == null ? 0 : rParent.Left.H, rParent.Right == null ? 0 : rParent.Right.H) + 1;
                rParent = rParent.Parent;
            }


            InsertFixup(parent);
        }

        /// <summary>
        /// When TRANSPLANT replaces the subtree rooted at node u with
        /// the subtree rooted at node v , node u’s parent becomes node v’s parent, and u’s parent ends up having v as its appropriate child.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        private void Transplant(AVLNode<K, V> u, AVLNode<K, V> v)
        {
            if (u.Parent == null)
            {
                root = v;
            }
            else if (u == u.Parent.Left)
            {
                u.Parent.Left = v;
            }
            else
            {
                u.Parent.Right = v;
            }
            if (v != null)
            {
                v.Parent = u.Parent;
            }
        }
        /// <summary>
        /// left rotate which runs at O(1)
        /// </summary>
        /// <param name="node">node must have non null right child</param>
        private void LeftRotate(AVLNode<K, V> node)
        {
            AVLNode<K, V> right = node.Right;
            node.Right = right.Left;
            if (right.Left != null)
            {
                right.Left.Parent = node;
            }
            node.H = System.Math.Max(node.Left == null ? 0 : node.Left.H, node.Right == null ? 0 : node.Right.H) + 1;
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

            right.H = System.Math.Max(right.Left == null ? 0 : right.Left.H, right.Right == null ? 0 : right.Right.H) + 1;
        }

        /// <summary>
        /// right rotate which runs at O(1)
        /// </summary>
        /// <param name="node">node must have non null left child</param>
        private void RightRotate(AVLNode<K, V> node)
        {
            AVLNode<K, V> left = node.Left;
            node.Left = left.Right;
            if (left.Right != null)
            {
                left.Right.Parent = node;
            }

            node.H = System.Math.Max(node.Left == null ? 0 : node.Left.H, node.Right == null ? 0 : node.Right.H) + 1;
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
            left.H = System.Math.Max(left.Left == null ? 0 : left.Left.H, left.Right == null ? 0 : left.Right.H) + 1;
        }
        #endregion
    }
}

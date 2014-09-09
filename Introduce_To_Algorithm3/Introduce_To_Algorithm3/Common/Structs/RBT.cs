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
    /// 
    /// we alreay maintain the size attribute
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"> </typeparam>
    public class RBT<K, V> where K : IComparable<K>, IEquatable<K>
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
        protected RBTreeNode<K, V> root;

        #endregion

        #region TreeWalk

        /// <summary>
        /// inorder tree walk, O(n)  return a sorted result
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
        private void InorderTreeWalk(ref List<Tuple<K, V>> lists, RBTreeNode<K, V> root)
        {
            if (root == null)
            {
                return;
            }

            InorderTreeWalk(ref lists, root.Left);
            lists.Add(new Tuple<K, V>(root.Key, root.Value));
            InorderTreeWalk(ref lists, root.Right);
        }


        /// <summary>
        /// inorder tree walk, O(n)  return a sorted result
        /// </summary>
        /// <returns></returns>
        public List<RBTreeNode<K,V>> InorderTreeWalk_()
        {
            List<RBTreeNode<K, V>> lists = new List<RBTreeNode<K, V>>();
            InorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// inorder tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="root"></param>
        private void InorderTreeWalk_(ref List<RBTreeNode<K, V>> lists, RBTreeNode<K, V> root)
        {
            if (root == null)
            {
                return;
            }

            InorderTreeWalk_(ref lists, root.Left);
            lists.Add(root);
            InorderTreeWalk_(ref lists, root.Right);
        }

        /// <summary>
        /// pre order walk tree O(n)
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
        /// <param name="root"></param>
        private void PreorderTreeWalk(ref List<Tuple<K, V>> lists, RBTreeNode<K, V> root)
        {
            if (root == null)
            {
                return;
            }

            lists.Add(new Tuple<K, V>(root.Key, root.Value));
            PreorderTreeWalk(ref lists, root.Left);
            PreorderTreeWalk(ref lists, root.Right);
        }

        /// <summary>
        /// pre order walk tree O(n)
        /// </summary>
        /// <returns></returns>
        public List<RBTreeNode<K,V>> PreorderTreeWalk_()
        {
            List<RBTreeNode<K, V>> lists = new List<RBTreeNode<K, V>>();
            PreorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// pre order walk tree
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="root"></param>
        private void PreorderTreeWalk_(ref List<RBTreeNode<K, V>> lists, RBTreeNode<K, V> root)
        {
            if (root == null)
            {
                return;
            }

            lists.Add(root);
            PreorderTreeWalk_(ref lists, root.Left);
            PreorderTreeWalk_(ref lists, root.Right);
        }

        /// <summary>
        /// post order walk tree runs at O(n)
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
        /// <param name="treeNode"></param>
        private void PostorderTreeWalk(ref List<Tuple<K, V>> lists, RBTreeNode<K, V> treeNode)
        {
            if (root == null)
            {
                return;
            }

            PostorderTreeWalk(ref lists, root.Left);
            PostorderTreeWalk(ref lists, root.Right);
            lists.Add(new Tuple<K, V>(root.Key, root.Value));
        }

        /// <summary>
        /// post order walk tree runs at O(n)
        /// </summary>
        /// <returns></returns>
        public List<RBTreeNode<K,V>> PostorderTreeWalk_()
        {
            List<RBTreeNode<K, V>> lists = new List<RBTreeNode<K, V>>();
            PostorderTreeWalk_(ref lists, root);
            return lists;
        }

        /// <summary>
        /// post order walk tree
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="treeNode"></param>
        private void PostorderTreeWalk_(ref List<RBTreeNode<K, V>> lists, RBTreeNode<K, V> treeNode)
        {
            if (root == null)
            {
                return;
            }

            PostorderTreeWalk_(ref lists, root.Left);
            PostorderTreeWalk_(ref lists, root.Right);
            lists.Add(root);
        }

        #endregion

        #region Search & Min & Max & Successor & Precedessor

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
            if (root == null)
            {
                return null;
            }

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
            if (root == null)
            {
                return null;
            }

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
            if (root == null)
            {
                return null;
            }

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
            if (root == null)
            {
                return null;
            }

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
            if (root == null)
            {
                return null;
            }

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
            if (root == null)
            {
                return null;
            }

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
            if (root == null)
            {
                return null;
            }

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
            if (root == null)
            {
                return null;
            }

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

        #endregion

        #region Insert & Delete

        /// <summary>
        /// delete the node
        /// </summary>
        /// <param name="node"></param>
        public void Delete(RBTreeNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            count--;

            //node x that moves into node y’s original position.
            //node y as the node either removed from the tree or moved within the tree.
            //parent is the path where size need to minus 1
            RBTreeNode<K, V> y = node, x = null, parent = null;
            Color yOriginColor = y.Color;
            if (node.Left == null)
            {
                x = node.Right;
                parent = node.Parent;
                Transplant(node, node.Right);
            }
            else if (node.Right == null)
            {
                x = node.Left;
                parent = node.Parent;
                Transplant(node, node.Left);
            }
            else
            {
                y = Minimum_(node.Right);
                yOriginColor = y.Color;
                x = y.Right;
                if (y.Parent == node)
                {
                    parent = y;
                    y.Size = node.Size;
                }
                else
                {
                    //the post one is not right the right one
                    parent = y.Parent;
                    y.Size = node.Size;
                    Transplant(y, y.Right);
                    //because the post one is not the right child, then node.right is not null
                    y.Right = node.Right;
                    y.Right.Parent = y;
                }

                Transplant(node, y);
                y.Left = node.Left;
                //node.left is not null
                y.Left.Parent = y;
                y.Color = node.Color;
            }

            RBTreeNode<K, V> pNode = parent;
            while (pNode != null)
            {
                pNode.Size--;
                pNode = pNode.Parent;
            }


            if (yOriginColor == Color.BLACK)
            {
                // x can be null. if y originColor red, no violation of rbt
                // node x moves into node y’s original position
                //we can sure here parent is the parent of x ,but because x can be null, we need to use parent as a parameter
                DeleteFixUp(x, parent);
            }
            //if yOriginColor is red, then x is null and y is leaf node
        }

        /// <summary>
        /// x must have at most one child 
        /// 1)if the root deleted, then a red (must be red or it have none children) child be new root
        /// 2)all x's path black height less 1
        /// 3)the father and x node may be both red
        /// </summary>
        /// <param name="x"></param>
        /// <param name="parent"></param>
        private void DeleteFixUp(RBTreeNode<K, V> x, RBTreeNode<K, V> parent)
        {
            RBTreeNode<K, V> w = null;
            while (x != root && IsBlack(x))
            {
                //if x is not root then the parent is not null
                // x is double black & x can be null
                if (x == parent.Left)
                {
                    //since node x is double black, node w can not be null, because the number of blacks on simple path from x.p to the leaf w would be smaller than the the number on simple path from x.p to x.
                    w = parent.Right;
                    if (IsRed(w))
                    {
                        w.Color = Color.BLACK;
                        parent.Color = Color.RED;
                        LeftRotate(parent);
                        w = parent.Right;
                        //w still can not be null here
                    }

                    if (w != null && IsBlack(w.Left) && IsBlack(w.Right))
                    {
                        w.Color = Color.RED;
                        x = parent;
                        parent = parent.Parent;
                    }
                    else if (w != null)
                    {
                        if (IsBlack(w.Right))
                        { 
                            //w.left is red 
                            if (w.Left != null) w.Left.Color = Color.BLACK;
                            w.Color = Color.RED;
                            RightRotate(w);
                            w = parent.Right;
                        }
                        if (w != null)
                        {
                            w.Color = parent.Color;
                            parent.Color = Color.BLACK;
                            if (w.Right != null)
                            {
                                w.Right.Color = Color.BLACK;
                            }
                            LeftRotate(parent);
                            x = root;
                        }
                    }
                }
                else
                {
                    w = parent.Left;
                    if (IsRed(w))
                    {
                        w.Color = Color.BLACK;
                        parent.Color = Color.RED;
                        RightRotate(parent);
                        w = parent.Left;
                    }
                    if (w != null && IsBlack(w.Left) && IsBlack(w.Right))
                    {
                        w.Color = Color.RED;
                        x = parent;
                        parent = parent.Parent;
                    }
                    else if (w != null)
                    {
                        if (IsBlack(w.Left))
                        {
                            if (w.Right != null) w.Right.Color = Color.BLACK;
                            w.Color = Color.RED;
                            LeftRotate(w);
                            w = parent.Left;
                        }
                        if (w != null)
                        {
                            w.Color = parent.Color;
                            parent.Color = Color.BLACK;
                            if (w.Left != null)
                            {
                                w.Left.Color = Color.BLACK;
                            }
                            RightRotate(parent);
                            //Setting x to be the root causes the while loop to terminate when it tests the loop condition.
                            x = root;
                        }
                    }
                }
            }
            //if x origin color is red,then color black, which add 1 black at the path
            if (x != null)
                x.Color = Color.BLACK;
        }

        private bool IsBlack(RBTreeNode<K, V> rBTreeNode)
        {
            return rBTreeNode == null || rBTreeNode.Color == Color.BLACK;
        }
        private bool IsRed(RBTreeNode<K, V> rBTreeNode)
        {
            return !IsBlack(rBTreeNode);
        }

        /// <summary>
        /// When TRANSPLANT replaces the subtree rooted at node u with
        /// the subtree rooted at node v , node u’s parent becomes node v’s parent, and u’s parent ends up having v as its appropriate child.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        private void Transplant(RBTreeNode<K, V> u, RBTreeNode<K, V> v)
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
        /// Red Black insert
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key, V val)
        {
            RBTreeNode<K, V> node = null, inserted = new RBTreeNode<K, V>(key, val);
            count++;
            RBTreeNode<K, V> x = root;
            while (x != null)
            {
                node = x;
                node.Size++;
                if (key.CompareTo(x.Key) < 0)
                {
                    x = x.Left;
                }
                else
                {
                    x = x.Right;
                }
            }

            inserted.Parent = node;

            //first insert a node
            if (node == null)
            {
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

            inserted.Left = inserted.Right = null;
            inserted.Color = Color.RED;
            inserted.Size = 1;
            //fix up the insert node, so red black tree reserve
            InsertFixup(inserted);
        }

        /// <summary>
        /// fix up the insert node, so red black tree reserve
        /// </summary>
        /// <param name="inserted"></param>
        private void InsertFixup(RBTreeNode<K, V> inserted)
        {
            //if inserted.Parent == null, then inserted is root
            //if inserted.Parent.Color == RED,then must have inserted.Parent.Parent !=null. Because root.Color is BLACK
            while (inserted.Parent != null && inserted.Parent.Color == Color.RED)
            {
                //at the beginning of the loop, three invariant are keep
                //1)node inserted is red
                //2)if node.parent is the root, then inserted.parent is black
                //3)if inserted is root, then inserted.parent is null. 
                if (inserted.Parent == inserted.Parent.Parent.Left)
                {
                    RBTreeNode<K, V> y = inserted.Parent.Parent.Right;
                    if (y != null && y.Color == Color.RED)
                    {
                        inserted.Parent.Color = Color.BLACK;
                        y.Color = Color.BLACK;
                        inserted.Parent.Parent.Color = Color.RED;
                        inserted = inserted.Parent.Parent;
                    }
                    else
                    {
                        if (inserted == inserted.Parent.Right)
                        {
                            inserted = inserted.Parent;
                            LeftRotate(inserted);
                        }
                        inserted.Parent.Color = Color.BLACK;
                        inserted.Parent.Parent.Color = Color.RED;
                        RightRotate(inserted.Parent.Parent);
                    }

                }
                else
                {
                    RBTreeNode<K, V> y = inserted.Parent.Parent.Left;
                    if (y != null && y.Color == Color.RED)
                    {
                        inserted.Parent.Color = Color.BLACK;
                        y.Color = Color.BLACK;
                        inserted.Parent.Parent.Color = Color.RED;
                        inserted = inserted.Parent.Parent;
                    }
                    else
                    {
                        if (inserted == inserted.Parent.Left)
                        {
                            inserted = inserted.Parent;
                            RightRotate(inserted);
                        }
                        inserted.Parent.Color = Color.BLACK;
                        inserted.Parent.Parent.Color = Color.RED;
                        LeftRotate(inserted.Parent.Parent);
                    }
                }
            }

            root.Color = Color.BLACK;
        }


        /// <summary>
        /// left rotate 
        /// </summary>
        /// <param name="x">x must have right child</param>
        private void LeftRotate(RBTreeNode<K, V> x)
        {
            if (x.Right == null)
            {
                return;
            }

            RBTreeNode<K, V> y = x.Right;
            x.Right = y.Left;

            if (y.Left != null)
            {
                y.Left.Parent = x;
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

            y.Left = x;
            x.Parent = y;

            y.Size = x.Size;
            x.Size = Size(x.Left) + Size(x.Right) + 1;
        }



        /// <summary>
        /// right rotate 
        /// </summary>
        /// <param name="x">x must have left child</param>
        private void RightRotate(RBTreeNode<K, V> x)
        {
            if (x.Left == null)
            {
                return;
            }

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

            y.Size = x.Size;
            x.Size = Size(x.Left) + Size(x.Right) + 1;
        }

        #endregion

        #region Order

        /// <summary>
        /// select ith smallest element in inorder walk sequence, start from 1
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public RBTreeNode<K, V> Select(int i)
        {
            return Select(root, i);
        }

        /// <summary>
        /// select the ith smallest tree node in inorder tree walk sequence rooted at root.
        /// null if can't find one
        /// it runs at O(lgn)
        /// </summary>
        /// <param name="root"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private RBTreeNode<K, V> Select(RBTreeNode<K, V> root, int i)
        {
            if (root == null)
            {
                return null;
            }
            int r = 1 + (root.Left == null ? 0 : root.Left.Size);
            if (i == r)
            {
                return root;
            }
            else if (i < r)
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
        /// start from 1.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int Rank(RBTreeNode<K, V> node)
        {
            int i = Size(node.Left) + 1;
            while (node != root)
            {
                //at the start of each iteration of while loop, i is the rank of x.key in the subtree rooted at node.
                if (node == node.Parent.Right)
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
        private int Size(RBTreeNode<K, V> node)
        {
            return node == null ? 0 : node.Size;
        }

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
        private int MaxHeight(RBTreeNode<K, V> node)
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
        private int MinHeight(RBTreeNode<K, V> node)
        {
            if (node == null)
            {
                return 0;
            }

            return System.Math.Min(MinHeight(node.Left), MinHeight(node.Right)) + 1;
        }

        /// <summary>
        /// get the black height
        /// the black height defined as follow:
        /// 1)if node == null, blackHeight(node) = 0
        /// 2)all the black node number from node(included) to its leaf
        /// </summary>
        /// <returns></returns>
        public int BlackHeight()
        {
            return BlackHeight(root);
        }

        /// <summary>
        /// get the black height.
        /// the black height defined as follow:
        /// 1)if node == null, blackHeight(node) = 0
        /// 2)all the black node number from node(included) to its leaf
        /// </summary>
        /// <param name="rbTreeNode"></param>
        /// <returns></returns>
        private int BlackHeight(RBTreeNode<K, V> rbTreeNode)
        {
            int height = 0;

            while (rbTreeNode != null)
            {
                if (rbTreeNode.Color == Color.BLACK)
                {
                    height++;
                }

                rbTreeNode = rbTreeNode.Left;
            }

            return height;
        }

        #endregion

        #endregion

        #region TestUtil

        /// <summary>
        /// is the tree a real red black tree
        /// </summary>
        /// <returns></returns>
        public bool IsRealRbt()
        {
            if (root == null)
            {
                return true;
            }

            List<RBTreeNode<K,V>> list = InorderTreeWalk_();

            //root is black
            if (!IsBlack(root))
            {
                return false;
            }

            //if a node is red, his children can't be black
            foreach (RBTreeNode<K, V> rbTreeNode in list)
            {
                if (IsRed(rbTreeNode))
                {
                    if (IsRed(rbTreeNode.Left) || IsRed(rbTreeNode.Right))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// is size right
        /// </summary>
        /// <returns></returns>
        public bool IsSizeAttributeRight()
        {
            List<RBTreeNode<K, V>> list = InorderTreeWalk_();

            foreach (RBTreeNode<K, V> rbTreeNode in list)
            {
                if (Size(rbTreeNode) != Size(rbTreeNode.Left) + Size(rbTreeNode.Right) + 1)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Is count right
        /// </summary>
        /// <returns></returns>
        public bool IsCountAttributeRight()
        {
            List<RBTreeNode<K, V>> list = InorderTreeWalk_();

            return count == list.Count;
        }

        /// <summary>
        ///   is        node.left.key  &lt;=  node.key  &lt;= node.right.key 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsTreeNode(RBTreeNode<K,V> node)
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
        /// is it a tree
        /// </summary>
        /// <returns></returns>
        public bool IsTree()
        {
            List<RBTreeNode<K, V>> list = InorderTreeWalk_();

            foreach (RBTreeNode<K, V> node in list)
            {
                if (!IsTreeNode(node))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}

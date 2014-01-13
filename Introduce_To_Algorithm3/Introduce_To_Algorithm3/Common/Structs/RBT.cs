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
    public class RBT<K, V> where K : IComparable<K>, IEquatable<K>
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
        /// delete the node
        /// </summary>
        /// <param name="node"></param>
        public void Delete(RBTreeNode<K, V> node)
        {
            if (node == null) return;
            count--;

            //node x that moves into node y’s original position.
            //node y as the node either removed from the tree or moved within the tree.
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
                // x can be null, if red, no violation of rbt
                // node x moves into node y’s original position
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
                // x is double black
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



    }
}

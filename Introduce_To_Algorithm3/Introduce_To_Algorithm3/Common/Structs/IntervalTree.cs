using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// we choose a red black tree in which each node x contains an interval x.interval and the key of x is low endpoint x.interval.Low.
    /// </summary>
    public class IntervalTree
    {
        #region Member
        /// <summary>
        /// the root of the tree
        /// </summary>
        private IntervalTreeNode root;

        private int count;

        /// <summary>
        /// return the node of element in the tree
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        #endregion

        #region Insert & Delete is the the as rbt
        /// <summary>
        /// Red Black insert. adds the element x, who has interval attribute.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(Interval item)
        {
            Debug.Assert(item != null);
            IntervalTreeNode node = null,inserted = new IntervalTreeNode(item);
            count++;
            IntervalTreeNode x = root;

            while (x != null)
            {
                node = x;
                x = item.Low.CompareTo(node.Interval.Low) < 0 ? x.Left : x.Right;
            }

            inserted.Parent = node;

            //first insert a node
            if (node == null)
            {
                root = inserted;
            }
            else if (inserted.Interval.Low < node.Interval.Low)
            {
                node.Left = inserted;
            }
            else
            {
                node.Right = inserted;
            }

            inserted.Left = inserted.Right = null;
            inserted.Color = Color.RED;

            //fix up the insert node, so red black tree reserve
            InsertFixup(inserted);
        }


        /// <summary>
        /// fix up the insert node, so red black tree reserve
        /// </summary>
        /// <param name="inserted"></param>
        private void InsertFixup(IntervalTreeNode inserted)
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
                    IntervalTreeNode y = inserted.Parent.Parent.Right;
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
                    IntervalTreeNode y = inserted.Parent.Parent.Left;
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
        private void LeftRotate(IntervalTreeNode x)
        {
            IntervalTreeNode y = x.Right;
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
        }



        /// <summary>
        /// right rotate 
        /// </summary>
        /// <param name="x">x must have left child</param>
        private void RightRotate(IntervalTreeNode x)
        {
            IntervalTreeNode y = x.Left;
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

        #endregion 

        #region Search

        /// <summary>
        /// return a node which overlaps item
        /// returns a pointer to an element x in the interval tree T such that x:int overlaps interval i , or a pointer to the sentinel T:nil if no such element
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IntervalTreeNode Search(Interval item)
        {
            IntervalTreeNode x = root;
            while (x != null && !x.Interval.Overlap(item))
            {
                if(x.Left != null&& x.Left.Max >= item.Low)
                {
                    x = x.Left;
                }
                else
                {
                    x = x.Right;
                }
            }

            return x;
        }

        #endregion

    }
}

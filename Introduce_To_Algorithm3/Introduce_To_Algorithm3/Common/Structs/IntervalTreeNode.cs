using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{

    public class IntervalTreeNode
    {
        /// <summary>
        /// the key is Interval.Low
        /// </summary>
        public Interval Interval { get; set; }


        public IntervalTreeNode() { }


        public IntervalTreeNode(Interval item)
        {
            this.Interval = item;
        }


        /// <summary>
        /// the maximum value of any interval endpoint stored in the subtree rooted at x
        /// x.max = max( interval.high, x.left.max, x.right.max)
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// x.interval.low as key
        /// </summary>
        public int Key
        {
            get
            {
                return Interval.Key;
            }
        }

        public IntervalTreeNode Parent { get; set; }
        public IntervalTreeNode Left { get; set; }
        public IntervalTreeNode Right { get; set; }
        public Color Color;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// directed edge
    /// </summary>
    public class DirectedEdge:IComparable<DirectedEdge>
    {
        /// <summary>
        /// the directed from vertex
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// the directed to vertex
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// the weight of edge
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// create an edge from from to to with given weight
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="weight"></param>
        public DirectedEdge(int from, int to, double weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        /// <summary>
        /// override tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}--->{1} : {2}",From,To,Weight);
        }

        /// <summary>
        /// override GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        /// <summary>
        /// implement IComparable&lt;DirectedEdge&gt; interface
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(DirectedEdge other)
        {
            return Weight > other.Weight ? 1 : Weight < other.Weight ? -1 : 0;
        }
    }
}

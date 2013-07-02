using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// directed edge
    /// </summary>
    public class DirectedEdge
    {
        private readonly int v;

        private readonly int w;

        private readonly double weight;

        /// <summary>
        /// create an edge from v to w with given weight
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="weight"></param>
        public DirectedEdge(int start, int end, double weight)
        {
            this.v = start;
            this.w = end;
            this.weight = weight;
        }

        /// <summary>
        /// Return the vertex where this edge begins.
        /// </summary>
        /// <returns></returns>
        public int From()
        {
            return v;
        }

        /// <summary>
        /// Return the vertex where this edge ends.
        /// </summary>
        /// <returns></returns>
        public int To()
        {
            return w;
        }


        /// <summary>
        /// return the weight of this edge
        /// </summary>
        /// <returns></returns>
        public double Weight()
        {
            return weight;
        }


        public override string ToString()
        {
            return string.Format("{0}-->{1} , {2}", v, w, weight);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// undirected edge
    /// </summary>
    public class Edge : IComparable<Edge>
    {
        private readonly int v;

        private readonly int w;

        private readonly double weight;

        /// <summary>
        /// create an edge between v and w with given weight
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <param name="weight"></param>
        public Edge(int v, int w, double weight)
        {
            this.v = v;
            this.w = w;
            this.weight = weight;
        }

        /// <summary>
        /// return the weight of this edge
        /// </summary>
        /// <returns></returns>
        public double Weight()
        {
            return weight;
        }

        public int V()
        {
            return v;
        }

        public int W()
        {
            return w;
        }


    /// <summary>
        /// return either endpoint of this edge
        /// </summary>
        /// <returns></returns>
        public int Either()
        {
            return v;
        }

        /// <summary>
        /// Return the endpoint of this edge that is different from the given vertex
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public int Other(int vertex)
        {
            if (vertex == v) return w;
            if (vertex == w)
            {
                return v;
            }

            throw  new ArgumentException("illegal endpoint");
        }

        /// <summary>
        /// compare edges by weight
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Edge other)
        {
            if (this.weight < other.weight) return -1;
            else if (this.weight > other.weight) return 1;
            else
            {
                return 0;
            }
        }

    }
}

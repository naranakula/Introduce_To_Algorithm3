using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// single source short path problem: give a source s , is there a short path from s to any other vertex v.
    /// assume we have V vertices and all vertices is connected from s, then the shortest path must construct a V-1 edge tree and the root is s.
    /// assume there is a shortest path from s to v (v0,v1,v2,......vn). then the path (vi...vj) in this shortest path is also shortest path from vi to vj.
    /// </summary>
    public class SingleSourceShortPath
    {
        /// <summary>
        /// edgeTo[v] = last edge from s to v
        /// </summary>
        protected DirectedEdge[] edgeTo;

        /// <summary>
        /// distTo[v] = the weight from s to v
        /// </summary>
        protected double[] distTo;

        /// <summary>
        /// the single source
        /// </summary>
        private int source;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g"></param>
        /// <param name="source"></param>
        public SingleSourceShortPath(EdgeWeightedDigraph g,int source)
        {
            this.source = source;
            edgeTo = new DirectedEdge[g.V];
            distTo = new double[g.V];
            for (int i = 0; i < g.V; i++)
            {
                distTo[i] = double.PositiveInfinity;
            }

            distTo[source] = 0;
        }

        /// <summary>
        /// if p = (v0,v1,...,vk) is shortest path from s =v0 to vk, we relax the edges of p in order (v0,v1) (v1,v2)  (vk-1,vk), then we can get the distTo
        /// </summary>
        /// <param name="e"></param>
        protected virtual void Relax(DirectedEdge e)
        {
            if (distTo[e.To] > distTo[e.From] + e.Weight)
            {
                distTo[e.To] = distTo[e.From] + e.Weight;
                edgeTo[e.To] = e;
            }
        }

        /// <summary>
        /// is there a path from s to v
        /// there is always a path from s to s
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public virtual bool IsHavePathTo(int v)
        {
            if (source == v)
            {
                return true;
            }
            return edgeTo[v] != null;
        }

        /// <summary>
        /// the path from s to v
        /// the path from s to s is emtpy
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public virtual Stack<DirectedEdge> PathTo(int v)
        {
            if (!IsHavePathTo(v))
            {
                return null;
            }

            Stack<DirectedEdge> stack = new Stack<DirectedEdge>();
            while (edgeTo[v] != null)
            {
                stack.Push(edgeTo[v]);
                v = edgeTo[v].From;
            }
            return stack;
        }

        /// <summary>
        /// the shortest distance from s to v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public virtual double DistTo(int v)
        {
            return distTo[v];
        }
    }
}
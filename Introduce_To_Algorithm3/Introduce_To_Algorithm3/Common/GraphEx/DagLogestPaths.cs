using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    public class DagLogestPaths
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
        public DagLogestPaths(EdgeWeightedDigraph g, int source)
        {
            Init(g,source);

            TopologicalSort topological = new TopologicalSort(g);

            List<int> list = topological.GetTopoSort();

            foreach (var i in list)
            {
                foreach (DirectedEdge e in g.Adj(i))
                {
                    Relax(e);
                }
            }
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="g"></param>
        /// <param name="source"></param>
        protected void Init(EdgeWeightedDigraph g, int source)
        {
            this.source = source;
            edgeTo = new DirectedEdge[g.V];
            distTo = new double[g.V];
            for (int i = 0; i < g.V; i++)
            {
                distTo[i] = double.NegativeInfinity;
            }

            distTo[source] = 0;
        }

        protected void Relax(DirectedEdge e)
        {
            if (distTo[e.To] < distTo[e.From] + e.Weight)
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

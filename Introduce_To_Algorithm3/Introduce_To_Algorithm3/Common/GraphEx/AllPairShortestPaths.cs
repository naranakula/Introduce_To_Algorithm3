using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// given a EdgeWeightedDigraph, find the shortest path from v to w for each (v,w) in E.
    /// </summary>
    public class AllPairShortestPaths
    {
        /// <summary>
        /// weights[i,j] = the weights from i to j
        /// </summary>
        protected double[,] weights;

        /// <summary>
        /// hasPathTo[i,j] = whether or not has path from i to j
        /// </summary>
        protected bool[,] hasPathTo;

        /// <summary>
        /// pathTo[i,j] = the path from i to j, null if no such path
        /// </summary>
        protected List<DirectedEdge>[,] pathTo; 

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g"></param>
        public AllPairShortestPaths(EdgeWeightedDigraph g)
        {
            int n = g.V;
            weights = new double[n,n];
            hasPathTo = new bool[n,n];
            pathTo = new List<DirectedEdge>[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    weights[i, j] = double.PositiveInfinity;
                }
            }
        }

        /// <summary>
        /// is there a path from ---> to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public virtual bool HasPathTo(int from, int to)
        {
            return hasPathTo[from, to];
        }

        /// <summary>
        /// the weight of path from ---> to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public virtual double Weight(int from, int to)
        {
            return weights[from, to];
        }

        /// <summary>
        /// the path from ---> to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public virtual List<DirectedEdge> PathTo(int from, int to)
        {
            return pathTo[from, to];
        }

    }
}

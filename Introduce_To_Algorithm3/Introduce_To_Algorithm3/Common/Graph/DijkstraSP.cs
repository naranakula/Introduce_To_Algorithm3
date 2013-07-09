using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Structs;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// 
    /// </summary>
    public class DijkstraSP
    {
        /// <summary>
        /// distTo[v] = distance of shortest s-->v path
        /// </summary>
        private double[] distTo;

        /// <summary>
        /// edgeTo[v] = last edge on shortest s-->v path
        /// </summary>
        private DirectedEdge[] edgeTo;

        /// <summary>
        /// priority queue of vertices
        /// </summary>
        private IndexMinPQ<double> pq;


        public DijkstraSP(EdgeWeightedDigraph g, int s)
        {
            foreach (DirectedEdge e in g.Edges())
            {
                if(e.Weight()<0)
                    throw new ArgumentOutOfRangeException();
            }

            distTo = new double[g.V];
            edgeTo = new DirectedEdge[g.V];
            for (int v = 0; v < g.V; v++)
            {
                distTo[v] = double.PositiveInfinity;
            }
            distTo[s] = 0;

            //relax vertices in order of distance from s
            pq = new IndexMinPQ<double>(g.V);
            pq.Insert(s,distTo[s]);
            while (!pq.IsEmpty())
            {
                int v = pq.DelMin();
                foreach (DirectedEdge e in g.Adj(v))
                {
                    relax(e);
                }
            }
        }


        /// <summary>
        /// relax edge e and update pa if changed
        /// </summary>
        /// <param name="e"></param>
        private void relax(DirectedEdge e)
        {
            int v = e.From();
            int w = e.To();
            if (distTo[w] > distTo[v] + e.Weight())
            {
                
            }
        }

    }
}

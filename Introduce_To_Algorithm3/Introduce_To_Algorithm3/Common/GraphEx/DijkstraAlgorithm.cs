using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Structs;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// dijkstra's algorithm solves the single source path problem on a weighted,directed graph g=(v,e) for the case in which all edges weights are nonnegative. that is w(u,v)>=0. it run faster than bellmanford algorithm
    /// </summary>
    public class DijkstraAlgorithm:SingleSourceShortPath
    {
        public DijkstraAlgorithm(EdgeWeightedDigraph digraph, int source) : base(digraph, source)
        {
            //pq maintain a set of vertex need to deal with.
            IndexMinPQ<double> pq = new IndexMinPQ<double>(digraph.V);
            pq.Insert(source,distTo[source]);

            while (!pq.IsEmpty())
            {
                //when v pops up, the distance and path to v have been confirmed
                int v = pq.DelMin();
                foreach (DirectedEdge e in digraph.Adj(v))
                {
                    Relax(pq,e);
                }
            }
        }


        protected void Relax(IndexMinPQ<double> pq,DirectedEdge e)
        {
            int v = e.From;
            int w = e.To;

            if (distTo[w] > distTo[v] + e.Weight)
            {
                distTo[w] = distTo[v] + e.Weight;
                edgeTo[w] = e;
                if (pq.Contains(w))
                {
                    pq.DecreaseKey(w,distTo[w]);
                }
                else
                {
                    //this will never used
                    pq.Insert(w,distTo[w]);
                }
            }
        }

    }
}

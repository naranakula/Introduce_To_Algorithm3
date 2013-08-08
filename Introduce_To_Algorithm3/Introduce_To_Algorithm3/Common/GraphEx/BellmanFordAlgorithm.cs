using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// Bellman-Ford algorithm solves the single-source shortest-paths problem in the general case in which edge  weights may be negative.
    /// it also check whether or not there is a negative weight cycle that is reachalbe from source
    /// it runs at O(VE)
    /// </summary>
    public class BellmanFordAlgorithm:SingleSourceShortPath
    {

        public bool IsNegativeCycle { get; private set; }

        /// <summary>
        /// constructor
        /// for any vertex v reachable from s, the path must at most v-1 edges.
        /// we can get (vi-1,vi) in sequence because if p = (v0,v1,...,vk) is shortest path from s =v0 to vk, we relax the edges of p in order (v0,v1) (v1,v2)  (vk-1,vk), then we can get the distTo
        /// <param name="g"></param>
        /// <param name="source"></param>
        public BellmanFordAlgorithm(EdgeWeightedDigraph g, int source):base(g,source)
        {
            List<DirectedEdge> edges = g.Edges();
            for (int i = 0; i < g.V-1; i++)
            {
                foreach (DirectedEdge e in edges)
                {
                    base.Relax(e);
                }
            }

            //assume this is negative cycle (v0,v1,...vk) vk=v0. then there must be at least one distTo[e.To] < distTo[e.From] + e.Weight
            if (edges.Any(e => distTo[e.To] > distTo[e.From] + e.Weight))
            {
                IsNegativeCycle = true;
                throw new Exception("there is negative cycle");
            }
        }
    }
}

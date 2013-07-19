using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// this only works on directed acyclic graph.
    /// if dag contains a path from u to v, then u precedes v in the topological sort.
    /// we make just one pass over the vertices in the topological sort.
    /// it runs at O(v+e)
    /// </summary>
    public class DagShortestPaths:SingleSourceShortPath
    {
        public DagShortestPaths(EdgeWeightedDigraph g, int source) : base(g, source)
        {
            TopologicalSort topological = new TopologicalSort(g);
            List<int> list = topological.GetTopoSort();
            foreach (var i in list)
            {
                foreach (DirectedEdge e in g.Adj(i))
                {
                    base.Relax(e);
                }
            }
        }
    }
}

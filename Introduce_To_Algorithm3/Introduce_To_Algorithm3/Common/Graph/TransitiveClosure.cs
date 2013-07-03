using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// The transitive closure of a digraph G is another digraph with the same set of vertices, but with an edge from v to w if and only if w is reachable from v in G.
    /// </summary>
    public class TransitiveClosure
    {
        /// <summary>
        /// tc[v] = reachable from v
        /// </summary>
        private DirectedDFS[] tc;

        public TransitiveClosure(Digraph G)
        {
            tc = new DirectedDFS[G.V];
            for (int v = 0; v < G.V; v++)
                tc[v] = new DirectedDFS(G, v);
        }

        public bool Reachable(int v, int w)
        {
            return tc[v].IsHavePath(w);
        }
    }
}

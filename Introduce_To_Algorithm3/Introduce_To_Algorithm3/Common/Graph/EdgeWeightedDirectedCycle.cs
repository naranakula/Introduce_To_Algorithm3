using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    public class EdgeWeightedDirectedCycle
    {
        private bool[] marked;           // marked[v] = has vertex v been marked?
        private DirectedEdge[] edgeTo;        // edgeTo[v] = previous edge on path to v
        private bool[] onStack;            // onStack[v] = is vertex on the stack?
        private Stack<DirectedEdge> cycle;    // directed cycle (or null if no such cycle)

        public EdgeWeightedDirectedCycle(EdgeWeightedDigraph g)
        {
            marked = new bool[g.V];
            onStack = new bool[g.V];
            edgeTo = new DirectedEdge[g.V];

            for (int v = 0; v < g.V; v++)
            {
                if (!marked[v])
                    dfs(g, v);
            }
        }


        /// <summary>
        /// check that algorithm computes either the topological order or finds a directed cycle
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void dfs(EdgeWeightedDigraph g, int v)
        {
            onStack[v] = true;
            marked[v] = true;
            foreach (DirectedEdge e in g.Adj(v))
            {
                int w = e.To();

                //short circuit if directed cycle found
                if (cycle != null) return;
                else if (!marked[v]) //found new vertex
                {
                    edgeTo[w] = e;
                    dfs(g,w);
                }
                // trace back directed cycle
                else if (onStack[w])
                {
                    cycle = new Stack<DirectedEdge>();
                    DirectedEdge de = e;
                    while (de.From() != w)
                    {
                        cycle.Push(de);
                        de = edgeTo[de.From()];
                    }
                    cycle.Push(de);
                }
            }
            onStack[v] = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasCycle()
        {
            return cycle != null;
        }


        public IEnumerable<DirectedEdge> Cycle()
        {
            return cycle;
        }

    }
}

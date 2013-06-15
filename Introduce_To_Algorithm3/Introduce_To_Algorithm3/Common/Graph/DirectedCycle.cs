using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// cycle for directed graph 
    /// </summary>
    public class DirectedCycle
    {
        /// <summary>
        /// marked[v] = has vertex v been marked
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// edgeTo[v] = previous vertex on path to v
        /// </summary>
        private int[] edgeTo;

        /// <summary>
        /// onStack[v] = is vertex on the stack
        /// </summary>
        private bool[] onStack;

        /// <summary>
        /// directed cycle if null, then no such cycle
        /// </summary>
        private Stack<int> cycle;



        public DirectedCycle(Digraph g)
        {
            marked = new bool[g.V()];
            onStack = new bool[g.V()];
            edgeTo = new int[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                if (!marked[v])
                    dfs(g, v);
            }
        }


        /// <summary>
        ///  check that algorithm computes either the topological order or finds a directed cycle
        /// </summary>
        /// <param name="G"></param>
        /// <param name="v"></param>
        private void dfs(Digraph G, int v)
        {
            onStack[v] = true;
            marked[v] = true;
            foreach (int w in G.Adj(v))
            {
                //short circuit if directed cycle found
                if (cycle != null) return;
                else if (!marked[w])
                {
                    edgeTo[w] = v;
                    dfs(G, w);
                }
                //trace back directed cycle
                else if (onStack[w])
                {
                    cycle = new Stack<int>();
                    for (int x = v; x != w; x = edgeTo[x])
                    {
                        cycle.Push(x);
                    }
                    cycle.Push(w);
                    cycle.Push(v);
                }
            }
            onStack[v] = false;
        }

        public bool HasCycle() { return cycle != null; }
        public IEnumerable<int> Cycle() { return cycle; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// is a graph bipartite
    /// </summary>
    public class Bipartite
    {
        /// <summary>
        /// is the graph bipartite
        /// </summary>
        private bool isBipartite;

        /// <summary>
        /// color[v] gives vertices on one side of bipartition
        /// </summary>
        private bool[] color;

        /// <summary>
        /// marked[v] = true if v has been visited in dfs
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// edgeTo[v] = last edge on path to v
        /// </summary>
        private int[] edgeTo;

        /// <summary>
        /// odd length cycle。 if the graph not bipartite,then it must have odd length cycle
        /// </summary>
        private Stack<int> cycle;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g"></param>
        public Bipartite(Graph g)
        {
            isBipartite = true;
            color = new bool[g.V];
            marked = new bool[g.V];
            edgeTo = new int[g.V];

            for (int v = 0; v < g.V; v++)
            {
                if (!marked[v])
                {
                    dfs(g, v);
                }
            }
        }

        /// <summary>
        /// dfs
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void dfs(Graph g, int v)
        {
            marked[v] = true;
            foreach (int w in g.Adj(v))
            {
                if(cycle != null) return;

                if (!marked[w])
                {
                    edgeTo[w] = v;
                    color[w] = !color[v];
                    dfs(g, w);
                }
                // if the graph not bipartite,then it must have odd length cycle
                else if (color[w] == color[v])
                {
                    isBipartite = false;
                    cycle = new Stack<int>();
                    cycle.Push(w);
                    for (int i = v; i != w; i = edgeTo[i])
                    {
                        cycle.Push(i);
                    }
                    cycle.Push(w);
                }
            }
        }

        /// <summary>
        /// is graph bipartite
        /// </summary>
        /// <returns></returns>
        public bool IsBipartite()
        {
            return isBipartite;
        }

        /// <summary>
        /// if the graph not bipartite,then it must have odd length cycle
        /// </summary>
        /// <returns></returns>
        public Stack<int> OddCycle()
        {
            return cycle;
        }
    }
}

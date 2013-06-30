using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// An articulation vertex (or cut vertex) is a vertex whose removal increases the number of connected components. A graph is biconnected if it has no articulation vertices. 
    /// </summary>
    public class Biconnected
    {
        private int[] low;
        private int[] pre;
        private int cnt;
        private bool[] articulation;

        public Biconnected(Graph g)
        {
            low = new int[g.V];
            pre = new int[g.V];
            articulation = new bool[g.V];

            for (int i = 0; i < g.V; i++)
            {
                low[i] = -1;
                pre[i] = -1;
            }

            for (int v = 0; v < g.V; v++)
            {
                if (pre[v] == -1)
                {
                    dfs(g, v, v);
                }
            }
        }


        private void dfs(Graph g, int u, int v)
        {
            int children = 0;
            pre[v] = cnt++;
            low[v] = pre[v];
            foreach (int w in g.Adj(v))
            {
                if (pre[w] == -1)
                {
                    children++;
                    dfs(g, v, w);

                    //update low number
                    low[v] = System.Math.Min(low[v], low[w]);

                    //non-root of dfs is an articulation point
                    if (low[w] >= pre[v] && u != v)
                    {
                        articulation[v] = true;
                    }
                }
                // update low number - ignore reverse of edge leading to v because it was calculated
                else if (w != u)
                {
                    low[v] = System.Math.Min(low[v], low[w]);
                }
            }

            // root of DFS is an articulation point if it has more than 1 child
            if (u == v && children > 1)
                articulation[v] = true;
        }

        /// <summary>
        ///  is vertex v an articulation point?
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool IsArticulation(int v) { return articulation[v]; }


    }
}

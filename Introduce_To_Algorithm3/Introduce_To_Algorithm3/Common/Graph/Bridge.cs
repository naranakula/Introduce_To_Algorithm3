using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// a edge is bridge whose deletion increases the number of connected components. 
    /// </summary>
    public class Bridge
    {
        /// <summary>
        /// number of bridge
        /// </summary>
        private int bridges;

        /// <summary>
        /// counter
        /// </summary>
        private int cnt;

        /// <summary>
        /// pre[v] = order in which dfs examines v
        /// </summary>
        private int[] pre;

        /// <summary>
        /// low[v] = lowest predorder of any vertex conected to v
        /// </summary>
        private int[] low;


        public Bridge(Graph g)
        {
            low = new int[g.V];
            pre = new int[g.V];
            for (int i = 0; i < low.Length; i++)
            {
                low[i] = -1;
            }
            for (int i = 0; i < pre.Length; i++)
            {
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

        /// <summary>
        /// get the number of components
        /// </summary>
        /// <returns></returns>
        public int Components()
        {
            return bridges + 1;
        }


        private void dfs(Graph g, int bef, int cur)
        {
            pre[cur] = cnt++;
            low[cur] = pre[cur];
            foreach (int w in g.Adj(cur))
            {
                //pre[w] not checked
                if (pre[w] == -1)
                {
                    dfs(g,cur,w);
                    low[cur] = System.Math.Min(low[cur], low[w]);
                    if (low[w] == pre[w])
                    {
                        bridges++;
                    }
                }
                // update low number - ignore reverse of edge leading to v
                else if (w != bef)
                {
                    low[cur] = System.Math.Min(low[cur], pre[w]);
                }
            }
        }

        /// <summary>
        /// get the number of bridges
        /// </summary>
        /// <returns></returns>
        public int Bridges()
        {
            return bridges;
        }
    }
}

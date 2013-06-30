using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// Is a graph can be paint by only two color
    /// and every edge 
    /// </summary>
    public class TwoColor
    {
        /// <summary>
        /// marked[v] = vertex v have been marked
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// color[v] = true, 
        /// </summary>
        private bool[] color;

        /// <summary>
        /// is the graph can be paint only by two color
        /// </summary>
        private bool isTwoColorable = true;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g"></param>
        public TwoColor(Graph g)
        {
            marked = new bool[g.V];
            color = new bool[g.V];
            for (int s = 0; s < g.V; s++)
            {
                if (!isTwoColorable)
                {
                    return;
                }
                if (!marked[s])
                    dfs(g, s);
            }
        }


        private void dfs(Graph g, int v)
        {
            marked[v] = true;
            foreach (int w in g.Adj(v))
            {
                if(!marked[w])
                {
                    color[w] = !color[v];
                    dfs(g,w);
                }
                else if(color[w] == color[v])
                {
                    isTwoColorable = false;
                    break;
                }
            }
        }

        /// <summary>
        /// is twocolorable
        /// </summary>
        /// <returns></returns>
        public bool IsBipartite()
        {
            return isTwoColorable;
        }

    }
}

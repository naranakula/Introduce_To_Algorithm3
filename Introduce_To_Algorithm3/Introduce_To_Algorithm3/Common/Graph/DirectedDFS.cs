using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    public class DirectedDFS
    {
        /// <summary>
        /// marked[v] = true if v is reachable
        /// </summary>
        private bool[] marked;


        public DirectedDFS(Digraph g, int s)
        {
            marked = new bool[g.V()];
            dfs(g, s);
        }


        /// <summary>
        /// multi sources
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sources"></param>
        public DirectedDFS(Digraph g, IEnumerable<int> sources)
        {
            marked = new bool[g.V()];
            foreach (int v in sources)
            {
                dfs(g, v);
            }
        }


        private void dfs(Digraph g, int v)
        {
            marked[v] = true;
            foreach (int w in g.Adj(v))
            {
                if (!marked[w])
                {
                    dfs(g, w);
                }
            }
        }

        /// <summary>
        ///  is there a directed path from the source (or sources) to v?
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool IsHavePath(int v)
        {
            return marked[v];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// find strong connected
    /// </summary>
    public class StrongConnected
    {
        private List<HashSet<int>> sc;
        private bool[] marked;

        public StrongConnected(DiGraph g)
        {
            DepthFirstSearchs dfo = new DepthFirstSearchs(g);
            marked = new bool[g.V];
            sc = new List<HashSet<int>>();
            g = g.Reverse();
            foreach (int v in dfo.ReversePostOrder())
            {
                if (!marked[v])
                {
                    sc.Add(new HashSet<int>());
                    dfs(g, v);
                }
            }
        }



        private void dfs(DiGraph g, int v)
        {
            marked[v] = true;
            sc.Last().Add(v);
            foreach (int w in g.Adj(v))
            {
                if (!marked[w])
                {
                    dfs(g,w);
                }
            }
        }


        public int Count()
        {
            return sc.Count;
        }

        public List<HashSet<int>> GetSc()
        {
            return sc;
        }
    }
}

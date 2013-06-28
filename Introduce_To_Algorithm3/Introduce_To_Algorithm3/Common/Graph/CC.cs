using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// connected component
    /// </summary>
    public class CC
    {
        /// <summary>
        /// marked[v] = have vertex v been marked
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// id[v] = id of connected component containing v
        /// </summary>
        private int[] id;

        /// <summary>
        /// size[id] = number of vertices in given component
        /// </summary>
        private int[] size;

        /// <summary>
        /// number of connected components
        /// </summary>
        private int count;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g"></param>
        public CC(Graph g)
        {
            marked = new bool[g.V];
            size = new int[g.V];
            for (int i = 0; i < g.V; i++)
            {
                if (!marked[i])
                {
                    dfs(g, i);
                    count++;
                }
            }
        }

        /// <summary>
        /// depth first search
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void dfs(Graph g, int v)
        {
            marked[v] = true;
            id[v] = count;
            size[count]++;
            foreach (int w in g.Adj(v))
            {
                if (!marked[w])
                {
                    dfs(g,w);
                }
            }
        }

        /// <summary>
        /// id of connected component containingv
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int Id(int v)
        {
            return id[v];
        }

        /// <summary>
        /// number of connected components
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return count;
        }

        /// <summary>
        /// is w and v connected
        /// </summary>
        /// <param name="w"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool IsConnected(int w, int v)
        {
            return id[v] == id[w];
        }

        /// <summary>
        /// get the components
        /// </summary>
        /// <returns></returns>
        public List<HashSet<int>> Components()
        {
            List<HashSet<int>> lists = new List<HashSet<int>>();
            for (int i = 0; i < count; i++)
            {
                lists.Add(new HashSet<int>());
            }

            for (int i = 0; i < marked.Length; i++)
            {
                lists[id[i]].Add(i);
            }
            return lists;
        }

    }
}

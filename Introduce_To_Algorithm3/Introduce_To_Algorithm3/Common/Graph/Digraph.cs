using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// directed graph
    /// </summary>
    public class Digraph
    {
        /// <summary>
        /// number of vertices
        /// </summary>
        private readonly int v;

        /// <summary>
        /// the number of edges
        /// </summary>
        private int e;

        /// <summary>
        /// adj[i] represents edges begin the ith vertex    i------->adj[i]
        /// </summary> 
        private List<int>[] adj;

        /// <summary>
        /// create a empty graph with v vertices
        /// </summary>
        /// <param name="v"></param>
        public Digraph(int v)
        {
            if (v <= 0)
            {
                throw new Exception("number of vertices must be postive");
            }

            this.v = v;
            this.e = 0;
            adj = new List<int>[v];
            for (int i = 0; i < adj.Length; i++)
            {
                adj[i] = new List<int>();
            }
        }

        /// <summary>
        /// return the number of vertices
        /// </summary>
        /// <returns></returns>
        public int V()
        {
            return v;
        }

        /// <summary>
        /// return the number of edge
        /// </summary>
        /// <returns></returns>
        public int E()
        {
            return e;
        }

        /// <summary>
        /// add a edge from v to w  v----->w
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        public void AddEdge(int v, int w)
        {
            adj[v].Add(w);
            e++;
        }

        /// <summary>
        ///  Return the list of vertices pointed from vertex v 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public List<int> Adj(int i)
        {
            return adj[i];
        }

        /// <summary>
        /// return a reverse version of digraph
        /// </summary>
        /// <returns></returns>
        public Digraph Reverse()
        {
            Digraph R = new Digraph(v);
            for (int i = 0; i < v; i++)
            {
                foreach (int w in Adj(i))
                {
                    R.AddEdge(w, i);
                }
            }
            return R;
        }
    }
}

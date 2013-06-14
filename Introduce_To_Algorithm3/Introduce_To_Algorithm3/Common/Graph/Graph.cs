using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// a undirected graph implementation, using adjacenct list
    /// using [0,... ...,V-1]to represent V vertex
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// number of vertex
        /// </summary>
        private int v;
        /// <summary>
        /// number of edge
        /// </summary>
        private int e;

        /// <summary>
        /// the index i adj[i] represents all vertices connected to vertex i
        /// </summary>
        private List<int>[] adj;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public Graph(int v)
        {
            this.v = v;
            e = 0;
            adj = new List<int>[v];
            for (int i = 0; i < adj.Length; i++)
            {
                adj[i] = new List<int>();
            }
        }

        /// <summary>
        /// the number of vertices
        /// </summary>
        public int V { get { return v; } }

        /// <summary>
        /// the number of edge
        /// </summary>
        public int E { get { return e; } }


        /// <summary>
        /// return the vertices connected to v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public List<int> Adj(int v)
        {
            return adj[v];
        }


        /// <summary>
        /// add a edge
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        public void AddEdge(int v, int w)
        {
            e++;
            adj[v].Add(w);
            adj[w].Add(v);
        }
    }
}

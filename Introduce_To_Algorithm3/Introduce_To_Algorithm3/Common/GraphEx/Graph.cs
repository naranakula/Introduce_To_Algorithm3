using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// a adjacent list implementation of undirected graph
    /// we represent vertex from 0,1,.....,V
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// the number of vertices
        /// </summary>
        private int v;

        /// <summary>
        /// the number of edges
        /// </summary>
        private int e;

        /// <summary>
        /// the number of vertices
        /// </summary>
        public int V { get { return v; } }

        /// <summary>
        /// the number of edges
        /// </summary>
        public int E { get { return e; } }

        /// <summary>
        /// adj[i] represents there is a edge from i<------>adj[i]
        /// </summary>
        private List<int>[] adj;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="v">the number of vertices</param>
        public Graph(int v)
        {
            this.v = v;
            adj = new List<int>[v];
            for (int i = 0; i < adj.Length; i++)
            {
                adj[i] = new List<int>();
            }
        }

        /// <summary>
        /// get a list vertices for each w in list that v <-------> w
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public List<int> Adj(int v)
        {
            return adj[v];
        }

        /// <summary>
        /// add a edge  i-----j
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void AddEdge(int i, int j)
        {
            e++;
            adj[i].Add(j);
            adj[j].Add(i);
        }
    }
}

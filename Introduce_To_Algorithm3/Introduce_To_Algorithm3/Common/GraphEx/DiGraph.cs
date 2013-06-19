using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// a adjacent list implement of directed graph
    /// </summary>
    public class DiGraph
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
        /// adj[i] represent there is a edge from i ----> adj[i]
        /// </summary>
        public List<int>[] adjs;

        /// <summary>
        /// the number of vertices
        /// </summary>
        public int V { get { return v; } }

        /// <summary>
        /// the number of edges
        /// </summary>
        public int E { get { return e; } }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="v">the number of vertices</param>
        public DiGraph(int v)
        {
            this.v = v;
            adjs = new List<int>[v];
            for (int i = 0; i < adjs.Length; i++)
            {
                adjs[i] = new List<int>();
            }
        }

        /// <summary>
        /// add a edge from-------->to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void AddEdge(int from, int to)
        {
            e++;
            adjs[from].Add(to);
        }


        /// <summary>
        ///  Return the list of vertices pointed from vertex i
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public List<int> Adj(int i)
        {
            return adjs[i];
        }

        /// <summary>
        /// the reverse version
        /// </summary>
        /// <returns></returns>
        public DiGraph Reverse()
        {
            DiGraph graph = new DiGraph(v);
            for (int i = 0; i < adjs.Length; i++)
            {
                for (int j = 0; j < adjs[i].Count; j++)
                {
                    graph.AddEdge(j,i);
                }
            }
            return graph;
        }
    }
}

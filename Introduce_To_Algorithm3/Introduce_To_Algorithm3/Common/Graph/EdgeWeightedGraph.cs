using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    public class EdgeWeightedGraph
    {
        /// <summary>
        /// number of vertices
        /// </summary>
        private readonly int v;

        /// <summary>
        /// number of edges
        /// </summary>
        private int e;

        /// <summary>
        /// adj[v] = all edge  from v to adj[v] undirectly
        /// </summary>
        private List<Edge>[] adj;

        /// <summary>
        /// create an empty edge-weighted graph with v vertices
        /// </summary>
        /// <param name="v"></param>
        public EdgeWeightedGraph(int v)
        {
            if (v < 0) throw new ArgumentException("Number of vertices in a Graph must be nonnegative");
            this.v = v;
            this.e = 0;
            adj = new List<Edge>[v];
            for (int i = 0; i < v; i++)
            {
                adj[i] = new List<Edge>();
            }
        }

        /// <summary>
        /// return the number of vertices in this graph
        /// </summary>
        public int V
        {
            get { return v; }
        }

        /// <summary>
        /// return the number of edges in this graph
        /// </summary>
        public int E
        {
            get { return e; }
        }

        /// <summary>
        /// add 
        /// </summary>
        /// <param name="e"></param>
        public void AddEdge(Edge e)
        {
            int v = e.Either();
            int w = e.Other(v);
            adj[v].Add(e);
            adj[w].Add(e);
            (this.e)++;
        }

        /// <summary>
        /// Return the edges incident to vertex v as an Iterable.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public List<Edge> Adj(int v)
        {
            return adj[v];
        }

        /// <summary>
        /// Return all edges in this graph as an Iterable.
        /// </summary>
        /// <returns></returns>
        public List<Edge> Edges()
        {
            List<Edge> list = new List<Edge>();
            for (int v = 0; v < this.v; v++)
            {
                int selfLoops = 0;
                foreach (Edge edge in Adj(v))
                {
                    if (edge.Other(v) > v)
                    {
                        list.Add(edge);
                    }
                    else if(edge.Other(v) == v)
                    {
                        if(selfLoops % 2 == 0) list.Add(edge);
                        selfLoops++;
                    }
                }
            }

            return list;
        }
    }
}

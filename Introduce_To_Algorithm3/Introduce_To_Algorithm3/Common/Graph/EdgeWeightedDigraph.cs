using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    public class EdgeWeightedDigraph
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
        /// adj[v] = all edge  from v to adj[v] directly
        /// </summary>
        private List<DirectedEdge>[] adj;

        /// <summary>
        /// Create an empty edge-weighted digraph with V vertices.
        /// </summary>
        /// <param name="v"></param>
        public EdgeWeightedDigraph(int v)
        {
            if (v < 0) throw new ArgumentException("Number of vertices in a Digraph must be nonnegative");

            this.v = v;
            this.e = 0;
            adj = new List<DirectedEdge>[v];
            for (int i = 0; i < v; i++)
            {
                adj[i] = new List<DirectedEdge>();
            }
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="g"></param>
        public EdgeWeightedDigraph(EdgeWeightedDigraph g):this(g.V)
        {
            this.e = g.e;

            for (int v = 0; v < g.V; v++)
            {
                // reverse so that adjacency list is in same order as original
                Stack<DirectedEdge> reverse = new Stack<DirectedEdge>();
                foreach (DirectedEdge directedEdge in g.adj[v])
                {
                    reverse.Push(directedEdge);
                }

                foreach (DirectedEdge directedEdge in reverse)
                {
                    adj[v].Add(directedEdge);
                }
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
        /// add directed edge
        /// </summary>
        /// <param name="e"></param>
        public void AddEdge(DirectedEdge e)
        {
            adj[e.From()].Add(e);
            (this.e)++;
        }

        /// <summary>
        /// Return the edges incident from vertex v as an Iterable.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public List<DirectedEdge> Adj(int v)
        {
            return adj[v];
        }

        /// <summary>
        /// Return all edges in this digraph as an Iterable.
        /// </summary>
        /// <returns></returns>
        public List<DirectedEdge> Edges()
        {
            List<DirectedEdge> list = new List<DirectedEdge>();

            for (int v = 0; v < this.v; v++)
            {
                list.AddRange(adj[v]);
            }
            return list;
        }

    }
}

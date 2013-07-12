using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// Edge weighted diected graph
    /// </summary>
    public class EdgeWeightedDigraph:ICloneable
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
        /// adj[v] = all edge from v to adj[v] directly
        /// </summary>
        private List<DirectedEdge>[] adj;


        /// <summary>
        /// constructor
        /// create an empty edge-weighted digraph with numOfVertices
        /// </summary>
        /// <param name="numOfVertices"></param>
        public EdgeWeightedDigraph(int numOfVertices)
        {
            this.v = numOfVertices;
            this.e = 0;
            adj = new List<DirectedEdge>[numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                adj[i] = new List<DirectedEdge>();
            }
        }

        /// <summary>
        /// return number of vertices in this graph
        /// </summary>
        public int V
        {
            get { return v; }
        }

        /// <summary>
        /// return number of edges in this graph
        /// </summary>
        public int E
        {
            get { return e; }
        }

        /// <summary>
        /// add a directed edge
        /// </summary>
        /// <param name="e"></param>
        public void AddEdge(DirectedEdge edge)
        {
            adj[edge.From].Add(edge);
            (this.e)++;
        }

        /// <summary>
        /// return the edge incident from v 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public List<DirectedEdge> Adj(int v)
        {
            return adj[v];
        }

        /// <summary>
        /// return the edge in graph
        /// </summary>
        /// <returns></returns>
        public List<DirectedEdge> Edges()
        {
            List<DirectedEdge> result = new List<DirectedEdge>();
            for (int i = 0; i < adj.Length; i++)
            {
                for (int j = 0; j < adj[i].Count; j++)
                {
                    result.Add(adj[i][j]);
                }
            }

            return result;
        }


        /// <summary>
        /// implement deep ICloneable interface
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return Copy();
        }

        /// <summary>
        /// depth copy
        /// </summary>
        /// <returns></returns>
        public EdgeWeightedDigraph Copy()
        {
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(v);
            g.e = e;
            for (int i = 0; i < adj.Length; i++)
            {
                foreach (DirectedEdge edge in adj[i])
                {
                    g.adj[edge.From].Add(new DirectedEdge(edge.From,edge.To,edge.Weight));
                }
            }

            return g;
        }

        /// <summary>
        /// reverse graph
        /// </summary>
        /// <returns></returns>
        public EdgeWeightedDigraph Reverse()
        {
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(v);
            g.e = e;
            for (int i = 0; i < adj.Length; i++)
            {
                foreach (DirectedEdge edge in adj[i])
                {
                    g.adj[edge.To].Add(new DirectedEdge(edge.To, edge.From, edge.Weight));
                }
            }

            return g;
        }

    }
}

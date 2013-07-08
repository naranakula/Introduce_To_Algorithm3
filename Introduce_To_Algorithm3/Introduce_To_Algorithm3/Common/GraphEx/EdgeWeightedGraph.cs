using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// edge weighted undirected graph
    /// </summary>
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
        /// adjs[v] = all edge connect v to adjs[v] undirectly
        /// </summary>
        private List<Edge>[] adjs;

        /// <summary>
        /// create an empty edge-weighted graph with v vertices
        /// </summary>
        /// <param name="v"></param>
        public EdgeWeightedGraph(int v)
        {
            if(v<0)
                throw new ArgumentOutOfRangeException("Number of vertices in a graph must be nonnegative");

            this.v = v;
            this.e = 0;
            adjs = new List<Edge>[v];
            for (int i = 0; i < v; i++)
            {
                adjs[i] = new List<Edge>();
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
        /// the number of edges in graph
        /// </summary>
        public int E
        {
            get { return e; }
        }

        /// <summary>
        /// add an edge
        /// </summary>
        /// <param name="e"></param>
        public void AddEdge(Edge e)
        {
            int v = e.Either();
            int w = e.Other(v);
            adjs[v].Add(e);
            adjs[w].Add(e);
            this.e++;
        }

        /// <summary>
        /// return edges connect with v vertex
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public List<Edge> Adj(int v)
        {
            return adjs[v];
        }

        /// <summary>
        /// return all edges in graph
        /// </summary>
        /// <returns></returns>
        public List<Edge> Edges()
        {
            List<Edge> list = new List<Edge>();
            for (int v = 0; v < this.v; v++)
            {
                int selfLoops = 0;
                foreach (Edge edge in adjs[v])
                {
                    if (edge.Other(v) > v)
                    {
                        list.Add(edge);
                    }
                    else if(edge.Other(v) == v)
                    {
                        if (selfLoops%2 == 0)
                        {
                            list.Add(edge);
                        }
                        selfLoops++;
                    }
                }
            }

            return list;
        }

    }
}

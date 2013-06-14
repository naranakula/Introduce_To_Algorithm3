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
    public class UnDirectedGraph
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
        /// vrtices
        /// </summary>
        private List<UnDirectedVertex> vertices;


        public UnDirectedGraph(int v)
        {
            this.v = v;
            this.e = 0;
            vertices = new List<UnDirectedVertex>(v);
            for (int i = 0; i < v; i++)
            {
                vertices.Add(new UnDirectedVertex(i));
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
        public List<UnDirectedVertex> Adj(int v)
        {
            return vertices[v].AdjList;
        }

        /// <summary>
        /// add a edge
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        public void AddEdge(int v, int w)
        {
            e++;
            vertices[v].AdjList.Add(vertices[w]);
            vertices[w].AdjList.Add(vertices[v]);
        }

    }
}

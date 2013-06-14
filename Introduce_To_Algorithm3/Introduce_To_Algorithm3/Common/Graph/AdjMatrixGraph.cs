using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// a adjacent matrix graph represetation using v*v matrix
    /// </summary>
    public class AdjMatrixGraph
    {
        /// <summary>
        /// number of vertices
        /// </summary>
        private int v;

        /// <summary>
        /// number of edges
        /// </summary>
        private int e;

        /// <summary>
        /// if adj[i,j] and adj[j,i] true, then i and j connected
        /// </summary>
        private Boolean[,] adj;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="v"></param>
        public AdjMatrixGraph(int v)
        {
            this.v = v;
            this.e = 0;
            adj = new bool[v,v];
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
            if (!adj[v, w]) e++;
            adj[v, w] = true;
            adj[w, v] = true;
        }

        /// <summary>
        /// contains a edge v---->w
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool Contains(int v, int w)
        {
            return adj[v,w];
        }

        /// <summary>
        ///  Return the list of vertices pointed from vertex v 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public List<int> Adj(int i)
        {
            List<int> list = new List<int>();
            for (int j = 0; j < v; j++)
            {
                if(adj[i,j])
                    list.Add(j);
            }
            return list.Distinct().ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    public class AdjMatrixGraph
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
        /// if adjs[i,j] = true, the vertice i and j are connected
        /// </summary>
        private bool[,] adjs;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="v">the number of vertices</param>
        public AdjMatrixGraph(int v)
        {
            this.v = v;
            adjs = new bool[v,v];
        }

        /// <summary>
        /// add a edge from w to v
        /// </summary>
        /// <param name="w"></param>
        /// <param name="v"></param>
        public void AddEdge(int w, int v)
        {
            if (!adjs[w, v]) e++;
            adjs[w, v] = true;
            adjs[v, w] = true;
        }


        /// <summary>
        /// contains a edge v----w
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool Contains(int v, int w)
        {
            if (v == w) return true;
            return adjs[v, w];
        }

        /// <summary>
        /// get a list vertices for each w in list that i <-------> w
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public List<int> Adj(int i)
        {
            List<int> list = new List<int>();
            for (int j = 0; j < v; j++)
            {
                if(adjs[i,j])
                    list.Add(j);
            }
            return list;
        }

    }

}

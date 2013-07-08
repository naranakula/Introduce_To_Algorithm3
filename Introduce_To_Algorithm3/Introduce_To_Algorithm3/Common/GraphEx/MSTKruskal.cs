using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// kruskal's algorithm to find mst.
    /// kruskal sort all edges in increasing order. find the minest edge doesn't build mst and add it to mst
    ///  
    /// </summary>
    public class MSTKruskal
    {
        /// <summary>
        /// id[i] = the id of set which contains vertex i 
        /// </summary>
        private int[] id;

        /// <summary>
        /// sz[i] = the number of vertices in set which contains vertex i
        /// </summary>
        private int[] sz;

        /// <summary>
        /// edge in mst
        /// </summary>
        private List<Edge> mst;

        /// <summary>
        /// the weight of mst
        /// </summary>
        private double weight;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g"></param>
        public MSTKruskal(EdgeWeightedGraph g)
        {
            int size = g.V;
            mst = new List<Edge>(size);
            id = new int[size];
            sz = new int[size];
            for (int i = 0; i < size; i++)
            {
                id[i] = i;
                sz[i] = 1;
            }

            List<Edge> edges = g.Edges();
            //sort the edges into nondecreasing order by weight
            edges.Sort();

            foreach (Edge edge in edges)
            {
                int v = edge.Either();
                int w = edge.Other(v);
                //v and w is not connected
                if (FindSet(v) != FindSet(w))
                {
                    mst.Add(edge);
                    weight += edge.Weight();
                    if (mst.Count == size - 1)
                    {
                        //already find mst
                        break;
                    }
                    Union(v, w);
                }
            }

        }


        /// <summary>
        /// find the set id of vertex v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private int FindSet(int v)
        {
            while (v != id[v])
            {
                v = id[v];
            }
            return v;
        }

        /// <summary>
        /// union the set which contains v and the set contains w
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        private void Union(int v, int w)
        {
            int i = FindSet(v);
            int j = FindSet(w);
            if (i == j)
            {
                return;
            }

            if (sz[i] < sz[j])
            {
                id[i] = j;
                sz[j] += sz[i];
            }
            else
            {
                id[j] = i;
                sz[i] += sz[j];
            }
        }

        /// <summary>
        /// return the weight of mst
        /// </summary>
        /// <returns></returns>
        public double Weight()
        {
            return weight;
        }

        /// <summary>
        /// return the mst
        /// </summary>
        /// <returns></returns>
        public List<Edge> Mst()
        {
            return mst;
        }
    }
}

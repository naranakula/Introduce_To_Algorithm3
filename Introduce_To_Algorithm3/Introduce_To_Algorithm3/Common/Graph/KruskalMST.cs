using System;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Structs;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// kruskal's algorithm sort edge in increasing order.
    /// deal with each one edge, if the edge add to mst doesn't form a cycle, add to mst
    /// Kruskal's algorithm computes the MST of any connected edge-weighted graph with E edges and V vertices using extra space proportional to E and time proportional to E log E (in the worst case).
    /// </summary>
    public class KruskalMST
    {
        /// <summary>
        /// weight of mst
        /// </summary>
        private double weight;

        /// <summary>
        /// edges in mst
        /// </summary>
        private System.Collections.Generic.Queue<Edge> mst = new System.Collections.Generic.Queue<Edge>(); 

        /// <summary>
        /// kruskal's algorithm
        /// </summary>
        /// <param name="g"></param>
        public KruskalMST(EdgeWeightedGraph g)
        {
            //more efficient to build heap by passing array of edges
            MinHeap<Edge> pq = new MinHeap<Edge>();
            foreach (Edge e in g.Edges())
            {
                pq.Insert(e);
            }

            //rum greedy algorithm
            UF uf = new UF(g.V);
            while (!pq.IsEmpty() && mst.Count<g.V-1)
            {
                Edge e = pq.Pop();
                int v = e.Either();
                int w = e.Other(v);
                if (!uf.IsConnected(v, w))
                {
                    //v-w does not create a cycle
                    uf.Union(v,w);
                    mst.Enqueue(e);
                    weight += e.Weight();
                }
            }
        }

        /// <summary>
        /// edges in minimum spanning forest 
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.Queue<Edge> Edges()
        {
            return mst;
        }

        /// <summary>
        /// weight of minimum spanning forest
        /// </summary>
        /// <returns></returns>
        public double Weight()
        {
            return weight;
        }

        /// <summary>
        /// check optimality conditions (takes time proportional EVlgV)
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public bool Check(EdgeWeightedGraph g)
        {
            //check total weight
            double total = 0.0;

            foreach (var edge in Edges())
            {
                total += edge.Weight();
            }

            double EPSILON = 1E-12;
            if (System.Math.Abs(total - Weight()) > EPSILON)
            {
                //weight of edges must equal to weight()
                return false;
            }

            //check that it is acyclic
            UF uf = new UF(g.V);
            foreach (var edge in Edges())
            {
                int v = edge.Either();
                int w = edge.Other(v);
                if (uf.IsConnected(v, w))
                {
                    return false;
                }
                uf.Union(w,v);
            }

            //check that it is a spanning forest
            foreach (var edge in Edges())
            {
                int v = edge.Either();
                int w = edge.Other(v);
                if (!uf.IsConnected(w, v))
                {
                    return false;
                }
            }

            //check that is is a minimal spanning forest(cut optimality conditions0
            foreach (var edge in Edges())
            {
                int v = edge.Either();
                int w = edge.Other(v);

                //all edges in mst except e
                uf = new UF(g.V);
                foreach (var f in mst)
                {
                    int x = f.Either();
                    int y = f.Other(x);
                    if (f != edge)
                    {
                        uf.Union(x,y);
                    }
                }

                foreach (var f in g.Edges())
                {
                    int x = f.Either();
                    int y = f.Other(x);

                    if (!uf.IsConnected(x, y))
                    {
                        if (f.Weight() < edge.Weight())
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

    }
}

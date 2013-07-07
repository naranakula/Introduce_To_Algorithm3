using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Structs;


namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// uses space proportional to V and time proportional to E log V (in the worst case).
    /// </summary>
    public class PrimMST
    {
        /// <summary>
        /// edgeTo[v] = shortest edge from tree vertex to non tree verte
        /// </summary>
        private Edge[] edgeTo;

        /// <summary>
        /// distTo[v] = weidht of shortest such edge
        /// </summary>
        private double[] distTo;

        /// <summary>
        /// marked[v] = true if v on tree, false otherwise
        /// </summary>
        private bool[] marked;

        private IndexMinPQ<double> pq;


        public PrimMST(EdgeWeightedGraph g)
        {
            edgeTo = new Edge[g.V];
            distTo = new double[g.V];
            marked = new bool[g.V];
            pq = new IndexMinPQ<double>(g.V);
            for (int v = 0; v < g.V; v++)
            {
                distTo[v] = double.PositiveInfinity;
            }

            for (int v = 0; v < g.V; v++)
            {
                if (!marked[v])
                    prim(g, v);
            }
        }

        private void prim(EdgeWeightedGraph g, int s)
        {
            distTo[s] = 0.0;
            pq.Insert(s,distTo[s]);
            while (!pq.IsEmpty())
            {
                int v = pq.DelMin();
                scan(g, v);
            }
        }


        private void scan(EdgeWeightedGraph g, int v)
        {
            marked[v] = true;
            foreach (Edge e in g.Adj(v))
            {
                int w = e.Other(v);
                if(marked[w])
                    continue;//v-w is obsolete edge
                if (e.Weight() < distTo[w])
                {
                    distTo[w] = e.Weight();
                    edgeTo[w] = e;
                    if (pq.Contains(w))
                    {
                        pq.ChangeKey(w,distTo[w]);
                    }
                    else
                    {
                        pq.Insert(w,distTo[w]);
                    }
                }
            }
        }

        /// <summary>
        /// return edges in mst
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.Queue<Edge> Edges()
        {
            System.Collections.Generic.Queue<Edge> mst = new System.Collections.Generic.Queue<Edge>();
            for (int v = 0; v < edgeTo.Length; v++)
            {
                Edge e = edgeTo[v];
                if (e != null)
                {
                    mst.Enqueue(e);
                }
            }
            return mst;
        }

        /// <summary>
        /// return weight of mst
        /// </summary>
        /// <returns></returns>
        public double Weight()
        {
            double weight = 0.0;
            foreach (Edge e in Edges())
            {
                weight += e.Weight();
            }
            return weight;
        }

    }
}

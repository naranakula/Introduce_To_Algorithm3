using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Structs;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// a crossing edge is an edge that connects a vertex in one set vith a vertex in the other. the crossing edge of minimum weight is in the mst of the graph
    /// e lazy version of Prim's algorithm uses space proportional to E and time proportional to E log E (in the worst case) to compute the MST of a connected edge-weighted graph with E edges and V vertices; 
    /// </summary>
    public class LazyPrimMST
    {
        /// <summary>
        /// total weight of MST
        /// </summary>
        private double weight;

        /// <summary>
        /// edges in the mst
        /// </summary>
        private System.Collections.Generic.Queue<Edge> mst;

        /// <summary>
        /// marked[v] = true if v on tree
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// edges with one endpoint in tree
        /// </summary>
        private MinHeap<Edge> pq;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g"></param>
        public LazyPrimMST(EdgeWeightedGraph g)
        {
            mst = new System.Collections.Generic.Queue<Edge>();
            pq = new MinHeap<Edge>();
            marked = new bool[g.V];
            for (int v = 0; v < g.V; v++)
            {
                if (!marked[v])
                    prim(g, v);
            }
        }

        /// <summary>
        /// run prim's algorithm
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        private void prim(EdgeWeightedGraph g, int s)
        {
            scan(g, s);
            while (!pq.IsEmpty() && mst.Count()<g.V-1)//better to stop when mst has v-1 edge
            {
                Edge e = pq.Pop();
                int v = e.Either();
                int w = e.Other(v);
                if(marked[v] &&marked[w])
                    continue;//lazy, both v and w already scanned
                mst.Enqueue(e);
                weight += e.Weight();
                if(!marked[v])
                    scan(g,v);//v becomes part of tree

                if(!marked[w])
                    scan(g,w);//w becomes part of tree
            }
        }

        /// <summary>
        /// add all edges e incident to v onto pq if the other endpoint has not yet been scanned
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void scan(EdgeWeightedGraph g, int v)
        {
            System.Diagnostics.Debug.Assert(!marked[v]);
            marked[v] = true;
            foreach (Edge edge in g.Adj(v))
            {
                if(!marked[edge.Other(v)])
                    pq.Insert(edge);
            }
        }

        /// <summary>
        /// return edges in mst
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Edge> Edges()
        {
            return mst;
        }

        /// <summary>
        /// return weight of mst 
        /// </summary>
        /// <returns></returns>
        public double Weight()
        {
            return weight;
        }
    }
}

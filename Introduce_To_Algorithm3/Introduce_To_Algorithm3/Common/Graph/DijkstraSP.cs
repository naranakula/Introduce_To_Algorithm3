using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Structs;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// 
    /// </summary>
    public class DijkstraSP
    {
        /// <summary>
        /// distTo[v] = distance of shortest s-->v path
        /// </summary>
        private double[] distTo;

        /// <summary>
        /// edgeTo[v] = last edge on shortest s-->v path
        /// </summary>
        private DirectedEdge[] edgeTo;

        /// <summary>
        /// priority queue of vertices
        /// </summary>
        private IndexMinPQ<double> pq;

        private const double EPSILON = 1E-12;


        public DijkstraSP(EdgeWeightedDigraph g, int s)
        {
            foreach (DirectedEdge e in g.Edges())
            {
                if(e.Weight()<0)
                    throw new ArgumentOutOfRangeException("graph edge must have nonegative weights");
            }

            distTo = new double[g.V];
            edgeTo = new DirectedEdge[g.V];
            for (int v = 0; v < g.V; v++)
            {
                distTo[v] = double.PositiveInfinity;
            }
            distTo[s] = 0;

            //relax vertices in order of distance from s
            pq = new IndexMinPQ<double>(g.V);
            pq.Insert(s,distTo[s]);
            while (!pq.IsEmpty())
            {
                int v = pq.DelMin();
                foreach (DirectedEdge e in g.Adj(v))
                {
                    relax(e);
                }
            }
        }


        /// <summary>
        /// relax edge e and update pq if changed
        /// relax defined as follows: to relax an edge v-->w means to test whether the best known way from s to w is to go from s to v, then take the edge from v to w.If so, update our data structures to indicate that
        /// </summary>
        /// <param name="e"></param>
        private void relax(DirectedEdge e)
        {
            int v = e.From();
            int w = e.To();
            if (distTo[w] > distTo[v] + e.Weight())
            {
                distTo[w] = distTo[v] + e.Weight();
                edgeTo[w] = e;
                if (pq.Contains(w))
                {
                    pq.DecreaseKey(w,distTo[w]);
                }
                else
                {
                    pq.Insert(w,distTo[w]);
                }
            }
        }

        /// <summary>
        /// length of shortest path from s to v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double DistTo(int v)
        {
            return distTo[v];
        }

        /// <summary>
        /// is there a path from s to v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool HasPathTo(int v)
        {
            return distTo[v] < double.PositiveInfinity;
        }

        /// <summary>
        /// shortest path from s to v as iterable, null if no such path
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IEnumerable<DirectedEdge> PathTo(int v)
        {
            if (!HasPathTo(v))
            {
                return null;
            }

            System.Collections.Generic.Stack<DirectedEdge> path = new System.Collections.Generic.Stack<DirectedEdge>();
            for (DirectedEdge e = edgeTo[v]; e != null;e = edgeTo[e.From()] )
            {
                path.Push(e);
            }
            return path;
        }


        /// <summary>
        /// check optimality conditions
        /// (1)for all edge e: distTo[e.to()] <= distTo[e.from()] + e.weight()
        /// (2) for all edge e on the SPT: distTo[e.to()] == distTo[e.from()] + e.weight()
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Check(EdgeWeightedDigraph g, int s)
        {
            //check that edge weights are nonnegative
            foreach (DirectedEdge e in g.Adj(s))
            {
                int w = e.To();
                if (e.Weight() < 0)
                    return false;
            }

            //check that distTo[v] and edgeTo[v] are consistent
            if (System.Math.Abs(distTo[s] - 0.0) > EPSILON || edgeTo[s] != null)
            {
                return false;
            }

            for (int v = 0; v < g.V; v++)
            {
                if (v == s)
                {
                    continue;
                }

                if (edgeTo[v] == null && distTo[v] != double.PositiveInfinity)
                {
                    return false;
                }
            }

            //check all edges e = v-->w statisfy distTo[w] <= distTo[v] + e.weight()
            for (int v = 0; v < g.V; v++)
            {
                foreach (var e in g.Adj(v))
                {
                    int w = e.To();
                    if (distTo[v] == double.PositiveInfinity || distTo[w] == double.PositiveInfinity)
                    {
                        continue;
                    }

                    if (distTo[v] + e.Weight() < distTo[w])
                    {
                        return false;
                    }
                }
            }


            //check that all edge e = v-->w on spt satisfy distTo[w] = distTo[v] + e.weight()
            for (int w = 0; w < g.V; w++)
            {
                if (edgeTo[w] == null)
                {
                    continue;
                }

                DirectedEdge e = edgeTo[w];

                int v = e.From();
                if (w != e.To())
                {
                    return false;
                }


                if (distTo[v] + e.Weight() != distTo[w])
                {
                    return false;
                }
            }

            return true;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    public class BellmanFordSP
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
        /// onQueue[v] = is v currently on the queue?
        /// </summary>
        private bool[] onQueue;

        /// <summary>
        /// queue of vertices to relax
        /// </summary>
        private Queue<int> queue;

        /// <summary>
        /// number of calls to relax()
        /// </summary>
        private int cost;

        /// <summary>
        /// negative cycle or null if no such cycle
        /// </summary>
        private IEnumerable<DirectedEdge> cycle;


        public BellmanFordSP(EdgeWeightedDigraph g, int s)
        {
            distTo = new double[g.V];
            edgeTo = new DirectedEdge[g.V];
            onQueue = new bool[g.V];
            for (int v = 0; v < g.V; v++)
            {
                distTo[v] = double.PositiveInfinity;
            }

            distTo[s] = 0.0;
            
            //bellman-ford algorithm
            queue = new Queue<int>();
            queue.Enqueue(s);
            onQueue[s] = true;

            while (!(queue.Count == 0) && !HasNegativeCycle())
            {
                int v = queue.Dequeue();
                onQueue[v] = false;
                relax(g, v);
            }
        }

        /// <summary>
        /// relax vertex v and put other endpoints on queue if changed
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void relax(EdgeWeightedDigraph g, int v)
        {
            foreach (DirectedEdge e in g.Adj(v))
            {
                int w = e.To();
                if (distTo[w] > distTo[v] + e.Weight())
                {
                    distTo[w] = distTo[v] + e.Weight();
                    edgeTo[w] = e;
                    if (!onQueue[w])
                    {
                        queue.Enqueue(w);
                        onQueue[w] = true;
                    }
                }

                if (cost++%g.V == 0)
                {
                    findNegativeCycle();
                }

            }
        }

        /// <summary>
        /// is there a negative cycle reachable from s?
        /// </summary>
        /// <returns></returns>
        public bool HasNegativeCycle()
        {
            return cycle != null;
        }

        /// <summary>
        /// return a negative cycle, null if no such cycle
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DirectedEdge> NegativeCycle()
        {
            return cycle;
        }

        /// <summary>
        /// by finding a cycle in predecessor graph
        /// </summary>
        private void findNegativeCycle()
        {
            int v = edgeTo.Length;
            EdgeWeightedDigraph spt = new EdgeWeightedDigraph(v);
            for (int w = 0; w < v; w++)
            {
                if (edgeTo[v] != null)
                {
                    spt.AddEdge(edgeTo[v]);
                }
            }

            EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(spt);
            cycle = finder.Cycle();
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
        /// return length of shortest path from s to v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double DistTo(int v)
        {
            return distTo[v];
        }


        /// <summary>
        /// return view of shortest path from s to v, null if no such path
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IEnumerable<DirectedEdge> PathTo(int v)
        {
            if (!HasPathTo(v))
            {
                return null;
            }

            Stack<DirectedEdge> path = new Stack<DirectedEdge>();
            for (DirectedEdge e = edgeTo[v]; e != null;e = edgeTo[e.From()] )
            {
                path.Push(e);
            }

            return path;
        }

        /// <summary>
        /// check optimality conditions:either
        /// (1) there exists a negative cycle reachable from s
        /// (2) for all edges e = v-->w:  distTo[w] &lt;= distTo[v] + e.weight()
        /// (3) for all edges e = v-->w on the SPT: distTo[w] == distTo[v] + e.weight()
        ///  </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Check(EdgeWeightedDigraph g, int s)
        {
            //has a negative cycle
            if (HasNegativeCycle())
            {
                double weight = 0.0;
                foreach (DirectedEdge e in NegativeCycle())
                {
                    weight += e.Weight();
                }

                if (weight >= 0)
                {
                    return false;
                }
            }
            else
            {
                //no negative cycle reachable from s
                if (distTo[s] != 0.0 || edgeTo[s] != null)
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

                //check that all edges e=v-->w satisfy distTo[w]<=distTo[v]+e.weight()
                for (int v = 0; v < g.V; v++)
                {
                    foreach (var e in g.Adj(v))
                    {
                        int w = e.To();
                        if (distTo[v] + e.Weight() < distTo[w])
                        {
                            return false;
                        }
                    }
                }

                //check that all edges e = v-->w on spt satisfy distTo[w] == distTo[v] + e.weight()
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
            }

            return true;
        }


    }
}

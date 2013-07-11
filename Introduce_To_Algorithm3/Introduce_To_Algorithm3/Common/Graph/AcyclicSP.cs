using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    public class AcyclicSP
    {
        /// <summary>
        /// distTo[v] = distance of shortest s-->v path
        /// </summary>
        private double[] distTo;

        /// <summary>
        /// edgeTo[v] = last edge on shortest s-->v path
        /// </summary>
        private DirectedEdge[] edgeTo;


        public AcyclicSP(EdgeWeightedDigraph g, int s)
        {
            distTo = new double[g.V];
            edgeTo = new DirectedEdge[g.V];
            for (int v = 0; v < g.V; v++)
            {
                distTo[v] = double.PositiveInfinity;
            }

            distTo[s] = 0.0;

            //visit vertices in toplogical order
            Topological topological = new Topological(g);
            foreach (int v in topological.Order())
            {
                foreach (DirectedEdge e in g.Adj(v))
                {
                    relax(e);
                }
            }
        }

        /// <summary>
        /// relax
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
            }
        }

        /// <summary>
        /// return length of the shortest path from s to v,infinity if no such path
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
        /// return view of the shortest path from s to v, null if no such path
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// Residual network Gf consists of edges with capacities that represent how we can change the flow on edge of G.an edge in Gf with a residual capacity r(u,v) = c(u,v)-f(u,v). when r(u,v) = 0, they are not in Gf. Gf may also contains edges that are not in G.as an algorithm manipulates the flow, with the goall to increasing the total flow, it might need to decrease the flow on a particulat edge. In order to represent a posible decrease of a positive flowf(u,v) on a edge in G, we place an edge (v,u) into Gf with residual capacity r(v,u) = f(u,v).Let f be a flow in G, we define the residual capacity cf:
    ///              {   c(u,v)-f(u,v)    if(u,v)∈E
    /// cf(u,v) =    {   f(v,u)           if(v,u)∈E
    ///              {   0                otherwise
    /// we deal with a graph G(V,E) and a start and a sink, with positive edge(u,v) represent the capacity of max flow can pass that page. we doesn't hava antiparallel edge, because if we have one, we can add new vertex w and edge (u,w) and (w,v) with the same capacity. we we have multiple start and sinks, we add a new  start and a new sink. add finite flow from the new start to all source start and add finite flow from sinks to the new sink. the problem is the max flow can travel from start to sink.
    /// 
    /// if f is a flow in G and f2 is a flow in the corresponding residual network Gf, we define f argument f2 to be a function:
    ///                         {   f(u,v) + f2(u,v) - f2(v,u)      if(u,v)∈E
    /// (f argument f2)(u,v) =  {
    ///                         {   0                               otherwise
    /// 
    /// 
    /// an augmenting path is a simple path from s to t in the residual network Gf. we call the maximum amount by which we can increase the flow on each edge in an argument path p the residual capacity of p.
    /// so we can define f(u,v) on p = the residual capacity of p if (u,v) is on p. 0 otherwise.
    /// The Ford-Fulkerson  method repeatedly augments the flow along augmenting path until it has found a max flow.
    /// 
    /// 
    /// Ford-Fulkerson-Method(G,s,t)
    ///     Init flow f to 0
    ///     while there exists an augmenting path p in the residual network Gf
    ///         augment flow f along p
    ///     return f;
    /// 
    /// 
    /// If f is a flow in a flow network G=(V,E) with source s and sink t, then the following conditions are qwuivalent:
    /// 1) f is a max flow in g.
    /// 2) the residual network Gf contains no augmenting paths.
    /// 3) |f| = c(S,T) for some cut (S,T) of G
    /// </summary>
    public class MaxFlowFordFulkersonMethod
    {
        /// <summary>
        /// given a flow function
        /// </summary>
        public double[,] flow;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g"></param>
        public MaxFlowFordFulkersonMethod(EdgeWeightedDigraph g)
        {
            if (!IsValid(g))
            {
                throw new ArgumentException();
            }

            int n = g.V;
            flow = new double[n,n];
            /*
             * while there exists a path from s to t in the residual neteork Gf  
             *      C(p) = min{Cf(u,v): (u,v) is in p}
             *      for each edge (u,v) in p
             *          if(u,v)∈E
             *              （u,v).f = (u,v).f + C(p)
             *          else
             *               (v,u).f = (v,u).f - C(p)
             */
        }

        /// <summary>
        /// check Digraph is valid
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public bool IsValid(EdgeWeightedDigraph g)
        {
            if (g == null || g.V <= 0)
            {
                return false;
            }

            foreach (DirectedEdge e in g.Edges())
            {
                if (e.Weight < 0)
                {
                    return false;
                }
            }

            //TODO: add other check here
            
            return true;
        }

    }
}
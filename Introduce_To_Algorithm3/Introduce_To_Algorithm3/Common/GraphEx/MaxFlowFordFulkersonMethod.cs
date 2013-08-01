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
    /// </summary>
    public class MaxFlowFordFulkersonMethod
    {

    }
}

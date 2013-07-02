using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// A topological sort of a directed acyclic graph G=(V,E) is linear ordering of all its vertices such that if G contains an edge (u,v), then u appears before v in the ordering
    /// (if the graph contains a cycle, then no linear ordering is possible)
    /// many application use directed acyclic graph to indicate prcedences among event.
    /// in this case, edge (u,v) represents u must start before v
    /// 
    /// a directed graph can topological sort if and only if it is dag
    /// </summary>
    public class Topological
    {
        private IEnumerable<int> order;//topological order

        /// <summary>
        /// topological sort in a digraph
        /// </summary>
        /// <param name="g"></param>
        public Topological(Digraph g)
        {
            DirectedCycle finder = new DirectedCycle(g);
            if (!finder.HasCycle())
            {
                DepthFirstOrder dfs = new DepthFirstOrder(g);
                order = dfs.ReversePost();
            }
        }

        /// <summary>
        /// topological sort in an edge-weighted digraph
        /// </summary>
        /// <param name="g"></param>
        public Topological(EdgeWeightedDigraph g)
        {
            EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(g);
            if (!finder.HasCycle())
            {
                DepthFirstOrder dfs = new DepthFirstOrder(g);
                order = dfs.ReversePost();
            }
        }

        /// <summary>
        /// return topological order if a DAG; null otherwise
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> Order()
        {
            return order;
        }

        /// <summary>
        /// does digraph have a topological order?
        /// </summary>
        /// <returns></returns>
        public bool HasOrder()
        {
            return order != null;
        }
    }
}

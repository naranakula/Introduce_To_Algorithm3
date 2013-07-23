using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    public class AllPairShortestPathsUsingBellmanFordAlgorithm:AllPairShortestPaths
    {
        
        /// <summary>
        /// constructor
        /// 
        /// it runs at O(V^2*E)
        /// </summary>
        /// <param name="g"></param>
        public AllPairShortestPathsUsingBellmanFordAlgorithm(EdgeWeightedDigraph g) : base(g)
        {
            for (int start = 0; start < g.V; start++)
            {
                BellmanFordAlgorithm bellmanFord = new BellmanFordAlgorithm(g,start);
                for (int to = 0; to < g.V; to++)
                {
                    if (bellmanFord.IsHavePathTo(to))
                    {
                        hasPathTo[start, to] = true;
                        weights[start, to] = bellmanFord.DistTo(to);
                        pathTo[start, to] = bellmanFord.PathTo(to).ToList();
                    }
                }
            }
        }

    }
}

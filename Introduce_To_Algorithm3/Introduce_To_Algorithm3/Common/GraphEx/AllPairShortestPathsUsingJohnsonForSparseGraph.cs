using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// it runs at O(V^2*lgV+VE).
    /// it reports a matrix of shortest-path weights for all pairs vertices or reports a negative weight cycle.
    /// </summary>
    public class AllPairShortestPathsUsingJohnsonForSparseGraph:AllPairShortestPaths
    {
        public AllPairShortestPathsUsingJohnsonForSparseGraph(EdgeWeightedAdjMatrixDigraph g) : base(g)
        {
        }
    }
}

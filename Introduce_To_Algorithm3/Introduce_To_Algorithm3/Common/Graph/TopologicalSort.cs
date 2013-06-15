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
    /// </summary>
    public class TopologicalSort
    {
    }
}

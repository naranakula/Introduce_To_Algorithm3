using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{

    public class DijkstraAllPairsSP
    {
        private DijkstraSP[] all;

        public DijkstraAllPairsSP(EdgeWeightedDigraph g)
        {
            all = new DijkstraSP[g.V];
            for (int v = 0; v < g.V; v++)
            {
                all[v] = new DijkstraSP(g,v);
            }
        }


        public IEnumerable<DirectedEdge> PathTo(int s, int t)
        {
            return all[s].PathTo(t);
        }


        public bool IsPathTo(int s, int t)
        {
            return all[s].HasPathTo(t);
        }


        public double DistTo(int s, int t)
        {
            return all[s].DistTo(t);
        }

    }
}

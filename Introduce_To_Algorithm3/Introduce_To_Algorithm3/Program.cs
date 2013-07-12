using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.GraphEx;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            EdgeWeightedDigraph graph = new EdgeWeightedDigraph(4);

            graph.AddEdge(new DirectedEdge(0,1,0.1));
            graph.AddEdge(new DirectedEdge(0,2,0.4));
            graph.AddEdge(new DirectedEdge(1,2,0.2));
            graph.AddEdge(new DirectedEdge(1,0,0.1));

            var r = graph.Copy();
            r = graph.Reverse();
        }
    }
}

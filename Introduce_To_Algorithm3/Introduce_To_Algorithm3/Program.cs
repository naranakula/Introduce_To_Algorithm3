using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.GraphEx;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = Logger.GetInstance(@"D:\v-chlu\log.txt", true, true);
            EdgeWeightedDigraph digraph = new EdgeWeightedDigraph(5);
            digraph.AddEdge(new DirectedEdge(0,1,1));
            digraph.AddEdge(new DirectedEdge(0,3,2));
            digraph.AddEdge(new DirectedEdge(1,3,3));
            digraph.AddEdge(new DirectedEdge(1,2,1));
            digraph.AddEdge(new DirectedEdge(2,4,2));
            digraph.AddEdge(new DirectedEdge(3,4,4));
            logger.WriteLine("digraph has {0} vertices,{1} edges",digraph.V,digraph.E);
            DagShortestPaths bf =new DagShortestPaths(digraph,0);

            for (int i = 0; i < digraph.V; i++)
            {
                if (bf.IsHavePathTo(i))
                {
                    logger.WriteLine("there is a shortest path from {0} to {1}, weight {2}",0,i,bf.DistTo(i));

                    foreach (var s in bf.PathTo(i))
                    {
                        logger.WriteLine(s.ToString());
                    }
                }
            }

            logger.Close();
            logger.Refresh();
            for (int i = 0; i < digraph.V; i++)
            {
                if (bf.IsHavePathTo(i))
                {
                    logger.WriteLine("there is a shortest path from {0} to {1}, weight {2}", 0, i, bf.DistTo(i));

                    foreach (var s in bf.PathTo(i))
                    {
                        logger.WriteLine(s.ToString());
                    }
                }
            }
            logger.Close();
        }
    }
}

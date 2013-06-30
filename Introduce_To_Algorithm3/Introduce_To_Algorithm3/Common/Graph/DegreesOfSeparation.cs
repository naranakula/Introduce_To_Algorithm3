using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// source is 0, every connect to source is 1, then every connect to 1 is 2
    ///  
    /// </summary>
    public class DegreesOfSeparation
    {
        private int[] separations;
        public DegreesOfSeparation(Graph g, int s)
        {
            separations = new int[g.V];
            for (int i = 0; i < g.V; i++)
            {
                separations[i] = -1;
            }
            BreadthFirstSearch bfs = new BreadthFirstSearch(g,s);

            for (int i = 0; i < g.V; i++)
            {
                if (bfs.HasPathTo(i))
                {
                    separations[i] = bfs.PathTo(i).Count() - 1;
                }
            }
        }

        /// <summary>
        /// get the separation
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int Separation(int v)
        {
            return separations[v];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// strong connected component
    /// </summary>
    public class KosarajuSharirSCC
    {
        /// <summary>
        /// marked[v] = has vertex v has been visited
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// id[v] = id of strong component containing v
        /// </summary>
        private int[] id;

        /// <summary>
        /// number of strongly -connected components
        /// </summary>
        private int count;


        public KosarajuSharirSCC(Digraph g)
        {
            //compute reverse postorder of reverse graph
            DepthFirstOrder dfo = new DepthFirstOrder(g.Reverse());

            //run DFS on g, using reverse postorder to guide calculation
            marked = new bool[g.V];
            id = new int[g.V];

            foreach (int v in dfo.ReversePost())
            {
                if (!marked[v])
                {
                    dfs(g, v);
                    count++;
                }
            }
        }

        /// <summary>
        /// dfs on graph g
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void dfs(Digraph g, int v)
        {
            marked[v] = true;
            id[v] = count;
            foreach (int w in g.Adj(v))
            {
                if(!marked[w])
                    dfs(g,w);
            }
        }

        /// <summary>
        /// return the number of strongly connected components
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return count;
        }

        /// <summary>
        /// are v and w strongly connected
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool StronglyConnected(int v, int w)
        {
            return id[v] == id[w];
        }

        /// <summary>
        /// id of strongly component containing v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int Id(int v)
        {
            return id[v];
        }



    }
}

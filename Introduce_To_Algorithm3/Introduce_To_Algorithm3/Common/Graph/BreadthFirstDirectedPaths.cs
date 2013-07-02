using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// breadth first search for directed graph
    /// it runs at O(V+E)
    /// </summary>
    public class BreadthFirstDirectedPaths
    {
         public const int INFINITY = int.MaxValue;
        /// <summary>
        /// marked[v] = is there an s-v path
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// edgeTo[v] = previous edge on the shortest s-v path
        /// </summary>
        private int[] edgeTo;

        /// <summary>
        /// distTo[v] = number of edges on shortest s-v path
        /// </summary>
        private int[] distTo;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g">the graph used to bfs</param>
        /// <param name="s">the begin point --- s</param>
        public BreadthFirstDirectedPaths(Digraph g, int s)
        {
            marked = new bool[g.V];
            distTo = new int[g.V];
            edgeTo = new int[g.V];
            bfs(g, s);
        }

        /// <summary>
        /// bfs from multiply sources
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sources"></param>
        public BreadthFirstDirectedPaths(Digraph g, IEnumerable<int> sources)
        {
            marked = new bool[g.V];
            distTo = new int[g.V];
            edgeTo = new int[g.V];

            for (int i = 0; i < g.V; i++)
            {
                distTo[i] = INFINITY;
            }

            bfs(g,sources);
        }

        /// <summary>
        /// bfs from multi sources
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sources"></param>
        private void bfs(Digraph g, IEnumerable<int> sources)
        {
            Queue<int> q = new Queue<int>();
            foreach (int s in sources)
            {
                marked[s] = true;
                distTo[s] = 0;
                q.Enqueue(s);
            }

            while (q.Count > 0)
            {
                int v = q.Dequeue();
                foreach (int w in g.Adj(v))
                {
                    if (!marked[w])
                    {
                        edgeTo[w] = v;
                        distTo[w] = distTo[v] + 1;
                        marked[w] = true;
                        q.Enqueue(w);
                    }
                }
            }
        }

        /// <summary>
        /// bfs from single source
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        private void bfs(Digraph g, int s)
        {
            Queue<int> q = new Queue<int>();
            for (int i = 0; i < g.V; i++)
            {
                distTo[i] = INFINITY;
            }

            //we can reach s
            distTo[s] = 0;
            marked[s] = true;
            q.Enqueue(s);

            while (q.Count > 0)
            {
                int v = q.Dequeue();
                foreach (int w in g.Adj(v))
                {
                    if (!marked[w])
                    {
                        edgeTo[w] = v;
                        distTo[w] = distTo[v] + 1;
                        marked[w] = true;
                        q.Enqueue(w);
                    }
                }
            }
        }

        /// <summary>
        /// has a path between s and v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool HasPathTo(int v)
        {
            return marked[v];
        }

        /// <summary>
        /// length of shortest Path between s and v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int DistTo(int v)
        {
            return distTo[v];
        }

        /// <summary>
        /// shortest path between s and v, null if no such path
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IEnumerable<int> PathTo(int v)
        {
            if (!HasPathTo(v)) return null;

            Stack<int> path = new Stack<int>();

            int x;
            for (x = v;  distTo[v] != 0; x= edgeTo[x])
            {
                path.Push(x);
            }
            path.Push(x);

            return path;
        }
    }
}

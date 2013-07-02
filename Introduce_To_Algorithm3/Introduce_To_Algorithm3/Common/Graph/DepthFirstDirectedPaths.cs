using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// depth first search of directed graph
    /// </summary>
    public class DepthFirstDirectedPaths
    {
                /// <summary>
        /// the number connect to s directly or undirectly
        /// </summary>
        private int count;

        /// <summary>
        /// all the vertex connected to the s
        /// </summary>
        private List<int> connected; 
        /// <summary>
        /// marked[v] = is there a path from s to v
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// last vertex on known path to s
        /// </summary>
        private int[] edgeTo;

        /// <summary>
        /// represent the start vertex
        /// </summary>
        private readonly int s;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s">the start vertex</param>
        public DepthFirstDirectedPaths(Digraph g, int s)
        {
            marked = new bool[g.V];
            edgeTo = new int[g.V];
            this.s = s;
            connected = new List<int>();
            dfs(g, s);
        }

        /// <summary>
        /// depth first search
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void dfs(Digraph g, int v)
        {
            //before this run, marked[v] always false
            marked[v] = true;
            count++;
            connected.Add(v);
            foreach (int w in g.Adj(v))
            {
                if (!marked[w])
                {
                    edgeTo[w] = v;
                    dfs(g,w);
                }
            }
        }

        /// <summary>
        /// is there a path from s to v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool HasPathTo(int v)
        {
            return marked[v];
        }

        /// <summary>
        /// a path between s to v, null if no such path
        /// note: this may not the shortest path
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IEnumerable<int> PathTo(int v)
        {
            if (!HasPathTo(v))
            {
                return null;
            }

            Stack<int> stack = new Stack<int>();
            for (int x = v; x !=s; x=edgeTo[x])
            {
                stack.Push(x);
            }

            stack.Push(s);
            return stack;
        }

        /// <summary>
        /// the number connect to s directly or undirectly
        /// 
        /// if count == G.V() then all the vertex are connected
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// all the vertex connected to the s
        /// contains s
        /// </summary>
        public List<int> ConnnectedV
        {
            get { return connected; }
        }
    }
}

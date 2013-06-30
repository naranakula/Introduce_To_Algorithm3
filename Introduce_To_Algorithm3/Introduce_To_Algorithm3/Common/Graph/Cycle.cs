using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// acyclic detect
    /// </summary>
    public class Cycle
    {

        private bool[] marked;
        private int[] edgeTo;
        private Stack<int> cycle; 


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g"></param>
        public Cycle(Graph g)
        {
            if(HasSelfLoop(g)) return;
            if (HasParallelEdges(g)) return;
            
            marked = new bool[g.V];
            edgeTo = new int[g.V];
            for (int v = 0; v < g.V; v++)
            {
                if (!marked[v])
                {
                    dfs(g, -1, v);
                }
            }
        }

        private void dfs(Graph g, int u, int v)
        {
            marked[v] = true;

            foreach (int w in g.Adj(v))
            {
                //short circuit if cycle already found
                if(cycle != null) return;
                if (!marked[w])
                {
                    edgeTo[w] = v;
                    dfs(g,v,w);
                }
                else if(w != u)
                {
                    //check for cycle (but disregard reverse of edge leading to v, such as u-v)
                    cycle = new Stack<int>();
                    for (int x = v; x != w; x = edgeTo[x])
                    {
                        cycle.Push(x);
                    }
                    cycle.Push(w);
                    cycle.Push(v);
                }
            }
        }


        /// <summary>
        /// is there a self loop
        /// find only one not all of them
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private bool HasSelfLoop(Graph g)
        {
            for (int i = 0; i < g.V; i++)
            {
                foreach (int w in g.Adj(i))
                {
                    if (w == i)
                    {
                        cycle = new Stack<int>();
                        cycle.Push(w);
                        cycle.Push(w);
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// is there parallel edges
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public bool HasParallelEdges(Graph g)
        {
            marked = new bool[g.V];
            for (int v = 0; v < g.V; v++)
            {
                foreach (int w in g.Adj(v))
                {
                    if (marked[w])
                    {
                        cycle = new Stack<int>();
                        cycle.Push(v);
                        cycle.Push(w);
                        cycle.Push(v);
                        return true;
                    }
                    marked[w] = true;
                }

                //reset so marked[v] = false, for all v
                foreach (int w in g.Adj(v))
                {
                    marked[w] = false;
                }
            }
            return false;
        }

        /// <summary>
        /// if there is cycle
        /// </summary>
        /// <returns></returns>
        public bool HasCycle()
        {
            return cycle != null;
        }

        /// <summary>
        /// get one cycle, not all the cycle
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetCycle()
        {
            return cycle;
        }
    }
}
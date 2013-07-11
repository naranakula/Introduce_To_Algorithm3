using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// depth first search algorithm visits each vertex exactly once.
    /// Preorder :     put the vertex on a queue before the recursive calls
    /// Postorder:     put the vertex on a queue after the recursive calls
    /// Reverse postorder:  put the vertex on a stack after the recursive calls
    /// </summary>
    public class DepthFirstOrder
    {
        /// <summary>
        /// marked[v] = has v been marked in dfs
        /// </summary>
        private bool[] marked;

        /// <summary>
        /// pre[v] = preorder number of v
        /// </summary>
        private int[] pre;

        /// <summary>
        /// post[v] = postorder number of v
        /// </summary>
        private int[] post;

        /// <summary>
        /// vertices in preorder
        /// </summary>
        private Queue<int> preorder;

        /// <summary>
        /// vertices in postorder
        /// </summary>
        private Queue<int> postorder;

        /// <summary>
        /// counter for preorder numbering
        /// </summary>
        private int preCounter;

        /// <summary>
        /// counter for postorder numbering
        /// </summary>
        private int postCounter;

        /// <summary>
        /// depth first search preorder and postorder in a digraph
        /// </summary>
        /// <param name="g"></param>
        public DepthFirstOrder(Digraph g)
        {
            pre = new int[g.V];
            post = new int[g.V];
            postorder = new Queue<int>();
            preorder = new Queue<int>();
            marked = new bool[g.V];

            for (int v = 0; v < g.V; v++)
            {
                if (!marked[v]) 
                  dfs(g, v);
            }
        }

        /// <summary>
        /// depth-first search preorder and postorder in an edge-weighted digraph
        /// </summary>
        /// <param name="g"></param>
        public DepthFirstOrder(EdgeWeightedDigraph g)
        {
            pre = new int[g.V];
            post = new int[g.V];
            postorder = new Queue<int>();
            preorder = new Queue<int>();
            marked = new bool[g.V];

            for (int v = 0; v < g.V; v++)
            {
                if (!marked[v])
                    dfs(g, v);
            }
        }

        /// <summary>
        /// run DFS in digraph G from vertex v and compute preorder/postorder
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void dfs(Digraph g, int v)
        {
            if (marked[v]) return;
            marked[v] = true;
            pre[v] = preCounter++;
            preorder.Enqueue(v);

            foreach (int w in g.Adj(v))
            {
                if(!marked[v])
                    dfs(g,w);
            }
            postorder.Enqueue(v);
            post[v] = postCounter++;
        }

        /// <summary>
        /// run DFS in edge-weighted digraph G from vertex v and compute preorder/postorder
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void dfs(EdgeWeightedDigraph g, int v)
        {
            if(marked[v]) return;
            marked[v] = true;
            pre[v] = preCounter++;
            preorder.Enqueue(v);
            foreach (DirectedEdge e in g.Adj(v))
            {
                int w = e.To();
                if (!marked[w])
                {
                    dfs(g,w);
                }
            }
            postorder.Enqueue(v);
            post[v] = postCounter++;
        }

        /// <summary>
        /// pre[v]    = preorder  number of v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int Pre(int v)
        {
            return pre[v];
        }

        /// <summary>
        /// post[v]   = postorder number of v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int Post(int v)
        {
            return post[v];
        }

        /// <summary>
        ///  return vertices in postorder as an Iterable
        /// </summary>
        /// <returns></returns>
        public Queue<int> Post()
        {
            return postorder;
        }

        /// <summary>
        /// return vertices in preorder as an Iterable
        /// </summary>
        /// <returns></returns>
        public Queue<int> Pre()
        {
            return preorder;
        }

        /// <summary>
        /// eturn vertices in reverse postorder as an Iterable
        ///  a reverse postorder in a DAG provides a topological order.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> ReversePost()
        {
            return postorder.Reverse();
        }

    }
}

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
    }
}

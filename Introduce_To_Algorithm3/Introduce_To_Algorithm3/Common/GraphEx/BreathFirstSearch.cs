using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    public class BreadthFirstSearch
    {
        private VertexNode[] nodes;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="g">the graph used to bfs</param>
        /// <param name="s">the begin point --- s</param>
        public BreadthFirstSearch(Graph g, int s)
        {
            nodes = new VertexNode[g.V];
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new VertexNode(i);
                nodes[i].Color = VertexColor.White;
            }

            bfs(g, s);
        }

        private void bfs(Graph g, int s)
        {
            nodes[s].Parent = null;
            nodes[s].Distance = 0;
            nodes[s].Color = VertexColor.Gray;
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(s);
            while (queue.Count != 0)
            {
                int u = queue.Dequeue();
                foreach (int v in g.Adj(u))
                {
                    if (nodes[v].Color == VertexColor.White)
                    {
                        nodes[v].Color = VertexColor.Gray;
                        nodes[v].Distance = nodes[u].Distance + 1;
                        nodes[v].Parent = nodes[u];
                        queue.Enqueue(v);
                    }
                }
                nodes[u].Color = VertexColor.Black;
            }
        }



        /// <summary>
        /// has a path between s and v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool HasPathTo(int v)
        {
            return nodes[v].Distance != VertexNode.UNREACHABLE;
        }

        /// <summary>
        /// length of shortest Path between s and v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int DistTo(int v)
        {
            return nodes[v].Distance;
        }


        /// <summary>
        /// shortest path between s and v, null if no such path
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IEnumerable<int> PathTo(int v)
        {
            if (!HasPathTo(v)) return null;


            Stack<int> stack = new Stack<int>();
            while (nodes[v].Parent != null)
            {
                stack.Push(v);
                v = nodes[v].Parent.Id;
            }

            stack.Push(v);
            return stack;
        }
    }



    public class VertexNode
    {
        /// <summary>
        /// the id of vertex
        /// </summary>
        public int Id { get; private set; }

        public const int UNREACHABLE = -1;
        public int Distance = UNREACHABLE;

        public VertexNode(int id)
        {
            Id = id;
        }

        public VertexColor Color;
        public VertexNode Parent;
    }

    public enum VertexColor
    {
        White,//undiscovered
        Gray,
        Black//discovered
    }

}

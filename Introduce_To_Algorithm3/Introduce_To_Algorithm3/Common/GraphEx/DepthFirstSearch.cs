using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// depth first search 
    /// it construct a depth first tree
    /// it runs at O(V+E)
    /// </summary>
    public class DepthFirstSearch
    {
        private VertexNode[] nodes;
        private Queue<int> preOrder;
        private Queue<int> postOrder; 
        public DepthFirstSearch(Graph g, int s)
        {
            nodes = new VertexNode[g.V];
            preOrder = new Queue<int>();
            postOrder = new Queue<int>();
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new VertexNode(i);
                nodes[i].Color = VertexColor.White;
            }
            nodes[s].Distance = 0;
            dfs(g, s);
        }

        public DepthFirstSearch(DiGraph g, int s)
        {
            nodes = new VertexNode[g.V];
            preOrder = new Queue<int>();
            postOrder = new Queue<int>();
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new VertexNode(i);
                nodes[i].Color = VertexColor.White;
            }
            nodes[s].Distance = 0;
            dfs(g,s);
        }

        private int time = 0;
        private void dfs(Graph g, int s)
        {
            preOrder.Enqueue(s);
            nodes[s].Color = VertexColor.Gray;
            nodes[s].Discovered = ++time;
            foreach (int w in g.Adj(s))
            {
                if (nodes[w].Color == VertexColor.White)
                {
                    nodes[w].Parent = nodes[s];
                    nodes[w].Distance = nodes[s].Distance + 1;
                    dfs(g,w);
                }
            }
            nodes[s].Color = VertexColor.Black;
            nodes[s].Finished = ++time;
            postOrder.Enqueue(s);
        }


        private void dfs(DiGraph g, int s)
        {
            preOrder.Enqueue(s);
            nodes[s].Color = VertexColor.Gray;
            nodes[s].Discovered = ++time;
            foreach (int w in g.Adj(s))
            {
                if (nodes[w].Color == VertexColor.White)
                {
                    nodes[w].Parent = nodes[s];
                    nodes[w].Distance = nodes[s].Distance + 1;
                    dfs(g, w);
                }
            }
            nodes[s].Color = VertexColor.Black;
            nodes[s].Finished = ++time;
            postOrder.Enqueue(s);
        }


        /// <summary>
        /// distance to v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int DistanceTo(int v)
        {
            return nodes[v].Distance;
        }

        public bool HasPathTo(int v)
        {
            return nodes[v].Color != VertexColor.White;
        }

        public IEnumerable<int> PathTo(int v)
        {
            if (!HasPathTo(v)) return null;

            Stack<int> stack =new Stack<int>();
            while (nodes[v].Parent != null)
            {
                stack.Push(v);
                v = nodes[v].Parent.Id;
            }
            stack.Push(v);
            return stack;
        }

        public IEnumerable<int> LinkedTo()
        {
            return preOrder;
        }

        public IEnumerable<int> PreOrder()
        {
            return preOrder;
        }

        public IEnumerable<int> PostOrder()
        {
            return postOrder;
        }

        public IEnumerable<int> ReversePostOrder()
        {
            return postOrder.Reverse();
        }

        private class VertexNode
        {
            /// <summary>
            /// the id of vertex
            /// </summary>
            public int Id { get; private set; }

            public VertexNode(int id)
            {
                Id = id;
            }

            public const int UNREACHABLE = -1;
            public VertexColor Color;
            public VertexNode Parent;
            public int Distance = UNREACHABLE;
            public int Discovered;//the time discovered
            public int Finished;//the time finished
        }

    }
}

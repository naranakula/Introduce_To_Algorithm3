using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// search all vertices and construct many depth first trees
    /// </summary>
    public class DepthFirstSearchs
    {
        private VertexNode[] nodes;
        private Queue<int> preOrder;
        private Queue<int> postOrder;
        private int count;
        private List<HashSet<int>> strongConnect;

        public DepthFirstSearchs(DiGraph g)
        {
            preOrder = new Queue<int>();
            postOrder = new Queue<int>();
            strongConnect = new List<HashSet<int>>();
            nodes = new VertexNode[g.V];
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new VertexNode(i);
                nodes[i].Color = VertexColor.White;
            }

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].Color == VertexColor.White)
                {
                    strongConnect.Add(new HashSet<int>());
                    count++;
                    dfs(g, i);
                }
            }
        }

        private int time = 0;
        private void dfs(DiGraph g, int s)
        {
            preOrder.Enqueue(s);
            strongConnect.Last().Add(s);
            nodes[s].Color = VertexColor.Gray;
            nodes[s].Discovered = ++time;
            foreach (int w in g.Adj(s))
            {
                if (nodes[w].Color == VertexColor.White)
                {
                    nodes[w].Parent = nodes[s];
                    dfs(g,w);
                }
            }
            nodes[s].Color = VertexColor.Black;
            postOrder.Enqueue(s);
            nodes[s].Finished = ++time;
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

        public int NumberOfStrongConnected()
        {
            return count;
        }

        public List<HashSet<int>> StrongConnected()
        {
            return strongConnect;
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

            public VertexColor Color;
            public VertexNode Parent;
            public int Discovered;//the time discovered
            public int Finished;//the time finished
        }
    }
}

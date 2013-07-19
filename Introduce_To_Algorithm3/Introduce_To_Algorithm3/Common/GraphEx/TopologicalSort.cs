using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// it runs at O(v+e)
    /// </summary>
    public class TopologicalSort
    {
        private VertexNode[] nodes;
        private Queue<int> postOrder; 
        public TopologicalSort(DiGraph g)
        {
            postOrder = new Queue<int>();
            nodes = new VertexNode[g.V];
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new VertexNode(i);
                nodes[i].Color = VertexColor.White;
            }
            for (int i = 0; i < g.V; i++)
            {
                if (nodes[i].Color == VertexColor.White)
                    dfs(g, i);
            }
        }

        public TopologicalSort(EdgeWeightedDigraph g)
        {
            postOrder = new Queue<int>();
            nodes = new VertexNode[g.V];
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new VertexNode(i);
                nodes[i].Color = VertexColor.White;
            }
            for (int i = 0; i < g.V; i++)
            {
                if(nodes[i].Color == VertexColor.White)
                    dfs(g,i);
            }
        }

        private int time = 0;
        private void dfs(DiGraph g, int s)
        {
            nodes[s].Color = VertexColor.Gray;
            nodes[s].Discovered = ++time;
            foreach (int w in g.Adj(s))
            {
                if (nodes[w].Color == VertexColor.White)
                {
                    nodes[w].Parent = nodes[s];
                    dfs(g, w);
                }
            }
            nodes[s].Color = VertexColor.Black;
            nodes[s].Finished = ++time;
            postOrder.Enqueue(s);
        }

        private void dfs(EdgeWeightedDigraph g, int s)
        {
            nodes[s].Color = VertexColor.Gray;
            nodes[s].Discovered = ++time;
            foreach (DirectedEdge e in g.Adj(s))
            {
                int w = e.To;
                if (nodes[w].Color == VertexColor.White)
                {
                    nodes[w].Parent = nodes[s];
                    dfs(g,w);
                }
            }
            nodes[s].Color = VertexColor.Black;
            nodes[s].Finished = ++time;
            postOrder.Enqueue(s);
        }

        public List<int> GetTopoSort()
        {
            //return nodes.ToList().OrderByDescending(i => i.Finished).Select(i => i.Id).ToList();
            return postOrder.Reverse().ToList();
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

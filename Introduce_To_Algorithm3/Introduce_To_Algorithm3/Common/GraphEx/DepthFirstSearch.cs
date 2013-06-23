using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    public class DepthFirstSearch
    {
        private VertexNode[] nodes;
        public DepthFirstSearch(Graph g, int s)
        {
            nodes = new VertexNode[g.V];
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new VertexNode(i);
                nodes[i].Color = VertexColor.White;
            }
            int time = 0;
            dfs(g, s);
        }

        private void dfs(Graph g, int s)
        {
            nodes[s].Color = VertexColor.Gray;
            foreach (int w in g.Adj(s))
            {
                if (nodes[w].Color == VertexColor.White)
                {
                    nodes[w].Parent = nodes[s];
                    dfs(g,w);
                }
            }
            nodes[s].Color = VertexColor.Black;
        }


        private class VertexNode
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
            public int Discovered;//the time discovered
            public int Finished;//the time finished
        }

    }
}

﻿using System;
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

            dfs(g, s);
        }

        private int time = 0;
        private void dfs(Graph g, int s)
        {
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
            nodes[s].Finished = ++time;
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

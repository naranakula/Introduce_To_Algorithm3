using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// represent vetex of undirected graph
    /// </summary>
    public class UnDirectedVertex
    {
        /// <summary>
        /// the unique id for vetex
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// the edge contains this vetex
        /// </summary>
        public List<UnDirectedVertex> AdjList { get; set; }

        public UnDirectedVertex(int id)
        {
            Id = id;
            AdjList = new List<UnDirectedVertex>();
        }

        public UnDirectedVertex(int id, IEnumerable<UnDirectedVertex> list)
        {
            Id = id;
            AdjList = list.ToList();
        }
    }
}

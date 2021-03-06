﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    /// <summary>
    /// a adjacent matrix of directed graph
    /// </summary>
    public class EdgeWeightedAdjMatrixDigraph
    {
        /// <summary>
        /// number of vertices
        /// </summary>
        private int v;

        /// <summary>
        /// number of edges
        /// </summary>
        private int e;

        /// <summary>
        /// adj[i,j] = whether has a edge from i--->j 
        /// </summary>
        private bool[,] adj;

        /// <summary>
        /// weights[i,j] = the weight of edge from i-->j
        /// </summary>
        private double[,] weights;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="v"></param>
        public EdgeWeightedAdjMatrixDigraph(int v)
        {
            this.v = v;
            this.e = 0;
            adj = new bool[v,v];
            weights = new double[v,v];
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    // /2 for safety
                    weights[i, j] = i==j?0:double.PositiveInfinity/2;
                }
            }
        }


        /// <summary>
        /// return the number of edge
        /// </summary>
        /// <returns></returns>
        public int E
        {
            get { return e; }
        }

        /// <summary>
        /// return the number of vertices
        /// </summary>
        /// <returns></returns>
        public int V
        {
            get { return v; }
        }

        /// <summary>
        /// add a edge from ---> to and weights at weight
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="weight"></param>
        public void AddEdge(int from, int to, double weight)
        {
            if (!adj[from, to])
            {
                e++;
            }

            adj[from, to] = true;
            weights[from, to] = weight;
        }


        public void AddEdge(DirectedEdge e)
        {
            AddEdge(e.From,e.To,e.Weight);
        }

        /// <summary>
        /// has a edge from---> to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool HasEdge(int from, int to)
        {
            return adj[from, to];
        }

        /// <summary>
        /// the weight of edge from---> to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public double Weight(int from, int to)
        {
            return weights[from, to];
        }

        /// <summary>
        /// Return the list of vertices pointed from vertex v 
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public List<int> Adj(int from)
        {
            List<int> list = new List<int>();

            for (int to = 0; to < v; to++)
            {
                if(adj[from,to])
                    list.Add(to);
            }
            return list;
        }

        /// <summary>
        /// return a copy of weights
        /// </summary>
        /// <returns></returns>
        public double[,] Weights()
        {
            double[,] result = new double[weights.GetLength(0),weights.GetLength(1)];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = weights[i, j];
                }
            }
            return result;
        }
    }
}

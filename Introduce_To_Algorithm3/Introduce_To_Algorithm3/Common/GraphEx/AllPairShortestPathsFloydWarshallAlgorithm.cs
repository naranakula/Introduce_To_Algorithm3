using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{
    public class AllPairShortestPathsFloydWarshallAlgorithm
    {
        private double[,] weights;
        public AllPairShortestPathsFloydWarshallAlgorithm(EdgeWeightedAdjMatrixDigraph g)
        {
            int n = g.V;
            double[,] weis0 = g.Weights();
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        weis0[i, j] = System.Math.Min(weis0[i, j], weis0[i, k] + weis0[k,j]);
                    }
                }
            }
            weights = weis0;
        }


        public double[,] GetWeights()
        {
            return weights;
        }
    }
}

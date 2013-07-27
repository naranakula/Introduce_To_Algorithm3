using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GraphEx
{

    public class AllShortestPathsUsingMatrixMultiply
    {
        private double[,] weights;
        public AllShortestPathsUsingMatrixMultiply(EdgeWeightedAdjMatrixDigraph g)
        {
            weights = g.Weights();
            int m = 1;
            while (m<g.V-1)
            {
                weights = ExtendShortestPaths(weights);
                m = 2*m;
            }
        }


        private double[,] ExtendShortestPaths(double[,] weis)
        {
            int n = weis.GetLength(0);
            double[,] reuslt = new double[n,n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    reuslt[i, j] = double.PositiveInfinity;
                    for (int k = 0; k < n; k++)
                    {
                        reuslt[i, j] = System.Math.Min(reuslt[i, j], weis[i, k] + weis[k,j]);
                    }
                }
            }

            return reuslt;
        }


        public double[,] GetWeights()
        {
            return weights;
        }
    }
}

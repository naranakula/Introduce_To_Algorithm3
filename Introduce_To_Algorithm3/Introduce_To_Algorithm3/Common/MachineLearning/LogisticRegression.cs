﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Math;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    public static class LogisticRegression
    {
        /// <summary>
        /// 重复的最大次数
        /// </summary>
        public const int REPEATCOUNT = 10000;
        /// <summary>
        /// 允许的误差范围
        /// </summary>
        public const double TOLERANCE = 0.00001;
        public const double WALKSTEP = 0.001;
        /// <summary>
        /// sigmoid(x) = 1/(1+e^-x)  sigmoid函数在点(0,0.5)中心对称，取值在(0,1)，整个坐标严格单调递增。
        /// </summary>
        /// <param name="inX"></param>
        /// <returns></returns>
        public static double Sigmoid(double inX)
        {
            return 1.0/(1 + System.Math.Exp(-inX));
        }

        /// <summary>
        /// sigmoid(x) = 1/(1+e^-x)  sigmoid函数在点(0,0.5)中心对称，取值在(0,1)，整个坐标严格单调递增。
        /// </summary>
        /// <param name="inX"></param>
        /// <returns></returns>
        public static double[,] Sigmoid(this double[,] inX)
        {
            int m = inX.GetLength(0), n = inX.GetLength(1);
            double[,] result = new double[m,n];
            for (int i = 0; i < inX.GetLength(0); i++)
            {
                for (int j = 0; j < inX.GetLength(1); j++)
                {
                    result[i, j] = 1.0/(1 + System.Math.Exp(-inX[i,j]));
                }
            }
            return result;
        }


        public static double[] GradAscent(double[,] Features, double[,] classLabels)
        {
            int m = Features.GetLength(0);
            int n = Features.GetLength(1);
            double[,] weights = new double[n,1];
            for (int i = 0; i < n; i++)
            {
                weights[i, 0] = 1;
            }

            for (int i = 0; i < REPEATCOUNT; i++)
            {
                double[,] h = Sigmoid(Features.Multiply(weights));
                double[,] error = classLabels.Substract(h);
                weights = weights.Add(Features.Transpose().ScalarMultiply(WALKSTEP).Multiply(error));
                if (i > 9900)
                {
                    Console.WriteLine(weights[0,0]+"\t"+weights[1,0]+"\t"+weights[2,0]);
                }
            }
            double[] result = new double[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = weights[i, 0];
            }
            return result;
        }


        public static double[] StochasticGradientAscent(double[,] Features, double[,] classLabels)
        {
            int m = Features.GetLength(0);
            int n = Features.GetLength(1);
            double[,] weights = new double[n, 1];
            for (int i = 0; i < n; i++)
            {
                weights[i, 0] = 1;
            }

            for (int i = 0; i < m; i++)
            {
                double h = 0.0;
                double[,] temp = new double[n, 1];
                for (int j = 0; j < n; j++)
                {
                    h += Features[i, j]*weights[j, 0];
                    temp[j, 0] = Features[i, j];
                }
                double error = classLabels[i,0] - Sigmoid(h);
                weights = weights.Add(temp.ScalarMultiply(error*WALKSTEP));
            }
            double[] result = new double[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = weights[i, 0];
            }
            return result;
        }

        public static double[] StochasticGradientAscent(double[,] Features, double[,] classLabels, int numIter = 1000)
        {
            int m = Features.GetLength(0);
            int n = Features.GetLength(1);
            double[,] weights = new double[n, 1];
            for (int i = 0; i < n; i++)
            {
                weights[i, 0] = 1;
            }

            for (int i = 0; i < numIter; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    double alpha = 4/(1.0 + i + j) + WALKSTEP;
                    double h = 0.0;
                    double[,] temp = new double[n, 1];
                    for (int k = 0; k < n; k++)
                    {
                        h += Features[j, k] * weights[k, 0];
                        temp[k, 0] = Features[j, k];
                    }
                    double error = classLabels[j, 0] - Sigmoid(h);
                    weights = weights.Add(temp.ScalarMultiply(error * alpha));
                }
            }

            double[] result = new double[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = weights[i, 0];
            }
            return result;
        }
    }
}

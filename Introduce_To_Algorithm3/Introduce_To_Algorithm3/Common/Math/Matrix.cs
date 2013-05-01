using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    public static class Matrix
    {
        /// <summary>
        /// can matrix1 multipy matrix2
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool IsMatrixMul(int[,] matrix1, int[,] matrix2)
        {
            if (matrix1 == null || matrix2 == null)
            {
                return false;
            }
            int w1 = matrix1.GetLength(0), h1 = matrix1.GetLength(1);
            int w2 = matrix2.GetLength(0), h2 = matrix2.GetLength(1);
            if (w1 <= 0 || h1 <= 0 || w2 <= 0 || h2 <= 0 || h1 != w2)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// can matrix1 multipy matrix2
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <param name="result">if the return value is true, result can store the result of matrix1 multipy matrix2, but not the result of matrix1 multipy matrix2</param>
        /// <returns></returns>
        public static bool IsMatrixMul(int[,] matrix1, int[,] matrix2,out int[,] result)
        {
            result = null;
            if (matrix1 == null || matrix2 == null)
            {
                return false;
            }
            int w1 = matrix1.GetLength(0), h1 = matrix1.GetLength(1);
            int w2 = matrix2.GetLength(0), h2 = matrix2.GetLength(1);
            if (w1 <= 0 || h1 <= 0 || w2 <= 0 || h2 <= 0 || h1 != w2)
            {
                return false;
            }

            result = new int[w1,h2];
            return true;
        }


        /// <summary>
        /// multiply two matrix(not necessary n*n ) m*n  x  n*p = m*p
        /// it runs at (m*n*p)
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static int[,] Multiply(int [,] matrix1,int[,] matrix2)
        {
            int[,] result;
            if(!IsMatrixMul(matrix1,matrix2,out result))
            {
                throw new Exception("matrix can not be multiply");
            }

            for(int i=0;i<result.GetLength(0);i++)
            {
                for(int j=0;j<result.GetLength(1);j++)
                {
                    result[i, j] = 0;
                    for(int k=0;k<matrix1.GetLength(1);k++)
                    {
                        result[i, j] += matrix1[i, k]*matrix2[k, j];
                    }
                    
                }
            }

            return result;
        }

        /// <summary>
        /// Strassen’s remarkable recursive algorithm for multiplying n*n matrices. It runs in n^lg7 time
        /// 1)we assume that n is an exact power of 2.we divide n*n matrices into four n/2 * n2 matrices
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static int[,] StrassenMultiply(int [,] matrix1,int[,] matrix2)
        {
            int[,] result;
            if (!IsMatrixMul(matrix1, matrix2, out result) || result.GetLength(0) != result.GetLength(1))
            {
                throw new Exception("matrix can not be multiply");
            }


        }

        /// <summary>
        /// print matrix
        /// </summary>
        /// <param name="matrix1"></param>
        public static void PrintMatrix(this int[,] matrix1)
        {
            
        }

    }
}

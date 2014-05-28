using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    public static class MatrixUtils
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
        public static bool IsMatrixMul(int[,] matrix1, int[,] matrix2, out int[,] result)
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

            result = new int[w1, h2];
            return true;
        }


        /// <summary>
        /// multiply two matrix(not necessary n*n ) m*n  x  n*p = m*p
        /// it runs at (m*n*p)
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static int[,] Multiply(int[,] matrix1, int[,] matrix2)
        {
            int[,] result;
            if (!IsMatrixMul(matrix1, matrix2, out result))
            {
                throw new Exception("matrix can not be multiply");
            }

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < matrix1.GetLength(1); k++)
                    {
                        result[i, j] += matrix1[i, k] * matrix2[k, j];
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// Strassen’s remarkable recursive algorithm for multiplying n*n matrices. It runs in n^lg7 time
        /// 1)we assume that n is an exact power of 2.we divide n*n matrices into four n/2 * n2 matrices  {{A11,A12},{A21,A22}}  {{B11,B12},{B21,B22}}
        /// 2)Create 10 matrices S1,S2 ... S10, each of which is n/2 * n/2
        /// S1 = B12-B22
        /// S2 = A11+A12
        /// S3 = A21+A22
        /// S4 = B21-B11
        /// S5 = A11+A22
        /// S6 = B11+B22
        /// S7 = A12-A22
        /// S8 = B21+B22
        /// S9 = A11-A21
        /// S10 = B11+B12
        /// 3)compute 7 matrix P1 P2....P7
        /// P1 = A11*S1
        /// P2 = S2*B22
        /// P3 = S3*B11
        /// P4 = A22*S4
        /// P5 = S5*S6
        /// P6 = S7*S8
        /// P7 = S9*S10
        /// 4)Compute the desired submatrices C11  C12  C21  C22 of the result matrix C by adding and subtracting various combinations of the Pi matrices.
        /// C11 = P5+P4-P2+P6
        /// C12 = P1+P2
        /// C21 = P3+P4
        /// C22 = P5+P1-P3-P7
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static int[,] StrassenMultiply(int[,] matrix1, int[,] matrix2)
        {
            int[,] result;
            if (!IsMatrixMul(matrix1, matrix2, out result) || result.GetLength(0) != result.GetLength(1))
            {
                throw new Exception("matrix can not be multiply");
            }

            matrix1 = CreateN2Matrix(matrix1);
            matrix2 = CreateN2Matrix(matrix2);
            return Strassen(matrix1, matrix2);
        }

        /// <summary>
        /// Strassen’s remarkable recursive algorithm for multiplying n*n matrices. It runs in n^lg7 time
        /// 1)we assume that n is an exact power of 2.we divide n*n matrices into four n/2 * n2 matrices  {{A11,A12},{A21,A22}}  {{B11,B12},{B21,B22}}
        /// 2)Create 10 matrices S1,S2 ... S10, each of which is n/2 * n/2
        /// S1 = B12-B22
        /// S2 = A11+A12
        /// S3 = A21+A22
        /// S4 = B21-B11
        /// S5 = A11+A22
        /// S6 = B11+B22
        /// S7 = A12-A22
        /// S8 = B21+B22
        /// S9 = A11-A21
        /// S10 = B11+B12
        /// 3)compute 7 matrix P1 P2....P7
        /// P1 = A11*S1
        /// P2 = S2*B22
        /// P3 = S3*B11
        /// P4 = A22*S4
        /// P5 = S5*S6
        /// P6 = S7*S8
        /// P7 = S9*S10
        /// 4)Compute the desired submatrices C11  C12  C21  C22 of the result matrix C by adding and subtracting various combinations of the Pi matrices.
        /// C11 = P5+P4-P2+P6
        /// C12 = P1+P2
        /// C21 = P3+P4
        /// C22 = P5+P1-P3-P7
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        private static int[,] Strassen(int[,] matrix1, int[,] matrix2)
        {
            //when the matrix is small, use direct way to do this
            if (matrix1.GetLength(0) <= 4)
            {
                return Multiply(matrix1, matrix2);
            }

            int n = matrix1.GetLength(0);
            int[,] s1, s2, s3, s4, s5, s6, s7, s8, s9, s10;
            int[,] a11 = new int[n / 2, n / 2];
            int[,] a12 = new int[n / 2, n / 2];
            int[,] a21 = new int[n / 2, n / 2];
            int[,] a22 = new int[n / 2, n / 2];
            int[,] b11 = new int[n / 2, n / 2];
            int[,] b12 = new int[n / 2, n / 2];
            int[,] b21 = new int[n / 2, n / 2];
            int[,] b22 = new int[n / 2, n / 2];
            int[,] p1, p2, p3, p4, p5, p6, p7;

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    a11[i, j] = matrix1[i, j];
                }
            }

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    a12[i, j] = matrix1[i, j + n / 2];
                }
            }

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    a21[i, j] = matrix1[i + n / 2, j];
                }
            }

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    a22[i, j] = matrix1[i + n / 2, j + n / 2];
                }
            }


            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    b11[i, j] = matrix2[i, j];
                }
            }

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    b12[i, j] = matrix2[i, j + n / 2];
                }
            }

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    b21[i, j] = matrix2[i + n / 2, j];
                }
            }

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    b22[i, j] = matrix2[i + n / 2, j + n / 2];
                }
            }

            // 2)Create 10 matrices S1,S2 ... S10, each of which is n/2 * n/2
            // S1 = B12-B22
            // S2 = A11+A12
            // S3 = A21+A22
            // S4 = B21-B11
            // S5 = A11+A22
            // S6 = B11+B22
            // S7 = A12-A22
            // S8 = B21+B22
            // S9 = A11-A21
            // S10 = B11+B12

            s1 = Substract(b12, b22);
            s2 = Add(a11, a12);
            s3 = Add(a21, a22);
            s4 = Substract(b21, b11);
            s5 = Add(a11, a22);
            s6 = Add(b11, b22);
            s7 = Substract(a12, a22);
            s8 = Add(b21, b22);
            s9 = Substract(a11, a21);
            s10 = Add(b11, b12);

            // 3)compute 7 matrix P1 P2....P7
            // P1 = A11*S1
            // P2 = S2*B22
            // P3 = S3*B11
            // P4 = A22*S4
            // P5 = S5*S6
            // P6 = S7*S8
            // P7 = S9*S10
            p1 = Strassen(a11, s1);
            p2 = Strassen(s2, b22);
            p3 = Strassen(s3, b11);
            p4 = Strassen(a22, s4);
            p5 = Strassen(s5, s6);
            p6 = Strassen(s7, s8);
            p7 = Strassen(s9, s10);

            // C11 = P5+P4-P2+P6
            // C12 = P1+P2
            // C21 = P3+P4
            // C22 = P5+P1-P3-P7
            int[,] c11 = p5.Add(p4).Substract(p2).Add(p6);
            int[,] c12 = p1.Add(p2);
            int[,] c21 = p3.Add(p4);
            int[,] c22 = p5.Add(p1).Substract(p3).Substract(p7);

            int[,] c = new int[n, n];
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    c[i, j] = c11[i, j];
                }
            }

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    c[i, j + n / 2] = c12[i, j];
                }
            }

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    c[i + n / 2, j] = c21[i, j];
                }
            }

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    c[i + n / 2, j + n / 2] = c22[i, j];
                }
            }

            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static int[,] Add(this int[,] matrix1, int[,] matrix2)
        {
            int w = matrix1.GetLength(0), h = matrix1.GetLength(1);
            int[,] result = new int[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static double[,] Add(this double[,] matrix1, double[,] matrix2)
        {
            int w = matrix1.GetLength(0), h = matrix1.GetLength(1);
            double[,] result = new double[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static int[,] Substract(this int[,] matrix1, int[,] matrix2)
        {
            int w = matrix1.GetLength(0), h = matrix1.GetLength(1);
            int[,] result = new int[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static double[,] Substract(this double[,] matrix1, double[,] matrix2)
        {
            int w = matrix1.GetLength(0), h = matrix1.GetLength(1);
            double[,] result = new double[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// create a new n*n matrix where n is power of 2
        /// </summary>
        /// <param name="matrix">this is a square matrix</param>
        /// <returns></returns>
        public static int[,] CreateN2Matrix(this int[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new Exception("This must be square matrix");
            }
            int n = 1;
            while (n < matrix.GetLength(0))
            {
                n *= 2;
            }
            if (n == matrix.GetLength(0))
            {
                return matrix;
            }
            int[,] tmp = new int[n, n];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    tmp[i, j] = matrix[i, j];
                }
            }
            return tmp;
        }

        /// <summary>
        /// scalar multiply
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static int[,] ScalarMultiply(this int[,] matrix, int scalar)
        {
            int row = matrix.GetLength(0);
            int column = matrix.GetLength(1);

            int[,] result = new int[row, column];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    result[i, j] = scalar * matrix[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// scalar multiply
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static double[,] ScalarMultiply(this double[,] matrix, double scalar)
        {
            int row = matrix.GetLength(0);
            int column = matrix.GetLength(1);

            double[,] result = new double[row, column];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    result[i, j] = scalar * matrix[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// print matrix
        /// </summary>
        /// <param name="matrix"></param>
        public static string ToStr(this int[,] matrix)
        {
            if (matrix == null) return "Null";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sb.Append(matrix[i, j] + "  ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }


        #region linear equation

        /// <summary>
        /// a11x1+a12x2+... +a1nxn = b1;
        /// a21x1+a22x2+... +a2nxn = b2;
        /// ...
        /// an1x1+an2x2+... +annxn = bn
        /// 
        /// we can model this problem as Ax = b;
        /// then x = a^-1b and x is unique solution
        /// we can LUP deomposition is to find three n*n matrices L,U and P such that PA = LU
        /// where
        ///  L is a unit lower triangular matrix
        ///  U is upper triangulat matrix
        ///  P is permutation matrix
        ///  we call matrices L,U, and P satisfying PA = LU an LUP decomposition of matrix A.
        /// 
        /// PAx = Pb = LUx we define y = Ux, Ly = Pb  we can vector y "forward substitution"  y = Ux called "back substitution", AX = P^-1LUx = P^-1Ly = P^-1Pb = b
        /// 
        /// forward substitution can solve L P b in O(n^2) time.
        /// we can represent permutation P compactly by π[1,......,n]. only P（i,π[i]) = 1
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void LinearEquation(int[,] a, int[] b)
        {

        }

        /// <summary>
        /// a11x1+a12x2+... +a1nxn = b1;
        /// a21x1+a22x2+... +a2nxn = b2;
        /// ...
        /// an1x1+an2x2+... +annxn = bn
        /// 
        /// we can model this problem as Ax = b;
        /// then x = a^-1b and x is unique solution
        /// we can LUP deomposition is to find three n*n matrices L,U and P such that PA = LU
        /// where
        ///  L is a unit lower triangular matrix
        ///  U is upper triangulat matrix
        ///  P is permutation matrix
        /// 
        ///     PAx = Pb = LUx we define y = Ux, Ly = Pb  we can vector y "forward substitution"  y = Ux called "back substitution", AX = P^-1LUx = P^-1Ly = P^-1Pb = b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void LinearEquation(double[,] a, double[] b)
        {

        }

        #endregion

    }
}

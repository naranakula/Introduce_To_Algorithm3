using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    /// <summary>
    /// a matrix int[,]
    /// </summary>
    public class IntMatrix : Matrix<int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">the number of rows</param>
        /// <param name="column">the number of columns</param>
        public IntMatrix(int row, int column)
            : base(row, column)
        {
        }

        #region methods

        /// <summary>
        /// a diagonal matrix is a square which aij = 0 where i!=j
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDiagonal()
        {
            if (!IsSquare())
            {
                throw new Exception(@"it's not a square matrix");
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (i != j && Get(i, j) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// a n*n identity matrix is a diagnoal matrix with 1 along the diagonal
        /// </summary>
        /// <returns></returns>
        public virtual bool IsIdentity()
        {
            if (!IsDiagonal())
            {
                throw new Exception("It's not a disgonal matrix");
            }

            for (int i = 0; i < Rows; i++)
            {
                if (Get(i, i) != 1)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// a tridiagonal matrix is a matrix aij = 0 when |i-j|>1
        /// this means nonzero entries appear only on the main diagnal, immediately above the main diagnal and immediately below the main diagnal
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTridiagonal()
        {
            if (!IsSquare())
            {
                throw new Exception(@"it's not a square matrix");
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (System.Math.Abs(i-j)>1 && Get(i, j) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// an upper-triangular matrix is one for which aij =0 for i>j
        /// </summary>
        /// <returns></returns>
        public virtual bool IsUpperTriangular()
        {
            if (!IsSquare())
            {
                throw new Exception(@"it's not a square matrix");
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (i>j && Get(i, j) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// an upper-triangular matrix is unit upper-triangualr if it has all 1 along the diagonal
        /// </summary>
        /// <returns></returns>
        public virtual bool IsUnitUpperTriangular()
        {
            if (!IsUpperTriangular())
            {
                throw new Exception("the matrix is not upper triangular");
            }

            for (int i = 0; i < Rows; i++)
            {
                if (Get(i, i) != 1)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// an lower-triangular matrix is one for which aij =0 for i&lt;j
        /// </summary>
        /// <returns></returns>
        public virtual bool IsLowerTriangular()
        {
            if (!IsSquare())
            {
                throw new Exception(@"it's not a square matrix");
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (i < j && Get(i, j) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// an lower-triangular matrix is unit lower-triangualr if it has all 1 along the diagonal
        /// </summary>
        /// <returns></returns>
        public virtual bool IsUnitLowerTriangular()
        {
            if (!IsLowerTriangular())
            {
                throw new Exception("the matrix is not lower triangular");
            }

            for (int i = 0; i < Rows; i++)
            {
                if (Get(i, i) != 1)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// a permutation matrix has exactly one 1 in each row or column, and 0 elsewhere
        /// </summary>
        /// <returns></returns>
        public virtual bool IsPermutation()
        {
            if (!IsSquare())
            {
                throw new Exception(@"it's not a square matrix");
            }

            for (int i = 0; i < Rows; i++)
            {
                int count = 0;
                for (int j = 0; j < Columns; j++)
                {
                    int val = Get(i, j);
                    if (val != 1 && val != 0)
                    {
                        return false;
                    }
                    if (val == 1)
                    {
                        count++;
                    }
                    if (count > 1)
                    {
                        return false;
                    }
                }
            }

            for (int j = 0; j < Columns; j++)
            {
                int count = 0;
                for (int i = 0; i < Rows; i++)
                {
                    int val = Get(i, j);
                    if (val != 1 && val != 0)
                    {
                        return false;
                    }
                    if (val == 1)
                    {
                        count++;
                    }
                    if (count > 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// a symmetric matrix A satisfies A = the transpose of A
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSymmetric()
        {
            if (!IsSquare())
            {
                throw new Exception(@"it's not a square matrix");
            }

            Matrix<int> trans = Transpose();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (Get(i, j) != trans.Get(i, j))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region linear equation

        /// <summary>
        /// a11x1+a12x2+... +a1nxn = b1;
        /// a21x1+a22x2+... +a2nxn = b2;
        /// ...
        /// an1x1+an2x2+... +annxn = bn
        /// 
        /// we can model this problem as Ax = b;
        /// then x = a^-1b
        /// we can LUP deomposition is to find three n*n matrices L,U and P such that PA = LU
        /// where
        ///  L is a unit lower triangular matrix
        ///  U is upper triangulat matrix
        ///  P is permutation matrix
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void LinearEquation(int[,] a,int[] b)
        {
            
        }

        #endregion
    }
}

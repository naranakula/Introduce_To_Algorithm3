using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Math
{
    public class DoubleMatrix : Matrix<double>
    {
        #region constructor
        public DoubleMatrix(int row, int column)
            : base(row, column)
        {
        }

        public DoubleMatrix(double[,] from)
            : base(from)
        {
        }
        #endregion


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
                    if (i != j && System.Math.Abs(this[i, j]) > TOLERANCE)
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
                if (System.Math.Abs((int)(this[i, i] - 1)) > TOLERANCE)
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
                    if (System.Math.Abs(i - j) > 1 && System.Math.Abs(this[i, j]) > TOLERANCE)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// an upper-triangular matrix is one for which aij =0 for i&gt;j
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
                    if (i > j && System.Math.Abs(this[i, j]) > TOLERANCE)
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
                if (System.Math.Abs(Get(i, i) - 1) > TOLERANCE)
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
                    if (i < j && System.Math.Abs(Get(i, j)) > TOLERANCE)
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
                if (System.Math.Abs(Get(i, i) - 1) > TOLERANCE)
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
                    double val = Get(i, j);
                    if (System.Math.Abs(val - 1) > TOLERANCE && System.Math.Abs(val) > TOLERANCE)
                    {
                        return false;
                    }
                    if (System.Math.Abs(val - 1) < TOLERANCE)
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
                    double val = Get(i, j);
                    if (System.Math.Abs(val - 1) > TOLERANCE && System.Math.Abs(val) > TOLERANCE)
                    {
                        return false;
                    }
                    if (System.Math.Abs(val - 1) < TOLERANCE)
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

            Matrix<double> trans = Transpose();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (System.Math.Abs(Get(i, j) - trans.Get(i, j)) > TOLERANCE)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        public const double TOLERANCE = 10 ^ -6;
    }
}

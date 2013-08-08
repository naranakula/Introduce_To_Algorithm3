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



        #endregion
    }
}

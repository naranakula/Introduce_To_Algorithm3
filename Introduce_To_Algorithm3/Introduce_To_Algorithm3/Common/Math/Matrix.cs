using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    public class Matrix<T> where T:IEquatable<T>,IComparable<T>
    {
        #region members
        /// <summary>
        /// the number of rows in matrix
        /// </summary>
        private int _row;

        /// <summary>
        /// the number of columns in matrix
        /// </summary>
        private int _column;

        /// <summary>
        /// underly storage for matrix
        /// </summary>
        private T[,] _matrix;

        #endregion

        #region constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">the number of rows in matrix</param>
        /// <param name="column">the number of columns in matrix</param>
        public Matrix(int row, int column)
        {
            _row = row;
            _column = column;
            _matrix = new T[row, column];
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="from"></param>
        public Matrix(T[,] from)
        {
            _row = from.GetLength(0);
            _column = from.GetLength(1);
            _matrix = new T[_row, _column];
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _column; j++)
                {
                    _matrix[i, j] = from[i, j];
                }   
            }
        }

        #endregion

        #region get set
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">count from 0</param>
        /// <param name="column">count from 0</param>
        /// <returns></returns>
        public T Get(int row, int column)
        {
            return _matrix[row, column];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">count from 0</param>
        /// <param name="column">count from 0</param>
        /// <param name="value"></param>
        public void Set(int row, int column, T value)
        {
            _matrix[row, column] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">count from 0</param>
        /// <param name="column">count from 0</param>
        /// <returns></returns>
        public T this[int row, int column]
        {
            get { return _matrix[row, column]; }
            set
            {
                _matrix[row, column] = value;
            }
        }

        /// <summary>
        /// number of rows
        /// </summary>
        public int Rows
        {
            get { return _row; }
        }

        /// <summary>
        /// number of columns
        /// </summary>
        public int Columns
        {
            get { return _column; }
        }

        #endregion

        #region Transpose

        /// <summary>
        /// transpose a matrix
        /// if b is a transpose matrix of a, then b[i,j] = a[j,i]
        /// this means every row in a become a column in b
        /// </summary>
        /// <returns></returns>
        public virtual Matrix<T> Transpose()
        {
            Matrix<T> transpose = new Matrix<T>(_column,_row);
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _column; j++)
                {
                    transpose.Set(j,i,_matrix[i,j]);
                }
            }

            return transpose;
        }

        #endregion

        #region Square

        #region is square matrices

        /// <summary>
        /// is a n*n square matrix
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSquare()
        {
            return _row == _column;
        }


        #endregion

        #endregion

        #region convert

        /// <summary>
        /// convert to matrix
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static Matrix<T> ConvertFrom(T[,] from)
        {
            int rows = from.GetLength(0);
            int columns = from.GetLength(1);

            Matrix<T> matrix = new Matrix<T>(rows, columns);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = from[i, j];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Convert to
        /// </summary>
        /// <returns></returns>
        public T[,] ConvertTo()
        {
            T[,] matrix = new T[_row, _column];

            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _column; j++)
                {
                    matrix[i, j] = this[i, j];
                }
            }
            return matrix;
        }
        #endregion
    }
}

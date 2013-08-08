using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    /// <summary>
    /// a vertor is one dimensional arrry of number
    /// </summary>
    public class Vector<T>where T:IComparable<T>,IEquatable<T>
    {
        #region members

        /// <summary>
        /// underlying storage
        /// </summary>
        private T[] _vector;

        #endregion

        #region constructor
        /// <summary>
        /// construct a n-vector
        /// </summary>
        /// <param name="size"></param>
        public Vector(int size)
        {
            _vector = new T[size];
        }
        #endregion

        #region get set

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">count from 0</param>
        /// <returns></returns>
        public T Get(int n)
        {
            return _vector[n];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">count from 0</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set(int n,T value)
        {
            _vector[n] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">count from 0</param>
        /// <returns></returns>
        public T this[int n]
        {
            get { return _vector[n]; }
            set { _vector[n] = value; }
        }

        #endregion
    }
}

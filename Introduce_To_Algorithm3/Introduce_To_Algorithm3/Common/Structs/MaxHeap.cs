using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// the value of parent bigger than all his children
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaxHeap<T> where T:IComparable<T>
    {
        /// <summary>
        /// store the heap
        /// </summary>
        private T[] arr = new T[4];
        /// <summary>
        /// the heap length
        /// </summary>
        private int len;
        /// <summary>
        /// The parent index of i
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int Parent(int i)
        {
            return (i - 1) / 2;
        }

        /// <summary>
        /// return the leftchild
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int Left(int i)
        {
            return 2*i + 1;
        }

        /// <summary>
        /// return the right child
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int Right(int i)
        {
            return 2*i + 2;
        }

        /// <summary>
        /// get the capaticity of heap
        /// </summary>
        public int Capaticity
        {
            get { return arr.Length; }
        }

        /// <summary>
        /// get the length of heap
        /// </summary>
        public int Length
        {
            get { return len; }
        }
    }
}

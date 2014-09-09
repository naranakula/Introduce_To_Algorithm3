using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// First in Last Out
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Stack<T>
    {
        private T[] arr;
        /// <summary>
        /// elements in stack
        /// </summary>
        private int count;

        /// <summary>
        /// elements in stack
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// is stack empty
        /// </summary>
        public bool IsEmpty
        {
            get { return count <= 0; }
        }

        /// <summary>
        /// init stack
        /// </summary>
        public Stack()
        {
            arr = new T[4];
        }

        /// <summary>
        /// init stack with size
        /// </summary>
        /// <param name="size"></param>
        public Stack(int size)
        {
            arr = new T[size];
        }

        /// <summary>
        /// push a item into stack
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            if(count>=arr.Length)
            {
                Array.Resize(ref arr,count*2+1);
            }

            arr[count++] = item;
        }

        /// <summary>
        /// return but not remove one from stack
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if(IsEmpty)
            {
                throw new Exception("stack underflow");
            }

            return arr[count - 1];
        }

        /// <summary>
        /// return and remove one from stack
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (IsEmpty)
            {
                throw new Exception("stack underflow");
            }
            T tmp = arr[count - 1];
            arr[count - 1] = default(T);
            count--;
            if(count/2>2 &&(arr.Length)/count>2)
            {
                Array.Resize(ref arr, arr.Length/2);
            }

            return tmp;
        }
    }
}

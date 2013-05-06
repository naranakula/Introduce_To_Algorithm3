using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// fifo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Queue<T>
    {
        /// <summary>
        /// head 
        /// </summary>
        private QueueNode<T> head;

        private QueueNode<T> tail;

        private int count;
        /// <summary>
        /// return count
        /// </summary>
        public int Count
        {
            get { return count; }
        }
        /// <summary>
        /// whether the queue is empty
        /// </summary>
        public bool IsEmpty
        {
            get { return count <= 0; }
        }
        /// <summary>
        /// push a item into queue
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            count++;
            if (head == null)
            {
                head = tail = new QueueNode<T>(item);
                return;
            }
            tail.Next = new QueueNode<T>(item);
            tail = tail.Next;
        }

        /// <summary>
        /// return but not remove one from queue
        /// </summary>
        public T Peek()
        {
            if (IsEmpty)
            {
                throw new Exception("queue underflow");
            }

            return head.Key;
        }

        /// <summary>
        /// return and remove one from queue
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new Exception("queue underflow");
            }
            count--;
            T result = head.Key;
            if(IsEmpty)
            {
                head = tail = null;
                return result;
            }

            head = head.Next;
            return result;
        }
    }
}

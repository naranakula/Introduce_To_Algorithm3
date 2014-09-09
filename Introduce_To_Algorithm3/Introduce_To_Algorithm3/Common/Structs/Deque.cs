using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class Deque<T> where T:IComparable<T>
    {
        private LinkedListNode<T> head;
        private LinkedListNode<T> tail;

        /// <summary>
        /// the number of element in deque
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// is it empty
        /// </summary>
        public bool IsEmpty
        {
            get { return head == null || tail == null; }
        }

        /// <summary>
        /// front enqueue
        /// </summary>
        /// <param name="item"></param>
        public void FrontEnqueue(T item)
        {
            if (IsEmpty)
            {
                head = tail = new LinkedListNode<T>(item);
                Count++;
                return;
            }

            Count++;
            head.Prev = new LinkedListNode<T>(item);
            head.Prev.Next = head;
            head = head.Prev;
        }

        /// <summary>
        /// back enqueue
        /// </summary>
        /// <param name="item"></param>
        public void BackEnqueue(T item)
        {
            if (IsEmpty)
            {
                head = tail = new LinkedListNode<T>(item);
                Count++;
                return;
            }

            Count++;
            tail.Next = new LinkedListNode<T>(item);
            tail.Next.Prev = tail;
            tail = tail.Next;
        }

        /// <summary>
        /// front dequeue
        /// </summary>
        /// <returns></returns>
        public T FrontDequeue()
        {
            if (IsEmpty)
            {
                throw new OverflowException();
            }

            T result = head.Key;
            head = head.Next;
            if (head == null)
            {
                tail = null;
                Count = 0;
            }
            else
            {
                Count--;
                head.Next.Prev = null;
            }

            return result;
        }


        /// <summary>
        /// back dequeue
        /// </summary>
        /// <returns></returns>
        public T BackDequeue()
        {
            if (IsEmpty)
            {
                throw new OverflowException();
            }

            T result = tail.Key;

            tail = tail.Prev;
            if (tail == null)
            {
                head = null;
                Count = 0;
            }
            else
            {
                tail.Prev.Next = null;
                Count--;
            }
            return result;
        }
    }
}

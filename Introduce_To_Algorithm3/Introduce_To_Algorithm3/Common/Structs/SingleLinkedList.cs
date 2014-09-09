using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class SingleLinkedList<T> where T:IComparable<T>
    {
        private SingleListNode<T> head;

        private SingleListNode<T> tail;

        /// <summary>
        /// the number of item in list
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// is a empty list
        /// </summary>
        public bool IsEmpty
        {
            get { return head == null || tail == null; }
        }

        public void Insert(T item)
        {
            SingleListNode<T> node = new SingleListNode<T>(item);

            Count++;
            if (head == null || tail == null)
            {
                head = tail = node;
            }
            else
            {
                tail.Next = node;
            }
        }

        /// <summary>
        /// search
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public SingleListNode<T> Search(T item)
        {
            SingleListNode<T> node = head;

            while (node != null)
            {
                if (node.Key.CompareTo(item) == 0)
                {
                    return node;
                }
                else
                {
                    node = node.Next;
                }
            }

            return null;
        }

        /// <summary>
        /// is exist item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsEixst(T item)
        {
            return Search(item) != null;
        }

        /// <summary>
        /// delete the first item meet in list
        /// </summary>
        /// <param name="item"></param>
        public void Delete(T item)
        {
            if (IsEmpty)
            {
                return;
            }

            SingleListNode<T> node = head;

            while (node != null)
            {
                if (node.Key.CompareTo(item) == 0)
                {
                    //this node is head
                    if (tail == head)
                    {
                        tail = null;
                    }
                    head = head.Next;
                    
                    Count--;
                    return;
                }
                else if (node.Next != null && node.Next.Key.CompareTo(item) == 0)
                {
                    Count--;
                    if (tail == node.Next)
                    {
                        tail = node;
                        node.Next = null;
                    }
                    else
                    {
                        node.Next = node.Next.Next;
                    }
                    return;
                }
                else
                {
                    node = node.Next;
                }
            }
        }
    }
}

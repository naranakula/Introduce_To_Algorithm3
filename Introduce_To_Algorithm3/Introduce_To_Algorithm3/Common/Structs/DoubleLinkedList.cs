using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class DoubleLinkedList<T> where T:IComparable<T>
    {
        /// <summary>
        /// head of linkedlist
        /// </summary>
        private LinkedListNode<T> head;

        /// <summary>
        /// tail of linkedList
        /// </summary>
        private LinkedListNode<T> tail;

        /// <summary>
        /// is empty
        /// </summary>
        public bool IsEmpty
        {
            get { return head == null || tail == null; }
        }

        /// <summary>
        /// the number of element in list
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// find the first item . if not found,return null
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public LinkedListNode<T> Search(T item)
        {
            LinkedListNode<T> node = head;
            while(node!=null&&node.Key.CompareTo(item)!=0)
            {
                node = node.Next;
            }
            return node;
        }

        /// <summary>
        /// insert item into list
        /// </summary>
        /// <param name="item"></param>
        public void Insert(T item)
        {
            Count++;
            if(head == null)
            {
                head = tail = new LinkedListNode<T>(item);
                return;
            }

            LinkedListNode<T> node = new LinkedListNode<T>(item);
            node.Next = head;
            head.Prev = node;
            head = node;
        }

        /// <summary>
        /// delete from list
        /// </summary>
        /// <param name="node"></param>
        public void Delete(LinkedListNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            Count--;

            if(node.Prev != null)
            {
                node.Prev.Next = node.Next;
            }
            else
            {
                head = node.Next;
            }

            if (node.Next == null)
            {
                tail = node.Prev;
            }

            if(node.Next != null)
            {
                node.Next.Prev = node.Prev;
            }
        }

    }
}

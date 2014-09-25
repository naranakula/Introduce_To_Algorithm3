using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// sorted double Linked List
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class SortedDoubleLinkedList<K,V> where K:IComparable<K>,IEquatable<K>
    {
        /// <summary>
        /// head of linkedlist
        /// </summary>
        private LinkedListNode<K,V> head;

        /// <summary>
        /// tail of linkedList
        /// </summary>
        private LinkedListNode<K,V> tail;

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
        /// it runs at O(n)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public LinkedListNode<K,V> Search(K item)
        {
            LinkedListNode<K,V> node = head;

            while (node != null && node.Key.CompareTo(item) != 0)
            {
                if (node.Key.CompareTo(item) > 0)
                {
                    return null;
                }

                node = node.Next;
            }
            return node;
        }

        /// <summary>
        /// insert item into list, it runs at O(n)
        /// </summary>
        /// <param name="item"></param>
        public void Insert(K key, V val)
        {
            Count++;
            if (head == null)
            {
                head = tail = new LinkedListNode<K, V>(key, val);
                return;
            }

            LinkedListNode<K, V> newNode = new LinkedListNode<K, V>(key, val);

            LinkedListNode<K, V> node = head;
            //inserted point, new node are inserted after inserted Point, if insertedPoint is null, then newNode is head
            LinkedListNode<K, V> insertedPoint = null;
            while (node != null && node.Key.CompareTo(key) < 0)
            {
                insertedPoint = node;
                node = node.Next;
            }

            if (insertedPoint == null)
            {
                //newNode is smallest & it should be head
                newNode.Next = head;
                head.Prev = newNode;
            }
            else if(insertedPoint.Next == null)
            {
                //newNode is tail
                newNode.Prev = tail;
                tail.Next = newNode;
                tail = newNode;
            }
            else
            {
                LinkedListNode<K, V> next = insertedPoint.Next;

                newNode.Prev = insertedPoint;
                insertedPoint.Next = newNode;
                newNode.Next = next;
                next.Prev = newNode;
            }
        }

        /// <summary>
        /// delete from list
        /// </summary>
        /// <param name="node"></param>
        public void Delete(LinkedListNode<K,V> node)
        {
            if (node == null)
            {
                return;
            }

            Count--;

            if (node.Prev != null)
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

            if (node.Next != null)
            {
                node.Next.Prev = node.Prev;
            }
        }

        /// <summary>
        /// delete from list
        /// </summary>
        /// <param name="key"></param>
        public void Delete(K key)
        {
            var node = Search(key);

            Delete(node);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// used for skip list
    /// </summary>
    public class SortedSkipLinkedList<K, V> where K : IComparable<K>, IEquatable<K>
    {
        #region Member

        /// <summary>
        /// head of linkedlist
        /// </summary>
        private SkipListNode<K, V> head;

        /// <summary>
        /// tail of linkedList
        /// </summary>
        private SkipListNode<K, V> tail;

        /// <summary>
        /// is empty
        /// </summary>
        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        /// <summary>
        /// the number of element in list
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// get head
        /// </summary>
        public SkipListNode<K, V> HeadNode
        {
            get { return head; }
        }

        /// <summary>
        /// get tail
        /// </summary>
        public SkipListNode<K, V> TailNode
        {
            get { return tail; }
        } 

        #endregion

        #region Search

        /// <summary>
        /// find the first item . if not found,return null
        /// it runs at O(n)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SkipListNode<K, V> Search(K key)
        {
            SkipListNode<K, V> node = head;

            while (node != null)
            {
                if (node.IsMaxNode)
                {
                    node = null;
                    break;
                }

                if (node.IsMinNode)
                {
                    node = node.Next;
                    continue;
                }

                if (node.Key.CompareTo(key) == 0)
                {
                    return node;
                }

                if (node.Key.CompareTo(key) > 0)
                {
                    return null;
                }

                node = node.Next;
            }

            return node;
        } 

        #endregion

        #region Constructor


        public SortedSkipLinkedList()
        {
            head = new SkipListNode<K, V>(){IsMinNode = true};
            tail = new SkipListNode<K, V>(){IsMaxNode = true};
            head.Next = tail;
            tail.Prev = head;
        }  

        #endregion

        #region Insert

        /// <summary>
        /// search insert point p,
        /// new key are inserted after p & all the key before p(p is included) are smaller then key
        /// p can't be null, because each list have min & max 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SkipListNode<K, V> SearchInsertPoint(K key)
        {
            SkipListNode<K, V> node = head;
            SkipListNode<K, V> insertPoint = null;
            while (node != null)
            {
                if (node.IsMaxNode)
                {
                    break;
                }

                if (node.IsMinNode)
                {
                    insertPoint = node;
                    node = node.Next;
                    continue;
                }

                if (node.Key.CompareTo(key) < 0)
                {
                    insertPoint = node;
                    node = node.Next;
                }
                else
                {
                    break;
                }
            }

            return insertPoint;
        }

        /// <summary>
        /// insert item into list, it runs at O(n)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key, V val)
        {
            Count++;
            
            SkipListNode<K,V> newNode = new SkipListNode<K, V>(key, val);

            SkipListNode<K, V> insertPoint = SearchInsertPoint(key);

            newNode.Next = insertPoint.Next;
            insertPoint.Next.Prev = newNode;
            newNode.Prev = insertPoint;
            insertPoint.Next = newNode;
        }

        #endregion

        #region Delete

        /// <summary>
        /// delete from list
        /// </summary>
        /// <param name="node"></param>
        public void Delete(SkipListNode<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            if (node.IsMinNode || node.IsMaxNode)
            {
                return;
            }

            Count--;

            node.Prev.Next = node.Next;
            node.Next.Prev = node.Prev;
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

        #endregion

    }
}

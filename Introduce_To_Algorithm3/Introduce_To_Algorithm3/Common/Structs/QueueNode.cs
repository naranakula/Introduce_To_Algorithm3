using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class QueueNode<T>
    {
        public T Key;
        public QueueNode<T> Next;

        public QueueNode()
        {

        }

        public QueueNode(T key)
        {
            this.Key = key;
        }

        public QueueNode(T key, QueueNode<T> next)
        {
            this.Key = key;
            this.Next = next;
        }
    }

    public class QueueNode<T, V>
    {
        public T Key;
        public QueueNode<T> Next;

        public V Val;

        public QueueNode()
        {

        }

        public QueueNode(T key)
        {
            this.Key = key;
        }
        public QueueNode(T key, V val)
        {
            this.Key = key;
            this.Val = val;
        }

        public QueueNode(T key, QueueNode<T> next)
        {
            this.Key = key;
            this.Next = next;
        }

        public QueueNode(T key, V val, QueueNode<T> next)
        {
            this.Key = key;
            this.Next = next;
            this.Val = val;
        }
    }
}

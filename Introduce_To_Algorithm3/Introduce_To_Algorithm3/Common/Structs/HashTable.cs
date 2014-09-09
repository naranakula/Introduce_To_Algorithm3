using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class HashTable<K, V>
        where K : IComparable<K>, IEquatable<K>
        where V : class
    {
        private SingleListNode<K, V>[] arrs;

        private int count;
        /// <summary>
        /// elements in hashtable
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// construct
        /// </summary>
        public HashTable()
        {
            arrs = new SingleListNode<K, V>[4];
        }

        /// <summary>
        /// insert key and value into hashtable
        /// if the key already in the hashtable, change the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key, V val)
        {
            SingleListNode<K, V> results = search(key);

            if (results != null)
            {
                results.Value = val;
                return;
            }

            SingleListNode<K, V> node = new SingleListNode<K, V>(key, val);
            node.Next = arrs[GetPosition(key)];
            arrs[GetPosition(key)] = node;
            count++;

            if (count / arrs.Length >= 10)
            {
                SingleListNode<K, V>[] tmp = arrs;
                arrs = new SingleListNode<K, V>[count];
                count = 0;
                for (int i = 0; i < tmp.Length; i++)
                {
                    SingleListNode<K, V> sln = tmp[i];
                    while (sln != null)
                    {
                        Insert(sln.Key, sln.Value);
                        sln = sln.Next;
                    }
                }
            }
        }

        /// <summary>
        /// delete key in hashtable, if not contains , throw exception
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V Delete(K key)
        {
            if (!Contains(key))
            {
                throw new Exception("can not find key");
            }

            int position = GetPosition(key);
            SingleListNode<K, V> results = arrs[position];

           

            if (results.Key.Equals(key))
            {
                arrs[position] = results.Next;
            }
            else
            {
                SingleListNode<K, V> before = arrs[position];
                while (results != null && !results.Key.Equals(key))
                {
                    before = results;
                    results = results.Next;
                }
                before.Next = results.Next;
            }
            count--;
            if (count >= 4 && arrs.Length / count > 8)
            {
                SingleListNode<K, V>[] tmp = arrs;
                arrs = new SingleListNode<K, V>[count];
                count = 0;
                for (int i = 0; i < tmp.Length; i++)
                {
                    SingleListNode<K, V> sln = tmp[i];
                    while (sln != null)
                    {
                        Insert(sln.Key, sln.Value);
                        sln = sln.Next;
                    }
                }
            }
            return results.Value;
        }

        /// <summary>
        /// search the key from table
        /// If not found, return null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private SingleListNode<K, V> search(K key)
        {
            int position = GetPosition(key);
            SingleListNode<K, V> results = arrs[position];

            while (results != null && !results.Key.Equals(key))
            {
                results = results.Next;
            }

            return results;
        }

        /// <summary>
        /// search the key from table
        /// If not found, return null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V Search(K key)
        {
            SingleListNode<K, V> results = search(key);

            return results == null ? null : results.Value;
        }


        /// <summary>
        /// whether the Hashtable contains the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(K key)
        {
            SingleListNode<K, V> results = search(key);

            return results != null;

        }

        private int GetPosition(K key)
        {
            return System.Math.Abs(key.GetHashCode())%arrs.Length;
        }
    }
}

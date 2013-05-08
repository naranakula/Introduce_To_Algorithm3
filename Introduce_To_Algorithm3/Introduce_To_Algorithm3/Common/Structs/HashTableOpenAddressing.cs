using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    public class HashTableOpenAddressing<K, V>
        where K : IComparable<K>, IEquatable<K>
        where V : class
    {
        private HashItem<K, V>[] items;
        private int count;

        /// <summary>
        /// return the count in hashtable
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        public HashTableOpenAddressing()
        {
            items = new HashItem<K, V>[4];
        }

        /// <summary>
        /// insert key ,val into hashtable
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key, V val)
        {

            if (count >= items.Length)
            {
                HashItem<K, V>[] tmps = items;
                items = new HashItem<K, V>[count * 2];
                count = 0;
                for (int j = 0; j < tmps.Length; j++)
                {
                    if (tmps[j] != null)
                    {
                        Insert(tmps[j].Key, tmps[j].Value);
                    }
                }
            }


            int i = 0;
            while (i < items.Length)
            {
                int pos = Hash(key, i);
                if (items[pos] == null)
                {
                    items[pos] = new HashItem<K, V>(key, val);
                    count++;
                    return;
                }
                else if (items[pos].Key.Equals(key))
                {
                    items[pos].Value = val;
                    break;
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// delete key from hashtable
        /// </summary>
        /// <param name="key"></param>
        public void Delete(K key)
        {
            if(items.Length>=4 || (count>0 &&items.Length/count>=4))
            {
                int len = items.Length/2;
                len = len >= 4 ? len : 4;
                if(len != items.Length)
                {
                    count = 0;
                    HashItem<K, V>[] tmps = items;
                    items = new HashItem<K, V>[len];
                    for (int j = 0; j < tmps.Length; j++)
                    {
                        if (tmps[j] != null)
                        {
                            Insert(tmps[j].Key, tmps[j].Value);
                        }
                    }
                }
            }

            int i = 0;
            while (i < items.Length)
            {
                int pos = Hash(key, i);
                if (items[pos] == null)
                {
                    throw new Exception("hash table not contains key");
                }
                else if (items[pos].Key.Equals(key))
                {
                    count--;
                    items[pos] = null;
                    return ;
                }
                else
                {
                    i++;
                }
            }
        }


        /// <summary>
        /// whether hashtable contains key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(K key)
        {
            int i = 0;
            while (i < items.Length)
            {
                int pos = Hash(key, i);
                if (items[pos] == null)
                {
                    return false;
                }
                else if (items[pos].Key.Equals(key))
                {
                    return true;
                }
                else
                {
                    i++;
                }
            }

            throw new Exception("Must some thing wrong");
        }

        /// <summary>
        /// search key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V Search(K key)
        {
            int i = 0;
            while (i < items.Length)
            {
                int pos = Hash(key, i);
                if (items[pos] == null)
                {
                    throw new Exception("hash table not contains key");
                }
                else if (items[pos].Key.Equals(key))
                {
                    return items[pos].Value;
                }
                else
                {
                    i++;
                }
            }

            throw new Exception("Must some thing wrong");
        }


        /// <summary>
        /// map < h(k,0),... h(k,m-1)>  to [0....m-1]
        /// </summary>
        /// <param name="K"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public int Hash(object K, int i)
        {
            return System.Math.Abs(K.GetHashCode()+i)%items.Length;
        }


        /// <summary>
        /// map < h(k,0),... h(k,m-1)>  to [0....m-1]
        /// </summary>
        /// <param name="K"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public int DoubleHash(object k,int i)
        {
            /*
             h(k, i)=(   h1(k)+i*h2(k)   ) mod m
             
             The value h2(k) must be relatively prime to the hash-table size m for the entire
hash table to be searched.so h(k,i) can search all the [0,m-1].
             prove it later
             so let m is power of 2, and h2(k) is odd  
             
             */
            throw new NotImplementedException();
        }

        /// <summary>
        /// map < h(k,0),... h(k,m-1)>  to [0....m-1]
        /// </summary>
        /// <param name="K"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public int QuadraticHash(object k, int i)
        {
            /*
             h(k, i)=(   h(k)+C1*i+ C2*h(k)   ) mod m
           to make full use of
the hash table, the values of c1, c2, and m are constrained.so h(k,i) can search all the [0,m-1].
             */
            throw new NotImplementedException();
        }

        private void PerfectHash()
        {
            /*
             * 
             * hash all items hash to same plot again
             * 
             */
        }
    }

    public class HashItem<K, V>
        where K : IComparable<K>, IEquatable<K>
        where V : class
    {
        public K Key;
        public V Value;
        public HashItem() { }
        public HashItem(K key, V val)
        {
            this.Key = key;
            this.Value = val;
        }
    }
}

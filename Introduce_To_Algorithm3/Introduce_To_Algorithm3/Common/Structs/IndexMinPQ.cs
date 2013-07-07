using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// the IndexMinPQ clas represents an indexed priority queue of generic keys.
    /// in order to let clients refer to items on the pq, an integer between 0 and NMAX-1 is associated with each key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IndexMinPQ<T> where T : IComparable<T>
    {
        /// <summary>
        /// maximum number of elements on pq
        /// </summary>
        private int NMAX;
        /// <summary>
        /// number of elements on pq
        /// </summary>
        private int count;

        /// <summary>
        /// binary heap using 1-based indexing
        /// </summary>
        private int[] pq;

        /// <summary>
        /// inverse of pq - qp[pq[i]] = pq[qp[i]] = i
        /// </summary>
        private int[] qp;

        /// <summary>
        /// keys[i] = priority of i
        /// </summary>
        private T[] keys;

        /// <summary>
        /// create an empty indexed priority queue with indices between 0 and nMax - 1
        /// </summary>
        /// <param name="nMax">max number of item can in pq</param>
        public IndexMinPQ(int nMax)
        {
            if (nMax < 0)
                throw new ArgumentException("nMax can be negative");

            this.NMAX = nMax;
            keys = new T[nMax + 1];
            pq = new int[nMax + 1];
            qp = new int[nMax + 1];
            for (int i = 0; i <= nMax; i++)
            {
                qp[i] = -1;
            }
        }

        /// <summary>
        /// is the pq empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return count == 0;
        }

        /// <summary>
        /// is i an index on pq
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool Contains(int i)
        {
            if (i < 0 || i >= NMAX)
                throw new ArgumentException();

            return qp[i] != -1;
        }

        /// <summary>
        /// return the number of keys on pq
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return count;
        }

        /// <summary>
        /// associate key with index i
        /// </summary>
        /// <param name="i"></param>
        /// <param name="key"></param>
        public void Insert(int i, T key)
        {
            if(i<0||i>=NMAX)
                throw new IndexOutOfRangeException("i out of index");
            if(Contains(i))
                throw new ArgumentException("i already contained");

            count++;
            qp[i] = count;
            pq[count] = i;
            keys[i] = key;
            swim(count);
        }

        /// <summary>
        /// return the index associated with minimal key
        /// 1-based
        /// </summary>
        /// <returns></returns>
        public int MinIndex()
        {
            if(count == 0)
                throw new ArgumentException("Priority queue underflow");

            return pq[1];
        }

        /// <summary>
        /// return a minimal key
        /// </summary>
        /// <returns></returns>
        public T MinKey()
        {
            if (count == 0)
                throw new ArgumentException("Priority queue underflow");
            return keys[pq[1]];
        }

        /// <summary>
        /// delete a minimal key and return its associated index
        /// </summary>
        /// <returns></returns>
        public int DelMin()
        {
            if (count == 0)
                throw new ArgumentException("Priority queue underflow");
            int min = pq[1];
            exch(1, count--);
            sink(1);
            qp[min] = -1;
            keys[pq[count + 1]] = default(T);
            pq[count + 1] = -1;
            return min;
        }

        /// <summary>
        /// Return the key associated with index i.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T KeyOf(int i)
        {
            if (i < 0 || i >= NMAX)
                throw new IndexOutOfRangeException("i out of index");
            if (!Contains(i))
                throw new ArgumentException("no such element");

            return keys[i];
        }

        /// <summary>
        /// change the key associate with index i to specified value
        /// </summary>
        /// <param name="i"></param>
        /// <param name="key"></param>
        public void ChangeKey(int i, T key)
        {
            if (i < 0 || i >= NMAX)
                throw new IndexOutOfRangeException("i out of index");
            if (!Contains(i))
                throw new ArgumentException("no such element");

            keys[i] = key;
            swim(qp[i]);
            sink(qp[i]);
        }

        /// <summary>
        /// decrease the key associated with i to specified value
        /// </summary>
        /// <param name="i"></param>
        /// <param name="key"></param>
        public void DecreaseKey(int i, T key)
        {
            if (i < 0 || i >= NMAX)
                throw new IndexOutOfRangeException("i out of index");
            if (!Contains(i))
                throw new ArgumentException("no such element");
            if(keys[i].CompareTo(key)<=0) throw new ArgumentException("i must bigger");
            keys[i] = key;
            swim(qp[i]);
        }

        /// <summary>
        /// increase the key associate with index i to the specified value
        /// </summary>
        /// <param name="i"></param>
        /// <param name="key"></param>
        public void IncreaseKey(int i, T key)
        {
            if (i < 0 || i >= NMAX)
                throw new IndexOutOfRangeException("i out of index");
            if (!Contains(i))
                throw new ArgumentException("no such element");
            if (keys[i].CompareTo(key) >= 0) 
                throw new ArgumentException("i must smaller");
            keys[i] = key;
            sink(qp[i]);
        }

        /// <summary>
        /// delete the key associated with index i
        /// </summary>
        /// <param name="i"></param>
        public void Delete(int i)
        {
            if (i < 0 || i >= NMAX)
                throw new IndexOutOfRangeException("i out of index");
            if (!Contains(i))
                throw new ArgumentException("no such element");
            int index = qp[i];
            exch(index, count--);
            swim(index);
            sink(index);
            keys[i] = default(T);
            qp[i] = -1;
        }

        /// <summary>
        /// general helper functions
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private bool greater(int i, int j)
        {
            return keys[pq[i]].CompareTo(keys[pq[j]]) > 0;
        }


        private void exch(int i, int j)
        {
            int swap = pq[i];
            pq[i] = pq[j];
            pq[j] = swap;
            qp[pq[i]] = i;
            qp[pq[j]] = j;
        }

        private void sink(int k)
        {
            while (2*k<=count)
            {
                int j = 2*k;
                if (j < count && greater(j, j + 1))
                    j++;
                if (!greater(k, j))
                    break;
                exch(k,j);
                k = j;
            }
        }

        private void swim(int k)
        {
            while (k>1 && greater(k/2,k))
            {
                exch(k,k/2);
                k = k/2;
            }
        }

    }
}

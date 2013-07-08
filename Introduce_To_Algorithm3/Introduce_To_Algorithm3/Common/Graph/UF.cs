using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Graph
{
    /// <summary>
    /// the <see cref="UF"/> class represents a union-find data structure
    /// it supports the union and find operations, along with a method for determining the number of disjoint set.
    /// </summary>
    public class UF
    {
        /// <summary>
        /// id[i] = parent of i
        /// </summary>
        private int[] id;

        /// <summary>
        /// sz[i] = number of objects in subtree rooted at i
        /// </summary>
        private int[] sz;

        /// <summary>
        /// number of components
        /// </summary>
        private int count;

        /// <summary>
        /// create an empty union find data structure with n isolated sets
        /// </summary>
        /// <param name="n"></param>
        public UF(int n)
        {
            if (n < 0)
                throw new ArgumentException("n cann't be negative");
            count = n;
            id = new int[n];
            sz = new int[n];
            for (int i = 0; i < n; i++)
            {
                id[i] = i;
                sz[i] = 1;
            }
        }

        /// <summary>
        /// return the id of component corresponding to object p
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int Find(int p)
        {
            if(p<0 || p>=id.Length)
                throw new ArgumentOutOfRangeException("p out of range");

            while (p != id[p])
            {
                p = id[p];
            }

            return p;
        }

        /// <summary>
        /// return the number of disjoint sets
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return count;
        }

        /// <summary>
        /// a object p and q in the same set
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public bool IsConnected(int p, int q)
        {
            return Find(p) == Find(q);
        }

        /// <summary>
        /// replace sets containing p and q with their union
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        public void Union(int p, int q)
        {
            int i = Find(p);
            int j = Find(q);
            if (i == j)
                return;

            //make smaller root point to larger one
            if (sz[i] < sz[j])
            {
                id[i] = j;
                sz[j] += sz[i];
            }
            else
            {
                id[j] = i;
                sz[i] += sz[j];
            }
            count--;
        }
    }
}

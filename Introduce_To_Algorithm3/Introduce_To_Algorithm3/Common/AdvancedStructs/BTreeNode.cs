using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    /// <summary>
    /// we call t=M/2 as the minimum degree of the B- tree.
    /// each non root non leaf node has [t-1,2t-1] keys
    /// </summary>
    [Serializable]
    public class BTreeNode<K, V> where K : IComparable<K>, IEquatable<K>
    {
        /// <summary>
        /// the number of keys in current node it must between [t-1,2t-1]
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// the number of keys in current Node
        /// </summary>
        public int Count
        {
            get { return N; }
        }

        /// <summary>
        /// at least t-1 number keys , keys[i] &lt;= keys[i+1]
        /// </summary>
        public Tuple<K, V>[] KeyValues;

        /// <summary>
        /// at least t number children.     keys[i-1] &lt= children[i] &lt;= keys[i]
        /// </summary>
        public BTreeNode<K, V>[] Children;

        /// <summary>
        /// parent of this node
        /// </summary>
        public BTreeNode<K, V> Parent;

        /// <summary>
        /// whether is leaf
        /// </summary>
        public bool IsLeaf { get; internal set; }

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="minDegree">the min degree</param>
        public BTreeNode(int minDegree)
        {
            Debug.Assert(minDegree >= 2);
            Array.Resize<BTreeNode<K, V>>(ref Children, 2 * minDegree);
            Array.Resize(ref KeyValues, 2 * minDegree - 1);
            N = 0;
            Parent = null;
            IsLeaf = false;
        }

        #endregion

        #region Search

        /// <summary>
        /// int:the index we should search in child based on 0  bool:true if we found the key, false if not
        /// suppose the returned int is i
        ///               KeyValues[i]<key<= KeyValues[i+1]
        /// i=0           key<=KeyValues[0]
        /// i==N          key>KeyValues[N]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<int, bool> SquenceSearch(K key)
        {
            int i = 0;
            bool found = false;
            while (i < N && key.CompareTo(KeyValues[i].Item1) > 0)
            {
                i++;
            }
            if (i < N && key.CompareTo(KeyValues[i].Item1) == 0)
            {
                found = true;
            }

            return new Tuple<int, bool>(i, found);
        }

        /// <summary>
        /// int:the index we should search in child  bool:true if we found the key, false if not
        /// 
        /// if we found the key, then return the first one found
        /// if we can't found the key, then return the first one bigger than key
        /// suppose the returned int is i
        ///               KeyValues[i]<key<= KeyValues[i+1]
        /// i=0           key<=KeyValues[0]
        /// i==N          key>KeyValues[N]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<int, bool> Search(K key)
        {
            if (N < 10)
            {
                return SquenceSearch(key);
            }
            else
            {
                return BinarySearch(key);
            }
        }

        /// <summary>
        ///  if we found the key, then return the first one found
        ///  if we can't found the key, then return the first one bigger than key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<int, bool> BinarySearch(K key)
        {
            return BinarySearch(key, 0, N - 1);
        }

        /// <summary>
        /// if we found the key, then return the first one found
        /// if we can't found the key, then return the first one bigger than key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private Tuple<int, bool> BinarySearch(K key, int begin, int end)
        {
            if (end == begin)
            {
                return key.CompareTo(KeyValues[begin].Item1) <= 0 ? new Tuple<int, bool>(begin, key.CompareTo(KeyValues[begin].Item1) == 0) : new Tuple<int, bool>(begin + 1, false);
            }
            int mid = (begin + end) / 2;
            if (key.CompareTo(KeyValues[mid].Item1) <= 0)
            {
                return BinarySearch(key, begin, mid);
            }
            else
            {
                return BinarySearch(key, mid + 1, end);
            }
        }

        #endregion

    }
}

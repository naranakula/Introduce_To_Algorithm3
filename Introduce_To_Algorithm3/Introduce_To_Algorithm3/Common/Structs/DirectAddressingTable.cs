using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// we want to store [0,....,u-1] data, we use array keys[u]
    /// </summary>
    public class DirectAddressingTable<V>
    {
        #region Member

        /// <summary>
        /// the number of elements in table 
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// the number of elements can be in table
        /// </summary>
        public int Capacity { get; protected set; }

        /// <summary>
        /// keys. key[i] = true if i in table
        /// </summary>
        private bool[] keys;

        /// <summary>
        /// values
        /// </summary>
        private V[] vals;

        #endregion

        #region Constructor

        /// <summary>
        /// construt
        /// </summary>
        /// <param name="capacity">the number of elements can be in table</param>
        public DirectAddressingTable(int capacity)
        {
            this.Capacity = capacity;
            keys = new bool[capacity];
            vals = new V[capacity];
        } 

        #endregion

        #region Insert

        /// <summary>
        /// insert into table.
        /// if key alreay in table, update value , return false;
        /// else return true
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool Insert(int key, V val)
        {
            if (key >= Capacity)
            {
                throw  new OverflowException("key is too big");
            }

            if (keys[key])
            {
                vals[key] = val;
                return false;
            }
            else
            {
                keys[key] = true;
                vals[key] = val;
                Count++;
                return true;
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// if key in table, delete it & return true
        /// otherwise return false
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(int key)
        {
            if (key >= Capacity)
            {
                throw new OverflowException("key is too big");
            }

            if (keys[key])
            {
                keys[key] = false;
                vals[key] = default (V);
                Count--;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Contains

        /// <summary>
        /// contains
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(int key)
        {
            if (key < 0 || key >= Capacity)
            {
                return false;
            }

            return keys[key];
        }

        #endregion

        #region Search

        /// <summary>
        /// if contains, return true & value
        /// else return false
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<bool,V> Search(int key)
        {
            if (Contains(key))
            {
                return new Tuple<bool, V>(true,vals[key]);
            }
            else
            {
                return new Tuple<bool, V>(false, default(V));
            }
        }

        #endregion
    }
}

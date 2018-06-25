using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.ConcurrentCollections
{
    /// <summary>
    /// 线程安全的Red Black tree
    /// 底层实现使用SortedDictionary (rbtree实现)
    /// 建议键是字符串，键不能为null  值可以为null，但不建议为null
    /// 插入、删除、修改是O(logn)的
    /// 集合操作过程中键是不可修改的
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ConcurrentRbTree<TKey,TValue> where TKey:class
    {
        #region 属性

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// 底层的Red Black tree实现
        /// </summary>
        private readonly SortedDictionary<TKey,TValue> _rbTree = new SortedDictionary<TKey, TValue>();

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConcurrentRbTree()
        {
        }

        #endregion

        /// <summary>
        /// 如果key为null，返回false
        /// 否则如果key不在集合中，添加到集合返回true，否则返回false
        /// O(logn)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryAdd(TKey key, TValue value)
        {
            if (key == null)
            {
                return false;
            }

            lock (_locker)
            {
                if (!_rbTree.ContainsKey(key))
                {
                    //=和add的区别是add如果已经存在会抛异常
                    //_rbTree[key] = value;
                    //新增
                    _rbTree.Add(key,value);
                    return true;
                }
                else
                {
                    //什么也不做
                    return false;
                }
            }
        }

        /// <summary>
        /// 如果key为null，返回false
        /// 否则
        /// 如果已经包含key，则更新并返回true
        /// 否则新增，返回false
        /// O（lgn）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdate(TKey key, TValue value)
        {
            if (key == null)
            {
                return false;
            }

            lock (_locker)
            {
                if (_rbTree.ContainsKey(key))
                {
                    //更新
                    _rbTree[key] = value;
                    return true;
                }
                else
                {
                    //新增
                    _rbTree.Add(key,value);
                    return false;
                }
            }

        }


        /// <summary>
        /// 清空集合
        /// O（1） 只需将root设置为null
        /// </summary>
        public void Clear()
        {
            lock (_locker)
            {
                _rbTree.Clear();
            }
        }

        /// <summary>
        /// 如果key为null,返回false
        /// 集合中是否包含key
        /// O(lgn)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            if (key == null)
            {
                return false;
            }

            lock (_locker)
            {
                return _rbTree.ContainsKey(key);
            }
        }

        /// <summary>
        /// 如果key为null，返回false
        /// 否则返回key是否存在集合中
        /// O(lgn)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                value = default(TValue);
                return false;
            }


            lock (_locker)
            {
                return _rbTree.TryGetValue(key, out value);
            }

        }

        /// <summary>
        /// 如果key为null,返回false
        /// 如果集合包含key，移除并返回true
        /// 否则返回false
        /// O(lgn)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool TryRemove(TKey key)
        {
            if (key == null)
            {
                return false;
            }

            lock (_locker)
            {
                return _rbTree.Remove(key);
            }
        }

        /// <summary>
        /// 本身keys的获取是O(1)的
        /// 但ToList是O(n)的
        /// </summary>
        public List<TKey> KeyList
        {
            get
            {
                lock (_locker)
                {
                    return _rbTree.Keys.ToList();
                }
            }
        }

    }
}

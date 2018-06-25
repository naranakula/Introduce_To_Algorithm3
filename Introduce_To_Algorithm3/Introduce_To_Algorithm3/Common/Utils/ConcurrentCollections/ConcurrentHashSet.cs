using System.Collections.Generic;
using System.Linq;

namespace Introduce_To_Algorithm3.Common.Utils.ConcurrentCollections
{
    /// <summary>
    /// 并行集合
    /// T需要覆盖object的equals和gethashcode
    /// 如果T相等则hashcode必须一样，不相等hashcode也有可能一样
    /// 
    /// </summary>
    public class ConcurrentHashSet<T> where T:class
    {
        /// <summary>
        /// 底层的hashset
        /// </summary>
        private readonly HashSet<T> _hashSet = new System.Collections.Generic.HashSet<T>();

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();


        /// <summary>
        /// 添加数据项
        /// 如果为null，返回false，不添加
        /// 如果已经存在于集合，返回false，不添加
        /// 否则返回true，并添加到集合
        /// </summary>
        /// <param name="item">数据项不能为null</param>
        /// <returns></returns>
        public bool TryAdd(T item)
        {
            if (item == null)
            {
                return false;
            }

            lock (_locker)
            {
                if (_hashSet.Contains(item))
                {
                    return false;
                }
                else
                {
                    return _hashSet.Add(item);
                }
            }
        }

        /// <summary>
        /// 添加或者更新T
        /// 如果T为null,返回false，什么也不做
        /// 否则返回true
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddOrUpdate(T item)
        {
            if (item == null)
            {
                return false;
            }

            lock (_locker)
            {
                if (_hashSet.Contains(item))
                {
                    _hashSet.Remove(item);
                }

                return _hashSet.Add(item);
            }
        }

        /// <summary>
        /// 移除数据项
        /// 如果item为null，返回false，什么也不做
        /// 否则如果数据存在于集合中，将其删除返回true，如果数据不存在与集合中返回false
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryRemove(T item)
        {
            if (item == null)
            {
                return false;
            }

            lock (_locker)
            {
                //true if the element is successfully found and removed; otherwise, false. This method returns false if item is not found in the HashSet<T> object.
                return _hashSet.Remove(item);
            }
        }


        /// <summary>
        /// 如果item为null，直接返回false
        /// 否则返回item是否存在于集合中
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            if (item == null)
            {
                return false;
            }

            lock (_locker)
            {
                return _hashSet.Contains(item);
            }
        }


        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            lock (_locker)
            {
                _hashSet.Clear();
            }
        }

        /// <summary>
        /// 返回集合中元素的个数
        /// </summary>
        public int Count
        {
            get
            {
                lock (_locker)
                {
                    return _hashSet.Count;
                }
            }
        }


        /// <summary>
        /// 转成列表
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            lock (_locker)
            {
                return _hashSet.ToList();
            }
        }

    }
}

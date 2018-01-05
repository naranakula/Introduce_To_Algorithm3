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
    /// 建议键是字符串
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


    }
}

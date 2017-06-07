using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 获取数据项，如果缓存数据已经存在，直接返回缓存数据
    /// 否则通过getFunc获取数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheItem<T> where T :class 
    {
        /// <summary>
        /// 底层缓存数据
        /// </summary>
        private T cacheItem;
        
        /// <summary>
        /// 获取缓存数据的逻辑
        /// </summary>
        private Func<T> getFunc; 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="getFunc">获取数据的逻辑</param>
        public CacheItem(Func<T> getFunc)
        {
            this.getFunc = getFunc;
        } 

        /// <summary>
        /// 获取数据项，如果缓存数据已经存在，直接返回缓存数据
        /// 否则通过getFunc获取数据
        /// </summary>
        /// <returns></returns>
        public T Get(Action<Exception> exceptionHandler=null)
        {
            if (cacheItem != null)
            {
                return cacheItem;
            }

            try
            {
                T result = getFunc();
                cacheItem = result;
                return result;
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return default(T);
            }
        }
    }
}

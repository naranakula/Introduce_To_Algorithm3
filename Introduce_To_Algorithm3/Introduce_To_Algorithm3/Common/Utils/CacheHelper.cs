using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// use System.Runtime.Caching.MemoryCache which in System.Runtime.Caching.dll
    /// it like system.Web.Caching.Cache but it can runs at wpf or winform or console
    /// 
    /// System.Runtime.Caching.MemoryCache is thread safe  多线程安全
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        /// get a reference to default memorycache instance
        /// </summary>
        private static readonly MemoryCache cache = MemoryCache.Default;

        /// <summary>
        /// 如果key不存在，新增；否则，覆盖现有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds">过多少秒后过期，指的是Set之后多长时间过期</param>
        public static void Set<T>(string cacheKey, T value,int expireSeconds)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expireSeconds);
            cache.Set(cacheKey,value,policy);
        }


        /// <summary>
        /// 如果key不存在，新增；否则，什么也不做
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds">过多少秒后过期,指的是Add之后多长时间过期</param>
        /// <returns>true if insertion succeeded, or false if there is an already an entry in the cache that has the same key as key.</returns>
        public static bool Add<T>(string cacheKey, T value, int expireSeconds)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expireSeconds);
            //true if insertion succeeded, or false if there is an already an entry in the cache that has the same key as key.
            return cache.Add(cacheKey, value, policy);
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains(string key)
        {
            return cache.Contains(key);
        }

        /// <summary>
        /// 返回缓存项的个数
        /// </summary>
        /// <returns></returns>
        public static long GetCount()
        {
            return cache.GetCount();
        }

        /// <summary>
        /// A reference to the cache entry that is identified by key, if the entry exists; otherwise, null.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return cache.Get(key);
        }

        /// <summary>
        /// A reference to the cache entry that is identified by key, if the entry exists; otherwise, null.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T:class
        {
            return cache.Get(key) as T;
        }

        /// <summary>
        /// 删除一项，如果存在，返回已删除的缓存value，否则返回null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Remove(string key)
        {
            return cache.Remove(key);
        }

        /// <summary>
        /// 返回一个当时缓存的快照副本
        /// </summary>
        /// <returns></returns>
        public static List<KeyValuePair<string, object>> ToList()
        {
            return cache.ToList();
        }

        /// <summary>
        /// 获取所有key的快照副本
        /// </summary>
        /// <returns></returns>
        public static List<string> Keys()
        {
            return cache.Select(r => r.Key).ToList();
        }

        /// <summary>
        /// 清空cache
        /// </summary>
        public static void Clear()
        {
            var keys = cache.Select(r => r.Key).ToList();
            foreach (var key in keys)
            {
                cache.Remove(key);
            }
        }

        /// <summary>
        /// 从缓存中移除指定比例的缓存项
        /// </summary>
        /// <param name="percent">要移除的缓存项的总数的百分比 0-100</param>
        /// <returns>从缓存中移除的数量</returns>
        public static long Trim(int percent)
        {
            return cache.Trim(percent);
        }
    }
}

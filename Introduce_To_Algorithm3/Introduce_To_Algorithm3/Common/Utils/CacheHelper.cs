using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Caching.Configuration;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Semantics;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// use System.Runtime.Caching.MemoryCache which in System.Runtime.Caching.dll
    /// it like system.Web.Caching.Cache but it can runs at wpf or winform or console
    /// 
    /// System.Runtime.Caching.MemoryCache is thread safe  多线程安全
    /// MemoryCache的key和value均不能为null
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        /// get a reference to default memorycache instance
        /// 默认的限制缓存800M，总内存限制的扫描间隔2分钟
        /// </summary>
        private static MemoryCache _cache = null;// MemoryCache.Default;


        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 缓存实例
        /// </summary>
        public static MemoryCache Cache
        {
            get
            {
                lock (locker)
                {
                    return _cache;
                }
            }
            private set
            {
                lock (locker)
                {
                    _cache = value;
                }
            }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public static bool InitCacheHelper(Action<Exception> exceptionHandler = null)
        {
            try
            {
                lock (locker)
                {
                    if (_cache == null)
                    {
                        NameValueCollection config = new NameValueCollection(3);
                        //缓存可使用的物理内存的百分比（0到100的整数）。默认值为零，指示 MemoryCache 实例会基于计算机上安装的内存量来管理自己的内存。
                        config.Add("physicalMemoryLimitPercentage", "50");
                        //获取或设置 MemoryCache 对象的实例可增长到的最大内存大小（以兆字节为单位）。默认值为零，指示 MemoryCache 实例会基于计算机上安装的内存量来管理自己的内存。
                        config.Add("cacheMemoryLimitMegabytes", "1024");
                        //缓存实现将当前内存负载与为缓存实例设置的绝对内存和内存百分比限制进行比较所采用的时间间隔。HH:mm:ss 每2分钟扫描一次
                        config.Add("pollingInterval", "00:02:00");

                        _cache = new MemoryCache("cmlu.common.Cache", config);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return false;
            }
            
        }

        /// <summary>
        /// 如果key不存在，新增；否则，覆盖现有值
        /// 返回true表示未发生异常，false表示发生异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds">过多少秒后过期，指的是Set之后多长时间过期</param>
        /// <param name="exceptionHandler"></param>
        public static bool Set<T>(string cacheKey, T value,int expireSeconds,Action<Exception> exceptionHandler = null)
        {
            try
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.Priority = CacheItemPriority.Default;
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expireSeconds);
                Cache.Set(cacheKey, value, policy);
                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }


        /// <summary>
        /// 如果key不存在，新增；否则，什么也不做
        /// 返回true表示添加成功，false表示发生异常或者缓存中已经有指定的键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds">过多少秒后过期,指的是Add之后多长时间过期</param>
        /// <param name="exceptionHandler"></param>
        /// <returns>true if insertion succeeded, or false if there is an already an entry in the Cache that has the same key as key.</returns>
        public static bool Add<T>(string cacheKey, T value, int expireSeconds,Action<Exception> exceptionHandler = null)
        {
            try
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.Priority = CacheItemPriority.Default;
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expireSeconds);
                //true if insertion succeeded, or false if there is an already an entry in the Cache that has the same key as key.
                return Cache.Add(cacheKey, value, policy);
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
            
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="key"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool Contains(string key,Action<Exception> exceptionHandler = null)
        {
            try
            {
                return Cache.Contains(key);
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        /// <summary>
        /// 返回缓存项的个数
        /// </summary>
        /// <returns></returns>
        public static long GetCount()
        {
            return Cache.GetCount();
        }

        /// <summary>
        /// A reference to the Cache entry that is identified by key, if the entry exists; otherwise, null.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static object Get(string key,Action<Exception> exceptionHandler = null)
        {
            try
            {
                return Cache.Get(key);
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return null;
            }
        }

        /// <summary>
        /// A reference to the Cache entry that is identified by key, if the entry exists; otherwise, null.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static T Get<T>(string key,Action<Exception> exceptionHandler = null) where T:class
        {
            try
            {
                return Cache.Get(key) as T;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return null;
            }
        }

        /// <summary>
        /// 删除一项，如果存在，返回已删除的缓存value，否则返回null
        /// </summary>
        /// <param name="key"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static object Remove(string key,Action<Exception> exceptionHandler = null)
        {
            try
            {
                return Cache.Remove(key);

            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return null;
            }
        }

        /// <summary>
        /// 返回一个当时缓存的快照副本
        /// </summary>
        /// <returns></returns>
        public static List<KeyValuePair<string, object>> ToList()
        {
            return Cache.ToList();
        }

        /// <summary>
        /// 获取所有key的快照副本
        /// </summary>
        /// <returns></returns>
        public static List<string> Keys()
        {
            return Cache.Select(r => r.Key).ToList();
        }

        /// <summary>
        /// 清空Cache
        /// </summary>
        public static bool Clear(Action<Exception> exceptionHandler = null)
        {
            try
            {
                var keys = Cache.Select(r => r.Key).ToList();
                foreach (var key in keys)
                {
                    Cache.Remove(key);
                }
                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }

        }

        /// <summary>
        /// 从缓存中移除指定比例的缓存项
        /// </summary>
        /// <param name="percent">要移除的缓存项的总数的百分比 0-100</param>
        /// <returns>从缓存中移除的数量</returns>
        public static long Trim(int percent)
        {
            return Cache.Trim(percent);
        }
    }

}

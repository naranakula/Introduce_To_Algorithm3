using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public static class CacheWebHelper
    {
        /// <summary>
        /// 添加一个缓存项，如果已经存在相同的key,此方法将会覆盖现有值。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds">过多少秒后过期</param>
        public static void Insert(string key, object value, int expireSeconds)
        {
            HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.AddSeconds(expireSeconds), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void Clear()
        {
            var enumerator = HttpRuntime.Cache.GetEnumerator();

            List<string> keyList = new List<string>();

            while (enumerator.MoveNext())
            {
                string key = enumerator.Key as string;
                if (key != null)
                {
                    keyList.Add(key);
                }
            }

            foreach (var key in keyList)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        /// <summary>
        /// 移除指定键的cache
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class
        {
            Object obj = HttpRuntime.Cache.Get(key);
            return obj as T;
        }
    }
}

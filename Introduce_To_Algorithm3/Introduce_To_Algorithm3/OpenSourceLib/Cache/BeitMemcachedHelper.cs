using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeIT.MemCached;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// Memcached帮助类
    /// 可以在多线程使用
    /// </summary>
    public class BeITMemcachedHelper
    {
        #region 单例
        /// <summary>
        /// 单例
        /// </summary>
        private static BeITMemcachedHelper _instance;

        /// <summary>
        /// 使用的memcached客户端名字
        /// </summary>
        private string memcleintName;

        /// <summary>
        /// 构造函数
        /// </summary>
        private BeITMemcachedHelper()
        {
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static BeITMemcachedHelper GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (_instance == null)
            {
                _instance = new BeITMemcachedHelper();
            }

            return _instance;
        }
        #endregion

        #region Setup

        /// <summary>
        ///   method for setup an instance.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="servers">
        /// A list of memcached servers in standard notation: host:port. 
        /// If port is omitted, the default value of 11211 is used. 
        /// Both IP addresses and host names are accepted, for example:
        /// "localhost", "127.0.0.1", "cache01.example.com:12345", "127.0.0.1:12345", etc
        /// </param>
        /// <returns></returns>
        public bool Setup(string name,string[] servers)
        {
            try
            {
                memcleintName = name;
                MemcachedClient.Setup(name,servers);
                MemcachedClient client = MemcachedClient.GetInstance(name);
                client.SendReceiveTimeout = 5000;//设置接收发送连接超时时间
                client.MinPoolSize = 1;//连接池
                client.MaxPoolSize = 10;
                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error("设置memcached客户端失败："+ex);
                return false;
            }
        }

        /// <summary>
        ///   method for setup an instance.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="servers">
        /// A list of memcached servers in standard notation: host:port. 
        /// If port is omitted, the default value of 11211 is used. 
        /// Both IP addresses and host names are accepted, for example:
        /// "localhost", "127.0.0.1", "cache01.example.com:12345", "127.0.0.1:12345", etc
        /// </param>
        /// <returns></returns>
        public bool Setup(string name, IEnumerable<string> servers)
        {
            try
            {
                memcleintName = name;
                MemcachedClient.Setup(name, servers.ToArray());
                MemcachedClient client = MemcachedClient.GetInstance(name);
                client.SendReceiveTimeout = 5000;//设置接收发送连接超时时间
                client.MinPoolSize = 1;//连接池
                client.MaxPoolSize = 10;
                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error("设置memcached客户端失败：" + ex);
                return false;
            }
        }

        #endregion

        #region set

        /// <summary>
        /// This method corresponds to the "set" command in the memcached protocol. 
        /// It will unconditionally set the given key to the given value.
        /// Using the overloads it is possible to specify an expiry time
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <returns>returns true if the value was successfully set.</returns>
        public bool Set(string keyName,object value,TimeSpan expireTime) 
        {
            lock (this)
            {
                return MemcachedClient.GetInstance(memcleintName).Set(keyName, value, expireTime);
            }
        }

        /// <summary>
        /// This method corresponds to the "set" command in the memcached protocol. 
        /// It will unconditionally set the given key to the given value.
        /// Using the overloads it is possible to specify an expiry time
        /// </summary>
        /// <param name="memcachedClientName">配置的客户端名字</param>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        /// <param name="expireTime">returns true if the value was successfully set.</param>
        /// <returns></returns>
        public bool Set(string memcachedClientName,string keyName, object value, TimeSpan expireTime)
        {
            lock (this)
            {
                return MemcachedClient.GetInstance(memcachedClientName).Set(keyName, value, expireTime);
            }
        }

        #endregion

        #region get

        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public T Get<T>(string keyName) where T : class
        {
            object result = null;
            lock (this)
            {
                result = MemcachedClient.GetInstance(memcleintName).Get(keyName);
            }

            return result as T;
        }

        /// <summary>
        /// 获取int
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public int GetInt(string keyName)
        {
            object result = null;
            lock (this)
            {
                result = MemcachedClient.GetInstance(memcleintName).Get(keyName);
            }

            if (result is int)
            {
                return (int) result;
            }

            return 0;
        }

        /// <summary>
        /// 获取Double
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public double GetDouble(string keyName)
        {
            object result = null;
            lock (this)
            {
                result = MemcachedClient.GetInstance(memcleintName).Get(keyName);
            }

            if (result is double)
            {
                return (double)result;
            }

            return 0;
        }

        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="T">配置的客户端名字</typeparam>
        /// <param name="memcachedClientName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public T Get<T>(string memcachedClientName, string keyName) where T : class
        {
            object result = null;
            lock (this)
            {
                result = MemcachedClient.GetInstance(memcachedClientName).Get(keyName);
            }

            return result as T;
        }

        /// <summary>
        /// 获取int
        /// </summary>
        /// <param name="memcachedClientName">配置的客户端名字</param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public int GetInt(string memcachedClientName, string keyName)
        {
            object result = null;
            lock (this)
            {
                result = MemcachedClient.GetInstance(memcachedClientName).Get(keyName);
            }

            if (result is int)
            {
                return (int)result;
            }

            return 0;
        }

        /// <summary>
        /// 获取Double
        /// </summary>
        /// <param name="memcachedClientName">配置的客户端名字</param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public double GetDouble(string memcachedClientName, string keyName)
        {
            object result = null;
            lock (this)
            {
                result = MemcachedClient.GetInstance(memcachedClientName).Get(keyName);
            }

            if (result is double)
            {
                return (double)result;
            }

            return 0;
        }

        #endregion

        #region delete

        /// <summary>
        /// This method corresponds to the "delete" command in the memcache protocol.
        /// It will immediately delete the given key and corresponding value.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>true, if success</returns>
        public bool Delete(string keyName)
        {
            lock (this)
            {
                return MemcachedClient.GetInstance(memcleintName).Delete(keyName);
            }
        }

        /// <summary>
        /// This method corresponds to the "delete" command in the memcache protocol.
        /// It will immediately delete the given key and corresponding value.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="memcachedClientName">配置的客户端名字</param>
        /// <returns>true, if success</returns>
        public bool Delete(string memcachedClientName, string keyName)
        {
            lock (this)
            {
                return MemcachedClient.GetInstance(memcachedClientName).Delete(keyName);
            }
        }

        #endregion

        #region flush

        /// <summary>
        /// This method corresponds to the "flush_all" command in the memcached protocol.
        /// When this method is called, it will send the flush command to all servers, thereby deleting
        /// all items on all servers.
        /// </summary>
        /// <returns>true, if success</returns>
        public bool FlushAll()
        {
            lock (this)
            {
                return MemcachedClient.GetInstance(memcleintName).FlushAll();
            }
        }

        /// <summary>
        /// This method corresponds to the "flush_all" command in the memcached protocol.
        /// When this method is called, it will send the flush command to all servers, thereby deleting
        /// all items on all servers.
        /// </summary>
        /// <param name="memcachedClientName">配置的客户端名字</param>
        /// <returns>true, if success</returns>
        public bool FlushAll(string memcachedClientName)
        {
            lock (this)
            {
                return MemcachedClient.GetInstance(memcachedClientName).FlushAll();
            }
        }

        #endregion

    }
}

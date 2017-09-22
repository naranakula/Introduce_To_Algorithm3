using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.Record.Aggregates.Chart;
using StackExchange.Redis;

namespace Introduce_To_Algorithm3.OpenSourceLib.Redis_
{
    /// <summary>
    /// Redis是开源内存data structure store，可以作为db\cache\message broker
    /// 在使用Redis之前考虑下CacheHelper，减少系统复杂性
    /// Redis>memcached
    /// </summary>
    public class RedisClientHelper
    {
        /*
         
redis客户端:StackExchange.Redis
客户端官网:https://github.com/StackExchange/StackExchange.Redis

redis官网:https://redis.io/

redis官方不支持windows(memcached也不支持,不建议使用memcached)
Microsoft open tech group支持win64版本地址:https://github.com/MicrosoftArchive/redis
         */

        /*
         * StackExchange.Redis的核心是ConnectionMultiplexer，it hides details of multiple servers.
         * ConnectionMultiplexer是shared and reused.
         * 它是thread-safe的。应该创建后重用。当然也可以创建多个。
         */
        
        /// <summary>
        /// redis客户端连接
        /// </summary>
        private volatile ConnectionMultiplexer redisClient = null;

        /// <summary>
        /// 锁
        /// </summary>
        private object locker = new object();



        /*
         * Configuration配置使用,分割，配置项也使用,分割，name=value格式,没有=表示server
         * redis0:6380,redis1:6380,allowAdmin=true
         * 配置项如下：
         * Configuration string	ConfigurationOptions	Default	Meaning
         * abortConnect={bool}	AbortOnConnectFail	true (false on Azure)	If true, Connect will not create a connection while no servers are available
         * allowAdmin={bool}	AllowAdmin	false	Enables a range of commands that are considered risky
         * channelPrefix={string}	ChannelPrefix	null	Optional channel prefix for all pub/sub operations
         * connectRetry={int}	ConnectRetry	3	The number of times to repeat connect attempts during initial Connect
         * connectTimeout={int}	ConnectTimeout	5000	Timeout (ms) for connect operations
         * configChannel={string}	ConfigurationChannel	__Booksleeve_MasterChanged	Broadcast channel name for communicating configuration changes
configCheckSeconds={int}	ConfigCheckSeconds	60	Time (seconds) to check configuration. This serves as a keep-alive for interactive sockets, if it is supported.
         * defaultDatabase={int}	DefaultDatabase	null	Default database index, from 0 to databases - 1
         * keepAlive={int}	KeepAlive	-1	Time (seconds) at which to send a message to help keep sockets alive (60 sec default)
         * name={string}	ClientName	null	Identification for the connection within redis
         * password={string}	Password	null	Password for the redis server
         * proxy={proxy type}	Proxy	Proxy.None	Type of proxy in use (if any); for example “twemproxy”
         * resolveDns={bool}	ResolveDns	false	Specifies that DNS resolution should be explicit and eager, rather than implicit
         * serviceName={string}	ServiceName	null	Not currently implemented (intended for use with sentinel)
ssl={bool}	Ssl	false	Specifies that SSL encryption should be used
sslHost={string}	SslHost	null	Enforces a particular SSL host identity on the server’s certificate
sslProtocols={enum}	SslProtocols	null	Ssl/Tls versions supported when using an encrypted connection. Use ‘|’ to provide multiple values.
syncTimeout={int}	SyncTimeout	1000	Time (ms) to allow for synchronous operations
tiebreaker={string}	TieBreaker	__Booksleeve_TieBreak	Key to use for selecting a server in an ambiguous master scenario
version={string}	DefaultVersion	(3.0 in Azure, else 2.0)	Redis version level (useful when the server does not make this available)
writeBuffer={int}	WriteBuffer	4096	Size of the output buffer
         * 
         *重连只能代码中配置
         * ReconnectRetryPolicy (IReconnectRetryPolicy) - Default: ReconnectRetryPolicy = LinearRetry(ConnectTimeout);
         * 
         * 
         * 
         * 
         * 

         */

        /// <summary>
        /// 启动连接
        /// 合适的配置   localhost:6379,connectRetry=3,connectTimeout=5000,keepAlive=60
        /// 含义：在connect时重试次数,connect超时5s,keepAlive发送消息检查连接
        /// </summary>
        /// <param name="configuration">配置，逗号分割  server1:6379,server2:6379</param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool Start(string configuration,Action<Exception> exceptionHandler = null)
        {
            lock (locker)
            {
                if (redisClient != null)
                {
                    return true;
                }
            }

            try
            {
                ConfigurationOptions options = ConfigurationOptions.Parse(configuration);
                //重试时间间隔从4000ms增长到20000ms
                options.ReconnectRetryPolicy = new ExponentialRetry(4000,16000);
                redisClient = ConnectionMultiplexer.Connect(options);
                
                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }


        public bool Visit(Action<IDatabase> action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                int databaseNumber = 0;
                //IDatabase是廉价对象，不需要缓存使用
                IDatabase db = redisClient.GetDatabase(databaseNumber);

                //在IDatabase调用redis api
                action(db);

                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        #region IDatabase操作

        /// <summary>
        /// Indicates whether any servers are connected
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            try
            {
                return redisClient.IsConnected;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Redis中string是binary的。 string和byte[]均可作为key和value  
        //RedisKey可以隐式的与string和byte[]进行转换

        #region 设置超时，检测存在，删除

        /// <summary>
        /// EXISTS key [key ...]
        /// TIME：O(1)
        /// 返回key是否存在
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyExists(IDatabase db, string key)
        {
            return db.KeyExists(key);
        }

        /// <summary>
        /// Removes the specified key. A key is ignored if it does not exist.
        /// DEL key [key ...]
        /// TIME:O(1)
        /// 如果key被删除了，返回true
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyDelete(IDatabase db, string key)
        {
            return db.KeyDelete(key);
        }

        /// <summary>
        /// TIME：O（1）
        /// 
        /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted.
        /// 设置成功返回true，如果key不存在，则返回false。
        /// expiry为null，移除超时时间
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(IDatabase db, RedisKey key, TimeSpan? expiry)
        {
            return db.KeyExpire(key,expiry);
        }

        /// <summary>
        /// TTL key
        /// TIME： O(1)
        /// Returns the remaining time to live of a key that has a timeout.
        /// or nil when key does not exist or does not have a timeout.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public TimeSpan? KeyTimeToLive(IDatabase db, RedisKey key)
        {
            return db.KeyTimeToLive(key);
        }


        /// <summary>
        /// TYPE key
        /// TIME:O（1）
        /// 
        /// 返回type of key, or none when key does not exist.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisType KeyType(IDatabase db, RedisKey key)
        {
            return db.KeyType(key);
        }

        /// <summary>
        /// PING
        /// This command is often used to test if a connection is still alive, or to measure latency.
        /// 返回调用延迟
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public TimeSpan Ping(IDatabase db)
        {
            return db.Ping();
        }


        #endregion



        #region String

        /// <summary>
        /// SET key value [EX seconds] [PX milliseconds] [NX|XX]
        /// time: O(1)
        /// 设置key为value，如果key已经存在则覆盖，并且超时时间也覆盖
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeToLive">如果为null，表示不超时</param>
        public void StringSet(IDatabase db,string key,string value,TimeSpan? timeToLive)
        {
            db.StringSet(key, value, timeToLive, When.Always);
        }

        /// <summary>
        /// SET key value [EX seconds] [PX milliseconds] [NX|XX]
        /// time: O(1)
        /// 设置key为value，如果key已经存在则覆盖，并且超时时间也覆盖
        ///  redis allows raw binary data for both keys and values
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeToLive">如果为null，表示不超时</param>
        public void StringSet(IDatabase db, byte[] key, byte[] value, TimeSpan? timeToLive)
        {
            db.StringSet(key, value, timeToLive, When.Always);
        }

        /// <summary>
        /// GET key
        /// Time: O（1）
        /// 
        ///  Get the value of key. If the key does not exist the special value nil is returned
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string StringGet(IDatabase db, string key)
        {
            return db.StringGet(key);
        }

        /// <summary>
        /// GET key
        /// Time: O（1）
        /// 
        ///  Get the value of key. If the key does not exist the special value nil is returned
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] ByteStringGet(IDatabase db, string key)
        {
            return db.StringGet(key);
        }


        #endregion

        #region List  list是double linked list

        /// <summary>
        /// LINSERT key BEFORE|AFTER pivot value
        /// TIME：O（n） n是找到pivot之前搜索的数据项
        /// the length of the list after the insert operation, or -1 when the value pivot was not found.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="pivot"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListInsertBefore(IDatabase db,RedisKey key, RedisValue pivot, RedisValue value)
        {
            return db.ListInsertBefore(key, pivot, value);
        }

        /// <summary>
        /// LPOP key
        /// TIME：O（1）
        /// Removes and returns the first element of the list stored at key.
        /// 如果不存在，返回nil
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue ListLeftPop(IDatabase db, RedisKey key)
        {
            return db.ListLeftPop(key);
        }

        /// <summary>
        /// LPUSH key value [value ...]
        /// TIME:O（1）
        /// Insert the specified value at the head of the list stored at key. If key does not exist, it is created as empty list before performing the push operations.
        /// 返回the length of the list after the push operations.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListLeftPush(IDatabase db, RedisKey key, RedisValue value)
        {
            return db.ListLeftPush(key, value);
        }

        #endregion

        #endregion


        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="exceptionHandler"></param>
        public void Stop(Action<Exception> exceptionHandler = null)
        {
            if (redisClient != null)
            {
                try
                {
                    redisClient.Dispose();
                }
                catch (Exception e)
                {
                    exceptionHandler?.Invoke(e);
                }
                lock (locker)
                {
                    redisClient = null;
                }
            }
        }


    }
}

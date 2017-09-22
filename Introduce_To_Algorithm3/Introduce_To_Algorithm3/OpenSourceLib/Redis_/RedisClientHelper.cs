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

                //连接失败
                //redisClient.ConnectionFailed
                //连接恢复
                //redisClient.ConnectionRestored


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
        //RedisKey可以隐式的与string和byte[]进行转换 key和value不能为null

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
        /// PERSIST key
        /// Time complexity: O(1)
        /// 1 if the timeout was removed. 0 if key does not exist or does not have an associated timeout.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyPersist(IDatabase db, RedisKey key)
        {
            return db.KeyPersist(key);
        }


        /// <summary>
        /// TIME：O（1）
        /// 
        /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted.
        /// 设置成功返回true，如果key不存在，则返回false。
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(IDatabase db, RedisKey key, TimeSpan expiry)
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

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public Tuple<bool, TimeSpan?> TestConnect(Action<Exception> exceptionHandler =null)
        {
            Tuple<bool, TimeSpan?> result = null;
            Visit(db =>
            {
               TimeSpan ts = db.Ping();
               result = new Tuple<bool, TimeSpan?>(true,ts);
            }, ex =>
            {
                exceptionHandler?.Invoke(ex);
                result = new Tuple<bool, TimeSpan?>(false,null);
            });
            return result;
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

        /// <summary>
        /// LLEN key
        /// TIME ：O(1)
        /// Returns the length of the list stored at key. If key does not exist, it is interpreted as an empty list and 0 is returned.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(IDatabase db,RedisKey key)
        {
            return db.ListLength(key);
        }

        /// <summary>
        /// LRANGE key start stop
        /// O(n)
        /// 返回key索引的list的所有元素
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue[] ListRange(IDatabase db, RedisKey key)
        {
            return db.ListRange(key, start: 0, stop: -1);
        }

        /// <summary>
        /// LREM key count value
        /// O(N) where N is the length of the list.
        /// 移除所有等于value的元素
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListRemove(IDatabase db, RedisKey key, RedisValue value)
        {
            return db.ListRemove(key, value,count:0);
        }

        #endregion

        #region hash 建议使用

        /// <summary>
        /// HDEL key field [field ...]
        /// O(1)
        ///  Removes the specified fields from the hash stored at key.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public bool HashDelete(IDatabase db,RedisKey key, RedisValue hashField)
        {
            return db.HashDelete(key, hashField);
        }

        /// <summary>
        /// HEXISTS key field
        /// Time complexity: O(1)
        ///  1 if the hash contains field. 0 if the hash does not contain field, or key does not exist.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public bool HashExists(IDatabase db, RedisKey key, RedisValue hashField)
        {
            return db.HashExists(key, hashField);
        }

        /// <summary>
        /// HGET key field
        /// Time complexity: O(1)
        /// the value associated with field, or nil when field is not present in the hash or key does not exist.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public RedisValue HashGet(IDatabase db, RedisKey key, RedisValue hashField)
        {
            return db.HashGet(key, hashField);
        }

        /// <summary>
        /// HGETALL key
        /// Time complexity: O(N) where N is the size of the hash
        /// list of fields and their values stored in the hash, or an empty list when key does not exist.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public HashEntry[] HashGetAll(IDatabase db, RedisKey key)
        {
            return db.HashGetAll(key);
        }

        /// <summary>
        /// HKEYS key
        /// Time complexity: O(N) where N is the size of the hash.
        ///  list of fields in the hash, or an empty list when key does not exist.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue[] HashKeys(IDatabase db, RedisKey key)
        {
            return db.HashKeys(key);
        }

        /// <summary>
        /// HLEN key
        /// Time complexity: O(1)
        ///  number of fields in the hash, or 0 when key does not exist.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public long HashLength(IDatabase db, RedisKey key)
        {
            return db.HashLength(key);
        }

        /// <summary>
        /// HMSET key field value [field value ...]
        /// O(1)
        /// This command overwrites any specified fields already existing in the hash. If key does not exist, a new key holding a hash is created.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool HashSet(IDatabase db, RedisKey key, RedisValue hashField, RedisValue value)
        {
            return db.HashSet(key, hashField, value);
        }

        #endregion

        #region set 建议使用

        /// <summary>
        /// SADD key member [member ...]
        /// O(1) for each element added
        /// Add the specified members to the set stored at key. 
        /// True if the specified member was not already present in the set, else False
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetAdd(IDatabase db,RedisKey key, RedisValue value)
        {
            return db.SetAdd(key, value);
        }

        /// <summary>
        /// Time complexity: O(N) where N is the total number of elements in all given sets.
        /// 对多个集合执行交并差操作并返回结果
        /// 返回结果list with members of the resulting set.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="operation">集合的交并差操作</param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public RedisValue[] SetCombine(IDatabase db, SetOperation operation, RedisKey[] keys)
        {
            return db.SetCombine(operation, keys);
        }

        /// <summary>
        /// SISMEMBER key member
        /// Time complexity: O(1)
        /// 1 if the element is a member of the set. 0 if the element is not a member of the set, or if key does not exist.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetContains(IDatabase db, RedisKey key, RedisValue value)
        {
            return db.SetContains(key, value);
        }

        /// <summary>
        /// SCARD key
        /// Time complexity: O(1)
        /// 获取集合元素的数量
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SetLength(IDatabase db, RedisKey key)
        {
            return db.SetLength(key);
        }

        /// <summary>
        /// SMEMBERS key
        /// 返回集合所有元素
        /// O(N) where N is the set cardinality.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue[] SetMembers(IDatabase db, RedisKey key)
        {
            return db.SetMembers(key);
        }

        /// <summary>
        ///  Removes and returns a random element from the set value stored at key.
        /// Time complexity: O(1)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue SetPop(IDatabase db, RedisKey key)
        {
            return db.SetPop(key);
        }

        /// <summary>
        /// O(1),
        /// 返回集合中一个元素，不移除
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue SetRandomMember(IDatabase db, RedisKey key)
        {
            return db.SetRandomMember(key);
        }

        /// <summary>
        /// O(1)
        ///  Remove the specified member from the set stored at key. Specified members that are not a member of this set are ignored.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetRemove(IDatabase db, RedisKey key, RedisValue value)
        {
            return db.SetRemove(key, value);
        }


        #endregion

        #region sort


        /// <summary>
        /// SORT key [BY pattern] [LIMIT offset count] [GET pattern [GET pattern ...]] [ASC|DESC] [ALPHA] [STORE destination]
        ///  O(N+M*log(M)) where N is the number of elements in the list or set to sort, and M the number of returned elements. 
        /// Sorts a list, set or sorted set (numerically or alphabetically, ascending by default); 
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="order"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        public RedisValue[] Sort(IDatabase db, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Alphabetic)
        {
            return db.Sort(key, skip, take, order, sortType);
        }
        
        #endregion

        #region sorted set

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

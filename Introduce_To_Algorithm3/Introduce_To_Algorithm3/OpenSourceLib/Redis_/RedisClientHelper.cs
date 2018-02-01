using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using NPOI.HSSF.Record.Aggregates.Chart;
using StackExchange.Redis;

namespace Introduce_To_Algorithm3.OpenSourceLib.Redis_
{
    /// <summary>
    /// Redis是开源内存data structure store，可以作为db\cache\message broker
    /// 在使用Redis之前考虑下CacheHelper，减少系统复杂性
    /// Redis>memcached
    /// 
    /// redis binary string最大512m,可以是C#的string和byte[]
    /// RedisKey和RedisValue可以和string和byte[]进行隐式和显式转换
    /// 如果key不存在，返回nil，但是key存在，但不是操作的类型，会抛异常，如对hash类型进行set操作
    /// 
    /// 使用实例
    /// RedisClientHelper client = new RedisClientHelper();
    /// client.start()
    /// 
    /// 中间调用
    /// 
    /// 
    /// client.stop()
    /// 
    /// 
    /// Redis支持的数据类型:
    ///     Binary-safe string
    ///     list:根据插入顺序排列的list double linked list
    ///     set: unique,unsorted string elements
    ///     sorted set: similar to Sets but where every string element is associated to a floating number value, called score. 
    ///     hashes:  maps composed of fields associated with values. Both the field and the value are strings. 
    /// 
    /// 
    /// 
    /// 
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
        private volatile ConnectionMultiplexer _redisClient = null;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _locker = new object();

        #region 启动关闭

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
        /// 合适的配置   localhost:6379,connectRetry=3,connectTimeout=5000,keepAlive=53
        /// 含义：在connect时重试次数,connect超时5s,keepAlive 53s发送消息检查连接
        /// </summary>
        /// <param name="configuration">配置，逗号分割  server1:6379,server2:6379
        /// 合适的配置   localhost:6379,connectRetry=3,connectTimeout=5000,keepAlive=53
        /// </param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool Start(string configuration,Action<Exception> exceptionHandler = null)
        {
            lock (_locker)
            {
                if (_redisClient != null)
                {
                    return true;
                }
            }

            try
            {
                ConfigurationOptions options = ConfigurationOptions.Parse(configuration);
                //重试时间间隔从4000ms增长到20000ms
                options.ReconnectRetryPolicy = new ExponentialRetry(4000,16000);
                //首次连接，如果连接不上会报异常
                _redisClient = ConnectionMultiplexer.Connect(options);
                
                //redis配置变化
                _redisClient.ConfigurationChanged += RedisClientOnConfigurationChanged;
                //redis连接失败
                _redisClient.ConnectionFailed += RedisClientOnConnectionFailed;
                //redis连接恢复
                _redisClient.ConnectionRestored += RedisClientOnConnectionRestored;
                //a server replied with error message
                _redisClient.ErrorMessage += RedisClientOnErrorMessage;
                //内部错误
                _redisClient.InternalError += RedisClientOnInternalError;
                

                return true;
            }
            catch (Exception e)
            {
                _redisClient = null;
                exceptionHandler?.Invoke(e);
                return false;
            }
        }
        
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="exceptionHandler"></param>
        public void Stop(Action<Exception> exceptionHandler = null)
        {
            if (_redisClient != null)
            {
                try
                {
                    _redisClient.Dispose();
                }
                catch (Exception e)
                {
                    exceptionHandler?.Invoke(e);
                }
                lock (_locker)
                {
                    _redisClient = null;
                }
            }
        }

        #endregion

        #region Redis事件通知

        /// <summary>
        /// 内部错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="internalErrorEventArgs"></param>
        private void RedisClientOnInternalError(object sender, InternalErrorEventArgs internalErrorEventArgs)
        {
            NLogHelper.Error($"redis client internal error:{internalErrorEventArgs.Exception}");
        }

        /// <summary>
        /// a server replied with error message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="redisErrorEventArgs"></param>
        private void RedisClientOnErrorMessage(object sender, RedisErrorEventArgs redisErrorEventArgs)
        {
            NLogHelper.Error($"redis server出错:"+redisErrorEventArgs.Message);
        }

        /// <summary>
        /// 连接恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connectionFailedEventArgs"></param>
        private void RedisClientOnConnectionRestored(object sender, ConnectionFailedEventArgs connectionFailedEventArgs)
        {
            NLogHelper.Info($"redis连接恢复");
        }

        /// <summary>
        /// 连接失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connectionFailedEventArgs"></param>
        private void RedisClientOnConnectionFailed(object sender, ConnectionFailedEventArgs connectionFailedEventArgs)
        {
            NLogHelper.Error($"redis连接断开:"+connectionFailedEventArgs.Exception);
        }

        /// <summary>
        /// redis配置变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="endPointEventArgs"></param>
        private void RedisClientOnConfigurationChanged(object sender, EndPointEventArgs endPointEventArgs)
        {
            NLogHelper.Warn($"redis ConfigurationChanged:{endPointEventArgs.EndPoint}");
        }


        #endregion

        #region 通用任务

        /// <summary>
        /// 通用的redis任务
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool SafeAction(Action<IDatabase> action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                const int databaseNumber = 0;
                //IDatabase是廉价对象，不需要缓存使用且IDatabase没有dispose
                IDatabase db = _redisClient.GetDatabase(databaseNumber);

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

        #endregion

        #region IDatabase操作

        /// <summary>
        /// Indicates whether any servers are connected
        /// </summary>
        /// <returns></returns>
        public bool IsConnected(Action<Exception> exceptionHandler = null)
        {
            try
            {
                return _redisClient.IsConnected;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        //Redis中string是binary的。 string和byte[]均可作为key和value  
        //RedisKey可以隐式的与string和byte[]进行转换(TO AND FROM) key和value不能为null
        //RedisValue也可以隐式的与string和byte[]进行转换(TO AND FROM) RedisValue可以是基本类型如整数

        #region 设置key超时，检测存在，删除

        /// <summary>
        /// EXISTS key [key ...]
        /// TIME：O(1)
        /// 返回key是否存在
        /// http://redis.io/commands/exists
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
        /// http://redis.io/commands/del
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyDelete(IDatabase db, string key)
        {
            return db.KeyDelete(key);
        }

        /// <summary>
        /// PERSIST key 使key永不过期
        /// Time complexity: O(1)
        /// 1 if the timeout was removed. 0 if key does not exist or does not have an associated timeout.
        /// http://redis.io/commands/persist
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
        /// http://redis.io/commands/expire
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
        /// http://redis.io/commands/ttl
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
        /// http://redis.io/commands/type
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
        /// 用来测试redis服务是否仍然存活
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public TimeSpan Ping(IDatabase db)
        {
            return db.Ping();
        }

        /// <summary>
        /// 测试连接
        /// 返回(测试是否成功，ping时间）
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public Tuple<bool, TimeSpan?> TestConnect(Action<Exception> exceptionHandler =null)
        {
            Tuple<bool, TimeSpan?> result = null;
            SafeAction(db =>
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

        #region 事务

        /*
         * redis事务transaction提供如下保证:
         * 1、事务中所有的命令顺序执行， It can never happen that a request issued by another client is served in the middle of the execution of a Redis transaction. 
         * 2、原子性 either all of the commands or none are processed
         * 
         */

        #endregion

        #region String 这里指redis string,最大512m,可以是C#的string和byte[]

        //如果key不存在，返回nil，但是key存在，但不是操作的类型，会抛异常，如对hash类型进行set操作

        //在redis中,string最大可以512M

        /// <summary>
        /// SET key value [EX seconds] [PX milliseconds] [NX|XX]
        /// time: O(1)
        /// 设置key为value，如果key已经存在则覆盖，并且超时时间也覆盖
        /// redis Strings are binary safe, this means that a Redis string can contain any kind of data, for instance a JPEG image or a serialized protobuffer object.
        ///  A String value can be at max 512 Megabytes in length.
        /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
        /// http://redis.io/commands/set
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeToLive">如果为null，表示不超时</param>
        public bool StringSet(IDatabase db,RedisKey key,RedisValue value,TimeSpan? timeToLive=null)
        {
            return db.StringSet(key, value, timeToLive, When.Always);
        }

        /// <summary>
        /// GET key
        /// Time: O（1）
        /// 
        ///  Get the value of key. If the key does not exist the special value nil is returned
        /// http://redis.io/commands/mget
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue StringGet(IDatabase db, RedisKey key)
        {
            return db.StringGet(key);
        }

        /// <summary>
        /// Get the value of key. If the key does not exist the special value nil is returned. An error is returned if the value stored at key is not a string, because GET only handles string values.
        /// Time complexity: O(1)
        /// http://redis.io/commands/get
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValueWithExpiry StringGetWithExpiry(IDatabase db, RedisKey key)
        {
            return db.StringGetWithExpiry(key);
        }

        /// <summary>
        /// Returns the length of the string value stored at key.An error is returned when key holds a non-string value.
        /// Time complexity: O(1)
        /// http://redis.io/commands/strlen
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public long StringLength(IDatabase db,RedisKey key)
        {
            return db.StringLength(key);
        }


        #endregion

        //超时是在整个list\hash\set上的，而不是某一项上

        #region List  list是double linked list

        //key关联一个list,list是RedisValue的双端链表， 超时只能设置在key上

        /// <summary>
        /// return the element at index in list stored by key
        /// index is zero-based
        /// O(n) n是the number of elements to traverse to get to the element at index
        /// http://redis.io/commands/lindex
        /// 返回key关联的list的index索引的元素。
        /// 如果key不存在或index超出范围，返回nil
        /// 如果key关联的值，不是list类型，会抛异常
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public RedisValue ListGetByIndex(IDatabase db, RedisKey key, long index)
        {
            return db.ListGetByIndex(key, index);
        }

        /// <summary>
        /// 时间复杂性:O(n) n是找到pivot所需要遍历的元素数
        /// 在pivot之后插入value，如果key不存在，认为是empty list,没有操作
        /// 如果key存在但不是list类型，抛异常
        /// 
        /// 返回the length of the list after the insert operation, or -1 when the value pivot was not found.
        /// http://redis.io/commands/linsert
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="pivot"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListInsertAfter(IDatabase db, RedisKey key, RedisValue pivot, RedisValue value)
        {
            return db.ListInsertAfter(key, pivot, value);
        }


        /// <summary>
        /// LINSERT key BEFORE|AFTER pivot value
        /// TIME：O（n） n是找到pivot之前搜索的数据项
        /// the length of the list after the insert operation, or -1 when the value pivot was not found.
        /// http://redis.io/commands/linsert
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
        /// http://redis.io/commands/lpop
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
        /// 
        /// http://redis.io/commands/lpush
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
        /// Removes and returns the last element of the list stored at key.
        /// Time:O（1）
        /// 如果key不存在或者list为空返回nil
        /// 
        /// http://redis.io/commands/rpop
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue ListRightPop(IDatabase db,RedisKey key)
        {
            return db.ListRightPop(key);
        }

        /// <summary>
        /// 在list结尾处push一个元素
        /// 如果list不存在则创建list并push
        /// Time：O(1)
        /// 返回push操作后，list中元素的个数
        /// http://redis.io/commands/rpush
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListRightPush(IDatabase db, RedisKey key, RedisValue value)
        {
            return db.ListRightPush(key, value);
        }

        /// <summary>
        /// O(index)
        /// 如果索引超出，将会异常
        /// Sets the list element at index to value.
        /// https://redis.io/commands/lset
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void ListSetByIndex(IDatabase db, RedisKey key,long index, RedisValue value)
        {
            db.ListSetByIndex(key, index, value);
        }

        /// <summary>
        /// LLEN key
        /// TIME ：O(1)
        /// Returns the length of the list stored at key. If key does not exist, it is interpreted as an empty list and 0 is returned.
        /// http://redis.io/commands/llens
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
        /// 
        /// http://redis.io/commands/lrange
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
        /// http://redis.io/commands/lrem
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListRemove(IDatabase db, RedisKey key, RedisValue value)
        {
            //count = 0: Remove all elements equal to value.
            return db.ListRemove(key, value,count:0);
        }

        #endregion

        #region hash 建议使用

        //key关联一个hash,hash是field,value对的集合，hash作为一个整体，初始时默认是不过期的，不能对单独的hashFeild设置过期，只能整体过期或不过期

        /// <summary>
        /// HDEL key field [field ...]
        /// O(n) n是要移除的field的个数，在本接口中是O(1)
        ///  Removes the specified fields from the hash stored at key.
        /// 移除key索引的hash中field关联的数据项
        /// 如果key不存在或者field不存在，返回false，否则返回true
        /// http://redis.io/commands/hdel
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
        /// 如果key或者key关联的field不存在返回false，否则返回true
        /// http://redis.io/commands/hexists
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
        /// 如果key或者key关联的hash的field不存在返回nil
        /// nil可以通过RedisValue的IsNull或者IsNullOrEmpty判断
        /// http://redis.io/commands/hget
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
        /// http://redis.io/commands/hgetall
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
        /// 返回key关联的hashField的列表
        /// Returns all field names in the hash stored at key.
        /// http://redis.io/commands/hkeys
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue[] HashKeys(IDatabase db, RedisKey key)
        {
            return db.HashKeys(key);
        }

        /// <summary>
        /// Returns all values in the hash stored at key.
        /// O（n）n是key关联的hash的大小
        /// list of values in the hash, or an empty list when key does not exist.
        /// http://redis.io/commands/hvals
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue[] HashValues(IDatabase db, RedisKey key)
        {
            return db.HashValues(key);
        }


        /// <summary>
        /// HLEN key
        /// Time complexity: O(1)
        ///  number of fields in the hash, or 0 when key does not exist.
        /// http://redis.io/commands/hlen
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
        /// 如果可以不存在，创建一个key关联的hash,如果key存在并且field存在，覆盖现有的field
        /// http://redis.io/commands/hset
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

        //集合，不能有重复数据

        /// <summary>
        /// SADD key member [member ...]
        /// O(1) for each element added
        /// Add the specified members to the set stored at key. 如果值已经存在，则什么也不做，如果key不存在，则创建set
        /// True if the specified member was not already present in the set, else False
        /// http://redis.io/commands/sadd
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
        /// http://redis.io/commands/sunion
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
        /// Returns if member is a member of the set stored at key.
        /// http://redis.io/commands/sismember
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
        /// http://redis.io/commands/scard
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
        /// http://redis.io/commands/smembers
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
        /// 如果set为空或者不存在，返回nil
        /// Time complexity: O(1)
        /// http://redis.io/commands/spop
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
        /// 如果set为空或者set不存在，返回nil
        /// http://redis.io/commands/srandmember
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
        /// http://redis.io/commands/srem
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>True if the specified member was already present in the set, else False</returns>
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
        /// http://redis.io/commands/sort
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="order"></param>
        /// <param name="sortType"></param>
        /// <returns>Returns the sorted elements</returns>
        public RedisValue[] Sort(IDatabase db, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Alphabetic)
        {
            return db.Sort(key, skip, take, order, sortType);
        }

        #endregion

        #region sorted set

        //similar to Sets but where every string element is associated to a floating number value, called score. 
        //sorted set与set类似，但是sorted set每个元素都有一个关联的的double类型的score

        /// <summary>
        /// 在key索引的sorted set中添加带指定score的value
        /// O(logn) n是sorted set中的元素个数
        /// 如果member已经存在，则更新score并重新放置位置
        /// If a specified member is already a member of the sorted set, the score is updated and the element reinserted at the right position to ensure the correct ordering.
        /// 如果key不存在，则新建sorted set. 如果key存在但不是sorted set类型，则抛异常
        /// 
        /// http://redis.io/commands/zadd
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <returns>True if the value was added, False if it already existed (the score is still updated</returns>
        public bool SortedSetAdd(IDatabase db, RedisKey key, RedisValue value, double score)
        {
            return db.SortedSetAdd(key, value, score);
        }

        /// <summary>
        /// 返回sortedset的在min,max之间的元素的个数
        /// 如果key不存在，返回0
        /// Returns the number of elements in the sorted set at key with a score between min and max
        /// TIME:O(log(N)) with N being the number of elements in the sorted set.
        /// http://redis.io/commands/zcard
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public long SortedSetLength(IDatabase db,RedisKey key, double min = double.NegativeInfinity,
            double max = double.PositiveInfinity)
        {
            return db.SortedSetLength(key, min, max);
        }

        /// <summary>
        /// TIME:O(log(N)+M) with N being the number of elements in the sorted set and M the number of elements returned.
        /// sorted set从低到高排序后返回指定数量的元素
        /// 0表示第一个元素 -1表示最后一个元素 包含start和stop
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public RedisValue[] SortedSetRangeByRank(IDatabase db,RedisKey key, long start = 0, long stop = -1)
        {
            return db.SortedSetRangeByRank(key, start, stop);
        }

        /// <summary>
        /// TIME:O(log(N)+M) with N being the number of elements in the sorted set and M the number of elements returned.
        /// sorted set从低到高排序后返回指定数量的元素
        /// 0表示第一个元素 -1表示最后一个元素 包含start和stop
        /// http://redis.io/commands/zrange
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns>返回值带有score</returns>
        public SortedSetEntry[] SortedSetRangeByRankWithScores(IDatabase db,RedisKey key, long start = 0, long stop = -1)
        {
            return db.SortedSetRangeByRankWithScores(key, start, stop);
        }

        /// <summary>
        /// 返回指定score范围内的元素
        /// TIME:O(log(n)+m) n是sorted set中的元素的个数 m是返回元素的个数
        /// Returns all the elements in the sorted set at key with a score between min and max (including elements with score equal to min or max). The elements are considered to be ordered from low to high scores.
        /// http://redis.io/commands/zrangebyscore
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public RedisValue[] SortedSetRangeByScore(IDatabase db, RedisKey key, double start = double.NegativeInfinity,
            double stop = double.PositiveInfinity)
        {
            return db.SortedSetRangeByScore(key, start, stop);
        }

        /// <summary>
        /// 返回指定score范围内的元素
        /// TIME:O(log(n)+m) n是sorted set中的元素的个数 m是返回元素的个数
        /// Returns all the elements in the sorted set at key with a score between min and max (including elements with score equal to min or max). The elements are considered to be ordered from low to high scores.
        /// http://redis.io/commands/zrangebyscore
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public SortedSetEntry[] SortedSetRangeByScoreWithScores(IDatabase db,RedisKey key,
            double start = double.NegativeInfinity, double stop = double.PositiveInfinity)
        {
            return db.SortedSetRangeByScoreWithScores(key, start, stop);
        }

        /// <summary>
        /// 返回member的rank（排序）.sorted set是按score从低到高排序
        /// Time complexity: O(log(N))
        /// Returns the rank of member in the sorted set stored at key, with the scores ordered from low to high. The rank (or index) is 0-based, which means that the member with the lowest score has rank 0.
        /// http://redis.io/commands/zrank
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns>If member exists in the sorted set, the rank of member; If member does not exist in the sorted set or key does not exist, null</returns>
        public long? SortedSetRank(IDatabase db,RedisKey key, RedisValue member)
        {
            return db.SortedSetRank(key, member);
        }

        /// <summary>
        /// O(log n) n是sorted set中元素的个数
        /// Removes the specified members from the sorted set stored at key. Non existing members are ignored.
        /// http://redis.io/commands/zrem
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns>True if the member existed in the sorted set and was removed; False otherwise</returns>
        public bool SortedSetRemove(IDatabase db, RedisKey key, RedisValue member)
        {
            return db.SortedSetRemove(key, member);
        }

        /// <summary>
        ///  Returns the score of member in the sorted set at key; If member does not exist in the sorted set, or key does not exist, nil is returned.
        /// Time complexity: O(1)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public double? SortedSetScore(IDatabase db, RedisKey key, RedisValue member)
        {
            return db.SortedSetScore(key, member);
        }

        #endregion

        #endregion


    }
}

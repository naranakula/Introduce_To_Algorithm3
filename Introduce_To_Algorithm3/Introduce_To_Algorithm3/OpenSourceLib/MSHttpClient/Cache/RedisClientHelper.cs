using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Introduce_To_Algorithm3.OpenSourceLib.Cache
{
    /// <summary>
    /// Redis客户端的帮助类
    /// 该客户端使用StackExchange.Redis来实现,这是免费的
    /// 
    /// 注：Set和Get中的序列化支持可以使用BinaryFormatter\XmlSerializer\Json来序列化
    /// BinaryFormatter is terrible at versioning and refactoring, and is platform-specific. for binary: protobuf
    /// </summary>
    public class RedisClientHelper
    {
        #region 单例模式
        /// <summary>
        /// 客户端实例
        /// </summary>
        private static RedisClientHelper _instance;

        /// <summary>
        /// 支持逗号分隔的节点，默认端口号6379 如localhost,127.0.0.1:77
        /// </summary>
        private string _url;

        /// <summary>
        /// 连接Redis服务器,并创建一个实例，该实例是多线程安全的，并且允许复用
        ///  ConnectionMultiplexer  implements  IDisposable  and can be disposed when no longer required
        /// </summary>
        private ConnectionMultiplexer _multiplexer;

        /// <summary>
        /// 创建实例
        /// 支持逗号分隔的节点，默认端口号6379 如localhost,127.0.0.1:77
        /// </summary>
        /// <returns></returns>
        public static void Steup(string url)
        {
            if (_instance != null)
            {
                return;
            }

            _instance = new RedisClientHelper(url);
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        public static RedisClientHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 构建Redis实例
        /// </summary>
        /// <param name="url">支持逗号分隔的节点，默认端口号6379 如localhost,127.0.0.1:778</param>
        private RedisClientHelper(string url)
        {
            this._url = url;
            _multiplexer = Connect(url);
        }

        #endregion

        #region private Method

        /// <summary>
        /// 连接Redis服务器,并创建一个实例，该实例是多线程安全的，并且允许复用
        /// 该客户端自动识别Master服务器
        /// </summary>
        /// <param name="url">支持逗号分隔的节点，默认端口号6379 如localhost,127.0.0.1:778</param>
        /// <returns></returns>
        private ConnectionMultiplexer Connect(string url)
        {
            /*
            Configuration string	ConfigurationOptions	Meaning
            abortConnect={bool}	AbortOnConnectFail	If true, Connect will not create a connection while no servers are available
            allowAdmin={bool}	AllowAdmin	Enables a range of commands that are considered risky
            channelPrefix={string}	ChannelPrefix	Optional channel prefix for all pub/sub operations
            connectRetry={int}	ConnectRetry	The number of times to repeat connect attempts during initial Connect 
            connectTimeout={int}	ConnectTimeout	Timeout (ms) for connect operations
            configChannel={string}	ConfigurationChannel	Broadcast channel name for communicating configuration changes
            keepAlive={int}	KeepAlive	Time (seconds) at which to send a message to help keep sockets alive
            name={string}	ClientName	Identification for the connection within redis
            password={string}	Password	Password for the redis server
            proxy={proxy type}	Proxy	Type of proxy in use (if any); for example "twemproxy"
            resolveDns={bool}	ResolveDns	Specifies that DNS resolution should be explict and eager, rather than implicit
            serviceName={string}	ServiceName	Not currently implemented (intended for use with sentinel)
            ssl={bool}	Ssl	Specifies that SSL encryption should be used
            sslHost={string}	SslHost	Enforces a particular SSL host identity on the server's certificate
            syncTimeout={int}	SyncTimeout	Time (ms) to allow for synchronous operations
            tiebreaker={string}	TieBreaker	Key to use for selecting a server in an ambiguous master scenario
            version={string}	DefaultVersion	Redis version level (useful when the server does not make this available)
            writeBuffer={int}	WriteBuffer	Size of the output buffer
            */

            //配置连接项
            ConfigurationOptions options = ConfigurationOptions.Parse(url);
            //options.KeepAlive = 16;
            //options.ConnectRetry = 3;
            ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(options);

            //连接失败和重连回调函数
            //multiplexer.ConnectionFailed += multiplexer_ConnectionFailed;
            //multiplexer.ConnectionRestored += delegate(object sender, ConnectionFailedEventArgs args) {  };

            return multiplexer;
        }

        #endregion

        #region public method

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDatabase()
        {
            //The object returned from  GetDatabase  is a cheap pass-thru object, and does not need to be stored. 
            return _multiplexer.GetDatabase();
        }

        #region Set
        /// <summary>
        /// set字符串
        /// If key already holds a value, it is overwritten, regardless of its type. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime">过期时间，过期时间会重置</param>
        /// <returns>true，设置成功</returns>
        public bool SetString(string key, string value,TimeSpan expireTime)
        {
            return _multiplexer.GetDatabase().StringSet(key,value,expireTime);
        }

        /// <summary>
        /// If key already holds a value, it is overwritten, regardless of its type. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public bool Set(string key, byte[] value, TimeSpan expireTime)
        {
            return _multiplexer.GetDatabase().StringSet(key, value, expireTime);
        }

        /// <summary>
        /// If key already holds a value, it is overwritten, regardless of its type. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public bool Set(byte[] key, byte[] value, TimeSpan expireTime)
        {
            return _multiplexer.GetDatabase().StringSet(key, value, expireTime);
        }
        #endregion

        #region Get

        /// <summary>
        ///  the value of key, or nil when key does not exist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return _multiplexer.GetDatabase().StringGet(key);
        }

        /// <summary>
        ///  the value of key, or nil when key does not exist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(byte[] key)
        {
            return _multiplexer.GetDatabase().StringGet(key);
        }

        /// <summary>
        ///  the value of key, or nil when key does not exist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] GetBytes(string key)
        {
            return _multiplexer.GetDatabase().StringGet(key);
        }

        /// <summary>
        ///  the value of key, or nil when key does not exist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] GetBytes(byte[] key)
        {
            return _multiplexer.GetDatabase().StringGet(key);
        }

        #endregion

        #region Redis Sub/Pub

        /// <summary>
        ///   在频道上订阅,并回调方法
        ///  Note that all subscriptions are global: they are not scoped to the lifetime of the  ISubscriber  instance. 
        ///  ConnectionMultiplexer  will handle all the details of re-subscribing to the requested channels
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="action"></param>
        public void Subscribe(string channelName, Action<RedisChannel, RedisValue> action)
        {
            ISubscriber sub = _multiplexer.GetSubscriber();
            sub.Subscribe(channelName,action);
        }

        /// <summary>
        /// 在指定频道上发布消息
        /// 通常发布消息和订阅消息的机器是不同的
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="message"></param>
        /// <returns> the number of clients that received the message.</returns>
        public long Publish(string channelName,string message)
        {
            ISubscriber sub = _multiplexer.GetSubscriber();
            return sub.Publish(channelName,message);
        }

        /// <summary>
        /// 在指定频道上发布消息
        /// 通常发布消息和订阅消息的机器是不同的
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="message"></param>
        /// <returns> the number of clients that received the message.</returns>
        public long Publish(string channelName, byte[] message)
        {
            ISubscriber sub = _multiplexer.GetSubscriber();
            return sub.Publish(channelName, message);
        }

        #endregion

        #region Accessing individuals Servers

        /// <summary>
        /// 获取一个Redis Server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public IServer GetServer(string ip,int port)
        {
            // the object returned from  GetServer  is a cheap pass-thru object that does not need to be stored,
            IServer server = _multiplexer.GetServer(ip, port);
            return server;
        }

        #endregion

        #region 事务

        public void Transtion()
        {
            ITransaction transaction = _multiplexer.GetDatabase().CreateTransaction();
            //设置事务的检查条件
            //Constraints are basically pre-canned tests involving  WATCH , some kind of test, and a check on the result. If all the constraints pass, the  MULTI / EXEC  is issued; otherwise  UNWATCH  is issued.
            //transaction.AddCondition(Condition.HashNotExists());
            //排队事务操作
            //Note that the object returned from  CreateTransaction  only has access to the async methods - because the result of each operation will not be known until after  Execute  (or  ExecuteAsync ) has completed
            //transaction.HashSetAsync()

            bool committed = transaction.Execute();
            //if true: it was applied; if false: it was rolled back

        }

        #endregion

        #endregion
    }
}

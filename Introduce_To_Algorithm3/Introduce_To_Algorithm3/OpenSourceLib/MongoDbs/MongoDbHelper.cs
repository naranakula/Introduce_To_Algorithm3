using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Introduce_To_Algorithm3.OpenSourceLib.MongoDbs
{

    /// <summary>
    /// MongoDb帮助类
    /// nuget:MongoDB.Driver
    /// http://mongodb.github.io/mongo-csharp-driver/
    /// https://docs.mongodb.com/manual/introduction/
    /// 
    /// https://mongodb.github.io/mongo-csharp-driver/2.7/getting_started/quick_tour/
    /// </summary>
    public sealed class MongoDbHelper
    {


        /// <summary>
        /// Mongo db的连接字符串
        /// </summary>
        public const string MongoConnectionString = "mongodb://localhost:27017";



        #region 单例模式

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly Object Slocker = new object();

        /// <summary>
        /// 底层实例
        /// </summary>
        private static volatile MongoDbHelper _instance = null;



        /// <summary>
        /// 私有构造函数
        /// </summary>
        private MongoDbHelper()
        {
            
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static MongoDbHelper GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            lock (Slocker)
            {
                if (_instance == null)
                {
                    _instance = new MongoDbHelper();
                }
            }

            return _instance;
        }



        #endregion



        #region 实例变量

        /// <summary>
        /// MongoClient
        /// 
        /// MongoClient实例实际上表示一个数据库连接池（不止一个连接）。一个程序仅需要一个MongoClient实例，该实例是多线程安全的。
        /// 
        /// Typically you only create one MongoClient instance for a given cluster and use it across your application. Creating multiple MongoClients will, however, still share the same pool of connections if and only if the connection strings are identical.
        /// </summary>
        private volatile MongoClient _client;

        /// <summary>
        /// 实例锁
        /// </summary>
        private readonly object _locker = new object();


        #endregion

        #region 启动停止

        /// <summary>
        /// 启动
        /// 返回true,表示启动成功
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool Start(Action<Exception> exceptionHandler = null)
        {
            try
            {
                lock (_locker)
                {
                    if (_client == null)
                    {
                        _client = new MongoClient(MongoConnectionString);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                lock (_locker)
                {
                    _client = null;
                }

                exceptionHandler?.Invoke(e);
                return false;
            }
        }


        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            
        }


        #endregion

        #region 通用方法

        /// <summary>
        /// action safe
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public bool ActionSafe(Action<MongoClient> action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                action(_client);
                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }


        /// <summary>
        /// func safe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public T FuncSafe<T>(Func<MongoClient,T> func, Action<Exception> exceptionHandler = null)
        {
            try
            {
                return func(_client);
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return default(T);
            }
        }

        #endregion

        #region 业务方法
        
        /// <summary>
        /// 获取database, 如果不存在则新建
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public IMongoDatabase GetDatabase(string databaseName)
        {
            return _client.GetDatabase(databaseName);
        }

        /// <summary>
        /// 获取或者新建表  如果表不存在，则新建
        /// 获取类型的对象表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(IMongoDatabase database,string tableName) where T : class
        {
            return database.GetCollection<T>(tableName);
        }


        /// <summary>
        /// 获取无类型的通用的对象表
        /// 如果不存在则创建
        /// BsonDocument是通用的Bson Document
        /// Mongodb存储的都是Bson Document
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IMongoCollection<BsonDocument> GetCollection(IMongoDatabase database, string tableName)
        {
            return database.GetCollection<BsonDocument>(tableName);
        }



        #endregion


        /// <summary>
        /// 测试数据库连接是否正常
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool IsConnectionActive(string connString, Action<Exception> exceptionHandler = null)
        {
            try
            {
                //MongoClient实例实际上表示一个数据库连接池（不止一个连接）。一个程序仅需要一个MongoClient实例，该实例是多线程安全的。
                //Typically you only create one MongoClient instance for a given cluster and use it across your application. Creating multiple MongoClients will, however, still share the same pool of connections if and only if the connection strings are identical.
                MongoClient client = new MongoClient(connString);

                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }




    }
}

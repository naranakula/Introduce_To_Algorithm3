using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
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
        public const string MongoConnectionString = "mongodb://192.168.163.14:27017";



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
        public static IMongoCollection<T> GetCollection<T>(IMongoDatabase database,string tableName) where T : class
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
        public static IMongoCollection<BsonDocument> GetBsonDocumentCollection(IMongoDatabase database, string tableName)
        {
            return database.GetCollection<BsonDocument>(tableName);
        }

        #region 插入

        /// <summary>
        /// 插入单个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="document"></param>
        public static void InsertOne<T>(IMongoCollection<T> collection, T document) where T : class
        {
            collection.InsertOne(document);
        }

        /// <summary>
        /// 插入多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="docList"></param>
        public static void InsertMany<T>(IMongoCollection<T> collection, List<T> docList) where T : class
        {
            collection.InsertMany(docList);
        }



        #endregion

        #region 更新

        /// <summary>
        /// 更新最多1个document，可能是0个
        /// </summary>
        /// <param name="collection"></param>
        public static UpdateResult UpdateOne(IMongoCollection<BsonDocument> collection)
        {
            FilterDefinition<MongoDB.Bson.BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("Name", "Jack");
            UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("Name", "lcm");


            UpdateResult result = collection.UpdateOne(filter: filter, update: update);


            if (result.IsModifiedCountAvailable)
            {
                //修改的行数
                long modifiedCount = result.ModifiedCount;
            }

            return result;
        }


        /// <summary>
        /// 更新0个或者多个
        /// </summary>
        /// <param name="collection"></param>
        public static UpdateResult UpdateMany(IMongoCollection<BsonDocument> collection)
        {
            var filter = Builders<BsonDocument>.Filter.Lt("i", 100);
            var update = Builders<BsonDocument>.Update.Inc("i", 100);


            UpdateResult result = collection.UpdateMany(filter: filter, update: update);


            if (result.IsAcknowledged && result.IsModifiedCountAvailable)
            {
                //修改的行数
                long modifiedCount = result.ModifiedCount;
            }

            return result;
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除一个或者0个，至多删除一个
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static DeleteResult DeleteOne(IMongoCollection<BsonDocument> collection,Expression<Func<BsonDocument,bool>> filter)
        {
            DeleteResult result = collection.DeleteOne(filter: filter);
            return result;
        }

        /// <summary>
        /// 删除0个或者多个
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static DeleteResult DeleteMany(IMongoCollection<BsonDocument> collection, Expression<Func<BsonDocument, bool>> filter)
        {
            DeleteResult result = collection.DeleteMany(filter: filter);
            return result;
        }

        #endregion

        #region 查询


        /**
         * 
         * 可以使用BsonDocument作为所有对象的基类,也可以使用具体的对象
         * 
         * 支持skip limit sortby(descending) find
         * 
         * 
         * var collection = database.GetCollection<BsonDocument>(tableName);
         * BsonDocument model = collection.Find(r => r["Name"] == "lcm").FirstOrDefault();
         * NLogHelper.Info($"name={model}");
         * 
         * 
         */

        /// <summary>
        /// 计数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static long CountDocuments<T>(IMongoCollection<T> collection,Expression<Func<T,bool>> filter) where T : class
        {
            return collection.CountDocuments(filter:filter);
        }


        /// <summary>
        /// 获取第一个元素，如果找不到返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(IMongoCollection<T> collection,Expression<Func<T,bool>> filter) where T : class
        {
            return collection.Find(filter: filter).FirstOrDefault();
        }

        /// <summary>
        /// 获取一个过滤集合
        /// BsonDocument是所有的bson对象的抽象
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="filter">过滤</param>
        /// <param name="sortField">排序</param>
        /// <returns></returns>
        public static List<BsonDocument> GetDocumentList(IMongoCollection<BsonDocument> collection,
            Expression<Func<BsonDocument, bool>> filter,Expression<Func<BsonDocument,object>> sortField)
        {
            return collection.Find(filter: filter).SortBy(field:sortField).ToList();
        }

        /// <summary>
        /// 对于数据量较大的集合，进行遍历
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="filter"></param>
        /// <param name="foreachAction"></param>
        public static void Foreach(IMongoCollection<BsonDocument> collection,
            Expression<Func<BsonDocument, bool>> filter, Action<BsonDocument> foreachAction)
        {
            var cursor = collection.Find(filter: filter).ToCursor();

            foreach (BsonDocument docItem in cursor.ToEnumerable())
            {
                foreachAction(docItem);
            }
        }


        #endregion

        #region 管理监控相关

        /// <summary>
        /// 列出所有的数据库
        /// </summary>
        public void ListDataBase()
        {
            using (var cursor = _client.ListDatabases())
            {
                foreach (BsonDocument document in cursor.ToEnumerable())
                {
                    //{ "name" : "admin", "sizeOnDisk" : 32768.0, "empty" : false }
                    NLogHelper.Info(document.ToString());
                }
            }
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="databaseName"></param>
        public void DropDatabase(string databaseName)
        {
            _client.DropDatabase(databaseName);
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        public static void DropCollection(IMongoDatabase database,string tableName)
        {
            database.DropCollection(tableName);
        }


        public static void CreateIndex(IMongoCollection<BsonDocument> table)
        {
            //1表示升序 -1表示降序   Name表示索引字段
            CreateIndexModel<BsonDocument> indexModel = new CreateIndexModel<BsonDocument>(new BsonDocument("Name",1));
            table.Indexes.CreateOne(indexModel);
        }

        /// <summary>
        /// 列举表的索引
        /// </summary>
        /// <param name="table"></param>
        public static void ListIndex(IMongoCollection<BsonDocument> table)
        {
            using (var cursor = table.Indexes.List())
            {
                foreach (var indexItem in cursor.ToEnumerable())
                {
                    NLogHelper.Info(indexItem.ToString());
                }
            }
        }


        #endregion

        #endregion



        #region Bson 相关


        /// <summary>
        /// 创建BsonDocument
        /// 一个示例
        /// </summary>
        /// <returns></returns>
        public static BsonDocument CreateBsonDocument()
        {
            BsonDocument document = new BsonDocument();

            document["name"] = "MongoDb";
            document["type"] = "database";
            var info = document["info"] = new BsonDocument();
            info["x"] = 201;
            info["y"] = 121;

            return document;
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

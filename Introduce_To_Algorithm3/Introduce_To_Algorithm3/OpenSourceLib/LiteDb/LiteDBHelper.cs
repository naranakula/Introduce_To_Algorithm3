using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Introduce_To_Algorithm3.OpenSourceLib.LiteDb
{
    /// <summary>
    /// LiteDB的帮助类
    /// http://www.litedb.org/
    /// LiteDB - A .NET serverless NoSQL Document Store in a single data file
    /// 
    /// LiteDB stores data as Document in collections. Collections相当于table
    /// LiteDB stores document in the BSON data format.
    /// In the documents, the value of a field can be any of the BSON data types, including other documents, arrays, and arrays of documents. BSON is a fast and simple way to serialize documents in binary format.
    /// LiteDb的document直接存储类型结构信息和数据信息。
    /// LiteDB支持的数据类型如下：
    /// Null	Any .NET object with null value
    ///Int32	System.Int32
    ///Int64	System.Int64
    ///Double	System.Double
    ///String	System.String
    ///Document	System.Collection.Generic.Dictionary<string, BsonValue>
    ///Array	System.Collection.Generic.List<BsonValue>
    ///Binary	System.Byte[]
    ///ObjectId	LiteDB.ObjectId
    ///Guid	System.Guid
    ///Boolean	System.Boolean
    ///DateTime	System.DateTime 
    /// 
    /// The BsonDocument class is LiteDB's implemention of documents. Internally, a BsonDocument stores key-value pairs in a Dictionary<string, BsonValue>.
    /// </summary>
    public static class LiteDBHelper
    {
        /// <summary>
        /// 创建的数据库名
        /// </summary>
        private const string DB_NAME = "litedb.db";

        #region 通用方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler">异常处理</param>
        public static void Action(Action<LiteDatabase> action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                //打开数据库，如果存在；否则创建新的数据库
                using (LiteDatabase db = new LiteDatabase(DB_NAME))
                {
                    action(db);
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns></returns>
        public static T Func<T>(Func<LiteDatabase,T> func, Action<Exception> exceptionHandler = null) where T:class 
        {
            try
            {
                //打开数据库，如果存在；否则创建新的数据库
                using (LiteDatabase db = new LiteDatabase(DB_NAME))
                {
                    return func(db);
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return default(T);
            }
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化方法  配置数据库实体映射
        /// </summary>
        /// <param name="exceptionHandler"></param>
        public static void Init(Action<Exception> exceptionHandler=null)
        {
            //This Global instance used when no BsonMapper are passed in LiteDatabase ctor
            BsonMapper mapper = BsonMapper.Global;
            //映射的实体需满足如下性质：
            // Serialization rules:
            //     - Classes must be "public" with a public constructor (without parameters)
            //     - Properties must have public getter (can be read-only)
            //     - Entity class must have Id property, [ClassName]Id property or [BsonId] attribute
            //     - No circular references
            //     - Fields are not valid
            //     - IList, Array supports
            //     - IDictionary supports (Key must be a simple datatype - converted by ChangeType)

            //配置映射的实体

            /*mapper.Entity<Customer>()
        .Key(x => x.CustomerKey)
        .Field(x => x.Name, "customer_name")
        .Ignore(x => x.Age)
        .Index(x => x.Name, unique);*/


        }


        #endregion

    }


    #region 可以删除的测试相关内容


    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Phones { get; set; }
        public bool IsActive { get; set; }
    }


    #endregion

}

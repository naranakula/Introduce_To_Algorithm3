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
    /// 
    /// 种种迹象表明Sqlite远好于litedb
    /// http://www.litedb.org/
    /// LiteDB - A .NET serverless NoSQL Document Store in a single data file
    /// 
    /// BSON中时间是按照自1970年1月1日的毫秒数存储的，所以建议存储为UTC时间，并读取时转换
    /// 
    /// LiteDb使用_id作为主键，如果用户不指定，默认使用ObjectId类型
    /// ObjectId是12个字节的BSON类型
    /// Timestamp: Value representing the seconds since the Unix epoch (4 bytes)
    ///Machine: Machine identifier(3 bytes)
    ///Pid: Process id(2 bytes)
    ///Increment: A counter, starting with a random value(3 bytes)
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
    /// 
    /// In LiteDB, documents are stored in a collection that requires a unique _id field that acts as a primary key. Because ObjectIds are small, most likely unique, and fast to generate, LiteDB uses ObjectIds as the default value for the _id field if the _id field is not specified.
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

        #region 辅助方法

        #region objectid
        /// <summary>
        /// In LiteDB, documents are stored in a collection that requires a unique _id field that acts as a primary key. Because ObjectIds are small, most likely unique, and fast to generate, LiteDB uses ObjectIds as the default value for the _id field if the _id field is not specified.
        /// 
        /// </summary>
        /// <returns></returns>
        public static ObjectId NewObjectId()
        {
            //ToString()返回24位长度字符
            return ObjectId.NewObjectId();
        }

        /// <summary>
        /// 转换为objectid
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static ObjectId ObjectIdFromHexString(string hexString)
        {
            return new ObjectId(hexString);
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public static string ObjectIdToString(ObjectId objectId)
        {
            return objectId.ToString();
        }
        #endregion

        #region BsonDocument

        //The BsonDocument class is LiteDB's implementation of documents. Internally, a BsonDocument stores key-value pairs in a Dictionary<string, BsonValue>.

        /*
 Keys must contains only letters, numbers or _ and -
Keys are case-sensitive
Duplicate keys are not allowed
LiteDB keeps the original key order, including mapped classes. The only exception is for _id field that will always be the first field.


About document field values:
Values can be any BSON value data type: Null, Int32, Int64, Double, String, Embedded Document, Array, Binary, ObjectId, Guid, Boolean, DateTime, MinValue, MaxValue
When a field is indexed, the value must be less then 512 bytes after BSON serialization.
Non-indexed values themselves have no size limit, but the whole document is limited to 1Mb after BSON serialization. This size includes all extra bytes that are used by BSON.
_id field cannot be: Null, MinValue or MaxValue
_id is unique indexed field, so value must be less then 512 bytes
         * 
         */


        /// <summary>
        /// 创建一个document，永远使用_id作为主键
        /// </summary>
        /// <returns></returns>
        public static BsonDocument NewBsonDocument()
        {
            BsonDocument document = new BsonDocument();
            document["_id"] = ObjectId.NewObjectId();
            document["Name"] = "John Doe";
            document["CreateDate"] = DateTime.Now;
            document["Phones"] = new BsonArray { "8000-0000", "9000-000" };
            document["IsActive"] = true;
            document["IsAdmin"] = new BsonValue(true);
            return document;
        }


        #endregion

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

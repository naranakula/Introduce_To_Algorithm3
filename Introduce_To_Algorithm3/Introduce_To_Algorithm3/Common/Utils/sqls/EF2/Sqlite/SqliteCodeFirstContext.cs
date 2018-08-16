using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using SQLite.CodeFirst;
using System.Data.SQLite;
using System.Data;
using System.Data.Common;
using Introduce_To_Algorithm3.Common.Utils.strings;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite.DbMaps;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite.DbModels;
using Introduce_To_Algorithm3.Models;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite
{
    /// <summary>
    /// 通过第三方支持codefirst  Nuget组件：SQLite.CodeFirst
    /// 
    /// 右边是nuget的全部安装(按顺序):1、EntityFramework 2、System.Data.SQLite(自动添加依赖System.Data.SQLite.Core\System.Data.SQLite.Linq\System.Data.SQLite.EF6\) 3、
    /// 接下来在app.config中添加下面两行
    /// <provider invariantName="System.Data.SQLite" type="System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6" />
    /// <provider invariantName="System.Data.SQLite.EF6" type="System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6" />
    /// 
    /// SQLite.CodeFirst组件也依赖于System.Data.SQLite，但是Nuget中没有体现。经测试可以使用。
    /// 有下列初始化方式：
    /// SqliteCreateDatabaseIfNotExists
    /// SqliteDropCreateDatabaseAlways
    /// SqliteDropCreateDatabaseWhenModelChanges
    /// // ToTable("Person").HasKey(p=>p.PersonId);//设置表名和主键
    /// 主键数据库自动生成， 即自增主键
    /// //Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
    /// sqlite建议使用long做主键  实际上更应该使用字符串 guid做主键
    /// 已经做过测试，可用
    /// sqlite是区分大小写的
    /// limit 返回数据项的数量
    /// https://www.sqlite.org/
    /// http://system.data.sqlite.org
    ///     
    /// 每个表都应该有三个字段:
    ///         string Id             主键
    ///         DateTime UpdateTime   数据最近一次更新时间  创建时等于创建时间
    ///         DateTime CreateTime   数据创建时间 
    /// 
    /// </summary>
   // [DbConfigurationType(typeof(SQLiteConfiguration))]
    public class SqliteCodeFirstContext:DbContext
    {

        /*
         Sqlite Supports terabyte-sized databases and gigabyte-sized strings and blobs. 
         Sqlite的类型是和值一起存储的，因此支持动态类型。
         Sqlite只支持5种类型：NULL,INTEGER（有符号整形，最长8个字节），REAL(小数 8个字节) TEXT(文本，使用database的encoding)，BLOB(二进制数)
         Sqlite字段没有长度限制

         SQLite does not have a separate Boolean storage class. Instead, Boolean values are stored as integers 0 (false) and 1 (true).

         SQLite does not have a storage class set aside for storing dates and/or times. Instead, the built-in Date And Time Functions of SQLite are capable of storing dates and times as TEXT, REAL, or INTEGER values:

TEXT as ISO8601 strings ("YYYY-MM-DD HH:MM:SS.SSS").
REAL as Julian day numbers, the number of days since noon in Greenwich on November 24, 4714 B.C. according to the proleptic Gregorian calendar.
INTEGER as Unix Time, the number of seconds since 1970-01-01 00:00:00 UTC.


         */

        /// <summary>
        /// 连接字符串
        /// </summary>
        public const string ConnectionStr = "name=SqliteConStr";

        /// <summary>
        /// 底层连接字符串
        /// </summary>
        private static volatile string _connString = "";


        public static String TrueConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(_connString))
                {
                    return _connString;
                }

                try
                {
                    string conName = ConnectionStr.Split(new char[] {'='}, StringSplitOptions.RemoveEmptyEntries)[1];

                    _connString = System.Configuration.ConfigurationManager.ConnectionStrings[conName].ConnectionString;
                    return _connString;
                }
                catch 
                {
                    _connString = "";
                    return "";
                }
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public SqliteCodeFirstContext() : base(ConnectionStr)
        {

        }
        


        #region 定义DbSet表

        /// <summary>
        /// 键值对表  不用加virtual
        /// </summary>
        public DbSet<KvPair> KvPairs { get; set; }

        /// <summary>
        /// 带类型字典表
        /// </summary>
        public DbSet<DictItem> DictItems { get; set; }

        /// <summary>
        /// 缓存项
        /// </summary>
        public DbSet<CacheItem> CacheItems { get; set; }

        /// <summary>
        /// 键值对表  不用加virtual
        /// </summary>
        public DbSet<KvBytesPair> KvBytesPairs { get; set; }

        /// <summary>
        /// 带类型字典表
        /// </summary>
        public DbSet<DictBytesItem> DictBytesItems { get; set; }

        /// <summary>
        /// 缓存项
        /// </summary>
        public DbSet<CacheBytesItem> CacheBytesItems { get; set; }



        /// <summary>
        /// 列表项
        /// </summary>
        public DbSet<ListItem> ListItems { get; set; }

        /// <summary>
        /// 列表项
        /// </summary>
        public DbSet<ListBytesItem> ListBytesItems { get; set; }

        /// <summary>
        /// 日志项
        /// </summary>
        public DbSet<LogItem> LogItems { get; set; }

        /// <summary>
        /// 本地配置
        /// </summary>
        public DbSet<LocalConfig> LocalConfigs { get; set; }

        #endregion

        /// <summary>
        /// This method is called when the model for a derived(派生) context has been initialized, but before the model has been locked down and used to initialize the context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //设置初始化器，不像EfDbContext，这个不需要单独的Init函数
            //使用最新版的EF6.1.3，EF6.0不行
            var sqliteInitializer = new SqliteCreateDatabaseIfNotExists<SqliteCodeFirstContext>(modelBuilder);
            //var sqliteInitializer = new SqliteDropCreateDatabaseWhenModelChanges<SqliteCodeFirstContext>(modelBuilder);
            Database.SetInitializer(sqliteInitializer);

            //设置所有的表定义映射
            modelBuilder.Configurations.Add(new ListItemMap());
            //添加KVPair对表
            modelBuilder.Configurations.Add(new KvPairMap());
            //添加字典表映射
            modelBuilder.Configurations.Add(new DictItemMap());
            modelBuilder.Configurations.Add(new CacheItemMap());

            modelBuilder.Configurations.Add(new ListBytesItemMap());
            modelBuilder.Configurations.Add(new KvBytesPairMap());
            modelBuilder.Configurations.Add(new DictBytesItemMap());
            modelBuilder.Configurations.Add(new CacheBytesItemMap());
            modelBuilder.Configurations.Add(new LogItemMap());
            modelBuilder.Configurations.Add(new LocalConfigMap());



            modelBuilder.Configurations.Add(new BaseEntityMap());


            #region SqliteSqlGenerator

            //var model = modelBuilder.Build(Database.Connection);
            //ISqlGenerator sqlGenerator = new SqliteSqlGenerator();
            //string sql = sqlGenerator.Generate(model.StoreModel);


            #endregion


        }

        #region 原生SQL调用

        /// <summary>
        /// 执行sql语句, 返回受影响的行数
        /// CREATE TABLE IF NOT EXISTS [Person] (
        /// [Id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
        /// [Name] TEXT  NULL,
        /// [CreateTime] DATE  NULL
        /// )
        /// CREATE TABLE [Person] (
        /// [Id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
        /// [Name] NVARCHAR(50)  NULL,
        /// [CreateTime] DATE  NULL
        /// );
        /// 
        /// If the database file doesn't exist, the default behaviour is to create a new file. 
        /// 当你Open时，如果数据库不存在，则创建一个
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="sql"></param>
        public static int ExecNonQuery(string sql)
        {
            using (DbConnection conn = new SQLiteConnection(TrueConnectionString))
            {
                conn.Open();
                using (DbCommand command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecNonQuery( string sql, CommandType commandType, params SQLiteParameter[] parameters)
        {
            using (DbConnection conn = new SQLiteConnection(TrueConnectionString))
            {
                conn.Open();
                using (DbCommand command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        command.Parameters.Clear();
                        foreach (SQLiteParameter sqLiteParameter in parameters)
                        {
                            command.Parameters.Add(sqLiteParameter);
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行sqlCommand
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>

        public static int ExecuteSqlCommand(string sql, params SQLiteParameter[] parameters)
        {
            using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
            {
                if (parameters == null || parameters.Length == 0)
                {
                    return context.Database.ExecuteSqlCommand(sql);
                }
                else
                {
                    return context.Database.ExecuteSqlCommand(sql,parameters);
                }
            }
        }



        #endregion

        #region  通用任务

        /// <summary>
        /// 使用EfDbContext执行通用任务
        /// </summary>
        /// <param name="action"></param>
        public static void Action(Action<SqliteCodeFirstContext> action)
        {
            using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
            {
                action(context);
            }
        }

        /// <summary>
        /// 使用EfDbContext执行通用任务,并返回一个值
        /// </summary>
        /// <param name="func"></param>
        public static T Func<T>(Func<SqliteCodeFirstContext, T> func)
        {
            using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
            {
                return func(context);
            }
        }

        /// <summary>
        /// 安全的使用EfDbContext执行通用任务
        /// </summary>
        /// <param name="action">数据库操作</param>
        /// <param name="exceptionHanlder">异常处理</param>
        public static bool ActionSafe(Action<SqliteCodeFirstContext> action,Action<Exception> exceptionHanlder = null)
        {
            try
            {
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    action(context);
                }

                return true;
            }
            catch (Exception ex)
            {
                if (exceptionHanlder != null)
                {
                    exceptionHanlder(ex);
                }

                return false;
            }
        }

        /// <summary>
        /// 安全的使用EfDbContext执行通用任务,并返回一个值
        /// </summary>
        /// <param name="func"></param>
        /// <param name="exceptionHanlder">异常处理</param>
        public static T FuncSafe<T>(Func<SqliteCodeFirstContext, T> func, Action<Exception> exceptionHanlder = null)
        {
            try
            {
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    return func(context);
                }
            }
            catch (Exception ex)
            {
                if (exceptionHanlder != null)
                {
                    exceptionHanlder(ex);
                }

                return default(T);
            }
        }

        /// <summary>
        /// 安全的使用EfDbContext执行通用任务,并返回一个值
        /// </summary>
        /// <param name="func"></param>
        /// <param name="exceptionHanlder">异常处理</param>
        public static T FuncSafe<T>(Func<SqliteCodeFirstContext, T> func, Func<Exception,T> exceptionHanlder = null)
        {
            try
            {
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    return func(context);
                }
            }
            catch (Exception ex)
            {
                if (exceptionHanlder != null)
                {
                    return exceptionHanlder(ex);
                }
                else
                {
                    return default(T);
                }
            }
        }

        #endregion

        #region List 列表

        //比较简单不加了


        #endregion


        #region 键值对相关 经过测试

        /// <summary>
        /// 插入键值对，如果对应的键已经存在则更新，否则新增记录
        /// 键为null或空白，则什么也不做 值为null或者空白仍然添加或者更新
        /// 键在数据库中是按小写存的
        /// 注：即使在某些多线程同时写的极端情况，有唯一键保证，不会创建多条记录
        /// 当发生UNIQUE约束冲突，先存在的，导致冲突的行在更改或插入发生冲突的行之前被删除。这样，更改和插入总是被执行。命令照常执行且不返回错误信息。(经过测试)
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="value">值,数据库中按原样保存</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns>返回新增或修改数据项，如果没有返回null</returns>
        public static KvPair AddOrUpdateKvPair(string key, string value,Action<Exception> exceptionHandler =  null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                //键为null或空白，则什么也不做
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，有唯一键保证，不会创建多条记录
                    KvPair result = context.KvPairs.FirstOrDefault(r => r.Key == normalizedKey);
                    if (result == null)
                    {
                        KvPair newKvPair = new KvPair();
                        newKvPair.Key = normalizedKey;
                        newKvPair.Value = value;
                        newKvPair.UpdateTime = newKvPair.CreateTime = DateTime.Now;
                        context.KvPairs.Add(newKvPair);
                        context.SaveChanges();
                        return newKvPair;
                    }
                    else
                    {
                        //即使键值对没有变化，不更新
                        if (result.Value != value)
                        {
                            result.Value = value;
                            result.UpdateTime = DateTime.Now;
                            context.SaveChanges();
                        }
                        return result;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }
            
        }

        /// <summary>
        /// 如果键为null或空白，直接返回null
        /// 如果键对应的数据不存在，返回null
        /// 返回时，键会被归一化处理
        /// 键在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static KvPair GetKvPair(string key, Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，有唯一键保证，不会创建多条记录
                    KvPair result = context.KvPairs.FirstOrDefault(r => r.Key == normalizedKey);
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }


        /// <summary>
        /// 删除键关联的数据项，并返回
        /// 如果键为null或空白，直接返回null
        /// 如果键对应的数据不存在，返回null
        /// 返回时，键会被归一化处理
        /// 键在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static KvPair DeleteKvPair(string key, Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //在某些多线程同时写的极端情况，有了唯一键，不会创建多条记录
                    KvPair result = context.KvPairs.FirstOrDefault(r => r.Key == normalizedKey);
                    if (result != null )
                    {
                        context.KvPairs.Remove(result);
                        context.SaveChanges();
                    }

                    return result;
                }
                
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }


                return null;
            }

        }






        /// <summary>
        /// 插入键值对，如果对应的键已经存在则更新，否则新增记录
        /// 键为null或空白，则什么也不做 值为null或者空白仍然添加或者更新
        /// 键在数据库中是按小写存的
        /// 注：即使在某些多线程同时写的极端情况，有唯一键保证，不会创建多条记录
        /// 当发生UNIQUE约束冲突，先存在的，导致冲突的行在更改或插入发生冲突的行之前被删除。这样，更改和插入总是被执行。命令照常执行且不返回错误信息。(经过测试)
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="value">值,数据库中按原样保存</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns>返回新增或修改数据项，如果没有返回null</returns>
        public static KvBytesPair AddOrUpdateKvBytesPair(string key, byte[] value, Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                //键为null或空白，则什么也不做
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，有唯一键保证，不会创建多条记录
                    KvBytesPair result = context.KvBytesPairs.FirstOrDefault(r => r.Key == normalizedKey);
                    if (result == null)
                    {
                        KvBytesPair newKvBytesPair = new KvBytesPair();
                        newKvBytesPair.Key = normalizedKey;
                        newKvBytesPair.Value = value;
                        newKvBytesPair.UpdateTime = newKvBytesPair.CreateTime = DateTime.Now;
                        context.KvBytesPairs.Add(newKvBytesPair);
                        context.SaveChanges();
                        return newKvBytesPair;
                    }
                    else
                    {
                        //即使键值对没有变化，不更新
                        if (!StringUtils.EqualsEx(result.Value, value))
                        {
                            result.Value = value;
                            result.UpdateTime = DateTime.Now;
                            context.SaveChanges();
                        }
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }

        /// <summary>
        /// 如果键为null或空白，直接返回null
        /// 如果键对应的数据不存在，返回null
        /// 返回时，键会被归一化处理
        /// 键在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static KvBytesPair GetKvBytesPair(string key, Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，有唯一键保证，不会创建多条记录
                    KvBytesPair result = context.KvBytesPairs.FirstOrDefault(r => r.Key == normalizedKey);
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }


        /// <summary>
        /// 删除键关联的数据项，并返回
        /// 如果键为null或空白，直接返回null
        /// 如果键对应的数据不存在，返回null
        /// 返回时，键会被归一化处理
        /// 键在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static KvBytesPair DeleteKvBytesPair(string key, Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //在某些多线程同时写的极端情况，有了唯一键，不会创建多条记录
                    KvBytesPair result = context.KvBytesPairs.FirstOrDefault(r => r.Key == normalizedKey);
                    if (result != null)
                    {
                        context.KvBytesPairs.Remove(result);
                        context.SaveChanges();
                    }

                    return result;
                }

            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }


                return null;
            }

        }






        #endregion

        #region 字典表相关  键和类型共同确认一个数据项

        /// <summary>
        /// 添加字典表
        /// 键为null或空白，则什么也不做 值为null或者空白仍然添加或者更新
        /// 键和类型共同确认一个值
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="value"></param>
        /// <param name="type">字典类型，默认为空，键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler"></param>
        /// <returns>返回新增或者修改数据项，如果没有新增或修改返回null</returns>
        public static DictItem AddOrUpdateDictItem(string key, string value, string type = "",
            Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                //键为null或空白，则什么也不做
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，也不可能创建多条记录
                    DictItem oldDictItem = context.DictItems.FirstOrDefault(r => r.DictKey == normalizedKey && r.DictType == normalizedType);
                    if (oldDictItem == null)
                    {
                        //新增
                        DictItem newDictItem = new DictItem();
                        newDictItem.DictKey = normalizedKey;
                        newDictItem.DictType = normalizedType;
                        newDictItem.DictValue = value;
                        newDictItem.CreateTime = newDictItem.UpdateTime = DateTime.Now;
                        context.DictItems.Add(newDictItem);
                        context.SaveChanges();
                        return newDictItem;
                    }
                    else
                    {
                        //修改
                        if (oldDictItem.DictValue != value)
                        {
                            oldDictItem.DictValue = value;
                            oldDictItem.UpdateTime = DateTime.Now;
                            context.SaveChanges();
                        }

                        return oldDictItem;
                    }
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }
        }


        /// <summary>
        /// 如果键为null或空白，直接返回null
        /// 如果键和类型对应的数据不存在，返回null
        /// 键和类型共同确认一个数据项
        /// 返回时，键和类型会被归一化处理
        /// 键和类型在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="type">字典表类型 忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        public static DictItem GetDictItem(string key,string type = "", Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，可能会创建多条记录
                    DictItem result = context.DictItems.FirstOrDefault(r => r.DictKey == normalizedKey && r.DictType == normalizedType);
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }

        /// <summary>
        /// 根据类型获取字典表,如果异常，返回null
        /// </summary>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="type">字典表类型 忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        public static List<DictItem> GetDictItemsByType(string type = "", Action<Exception> exceptionHandler = null)
        {

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，有组合主键保证，不可能会创建多条记录
                    var result = context.DictItems.Where(r => r.DictType == normalizedType).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }

        /// <summary>
        /// 删除键和类型关联的数据项，并返回
        /// 如果键为null或空白，直接返回null
        /// 如果键对应的数据不存在，返回null
        /// 键和类型共同确认一个数据项
        /// 返回时，键和类型会被归一化处理
        /// 键和类型在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static DictItem DeleteDictItem(string key, string type = "", Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，可能会创建多条记录
                    DictItem result = context.DictItems.FirstOrDefault(r => r.DictKey == normalizedKey && r.DictType == normalizedType);
                    if (result != null)
                    {
                        context.DictItems.Remove(result);
                        context.SaveChanges();
                    }

                    return result;
                }

            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }


                return null;
            }

        }




        /// <summary>
        /// 添加字典表
        /// 键为null或空白，则什么也不做 值为null或者空白仍然添加或者更新
        /// 键和类型共同确认一个值
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="value"></param>
        /// <param name="type">字典类型，默认为空，键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler"></param>
        /// <returns>返回新增或者修改数据项，如果没有新增或修改返回null</returns>
        public static DictBytesItem AddOrUpdateDictBytesItem(string key, byte[] value, string type = "",
            Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                //键为null或空白，则什么也不做
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，也不可能创建多条记录
                    DictBytesItem oldDictBytesItem = context.DictBytesItems.FirstOrDefault(r => r.DictKey == normalizedKey && r.DictType == normalizedType);
                    if (oldDictBytesItem == null)
                    {
                        //新增
                        DictBytesItem newDictBytesItem = new DictBytesItem();
                        newDictBytesItem.DictKey = normalizedKey;
                        newDictBytesItem.DictType = normalizedType;
                        newDictBytesItem.DictValue = value;
                        newDictBytesItem.CreateTime = newDictBytesItem.UpdateTime = DateTime.Now;
                        context.DictBytesItems.Add(newDictBytesItem);
                        context.SaveChanges();
                        return newDictBytesItem;
                    }
                    else
                    {
                        //修改
                        if (!StringUtils.EqualsEx(oldDictBytesItem.DictValue, value))
                        {
                            oldDictBytesItem.DictValue = value;
                            oldDictBytesItem.UpdateTime = DateTime.Now;
                            context.SaveChanges();
                        }

                        return oldDictBytesItem;
                    }
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }
        }


        /// <summary>
        /// 如果键为null或空白，直接返回null
        /// 如果键和类型对应的数据不存在，返回null
        /// 键和类型共同确认一个数据项
        /// 返回时，键和类型会被归一化处理
        /// 键和类型在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="type">字典表类型 忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        public static DictBytesItem GetDictBytesItem(string key, string type = "", Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，可能会创建多条记录
                    DictBytesItem result = context.DictBytesItems.FirstOrDefault(r => r.DictKey == normalizedKey && r.DictType == normalizedType);
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }

        /// <summary>
        /// 根据类型获取字典表,如果异常，返回null
        /// </summary>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="type">字典表类型 忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        public static List<DictBytesItem> GetDictBytesItemsByType(string type = "", Action<Exception> exceptionHandler = null)
        {

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，有组合主键保证，不可能会创建多条记录
                    var result = context.DictBytesItems.Where(r => r.DictType == normalizedType).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }

        /// <summary>
        /// 删除键和类型关联的数据项，并返回
        /// 如果键为null或空白，直接返回null
        /// 如果键对应的数据不存在，返回null
        /// 键和类型共同确认一个数据项
        /// 返回时，键和类型会被归一化处理
        /// 键和类型在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static DictBytesItem DeleteDictBytesItem(string key, string type = "", Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，可能会创建多条记录
                    DictBytesItem result = context.DictBytesItems.FirstOrDefault(r => r.DictKey == normalizedKey && r.DictType == normalizedType);
                    if (result != null)
                    {
                        context.DictBytesItems.Remove(result);
                        context.SaveChanges();
                    }

                    return result;
                }

            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }


                return null;
            }

        }




        #endregion

        #region Cache相关 键和类型确认一个cache

        /// <summary>
        /// 如果添加或者修改成功返回数据项，
        /// 键为null或空白，则什么也不做 值为null或者空白仍然添加或者更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <param name="type"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static CacheItem AddOrUpdateCacheItem(string key, string value,DateTime expireTime, string type = "",
            Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                //键为null或空白，则什么也不做
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，也不可能创建多条记录
                    CacheItem cacheItem = context.CacheItems.FirstOrDefault(r => r.CacheKey == normalizedKey && r.CacheType == normalizedType);

                    if (cacheItem == null)
                    {
                        cacheItem = new CacheItem();
                        cacheItem.CacheKey = normalizedKey;
                        cacheItem.CacheType = normalizedType;
                        cacheItem.CreateTime = cacheItem.UpdateTime = DateTime.Now;
                        cacheItem.ExpireTime = expireTime;
                        cacheItem.CacheValue = value;
                        context.CacheItems.Add(cacheItem);
                        context.SaveChanges();
                    }
                    else
                    {
                        bool valueChanged = cacheItem.CacheValue != value;
                        bool expireTimeChanged = cacheItem.ExpireTime != expireTime;

                        if (valueChanged)
                        {
                            cacheItem.CacheValue = value;
                        }

                        if (expireTimeChanged)
                        {
                            cacheItem.ExpireTime = expireTime;
                        }

                        if (valueChanged || expireTimeChanged)
                        {
                            cacheItem.UpdateTime = DateTime.Now;
                            context.SaveChanges();
                        }

                       
                    }
                    return cacheItem;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }
        }



        /// <summary>
        /// 如果键为null或空白，直接返回null
        /// 如果键和类型对应的数据不存在，返回null
        /// 键和类型共同确认一个数据项
        /// 返回时，键和类型会被归一化处理
        /// 键和类型在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="type">数据项类型 忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        public static CacheItem GetCacheItem(string key, string type = "", Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，可能会创建多条记录
                    CacheItem result = context.CacheItems.FirstOrDefault(r => r.CacheKey == normalizedKey && r.CacheType == normalizedType);
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }

        /// <summary>
        /// 根据类型获取CacheItem表,如果异常，返回null
        /// </summary>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="type">字典表类型 忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        public static List<CacheItem> GetCacheItemsByType(string type = "", Action<Exception> exceptionHandler = null)
        {

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，有组合主键保证，不可能会创建多条记录
                    var result = context.CacheItems.Where(r => r.CacheType == normalizedType).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }

        /// <summary>
        /// 删除键和类型关联的数据项，并返回
        /// 如果键为null或空白，直接返回null
        /// 如果键对应的数据不存在，返回null
        /// 键和类型共同确认一个数据项
        /// 返回时，键和类型会被归一化处理
        /// 键和类型在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="type">类型</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static CacheItem DeleteCacheItem(string key, string type = "", Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，也不可能会创建多条记录
                    CacheItem result = context.CacheItems.FirstOrDefault(r => r.CacheKey == normalizedKey && r.CacheType == normalizedType);
                    if (result != null)
                    {
                        context.CacheItems.Remove(result);
                        context.SaveChanges();
                    }

                    return result;
                }

            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }


                return null;
            }

        }










        /// <summary>
        /// 如果添加或者修改成功返回数据项，
        /// 键为null或空白，则什么也不做 值为null或者空白仍然添加或者更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        /// <param name="type"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static CacheBytesItem AddOrUpdateCacheBytesItem(string key, byte[] value, DateTime expireTime, string type = "",
            Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                //键或者值为null或空白，则什么也不做
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，也不可能创建多条记录
                    CacheBytesItem cacheBytesItem = context.CacheBytesItems.FirstOrDefault(r => r.CacheKey == normalizedKey && r.CacheType == normalizedType);

                    if (cacheBytesItem == null)
                    {
                        cacheBytesItem = new CacheBytesItem();
                        cacheBytesItem.CacheKey = normalizedKey;
                        cacheBytesItem.CacheType = normalizedType;
                        cacheBytesItem.CreateTime = cacheBytesItem.UpdateTime = DateTime.Now;
                        cacheBytesItem.ExpireTime = expireTime;
                        cacheBytesItem.CacheValue = value;
                        context.CacheBytesItems.Add(cacheBytesItem);
                        context.SaveChanges();
                    }
                    else
                    {
                        bool valueChanged = !StringUtils.EqualsEx(cacheBytesItem.CacheValue, value);
                        bool expireTimeChanged = cacheBytesItem.ExpireTime != expireTime;

                        if (valueChanged)
                        {
                            cacheBytesItem.CacheValue = value;
                        }

                        if (expireTimeChanged)
                        {
                            cacheBytesItem.ExpireTime = expireTime;
                        }

                        if (valueChanged || expireTimeChanged)
                        {
                            cacheBytesItem.UpdateTime = DateTime.Now;
                            context.SaveChanges();
                        }


                    }
                    return cacheBytesItem;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }
        }



        /// <summary>
        /// 如果键为null或空白，直接返回null
        /// 如果键和类型对应的数据不存在，返回null
        /// 键和类型共同确认一个数据项
        /// 返回时，键和类型会被归一化处理
        /// 键和类型在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="type">数据项类型 忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        public static CacheBytesItem GetCacheBytesItem(string key, string type = "", Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，可能会创建多条记录
                    CacheBytesItem result = context.CacheBytesItems.FirstOrDefault(r => r.CacheKey == normalizedKey && r.CacheType == normalizedType);
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }

        /// <summary>
        /// 根据类型获取CacheBytesItem表,如果异常，返回null
        /// </summary>
        /// <param name="exceptionHandler">异常处理</param>
        /// <param name="type">字典表类型 忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        public static List<CacheBytesItem> GetCacheBytesItemsByType(string type = "", Action<Exception> exceptionHandler = null)
        {

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，有组合主键保证，不可能会创建多条记录
                    var result = context.CacheBytesItems.Where(r => r.CacheType == normalizedType).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }

        }

        /// <summary>
        /// 删除键和类型关联的数据项，并返回
        /// 如果键为null或空白，直接返回null
        /// 如果键对应的数据不存在，返回null
        /// 键和类型共同确认一个数据项
        /// 返回时，键和类型会被归一化处理
        /// 键和类型在数据库中是按小写存的
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="type">类型</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static CacheBytesItem DeleteCacheBytesItem(string key, string type = "", Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    //即使在某些多线程同时写的极端情况，也不可能会创建多条记录
                    CacheBytesItem result = context.CacheBytesItems.FirstOrDefault(r => r.CacheKey == normalizedKey && r.CacheType == normalizedType);
                    if (result != null)
                    {
                        context.CacheBytesItems.Remove(result);
                        context.SaveChanges();
                    }

                    return result;
                }

            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }


                return null;
            }

        }








        #endregion

        #region 日志相关

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logContent"></param>
        /// <param name="logSource"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool AddLog(LogType logType,string logContent, string logSource = "", Action<Exception> exceptionHandler = null)
        {
            if (String.IsNullOrWhiteSpace(logContent))
            {
                return true;
            }

            try
            {
                

                using(SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    LogItem logItem = new LogItem();
                    logItem.Id = GuidUtils.GetGuid32();
                    logItem.LogType = logType.ToString().ToLower();
                    logItem.LogSource = logSource ?? string.Empty;
                    logItem.LogContent = logContent;
                    logItem.CreateTime = DateTime.Now;

                    context.LogItems.Add(logItem);
                    context.SaveChanges();
                }
                return true;
            }
            catch(Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return false;
            }
        }

        #endregion


        #region 本地配置

        /// <summary>
        /// 添加或者更新数据项
        /// 如果添加失败返回null，否则返回数据库中数据项
        /// </summary>
        /// <param name="configKey">键不能为空，键会做归一化处理，大小写不敏感,键最长不要超过128</param>
        /// <param name="configValue">值不能为空</param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public LocalConfig AddOrUpdateLocalConfig(string configKey, string configValue,Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(configKey) || string.IsNullOrWhiteSpace(configValue))
            {
                return null;
            }

            configKey = StringUtils.Normalize(configKey);
            try
            {
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    var oldItem = context.LocalConfigs.FirstOrDefault(r => r.ConfigKey == configKey);

                    if (oldItem == null)
                    {
                        //新增
                        var newItem = new LocalConfig();
                        newItem.ConfigKey = configKey;
                        newItem.ConfigValue = configValue;
                        newItem.CreateTime = newItem.UpdateTime = DateTime.Now;
                        context.LocalConfigs.Add(newItem);
                        context.SaveChanges();
                        return newItem;
                    }
                    else if (oldItem.ConfigValue != configValue)
                    {
                        //更新
                        oldItem.ConfigValue = configValue;
                        oldItem.UpdateTime = DateTime.Now;
                        context.SaveChanges();
                        return oldItem;
                    }
                    else
                    {
                        //不变
                        return oldItem;
                    }
                }
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return null;
            }
        }


        /// <summary>
        /// 获取所有的本地配置项
        /// 配置项的键做归一化处理
        /// 如果获取失败，返回null
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public List<LocalConfig> GetAllLocalConfigList(Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    var list = context.LocalConfigs.ToList();
                    foreach (var item in list)
                    {
                        item.ConfigKey = StringUtils.Normalize(item.ConfigKey);
                    }

                    return list;
                }
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return null;
            }
        }

        /// <summary>
        /// 删除指定配置项
        /// 返回删除的配置项，如果没有返回null
        /// </summary>
        /// <param name="configKey">键将会归一化处理</param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public LocalConfig RemoveLocalConfig(string configKey,Action<Exception> exceptionHandler = null)
        {

            if (String.IsNullOrWhiteSpace(configKey))
            {
                return null;
            }

            try
            {
                configKey = StringUtils.Normalize(configKey);
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    LocalConfig removeItem = context.LocalConfigs.FirstOrDefault(r => r.ConfigKey == configKey);
                    if (removeItem != null)
                    {
                        context.LocalConfigs.Remove(removeItem);
                        context.SaveChanges();

                        return removeItem;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return null;
            }
        }


        #endregion

    }





    #region 样例 供写代码时查看使用

    /// <summary>
    /// 样例 供写代码时查看使用
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// ID 36位guid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 字符串
        /// 无需配置因为sqlite是变长的
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 不为null的时间
        /// </summary>
        public DateTime NonNullTime { get; set; }

        /// <summary>
        /// 可null时间
        /// </summary>
        public DateTime? NullableTime { get; set; }

        /// <summary>
        /// bool类型例子
        /// </summary>
        public bool BoolExample { get; set; }

        /// <summary>
        /// byte[]的例子
        /// 因为sqlite是可变类型，无需配置
        /// </summary>
        public byte[] BytesExample { get; set; }
    }

    /// <summary>
    /// 样例 供写代码时查看使用
    /// </summary>
    public class BaseEntityMap : EntityTypeConfiguration<BaseEntity>
    {
        public BaseEntityMap()
        {
            ToTable(nameof(BaseEntity)).HasKey(t => t.Id);
            //guid带-36位，不带32位
            Property(t => t.Id).IsRequired().HasMaxLength(36).IsUnicode().IsVariableLength().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            //生成如下sql
            //CREATE TABLE "BaseEntity" ([Id] nvarchar (36) NOT NULL PRIMARY KEY, [Name] nvarchar, [NonNullTime] datetime NOT NULL, [NullableTime] datetime, [BoolExample] bit NOT NULL, [BytesExample] blob (2147483647))
        }
    }


    #endregion


    #region 可删除部分


    /// <summary>
    /// 本地图片
    /// sqlite数据库表示例
    /// </summary>
    public class LocalImage
    {
        /// <summary>
        /// 主键
        /// 可以使用Long型，建议使用字符串
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 存放sqlserver数据库的Id
        /// 用于外键， 至少36位guid
        /// SQLite变长记录，字段不需要指定长度。
        /// </summary>
        public string GuidId { get; set; }


        public double Size { get; set; }

        /// <summary>
        /// 本地保存路径
        /// SQLite变长记录，字段不需要指定长度。
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// 创建时间  不会产生UTC问题,读取时全部转换为了本地时间
        /// 直接使用本地时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 数据库表映射
    /// </summary>
    public class LocalImageMap : EntityTypeConfiguration<LocalImage>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LocalImageMap()
        {
            ToTable("LocalImage").HasKey(p => p.Id);
            //自增主键
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //底层的sql:CREATE TABLE "LocalImage" ([Id] integer, [GuidId] nvarchar, [Size] float NOT NULL, [LocalPath] nvarchar, [CreateTime] datetime NOT NULL, PRIMARY KEY(Id))
            //long 底层对应 INTEGER
            //double 底层对应 REAL
            //string 底层对应 TEXT
            //datetime 底层对应 NUMERIC  再底层对应 TEXT TEXT as ISO8601 strings Use the ISO-8601 format. Uses the "yyyy-MM-dd HH:mm:ss.FFFFFFFK" format for UTC DateTime values and "yyyy-MM-dd HH:mm:ss.FFFFFFF" format for local DateTime values). 
        }
    }

    #endregion

}

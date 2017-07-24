using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using SQLite.CodeFirst;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite
{
    /// <summary>
    /// 通过第三方支持codefirst  Nuget组件：SQLite.CodeFirst
    /// 该组件也依赖于System.Data.SQLite，但是Nuget中没有体现。经测试可以使用。
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
    /// https://www.sqlite.org/
    /// http://system.data.sqlite.org
    /// </summary>
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
        private const string ConnectionStr = "name=SqliteConStr";

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
            //添加KVPair对表
            modelBuilder.Configurations.Add(new KvPairMap());
        }


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
        public static void ActionSafe(Action<SqliteCodeFirstContext> action,Action<Exception> exceptionHanlder = null)
        {
            try
            {
                using (SqliteCodeFirstContext context = new SqliteCodeFirstContext())
                {
                    action(context);
                }
            }
            catch (Exception ex)
            {
                if (exceptionHanlder != null)
                {
                    exceptionHanlder(ex);
                }
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

        #endregion

        #region 键值对相关 经过测试

        /// <summary>
        /// 插入键值对，如果对应的键已经存在则更新，否则新增记录
        /// 键或者值为null或空白，则什么也不做
        /// 
        /// 注：即使在某些多线程同时写的极端情况，有唯一键保证，不会创建多条记录
        /// 当发生UNIQUE约束冲突，先存在的，导致冲突的行在更改或插入发生冲突的行之前被删除。这样，更改和插入总是被执行。命令照常执行且不返回错误信息。(经过测试)
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="value">值,数据库中按原样保存</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static void Add(string key, string value,Action<Exception> exceptionHandler =  null)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                //键或者值为null或空白，则什么也不做
                return;
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
                    }
                    
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
        /// 如果键为null或空白，直接返回null
        /// 如果键对应的数据不存在，返回null
        /// 返回时，键会被归一化处理
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static KvPair Get(string key, Action<Exception> exceptionHandler = null)
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
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现，在数据库中全部保存了小写)</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static KvPair Delete(string key, Action<Exception> exceptionHandler = null)
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

        #endregion


    }


    #region 键值对表，字符串为键，可使用json或者xml为值 经过测试

    /// <summary>
    /// 键值对表
    /// </summary>
    public class KvPair
    {
        /// <summary>
        /// 主键
        /// 可以使用Long型，
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 键 方法保证不为空或null
        /// SQLite变长记录，字段不需要指定长度。
        /// 带有非聚簇索引IX_Key
        /// </summary>
        [Index("IX_Key", IsClustered = false)]
        //当发生UNIQUE约束冲突，先存在的，导致冲突的行在更改或插入发生冲突的行之前被删除。这样，更改和插入总是被执行。命令照常执行且不返回错误信息。
        [SQLite.CodeFirst.Unique(OnConflictAction.Replace)]//唯一键
        public string Key { get; set; }

        /// <summary>
        /// 值 方法保证不为空或null
        /// SQLite变长记录，字段不需要指定长度
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// 更新时间  不会产生UTC问题,读取时全部转换为了本地时间
        /// 直接使用本地时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间  不会产生UTC问题,读取时全部转换为了本地时间
        /// 直接使用本地时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 数据库表映射
    /// </summary>
    public class KvPairMap : EntityTypeConfiguration<KvPair>
    {
        /// <summary>
        /// 构造函数
        public KvPairMap()
        {
            ToTable("KvPair").HasKey(p => p.Id);
            //自增主键
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //底层的sql:CREATE TABLE "KvPair" ([Id] integer, [Key] nvarchar UNIQUE ON CONFLICT REPLACE, [Value] nvarchar, [UpdateTime] datetime NOT NULL, [CreateTime] datetime NOT NULL, PRIMARY KEY(Id))//sqlite默认integer主键自增
            //select * from sqlite_master
            //CREATE INDEX IX_Key ON "KvPair" (Key)
            //long 底层对应 INTEGER
            //double 底层对应 REAL
            //string 底层对应 TEXT
            //datetime 底层对应 NUMERIC  再底层对应 TEXT TEXT as ISO8601 strings Use the ISO-8601 format. Uses the "yyyy-MM-dd HH:mm:ss.FFFFFFFK" format for UTC DateTime values and "yyyy-MM-dd HH:mm:ss.FFFFFFF" format for local DateTime values). 
        }
    }

    #endregion


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

}

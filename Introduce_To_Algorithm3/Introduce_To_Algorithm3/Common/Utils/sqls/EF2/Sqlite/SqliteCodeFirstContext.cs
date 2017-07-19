using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// 
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

        /// <summary>
        /// get;set;
        /// </summary>
        public DbSet<LocalImage> LocalImages { get; set; }

        /// <summary>
        /// This method is called when the model for a derived(派生) context has been initialized, but before the model has been locked down and used to initialize the context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //设置初始化器，不像EfDbContext，这个不需要单独的Init函数
            //使用最新版的EF6.1.3，EF6.0不行
            //var sqliteInitializer = new SqliteCreateDatabaseIfNotExists<SqliteCodeFirstContext>(modelBuilder);
            var sqliteInitializer = new SqliteDropCreateDatabaseWhenModelChanges<SqliteCodeFirstContext>(modelBuilder);
            Database.SetInitializer(sqliteInitializer);

            //设置所有的表定义映射
            modelBuilder.Configurations.Add(new LocalImageMap());
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


    }


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

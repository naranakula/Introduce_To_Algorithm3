using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2
{
    #region EfDbContext 数据库类

    /// <summary>
    /// DbContext类型
    /// </summary>
    public class EfDbContext:DbContext
    {
        #region Private Member

        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// </summary>
        private static string nameOrConnectionString;

        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// </summary>
        public static string NameOrConnectionString
        {
            get { return nameOrConnectionString;}
            set { nameOrConnectionString = value; }
        }
        #endregion

        #region 构造函数

        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// </summary>
        public EfDbContext() : base(nameOrConnectionString)
        {

        }

        #endregion

        #region 静态构造函数

        /// <summary>
        /// 静态构造函数，设置 database initializer to use for the given context type. The database initializer is called when a the given System.Data.Entity.DbContext type  is used to access a database for the first time.The default strategy for Code First contexts is an instance of System.Data.Entity.CreateDatabaseIfNotExists&lt;TContext>.
        /// </summary>
        static EfDbContext()
        {
            Database.SetInitializer<EfDbContext>(null);
        }

        #endregion

        #region 覆盖方法

        /// <summary>
        /// This method is called when the model for a derived(派生) context has been initialized, but before the model has been locked down and used to initialize the context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configures the default database schema name.
            modelBuilder.HasDefaultSchema("dbo");

            //设置所有的表定义映射
            //modelBuilder.Configurations.Add(new ProvinceMap());

        }

        #endregion

        #region 定义DbSet<T>  Properties，每个DbSet<T>代表一个表

        //public DbSet<Person> Persons{get; set;}

        #endregion

        #region 静态方法

        #region  通用任务

        /// <summary>
        /// 使用EfDbContext执行通用任务
        /// </summary>
        /// <param name="action"></param>
        public static void Action(Action<EfDbContext> action)
        {
            using (EfDbContext context = new EfDbContext())
            {
                action(context);
            }
        }

        /// <summary>
        /// 使用EfDbContext执行通用任务,并返回一个值
        /// </summary>
        /// <param name="func"></param>
        public static T Func<T>(Func<EfDbContext,T> func) 
        {
            using (EfDbContext context = new EfDbContext())
            {
                return func(context);
            }
        }

        #endregion

        #region DbSet相关

        #region 查询 应尽量使用 Linq Method 或者 Linq Query

        /// <summary>
        /// Uses the primary key value to attempt to find an entity tracked by the context. If the entity is not in the context then a query will be executed and evaluated against the data in the data source, and null is returned if the entity is not found in the context or in the data source. Note that the Find also returns entities that have been added to the context but have not yet been saved to the database.
        /// 查询使用 Linq Method 或者 Linq Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Find<T>(DbSet<T> dbSet, Guid id) where T : class
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Uses the primary key value to attempt to find an entity tracked by the context. If the entity is not in the context then a query will be executed and evaluated against the data in the data source, and null is returned if the entity is not found in the context or in the data source. Note that the Find also returns entities that have been added to the context but have not yet been saved to the database.
        /// 查询使用 Linq Method 或者 Linq Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Find<T>(DbSet<T> dbSet, int id) where T : class
        {
            return dbSet.Find(id);
        }


        #region 尽量不用
        /// <summary>
        ///  Creates a raw SQL query that will return entities in this set. By default, the entities returned are tracked by the context; this can be changed by calling AsNoTracking on theDbSqlQuery<T> returned from this method.
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DbSqlQuery<T> SqlQuery<T>(DbSet<T> dbSet, string sql, params object[] parameters) where T:class
        {
            return dbSet.SqlQuery(sql, parameters);
        }

        /// <summary>
        /// A SQL query returning instances of any type, including primitive types,
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DbRawSqlQuery<T> SqlQuery<T>(DbContext dbContext, string sql, params object[] parameters) where T:class
        {
            return dbContext.Database.SqlQuery<T>(sql, parameters);
        }

        /// <summary>
        /// ExecuteSqlCommnad method is useful in sending non-query commands to the database, such as the Insert, Update or Delete command. 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteSqlCommand(DbContext dbContext, string sql, params object[] parameters)
        {
            return dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        /// Entities returned as AsNoTracking, will not be tracked by DBContext. This will be significant performance boost for read only entities. 
        /// </summary>
        /// <param name="dbQuery"></param>
        /// <returns></returns>
        public static DbQuery<T> AsNoTracking<T>(DbQuery<T> dbQuery) where T:class 
        {
            return dbQuery.AsNoTracking();
        }
        #endregion

        #endregion

        #region Add

        /// <summary>
        ///  Adds the given entity to the context the Added state. When the changes are being saved, the entities in the Added states are inserted into the database. After the changes are saved, the object state changes to Unchanged. 
        /// 
        /// 实体有5种状态： Detached,Unchanged,Added,Deleted,Modified
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Add<T>(DbSet<T> dbSet, T entity) where T : class
        {
            return dbSet.Add(entity);
        }

        /// <summary>
        /// Add a number of object
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public static IEnumerable<T> AddRange<T>(DbSet<T> dbSet, List<T> entityList) where T : class
        {
            return dbSet.AddRange(entityList);
        }

        #endregion

        #region Remove

        /// <summary>
        /// Marks the given entity as Deleted. When the changes are saved, the entity is deleted from the database. The entity must exist in the context in some other state before this method is called. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Remove<T>(DbSet<T> dbSet, T entity) where T : class
        {
            return dbSet.Remove(entity);
        }

        /// <summary>
        /// remove a number of object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public static IEnumerable<T> RemoveRange<T>(DbSet<T> dbSet, List<T> entityList) where T : class
        {
            return dbSet.RemoveRange(entityList);
        }

        #endregion

        #region 直接更改Entity状态

        /// <summary>
        /// 直接更改Entity状态
        /// 实体有5种状态： Detached,Unchanged,Added,Deleted,Modified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        /// <param name="state"></param>
        public static void StateChange<T>(DbContext dbContext, T entity, EntityState state) where T:class
        {
            dbContext.Entry(entity).State = state;
        }

        /// <summary>
        /// 将entity转换为Added状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        public static void StateAdd<T>(DbContext dbContext, T entity) where T : class
        {
            dbContext.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// 将entity转换为Modified状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        public static void StateUpdate<T>(DbContext dbContext, T entity) where T : class
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 将entity转换为Deleted状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        public static void StateDelete<T>(DbContext dbContext, T entity) where T : class
        {
            dbContext.Entry(entity).State = EntityState.Deleted;
        }

        #endregion 

        #region DbEntity相关

        /// <summary>
        /// Gets a System.Data.Entity.Infrastructure.DbEntityEntry&lt;TEntity> object for the given entity providing access to information about the entity and the ability to perform actions on the entity.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static DbEntityEntry<T> GetDbEntityEntry<T>(DbContext dbContext, T entity) where T : class
        {
            return dbContext.Entry(entity);
        }

        /// <summary>
        /// Reloads the entity from the database overwriting any property values with values from the database. The entity will be in the Unchanged state after calling this method.
        /// </summary>
        /// <param name="entry"></param>
        public static void Reload<T>(DbEntityEntry<T> entry) where T:class
        {
            entry.Reload();
        }

        /// <summary>
        /// 一个使用DbEntityEntry的例子
        /// </summary>
        /// <param name="entry"></param>
        public static void ShowDbEntityEntry<T>(DbEntityEntry<T> entry)where T:class
        {
            System.Console.WriteLine(entry.State);
            Console.WriteLine(entry.Entity);
            foreach (var propertyName in entry.CurrentValues.PropertyNames)
            {
                System.Console.WriteLine("Property Name: {0}", propertyName);

                //get original value
                var orgVal = entry.OriginalValues[propertyName];
                System.Console.WriteLine("     Original Value: {0}", orgVal);

                //get current values
                var curVal = entry.CurrentValues[propertyName];
                System.Console.WriteLine("     Current Value: {0}", curVal);
            }
        }

        /// <summary>
        /// 一个使用DbEntityEntry的例子
        /// </summary>
        /// <param name="entry"></param>
        public static void ShowDbEntityEntry(DbEntityEntry entry)
        {
            System.Console.WriteLine(entry.State);
            foreach (var propertyName in entry.CurrentValues.PropertyNames)
            {
                System.Console.WriteLine("Property Name: {0}", propertyName);

                //get original value
                var orgVal = entry.OriginalValues[propertyName];
                System.Console.WriteLine("     Original Value: {0}", orgVal);

                //get current values
                var curVal = entry.CurrentValues[propertyName];
                System.Console.WriteLine("     Current Value: {0}", curVal);
            }
        }

        #endregion

        #endregion

        #region 很少使用

        #region SaveChange

        /// <summary>
        ///  Saves all changes made in this context to the underlying database.
        /// 返回影响的行数
        /// SaveChanges不调用不会保存到数据库
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static int SaveChanges(DbContext dbContext)
        {
            return dbContext.SaveChanges();
        }

        #endregion

        #region 创建数据库和其中的表 IfNotExists
        /// <summary>
        /// if a database with the same name does not already exist on the server, Create it.
        /// True if the database did not exist and was created; false otherwise.
        /// //注：该函数创建数据库，同时创建其中的表
        /// //注：或者不需要显示调用，数据库在第一次查询、修改或者插入时，自动创建
        /// </summary>
        public static bool CreateIfNotExists()
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Database.CreateIfNotExists();
            }
        }

        #endregion

        #region DbContext To ObjectContext之间转换

        /**
         * A DbContext instance represents a combination of the Unit Of Work and Repository patterns such that it can be used to query from a database and group all changes that will then be written back to the store as a unit(事务).  DbContext is conceptually similar to ObjectContext.
         * 
         * ObjectContext is a class which manages the all Database Operation like database connection, and manages different entities of the Entity Model. We can say ObjectContext is the primary class for accessing or working together with entities which is defines in conceptual model. 
         * 
         * DbContext is conceptually similar to ObjectContext. DbContext is nothing but ObjectContext wrapper, we can say it is lightweight alternative of the ObjectContext. DbContext can be used for DataBase first, code first and model first development. DbContext mainly contains set of API that is very easy to use API exposed by ObjectContext. This APIs are also allowing us to use Code First approach that ObjectContext does not allow. ObjectContext is only useful in Model First and Database First approach。
         * The ObjectContext class is not thread-safe.Any public static members of DbContext are thread-safe. Any instance members of DbContext are not guaranteed to be thread safe.
         * EF 使用Delayed Query
         * 
         */

        /// <summary>
        /// 将高层的DbContext转换为较低阶的ObjectContext
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static ObjectContext ConvertTo(DbContext dbContext)
        {
            IObjectContextAdapter adapter = dbContext as IObjectContextAdapter;
            return adapter == null ? null : adapter.ObjectContext;
        }

        #endregion

        #endregion 

        #endregion

    }

    #endregion

    #region Database Initializer

    /*
     * 常用的Database Initializer有如下几种：
     * 
     */

    #endregion

    #region BaseEntity

    /// <summary>
    /// 基类Entity
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近一次修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }

    /// <summary>
    /// Allows configuration to be performed for an entity type in a model.
    /// 将在OnModelCreating用来配置表的定义,每个Entity均应该对应一个Map
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseMap<T> : EntityTypeConfiguration<T> where T : class
    {
        public BaseMap()
        {
            //配置表T的属性

            // ToTable("Person").HasKey(p=>p.PersonId);//设置表名和主键
            //设置属性列字段
            //Property(p => p.FirstName)
            //    .HasColumnName("FirstName")
            //    .IsOptional()
            //    .HasMaxLength(30)
            //    .IsUnicode()
            //    .IsVariableLength();
            //Property(p => p.MiddleName).IsOptional().IsFixedLength().IsUnicode(false).HasMaxLength(1);
        } 
    }

    #endregion

}

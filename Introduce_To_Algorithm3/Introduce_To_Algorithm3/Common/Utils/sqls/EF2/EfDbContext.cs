using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Hosting;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2
{
    #region EfDbContext 数据库类

    /// <summary>
    /// DbContext类型
    /// </summary>
    public class EfDbContext : DbContext
    {
        #region Private Member

        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// name=ConnString格式
        /// </summary>
        private static string _nameOrConnectionString = "name=MySqlConStr";

        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// name=ConnString格式
        /// </summary>
        public static string NameOrConnectionString
        {
            get { return _nameOrConnectionString; }
            set { _nameOrConnectionString = value; }
        }
        #endregion

        #region 构造函数

        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// name=ConnString格式
        /// 设置为保护或者私有的，是希望通过通用的action或Func实例化
        /// </summary>
        protected EfDbContext() : base(_nameOrConnectionString)
        {
            
        }

        #endregion

        #region 设置上下文

        /// <summary>
        /// 静态构造函数，设置 database initializer to use for the given context type. The database initializer is called when a the given System.Data.Entity.DbContext type  is used to access a database for the first time.The default strategy for Code First contexts is an instance of System.Data.Entity.CreateDatabaseIfNotExists&lt;TContext>.
        /// </summary>
        static EfDbContext()
        {
            //Database.SetInitializer<EfDbContext>(null);

            IDatabaseInitializer<EfDbContext> initializer;
            if (!Database.Exists(_nameOrConnectionString))
            {
                //初始化代码放在CreateDatabaseIfNotExists中
                initializer = new CreateDatabaseIfNotExists<EfDbContext>();
            }
            else
            {
                //初始化代码不要放在MigrateDatabaseToLatestVersion中
                initializer = new MigrateDatabaseToLatestVersion<EfDbContext, MigrationConfiguration>();

                ////相当于null，不进行初始化
                //initializer = new NullDatabaseInitializer<EfDbContext>();
            }

            // The database initializer is called when a the given System.Data.Entity.DbContext type is used to access a database for the first time.
            //因为第一次访问数据库时调用Seed来初始化，所以目前检查数据库是否存在并没有调用Seed
            Database.SetInitializer(initializer);

        }

        /// <summary>
        /// 不再使用静态构造函数，设置 database initializer to use for the given context type. The database initializer is called when a the given System.Data.Entity.DbContext type  is used to access a database for the first time.The default strategy for Code First contexts is an instance of System.Data.Entity.CreateDatabaseIfNotExists&lt;TContext>.
        /// 程序启动时执行，与静态构造函数二选一
        /// </summary>
        public static void Init()
        {
            //Database.SetInitializer<EfDbContext>(null);

            IDatabaseInitializer<EfDbContext> initializer;
            if (!Database.Exists(_nameOrConnectionString))
            {
                //初始化代码放在CreateDatabaseIfNotExists中
                initializer = new CreateDatabaseIfNotExists<EfDbContext>();
            }
            else
            {
                //初始化代码不要放在MigrateDatabaseToLatestVersion中
                initializer = new MigrateDatabaseToLatestVersion<EfDbContext, MigrationConfiguration>();

                ////相当于null，不进行初始化
                //initializer = new NullDatabaseInitializer<EfDbContext>();
            }

            // The database initializer is called when a the given System.Data.Entity.DbContext type is used to access a database for the first time.
            //因为第一次访问数据库时调用Seed来初始化，所以目前检查数据库是否存在并没有调用Seed
            Database.SetInitializer(initializer);
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

            #region 设置默认的Schema
            //Configures the default database schema name.
            modelBuilder.HasDefaultSchema("dbo");
            #endregion

            #region 设置Map
            //设置所有的表定义映射
            //modelBuilder.Configurations.Add(new ProvinceMap());

            //以下代码自动注册所有的Map，自动获取当前代码中的Map,并注册
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.BaseType != null && !type.IsGenericType && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            #endregion

            #region 定义Relation   One-To-Many（OneOrZero-To-Many) Many-To-Many One-to-One(单向双向) (or One-to-Zero-to-One).

            //建议在该方法中定义Relation,而不是在Map中定义
            //删除的级联在这里定义，默认是不级联删除的
            //添加默认是级联的，添加父项会自动把关联的子项添加
            //修改是级联的，因为修改的数据实际上是查出来的数据，是被context管理状态的
            //CreateOneToManyMap(modelBuilder);
            //CreateManyToManyMap(modelBuilder);
            //CreateOneToOneMap(modelBuilder);

            #endregion
        }

        #endregion

        #region 定义DbSet<T>  Properties，每个DbSet<T>代表一个表

        //public DbSet<Person> Persons{get; set;}

        /// <summary>
        ///  Returns a System.Data.Entity.DbSet<TEntity> instance for access to entities of the given type in the context and the underlying store.
        /// 有了该方法，可以不用定义DbSet Properties了
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DbSet<T> DbSet<T>() where T : class
        {
            return Set<T>();
        }

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
        public static T Func<T>(Func<EfDbContext, T> func)
        {
            using (EfDbContext context = new EfDbContext())
            {
                return func(context);
            }
        }

        /// <summary>
        /// 使用EfDbContext执行通用任务
        /// 带有事务
        /// 尽量使用隐式事务
        /// you should always call Complete() within a transaction scope to commit the transaction, even if there are only select statements within the scope. If you want to roll back the transaction, there is no specific rollback method, but rather you just don’t call Complete() and let the scope get disposed.
        /// </summary>
        /// <param name="action"></param>
        public static void ActionWithTransaction(Action<EfDbContext> action)
        {
            using (EfDbContext context = new EfDbContext())
            {
                //该范围需要一个事务。 如果已经存在环境事务，则使用该环境事务。 否则，在进入范围之前创建新的事务。 这是默认值。
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    action(context);
                    //在Complete之前的SaveChanges调用，并不会真正的保存的数据库
                    //you should always call Complete() within a transaction scope to commit the transaction, even if there are only select statements within the scope. If you want to roll back the transaction, there is no specific rollback method, but rather you just don’t call Complete() and let the scope get disposed.
                    ts.Complete();
                }

            }
        }

        /// <summary>
        /// 使用EfDbContext执行通用任务,并返回一个值
        /// 带有事务
        /// 尽量使用隐式事务
        /// you should always call Complete() within a transaction scope to commit the transaction, even if there are only select statements within the scope. If you want to roll back the transaction, there is no specific rollback method, but rather you just don’t call Complete() and let the scope get disposed.
        /// </summary>
        /// <param name="func"></param>
        public static T FuncWithTransaction<T>(Func<EfDbContext, T> func)
        {
            using (EfDbContext context = new EfDbContext())
            {
                //该范围需要一个事务。 如果已经存在环境事务，则使用该环境事务。 否则，在进入范围之前创建新的事务。 这是默认值。
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    T result = func(context);
                    //在Complete之前的SaveChanges调用，并不会真正的保存的数据库
                    //you should always call Complete() within a transaction scope to commit the transaction, even if there are only select statements within the scope. If you want to roll back the transaction, there is no specific rollback method, but rather you just don’t call Complete() and let the scope get disposed.
                    ts.Complete();
                    return result;
                }
            }
        }

        #endregion

        #region DbSet相关

        #region 查询 应尽量使用 Linq Method 或者 Linq Query

        /*
         * EF支持的String操作：First FirstOrDefault Single SinglrOrDefault Any All StartsWith EndsWith Count Sum Min Max Average ToUpper ToLower Contains Skip Take Distinct Union Intersect Except  
         * 注：string的contains类似于sql的like  IEnumerable<string>(集合中可以有null)的contains类似于 sql的 IN
         * 尽量使用DbFunctions来执行操作
         */

        /// <summary>
        /// Uses the primary key value to attempt to find an entity tracked by the context. If the entity is not in the context then a query will be executed and evaluated against the data in the data source, and null is returned if the entity is not found in the context or in the data source. Note that the Find also returns entities that have been added to the context but have not yet been saved to the database.
        /// 查询使用 Linq Method 或者 Linq Query
        /// 注：Find本质上转换为了SingleOrDefault，每个主键只能对应一项数据
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

        /// <summary>
        /// Entities returned as AsNoTracking, will not be tracked by DBContext. This will be significant performance boost for read only entities. 
        /// </summary>
        /// <param name="dbQuery"></param>
        /// <returns></returns>
        public static DbQuery<T> AsNoTracking<T>(DbQuery<T> dbQuery) where T : class
        {
            return dbQuery.AsNoTracking();
        }

        #region 直接执行命令

        /// <summary>
        ///  Creates a raw SQL query that will return entities in this set. By default, the entities returned are tracked by the context; this can be changed by calling AsNoTracking on theDbSqlQuery<T> returned from this method.
        /// 该方法用于查询。直到迭代返回结果时，sql才执行。
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DbSqlQuery<T> SqlQuery<T>(DbSet<T> dbSet, string sql, params object[] parameters) where T : class
        {
            return dbSet.SqlQuery(sql, parameters);
        }

        /// <summary>
        /// A SQL query returning instances of any type, including primitive types,
        /// 该方法用于查询。直到迭代返回结果时，sql才执行。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DbRawSqlQuery<T> SqlQuery<T>(DbContext dbContext, string sql, params object[] parameters) where T : class
        {
            return dbContext.Database.SqlQuery<T>(sql, parameters);
        }

        /// <summary>
        /// ExecuteSqlCommnad method is useful in sending non-query commands to the database, such as the Insert, Update or Delete command. 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>影响的行数</returns>
        public static int ExecuteSqlCommand(DbContext dbContext, string sql, params object[] parameters)
        {
            return dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        #endregion

        #endregion

        #region Add

        /// <summary>
        ///  Adds the given entity to the context the Added state. When the changes are being saved, the entities in the Added states are inserted into the database. After the changes are saved, the object state changes to Unchanged. 
        /// Add 会把关联的表的数据插入到数据库  Modify不会。When you set the state to modified, Entity Framework does not propagate  this change to the entire object graph.
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
        /// 
        /// 在不级联删除的情况下，它会将子项关联标志为 null（如果子项外键可null,否则抛出异常说不能置为null）,可以删除所有子项，再删除父项
        /// 尽管传递的是一个对象，实际上只有主键在起作用
        /// Remove之间必须被context管理状态，直接更改State不需要之间被Context管理
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
        public static void StateChange<T>(DbContext dbContext, T entity, EntityState state) where T : class
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
        public static void Reload<T>(DbEntityEntry<T> entry) where T : class
        {
            entry.Reload();
        }

        /// <summary>
        /// 一个使用DbEntityEntry的例子
        /// </summary>
        /// <param name="entry"></param>
        public static void ShowDbEntityEntry<T>(DbEntityEntry<T> entry) where T : class
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

        #region 关系Relation

        #region OneToOne

        /*
         * 注：实际上这是一个One-To-ZeroOrOne接口
         * One对应OneToOneLeft    ZeroOrOne对应OneToOneRight
         * 存在左边的项，并不一定存在右边的项，如果存在右边的项，右边的项只能一条
         */

        /// <summary>
        /// 对应One-To-ZeroOrOne的左边One
        /// </summary>
        public class OneToOneLeft
        {
            /// <summary>
            /// Id作为主键
            /// </summary>
            public Guid Id { get; set; }

            public virtual OneToOneRight Right { get; set; }
        }

        /// <summary>
        /// 对应One-To-ZeroOrOne的右边ZeroOrOne
        /// </summary>
        public class OneToOneRight
        {
            /// <summary>
            /// 作为主键和关联到Left的外键
            /// </summary>
            public Guid Id { get; set; }

            public virtual OneToOneLeft Left { get; set; }
        }

        /// <summary>
        /// 创建一个OneToZeroOrOne映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void CreateOneToOneMap(DbModelBuilder modelBuilder)
        {
            //Right是可选的，Left是必选的，删除是级联的， 这样可以删除right，并不会删除left，删除left时级联的删除Right
            modelBuilder.Entity<OneToOneLeft>().HasOptional(s => s.Right).WithRequired(s => s.Left).WillCascadeOnDelete(true);
            //两种写法选择一种
            //modelBuilder.Entity<OneToOneRight>().HasRequired(s=>s.Left).WithOptional(s=>s.Right).WillCascadeOnDelete(true);
        }

        #endregion

        #region OneToMany

        /*
         * 这实际上是 OneToZeroMany 模式
         * 左边是One,它可以有零个或者多个Right
         * 
         */

        /// <summary>
        /// 对应OneToMany的One
        /// </summary>
        public class OneToManyLeft
        {
            public OneToManyLeft()
            {
                //EF默认反射成HashSet
                Rights = new HashSet<OneToManyRight>();
            }

            /// <summary>
            /// 主键
            /// </summary>
            public Guid Id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public virtual ICollection<OneToManyRight> Rights { get; set; }
        }

        /// <summary>
        /// 对应OneToMany的Many
        /// </summary>
        public class OneToManyRight
        {
            /// <summary>
            /// 主键
            /// </summary>
            public Guid Id { get; set; }

            /// <summary>
            /// 外键
            /// </summary>
            public Guid LeftId { get; set; }

            /// <summary>
            /// 可以有也可以没有
            /// </summary>
            public virtual OneToManyLeft Left { get; set; }
        }

        /// <summary>
        /// 创建一个One To Many Map
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void CreateOneToManyMap(DbModelBuilder modelBuilder)
        {
            //一对多关系，可以级联删除，删除右边不会影响左边，删除左边会删除右边
            modelBuilder.Entity<OneToManyRight>().HasRequired(s => s.Left).WithMany(s => s.Rights).HasForeignKey(s => s.LeftId).WillCascadeOnDelete(true);
        }

        #endregion

        #region ManyToMany

        /*
         * 这是一个ManyToMany的映射,实际上是ZeroOrMany-To-ZeroOrMany映射
         * 在ManyToMany中左和右是没有主次之分的
         * 在删除时，删除一边，不会删除另一边，但会删除掉相应的连接表项
         */

        /// <summary>
        /// 在ManyToMany中左和右是没有主次之分的
        /// 在删除时，删除一边，不会删除另一边，但会删除掉相应的连接表项
        /// </summary>
        public class ManyToManyLeft
        {
            public ManyToManyLeft()
            {
                Rights = new HashSet<ManyToManyRight>();
            }
            /// <summary>
            /// 主键
            /// </summary>
            public Guid Id { get; set; }
            /// <summary>
            /// 外键是通过关联表来实现的，类中不需要外键
            /// </summary>
            public virtual ICollection<ManyToManyRight> Rights { get; set; }
        }

        /// <summary>
        /// 在ManyToMany中左和右是没有主次之分的
        /// 在删除时，删除一边，不会删除另一边，但会删除掉相应的连接表项
        /// </summary>
        public class ManyToManyRight
        {
            public ManyToManyRight()
            {
                Lefts = new HashSet<ManyToManyLeft>();
            }
            /// <summary>
            /// 主键
            /// </summary>
            public Guid Id { get; set; }
            /// <summary>
            /// 外键是通过关联表来实现的，类中不需要外键
            /// </summary>
            public virtual ICollection<ManyToManyLeft> Lefts { get; set; }
        }

        /// <summary>
        /// 创建一个ManyToMany Map
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void CreateManyToManyMap(DbModelBuilder modelBuilder)
        {
            //在删除时，删除一边，不会删除另一边，但会删除掉相应的连接表项
            modelBuilder.Entity<ManyToManyLeft>().HasMany(s => s.Rights).WithMany(s => s.Lefts).Map(m =>
            {
                m.MapLeftKey("LeftId");
                m.MapRightKey("RightId");
                m.ToTable("LeftRight");
            });
        }

        #endregion

        #endregion

        #region DbFunctions

        //推荐使用DbFunctions SqlDbFunctions

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
        /// //注：或者不需要显式调用，数据库在第一次查询、修改或者插入时，自动创建
        /// </summary>
        public static bool CreateIfNotExists()
        {
            using (EfDbContext context = new EfDbContext())
            {
                return context.Database.CreateIfNotExists();
            }
        }

        /// <summary>
        /// 强制检查初始化
        /// 在程序启动时，第一次查询时，会自动初始化。所以可以不显式调用
        /// By default, Code First runs the database initialization logic once per AppDomain when the context is used for the first time. 
        /// </summary>
        /// <param name="force">Specifying false will skip the initialization process if it has already executed. A value of true will initialize the database again even if it was already initialized.</param>
        public static void Initialize(bool force = false)
        {
            using (EfDbContext context = new EfDbContext())
            {
                context.Database.Initialize(force);
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
     * null(不自动创建数据库,not to execute any initialization logic at all.),CreateDatabaseIfNotExists(默认的),DropCreateDatabaseWhenModelChanges,DropCreateDatabaseAlways,MigrateDatabaseToLatestVersion
     * 
     * null等价于new NullDatabaseInitializer<T>()
     * 
     * CreateDatabaseIfNotExists class creates a database only if it doesn't exist. 这是默认的
     * 
     * DropCreateDatabaseWhenModelChanges creates a database if it doesn't exist already. Additionally, if a database is already present but there is a mismatch between the model classes and table schema then it deletes the database and re-creates it. 
     * 
     * DropCreateDatabaseAlways class deletes and creates a database whether or not it is already present or not. 
     * 
     * 
     */

    /// <summary>
    /// CreateDatabaseIfNotExists class creates a database only if it doesn't exist.
    /// </summary>
    public class EfCreateDatabaseIfNotExists : CreateDatabaseIfNotExists<EfDbContext>
    {
        /// <summary>
        /// Seed方法在每次新建数据库后调用，如果已经存在则不调用
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(EfDbContext context)
        {
            base.Seed(context);

            //在数据库中加入初始化数据
            //为了安全起见，尽量使用AddOrUpdate 实际上没有必要，只有在数据迁移时，使用AddOrUpdate
            //context.Persons.AddOrUpdate();
        }
    }

    /// <summary>
    /// DropCreateDatabaseWhenModelChanges creates a database if it doesn't exist already. Additionally, if a database is already present but there is a mismatch between the model classes and table schema then it deletes the database and re-creates it. 
    /// </summary>
    public class DropCreateDatabaseIfModelChanges : DropCreateDatabaseIfModelChanges<EfDbContext>
    {
        /// <summary>
        /// Seed方法在每次新建数据库后调用，如果没有新建数据库或者model不变，则不调用
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(EfDbContext context)
        {
            base.Seed(context);

            //在数据库中加入初始化数据
            //为了安全起见，尽量使用AddOrUpdate，实际上没有必要，只有在数据迁移时，使用AddOrUpdate
            //context.Persons.AddOrUpdate();
        }
    }

    /// <summary>
    ///DropCreateDatabaseAlways class deletes and creates a database whether or not it is already present or not. 
    /// </summary>
    public class EfDropCreateDatabaseAlways : DropCreateDatabaseAlways<EfDbContext>
    {
        /// <summary>
        /// Seed方法在每次新建数据库后调用
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(EfDbContext context)
        {
            base.Seed(context);

            //在数据库中加入初始化数据
            //为了安全起见，尽量使用AddOrUpdate 实际上没有必要，只有在数据迁移时，使用AddOrUpdate
            //context.Persons.AddOrUpdate();
        }
    }


    #region 数据库迁移

    /// <summary>
    /// 配置数据迁移
    /// </summary>
    public class MigrationConfiguration : DbMigrationsConfiguration<EfDbContext>
    {
        public MigrationConfiguration()
        {
            //必须支持允许自动迁移，这样当数据库结构改变后就可以自动迁移了
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;//允许数据损失
            //Gets or sets the string used to distinguish migrations belonging to this configuration from migrations belonging to other configurations using the same database.
            ContextKey = "CmluMigrationConfiguration";
        }

        /// <summary>
        /// If the System.Data.Entity.MigrateDatabaseToLatestVersion<TContext,TMigrationsConfiguration>database initializer is being used, then this method will be called each time that the initializer runs.
        /// 意味着每次程序启动，都会执行Seed,而不是仅仅迁移后执行
        /// Runs after upgrading to the latest migration to allow seed data to be updated.
        /// 
        /// if AutomaticMigrationsEnabled=true,the DbMigrationsConfiguration Seed method will run after each migration is applied or every time that the initializer runs.initializer is called when a the given System.Data.Entity.DbContext type is used to access a database for the first time.意味着每次迁移后或者第一次DbContext访问数据库时会调用seed，但不是每次DbContext创建时调用。 
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(EfDbContext context)
        {
            //  每次程序启动，都会执行Seed,而不是仅仅迁移后执行

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }

    /// <summary>
    /// 当AutomaticMigrationsEnabled = true时，该初始化器自动初始化到最新的Model版本
    /// </summary>
    public class DbMigrateDatabaseInitier : MigrateDatabaseToLatestVersion<EfDbContext, MigrationConfiguration>
    {

    }


    #endregion

    #endregion

    #region BaseEntity

    /// <summary>
    /// 基类Entity
    /// EF支持枚举
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 主键
        /// DatabaseGeneratedOption.Identity is used to create an auto-increment column in the table by a unique value.int类型需要，Guid类型不需要
        /// 除Sql Server外，MySql Sqlite尽量使用long或者int类型作为主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近一次修改时间
        /// ConcurrencyCheck用于检查在读取该字段到再次更新该字段时，之间是否有其他人更改过该字段，如果有抛出DbUpdateConcurrencyException异常
        /// 用于非sqlserver的并行检查，该数据类型可以是DateTime或者Guid
        /// ConcurrencyCheck只用于检查该字段
        /// DateTime可以精确到千万分之一秒
        /// </summary>
        [ConcurrencyCheck]
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 乐观锁来支持并发控制
        /// 如果同时多个用户修改，只有第一个用户修改成功，第二个用户需要处理DbUpdateConcurrencyException异常
        /// 目前只在Sql Server中支持，
        /// RowVerion和ConcurrencyCheck的区别是每次Add和Update,rowVersion会自动incremented
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }


        
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
            #region 设置表名和主键
            //设置表名和主键
            // ToTable("Person").HasKey(p=>p.PersonId);//设置表名和主键
            // Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            #endregion

            #region 设置属性列字段

            //设置属性列字段
            //字符串
            //Property(p => p.FirstName)
            //    .HasColumnName("FirstName")
            //    .IsOptional()
            //    .HasMaxLength(30)
            //    .IsUnicode()
            //    .IsVariableLength();
            //Property(p => p.MiddleName).IsOptional().IsFixedLength().IsUnicode(false).HasMaxLength(1);

            //设置Timestamp属性和调用IsRowVersion两选一，目前只在Sql Server中支持，其它数据库的并发自行百度
            //Property(t => t.RowVersion).IsRowVersion();

            //用于非sqlserver数据库的并行检查
            //Property(p => p.ModifyTime).IsConcurrencyToken();

            #endregion

            #region 设置关系 One-To-One  One-To-Many  Many-To-Many

            //建议在OnModelCreating中设置，不要在Map中设置
            #endregion
        }
    }

    #endregion

}

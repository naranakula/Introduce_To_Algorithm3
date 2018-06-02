using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Hosting;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using NLog.Internal;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;
using System.ComponentModel.DataAnnotations.Schema;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbModels;
using Introduce_To_Algorithm3.Common.Utils.strings;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbMaps;
using Introduce_To_Algorithm3.Models;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2
{
    #region EfDbContext 数据库类

    /// <summary>
    /// DbContext类型
    /// 数据库迁移只影响必须影响的部分，无关的表不影响，也就是说不会把没有映射的原来存在的表删掉
    /// 应该尽量使用Guid作为主键，因为guid可以更好的保证插入的并行性，并且合并数据库时有决定性优势
    /// fwghso from where group having select orderby
    /// 事务的默认隔离级别是数据库默认的隔离级别 对于sqlserver是READ COMMITTED  （读只能读到提交的数据）
    /// read committed解释如下
    /// 事务开始
    ///    查询1
    ///    修改1
    ///    查询2 //查询2(即使在另外一个事务中)读取不到修改1修改的数据，只能查询到savechanges之后提交的数据(即事务结束后提交的数据)
    /// 事务结束
    /// 
    /// 
    /// 注意事项：
    /// 1   主键尽量使用 string\long,不要使用guid
    /// 2   迁移只能新增字段,将现有string字段增长,新增表,新增关系(其它操作不允许)
    /// 
    /// 
    /// 每个表都应该有三个字段:
    ///         string Id             主键
    ///         DateTime UpdateTime   数据最近一次更新时间  创建时等于创建时间
    ///         DateTime CreateTime   数据创建时间 
    /// 
    /// </summary>
    public class EfDbContext : DbContext
    {
        #region Private Member

        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// name=ConnString格式
        /// 
        /// 如果是连接字符串，使用如下格式
        /// provider=System.Data.SqlClient;provider connection string="Data Source=.;Initial Catalog=AdventureWorks;Integrated Security=True"
        /// </summary>
        public const string _nameOrConnectionString = "name=SqlSeverConnString";

        //直接传递连接字符串
        //public const string ConstNameOrConnectionString = @"Server=192.168.163.12,1433;Database=TestDb;User Id=sa;Password=system2000,.;Pooling=True;Max Pool Size=31;MultipleActiveResultSets=True;";

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object _locker = new object();
        
        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// name=ConnString格式
        /// </summary>
        public static string NameOrConnectionString
        {
            get { return _nameOrConnectionString; }
        }


        /// <summary>
        /// 实际的连接字符串
        /// </summary>
        private static volatile string _trueConStrCache = string.Empty;
        

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string TrueConnectionString
        {
            get
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(_trueConStrCache))
                    {
                        return _trueConStrCache;
                    }

                    string conStrKey = _nameOrConnectionString.Split('=')[1];
                    _trueConStrCache = System.Configuration.ConfigurationManager.ConnectionStrings[conStrKey]
                        .ConnectionString.Trim();
                    return _trueConStrCache;
                }
                catch
                {
                    return "";
                }
            }
        }


        /// <summary>
        /// 数据库的名字
        /// </summary>
        private static volatile string _dbNameCache = string.Empty;


        /// <summary>
        /// 从连接字符串中获取数据库的名字
        /// </summary>
        public static string DbName
        {
            get
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(_dbNameCache))
                    {
                        return _dbNameCache;
                    }

                    string conStr = TrueConnectionString;
                    string[] items = conStr.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in items)
                    {
                        string[] keyValuePairs = item.Split(new char[] {'='}, StringSplitOptions.RemoveEmptyEntries);
                        if (keyValuePairs.Length >= 2)
                        {
                            string value = keyValuePairs[0];
                            if (StringUtils.EqualsEx(value, "database") ||
                                StringUtils.EqualsEx(value, "Initial Catalog"))
                            {
                                _dbNameCache = keyValuePairs[1].Trim();
                                break;
                            }
                        }
                    }

                    return _dbNameCache;
                }
                catch
                {
                    return "";
                }
            }
        }


        #endregion

        #region 构造函数

        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// name=ConnString格式
        /// //设置为保护或者私有的，是希望通过通用的action或Func实例化
        /// 必须是共有的，因为DbIniter需要
        /// </summary>
        public EfDbContext() : base(_nameOrConnectionString)
        {
            //改为false，之后延迟加载及时为true也不起作用，除非显示的Include
            //Include是显式加载，即使ProxyCreationEnabled = false和LazyLoadingEnabled = false，仍然会起作用
            //this.Configuration.ProxyCreationEnabled = false;//默认是true的
            //延迟加载导航属性
            this.Configuration.LazyLoadingEnabled = false;//默认是true的

            #region 设置命令超时时间  默认是30s
            //方式1
            IObjectContextAdapter objectContext = (this as IObjectContextAdapter);
            //超时时间单位秒
            objectContext.ObjectContext.CommandTimeout = 120;

            //方式2 超时时间单位秒 可以放到ActionSafe里
            this.Database.CommandTimeout = 120;

            #endregion

        }


        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// name=ConnString格式
        /// //设置为保护或者私有的，是希望通过通用的action或Func实例化
        /// 必须是共有的，因为DbIniter需要
        /// 如果两个数据库，最好两个EfDbContext,即使他们完全相同
        /// </summary>
        public EfDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            //改为false，之后延迟加载及时为true也不起作用，除非显示的Include
            //Include是显式加载，即使ProxyCreationEnabled = false和LazyLoadingEnabled = false，仍然会起作用
            //this.Configuration.ProxyCreationEnabled = false;//默认是true的
            //延迟加载导航属性
            this.Configuration.LazyLoadingEnabled = false;//默认是true的

            #region 设置命令超时时间  默认是30s
            ////方式1
            //IObjectContextAdapter objectContext = (this as IObjectContextAdapter);
            ////超时时间单位秒
            //objectContext.ObjectContext.CommandTimeout = 120;

            ////方式2 超时时间单位秒 可以放到ActionSafe里
            //this.Database.CommandTimeout = 120;

            #endregion

        }

        #endregion

        #region 测试数据库连接


        /// <summary>
        /// 测试数据库连接是否畅通。
        /// </summary>
        /// <returns>
        /// 如果返回值为true,表示数据库连接畅通。
        /// 如果返回值为false，表示数据库连接不畅通。通常修改连接字符串可解决该问题。
        /// </returns>
        public static bool IsConnectionAvailable(Action<Exception> exceptionHandler = null)
        {
            bool result = true;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(TrueConnectionString);
                //超时时间默认15s
                conn.Open();
                result = true;
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                result = false;
            }
            finally
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Close();
                    }
                    catch { }
                }
            }
            return result;
        }


        #endregion

        #region 设置上下文

        ///// <summary>
        ///// 静态构造函数，设置 database initializer to use for the given context type. The database initializer is called when a the given System.Data.Entity.DbContext type  is used to access a database for the first time.The default strategy for Code First contexts is an instance of System.Data.Entity.CreateDatabaseIfNotExists&lt;TContext>.
        ///// </summary>
        //static EfDbContext()
        //{
        //    Init();
        //}

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
                //initializer = new CreateDatabaseIfNotExists<EfDbContext>();
                initializer = new EfCreateDatabaseIfNotExists();
                // The database initializer is called when a the given System.Data.Entity.DbContext type is used to access a database for the first time.
                //因为第一次访问数据库时调用Seed来初始化，所以目前检查数据库是否存在并没有调用Seed
                Database.SetInitializer(initializer);

                //将初始化和创建数据库提前
                ActionSafe(dbContext =>
                {
                    //dbContext.Database.CreateIfNotExists();//这一句不能加，否则不会进行初始化
                    //false，保证如果之前运行了，初始化不再运行
                    //不能使用查询 int count = context.DbSet.Count(),此时正在创建数据库
                    dbContext.Database.Initialize(false);//单独这一句可以创建数据库和进行初始化
                }, ex =>
                {
                    //记录日志
                    NLogHelper.Error("初始化数据库失败："+ex);
                });
            }
            else
            {
                //初始化代码不要放在MigrateDatabaseToLatestVersion中
                //实际上MigrateDatabaseToLatestVersion在数据库不存在的情况下，会创建数据库（已测试）
                //不再需要CreateDatabaseIfNotExists
                initializer = new MigrateDatabaseToLatestVersion<EfDbContext, MigrationConfiguration>();

                ////相当于null，不进行初始化
                //initializer = new NullDatabaseInitializer<EfDbContext>();
                // The database initializer is called when a the given System.Data.Entity.DbContext type is used to access a database for the first time.
                //因为第一次访问数据库时调用Seed来初始化，所以目前检查数据库是否存在并没有调用Seed
                Database.SetInitializer(initializer);
            }
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
            //modelBuilder.HasDefaultSchema("dbo");
            #endregion

            #region 设置Map

            //建议使用显式的定义，而不是通过反射
            //设置所有的表定义映射
            modelBuilder.Configurations.Add(new ListItemMap());
            modelBuilder.Configurations.Add(new KvPairMap());
            modelBuilder.Configurations.Add(new DictItemMap());
            modelBuilder.Configurations.Add(new CacheItemMap());


            modelBuilder.Configurations.Add(new ListBytesItemMap());
            modelBuilder.Configurations.Add(new KvBytesPairMap());
            modelBuilder.Configurations.Add(new DictBytesItemMap());
            modelBuilder.Configurations.Add(new CacheBytesItemMap());

            modelBuilder.Configurations.Add(new LogItemMap());


            modelBuilder.Configurations.Add(new CommonEfEntityMap());

            modelBuilder.Configurations.Add(new BaseEntityMap());

            //modelBuilder.Configurations.Add(new PersonMap());
            //modelBuilder.Configurations.Add(new PhoneMap());
            //modelBuilder.Entity<Phone>().HasRequired(s => s.Person).WithMany(s => s.Phones).HasForeignKey(s => s.PersonId).WillCascadeOnDelete(true);

            //以下代码自动注册所有的Map，自动获取当前代码中的Map,并注册
            //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.BaseType != null && !type.IsGenericType && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)).ToList();
            //foreach (var type in typesToRegister)
            //{
            //    dynamic configurationInstance = Activator.CreateInstance(type);
            //    modelBuilder.Configurations.Add(configurationInstance);
            //}

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

        //该定义不是必须的
        //不需要加virtual
        //public DbSet<Person> Persons{get; set;}
        /// <summary>
        /// 键值对
        /// 键最长256， 值 max
        /// </summary>
        public DbSet<KvPair> KvPairs { get; set; }


        /// <summary>
        /// 带类型字典表 键类型最长256， 值 max
        /// </summary>
        public DbSet<DictItem> DictItems { get; set; }


        /// <summary>
        /// 缓存项
        /// </summary>
        public DbSet<CacheItem> CacheItems { get; set; }


        /// <summary>
        /// 键值对
        /// 键最长256， 值 max
        /// </summary>
        public DbSet<KvBytesPair> KvBytesPairs { get; set; }


        /// <summary>
        /// 带类型字典表 键类型最长256， 值 max
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
        /// 日志类型
        /// </summary>
        public DbSet<LogItem> LogItems { get; set; }

        /// <summary>
        /// 通用ef entity
        /// </summary>
        public DbSet<CommonEfEntity> CommonEfEntities { get; set; }


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

        #region 静态方法 原生调用

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
        /// 安全的使用EfDbContext执行通用任务
        /// 返回true，表示没有异常，false表示发生异常
        /// </summary>
        /// <param name="action">数据库操作</param>
        /// <param name="exceptionHanlder">异常处理</param>
        public static bool ActionSafe(Action<EfDbContext> action, Action<Exception> exceptionHanlder = null)
        {
            try
            {
                using (EfDbContext context = new EfDbContext())
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
        public static T FuncSafe<T>(Func<EfDbContext, T> func, Action<Exception> exceptionHanlder = null)
        {
            try
            {
                using (EfDbContext context = new EfDbContext())
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
        public static T FuncSafe<T>(Func<EfDbContext, T> func, Func<Exception,T> exceptionHanlder = null)
        {
            try
            {
                using (EfDbContext context = new EfDbContext())
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
         * 经测试，EF字符串不支持String.IsNullOrWhiteSpace 但支持IsNullOrEmpty
         * 注：string的contains类似于sql的like  IEnumerable<string>(集合中可以有null)的contains类似于 sql的 IN
         * 尽量使用DbFunctions来执行操作
         * DateTime.Now 会转化为SysDateTime()  但不要使用DateTime的函数操作
         * 
         * 查询的Union相当于sql的union，是去重的，不去重使用concat连接
         * DateTime可以使用.year .day .month
         */

        /*
         //查询多级深度子项，使用select包含多级子项
         //如Passenger有关联Baggage,Baggage关联Unpackrecord,Unpackrecord关联Good
        var passengerItems = context.Passengers.Where(r => r.CreatedTime < expireTime)
                .Include(r => r.Baggages.Select(b => b.Unpackrecords.Select(u => u.Goods)))
                .Include(r => r.Boardings)
                .Include(r => r.Checkinrecords);

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

            /*
             * 删除关联子项
             */
            /*
            if (parent.Children != null)
            {
                //加上ToList,否则有可能 generates "Collection was modified; enumeration operation may not execute error"
                foreach (var child in parent.Children.ToList())
                {
                    //注意是从context中删除
                    context.Children.Remove(child);
                }
            }
            context.Parents.Remove(parent);
            */
            
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
            //加载外键关联对象
            //dbContext.Entry(entity).Reference(c => c.Provinces).Load();
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


        #region 直接执行命令  推荐ExecuteSqlCommand

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

        /// <summary>
        /// A SQL query returning instances of any type, including primitive types,
        /// 该方法用于查询。直到迭代返回结果时，sql才执行。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] parameters) where T : class
        {
            using (EfDbContext dbContext = new EfDbContext())
            {
                return dbContext.Database.SqlQuery<T>(sql, parameters);
            }
        }

        /// <summary>
        /// ExecuteSqlCommnad method is useful in sending non-query commands to the database, such as the Insert, Update or Delete command. 
        /// </summary>
        /// <param name="sql">UPDATE dbo.Posts SET Rating = 5 WHERE Author = @p0  使用@po表示参数</param>
        /// <param name="parameters">SqlParameter parameter = new SqlParameter("@p0", DbType.DateTime);parameter.Value = DateTime.Now;</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns>影响的行数 , -1表示命令执行失败</returns>
        public static int ExecuteSqlCommand(string sql, Action<Exception> exceptionHandler, params SqlParameter[] parameters)
        {
            try
            {
                using (EfDbContext dbContext = new EfDbContext())
                {
                    if (parameters == null || parameters.Length == 0)
                    {
                        return dbContext.Database.ExecuteSqlCommand(sql);
                    }
                    else
                    {
                        return dbContext.Database.ExecuteSqlCommand(sql, parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return -1;
            }
        }

        #endregion


        #region 无事务的原生调用

        /// <summary>
        /// ExecuteNonQuery操作，对数据库进行 增、删、改 操作（1）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteRawNonQuery(string sql)
        {
            return ExecuteRawNonQuery(sql, CommandType.Text, null);
        }

        /// <summary>
        /// ExecuteNonQuery操作，对数据库进行 增、删、改 操作（2）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteRawNonQuery(string sql, CommandType commandType)
        {
            return ExecuteRawNonQuery(sql, commandType, null);
        }

        /// <summary>
        /// ExecuteNonQuery操作，对数据库进行 增、删、改 操作（3）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 参数使用@名称表示 前面加@表示参数 </param>
        /// <returns> </returns>
        public static int ExecuteRawNonQuery(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(TrueConnectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            if (parameter != null)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                    }
                    connection.Open();
                    count = command.ExecuteNonQuery();
                }
            }
            return count;
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataSet类型结果（1）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <returns>DataSet代表了select语句的结果。</returns>
        public static DataSet ExecuteRawDataSet(string sql)
        {
            return ExecuteRawDataSet(sql, CommandType.Text, null);
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataSet类型结果（2）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns>DataSet代表了select语句的结果。</returns>
        public static DataSet ExecuteRawDataSet(string sql, CommandType commandType)
        {
            return ExecuteRawDataSet(sql, commandType, null);
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataSet类型结果（3）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组  前面加@表示参数</param>
        /// <returns>DataSet代表了select语句的结果。</returns>
        public static DataSet ExecuteRawDataSet(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(TrueConnectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            if (parameter != null)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（1）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <returns> </returns>
        public static DataTable ExecuteRawDataTable(string sql)
        {
            return ExecuteRawDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（2）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns> </returns>
        public static DataTable ExecuteRawDataTable(string sql, CommandType commandType)
        {
            return ExecuteRawDataTable(sql, commandType, null);
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（3）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组  前面加@表示参数</param>
        /// <returns> </returns>
        public static DataTable ExecuteRawDataTable(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(TrueConnectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            if (parameter != null)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(data);
                    }
                }
            }
            return data;
        }

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
                //EF默认反射成HashSet，不用检查null和初始化
                Rights = new HashSet<OneToManyRight>();
            }

            /// <summary>
            /// 主键
            /// </summary>
            public Guid Id { get; set; }
            /// <summary>
            /// ICollection在EF中默认反射成HashSet
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
            //一对多关系，可以级联删除，删除右边不会影响左边，删除左边会删除右边 默认是不级联的
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
            /// ICollection在EF中默认反射成HashSet
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
            /// ICollection在EF中默认反射成HashSet
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
                m.ToTable("LeftRight");//创建中间表LeftRight
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
        /// 不会进行数据库初始化
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
        /// 强制检查初始化 单独这一句可以创建数据库和进行初始化
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


        #region List 列表

        //比较简单不加了


        #endregion


        #region 键值对相关   经过测试

        /// <summary>
        /// 插入键值对，如果对应的键已经存在则更新，否则新增记录
        /// 键为null或空白，则什么也不做 值为null或者空白仍然添加或者更新
        /// 键在数据库中是按小写存的
        /// 返回新增或者修改的数据项，如果失败，返回null
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白</param>
        /// <param name="value">值,数据库中按原样保存</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static KvPair AddOrUpdateKvPair(string key, string value, Action<Exception> exceptionHandler = null)
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
                using (EfDbContext context = new EfDbContext())
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
                        //如果键值对没有变化，不更新
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
        /// <param name="key">键 ,键忽略大小写，忽略前后空白</param>
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
                using (EfDbContext context = new EfDbContext())
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
        /// <param name="key">键 ,键忽略大小写，忽略前后空白</param>
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
                using (EfDbContext context = new EfDbContext())
                {
                    //在某些多线程同时写的极端情况，有了唯一键，不会创建多条记录
                    KvPair result = context.KvPairs.FirstOrDefault(r => r.Key == normalizedKey);
                    if (result != null)
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
        /// 返回新增或者修改的数据项，如果失败，返回null
        /// </summary>
        /// <param name="key">键 ,键忽略大小写，忽略前后空白</param>
        /// <param name="value">值,数据库中按原样保存</param>
        /// <param name="exceptionHandler">异常处理</param>
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
                using (EfDbContext context = new EfDbContext())
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
        /// <param name="key">键 ,键忽略大小写，忽略前后空白</param>
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
                using (EfDbContext context = new EfDbContext())
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
        /// <param name="key">键 ,键忽略大小写，忽略前后空白</param>
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
                using (EfDbContext context = new EfDbContext())
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
                using (EfDbContext context = new EfDbContext())
                {
                    //即使在某些多线程同时写的极端情况，有可能创建多条记录
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
        public static DictItem GetDictItem(string key, string type = "", Action<Exception> exceptionHandler = null)
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
                using (EfDbContext context = new EfDbContext())
                {
                    //即使在某些多线程同时写的极端情况，有组合主键保证，不可能会创建多条记录
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
                using (EfDbContext context = new EfDbContext())
                {
                    //即使在某些多线程同时写的极端情况，有组合主键保证，不可能会创建多条记录
                    var result = context.DictItems.Where(r=>r.DictType == normalizedType).ToList();
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
                using (EfDbContext context = new EfDbContext())
                {
                    //即使在某些多线程同时写的极端情况，也不可能会创建多条记录(主键)
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
                using (EfDbContext context = new EfDbContext())
                {
                    //即使在某些多线程同时写的极端情况，有可能创建多条记录
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
                using (EfDbContext context = new EfDbContext())
                {
                    //即使在某些多线程同时写的极端情况，有组合主键保证，不可能会创建多条记录
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
                using (EfDbContext context = new EfDbContext())
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
                using (EfDbContext context = new EfDbContext())
                {
                    //即使在某些多线程同时写的极端情况，也不可能会创建多条记录(主键)
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
        public static CacheItem AddOrUpdateCacheItem(string key, string value, DateTime expireTime, string type = "",
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
                using (EfDbContext context = new EfDbContext())
                {
                    //即使在某些多线程同时写的极端情况，也不可能创建多条记录
                    CacheItem oldCacheItem = context.CacheItems.FirstOrDefault(r => r.CacheKey == normalizedKey && r.CacheType == normalizedType);

                    if (oldCacheItem == null)
                    {
                        oldCacheItem = new CacheItem();
                        oldCacheItem.CacheKey = normalizedKey;
                        oldCacheItem.CacheType = normalizedType;
                        oldCacheItem.CreateTime = oldCacheItem.UpdateTime = DateTime.Now;
                        oldCacheItem.ExpireTime = expireTime;
                        oldCacheItem.CacheValue = value;
                        context.CacheItems.Add(oldCacheItem);
                        context.SaveChanges();
                    }
                    else
                    {
                        bool valueChanged = oldCacheItem.CacheValue != value;
                        bool expireTimeChanged = oldCacheItem.ExpireTime != expireTime;

                        if (valueChanged)
                        {
                            oldCacheItem.CacheValue = value;
                        }

                        if (expireTimeChanged)
                        {
                            oldCacheItem.ExpireTime = expireTime;
                        }

                        if (valueChanged || expireTimeChanged)
                        {
                            oldCacheItem.UpdateTime = DateTime.Now;
                            context.SaveChanges();
                        }


                    }
                    return oldCacheItem;
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
                using (EfDbContext context = new EfDbContext())
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
                using (EfDbContext context = new EfDbContext())
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
                using (EfDbContext context = new EfDbContext())
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
                //键为null或空白，则什么也不做
                return null;
            }

            try
            {
                //键 ,键忽略大小写，忽略前后空白(注sqlite本身是区分大小写的，本功能有C#代码实现)
                //在数据库中全部保存了小写
                string normalizedKey = key.Trim().ToLower();
                string normalizedType = type == null ? string.Empty : type.Trim().ToLower();
                using (EfDbContext context = new EfDbContext())
                {
                    //即使在某些多线程同时写的极端情况，也不可能创建多条记录
                    CacheBytesItem oldCacheBytesItem = context.CacheBytesItems.FirstOrDefault(r => r.CacheKey == normalizedKey && r.CacheType == normalizedType);

                    if (oldCacheBytesItem == null)
                    {
                        oldCacheBytesItem = new CacheBytesItem();
                        oldCacheBytesItem.CacheKey = normalizedKey;
                        oldCacheBytesItem.CacheType = normalizedType;
                        oldCacheBytesItem.CreateTime = oldCacheBytesItem.UpdateTime = DateTime.Now;
                        oldCacheBytesItem.ExpireTime = expireTime;
                        oldCacheBytesItem.CacheValue = value;
                        context.CacheBytesItems.Add(oldCacheBytesItem);
                        context.SaveChanges();
                    }
                    else
                    {
                        bool valueChanged = !StringUtils.EqualsEx(oldCacheBytesItem.CacheValue, value);
                        bool expireTimeChanged = oldCacheBytesItem.ExpireTime != expireTime;

                        if (valueChanged)
                        {
                            oldCacheBytesItem.CacheValue = value;
                        }

                        if (expireTimeChanged)
                        {
                            oldCacheBytesItem.ExpireTime = expireTime;
                        }

                        if (valueChanged || expireTimeChanged)
                        {
                            oldCacheBytesItem.UpdateTime = DateTime.Now;
                            context.SaveChanges();
                        }


                    }
                    return oldCacheBytesItem;
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
                using (EfDbContext context = new EfDbContext())
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
                using (EfDbContext context = new EfDbContext())
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
                using (EfDbContext context = new EfDbContext())
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
        public static bool AddLog(LogType logType, string logContent, string logSource = "", Action<Exception> exceptionHandler = null)
        {
            if (String.IsNullOrWhiteSpace(logContent))
            {
                return true;
            }

            try
            {


                using (EfDbContext context = new EfDbContext())
                {
                    LogItem logItem = new LogItem();
                    logItem.Id = GuidUtils.GetGuid32();
                    logItem.LogType = logType.ToString();
                    logItem.LogSource = logSource ?? string.Empty;
                    logItem.LogContent = logContent;
                    logItem.CreateTime = DateTime.Now;

                    context.LogItems.Add(logItem);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return false;
            }
        }

        #endregion






        #region Z.EntityFramework.Plus.EF6  拓展EF的功能，增加批量操作 没什么用不需要，使用原生sql

        //https://github.com/zzzprojects/EntityFramework-Plus
        //http://entityframework-plus.net/


        #region Batch Delete 批量删除，不必先加载对象

        /// <summary>
        /// 删除where指定查询条件的数据，不用查询
        /// Batch Delete Without Loading Them
        /// </summary>
        public int BatchDelete<T>(DbSet<T> dbSet, Expression<Func<T, bool>> where) where T:class
        {
            return dbSet.Where(where).Delete();
        }


        /// <summary>
        ///  删除where指定查询条件的数据，不用查询
        ///  Batch Delete Without Loading Them
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="where"></param>
        /// <param name="batchSize">默认值是4000
        /// BatchSize property sets the amount of rows to delete in a single batch.</param>
        /// <returns></returns>
        public int BatchDelete<T>(DbSet<T> dbSet, Expression<Func<T, bool>> where,int batchSize=100) where T : class
        {
            return dbSet.Where(where).Delete(x=>x.BatchSize = batchSize);
        }

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
        //public override void InitializeDatabase(EfDbContext context)
        //{
        //    base.InitializeDatabase(context);
        //}

        /// <summary>
        /// Seed方法在每次新建数据库后调用，如果已经存在则不调用
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(EfDbContext context)
        {
            base.Seed(context);
            
            //在数据库中加入初始化数据
            //为了安全起见，尽量使用AddOrUpdate 实际上没有必要，只有在数据迁移时，使用AddOrUpdate
            //尽量使用AddOrUpdate
            //context.Persons.AddOrUpdate();

            //框架会调用savechanges，但为了确保，最后最好加savechanges
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
    /// 
    /// 如果只有一个MigrationConfiguration类，则CreateDatabaseIfNotExists DropCreateDatabaseAlways DropCreateDatabaseIfModelChanges也自动使用该配置，否则（有多个），不使用
    /// </summary>
    public class MigrationConfiguration : DbMigrationsConfiguration<EfDbContext>
    {
        public MigrationConfiguration()
        {
            if (!Database.Exists(EfDbContext.NameOrConnectionString))
            {
                //第一次创建数据库时数据库不存在
                //如果只有一个MigrationConfiguration类，则CreateDatabaseIfNotExists DropCreateDatabaseAlways DropCreateDatabaseIfModelChanges也自动使用该配置，否则（有多个），不使用
                //如果数据库没有创建，则允许迁移,第一次创建数据库时允许自动迁移

                //必须支持允许自动迁移，这样当数据库结构改变后就可以自动迁移了
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true; //允许数据损失
                //Gets or sets the string used to distinguish migrations belonging to this configuration from migrations belonging to other configurations using the same database.
                ContextKey = "CmluMigrationConfiguration";
            }
            else
            {
                //数据库已经存在，进行相关配置
                //实际上可以自动迁移，禁用自动迁移是在开发模式下

                //必须支持允许自动迁移，这样当数据库结构改变后就可以自动迁移了
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true; //允许数据损失
                //Gets or sets the string used to distinguish migrations belonging to this configuration from migrations belonging to other configurations using the same database.
                ContextKey = "CmluMigrationConfiguration";
            }
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
            //  会被调用多次，使用AddOrUpdate


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


    ///// <summary>
    ///// 配置数据迁移
    ///// </summary>
    //public class MigrationConfiguration2 : DbMigrationsConfiguration<EfDbContext>
    //{
    //    public MigrationConfiguration2()
    //    {
    //        //必须支持允许自动迁移，这样当数据库结构改变后就可以自动迁移了
    //        AutomaticMigrationsEnabled = true;
    //        AutomaticMigrationDataLossAllowed = true;//允许数据损失
    //        //Gets or sets the string used to distinguish migrations belonging to this configuration from migrations belonging to other configurations using the same database.
    //        ContextKey = "CmluMigrationConfiguration";
    //    }

    //    /// <summary>
    //    /// If the System.Data.Entity.MigrateDatabaseToLatestVersion<TContext,TMigrationsConfiguration>database initializer is being used, then this method will be called each time that the initializer runs.
    //    /// 意味着每次程序启动，都会执行Seed,而不是仅仅迁移后执行
    //    /// Runs after upgrading to the latest migration to allow seed data to be updated.
    //    /// 
    //    /// if AutomaticMigrationsEnabled=true,the DbMigrationsConfiguration Seed method will run after each migration is applied or every time that the initializer runs.initializer is called when a the given System.Data.Entity.DbContext type is used to access a database for the first time.意味着每次迁移后或者第一次DbContext访问数据库时会调用seed，但不是每次DbContext创建时调用。 
    //    /// </summary>
    //    /// <param name="context"></param>
    //    protected override void Seed(EfDbContext context)
    //    {
    //        //  每次程序启动，都会执行Seed,而不是仅仅迁移后执行

    //        //  You can use the DbSet<T>.AddOrUpdate() helper extension method to avoid creating duplicate seed data. E.g.
    //        //
    //        //    context.People.AddOrUpdate(
    //        //      p => p.FullName,
    //        //      new Person { FullName = "Andrew Peters" },
    //        //      new Person { FullName = "Brice Lambson" },
    //        //      new Person { FullName = "Rowan Miller" }
    //        //    );
    //        //
    //    }
    //}


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
    /// 不要使用基类，这里是作为一个例子，供以后写代码方便查看使用
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 主键
        /// DatabaseGeneratedOption.Identity is used to create an auto-increment column in the table by a unique value.int类型需要，Guid类型不需要
        /// 除Sql Server外，MySql Sqlite尽量使用long或者int类型作为主键
        /// 实际上应该尽量使用Guid作为主键，因为guid可以更好的保证插入的并行性，并且合并数据库时有决定性优势
        /// 尽量不要使用guid，而使用varchar(32)
        /// </summary>
        //public Guid Id { get; set; }

        ///建议使用36为变长字符串
        public String Id { get; set; }

        /// <summary>
        /// ?表示可选的
        /// </summary>
        public DateTime? ModifiedTime { get; set; }

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

        //索引分为聚集索引和非聚集索引，聚集索引是数据存储的物理顺序
        //聚集索引一个表只能有一个,而非聚集索引一个表可以存在多个
        [Index("IX_Name_DepartmentMaster", IsClustered = false)]
        public string Name { get; set; }

        /// <summary>
        /// 乐观锁来支持并发控制
        /// 如果同时多个用户修改，只有第一个用户修改成功，第二个用户需要处理DbUpdateConcurrencyException异常
        /// 目前只在Sql Server中支持，
        /// RowVerion和ConcurrencyCheck的区别是每次Add和Update,rowVersion会自动incremented
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// 如果不配置，默认Byte[]->[varbinary](max) NULL,,Boolean->bit
        /// </summary>
        public byte[] BytesExample { get; set; }

        /// <summary>
        /// Create时为0,每次update加1
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// 预留字段
        /// </summary>
        public string ReservedField { get; set; }
    }

    /// <summary>
    /// 做测试的例子
    /// </summary>
    public class BaseEntityMap : EntityTypeConfiguration<BaseEntity>
    {
        public BaseEntityMap()
        {
            ToTable(nameof(BaseEntity)).HasKey(t => t.Id);
            //guid带-36位，不带32位
            Property(t => t.Id).IsRequired().HasMaxLength(36).IsUnicode().IsVariableLength().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //字符串常用配置
            Property(t => t.Name).IsOptional().HasMaxLength(128).IsUnicode().IsVariableLength();

            //预留字段，每张表都应该预留字段
            Property(t => t.ReservedField).IsOptional().HasMaxLength(null).IsUnicode().IsVariableLength();
            
            //如果不加下面的配置，将是[BytesExample] [varbinary](max) NULL,  添加之后是[BytesExample] [varbinary](2048) NULL,
            Property(t => t.BytesExample).IsOptional().HasMaxLength(2048);



            //           USE[SqlDb]
            //           GO
            //
            ///****** Object:  Table [dbo].[BaseEntity]    Script Date: 2017/12/13 11:12:19 ******/
            //                SET ANSI_NULLS ON
            //            GO

            //                SET QUOTED_IDENTIFIER ON
            //            GO

            //                SET ANSI_PADDING ON
            //            GO

            //                CREATE TABLE[dbo].[BaseEntity](

            //                [Id][nvarchar](36) NOT NULL,

            //                [ModifiedTime] [datetime] NULL,
            //                [CreateTime]
            //                [datetime]
            //            NOT NULL,

            //                [ModifyTime] [datetime]
            //            NOT NULL,

            //                [Name] [nvarchar] (128) NULL,
            //                [RowVersion]
            //                [timestamp]
            //            NOT NULL,

            //                [BytesExample] [varbinary] (2048) NULL,
            //            CONSTRAINT[PK_dbo.BaseEntity] PRIMARY KEY CLUSTERED
            //                (
            //                [Id] ASC
            //                )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
            //                ) ON[PRIMARY]

            //            GO

            //                SET ANSI_PADDING OFF
            //            GO

        }
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
            //主键数据库自动生成， 即自增主键
            //DatabaseGeneratedOption的三个属性：None:不是服务器生成，默认值；Identity：插入时，由数据库生成值，更新时保持不变（int或long型自增主键），自己设置无效；Computed:在插入或更新行时，数据库生成值，自己设置无效
            // Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            // ToTable("TableName").HasKey(p=>p.Id);//设置表名和主键
            //Property(x => x.Id).HasMaxLength(36)
            //    .IsUnicode()
            //    .IsVariableLength();
            #endregion

            #region 设置属性列字段

            //设置属性列字段
            //字符串
            //Property(p => p.FirstName)
            //    .HasColumnName("FirstName")
            //    .IsOptional()//.IsRequired()
            //    .HasMaxLength(30)
            //    .IsUnicode()
            //    .IsVariableLength();
            //Property(p => p.MiddleName).IsOptional().IsFixedLength().IsUnicode(false).HasMaxLength(1);

            //时间  对应默认数据库类型datetime,SqlServer的datetime有效范围是1753年1月1日到9999年12月31日
            //Property(t => t.CreateTime).IsOptional().HasColumnName("CreateTime");
            //HasColumnType指定类型 datetime2的范围0001-01-01 到 9999-12-31
            //Property(t => t.ModifiedTime).IsOptional().HasColumnName("ModifiedTime").HasColumnType("datetime2");

            //设置Timestamp属性和调用IsRowVersion两选一，目前只在Sql Server中支持，其它数据库的并发自行百度
            //Property(t => t.RowVersion).IsRowVersion();

            //用于非sqlserver数据库的并行检查
            //Property(p => p.ModifyTime).IsConcurrencyToken();

            #endregion


            #region 设置索引

            //索引分为聚集索引和非聚集索引，聚集索引是数据存储的物理顺序
            //聚集索引一个表只能有一个,而非聚集索引一个表可以存在多个
            //聚集索引存储记录是物理上连续存在，而非聚集索引是逻辑上的连续，物理存储并不连续
            //主键自动为聚集索引
            //设置索引和设置属性，应该连在一起
            //without the given name and has no column order, clustering, or
            //     uniqueness specified. //索引默认是不唯一非聚集的， 使用默认的命名为IX_列名
            //Property(t => t.Name).HasColumnName("Name").IsOptional().HasMaxLength(512)
            //    .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IndexName")));
            //Property(t => t.CreateTime).IsOptional().HasColumnName("CreateTime").HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            #endregion

            #region 设置关系 One-To-One  One-To-Many  Many-To-Many

            //建议在OnModelCreating中设置，不要在Map中设置
            // HasRequired(s => s.Flight).WithMany(s => s.FlightVias).HasForeignKey(t => t.FlightId).WillCascadeOnDelete(true);
            #endregion
        }
    }

    #endregion

}





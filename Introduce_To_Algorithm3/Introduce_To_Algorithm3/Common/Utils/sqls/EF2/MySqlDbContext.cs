using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Entity;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2
{
    /// <summary>
    /// 为MySql设置DbConfiguration.This step is optional but highly recommended, since it adds all the dependency resolvers for MySql classes. 
    /// This can by done by Adding the DbConfigurationTypeAttribute on the context class
    /// 参见文档
    /// http://dev.mysql.com/doc/connector-net/en/connector-net-introduction.html
    /// 除Sql Server外，MySql Sqlite尽量使用long或者int类型作为主键
    /// 
    //    /// Set the new DbConfiguration class for MySql. This step is optional but highly recommended, since it adds all the dependency resolvers for MySql classes. This can be done in three ways建议使用第三种:
    //	Adding the DbConfigurationTypeAttribute on the context class:
    //	1 [DbConfigurationType(typeof(MySqlEFConfiguration))]
    //	2 Calling DbConfiguration.SetConfiguration(new MySqlEFConfiguration()) at the application startup.
    //	3 Set the DbConfiguration type in the configuration file: 建议使用第三种
    //<entityFramework codeConfigurationType="MySql.Data.Entity.MySqlEFConfiguration, MySql.Data.Entity.EF6">
    /// 
    /// 每个表都应该有三个字段:
    ///         string Id             主键
    ///         DateTime UpdateTime   数据最近一次更新时间  创建时等于创建时间
    ///         DateTime CreateTime   数据创建时间 
    /// 
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MySqlDbContext:DbContext
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
        /// //设置为保护或者私有的，是希望通过通用的action或Func实例化
        /// 必须是共有的，因为DbIniter需要
        /// </summary>
        public MySqlDbContext() : base(_nameOrConnectionString)
        {

        }
        #endregion

        /// <summary>
        /// 使用MySqlDbContext执行通用任务
        /// </summary>
        /// <param name="action"></param>
        public static void Action(Action<MySqlDbContext> action)
        {
            using (MySqlDbContext context = new MySqlDbContext())
            {
                action(context);
            }
        }

        /// <summary>
        /// 使用MySqlDbContext执行通用任务,并返回一个值
        /// </summary>
        /// <param name="func"></param>
        public static T Func<T>(Func<MySqlDbContext, T> func)
        {
            using (MySqlDbContext context = new MySqlDbContext())
            {
                return func(context);
            }
        }


        /// <summary>
        /// 安全的使用MySqlDbContext执行通用任务
        /// </summary>
        /// <param name="action">数据库操作</param>
        /// <param name="exceptionHanlder">异常处理</param>
        public static void ActionSafe(Action<MySqlDbContext> action, Action<Exception> exceptionHanlder = null)
        {
            try
            {
                using (MySqlDbContext context = new MySqlDbContext())
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
        /// 安全的使用MySqlDbContext执行通用任务,并返回一个值
        /// </summary>
        /// <param name="func"></param>
        /// <param name="exceptionHanlder">异常处理</param>
        public static T FuncSafe<T>(Func<MySqlDbContext, T> func, Action<Exception> exceptionHanlder = null)
        {
            try
            {
                using (MySqlDbContext context = new MySqlDbContext())
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
        /// 配置表映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //ToDo:
            //modelBuilder.Configurations.Add(new UserMessageMap());
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            IDatabaseInitializer<MySqlDbContext> initializer;

            if (!Database.Exists(_nameOrConnectionString))
            {
                //初始化代码放在CreateDatabaseIfNotExists中
                //initializer = new CreateDatabaseIfNotExists<MySqlDbContext>();
                initializer = new CreateDatabaseIfNotExists<MySqlDbContext>();
            }
            else
            {
                //初始化代码不要放在MigrateDatabaseToLatestVersion中
                //initializer = new MigrateDatabaseToLatestVersion<MySqlDbContext, MigrationConfiguration>();

                //MySql的数据库迁移有bug，不能用
                ////相当于null，不进行初始化
                initializer = new NullDatabaseInitializer<MySqlDbContext>();
            }

            // The database initializer is called when a the given System.Data.Entity.DbContext type is used to access a database for the first time.
            //因为第一次访问数据库时调用Seed来初始化，所以目前检查数据库是否存在并没有调用Seed
            Database.SetInitializer(initializer);
        }
    }

}

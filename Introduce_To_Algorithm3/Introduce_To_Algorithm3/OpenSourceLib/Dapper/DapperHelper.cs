using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Introduce_To_Algorithm3.OpenSourceLib.Dapper
{
    /// <summary>
    /// orm, 轻量级 半自动的orm
    /// Dapper假设所有的连接已经打开
    /// Dapper很好
    /// Dapper是拓展IDbConnection的Nuget库
    /// </summary>
    public static class DapperHelper
    {
        #region 查询

        /// <summary>
        /// 查询
        /// 
        /// 
            ////public class Dog
            ////{
            ////    public int? Age { get; set; }
            ////    public Guid Id { get; set; }
            ////    public string Name { get; set; }
            ////    public float? Weight { get; set; }

            ////    public int IgnoredProperty { get { return 1; } }
            ////}            

            ////var guid = Guid.NewGuid();
            ////var dog = connection.Query<Dog>("select Age = @Age, Id = @Id", new { Age = (int?)null, Id = guid });

            ////dog.Count()
            ////    .IsEqualTo(1);

            ////dog.First().Age
            ////    .IsNull();

            ////dog.First().Id
            ////    .IsEqualTo(guid);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(string sql, object param = null,
            SqlTransaction transaction = null, bool buffered = true,int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        {
            return Func<IEnumerable<T>>((conn) =>
            {
                return conn.Query<T>(sql, param, transaction, true,commandTimeout,commandType);
            });
        } 

        #endregion


        #region 辅助方法

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static readonly String _connectionStr = "Data Source=192.168.163.204;Initial Catalog=Fids;User ID=fids;Password=fids";

        /// <summary>
        /// 通用action
        /// </summary>
        /// <param name="action">传入的connection已经打开</param>
        public static void Action(Action<IDbConnection> action)
        {
            using (IDbConnection connection = new SqlConnection(_connectionStr))
            {
                connection.Open();
                if (action != null)
                {
                    action(connection);
                }
            }
        }

        /// <summary>
        /// 执行通用Func
        /// </summary>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="func">传入的connection已经打开</param>
        /// <returns></returns>
        public static T Func<T>(Func<IDbConnection, T> func)
        {
            using (IDbConnection connection = new SqlConnection(_connectionStr))
            {
                connection.Open();
                if (func != null)
                {
                    return func(connection);
                }
                else
                {
                    return default(T);
                }
            }
        }


        #endregion


        #region 测试代码

        /// <summary>
        /// 测试代码
        /// </summary>
        /// <param name="args"></param>
        public static void TestMain(String[] args)
        {
            
        }

        #endregion

    }
}

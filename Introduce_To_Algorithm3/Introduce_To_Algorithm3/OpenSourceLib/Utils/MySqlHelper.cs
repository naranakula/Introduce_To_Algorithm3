﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// MySql帮助类
    /// Nuget  MySql.Data
    /// 如果需要EntityFramework支持再添加nuget:MySql.Data.Entity
    /// </summary>
    public class MySqlHelper
    {
        #region 单件实例
        /// <summary>
        /// 私有的构造函数
        /// </summary>
        private MySqlHelper()
        {
            
        }

        /// <summary>
        /// MySqlHelper的单件实例
        /// </summary>
        private static volatile MySqlHelper _instance;

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();


        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string _connectionString = "server=localhost;port=3306;database=DbTest;uid=root;password=558276344;";

        /// <summary>
        /// 获取使用默认连接字符串的SqlHelper唯一实例
        /// </summary>
        /// <returns></returns>
        public  static MySqlHelper GetInstance()
        {
            //如果_instance为null，则构建一个实例
            if (_instance != null)
            {
                return _instance;
            }

            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new MySqlHelper();
                }
            }

            //如果_instance为null，则构建一个实例
            return _instance;
        }

        /// <summary>
        /// 设置连接字符串，并返回该连接字符串的唯一MySqlHelper实例
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static MySqlHelper GetInstance(string connString)
        {
            if(_instance == null)
            {
                //创建实例
                _instance = new MySqlHelper();
            }

            if(!string.IsNullOrEmpty(connString))
            {
                //设置连接字符串
                _instance._connectionString = connString;
            }

            return _instance;
        }

        #endregion

        #region 处理连接，不需要外界关心连接的打开和关闭

        /// <summary>
        /// 处理连接，不需要外界关心连接的打开和关闭
        /// </summary>
        /// <param name="action">处理连接的回调</param>
        /// <param name="exceptionHandler">异常处理</param>
        public void HandleConnection(Action<MySqlConnection> action,Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    action?.Invoke(connection);
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

        #endregion

        #region 测试连接是否畅通

        /// <summary>
        /// 测试数据库连接是否畅通
        /// </summary>
        /// <returns>返回true，表示数据库连接畅通；表示false，表示数据库连接不畅通</returns>
        public bool IsConnectionActive(Action<Exception> exceptionHandler = null)
        {
            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(_connectionString);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return false;
            }
            finally
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Close();
                    }
                    catch
                    {
                    }
                }
            }
        }

        #endregion

        #region 执行sql语句

        /// <summary>
        /// ExecuteNonQuery操作，对数据库进行 增、删、改 操作（1）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, CommandType.Text, null);
        }

        /// <summary>
        /// ExecuteNonQuery操作，对数据库进行 增、删、改 操作（2）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string sql, CommandType commandType)
        {
            return ExecuteNonQuery(sql, commandType, null);
        }


        /// <summary>
        /// ExecuteNonQuery操作，对数据库进行 增、删、改 操作（3）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns> </returns>
        public int ExecuteNonQuery(string sql, CommandType commandType, MySqlParameter[] parameters)
        {
            int count = 0;
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (MySqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    conn.Open();
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
        public DataSet ExecuteDataSet(string sql)
        {
            return ExecuteDataSet(sql, CommandType.Text, null);
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataSet类型结果（2）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns>DataSet代表了select语句的结果。</returns>
        public DataSet ExecuteDataSet(string sql, CommandType commandType)
        {
            return ExecuteDataSet(sql, commandType, null);
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataSet类型结果（3）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns>DataSet代表了select语句的结果。</returns>
        public DataSet ExecuteDataSet(string sql, CommandType commandType, MySqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (MySqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(ds);
                }
            }
            return ds;
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（1）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <returns> </returns>
        public DataTable ExecuteDataTable(string sql)
        {
            return ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（2）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns> </returns>
        public DataTable ExecuteDataTable(string sql, CommandType commandType)
        {
            return ExecuteDataTable(sql, commandType, null);
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果（3）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns> </returns>
        public DataTable ExecuteDataTable(string sql, CommandType commandType, MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        //clear是为了调试写的
                        command.Parameters.Clear();
                        foreach (MySqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        /// <summary>
        /// ExecuteReader执行一查询，返回一<c>SqlDataReader</c>对象实例（1）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <returns></returns>
        /// <remarks>
        /// <example>
        /// 返回值SqlDataReader使用完毕后，应关闭。
        /// <code>
        /// MySqlDataReader sdr = object.ExecuteReader(sql);
        /// while(sdr.Read())
        /// {
        /// }
        /// sdr.Close();
        /// </code>
        /// </example>
        /// </remarks>
        public MySqlDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, CommandType.Text, null);
        }



        /// <summary>
        /// ExecuteReader执行一查询，返回一SqlDataReader对象实例（2）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns> </returns>
        public MySqlDataReader ExecuteReader(string sql, CommandType commandType)
        {
            return ExecuteReader(sql, commandType, null);
        }

        /// <summary>
        /// ExecuteReader执行一查询，返回MySqldataReader对象实例(3)
        /// </summary>
        /// <param name="sql">要执行的sql语句或者存储过程</param>
        /// <param name="commandType">要执行的查询类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        /// <example>
        /// 返回值SqlDataReader使用完毕后，应关闭。
        /// <code>
        /// MySqlDataReader sdr = object.ExecuteReader(sql);
        /// while(sdr.Read())
        /// {
        /// }
        /// sdr.Close();
        /// </code>
        /// </example>
        public MySqlDataReader ExecuteReader(string sql, CommandType commandType, MySqlParameter[] parameters)
        {
            MySqlConnection conn = new MySqlConnection(_connectionString);
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.CommandType = commandType;
            if (parameters != null)
            {
                foreach (MySqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            conn.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }


        /// <summary>
        /// ExecuteScalar执行一查询，返回查询结果的第一行第一列（1）,如果为空，返回null.
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <returns> </returns>
        public Object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, CommandType.Text, null);
        }

        /// <summary>
        /// ExecuteScalar执行一查询，返回查询结果的第一行第一列（2）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns> </returns>
        public Object ExecuteScalar(string sql, CommandType commandType)
        {
            return ExecuteScalar(sql, commandType, null);
        }


        /// <summary>
        /// ExecuteScalar执行一查询，返回查询结果的第一行第一列（3）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns> </returns>
        public Object ExecuteScalar(string sql, CommandType commandType, MySqlParameter[] parameters)
        {
            object result = null;
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        foreach (MySqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    conn.Open();
                    result = command.ExecuteScalar();
                }
            }
            return result;
        }


        #endregion
    }
}

using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

//using System.Data.OracleClient;
//System.Data.OracleClient是微软实现的，已经废弃

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// Oracle的帮助类，需要System.Data.OracleClient.dll
    /// Nuget:Oracle.ManagedDataAccess
    /// 如果需要EntityFramework支持，再添加Oracle.ManagedDataAccess.EntityFramework
    /// </summary>
    public class OracleHelper
    {
        #region 单件实例
        /// <summary>
        /// 私有的构造函数
        /// </summary>
        private OracleHelper()
        {
            
        }

        /// <summary>
        /// OracleHelper的单件实例
        /// </summary>
        private static volatile OracleHelper _instance;

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 数据库连接字符串
        /// 仅需要nuget的Oracle.ManagedDataAccess
        /// </summary>
        //这种方式需要配置tnsnames.ora文件
        //private string _connectionString = "Data Source=数据库名;User Id=用户名;Password=密码";
        //这种方式不需要tnsnames.ora和app.config里有其它配置
        private string _connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyOracleSID)));User Id = myUsername; Password=myPassword;";

        /// <summary>
        /// 获取使用默认连接字符串的SqlHelper唯一实例
        /// </summary>
        /// <returns></returns>
        public  static OracleHelper GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new OracleHelper();
                }
            }

            //如果_instance为null，则构建一个实例
            return _instance;
        }

        /// <summary>
        /// 设置连接字符串，并返回该连接字符串的唯一OracleHelper实例
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static OracleHelper GetInstance(string connString)
        {
            if(_instance == null)
            {
                //创建实例
                _instance = new OracleHelper();
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
        /// <param name="exceptionHandler"></param>
        public void HandleConnection(Action<OracleConnection> action,Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
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
            OracleConnection conn = null;

            try
            {
                conn = new OracleConnection(_connectionString);
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
                if(conn != null)
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
        public int ExecuteNonQuery(string sql, CommandType commandType, OracleParameter[] parameters)
        {
            int count = 0;
            using(OracleConnection conn = new OracleConnection(_connectionString))
            {
                using(OracleCommand command = new OracleCommand(sql,conn))
                {
                    command.CommandType = commandType;
                    if(parameters != null)
                    {
                        foreach (OracleParameter parameter in parameters)
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
        public DataSet ExecuteDataSet(string sql,CommandType commandType,OracleParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using(OracleConnection conn = new OracleConnection(_connectionString))
            {
                using(OracleCommand command = new OracleCommand(sql,conn))
                {
                    command.CommandType = commandType;
                    if(parameters != null)
                    {
                        foreach (OracleParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    OracleDataAdapter adapter = new OracleDataAdapter(command);
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
        public DataTable ExecuteDataTable(string sql,CommandType commandType,OracleParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using(OracleConnection conn = new OracleConnection(_connectionString))
            {
                using(OracleCommand command = new OracleCommand(sql,conn))
                {
                    command.CommandType = commandType;
                    if(parameters != null)
                    {
                        //clear是为了调试写的
                        command.Parameters.Clear();
                        foreach (OracleParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    OracleDataAdapter adapter = new OracleDataAdapter(command);
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
        public OracleDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, CommandType.Text, null);
        }



        /// <summary>
        /// ExecuteReader执行一查询，返回一SqlDataReader对象实例（2）
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns> </returns>
        public OracleDataReader ExecuteReader(string sql, CommandType commandType)
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
        public OracleDataReader ExecuteReader(string sql, CommandType commandType, OracleParameter[] parameters)
        {
            OracleConnection conn = new OracleConnection(_connectionString);
            OracleCommand command = new OracleCommand(sql, conn);
            command.CommandType = commandType;
            if (parameters != null)
            {
                foreach (OracleParameter parameter in parameters)
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
        public Object ExecuteScalar(string sql, CommandType commandType, OracleParameter[] parameters)
        {
            object result = null;
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                using (OracleCommand command = new OracleCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        foreach (OracleParameter parameter in parameters)
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

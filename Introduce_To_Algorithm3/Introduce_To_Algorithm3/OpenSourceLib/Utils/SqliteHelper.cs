using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Data.SQLite;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// If the database file doesn't exist, the default behaviour is to create a new file. 
    /// 当你Open时，如果数据库不存在，则创建一个
    /// Date 表示年月日 Time表示时间 DateTime表示时间
    /// 实际上日期可以通过Text类型存储
    /// </summary>
    public class SqliteHelper
    {
        /// <summary>
        /// 基本的连接串
        /// </summary>
        private const string basicConnString = "Data Source=dbname.db;Version=3;";

        /// <summary>
        /// 带密码的数据库连接串,如果连接串中的data source不是全路径，默认是当前运行目录
        /// </summary>
        private const string pwdConnString = "Data Source=dbname;Version=3;Password=mypwd;";


        #region 处理连接，不需要外界关心连接的打开和关闭

        /// <summary>
        /// 处理连接，不需要外界关心连接的打开和关闭
        /// </summary>
        /// <param name="action">处理连接的回调</param>
        /// <param name="exceptionHandler">异常处理</param>
        public void HandleConnection(Action<SQLiteConnection> action,Action<Exception> exceptionHandler=null)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(basicConnString))
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


        /// <summary>
        /// 创建数据库，如果不是全限定路径，将在当前目录下创建
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="isFullPath">是否是全限定路径</param>
        public static void CreateDB(string dbName, string password, bool isFullPath)
        {
            if (isFullPath)
            {
                if (!File.Exists(dbName))
                {
                    SQLiteConnection.CreateFile(dbName);

                    if (!string.IsNullOrEmpty(password))
                    {
                        using (
                            SQLiteConnection conn =
                                new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbName)))
                        {
                            conn.Open();
                            conn.ChangePassword(password);
                        }
                    }

                }

                return;
            }
            //非权限定路径
            if (!File.Exists(dbName))
            {
                string path = Path.Combine(System.Environment.CurrentDirectory, dbName);
                SQLiteConnection.CreateFile(path);
                if (!string.IsNullOrEmpty(password))
                {
                    using (
                        SQLiteConnection conn =
                            new SQLiteConnection(string.Format("Data Source={0};Version=3;", path)))
                    {
                        conn.Open();
                        conn.ChangePassword(password);
                    }
                }
            }
        }

        /// <summary>
        /// 创建数据库，如果不是全限定路径，将在当前目录下创建
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="isFullPath">是否是全限定路径</param>
        public static void CreateDB(string dbName,  bool isFullPath)
        {
            if (isFullPath)
            {
                if (!File.Exists(dbName))
                {
                    SQLiteConnection.CreateFile(dbName);
                }

                return;
            }
            //非权限定路径
            if (!File.Exists(dbName))
            {
                string path = Path.Combine(System.Environment.CurrentDirectory, dbName);
                SQLiteConnection.CreateFile(path);
            }
        }


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
        public static int ExecNonQuery(string connString, string sql)
        {
            using (DbConnection conn = new SQLiteConnection(connString))
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
        public static int ExecNonQuery(string connString, string sql, CommandType commandType, SQLiteParameter[] parameters)
        {
            using (DbConnection conn = new SQLiteConnection(connString))
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
        /// 执行一组sql, 返回受影响的总行数
        /// </summary>
        /// <param name="sqlList"></param>
        public static int ExecuteNonQueryList(string connString, List<string> sqlList)
        {
            using (DbConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();
                using (DbCommand command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    int count = 0;
                    foreach (string item in sqlList)
                    {
                        command.CommandText = item;
                        count += command.ExecuteNonQuery();
                    }
                    return count;
                }
            }
        }


        /// <summary>
        /// 执行sql查询
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet ExecDataSet(string connString, string query)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();
                using (SQLiteCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }


        /// <summary>
        /// 执行sql查询
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet ExecDataSet(string connString, string query, CommandType commandType, IEnumerable<SQLiteParameter> parameters)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();
                using (SQLiteCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        command.Parameters.Clear();
                        foreach (SQLiteParameter sqLiteParameter in parameters)
                        {
                            command.Parameters.Add(sqLiteParameter);
                        }
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }


        /// <summary>
        /// 执行sql查询
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable ExecDataTable(string connString, string query)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();
                using (SQLiteCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }


        /// <summary>
        /// 执行sql查询
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable ExecDataTable(string connString, string query, CommandType commandType, IEnumerable<SQLiteParameter> parameters)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();
                using (SQLiteCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        command.Parameters.Clear();
                        foreach (SQLiteParameter sqLiteParameter in parameters)
                        {
                            command.Parameters.Add(sqLiteParameter);
                        }
                    }

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// 如果关闭reader,则关联的connection也关闭
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SQLiteDataReader ExecReader(string connString, string sql, CommandType commandType, IEnumerable<SQLiteParameter> parameters)
        {
            SQLiteConnection connection = new SQLiteConnection(connString);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.CommandType = commandType;
            if (parameters != null)
            {
                command.Parameters.Clear();
                foreach (SQLiteParameter sqLiteParameter in parameters)
                {
                    command.Parameters.Add(sqLiteParameter);
                }
            }
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static SQLiteDataReader ExecReader(string connString, string sql)
        {
            return ExecReader(connString, sql, CommandType.Text, null);
        }


        /// <summary>
        /// 返回查询结果的第一行第一列
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Object ExecScalar(string connString, string sql, CommandType commandType, SQLiteParameter[] parameters)
        {
            object result = null;
            using (SQLiteConnection conn = new SQLiteConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        command.Parameters.Clear();
                        foreach (SQLiteParameter sqLiteParameter in parameters)
                        {
                            command.Parameters.Add(sqLiteParameter);
                        }
                    }
                    conn.Open();
                    result = command.ExecuteScalar();
                }
            }
            return result;
        }


        public static Object ExecScalar(string connString, string sql)
        {
            return ExecScalar(connString, sql, CommandType.Text, null);
        }

        /// <summary>
        /// 指定的表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool IsTableExist(string tableName, string connString)
        {
            int count = 0;
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(conn))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT COUNT(*) FROM sqlite_master where type='table' and name=@name";
                    SQLiteParameter parameter = new SQLiteParameter("@name",DbType.String);
                    parameter.Value = tableName;
                    command.Parameters.Add(parameter);
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return count > 0;
        }

        /// <summary>
        /// 是否数据库可以连接
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static bool IsCanConnect(string connString)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch 
            {
                return false;
            }
        }

        public static void ExecTransaction(string connString,IEnumerable<string> sqlList)
        {
            using(SQLiteConnection conn = new SQLiteConnection(connString))
            {
                //开始事务
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        //提交事务
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        //回滚事务
                        transaction.Rollback();
                    }
                }

            }
        }
    }
}

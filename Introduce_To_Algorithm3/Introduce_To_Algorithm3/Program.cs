using System;
using System.Configuration;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SqliteHelper.ExecNonQuery(ConfigurationManager.ConnectionStrings["SqliteConStr"].ConnectionString, @"CREATE TABLE [Person] (
[Id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
[Name] NVARCHAR(50)  NULL,
[CreateTime] DATETIME  NULL
)");
        }
    }
}

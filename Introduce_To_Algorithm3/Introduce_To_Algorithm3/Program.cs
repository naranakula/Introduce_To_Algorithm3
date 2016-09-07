using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Introduce_To_Algorithm3.Common.DynamicProgramming;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Introduce_To_Algorithm3.Common.MachineLearning.GA;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.files;
using Introduce_To_Algorithm3.Common.Utils.sqls;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2;
using Introduce_To_Algorithm3.OpenSourceLib.Excel;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using Microsoft.CodeAnalysis.CSharp;
using MySql.Data.MySqlClient;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Util;
using MySqlHelper = Introduce_To_Algorithm3.OpenSourceLib.Utils.MySqlHelper;
using Person = Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Person;


namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string s = LCS.LcsConsecutive("hello", "woselrld");

            NLogHelper.Info(s);
            
            NLogHelper.Info(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
            NLogHelper.Info(Assembly.GetExecutingAssembly().Location);
            NLogHelper.Info(AppDomain.CurrentDomain.BaseDirectory);

            NLogHelper.Trace(DeviceId.UniqueDeviceId());
            NLogHelper.Debug(DeviceId.UniqueDeviceId());
            NLogHelper.Info(DeviceId.UniqueDeviceId());

            NLogHelper.Warn(DeviceId.UniqueDeviceId());
            NLogHelper.Error(DeviceId.UniqueDeviceId());
            NLogHelper.Fatal(DeviceId.UniqueDeviceId());


            //            string sqlConStr = "Data Source=192.168.163.218;Initial Catalog=FidsContext0317;User ID=sa;Password=system2000,.";
            //            string mySqlConStr = "server=192.168.163.225;port=3306;database=qdcargo;uid=root;password=123456";
            //            MySqlHelper mySqlHelper = MySqlHelper.GetInstance(mySqlConStr);
            //            SqlHelper sqlHelper = SqlHelper.GetInstance(sqlConStr);

            //            String sql = @"SELECT [Id]
            //      ,[Iata]
            //      ,[Icao]
            //      ,[AirportName]
            //      ,[AirportNameEg]
            //      ,[AirportNameBrief]
            //      ,[AirportNameEgBrief]
            //      ,[City]
            //  FROM [Airport]";

            //            String mysql = @"INSERT INTO airport
            //(ID,
            //IATA,
            //ICAO,
            //CITY,
            //AIRPORT_NAME,
            //AIRPORT_NAMEEG,
            //AIRPORT_NAME_BRIEF,
            //AIRPORT_NAMEEG_BRIEF,
            //CREATION_DATE)
            //VALUES
            //('{0}',
            //'{1}',
            //'{2}',
            //@CITY,
            //@AIRPORT_NAME,
            //@AIRPORT_NAMEEG,
            //@AIRPORT_NAME_BRIEF,
            //@AIRPORT_NAMEEG_BRIEF,
            //@CREATION_DATE)
            //";

            //            DataTable dt = sqlHelper.ExecuteDataTable(sql);

            //            foreach (DataRow dr in dt.Rows)
            //            {
            //                string id = dr["Id"].ToString().Replace("-", "");
            //                string iata = dr["Iata"].ToString();
            //                string icao = dr["Icao"].ToString();
            //                string airportName = dr["AirportName"].ToString();
            //                string airportNameEg = dr["AirportNameEg"].ToString();
            //                string airportNameBrief = dr["AirportNameBrief"].ToString();
            //                string airportNameEgBrief = dr["AirportNameEgBrief"].ToString();
            //                string city = dr["City"].ToString();
            //                String curMysql = mysql.FormatWith(id, iata, icao);

            //                MySqlParameter[] parameters = new MySqlParameter[6];
            //                parameters[0] = new MySqlParameter("@CITY",MySqlDbType.VarString);
            //                parameters[0].Value = city;
            //                parameters[1] = new MySqlParameter("@AIRPORT_NAME", MySqlDbType.VarString);
            //                parameters[1].Value = airportName;
            //                parameters[2] = new MySqlParameter("@AIRPORT_NAMEEG", MySqlDbType.VarString);
            //                parameters[2].Value = airportNameEg;
            //                parameters[3] = new MySqlParameter("@AIRPORT_NAME_BRIEF", MySqlDbType.VarString);
            //                parameters[3].Value = airportNameBrief;
            //                parameters[4] = new MySqlParameter("@AIRPORT_NAMEEG_BRIEF", MySqlDbType.VarString);
            //                parameters[4].Value = airportNameEgBrief;
            //                parameters[5] = new MySqlParameter("@CREATION_DATE", MySqlDbType.Timestamp);
            //                parameters[5].Value = DateTime.Now;

            //                mySqlHelper.ExecuteNonQuery(curMysql, CommandType.Text, parameters);
            //            }



        }
    }
}

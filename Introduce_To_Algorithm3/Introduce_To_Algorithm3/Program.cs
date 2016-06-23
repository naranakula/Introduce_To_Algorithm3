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
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Introduce_To_Algorithm3.Common.Utils.sqls;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2;
using Introduce_To_Algorithm3.OpenSourceLib.Excel;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using MySql.Data.MySqlClient;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Util;
using MySqlHelper = Introduce_To_Algorithm3.OpenSourceLib.Utils.MySqlHelper;


namespace Introduce_To_Algorithm3
{
    public class Program
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);
        /// <summary>
        /// 最小化窗口，即使拥有窗口的线程被挂起也会最小化。在从其他线程最小化窗口时才使用这个参数。
        /// </summary>
        public const int SW_FORCEMINIMIZE = 11;

        /// <summary>
        /// 隐藏窗口并激活其他窗口。
        /// </summary>
        public const int SW_HIDE = 0;
        /// <summary>
        /// 窗口原来的位置以原来的尺寸激活和显示窗口。
        /// </summary>
        public const int SW_SHOW = 5;
        /// <summary>
        /// 最大化指定的窗口
        /// </summary>
        public const int SW_MAXIMIZE = 3;
        /// <summary>
        /// 最小化指定的窗口并且激活在Z序中的下一个顶层窗口。
        /// </summary>
        public const int SW_MINIMIZE = 6;

        /// <summary>
        /// 激活窗口并将其最小化
        /// </summary>
        public const int SW_SHOWMINIMIZED = 2;
        /// <summary>
        /// 激活窗口并将其最大化。
        /// </summary>
        public const int SW_SHOWMAXIMIZED = 3;
        /// <summary>
        /// 激活窗口并将其最小化。
        /// </summary>
        public const int SW_SHOWNOACTIVATE = 4;

        public static void Main(string[] args)
        {
            Process[] processArr = Process.GetProcessesByName("unityapp");
            if (processArr.Length < 1)
            {
                return;
            }

            var intptr = FindWindow(null, "Unity3D");
            Console.WriteLine(intptr);
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(2000);
                ShowWindow(intptr, SW_MAXIMIZE);
                Thread.Sleep(2000);
                ShowWindow(intptr, SW_MINIMIZE);
            }

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

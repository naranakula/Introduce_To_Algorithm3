using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Introduce_To_Algorithm3.Common.Math;
using Introduce_To_Algorithm3.Common.Structs;
using Introduce_To_Algorithm3.Common.Utils;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(
                CodeCounter.GetCodeLines(
                    @"C:\Users\chlu\Documents\GitHub\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3"));

//            string connstring = @"User ID=sa;Initial Catalog=SFIDS;Data Source=chlu-pc\CHLU;Password=558276344";
//            SqlHelper helper = SqlHelper.GetInstance(connstring);
//            string sql = @"INSERT INTO [SFIDS].[dbo].[His_ImServer]
//           ([Id]
//           ,[FlightNo]
//           ,[FlightDate]
//           ,[PlanTakeOff]
//           ,[PlanArrTime]
//           ,[AlterTakeOff]
//           ,[AlterArrTime]
//           ,[AirRoute]
//           ,[Region]
//           ,[Status]
//           ,[AbnStatus]
//           ,[AbnReason]
//           ,[SecurityChannelCode]
//           ,[BoardGateCode]
//           ,[CheckinCounterCode]
//           ,[CnName]
//           ,[EnName]
//           ,[CardType]
//           ,[CardNo]
//           ,[Gender]
//           ,[Age]
//           ,[Telphone]
//           ,[BoardNo]
//           ,[Departure]
//           ,[Destination]
//           ,[SeatNo]
//           ,[AirState]
//           ,[HighTempture]
//           ,[LowTempture]
//           ,[WindState]
//           ,[Icon]
//           ,[AirQuality]
//           ,[Description]
//           ,[ClientCode]
//           ,[CreateTime])
//     VALUES
//           (@Id
//           ,@FlightNo
//           ,@FlightDate
//           ,@PlanTakeOff
//           ,@PlanArrTime
//           ,@AlterTakeOff
//           ,@AlterArrTime
//           ,@AirRoute
//           ,@Region
//           ,@Status
//           ,@AbnStatus
//           ,@AbnReason
//           ,@SecurityChannelCode
//           ,@BoardGateCode
//           ,@CheckinCounterCode
//           ,@CnName
//           ,@EnName
//           ,@CardType
//           ,@CardNo
//           ,@Gender
//           ,@Age
//           ,@Telphone
//           ,@BoardNo
//           ,@Departure
//           ,@Destination
//           ,@SeatNo
//           ,@AirState
//           ,@HighTempture
//           ,@LowTempture
//           ,@WindState
//           ,@Icon
//           ,@AirQuality
//           ,@Description
//           ,@ClientCode
//           ,@CreateTime)";

//            for (int i = 0; i < 100; i++)
//            {
                
//                try
//                {
//                    SqlParameter[] parameters = new SqlParameter[35];
//                    parameters[0] = new SqlParameter("@Id",SqlDbType.UniqueIdentifier);
//                    parameters[0].Value = Guid.NewGuid();
//                    parameters[1] = new SqlParameter("@FlightNo", SqlDbType.NVarChar);
//                    parameters[1].Value = 1000+i%8;
//                    parameters[2] = new SqlParameter("@FlightDate", SqlDbType.DateTime);
//                    parameters[2].Value = DateTime.Now;
//                    parameters[3] = new SqlParameter("@PlanTakeOff", SqlDbType.DateTime);
//                    parameters[3].Value = DateTime.Now;
//                    parameters[4] = new SqlParameter("@PlanArrTime", SqlDbType.DateTime);
//                    parameters[4].Value = DateTime.Now;
//                    parameters[5] = new SqlParameter("@AlterTakeOff", SqlDbType.DateTime);
//                    parameters[5].Value = DateTime.Now;
//                    parameters[6] = new SqlParameter("@AlterArrTime", SqlDbType.DateTime);
//                    parameters[6].Value = DateTime.Now;
//                    parameters[7] = new SqlParameter("@AirRoute", SqlDbType.NVarChar);
//                    parameters[7].Value = "AirRoute";
//                    parameters[8] = new SqlParameter("@Status", SqlDbType.NVarChar);
//                    parameters[8].Value = "Status";
//                    parameters[10] = new SqlParameter("@Region", SqlDbType.NVarChar);
//                    parameters[10].Value = "region";
//                    parameters[11] = new SqlParameter("@ClientCode", SqlDbType.NVarChar);
//                    parameters[11].Value = "ClientCode";
//                    parameters[12] = new SqlParameter("@AbnStatus", SqlDbType.NVarChar);
//                    parameters[12].Value = "AbnStatus";
//                    parameters[13] = new SqlParameter("@AbnReason", SqlDbType.NVarChar);
//                    parameters[13].Value = "AbnReason";
//                    parameters[14] = new SqlParameter("@SecurityChannelCode", SqlDbType.NVarChar);
//                    parameters[14].Value = "111212";
//                    parameters[15] = new SqlParameter("@BoardGateCode", SqlDbType.NVarChar);
//                    parameters[15].Value = "11212";
//                    parameters[16] = new SqlParameter("@CheckinCounterCode", SqlDbType.NVarChar);
//                    parameters[16].Value = "112";
//                    parameters[17] = new SqlParameter("@CnName", SqlDbType.NVarChar);
//                    parameters[17].Value = "CnName";
//                    parameters[18] = new SqlParameter("@EnName", SqlDbType.NVarChar);
//                    parameters[18].Value = "EnName";
//                    parameters[19] = new SqlParameter("@CardType", SqlDbType.NVarChar);
//                    parameters[19].Value = "身份证";
//                    parameters[20] = new SqlParameter("@CardNo", SqlDbType.NVarChar);
//                    parameters[20].Value = "112sa1111111123";
//                    parameters[21] = new SqlParameter("@Gender", SqlDbType.Bit);
//                    parameters[21].Value = true;
//                    parameters[22] = new SqlParameter("@Age", SqlDbType.Int);
//                    parameters[22].Value = 20;
//                    parameters[23] = new SqlParameter("@Telphone", SqlDbType.NVarChar);
//                    parameters[23].Value = "123774774";
//                    parameters[24] = new SqlParameter("@BoardNo", SqlDbType.NVarChar);
//                    parameters[24].Value = "123774774";
//                    parameters[25] = new SqlParameter("@Departure", SqlDbType.NVarChar);
//                    parameters[25].Value = "tao";
//                    parameters[26] = new SqlParameter("@Destination", SqlDbType.NVarChar);
//                    parameters[26].Value = "dao";
//                    parameters[27] = new SqlParameter("@SeatNo", SqlDbType.NVarChar);
//                    parameters[27].Value = "27388";
//                    parameters[28] = new SqlParameter("@AirState", SqlDbType.NVarChar);
//                    parameters[28].Value = "良好";
//                    parameters[29] = new SqlParameter("@HighTempture", SqlDbType.NVarChar);
//                    parameters[29].Value = "29c";
//                    parameters[30] = new SqlParameter("@LowTempture", SqlDbType.NVarChar);
//                    parameters[30].Value = "29c";
//                    parameters[31] = new SqlParameter("@WindState", SqlDbType.NVarChar);
//                    parameters[31].Value = "威风";
//                    parameters[32] = new SqlParameter("@Icon", SqlDbType.NVarChar);
//                    parameters[32].Value = "Icon";
//                    parameters[33] = new SqlParameter("@AirQuality", SqlDbType.NVarChar);
//                    parameters[33].Value = "秦心";
//                    parameters[9] = new SqlParameter("Description", SqlDbType.NVarChar);
//                    parameters[9].Value = "良好";
//                    parameters[34] = new SqlParameter("CreateTime", SqlDbType.DateTime);
//                    parameters[34].Value = DateTime.Now;

//                    helper.ExecuteNonQuery(sql,CommandType.Text,parameters);
//                }
//                catch (Exception ex )
//                {
                    
//                    throw ex;
//                }
//                Thread.Sleep(1010);
//                Console.WriteLine(i);
//            }
        }
    }
}
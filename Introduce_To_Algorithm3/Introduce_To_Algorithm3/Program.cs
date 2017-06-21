using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.Serial;
using Introduce_To_Algorithm3.OpenSourceLib.Dapper;
using Introduce_To_Algorithm3.OpenSourceLib.FTP;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
            EdtFtpHelper ftpHelper = new EdtFtpHelper("172.17.12.95", "scmis", "scmis", 21);

            string imageFileName = @"/PassengerImg/2017-06-20\SC4697\SC4697-20170620-139-131423847679843750.jpg";

            ftpHelper.DownloadFile(@"D:\images\黄辉.jpg", imageFileName,ex=> { NLogHelper.Error(ex.ToString()); });
            */

            NLogHelper.Info(GuidUtils.GetGuid2());
            NLogHelper.Trace("Trace");
            NLogHelper.Debug("Debug");
            NLogHelper.Info("Info");
            NLogHelper.Warn("Warn");
            NLogHelper.Error("Error");
            NLogHelper.Fatal("Fatal");
            
        }
    }
}

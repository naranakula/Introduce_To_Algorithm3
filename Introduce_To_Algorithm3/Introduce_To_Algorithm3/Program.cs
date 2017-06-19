using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.Serial;
using Introduce_To_Algorithm3.OpenSourceLib.Dapper;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {

            SerialPortUtils portUtils = SerialPortUtils.GetInstance();
            portUtils.InitSerialPort("COM10", 9600);

            portUtils.Start();

            System.Console.ReadLine();
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

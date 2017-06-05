using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NLogHelper.Info(CodeCounter.GetCodeLines(@"C:\Users\cmlu\Documents\GitHub\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3") +"");
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

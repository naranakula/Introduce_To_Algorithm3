using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(MlHelper.ToString(MlHelper.Variance( MlHelper.Rand(160,6))));
        }
    }
}

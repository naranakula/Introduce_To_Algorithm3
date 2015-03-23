using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Utility.Commons;
using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Introduce_To_Algorithm3.Common.Math;
using Introduce_To_Algorithm3.Common.Structs;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Utils.threads;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using Introduce_To_Algorithm3.OpenSourceLib.Utils.quartzs;
using log4net.Util;
using MathNet.Numerics.LinearAlgebra.Double;
using Quartz;
using Quartz.Impl;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
        }
    }

    /*
     * 
    //正确使用wcf的示例
    CalculatorClient wcfClient = new CalculatorClient();
    try
    {
        Console.WriteLine(wcfClient.Add(4, 6));
        wcfClient.Close();
    }
    catch (TimeoutException timeProblem)
    {
          Console.WriteLine("The service operation timed out. " + timeProblem.Message);
          Console.ReadLine();
          wcfClient.Abort();
    }
    catch (FaultException<GreetingFault> greetingFault)
        {
          Console.WriteLine(greetingFault.Detail.Message);
          Console.ReadLine();
          wcfClient.Abort();
        }
        catch (FaultException unknownFault)
        {
          Console.WriteLine("An unknown exception was received. " + unknownFault.Message);
          Console.ReadLine();
          wcfClient.Abort();
        }
        catch (CommunicationException commProblem)
        {
          Console.WriteLine("There was a communication problem. " + commProblem.Message + commProblem.StackTrace);
          Console.ReadLine();
          wcfClient.Abort();
        }*/



}
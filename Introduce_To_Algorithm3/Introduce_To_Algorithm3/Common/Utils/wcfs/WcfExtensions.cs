using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils.wcfs
{
    /// <summary>
    /// Wcf客户端调用拓展
    /// </summary>
    public  static class WcfExtensions
    {
        /// <summary>
        /// wcf调用拓展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="action"></param>
        /// <param name="exceptionHandler">异常处理</param>
        public static void Using<T>(this T client, Action<T> action,Action<Exception> exceptionHandler = null) where T : ICommunicationObject
        {
            if (client == null)
            {
                return;
            }
            try
            {
                if (action != null)
                {
                    action(client);
                }
                client.Close();
            }
            catch (TimeoutException te)
            {
                client.Abort();
                if (exceptionHandler != null)
                {
                    exceptionHandler(te);
                }
            }
            catch (FaultException fe)
            {
                client.Abort();
                if (exceptionHandler != null)
                {
                    exceptionHandler(fe);
                }
            }
            catch (CommunicationException ce)
            {
                client.Abort();
                if (exceptionHandler != null)
                {
                    exceptionHandler(ce);
                }
            }
            catch (Exception e)
            {
                client.Abort();
                if (exceptionHandler != null)
                {
                    exceptionHandler(e);
                }
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
}

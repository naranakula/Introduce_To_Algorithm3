using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.wcfs
{
    public  static class WcfExtensions
    {
        /// <summary>
        /// wcf调用拓展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="action"></param>
        public static void Using<T>(this T client, Action<T> action) where T : ICommunicationObject
        {
            if (client == null)
            {
                return;
            }
            try
            {
                action(client);
                client.Close();
            }
            catch (TimeoutException te)
            {
                client.Abort();
            }
            catch (FaultException fe)
            {
                client.Abort();
            }
            catch (CommunicationException ce)
            {
                client.Abort();
            }
            catch (Exception e)
            {
                client.Abort();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
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
        /// 详情查看文档:https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/avoiding-problems-with-the-using-statement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="action"></param>
        /// <param name="exceptionHandler">异常处理</param>
        public static void Using<T>(this T client, Action<T> action, Action<Exception> exceptionHandler = null) where T : ICommunicationObject
        {
            //是否异常处理，保证异常只处理一次
            bool isExceptionHandled = false;

            try
            {
                dynamic dynamicClient = client;

                ServiceEndpoint endpoint = dynamicClient.Endpoint;

                endpoint.Address = GenerateAddress(endpoint.Address, "localhost", 3668);

                if (action != null)
                {
                    try
                    {
                        action(client);
                    }
                    catch (Exception ex)
                    {
                        if (exceptionHandler != null)
                        {
                            exceptionHandler(ex);
                            isExceptionHandled = true;
                        }
                    }
                }


                client.Close();
            }
            catch (TimeoutException te)
            {
                client.Abort();
                if (exceptionHandler != null && isExceptionHandled == false)
                {
                    exceptionHandler(te);
                }
            }
            catch (FaultException fe)
            {
                client.Abort();
                if (exceptionHandler != null && isExceptionHandled == false)
                {
                    exceptionHandler(fe);
                }
            }
            catch (CommunicationException ce)
            {
                client.Abort();
                if (exceptionHandler != null && isExceptionHandled == false)
                {
                    exceptionHandler(ce);
                }
            }
            catch (Exception e)
            {
                client.Abort();

                if (exceptionHandler != null && isExceptionHandled == false)
                {
                    exceptionHandler(e);
                }
            }
        }


        /// <summary>
        /// 替换url中的ip和端口号
        /// </summary>
        /// <param name="oldAddress"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private static EndpointAddress GenerateAddress(EndpointAddress oldAddress, string ip, int port)
        {
            string oldAddressString = oldAddress.ToString().Trim();

            int index1 = oldAddressString.IndexOf("://", StringComparison.InvariantCulture);

            if (index1 < 0)
            {
                return oldAddress;
            }


            int index2 = oldAddressString.IndexOf("/", index1 + 3, StringComparison.InvariantCulture);

            if (index2 < 0)
            {
                return oldAddress;
            }

            string newAddressString = oldAddressString.Substring(0, index1 + 3) + ip + ":" + port + oldAddressString.Substring(index2);
            return new EndpointAddress(newAddressString);
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

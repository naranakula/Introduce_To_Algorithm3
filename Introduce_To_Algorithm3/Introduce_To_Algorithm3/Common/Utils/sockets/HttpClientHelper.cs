using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets
{
    /// <summary>
    /// HttpClient帮助类
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        /// 默认的超时时间 单位秒
        /// </summary>
        public const int DefaultTimeoutInSeconds = 7;

        /*
         * basic http auth:
         * 1、每次请求加上用户名密码 用户名:密码用冒号隔开 
         * 2、使用utf-8转换为字节
         * 2、使用base64转换字符串
         * 3、添加http头 Authorization: The authorization method and a space (e.g. "Basic ") is then prepended to the encoded string.
          Authorization:Basic base64
         * Authorization: Basic QWxhZGRpbjpPcGVuU2VzYW1l
         * 
         *  Dictionary<string,string> dict = new Dictionary<string, string>();
         *  dict.Add("Accept", "application/json");
            dict.Add("Accept-Charset","utf-8");
            string userNameAndPwd = StringUtils.ToBase64String(Encoding.UTF8.GetBytes("admin:admin"));
            dict.Add("Authorization", $"Basic {userNameAndPwd}");
            
         * 
         */


        #region Get相关

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="responseHandler">响应处理</param>
        /// <param name="headerDict">http头内容，如果为null，使用默认的http头，否则将会设置http头，如果指定的http头已经存在，则覆盖而不是append</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns>是否有异常</returns>
        public static bool Get(string requestUrl, Action<HttpResponseMessage> responseHandler = null,
            Dictionary<string, string> headerDict = null, Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0,0,DefaultTimeoutInSeconds);
                    

                    #region 处理http头
                    HandleHttpHeader(client, headerDict);
                    #endregion

                    using (HttpResponseMessage response = client.GetAsync(requestUrl).Result)
                    {
                        responseHandler?.Invoke(response);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        /// <summary>
        /// get获取字符串
        /// </summary>
        /// <param name="requestUrl">请求的地址</param>
        /// <param name="encoding">解析响应使用的编码</param>
        /// <param name="headerDict">http头内容，如果为null，使用默认的http头，否则将会设置http头，如果指定的http头已经存在，则覆盖而不是append</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns>如果异常返回null，否则返回响应包体内容</returns>
        public static string GetString(string requestUrl, Encoding encoding,
            Dictionary<string, string> headerDict = null, Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, DefaultTimeoutInSeconds);


                    #region 处理http头
                    HandleHttpHeader(client, headerDict);
                    #endregion

                    //using (var stream = client.GetStreamAsync(requestUrl).Result)
                    //{
                    //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    //    {
                    //        return reader.ReadToEnd();
                    //    }
                    //}

                    using (HttpResponseMessage response = client.GetAsync(requestUrl).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            byte[] byteArr = content.ReadAsByteArrayAsync().Result;
                            return encoding.GetString(byteArr);
                        }
                    }
                }
                

            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return null;
            }
        }

        #endregion

        #region Post相关

        /// <summary>
        /// post string content
        /// </summary>
        /// <param name="requestUrl">请求的地址</param>
        /// <param name="content">上传的body</param>
        /// <param name="encoding">内容的编码</param>
        /// <param name="responseHandler">响应处理</param>
        /// <param name="headerDict">http头内容，如果为null，使用默认的http头，否则将会设置http头，如果指定的http头已经存在，则覆盖而不是append</param>
        /// <param name="mediaType">要使用的内容的媒体类型。</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns>是否有异常</returns>
        public static bool PostString(string requestUrl, string content, Encoding encoding, string mediaType = "application/json", Action<HttpResponseMessage> responseHandler = null, Dictionary<string, string> headerDict = null, Action<Exception> exceptionHandler = null)
        {
            try
            {
                using (StringContent bodyContent = new StringContent(content, encoding, mediaType))
                {
                    using (HttpClient client = new HttpClient())
                    {

                        client.Timeout = new TimeSpan(0, 0, DefaultTimeoutInSeconds);

                        #region 处理http头
                        HandleHttpHeader(bodyContent,headerDict);
                        #endregion
                        

                        using (HttpResponseMessage response = client.PostAsync(requestUrl, bodyContent).Result)
                        {
                            responseHandler?.Invoke(response);
                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return false;
            }
        }

        /// <summary>
        /// 提交只包含文本的表单内容，形式为键值对，类型为application/x-www-form-urlencoded
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="content"></param>
        /// <param name="responseHandler"></param>
        /// <param name="headerDict"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool PostFormUtlEncodedContent(string requestUrl, IList<KeyValuePair<string, string>> content,
            Action<HttpResponseMessage> responseHandler = null, Dictionary<string, string> headerDict = null,
            Action<Exception> exceptionHandler = null)
        {

            try
            {
                using (FormUrlEncodedContent bodyContent = new FormUrlEncodedContent(content))
                {
                    using (HttpClient client = new HttpClient())
                    {

                        client.Timeout = new TimeSpan(0, 0, DefaultTimeoutInSeconds);

                        #region 处理http头
                        HandleHttpHeader(bodyContent, headerDict);
                        #endregion


                        using (HttpResponseMessage response = client.PostAsync(requestUrl, bodyContent).Result)
                        {
                            responseHandler?.Invoke(response);
                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return false;
            }

        }




        #endregion


        #region 辅助方法

        /// <summary>
        /// 处理http 头，如果为null，使用默认的http头，否则将会设置http头，如果指定的http头已经存在，则覆盖而不是append
        /// </summary>
        /// <param name="client"></param>
        /// <param name="headerDict"></param>
        private static void HandleHttpHeader(HttpClient client, Dictionary<string, string> headerDict)
        {
            if (headerDict == null || client == null)
            {
                return;
            }

            foreach (var dictItem in headerDict)
            {
                if (client.DefaultRequestHeaders.Contains(dictItem.Key))
                {
                    client.DefaultRequestHeaders.Remove(dictItem.Key);
                }

                client.DefaultRequestHeaders.Add(dictItem.Key, dictItem.Value);
            }

        }


        /// <summary>
        /// 处理http 头，如果为null，使用默认的http头，否则将会设置http头，如果指定的http头已经存在，则覆盖而不是append
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="headerDict"></param>
        private static void HandleHttpHeader(HttpContent httpContent, Dictionary<string, string> headerDict)
        {
            if (headerDict == null || httpContent == null)
            {
                return;
            }

            foreach (var dictItem in headerDict)
            {
                
                if (httpContent.Headers.Contains(dictItem.Key))
                { 
                    httpContent.Headers.Remove(dictItem.Key);
                }

                httpContent.Headers.Add(dictItem.Key, dictItem.Value);
            }

        }


        #endregion


    }
}

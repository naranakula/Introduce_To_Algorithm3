using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Extensions;

namespace Introduce_To_Algorithm3.OpenSourceLib.RestSharp
{
    /// <summary>
    /// Simple REST and HTTP API Client for .NET
    /// https://github.com/restsharp/RestSharp/wiki/Deserialization
    /// RestSharp包含deserializer来处理XML和JSON,在接受响应后，RestSharp根据Response的Content Type来选择合适的deserializer。 对DateTime的支持见文档
    /// application/json - JsonDeserializer
    /// application/xml - XmlDeserializer
    /// text/json - JsonDeserializer
    /// text/xml - XmlDeserializer
    /// </summary>
    public class RestSharpHelper
    {
        /// <summary>
        /// BaseUrl 如：http://www.baidu.com
        /// </summary>
        private const String DefaultBaseUrl = "http://www.baidu.com";

        #region 单例
        /// <summary>
        /// 单例
        /// </summary>
        private static RestSharpHelper instance;

        /// <summary>
        /// 私有底层RestClient实例 相当于一个session，不同的实例相当于不同的session
        /// </summary>
        private RestClient restClient;

        /// <summary>
        /// 基Url
        /// </summary>
        private readonly string baseUrl;

        /// <summary>
        /// 私有构造函数
        /// 使用DefaultBaseUrl
        /// </summary>
        private RestSharpHelper()
        {
            baseUrl = DefaultBaseUrl;
            restClient = new RestClient(DefaultBaseUrl);
        }

        /// <summary>
        /// 私有构造函数
        /// </summary>
        /// <param name="baseUrl">基Url</param>
        private RestSharpHelper(string baseUrl)
        {
            this.baseUrl = baseUrl;
            restClient = new RestClient(baseUrl);
        }

        /// <summary>
        /// 单例
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static RestSharpHelper GetInstance(string baseUrl)
        {
            if (instance != null)
            {
                return instance;
            }

            if (instance == null)
            {
                instance = new RestSharpHelper(baseUrl);
            }

            return instance;
        }

        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static RestSharpHelper GetInstance()
        {
            if (instance != null)
            {
                return instance;
            }

            if (instance == null)
            {
                instance = new RestSharpHelper();
            }

            return instance;
        }

        #endregion

        /// <summary>
        /// 底层实例
        /// </summary>
        public RestClient RestClient
        {
            get { return restClient; }
        }

        /// <summary>
        /// 初始化配置RestClient
        /// 调用一次
        /// </summary>
        public void InitRestClient()
        {
            //认证 设置用户名密码
            restClient.Authenticator = new SimpleAuthenticator("usernameKey","username","passwordKey","password");

            //支持Cookies
            restClient.CookieContainer = new CookieContainer();
            //设置编码格式
            restClient.Encoding = Encoding.UTF8;
            //超时时间5s
            restClient.Timeout = 5000;

            restClient.ReadWriteTimeout = 5000;
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        public void Execute()
        {
            #region 构建请求
            RestRequest request = new RestRequest(Method.GET);//指定请求的Method
            request.Resource = "status/time.xml";//最终的请求地址是baseUrl/resource
            //添加header请求头
            request.AddHeader("name", "value");
            //基于Method添加post请求体或者URL requeststring
            request.AddParameter("name", "value");
            //Calls AddParameter() for all public, readable properties of obj
            request.AddObject(new object());
            //将对象序列化为json，添加到请求体
            request.AddJsonBody(new object());
            //上传文件 Adds a file to the Files collection to be included with a POST or PUT request (other methods do not support file uploads).
            string filePath = "";
            request.AddFile("file", filePath);
            #endregion

            #region 获取原生字符串响应

            //原生响应
            IRestResponse rawresponse = restClient.Execute(request);

            if (rawresponse.ResponseStatus == ResponseStatus.Completed && rawresponse.StatusCode == HttpStatusCode.OK)
            {
                string content = rawresponse.Content;//响应包体
                IList<Parameter> headers = rawresponse.Headers;//响应包头
            }


            #endregion

            #region 响应自动deserialize

            //原生响应 DataTable是演示用
            IRestResponse<DataTable> response = restClient.Execute<DataTable>(request);

            if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)
            {
                string content = rawresponse.Content;//响应包体
                IList<Parameter> headers = rawresponse.Headers;//响应包头
                DataTable dt = response.Data;//获取deserialize的对象
            }


            #endregion

            //下载文件并保存 注：DownloadData已经下载完文件，大文件会产生内存问题
            restClient.DownloadData(request).SaveAs(filePath);

            //异步执行
            var asyncHandle =restClient.ExecuteAsync(request, response1 =>
            {
                if (response1.StatusCode == HttpStatusCode.OK)
                {
                    //调用成功
                }
            });

            //异步执行 带deserialize
            restClient.ExecuteAsync<DataTable>(request, response1 =>
            {
                if (response1.StatusCode == HttpStatusCode.OK)
                {
                    //调用成功
                    DataTable dt = response1.Data;
                }
            });

            #region 避免大文件问题的下载

            string tempFile = Path.GetTempFileName();
            using (var writer = File.OpenWrite(tempFile))
            {
                var client = new RestClient(baseUrl);
                var fileRequest = new RestRequest("Assets/LargeFile.7z");
                //写入到文件中而不是内存
                request.ResponseWriter = (responseStream) => responseStream.CopyTo(writer);
                byte[] byteResponse = client.DownloadData(request);
            }

            #endregion
        }
    }

    
}

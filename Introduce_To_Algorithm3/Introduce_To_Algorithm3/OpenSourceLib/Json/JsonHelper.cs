using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Introduce_To_Algorithm3.OpenSourceLib.Json
{
    /// <summary>
    /// JSON帮助类
    /// 详情咨询网站：http://www.newtonsoft.com/json/help/html/Introduction.htm
    /// 支持list\Dictionary\object的序列化和反序列化
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// json序列化的设置
        /// </summary>
        private static readonly JsonSerializerSettings _jsonSerializerSettings;

        #region 静态构造函数

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static JsonHelper()
        {
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,//当反序列化时，如果json字符串中包含不在对象中的成员，则忽略而不抛异常,这是Json默认的处理方式
                //经试验证明，当json缺少包含在对象中的成员时，也可以反序列化
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,//序列化时忽略循环引用， The first time an object is encountered it will be serialized as usual but if the object is encountered as a child object of itself the serializer will skip serializing it.，默认的方式是Error
                NullValueHandling = NullValueHandling.Include,//Json.NET writes null values to JSON when serializing and sets null values to fields/properties when deserializing.这是json默认的处理方式
                DefaultValueHandling = DefaultValueHandling.Include,//这是默认的处理方式，Json.NET will write a field/property value to JSON when serializing if the value is the same as the field/property's default value. The Json.NET deserializer will continue setting a field/property if the JSON value is the same as the default value.
            };

            //定义在序列化反序列化时DateTime类型使用的格式
            _jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter(){DateTimeFormat = "yyyy-MM-dd HH:mm:ss"});
        }

        #endregion 

        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="obj">如果obj为null,则返回字符串null</param>
        /// <returns>如果操作success,则Result包含结果</returns>
        public static OperationResult<string> ToJson(Object obj)
        {
            try
            {
                return new OperationResult<string>()
                {
                    ResultType = OperationResultType.Success,
                    Result = JsonConvert.SerializeObject(obj, Formatting.None, _jsonSerializerSettings)
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<string>()
                {
                    ResultType = OperationResultType.Error,
                    Result = null,
                    ResultException = ex
                };
            }
        }

        /// <summary>
        /// 将json字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static OperationResult<T> FromJson<T>(string jsonStr)
        {
            try
            {
                return new OperationResult<T>()
                {
                    ResultType = OperationResultType.Success,
                    Result = JsonConvert.DeserializeObject<T>(jsonStr, _jsonSerializerSettings)
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<T>()
                {
                    ResultType = OperationResultType.Error,
                    Result = default(T),
                    ResultException = ex
                };
            }
            
        }
    }
}

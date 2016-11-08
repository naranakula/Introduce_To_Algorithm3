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
        /// 
        /// [JsonConverter(typeof(StringEnumConverter))]
        /// 对于枚举属性，可以使用上面的注解为字符串，默认枚举转换为了相应的整数
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

            //定义在序列化反序列化时DateTime类型使用的格式,去掉这句试用默认的时间处理方式
            //Json.Net中自带的两个处理日期的类,默认是IsoDateTimeConverter ,它的默认格式是"yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK".另一个是JavaScriptTimeConverter,它的格式是 "new Date(ticks)",其实返回的是一个JavaScript的Date对象.
            //_jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter(){DateTimeFormat = "yyyy-MM-dd HH:mm:ss"});
            //_jsonSerializerSettings.Converters.Add(new JavaScriptDateTimeConverter());
        }

        #endregion

        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="obj">如果obj为null,则返回字符串null</param>
        /// <param name="format">None表示不缩进，更紧凑，Indented表示缩进，更好看</param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static string ToJson(Object obj,Action<Exception>  exceptionHandler=null,Formatting format = Formatting.None )
        {

            try
            {
                return JsonConvert.SerializeObject(obj, format, _jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }
        }

        /// <summary>
        /// 将json字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static T FromJson<T>(string jsonStr, Action<Exception> exceptionHandler = null)
        {
            try
            {

                return JsonConvert.DeserializeObject<T>(jsonStr, _jsonSerializerSettings);
                
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return default(T);
            }
            
        }
    }
}

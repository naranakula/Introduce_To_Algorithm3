using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Introduce_To_Algorithm3.Common.Utils.strings;
using Introduce_To_Algorithm3.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace Introduce_To_Algorithm3.OpenSourceLib.Json
{
    /// <summary>
    /// JSON帮助类
    /// 详情咨询网站：http://www.newtonsoft.com/json/help/html/Introduction.htm
    /// 支持list\Dictionary\object的序列化和反序列化
    /// [JsonIgnore]//JsonIgnore标记忽略序列化
    /// 对象不需要加serialized等标记
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// json序列化的设置
        /// 
        /// [JsonConverter(typeof(StringEnumConverter))]
        /// 对于枚举属性，可以使用上面的注解为字符串，默认枚举转换为了相应的整数
        /// </summary>
        private static volatile JsonSerializerSettings _jsonSerializerSettings;

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
                //尽量避免循环引用[JsonIgnore]//JsonIgnore标记忽略序列化
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,//序列化时忽略循环引用， The first time an object is encountered it will be serialized as usual but if the object is encountered as a child object of itself the serializer will skip serializing it.，默认的方式是Error  Serialize (存在循环引用但不是无限循环)This option forces Json.NET to serialize objects in reference loops.This is useful if objects are nested but not indefinitely.
                NullValueHandling = NullValueHandling.Include,//Json.NET writes null values to JSON when serializing and sets null values to fields/properties when deserializing.这是json默认的处理方式
                DefaultValueHandling = DefaultValueHandling.Include,//这是默认的处理方式，Json.NET will write a field/property value to JSON when serializing if the value is the same as the field/property's default value. The Json.NET deserializer will continue setting a field/property if the JSON value is the same as the default value.  Ignore:忽略
                DateFormatHandling = DateFormatHandling.IsoDateFormat,//IsoDateFormat 默认值 By default Json.NET writes dates in the ISO 8601 format, e.g. "2012-03-21T05:40Z".//MicrosoftDateFormat Dates are written in the Microsoft JSON format, e.g. "\/Date(1198908717056)\/".
                TypeNameHandling = TypeNameHandling.None,//None是默认值，序列化和反序列化时不包含类型信息
            };

            //定义在序列化反序列化时DateTime类型使用的格式,去掉这句试用默认的时间处理方式
            //Json.Net中自带的两个处理日期的类,默认是IsoDateTimeConverter ,它的默认格式是"yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK".另一个是JavaScriptTimeConverter,它的格式是 "new Date(ticks)",其实返回的是一个JavaScript的Date对象.
            //_jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter(){DateTimeFormat = "yyyy-MM-dd HH:mm:ss"});
            //_jsonSerializerSettings.Converters.Add(new JavaScriptDateTimeConverter());
        }

        #endregion

        #region 序列化反序列化

        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="obj">如果obj为null,则返回字符串null</param>
        /// <param name="format">None表示不缩进，更紧凑，Indented表示缩进，更好看</param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static string ToJson(Object obj, Formatting format = Formatting.Indented, Action < Exception>  exceptionHandler=null)
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

        #endregion

        #region Linq to json 

        /// <summary>
        /// 解析json字符串
        /// </summary>
        public static void ParseJsonExample()
        {
            /*
             * Json有三种类型：
             * .net基础类型 对应 JValue  如 1 没有标志
             * 数组集合类型 对应 JArray  以[]为标志
             * 其它.net类型 对应 JObject 以{}为标志
             * 
             * 使用正确的类型来解析json字符串
             */
            string jsonStr = @"{
      'CPU': 'Intel',
      'Drives': [
      'DVD read/writer',
      '500 gigabyte hard drive'
      ]
      }";
            JObject jObject = JObject.Parse(jsonStr);
            //GetValue和[]是一个操作  最后使用下面定义的拓展方法
            string cpu = (string) jObject.GetValue("CPu", StringComparison.CurrentCultureIgnoreCase);
            cpu = (string)jObject["CPU"];
            List<string> allDrives = jObject["Drives"].Select(r => (string) r).ToList();
            allDrives = jObject["Drives"].Values<string>().ToList();
            allDrives = (from r in jObject["Drives"] select r.Value<string>()).ToList();
            jObject = (JObject)jObject.GetValue("data");
            jsonStr = @"[
  'Small',
  'Medium',
  'Large'
]";
            //该字符串不能用JObject来解析
            JArray jArray = JArray.Parse(jsonStr);
            List<string> nameList = jArray.Select(r => (string) r).ToList();





            String str = "{\"code\":\"0\",\"msg\":\"success\",\"data\":{\"id\":\"2c96232a64916f1b0164916f2aae0002\",\"airportCode\":\"XMN\",\"description\":\"厦门机场\",\"serverInfos\":[{\"id\":\"2c96232a64916f1b0164916f2a9b0001\",\"protocol\":\"http\",\"url\":\"http://www.baidu.com\",\"isDefault\":\"1\"}],\"rules\":[{\"id\":\"2c96232a64916f1b0164916f29f80000\",\"startTime\":\"00:00\",\"finishTime\":\"04:00\",\"isDefault\":\"1\"}]}}";
            JObject jobject = JObject.Parse(str);
            JObject data = (JObject)jobject.GetValue("data");
            JArray serverInfos = (JArray)data.GetValue("serverInfos");
            for (int i = 0; i < serverInfos.Count; i++)
            {
                JObject serverInfo = (JObject)serverInfos[i];
                String protocol = (string)serverInfo.GetValue("protocol");
                string url = (string)serverInfo.GetValue("url");

                if (StringUtils.EqualsEx("https", protocol))
                {
                    break;
                }
            }


            JArray rules = (JArray)data.GetValue("rules");

            for (int i = 0; i < rules.Count; i++)
            {
                JObject item = (JObject)rules[i];
                string startTime = (string)item.GetValue("startTime");
                string endTime = (string)item.GetValue("finishTime");
            }



        }

        /// <summary>
        /// 根据PropertyName获取字符串 返回<字符串，是否解析成功>
        /// </summary>
        /// <param name="jobject"></param>
        /// <param name="propertyName"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static Tuple<string, bool> GetString(this JObject jobject, string propertyName,Action<Exception> exceptionHandler=null)
        {
            try
            {
                string propertyValue = (string)jobject.GetValue(propertyName, StringComparison.CurrentCultureIgnoreCase);
                return new Tuple<string, bool>(propertyValue,true);
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return new Tuple<string, bool>(string.Empty,false);
            }
        }

        /// <summary>
        /// 根据PropertyName获取字符串 返回<字符串，是否解析成功>
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="propertyName"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static Tuple<string, bool> GetString(this JToken jToken, string propertyName, Action<Exception> exceptionHandler = null)
        {
            try
            {
                string propertyValue = (string)jToken[propertyName];
                return new Tuple<string, bool>(propertyValue, true);
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                return new Tuple<string, bool>(string.Empty, false);
            }
        }

        #endregion

        #region Json 和 Xml 之间转换

        /// <summary>
        /// 将xml转换为json字符串
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static string FromXmlToJson(string xmlStr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            string jsonStr = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);

            /*
             //两种方式都可以
            XDocument doc = XDocument.Parse(xmlStr);
            string jsonStr = JsonConvert.SerializeXNode(doc);
            */
            return jsonStr;
        }

        /// <summary>
        /// 将json转换为xml 只能转换JObject，不能转换JArray
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="rootName">增加根元素的名字</param>
        /// <returns></returns>
        public static string FromJsonToXml(string jsonStr,string rootName="root")
        {
            //XmlDocument doc1 = JsonConvert.DeserializeXmlNode(jsonStr,rootName);
            XDocument doc = JsonConvert.DeserializeXNode(jsonStr,rootName);
            return doc.ToString();
        }


        #endregion

    }

    /// <summary>
    /// Json会序列化所有的public Fields 和 Properties
    /// </summary>
    public class JsonObjectExample
    {
        [JsonIgnore]//JsonIgnore标记忽略序列化
        public string Name { get; set; }

        public int Number { get; set; }
    }
}

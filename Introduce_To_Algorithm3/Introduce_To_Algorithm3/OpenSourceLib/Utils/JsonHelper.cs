using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// JSON 帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Json序列器设备
        /// </summary>
        private static readonly JsonSerializerSettings JsonSettings;

        /// <summary>
        /// 时间格式
        /// </summary>
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static JsonHelper()
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = DateTimeFormat };
            JsonSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            JsonSettings.Converters.Add(dateTimeConverter);
        }

        /// <summary>
        /// convert to json string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            try
            {
                return JsonConvert.SerializeObject(obj, Formatting.None, JsonSettings);
            }
            catch (Exception e)
            {
                throw new JsonSerializationException(e.Message);
            }
        }

        /// <summary>
        /// convert json string to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString, JsonSettings);
            }
            catch (Exception e)
            {
                throw new JsonSerializationException(e.Message);
            }
        }
    }
}

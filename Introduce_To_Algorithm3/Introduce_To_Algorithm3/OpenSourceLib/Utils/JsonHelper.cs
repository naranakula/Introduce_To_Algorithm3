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
    /// json helper
    /// </summary>
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings JsonSettings;
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        static JsonHelper()
        {
            var dataTimeConverter = new IsoDateTimeConverter {DateTimeFormat = DateTimeFormat};
            JsonSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            JsonSettings.Converters.Add(dataTimeConverter);
        }

        /// <summary>
        /// Convert to json string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            if (obj == null) return null;
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

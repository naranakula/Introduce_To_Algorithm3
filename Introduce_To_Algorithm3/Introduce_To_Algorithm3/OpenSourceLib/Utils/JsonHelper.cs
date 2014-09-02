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
        private static JsonSerializerSettings jsonSettings;
        private static string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        static JsonHelper()
        {
            var dataTimeConverter = new IsoDateTimeConverter() {DateTimeFormat = dateTimeFormat};
            jsonSettings = new JsonSerializerSettings();
            jsonSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonSettings.Converters.Add(dataTimeConverter);
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
                return JsonConvert.SerializeObject(obj, Formatting.None, jsonSettings);
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
                return JsonConvert.DeserializeObject<T>(jsonString, jsonSettings);
            }
            catch (Exception e)
            {
                throw new JsonSerializationException(e.Message);
            }
        }

    }
}

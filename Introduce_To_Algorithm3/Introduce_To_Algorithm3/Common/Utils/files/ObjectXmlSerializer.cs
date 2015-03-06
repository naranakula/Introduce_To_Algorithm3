/**
 * author:  cmlu
 * date:    2015年1月7日
 * desc:    将对象和xml之间进行格式转换
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public static class ObjectXmlSerializer
    {
        /// <summary>
        /// 将对象转换为xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml<T>(T obj) where T:class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            MemoryStream mem = new MemoryStream();
            using (XmlTextWriter writer = new XmlTextWriter(mem, Encoding.UTF8))
            {
                serializer.Serialize(writer,obj);
            }

            return Encoding.UTF8.GetString(mem.ToArray());
        }

        /// <summary>
        /// 将xml解析为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToObject<T>(string str) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(str)),Encoding.UTF8))
            {
                return (T) serializer.Deserialize(reader);
            }
        }
    }
}

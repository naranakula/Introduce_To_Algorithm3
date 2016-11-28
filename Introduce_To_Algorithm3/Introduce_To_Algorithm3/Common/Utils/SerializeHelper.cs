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
    /// <summary>
    /// xml序列化帮助类 xml序列化不需要Serializable
    /// 使用这个
    /// </summary>
    public static class SerializeHelper
    {
        
        /// <summary>
        /// 将对象序列化到文件destFile中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="destFile">如果文件不存在，创建；否则覆盖</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static void ToFile<T>(T obj, string destFile,Action<Exception> exceptionHandler=null) where T : class
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;

                using (StreamWriter streamWriter = new StreamWriter(destFile, false, Encoding.UTF8))
                {
                    using (XmlWriter writer = XmlWriter.Create(streamWriter, settings))
                    {
                        serializer.Serialize(writer,obj,ns);
                    }
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
            

        }

        /// <summary>
        /// 将对象转换为xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns></returns>
        public static string ToXmlString<T>(T obj,Action<Exception> exceptionHandler) where T : class
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(String.Empty, string.Empty);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
                    {
                        serializer.Serialize(writer,obj,ns);
                    }

                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }

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
        /// 将xml字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static T ConvertStringToObject<T>(string xmlString, Action<Exception> exceptionHandler = null) where T:class 
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
                {
                    using (StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8))
                    {
                        return (T)serializer.Deserialize(reader);
                    }
                }
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
        /// 加载xml文件将其转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns></returns>
        public static T LoadFileToObject<T>(string fileName, Action<Exception> exceptionHandler = null) where T : class 
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8))
                {
                    return (T)serializer.Deserialize(reader);
                }
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

    }
}

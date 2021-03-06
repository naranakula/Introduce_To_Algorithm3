﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Com.Utility.Commons
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public class SerializeHelper
    {

        /// <summary>
        /// 将对象T序列化到文件destFile
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="destFile"></param>
        public static void SerializeToFile<T>(T obj, string destFile) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            //覆盖的含义是将原来的内容清除，重新写  已测
            using (StreamWriter streamWriter = new StreamWriter(destFile, false, Encoding.UTF8))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter, settings))
                {
                    serializer.Serialize(xmlWriter, obj, ns);
                }
            }

        }

        /// <summary>
        /// 将对象T序列化,并返回字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static string SerializeToString<T>(T obj) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
                {
                    serializer.Serialize(writer, obj, ns);
                }
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// 从文件中反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T DeserializeFromFile<T>(string filePath) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                return (T)serializer.Deserialize(reader);
            }
        }


        /// <summary>
        /// 从字符串中反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T DeserializeFromString<T>(string xmlString) where T : class
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

        /// <summary>
        /// 将对象转换为xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml<T>(T obj) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            MemoryStream mem = new MemoryStream();
            using (XmlWriter writer = XmlWriter.Create(mem, settings))
            {
                serializer.Serialize(writer, obj, ns);
            }

            return Encoding.UTF8.GetString(mem.ToArray());
        }


        /// <summary>
        /// 将对象序列化到destFile文件中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="destFile"></param>
        public static void ToFile<T>(T obj, String destFile) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            using (StreamWriter streamWriter = new StreamWriter(destFile, false, Encoding.UTF8))
            {
                using (XmlWriter writer = XmlWriter.Create(streamWriter, settings))
                {
                    serializer.Serialize(writer, obj, ns);
                }
            }
        }

        /// <summary>
        /// 将xml解析为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="isFile">true,表示str表示文件名；false,表示str表示要转换的xml字符串</param>
        /// <returns></returns>
        public static T ToObject<T>(string str, bool isFile = true) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (!isFile)
            {
                using (StreamReader reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(str)), Encoding.UTF8))
                {
                    return serializer.Deserialize(reader) as T;
                }
            }
            else
            {
                using (StreamReader reader = new StreamReader(str, Encoding.UTF8))
                {
                    return serializer.Deserialize(reader) as T;
                }
            }
        }
        /// <summary>
        /// 把对象序列化到xml文件中
        /// </summary>
        /// <param name="instance">要序列化的对象</param>
        /// <param name="xmlFile">xml文件,文件存在覆盖</param>
        public static void SerializeXml<T>(object instance,string xmlFile)
        {
            if(instance == null || !(instance is T))
            {
                return;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using(FileStream fs = new FileStream(xmlFile,FileMode.Create,FileAccess.Write))
            {
                //开始序列化对象
                serializer.Serialize(fs,instance);
            }
        }

        /// <summary>
        /// 将xml文件反序列化为对象
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="xmlFile">xml文件名</param>
        /// <returns></returns>
        public static T DeSerializeXml<T>(string xmlFile)
        {
            //创建xml序列化对象
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            //创建文件流
            using(FileStream fs = new FileStream(xmlFile,FileMode.Open,FileAccess.Read))
            {
                return (T) serializer.Deserialize(fs);
            }
        }

        /// <summary>
        /// 将对象序列化到soap格式的xml文件中
        /// 从 .NET Framework 3.5 版开始，SoapFormatter类已过时。应改用 BinaryFormatter。
        /// </summary>
        /// <param name="instance">要序列化的对象</param>
        /// <param name="xmlFile">xml文件名</param>
        public static void SerializeSoapXml(object instance,string xmlFile)
        {
            if(instance == null)
            {
                return;
            }

            //创建soap序列化对象
            SoapFormatter serializer = new SoapFormatter();
            //创建文件流
            using(FileStream fs = new FileStream(xmlFile,FileMode.Create,FileAccess.Write))
            {
                //开始序列化对象
                serializer.Serialize(fs,instance);
            }
        }

        /// <summary>
        /// 将Soap格式的文件反序列化为对象
        /// 从 .NET Framework 3.5 版开始，SoapFormatter类已过时。应改用 BinaryFormatter
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="xmlFile">xml文件名</param>
        /// <returns></returns>
        public static T DeSerializeSoapXml<T>(string xmlFile)
        {
            SoapFormatter serializer = new SoapFormatter();
            using(FileStream fs = new FileStream(xmlFile,FileMode.Open,FileAccess.Read))
            {
                return (T)serializer.Deserialize(fs);
            }
        }

        /// <summary>
        /// 将对象序列化到二进制文件中
        /// </summary>
        /// <param name="instance">要序列化的对象</param>
        /// <param name="fileName">文件名，保存二进制序列化数据，后缀一般为.bin</param>
        public static void SerializeBinary(object instance,string fileName)
        {
            if(instance == null)
            {
                return;
            }
            BinaryFormatter serializer = new BinaryFormatter();
            
            //创建文件流
            using(FileStream fs = new FileStream(fileName,FileMode.Create,FileAccess.Write))
            {
                serializer.Serialize(fs,instance);
            }
        }

        /// <summary>
        /// 将二进制文件反序列化对象
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="fileName">文件名，保存二进制序列化数据的位置</param>
        /// <returns></returns>
        public static T DeSerializeBinary<T>(string fileName)
        {
            //创建二进制序列化对象
            BinaryFormatter serializer = new BinaryFormatter();
            //创建文件流
            using(FileStream fs = new FileStream(fileName,FileMode.Open,FileAccess.Read))
            {
                return (T) serializer.Deserialize(fs);
            }
        }

        /// <summary>
        /// 把对象序列化到字节数组中
        /// </summary>
        /// <param name="instance">要序列化的对象</param>
        /// <returns></returns>
        public static byte[] SerilizeBytes(object instance)
        {
            if (instance == null)
            {
                return null;
            }
            //序列化的字节数组
            byte[] buffer;
            //创建二进制序列化对象
            BinaryFormatter serializer = new BinaryFormatter();
            //创建一个内存流
            using(MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms,instance);
                //重置内存流的当前位置为开始位置
                ms.Seek(0, SeekOrigin.Begin);
                //初始化buffer,并从内存流中读取数据
                buffer = new byte[ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// 把字节数组反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T DeSerializeBytes<T>(byte[] buffer)
        {
            if(buffer == null || buffer.Length == 0)
            {
                //返回T的默认值，值类型为0，类类型为null
                return default(T);
            }
            //创建二进制序列化对象
            BinaryFormatter serializer = new BinaryFormatter();
            
            //创建一个内存流
            using(MemoryStream ms = new MemoryStream())
            {
                //写缓冲区数据到内存流
                ms.Write(buffer, 0, buffer.Length);
                //重置内存流的位置 ，与 ms.Seek(0, SeekOrigin.Begin);方法的效果一直
                ms.Position = 0;
                return (T)serializer.Deserialize(ms);
            }
        }

        /// <summary>
        /// 对象序列化成byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// byte[]序列化成对象
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        public static object BytesToObject(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }
    }
}

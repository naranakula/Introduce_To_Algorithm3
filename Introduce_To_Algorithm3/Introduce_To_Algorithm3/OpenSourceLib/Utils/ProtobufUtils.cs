//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ProtoBuf;

//namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
//{
//    /// <summary>
//    /// Protobuf 是google的一种数据交换格式。支持数据的序列化和反序列化。支持不同平台的通信。
//    /// 效率和兼容性优异，二进制格式,平台独立，可拓展
//    /// Protobuf is equally comfortable serializing fields and properties, and it can serialize both public and private fields and properties 。 but the must have  [ProtoMember(N)] attribute
//    /// </summary>
//    public static class ProtobufUtils<T> where T:class
//    {
//        /// <summary>
//        /// 序列化信息到文件中
//        /// </summary>
//        /// <param name="obj"></param>
//        /// <param name="file"></param>
//        public static void Serialize(T obj, string file)
//        {
//            //如果指定的文件不存在，则创建该文件；如果存在并且不是只读的，则将覆盖其内容。
//            using (var stream = File.Create(file))
//            {
//                Serializer.Serialize(stream,obj);
//            }
//        }

//        /// <summary>
//        /// 序列化为字节数组
//        /// </summary>
//        /// <param name="obj"></param>
//        /// <returns></returns>
//        public static byte[] Serialize(T obj)
//        {
//            //序列化的字节数组
//            byte[] buffer = null;

//            using (MemoryStream stream = new MemoryStream())
//            {
//                Serializer.Serialize(stream,obj);
//                stream.Seek(0, SeekOrigin.Begin);
//                //初始化buffer，并从内存流中读取数据
//                buffer = new byte[stream.Length];
//                stream.Read(buffer, 0, buffer.Length);
//            }

//            return buffer;
//        }

//        /// <summary>
//        /// 反序列化
//        /// </summary>
//        /// <param name="file"></param>
//        /// <returns></returns>
//        public static T Deserialize(string file)
//        {
//            T obj = null;
//            using (var stream = File.OpenRead(file))
//            {
//                obj = Serializer.Deserialize<T>(stream);
//            }

//            return obj;
//        }

//        /// <summary>
//        /// 反序列化
//        /// </summary>
//        /// <param name="file"></param>
//        /// <returns></returns>
//        public static T Deserialize(byte[] buffer)
//        {
//            //创建内存流
//            using (MemoryStream ms = new MemoryStream())
//            {
//                //写缓冲区数据到内存流
//                ms.Write(buffer, 0, buffer.Length);
//                //重置内存流的位置 ，与 ms.Seek(0, SeekOrigin.Begin);方法的效果一直
//                ms.Position = 0;
//                return Serializer.Deserialize<T>(ms);
//            }

//        }
//    }

//    [ProtoContract]
//    public class Address
//    {
//        [ProtoMember(1)]
//        public string Line1 { get; set; }

//        [ProtoMember(2)]
//        public string Line2 { get; set; }
//    }


//    /// <summary>
//    /// the member name are not encoded in the proto. instead you must picl an unique integer to identify each member.
//    /// the number must positive integers.
//    /// the number must be unique within a single type. but the same numbers can be re-used in sub-types if inheritance is enabled 
//    /// lower number take less space
//    /// the identifier is important.you can change the member-name, or shift it between a property and a field, but changing the identifier changes the data 
//    /// 
//    /// 
//    /// 
//    /// 支持的数据类型
//    /// 有protocontract标记的定制类，该类必须有默认的构造函数
//    /// common primitive types
//    /// single dimension arrays T[]
//    /// List<T> / IList<T> 
//    ///Dictionary<TKey,TValue> / IDictionary<TKey,TValue> 
//    ///any type which implements IEnumerable<T> and has an Add(T) method 
//    ///The code assumes that types will be mutable around the elected members. Accordingly, custom structs are not supported, since they should be immutable. 
//    /// </summary>
//    [ProtoContract]
//    public class Person
//    {
//        [ProtoMember(1)]
//        public int Id { get; set; }

//        [ProtoMember(2)]
//        public string Name { get; set; }

//        [ProtoMember(3)]
//        public Address Address { get; set; }
//    }

//}

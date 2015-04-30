using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// Protobuf 是google的一种数据交换格式。支持数据的序列化和反序列化。支持不同平台的通信。
    /// 效率和兼容性优异，二进制格式,平台独立，可拓展
    /// </summary>
    public static class ProtobufUtils
    {
    }

    [ProtoContract]
    public class Address
    {
        [ProtoMember(1)]
        public string Line1 { get; set; }

        [ProtoMember(2)]
        public string Line2 { get; set; }
    }


    /// <summary>
    /// the member name are not encoded in the proto. instead you must picl an unique integer to identify each member.
    /// the number must positive integers.
    /// the number must be unique within a single type. but the same numbers can be re-used in sub-types if inheritance is enabled 
    /// lower number take less space
    /// the identifier is important.you can change the member-name, or shift it between a property and a field, but changing the identifier changes the data 
    /// 
    /// 
    /// 
    /// 支持的数据类型
    /// 有protocontract标记的定制类，该类必须有默认的构造函数
    /// common primitive types
    /// single dimension arrays T[]
    /// List<T> / IList<T> 
    ///Dictionary<TKey,TValue> / IDictionary<TKey,TValue> 
    ///any type which implements IEnumerable<T> and has an Add(T) method 
    ///The code assumes that types will be mutable around the elected members. Accordingly, custom structs are not supported, since they should be immutable. 
    /// </summary>
    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public Address Address { get; set; }
    }

}

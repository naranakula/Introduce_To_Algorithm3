using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cmlu.examples.pbs;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace Introduce_To_Algorithm3.OpenSourceLib.grpcs.protos
{
    /// <summary>
    /// pb帮助类
    /// 序列化 反序列化支持
    /// </summary>
    public static class PbHelper
    {
        public static void TestMain(string[] args)
        {
            Jack jack = new Jack();
            if (jack.Person == null)
            {
                //将会进入
                //对于C#默认的组合field是null
                Console.WriteLine("default value for combined message field is null");
            }
            
            //ToString()是用于测试的代码，很慢
            //{}
            string str = jack.ToString();
            Person person = new Person();
            person.Id = 1;
            person.Email = "";
            person.Name = "jack";
            //Phones已经实例化
            // One point to highlight is that any properties corresponding to repeated fields are read-only. 
            //repeated的Phones是只读的
            //The collection type for repeated fields is always RepeatedField<T>. 
            person.Phones.Add(new Person.Types.PhoneNumber { Number = "555-1212" });
            //{ "name": "jack", "id": 1, "phones": [ { "number": "555-1212" } ] }
            str = person.ToString();

            #region TimeStamp

            //传入的参数必须是Utc时间
            person.CreateTime = Timestamp.FromDateTime(DateTime.UtcNow);

            //返回的时间一定是utc时间
            DateTime createTime = person.CreateTime.ToDateTime();

            //{ "name": "jack", "id": 1, "createTime": "2018-06-09T01:31:37.489723400Z", "phones": [ { "number": "555-1212" } ] }
            str = person.ToString();


            //Google.Protobuf.WellKnownTypes.Timestamp有两部分组成:1、自 UTC time since Unix epoch 1970-01-01T00:00:00Z.的秒数  2、Negative second values with fractions must still have non-negative nanos values that count forward in time. Must be from 0 to 999,999,999 inclusive.


            #endregion


            #region 将对象序列化

            byte[] bytes = null;
            //方式1
            using (MemoryStream stream = new MemoryStream())
            {
                //写入流中
                person.WriteTo(stream);
                bytes = stream.ToArray();
            }

            //转换为字节
            //方式2
            byte[] byte2 = person.ToByteArray();
            //转换为字节
            //方式3
            ByteString byteString = person.ToByteString();
            byte2 = byteString.ToByteArray();
            #endregion

            #region 将字节反序列化

            person = Person.Parser.ParseFrom(bytes);
            person = Person.Parser.ParseFrom(byteString);

            #endregion

            #region 反射

            var descriptor = Person.Descriptor;

            string tab = "\t";
            foreach (var field in descriptor.Fields.InDeclarationOrder())
            {

                //获取类的字段，并从实例获取字段值
                Console.WriteLine($"{field.FieldNumber}{tab}{field.Name}{tab}{field.Accessor.GetValue(person)}");
            }
            
            #endregion

        }
    }
}

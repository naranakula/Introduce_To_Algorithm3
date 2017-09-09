using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cmlu.examples.pbs;
using Google.Protobuf;

namespace Introduce_To_Algorithm3.OpenSourceLib.grpcs.protos
{
    /// <summary>
    /// pb帮助类
    /// </summary>
    public static class PbHelper
    {
        public static void TestMain(string[] args)
        {
            #region 将对象序列化
            Person person = new Person();
            person.Id = 1;
            person.Email = "";
            person.Name = "jack";
            //Phones已经实例化
            // One point to highlight is that any properties corresponding to repeated fields are read-only. 
            //repeated的Phones是只读的
            //The collection type for repeated fields is always RepeatedField<T>. 
            person.Phones.Add(new Person.Types.PhoneNumber { Number = "555-1212" });

            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                //写入流中
                person.WriteTo(stream);
                bytes = stream.ToArray();
            }

            //转换为字节
            byte[] byte2 = person.ToByteArray();
            //转换为字节
            ByteString byteString = person.ToByteString();

            #endregion

            #region 将字节反序列化

            person = Person.Parser.ParseFrom(bytes);
            person = Person.Parser.ParseFrom(byteString);

            #endregion
        }
    }
}

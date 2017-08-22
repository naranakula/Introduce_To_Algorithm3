using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.files
{
    /// <summary>
    /// 读写内存
    /// </summary>
    public static class MemoryStreamUtils
    {

        /// <summary>
        /// 向memorystream写数据，并获取写的内容的例子
        /// </summary>
        /// <param name="writeAction"></param>
        /// <returns></returns>
        public static byte[] WriteBytes(Action<MemoryStream> writeAction)
        {
            byte[] result = null;
            using (MemoryStream stream = new MemoryStream())
            {
                //创建一个 canRead canWrite canSeek 容量自动拓展的内存流

                //写操作自动拓展内存
                //stream.WriteByte(b);//写一个字节
                //stream.Write(buffer,offset,count);//写多个字节

                if (writeAction != null)
                {
                    writeAction(stream);
                }

                //返回内存流全部内容数据(不是底层buffer,仅仅数据)的副本 忽略position
                result = stream.ToArray();
            }
            

            return result;
        }

    }
}

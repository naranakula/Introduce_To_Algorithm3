using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// xml序列化帮助类
    /// </summary>
    public static class SerializeHelper
    {

        /// <summary>
        /// 将对象序列化到文件destFile中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="destFile">如果文件不存在，创建；否则覆盖</param>
        public static void ToFile<T>(T obj, string destFile) where T : class
        {
            
        }



    }
}

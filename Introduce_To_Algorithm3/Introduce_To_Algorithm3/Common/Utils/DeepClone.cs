using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 深复制
    /// </summary>
    public class DeepClone
    {
        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target">如果为null，则新建对象，否则使用传入的对象</param>
        /// <returns></returns>
        public static T Clone<T>(T source,T target = null) where T : class, new()
        {
            T copy = null;
            if (target == null)
            {
                copy = new T();
            }
            else
            {
                copy = target;
            }

            //公有实例属性, 不包含静态属性
            PropertyInfo[] propertyInfos = typeof (T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo infoItem in propertyInfos)
            {
                MethodInfo[] methodInfos = infoItem.GetAccessors();
                //可读可写并且非virtual
                if (infoItem.CanWrite && infoItem.CanRead && !methodInfos[0].IsVirtual)
                {
                    object srcValue = infoItem.GetValue(source);

                    infoItem.SetValue(copy, srcValue);
                }
            }

            return copy;
        }
    }
}

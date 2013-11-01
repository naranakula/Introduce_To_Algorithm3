using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public class ReflectionHelper
    {
        #region 加载程序集

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称,可以加也可以不加程序集的后缀，如.dll</param>        
        public static Assembly LoadAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assemblyFile">程序集名称,可以加也可以不加程序集的后缀，如.dll</param>        
        public static Assembly LoadFrom(string assemblyFile)
        {
            try
            {
                return Assembly.LoadFrom(assemblyFile);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 获取程序集的类型

        /// <summary>
        /// 获取本地程序集的类型
        /// </summary>
        /// <param name="typeName">类型名称，应是全限定类型名,区分大小写</param>
        /// <returns></returns>
        public static Type GetType(string typeName)
        {
            try
            {
                return Type.GetType(typeName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static Type GetType(
            string typeName,
            bool throwOnError,
            bool ignoreCase
            )
        {
            return Type.GetType(typeName, throwOnError, ignoreCase);
        }

        /// <summary>
        /// 获取指定程序集中的类型
        /// </summary>
        /// <param name="assembly">指定的程序集</param>
        /// <param name="typeName">全限定类型名</param>
        /// <returns></returns>
        public static Type GetType(Assembly assembly, string typeName)
        {
            try
            {
                return assembly.GetType(typeName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 动态创建对象实例

        /// <summary>
        /// 创建类型的实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="parameters">传递给构造函数的参数,与要调用构造函数的参数数量、顺序和类型匹配的参数数组。如果 args 为空数组或 null引用，则调用不带任何参数的构造函数（默认构造函数）。</param>
        /// <returns></returns>
        public static object CreateInstance(Type type, params object[] parameters)
        {
            try
            {
                return Activator.CreateInstance(type, parameters);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 创建类的实例
        /// </summary>
        /// <param name="className">全限定类型名</param>
        /// <param name="parameters">传递给构造函数的参数，与要调用构造函数的参数数量、顺序和类型匹配的参数数组。如果为null调用默认构造函数</param>
        /// <returns></returns>
        public static object CreateInstance(string className, params object[] parameters)
        {
            try
            {
                //获取类型
                Type type = GetType(className);
                //类型为空则返回
                if (type == null)
                {
                    return null;
                }
                return CreateInstance(type, parameters);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 创建类的实例
        /// </summary>
        /// <typeparam name="T">要转换的类型名</typeparam>
        /// <param name="className">全限定类型名</param>
        /// <param name="parameters">传递给构造函数的参数</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string className, params object[] parameters)
        {
            try
            {
                return (T)CreateInstance(className, parameters);
            }
            catch (Exception)
            {
                //表示T类型的默认值：值类型为0，类类型为null
                return default(T);
            }
        }

        #endregion

        #region 获取类的命名空间

        /// <summary>
        /// 获取类T的命令名空间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetNamespace<T>()
        {
            return typeof(T).Namespace;
        }

        #endregion

        #region 调用方法和设置属性值
        //可以使用dynamic来实现
        #endregion
    }
}

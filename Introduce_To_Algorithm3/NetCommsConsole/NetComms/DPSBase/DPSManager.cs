using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using NLog.LayoutRenderers.Wrappers;

namespace NetCommsConsole.NetComms.DPSBase
{
    /// <summary>
    /// Automatically detects and manages the use of DataSerializer and DataProcessor
    /// Any DataSerializer or DataProcessor in an assembly located in the working directory (including subdirectories) will be automatically detected.
    /// </summary>
    public sealed class DPSManager
    {
        #region Comparers

        /// <summary>
        /// 类型比较
        /// </summary>
        private class ReflectedTypeComparer : IEqualityComparer<Type>
        {
            /// <summary>
            /// 实例
            /// </summary>
            public static ReflectedTypeComparer Instance { get; private set; }

            static ReflectedTypeComparer()
            {
                Instance = new ReflectedTypeComparer();
            }

            /// <summary>
            /// 构造函数
            /// </summary>
            public ReflectedTypeComparer() { }

            #region IEqualityComparer接口

            public bool Equals(Type x, Type y)
            {
                return x.AssemblyQualifiedName == y.AssemblyQualifiedName;
            }

            public int GetHashCode(Type obj)
            {
                return obj.AssemblyQualifiedName.GetHashCode();
            }
            #endregion
        }

        /// <summary>
        /// 程序集比较器
        /// </summary>
        private class AssemblyComparer : IEqualityComparer<AssemblyName>
        {
            /// <summary>
            /// 程序集比较器
            /// </summary>
            public static AssemblyComparer Instance { get; private set; }

            static AssemblyComparer()
            {
                Instance = new AssemblyComparer();
            }

            public AssemblyComparer()
            {
                
            }


            #region IEqualityComparer接口
            public bool Equals(AssemblyName x, AssemblyName y)
            {
                return x.FullName == y.FullName;
            }

            public int GetHashCode(AssemblyName obj)
            {
                return obj.FullName.GetHashCode();
            }
            #endregion
        }

        #endregion

        /// <summary>
        /// 要加载的Assembly
        /// </summary>
        private Dictionary<string,bool> AssembliesToLoad = new Dictionary<string, bool>(); 
        private Dictionary<string,DataSerializer> SerializersByType = new Dictionary<string, DataSerializer>(); 
        private Dictionary<string,DataProcessor> DataProcessorsByType = new Dictionary<string, DataProcessor>(); 
        private Dictionary<byte,string> DataSerializerIdToType = new Dictionary<byte, string>();
        private Dictionary<byte,string> DataProcessorIdToType = new Dictionary<byte, string>(); 
        
        private object addRemoveObjectLocker = new object();
        private ManualResetEvent loadCompleted = new ManualResetEvent(false);

        #region 单例
        private static object instance = null;
        private static object singletonLocker = new object();

        /// <summary>
        /// 单例
        /// </summary>
        private static DPSManager Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance as DPSManager;
                }

                if (instance == null)
                {
                    lock (singletonLocker)
                    {
                        if (instance == null)
                        {
                            instance = new DPSManager();
                        }
                    }
                }

                return instance as DPSManager;
            }
        }
        #endregion

        #region Public Methods


        public static DataSerializer GetDataSerializer<T>() where T : DataSerializer
        {
            if (!Instance.SerializersByType.ContainsKey(typeof (T).AssemblyQualifiedName))
            {
                //Make the serializer
                var serializer = CreateObjectWithParameterlessCtor(typeof (T).AssemblyQualifiedName) as DataSerializer;

                
            }


            throw new NotImplementedException();
        }

        #endregion

#region 辅助方法

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static object CreateObjectWithParameterlessCtor(string typeName)
        {
            var typeToCreate = Type.GetType(typeName);
            var constructor = typeToCreate.GetConstructor(BindingFlags.Instance, null, new Type[] {}, null);
            if (constructor == null)
            {
                constructor = typeToCreate.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { }, null);
            }

            return constructor.Invoke(null);
        }

#endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetCommsConsole.NetComms.DPSBase
{
    /// <summary>
    /// Provides methos that convert an object into a byte[]
    /// </summary>
    public abstract class DataSerializer
    {
        /// <summary>
        /// 缓存的类型标识符
        /// </summary>
        private static Dictionary<Type,byte> cachedIdentifiers = new Dictionary<Type, byte>(); 

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// implemented as singleton.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static T GetInstance<T>() where T : DataSerializer
        {
            T instance = DPSManager.GetDataSerializer<T>() as T;

            if (instance == null)
            {
                //if the instance is null the type was not added as part of composition
                //create a new instance of T and it to helper as a serializer
                var construct = typeof (T).GetConstructor(new Type[] {});
                if (construct == null)
                {
                    construct = typeof (T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0],
                        null);
                }

                if (construct == null)
                {
                    throw new Exception("the serializer must have a default ");
                }


                instance = construct.Invoke(new object[] {}) as T;

                DPSManager.AddDataSerializer(instance);
            }

            return instance; 
        }





    }
}

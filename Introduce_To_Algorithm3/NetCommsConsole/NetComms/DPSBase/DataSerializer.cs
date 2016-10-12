using System;
using System.Collections.Generic;
using System.Linq;
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
            throw  new NotImplementedException();
            
        }

    }
}

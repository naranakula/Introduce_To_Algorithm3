using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms.DPSBase
{
    /// <summary>
    /// Provides methods that process data in a <see cref="System.IO.Stream"/> into another <see cref="System.IO.Stream"/>.  Can be used to provide features such as data compression or encryption
    /// </summary>
    public abstract class DataProcessor
    {
        private static Dictionary<Type,byte> cachedIdentifiers = new Dictionary<Type, byte>(); 
        private static Dictionary<Type,bool> cachedIsSecurity = new Dictionary<Type, bool>();
        private static object locker = new object();

        /// <summary>
        /// Helper methods to allow DataProcessor implemented as a singleton.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("Instances of singleton DataProcessors should be accessed via the DPSManager")]
        protected static T GetInstance<T>() where T : DataProcessor
        {
            throw  new NotImplementedException();
        }

    }
}

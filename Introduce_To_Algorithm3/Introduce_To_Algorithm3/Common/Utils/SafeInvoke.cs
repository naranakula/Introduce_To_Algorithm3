using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 安全调用
    /// </summary>
    public static class SafeInvoke
    {
        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="action"></param>
        public static void Safe(this Action action)
        {
            try
            {
                action();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        public static void Safe<T>(this Action<T> action,T obj)
        {
            try
            {
                action(obj);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="func"></param>
        public static TR Safe<TR>(this Func<TR> func)
        {
            try
            {
                return func();
            }
            catch 
            {
                return default(TR);
            }
        }

        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="func"></param>
        /// <param name="obj"></param>
        public static TR Safe<TR,TP>(this Func<TP,TR> func,TP obj)
        {
            try
            {
                return func(obj);
            }
            catch 
            {
                return default(TR);
            }
        }
    }
}

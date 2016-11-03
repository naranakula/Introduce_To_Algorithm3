using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
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
        /// <param name="exceptionHandler"></param>
        public static void Safe(this Action action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
        }

        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        /// <param name="exceptionHandler"></param>
        public static void Safe<T>(this Action<T> action, T obj, Action<Exception> exceptionHandler = null)
        {
            try
            {
                action(obj);
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
        }

        /// <summary>
        /// 安全调用 调用失败，返回default(TR)
        /// </summary>
        /// <param name="func"></param>
        /// <param name="exceptionHandler"></param>
        public static TR Safe<TR>(this Func<TR> func, Action<Exception> exceptionHandler = null)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
                return default(TR);
            }
        }

        /// <summary>
        /// 安全调用 调用失败，返回default(TR)
        /// </summary>
        /// <param name="func"></param>
        /// <param name="obj"></param>
        /// <param name="exceptionHandler"></param>
        public static TR Safe<TR, TP>(this Func<TP, TR> func, TP obj, Action<Exception> exceptionHandler = null)
        {
            try
            {
                return func(obj);
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
                return default(TR);
            }
        }
    }
}

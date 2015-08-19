using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Models;

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

        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="action"></param>
        public static OperationResult<bool> Invoke(this Action action)
        {
            try
            {
                action();
                return new OperationResult<bool>(){Result = true,ResultType = OperationResultType.Success};
            }
            catch(Exception ex)
            {
                return new OperationResult<bool>() { Result = false, ResultType = OperationResultType.Error,ResultException = ex};
            }
        }

        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        public static OperationResult<bool> Invoke<T>(this Action<T> action, T obj)
        {
            try
            {
                action(obj);
                return new OperationResult<bool>() { Result = true, ResultType = OperationResultType.Success };
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>() { Result = false, ResultType = OperationResultType.Error, ResultException = ex };
            }
        }

        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="func"></param>
        public static OperationResult<TR> Invoke<TR>(this Func<TR> func)
        {
            try
            {
                TR tr = func();
                return new OperationResult<TR>(){Result = tr,ResultType = OperationResultType.Success};
            }
            catch(Exception ex)
            {
                return new OperationResult<TR>(){Result = default(TR),ResultType = OperationResultType.Error,ResultException = ex};
            }
        }

        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="func"></param>
        /// <param name="obj"></param>
        public static OperationResult<TR> Invoke<TR, TP>(this Func<TP, TR> func, TP obj)
        {
            try
            {
                TR tr = func(obj);
                return new OperationResult<TR>() { Result = tr, ResultType = OperationResultType.Success };
            }
            catch(Exception ex)
            {
                return new OperationResult<TR>() { Result = default(TR), ResultType = OperationResultType.Error, ResultException = ex };
            }
        }
    }
}

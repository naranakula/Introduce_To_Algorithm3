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
        /// <param name="exceptionHandler"></param>
        /// <param name="finallyHandler">最终处理</param>
        public static void Safe(this Action action,Action<Exception> exceptionHandler = null,Action finallyHandler = null)
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
            finally
            {
                finallyHandler?.Invoke();
            }
        }

        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        /// <param name="exceptionHandler"></param>
        public static void Safe<T>(this Action<T> action,T obj, Action<Exception> exceptionHandler = null)
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
        /// <param name="finallyHandler"></param>
        public static TR Safe<TR>(this Func<TR> func, Action<Exception> exceptionHandler = null,Action finallyHandler = null)
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
            finally
            {
                finallyHandler?.Invoke();
            }
        }

        /// <summary>
        /// 安全调用 调用失败，返回default(TR)
        /// </summary>
        /// <param name="func"></param>
        /// <param name="obj"></param>
        /// <param name="exceptionHandler"></param>
        public static TR Safe<TR,TP>(this Func<TP,TR> func,TP obj, Action<Exception> exceptionHandler = null)
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

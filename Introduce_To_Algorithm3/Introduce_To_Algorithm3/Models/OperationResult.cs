using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Models
{
    /// <summary>
    /// 操作结果类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OperationResult<T>
    {
        /// <summary>
        /// 实际的操作结果
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        ///  操作结果类型，标志是否操作成功或者失败
        /// </summary>
        public OperationResultType ResultType { get; set; }

        /// <summary>
        /// 操作异常时的异常
        /// </summary>
        public Exception ResultException { get; set; }
    }

    /// <summary>
    /// 操作结果类型，如是否操作成功或者失败
    /// </summary>
    public enum OperationResultType
    {
        /// <summary>
        /// 发生错位引起的失败
        /// </summary>
        Error,

        /// <summary>
        /// 成功，唯一标识成功的，其它均为失败
        /// </summary>
        Success,
    }
}

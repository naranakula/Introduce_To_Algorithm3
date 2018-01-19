using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Models
{


    /// <summary>
    /// 通用异常
    /// </summary>
    public class CommonException:Exception
    {
        /// <summary>
        /// 异常编码
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// 异常描述
        /// </summary>
        public string ErrorReason { get; private set; }

        /// <summary>
        /// 内部异常
        /// </summary>
        public Exception TheInnerException { get; set; }

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CommonException()
        {
            ErrorCode = 0;
            ErrorReason = string.Empty;
            TheInnerException = null;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode">异常编码</param>
        /// <param name="errorReason">异常原因</param>
        public CommonException(int errorCode, string errorReason):base($"errorCode={errorCode},errorReason={errorReason}")//s=null $"s"=空字符串
        {
            ErrorCode = errorCode;
            ErrorReason = errorReason;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode">异常编码</param>
        /// <param name="errorReason">异常描述</param>
        /// <param name="innerException">内部异常</param>
        public CommonException(int errorCode, string errorReason,Exception innerException) : base($"errorCode={errorCode},errorReason={errorReason}")//s=null $"s"=空字符串
        {
            ErrorCode = errorCode;
            ErrorReason = errorReason;
            TheInnerException = innerException;
        }


        #endregion
        
        /// <summary>
        /// 覆盖ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string reason = ErrorReason ?? string.Empty;
            return $"error code={ErrorCode},error reason={reason}";
        }
    }


}

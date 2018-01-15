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
        public int ExceptionCode { get; private set; }

        /// <summary>
        /// 异常描述
        /// </summary>
        public string ExceptionDesc { get; private set; }

        /// <summary>
        /// 内部异常
        /// </summary>
        public Exception TheInnerException { get; private set; }

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CommonException()
        {
            ExceptionCode = 0;
            ExceptionDesc = string.Empty;
            TheInnerException = null;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exceptionCode">异常编码</param>
        /// <param name="exceptionDesc">异常描述</param>
        public CommonException(int exceptionCode, string exceptionDesc)
        {
            ExceptionCode = exceptionCode;
            ExceptionDesc = exceptionDesc;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exceptionCode">异常编码</param>
        /// <param name="exceptionDesc">异常描述</param>
        /// <param name="innerException">内部异常</param>
        public CommonException(int exceptionCode, string exceptionDesc,Exception innerException)
        {
            ExceptionCode = exceptionCode;
            ExceptionDesc = exceptionDesc;
            TheInnerException = innerException;
        }


        #endregion


        /// <summary>
        /// 覆盖ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string desc = ExceptionDesc ?? string.Empty;
            return $"code={ExceptionCode},desc={desc}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Models
{


    /// <summary>
    /// 通用的操作结果
    /// 
    /// ResultCode结果码 0表示正常
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonResult<T>
    {
        #region 构造函数

        public CommonResult()
        {
            
        }


        public CommonResult(int resultCode, T result)
        {
            this.ResultCode = resultCode;
            this.Result = result;
        }


        public CommonResult(int resultCode, T result, Exception exception)
        {
            this.ResultCode = resultCode;
            this.Result = result;
            this.ResultException = exception;
        }

        public CommonResult(int resultCode, T result, Exception exception,string description)
        {
            this.ResultCode = resultCode;
            this.Result = result;
            this.ResultException = exception;
            this.ResultDesc = description;
        }


        #endregion

        /// <summary>
        /// 实际的结果
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// 结果码
        /// 通常0表示正常
        /// </summary>
        public int ResultCode { get; set; }

        /// <summary>
        /// 对结果的一个描述
        /// </summary>
        public string ResultDesc { get; set; }

        /// <summary>
        /// 结果失败时的异常信息
        /// </summary>
        public Exception ResultException { get; set; }

        /// <summary>
        /// 覆盖ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(ResultDesc))
            {
                return $"ResultCode={ResultCode}";
            }
            else
            {
                return $"ResultCode={ResultCode},ResultDesc={ResultDesc}";
            }
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Models
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public class ExceptionEx:Exception
    {
        /// <summary>
        /// 异常码
        /// </summary>
        public int ExceptionCode { get; set; }

        /// <summary>
        /// 异常描述
        /// </summary>
        public string ExceptionDesc { get; set; }

        /// <summary>
        /// 内部异常
        /// </summary>
        public Exception TheInnerException { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Models
{
    /// <summary>
    /// 通用模型
    /// </summary>
    public class CommonModel
    {
        /// <summary>
        /// 模型类型
        /// </summary>
        public string CommonModelType { get; set; }


        /// <summary>
        /// 模型对象
        /// </summary>
        public object CommonModelObject { get; set; }

    }

    /// <summary>
    /// 对象类型常量
    /// </summary>
    public class CommonModelType
    {
        public const string LogType = "log";
    }
}

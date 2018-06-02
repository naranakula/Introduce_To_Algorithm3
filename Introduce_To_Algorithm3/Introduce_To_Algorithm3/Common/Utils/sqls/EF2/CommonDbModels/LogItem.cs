using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbModels
{

    /// <summary>
    /// 日志项
    /// </summary>
    public class LogItem
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType { get; set; }


        /// <summary>
        /// 日志来源  如来自哪台机器或者哪个ip
        /// </summary>
        public string LogSource { get; set; }


        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

}

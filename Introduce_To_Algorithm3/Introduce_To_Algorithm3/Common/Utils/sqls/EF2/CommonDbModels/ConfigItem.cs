using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbModels
{
    /// <summary>
    /// 配置项
    /// </summary>
    public class ConfigItem
    {

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 配置项键
        /// </summary>
        public string ConfigKey { get; set; }


        /// <summary>
        /// 配置项值
        /// </summary>
        public string ConfigValue { get; set; }


        /// <summary>
        /// 空表示为所有配置
        /// 不为空表示为某个指定客户端配置
        /// 为谁做的配置
        /// </summary>
        public string ConfigFor { get; set; }


        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}

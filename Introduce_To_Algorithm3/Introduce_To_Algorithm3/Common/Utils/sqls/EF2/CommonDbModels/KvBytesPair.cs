using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbModels
{


    /// <summary>
    /// 键值对
    /// </summary>
    public class KvBytesPair
    {
        /// <summary>
        /// 键 最长长度128
        /// 主键 聚簇索引
        /// 大小写不敏感
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值  [varbinary](max) NULL 最长2^31-1 约2g
        /// </summary>
        public byte[] Value { get; set; }

        /// <summary>
        /// 更新时间  不会产生UTC问题,读取时全部转换为了本地时间
        /// 直接使用本地时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间  不会产生UTC问题,读取时全部转换为了本地时间
        /// 直接使用本地时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }



}

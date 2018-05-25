using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbModels
{


    /// <summary>
    /// 带类型字典表
    /// </summary>
    public class DictBytesItem
    {
        /// <summary>
        /// 键 最长长度128
        /// 主键 聚簇索引
        /// 大小写不敏感
        /// </summary>
        public string DictKey { get; set; }


        /// <summary>
        /// 字典类型 最长长度128
        /// 大小写不敏感
        /// </summary>
        public string DictType { get; set; }


        /// <summary>
        /// 值  nvarchar(max) 最长2^31-1 约2g
        /// </summary>
        public byte[] DictValue { get; set; }



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

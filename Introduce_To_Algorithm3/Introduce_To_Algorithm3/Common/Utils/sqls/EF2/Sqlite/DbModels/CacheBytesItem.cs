using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite.DbModels
{


    /// <summary>
    /// Cache项
    /// </summary>
    public class CacheBytesItem
    {
        /// <summary>
        /// 键 方法保证不为空或null
        /// SQLite变长记录，字段不需要指定长度。
        /// 长度不要超过128
        /// </summary>
        public string CacheKey { get; set; }


        /// <summary>
        /// cache类型，默认为空，表示不分类型
        /// 长度不要超过128
        /// </summary>
        public string CacheType { get; set; }


        /// <summary>
        /// 值 方法保证不为空或null
        /// SQLite变长记录，字段不需要指定长度
        /// </summary>
        public byte[] CacheValue { get; set; }

        /// <summary>
        /// Cache的过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }


        /// <summary>
        /// 更新时间  不会产生UTC问题,读取时全部转换为了本地时间
        /// 直接使用本地时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 首次 创建时间  不会产生UTC问题,读取时全部转换为了本地时间
        /// 直接使用本地时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }


}

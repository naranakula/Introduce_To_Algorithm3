using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbModels
{


    /// <summary>
    /// Cache项
    /// </summary>
    public class CacheItem
    {

        /// <summary>
        /// 缓存键  和 CacheType共同构成主键
        /// 大小写不敏感
        /// </summary>
        public string CacheKey { get; set; }

        /// <summary>
        /// 缓存类型
        /// 大小写不敏感
        /// </summary>
        public string CacheType { get; set; }


        /// <summary>
        /// 值
        /// 
        /// </summary>
        public string CacheValue { get; set; }

        /// <summary>
        /// Cache的过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }


        /// <summary>
        /// 更新时间  
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 首次 创建时间 
        /// </summary>
        public DateTime CreateTime { get; set; }

    }




}

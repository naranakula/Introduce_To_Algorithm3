using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite.DbModels;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite.DbMaps
{


    /// <summary>
    /// 数据库表映射
    /// </summary>
    public class CacheItemMap : EntityTypeConfiguration<CacheItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheItemMap()
        {
            ToTable(nameof(CacheItem)).HasKey(p => new { p.CacheKey, p.CacheType });

            /*
             CREATE TABLE "CacheItem" ([CacheKey] nvarchar (128) NOT NULL, [CacheType] nvarchar (128) NOT NULL, [CacheValue] nvarchar, [ExpireTime] datetime NOT NULL, [UpdateTime] datetime NOT NULL, [CreateTime] datetime NOT NULL, PRIMARY KEY(CacheKey, CacheType))
             
             */
        }
    }


}

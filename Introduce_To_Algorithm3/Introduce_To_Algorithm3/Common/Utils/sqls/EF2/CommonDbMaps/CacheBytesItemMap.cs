using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbModels;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbMaps
{



    /// <summary>
    /// 数据库映射
    /// </summary>
    public class CacheBytesItemMap : EntityTypeConfiguration<CacheBytesItem>
    {

        public CacheBytesItemMap()
        {
            ToTable(nameof(CacheBytesItem)).HasKey(p => new { p.CacheKey, p.CacheType });

            //主键
            Property(t => t.CacheKey).IsRequired()
                .IsUnicode()
                .HasMaxLength(128)
                .IsVariableLength();

            Property(t => t.CacheType).IsRequired()//所有的key组成属性必须not null
                .IsUnicode()
                .HasMaxLength(128)
                .IsVariableLength();

            Property(x => x.CacheValue)
                .IsOptional()
                .HasMaxLength(null)
                .IsVariableLength();
        }

    }





}

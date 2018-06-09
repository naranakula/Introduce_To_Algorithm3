using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
    public class ConfigItemMap:EntityTypeConfiguration<ConfigItem>
    {

        public ConfigItemMap()
        {

            ToTable(nameof(ConfigItem)).HasKey(p => p.Id);
            
            //变长 nvarchar(36)
            Property(x => x.Id)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(36)
                .IsVariableLength()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            //nvarchar(128)
            Property(t => t.ConfigKey).IsRequired().IsUnicode().HasMaxLength(128).IsVariableLength();


            //nvarchar(128)
            Property(t => t.ConfigFor).IsOptional().IsUnicode().HasMaxLength(128).IsVariableLength();


            //nvarchar(max)
            Property(x => x.ConfigValue)
                .IsOptional()
                .IsUnicode()
                .HasMaxLength(null)
                .IsVariableLength();
        }

    }
}

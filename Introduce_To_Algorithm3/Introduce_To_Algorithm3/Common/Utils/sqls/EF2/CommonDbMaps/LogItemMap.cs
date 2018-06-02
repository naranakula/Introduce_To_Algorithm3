using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.CommonDbMaps
{
    /// <summary>
    /// 数据库映射
    /// </summary>
    public class LogItemMap:EntityTypeConfiguration<LogItem>
    {
        public LogItemMap()
        {
            ToTable(nameof(LogItem)).HasKey(p => p.Id);
            //变长 nvarchar(36)
            Property(x => x.Id)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(36)
                .IsVariableLength()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //变长 nvarchar(64)
            Property(x => x.LogType)
                .IsOptional()
                .IsUnicode()
                .HasMaxLength(64)
                .IsVariableLength();

            //变长 nvarchar(128)
            Property(x => x.LogSource)
                .IsOptional()
                .IsUnicode()
                .HasMaxLength(128)
                .IsVariableLength();

            //nvarchar(max)
            Property(x => x.LogContent)
                .IsOptional()
                .IsUnicode()
                .HasMaxLength(null)
                .IsVariableLength();
        }
    }
}

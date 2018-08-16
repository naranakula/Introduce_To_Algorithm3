using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
    public class LogItemMap : EntityTypeConfiguration<LogItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LogItemMap()
        {
            ToTable(nameof(LogItem)).HasKey(p => p.Id);
            //变长 nvarchar(36)
            Property(x => x.Id)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(36)
                .IsVariableLength()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        }
    }



}

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
    /// 数据库映射
    /// </summary>
    public class LocalConfigMap : EntityTypeConfiguration<LocalConfig>
    {
        public LocalConfigMap()
        {
            ToTable(nameof(LocalConfig)).HasKey(t => t.ConfigKey);
            Property(t => t.ConfigKey).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }


}

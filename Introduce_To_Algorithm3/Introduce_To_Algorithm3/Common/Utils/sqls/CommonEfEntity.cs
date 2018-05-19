using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls
{
    /// <summary>
    /// 一个Entity的通用模板
    /// </summary>
    public class CommonEfEntity
    {
        /// <summary>
        /// id 最长36位id
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 数据版本号, Create时为0,每次update加1
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// 更新时间 创建时与CreateTime一致
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 数据库映射
    /// </summary>
    public class CommonEfEntityMap : EntityTypeConfiguration<CommonEfEntity>
    {
        public CommonEfEntityMap()
        {

            ToTable(nameof(CommonEfEntity)).HasKey(t => t.Id);
            //guid带-36位，不带32位
            Property(t => t.Id).IsRequired().HasMaxLength(36).IsUnicode().IsVariableLength().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }

}

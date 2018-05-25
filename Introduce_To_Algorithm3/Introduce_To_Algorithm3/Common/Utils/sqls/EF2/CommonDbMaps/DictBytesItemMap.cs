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
    /// 数据库表映射
    /// </summary>
    public class DictBytesItemMap : EntityTypeConfiguration<DictBytesItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DictBytesItemMap()
        {
            ToTable(nameof(DictBytesItem)).HasKey(t => new { t.DictKey, t.DictType });/*组合主键*/
            //主键
            Property(t => t.DictKey).IsRequired()
                .IsUnicode()
                .HasMaxLength(128)
                .IsVariableLength();

            Property(t => t.DictType).IsRequired()//所有的key组成属性必须not null
                .IsUnicode()
                .HasMaxLength(128)
                .IsVariableLength();

            Property(x => x.DictValue)
                .IsOptional()
                .HasMaxLength(null)
                .IsVariableLength();
            //底层的sql:CREATE TABLE "KvPair" ([Id] integer, [Key] nvarchar UNIQUE ON CONFLICT REPLACE, [Value] nvarchar, [UpdateTime] datetime NOT NULL, [CreateTime] datetime NOT NULL, PRIMARY KEY(Id))//sqlite默认integer主键自增
            //select * from sqlite_master
            //CREATE INDEX IX_Key ON "KvPair" (Key)
            //long 底层对应 INTEGER
            //double 底层对应 REAL
            //string 底层对应 TEXT
            //datetime 底层对应 NUMERIC  再底层对应 TEXT TEXT as ISO8601 strings Use the ISO-8601 format. Uses the "yyyy-MM-dd HH:mm:ss.FFFFFFFK" format for UTC DateTime values and "yyyy-MM-dd HH:mm:ss.FFFFFFF" format for local DateTime values). 
        }
    }






}

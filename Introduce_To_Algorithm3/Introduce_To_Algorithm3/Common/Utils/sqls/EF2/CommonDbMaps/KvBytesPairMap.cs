﻿using System;
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
    /// 键值对映射
    /// </summary>
    public class KvBytesPairMap : EntityTypeConfiguration<KvBytesPair>
    {
        public KvBytesPairMap()
        {
            ToTable(nameof(KvBytesPair)).HasKey(t => t.Key);

            //变长 nvarchar(128)
            Property(x => x.Key)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(128)
                .IsVariableLength()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //[varbinary](max) NULL
            Property(x => x.Value)
                .IsOptional()
                .HasMaxLength(null)
                .IsVariableLength();

        }
    }



}

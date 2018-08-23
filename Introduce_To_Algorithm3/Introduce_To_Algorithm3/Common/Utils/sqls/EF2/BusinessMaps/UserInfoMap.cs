using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2.BusinessModels;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.BusinessMaps
{
    /// <summary>
    /// 用户信息映射
    /// </summary>
    public class UserInfoMap : EntityTypeConfiguration<UserInfo>
    {
        public UserInfoMap()
        {
            ToTable(nameof(UserInfo)).HasKey(t => t.Id);

            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //帐号配置
            Property(t => t.AccountNo)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(128)
                .IsVariableLength();

            Property(t => t.NickName).IsRequired().IsUnicode().HasMaxLength(128).IsVariableLength();

            Property(t => t.Password).IsRequired().IsUnicode().HasMaxLength(128).IsVariableLength();

            Property(t => t.PasswordHash).IsRequired().IsUnicode(false).HasMaxLength(128).IsVariableLength();

            Property(t => t.CurrentLoginToken).IsOptional().IsUnicode(false).HasMaxLength(36).IsVariableLength();

            //设置索引  默认使用索引名IX_AccountNo，不唯一，非聚集
            //HasIndex(t => t.AccountNo);//默认使用索引名IX_AccountNo，不唯一，非聚集

            HasIndex(t => t.AccountNo).HasName("IX_AccountNo").IsClustered(false).IsUnique(true);


            /*

            设置索引
            //befores ef 6.2
            modelBuilder.Entity<Person>()
                .Property(e => e.Name)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute { IsUnique = true }));

            // after ef 6.2
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // multi column index
            modelBuilder.Entity<Person>()
                .HasIndex(p => new { p.Name, p.Firstname })
                .IsUnique();

          */
        }
    }
}

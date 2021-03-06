﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2
{
    /// <summary>
    /// 需要先创建用户
    /// 用户有默认的表空间，所有Code First创建的表都在该表空间中
    /// 
    /// 每个表都应该有三个字段:
    ///         string Id             主键
    ///         DateTime UpdateTime   数据最近一次更新时间  创建时等于创建时间
    ///         DateTime CreateTime   数据创建时间 
    /// 
    /// 
    /// </summary>
    public class OracleDbContext:DbContext
    {
        /// <summary>
        /// 给定字符串用作将连接到的数据库的名称或连接字符串
        /// name=ConnString格式
        /// </summary>
        private static string _nameOrConnectionString = "name=OracleConStr";


        public OracleDbContext()
            : base(_nameOrConnectionString)
        {

        }

        /// <summary>
        /// 覆盖父类方法，创建模型
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //这里指定的是用户名，注意大写
            modelBuilder.HasDefaultSchema("CMLU");

            modelBuilder.Configurations.Add(new PersonMap());
        }


        /// <summary>
        /// 测试
        /// </summary>
        public static void TestMain()
        {
            using (OracleDbContext context = new OracleDbContext())
            {
                Person person = new Person(){Name="Test",CreateTime = DateTime.Now};
                context.Set<Person>().Add(person);
                context.SaveChanges();
                int count = context.Set<Person>().Count();
                Console.WriteLine("共有"+count+"个数据");
            }
        }

    }


    public class Person
    {
        public Person()
        {
            this.Phones = new List<Phone>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual ICollection<Phone> Phones { get; set; } 

    }
    /// <summary>
    /// 电话
    /// </summary>
    public class Phone
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string PhoneNumber { get; set; }
        public virtual  Person Person { get; set; }
    }

    public class PhoneMap : EntityTypeConfiguration<Phone>
    {
        public PhoneMap()
        {
            ToTable("Phone").HasKey(t => t.Id);
            Property(t => t.PhoneNumber).HasColumnName("PhoneNumber").IsOptional().HasMaxLength(512);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            ToTable("Person").HasKey(t => t.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //without the given name and has no column order, clustering, or
            //     uniqueness specified. //索引默认是不唯一非聚集的， 使用默认的命名为IX_列名
            Property(t => t.Name).HasColumnName("Name").IsOptional().HasMaxLength(512)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,new IndexAnnotation(new IndexAttribute()));
            Property(t => t.CreateTime).IsOptional().HasColumnName("CreateTime").HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
        }
    }

}

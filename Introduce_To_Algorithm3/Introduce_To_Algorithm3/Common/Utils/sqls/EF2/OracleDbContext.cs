using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2
{
    /// <summary>
    /// 需要先创建用户
    /// 用户有默认的表空间，所有Code First创建的表都在该表空间中
    /// </summary>
    public class OracleDbContext:DbContext
    {
        public OracleDbContext()
            : base("name=OracleConStr")
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
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            ToTable("Person").HasKey(t => t.Id);
            Property(t => t.Name).HasColumnName("Name").IsOptional().HasMaxLength(512);
            Property(t => t.CreateTime).IsOptional().HasColumnName("CreateTime");
        }
    }

}

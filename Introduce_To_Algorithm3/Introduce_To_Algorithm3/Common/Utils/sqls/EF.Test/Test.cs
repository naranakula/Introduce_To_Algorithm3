
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF.Test
{
    class Test
    {
        public void test()
        {
            Database.SetInitializer(new Initializer());
            using (TestContext context = new TestContext())
            {
                Phone phone = context.Phone.Find(1);
                Console.WriteLine(phone);
            }
            Thread.Sleep(1000);

        }
    }

    public class Initializer : CreateDatabaseIfNotExists<TestContext>
    {
        public override void InitializeDatabase(TestContext context)
        {
            base.InitializeDatabase(context);
        }

        protected override void Seed(TestContext context)
        {
            //可以在数据库创建后运行的程序，如添加初始数据的代码
            Person person = new Person() { FirstName = "A", LastName = "N", MidName = "L" };
            person.Companies.Add(new Company() { CompanyName = "com" });
            person.Phones.Add(new Phone() { PhoneNumber = "1522121" });
            person.Student = new Student() { CollegeName = "zhejiang" };

            context.People.Add(person);

            context.SaveChanges();
        }
    }

    public class TestContext : DbContext
    {
        public TestContext()
            : base("name=ConnString")
        {

        }

        /// <summary>
        /// 数据库表的名称不是由People生成的
        /// </summary>
        public DbSet<Person> People { get; set; }


        public DbSet<Phone> Phone { get; set; }

        /// <summary>
        /// 创建数据库的第一个实例时仅调用此方法一次
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Person>()
            //    .Property(p => p.FirstName)
            //    .HasColumnName("FirstName")
            //    .HasMaxLength(30)
            //    .IsFixedLength()
            //    .IsUnicode(true).IsRequired();

            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new StudentMap());
        }
    }

    public class BaseMigrationConfiguration : DbMigrationsConfiguration<TestContext>
    {
        public BaseMigrationConfiguration()
        {
            //必须支持允许自动迁移，这样当数据库结构改变后就可以自动迁移了
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
            //Gets or sets the string used to distinguish migrations belonging to this configuration from migrations belonging to other configurations using the same database.
            ContextKey = "CmluMigrationConfiguration";
        }

        protected override void Seed(TestContext context)
        {
            Console.WriteLine("Seed called");
        }
    }

    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            ToTable("Person");
            Property(p => p.FirstName).HasColumnName("FirstName").HasMaxLength(30)/*.IsOptional().IsFixedLength()*/.IsVariableLength().IsUnicode(true).IsOptional();
            Ignore(p => p.MidName);
            Property(p => p.RowVersion).IsFixedLength().HasMaxLength(8).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed).IsRowVersion();
            HasMany(p => p.Phones).WithOptional(phone => phone.Person).HasForeignKey(ph => ph.PersonId)/*.WillCascadeOnDelete(true)*/;
            HasMany(p => p.Companies).WithMany(c => c.Persons).Map(m =>
            {
                m.MapLeftKey("PersonId");
                m.MapRightKey("CompanyId");
                m.ToTable("PersonCompany");
            });

        }
    }

    public class StudentMap : EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            HasKey(s => s.PersonId).HasRequired(s => s.Person).WithOptional(p => p.Student);
        }
    }

    public class Student
    {
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
        public string CollegeName { get; set; }
    }

    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }

    public class Person
    {
        public Person()
        {
            Phones = new List<Phone>();
            Companies = new List<Company>();
        }

        public int PersonId { get; set; }
        public string FirstName { get; set; }
        [MaxLength(30)]
        [Column(TypeName = "nvarchar")]
        public string LastName { get; set; }
        public string MidName { get; set; }
        public virtual Student Student { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

    }

    public class Phone
    {
        public Phone()
        {
        }

        public int PhoneId { get; set; }
        public string PhoneNumber { get; set; }
        public int? PersonId { get; set; }

        public virtual Person Person { get; set; }

    }
}

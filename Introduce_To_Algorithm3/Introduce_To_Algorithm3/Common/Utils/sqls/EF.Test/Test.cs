
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
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
            using (BaseContext context = new BaseContext())
            {
                Phone phone = context.Phone.Find(1);
                Console.WriteLine(phone);
            }
            Thread.Sleep(1000);

        }
    }

    public class Initializer : CreateDatabaseIfNotExists<BaseContext>
    {
        public override void InitializeDatabase(BaseContext context)
        {
            base.InitializeDatabase(context);
        }

        protected override void Seed(BaseContext context)
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

    public class BaseContext : DbContext
    {
        public BaseContext()
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

    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            ToTable("Person");
            Property(p => p.FirstName).HasColumnName("FirstName").HasMaxLength(30)/*.IsOptional().IsFixedLength()*/.IsVariableLength().IsUnicode(true).IsRequired();
            Ignore(p => p.MidName);
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

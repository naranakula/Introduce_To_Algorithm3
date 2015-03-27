using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace Introduce_To_Algorithm3.Common.Utils.sqls
{
    /// <summary>
    /// 
    /**
1.Code First includes types defined as a DbSet property in context class.
2.Code First includes reference types included in entity types even if they are defined in different assembly.
3.Code First includes derived classes even if only base class is defined as DbSet property.
The default convention for primary key is that Code First would create a primary key for a property if the property name is Id or <class name>Id (NOT case sensitive).
Code First infer relationship between the two entities using navigation property. This navigation property can be simaple reference type or collection type.
It is recommanded to include a foreign key property on the dependent end of a relationship. 
     * 
     * 
     * 
     * 
     * 
     */
    /// </summary>
    public class DbContextExtension<T>:DbContext where T:class
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// creates a database in your local SQLEXPRESS server with a name that matches your {Namespace}.{Context class name}.
        /// </summary>
        public DbContextExtension()
        {
            
        }

        /// <summary>
        /// Code First creates a database with the name you specified in base constructor in the local SQLEXPRESS database server
        /// </summary>
        /// <param name="dbName"></param>
        public DbContextExtension(string dbName) : base(dbName)
        {
            Database.SetInitializer<DbContextExtension<T>>(new CreateDatabaseIfNotExists<DbContextExtension<T>>());

            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseIfModelChanges<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseAlways<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new SchoolDBInitializer());
            //Disable initializer
            //Database.SetInitializer<SchoolDBContext>(null);


//            There are four different database Initialization strategies:
//1.CreateDatabaseIfNotExists: This is default initializer. As name suggests, it will create the database if none exists as per the configuration. However, if you change the model class and then run the application with this initializer, then it will throw an exception. 
//2.DropCreateDatabaseIfModelChanges: This initializer drops an existing database and creates a new database, if your model classes (entity classes) have been changed. So you don't have to worry about maintaining your database schema, when your model classes change. 
//3.DropCreateDatabaseAlways: As the name suggests, this initializer drops an existing database every time you run the application, irrespective of whether your model classes have changed or not. This will be useful, when you want fresh database, every time you run the application, while you are developing the application. 
//4.Custom DB Initializer: You can also create your own custom initializer, if any of the above don't satisfy your requirements or you want to do some other process that initializes the database using above initializer. 
            //EF4.3 介绍了和并，when your domain model changes without losing any existing data.it uses new db intializer called MigrateDatabaseToLastestVersion
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<SchoolDBContext, Configuration>("SchoolDBConnectionString"));

    //        internal sealed class Configuration : DbMigrationsConfiguration<SchoolDataLayer.SchoolDBContext>
    //{
    //    public Configuration()
    //    {
            //        AutomaticMigrationsEnabled = true; AutomaticMigrationDataLossAllowed = true ;
    //    }

    //    protected override void Seed(SchoolDataLayer.SchoolDBContext context)
    //    {
    //        //  This method will be called after migrating to the latest version.

    //        //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
    //        //  to avoid creating duplicate seed data. E.g.
    //        //
    //        //    context.People.AddOrUpdate(
    //        //      p => p.FullName,
    //        //      new Person { FullName = "Andrew Peters" },
    //        //      new Person { FullName = "Brice Lambson" },
    //        //      new Person { FullName = "Rowan Miller" }
    //        //    );
    //        //
    //    }
    //}
     
        }

        /// <summary>
        /// You can also define connection string in app.config or web.config and specify connection string name starting with "name=" in the base constructor of the context class.
        /// </summary>
        /// <param name="connectionString"></param>
        public DbContextExtension(StringBuilder connectionString)
            : base(string.Format("name={0}",connectionString))
        {

        }

        #endregion


        #region 异步


        /*
         
         
          
            private static async Task<Student> GetStudent()
            {
                Student student = null;

                using (var context = new SchoolDBEntities())
                {
                    Console.WriteLine("Start GetStudent...");
              
                    student = await (context.Students.Where(s => s.StudentID == 1).FirstOrDefaultAsync<Student>());
            
                    Console.WriteLine("Finished GetStudent...");
               
                }

                return student;
            } 
         
           
            private static async Task SaveStudent(Student editedStudent)
            {

                using (var context = new SchoolDBEntities())
                {
                    context.Entry(editedStudent).State = EntityState.Modified;
                    context.Database.Log = Console.Write;//EF 6 provides a simple mechanism to log everything that Entity Framework is doing.
          
         
                    Console.WriteLine("Start SaveStudent...");
                
                    int x = await (context.SaveChangesAsync());
                
                    Console.WriteLine("Finished SaveStudent...");
                }
        
            }
         
            public static void AsyncQueryAndSave()
            {
                var student = GetStudent();
            
                Console.WriteLine("Let's do something else till we get student..");

                student.Wait();

            
                var studentSave = SaveStudent(student.Result);
            
                Console.WriteLine("Let's do something else till we get student.." );

                studentSave.Wait();

            }

         
         
         */

        #endregion


        #region 事务

        /**
         Entity Framework by default wraps Insert, Update or Delete operation in a transaction, whenever you execute SaveChanges(). EF starts a new transaction for each operation and completes the transaction when the operation finishes
          经测试这里的意思是SaveChanges之前的CUD操作包含在同一个事务，即每次SaveChanges期间的操作包含在一个事务

        EF 6 has introduced database.BeginTransaction and Database.UseTransaction to provide more control over transactions.you can execute multiple operations in a single transaction as shown below: 


using (var context = new BloggingContext()) 
            { 
                using (var dbContextTransaction = context.Database.BeginTransaction()) 
                { 
                    try 
                    { 
                        context.Database.ExecuteSqlCommand( 
                            @"UPDATE Blogs SET Rating = 5" + 
                                " WHERE Name LIKE '%Entity Framework%'" 
                            ); 
 
                        var query = context.Posts.Where(p => p.Blog.Rating >= 5); 
                        foreach (var post in query) 
                        { 
                            post.Title += "[Cool Blog]"; 
                        } 
 
                        context.SaveChanges(); 
 
                        dbContextTransaction.Commit(); 
                    } 
                    catch (Exception) 
                    { 
                        dbContextTransaction.Rollback(); 
                    } 
                } 
            } 
         * 
         * 
         * using (var conn = new SqlConnection("...")) 
            { 
               conn.Open(); 
 
               using (var sqlTxn = conn.BeginTransaction(System.Data.IsolationLevel.Snapshot)) 
               { 
                   try 
                   { 
                       var sqlCommand = new SqlCommand(); 
                       sqlCommand.Connection = conn; 
                       sqlCommand.Transaction = sqlTxn; 
                       sqlCommand.CommandText = 
                           @"UPDATE Blogs SET Rating = 5" + 
                            " WHERE Name LIKE '%Entity Framework%'"; 
                       sqlCommand.ExecuteNonQuery(); 
 
                       using (var context =  
                         new BloggingContext(conn, contextOwnsConnection: false)) 
                        { 
                            context.Database.UseTransaction(sqlTxn); 
 
                            var query =  context.Posts.Where(p => p.Blog.Rating >= 5); 
                            foreach (var post in query) 
                            { 
                                post.Title += "[Cool Blog]"; 
                            } 
                           context.SaveChanges(); 
                        } 
 
                        sqlTxn.Commit(); 
                    } 
                    catch (Exception) 
                    { 
                        sqlTxn.Rollback(); 
                    } 
                } 
            } 
        } 
    } 

        database.UseTransaction allows the DbContext to use a transaction which was started outside of the Entity Framework. 
        
          




          using(TransactionScope ts = new TransactionScope())
 {
      AccountData ad = new AccountData { Name = "jack",Password="1234");
      db.AccountData.AddObject(ad);
      try
      {
          db.SaveChanges();
          AccountDetailData add = new AccountDetailData(AccountId = ad.Id, Realname="超人");
          db.AccountDetailData.AddObject(add);
          db.SaveChanges();
          ts.Complete();
      }
      catch(){
         //异常后事务自动回滚
          }
 }




          




          





         */

        #endregion


        #region AddRegion
        #endregion
        //public DbSet<Student> Students { get; set; }
        //public DbSet<Standard> Standards { get; set; }

        /// <summary>
        /// 在创建派生上下文的第一个实例时仅调用此方法一次。 然后将缓存该上下文的模型，并且该模型适用于应用程序域中的上下文的所有后续实例。
        /// all your configuration code using Fluent API should be in OnModelCreating method. DbModelBuilder is a main class using which you can configure all your domain classes because at this point all your domain classes would have initialized.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure default schema
            modelBuilder.HasDefaultSchema("Admin");

            //Map entity to table
            // modelBuilder.Entity<Student>().ToTable("StudentInfo");
            modelBuilder.Entity<T>().ToTable("StandardInfo", "dbo");

            //Configure table column
            //HasMaxLength method to set size of a column. IsFixedLength method converts nvarchar to nchar type
            //configure a property as concurrency column using ConcurrencyToken method 
            //use IsRowVersion() method for byte[] property to make it as concurrency column
            //modelBuilder.Entity<T>().Property(p => p.DateOfBirth).HasColumnName("DoB")
            //            .HasColumnOrder(3)
            //            .HasColumnType("datetime2").IsRequired().IsOptional().HasMaxLength(50).IsFixedLength().IsConcurrencyToken().HasKey<int>(s => s.StudentKey).IsRowVersion();


            //sets one-to-zero or one relationship 
            //modelBuilder.Entity<T>().HasOptional().WithRequired()          
            //one-to-many 
            //modelBuilder.Entity<Student>()
            //            .HasRequired<Standard>(s => s.Standard)
            //            .WithMany(s => s.Students)
            //            .HasForeignKey(s => s.StdId);
            //one-to-many 
            //modelBuilder.Entity<Student>()
            //            .HasOptional<Standard>(s => s.Standard)
            //            .WithMany(s => s.Students)
            //            .HasForeignKey(s => s.StdId);

            //many-to-many
            //modelBuilder.Entity<Student>()
            //      .HasMany<Course>(s => s.Courses)
            //      .WithMany(c => c.Students)
            //      .Map(cs =>
            //      {
            //          cs.MapLeftKey("StudentRefId");
            //          cs.MapRightKey("CourseRefId");
            //          cs.ToTable("StudentCourse");
            //      });



            // Moved all Student related configuration to StudentEntityConfiguration class
            //modelBuilder.Configurations.Add(new StudentEntityConfiguration());


            base.OnModelCreating(modelBuilder);
        }
    }

    //public class StudentEntityConfiguration : EntityTypeConfiguration<Student>
    //{
    //    public StudentEntityConfiguration()
    //    {

    //        this.ToTable("StudentInfo");

    //        this.HasKey<int>(s => s.StudentKey);


    //        this.Property(p => p.DateOfBirth)
    //                .HasColumnName("DoB")
    //                .HasColumnOrder(3)
    //                .HasColumnType("datetime2");

    //        this.Property(p => p.StudentName)
    //                .HasMaxLength(50);

    //        this.Property(p => p.StudentName)
    //                .IsConcurrencyToken();

    //        this.HasMany<Course>(s => s.Courses)
    //           .WithMany(c => c.Students)
    //           .Map(cs =>
    //           {
    //               cs.MapLeftKey("StudentId");
    //               cs.MapRightKey("CourseId");
    //               cs.ToTable("StudentCourse");
    //           });
    //    }
    //}



    //public class SchoolDBInitializer : CreateDatabaseIfNotExists<SchoolDBContext>
    //{
    //    protected override void Seed(SchoolDBContext context)
    //    {
    //        base.Seed(context);
    //    }
    //}


    ///// <summary>
    ///// You can insert data into your database tables during the database initialization process. This will be important if you want to provide some test data for your application or to provide some default master data for your application. 
    ///// </summary>
    //public class SchoolDBInitializer : DropCreateDatabaseAlways<SchoolDBContext>
    //{
    //    protected override void Seed(SchoolDBContext context)
    //    {
    //        IList<Standard> defaultStandards = new List<Standard>();

    //        defaultStandards.Add(new Standard() { StandardName = "Standard 1", Description = "First Standard" });
    //        defaultStandards.Add(new Standard() { StandardName = "Standard 2", Description = "Second Standard" });
    //        defaultStandards.Add(new Standard() { StandardName = "Standard 3", Description = "Third Standard" });

    //        foreach (Standard std in defaultStandards)
    //            context.Standards.Add(std);

    //        base.Seed(context);
    //    }
    //}
        









}
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls
{
    /// <summary>
    /// EF支持三种类型的查询：1）Linq to Entities 2)  Entity SQL　3)Native SQL
    /// ////////////////////////////Linq  to Entities///////////////////////////////
/// LINQ Method syntax:            
////   //Querying with LINQ to Entities 
////    using (var context = new SchoolDBEntities())
////    {
////        var L2EQuery = context.Students.where(s => s.StudentName == "Bill");
        
////        var student = L2EQuery.FirstOrDefault<Student>();

////    }
        
////LINQ Query syntax:
 
////    using (var context = new SchoolDBEntities())
////    {
////        var L2EQuery = from st in context.Students
////                       where st.StudentName == "Bill"
////                       select st;
   
////        var student = L2EQuery.FirstOrDefault<Student>();
////     }
    /// ////////////////////////////Linq  to Entities///////////////////////////////
    /*
     *\\\\\\\\\\\\\\\\Entity  SQL\\\\\\\\\\\\\\\\\\\
     *
     * 
    //Querying with Object Services and Entity SQL
    string sqlString = "SELECT VALUE st FROM SchoolDBEntities.Students " +
                        "AS st WHERE st.StudentName == 'Bill'";
    
    var objctx = (ctx as IObjectContextAdapter).ObjectContext;
                
    ObjectQuery<Student> student = objctx.CreateQuery<Student>(sqlString);
                    Student newStudent = student.First<Student>();
    

You can also use EntityConnection and EntityCommand to execute Entity SQL as shown below:

    
    using (var con = new EntityConnection("name=SchoolDBEntities"))
    {
        con.Open();
        EntityCommand cmd = con.CreateCommand();
        cmd.CommandText = "SELECT VALUE st FROM SchoolDBEntities.Students as st where st.StudentName='Bill'";
        Dictionary<int, string> dict = new Dictionary<int, string>();
        using (EntityDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection))
        {
                while (rdr.Read())
                {
                    int a = rdr.GetInt32(0);
                    var b = rdr.GetString(1);
                    dict.Add(a, b);
                }
        }               
    }


     * 
     *\\\\\\\\\\\\\\\\\\Entity  SQL\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
     *
     * 
     * 
     * \\\\\\\\\\\\\\\\\\\\\\\\Native SQL\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
     * Native SQL You can execute native SQL queries for a relational database as shown below:

    
    using (var ctx = new SchoolDBEntities())
    {
        var studentName = ctx.Students.SqlQuery("Select studentid, studentname, standardId from Student where studentname='Bill'").FirstOrDefault<Student>();
    }    

     * \\\\\\\\\\\\\\\\\\\\\\\\\Native SQL\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
     */
    /// </summary>
    public static class EfHelper<T> where T:class
    {
        #region DbContext相关
        /// <summary>
        /// 将高层的DbContext转换为较低阶的ObjectContext
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static ObjectContext ConvertTo(DbContext dbContext)
        {
            IObjectContextAdapter ioca = dbContext as IObjectContextAdapter;
            return ioca == null ? null : ioca.ObjectContext;
        }


        public static void SaveChanges(DbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        public static void Excute()
        {/*
            using (var context = new SchoolDBEntities())
            {
                context.Configuration.AutoDetectChangesEnabled = true;// false then context cannot detect changes made to existing entities so do not execute update query.默认是true
                var studentList = context.Students.ToList<Student>();

                //Perform create operation
                context.Students.Add(new Student() { StudentName = "New Student" });

                //Perform Update operation
                Student studentToUpdate = studentList.Where(s => s.StudentName == "student1").FirstOrDefault<Student>();
                studentToUpdate.StudentName = "Edited student1";

                //Perform delete operation
                context.Students.Remove(studentList.ElementAt<Student>(0));

                //Execute Inser, Update & Delete queries in the database
                context.SaveChanges();

            }*/
        }

        #endregion

        #region DBSet相关

        /// <summary>
        ///  Adds the given entity to the context the Added state. When the changes are being saved, the entities in the Added states are inserted into the database. After the changes are saved, the object state changes to Unchanged. 
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Add(DbSet<T> dbSet, T entity)
        {
            return dbSet.Add(entity);
        }

        /// <summary>
        /// Entities returned as AsNoTracking, will not be tracked by DBContext. This will be significant performance boost for read only entities. 
        /// </summary>
        /// <param name="dbSet"></param>
        /// <returns></returns>
        public static DbQuery<T> AsNoTracking(DbSet<T> dbSet)
        {
            return dbSet.AsNoTracking();
        }

        /// <summary>
        /// Attach the given entity to the context in unchanged state
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Attch(DbSet<T> dbSet, T entity)
        {
            return dbSet.Attach(entity);
        }

        /// <summary>
        /// Creates a new instance of an entity for the type of this set. This instance is not added or attached to the set. The instance returned will be a proxy if the underlying context is configured to create proxies and the entity type meets the requirements for creating a proxy.
        /// </summary>
        /// <param name="dbSet"></param>
        /// <returns></returns>
        public static T Create(DbSet<T> dbSet)
        {
            return dbSet.Create();
        }

        /// <summary>
        /// Uses the primary key value to attempt to find an entity tracked by the context. If the entity is not in the context then a query will be executed and evaluated against the data in the data source, and null is returned if the entity is not found in the context or in the data source. Note that the Find also returns entities that have been added to the context but have not yet been saved to the database.
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Find(DbSet<T> dbSet,Guid id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// 可以将离线的entity转为dbcontext在线
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        /// <param name="state"></param>
        public static void Change(DbContext dbContext, T entity, EntityState state)
        {
            dbContext.Entry(entity).State = state;
        }


        /// <summary>
        /// Update可以将离线的entity转为dbcontext在线
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        /// <param name="state"></param>
        public static void Update(DbContext dbContext, T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Marks the given entity as Deleted. When the changes are saved, the entity is deleted from the database. The entity must exist in the context in some other state before this method is called. 
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Remove(DbSet<T> dbSet,T entity)
        {
            return dbSet.Remove(entity);
        }

        /// <summary>
        /// Creates a raw SQL query that will return entities in this set. By default, the entities returned are tracked by the context; this can be changed by calling AsNoTracking on theDbSqlQuery<T> returned from this method.
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DbSqlQuery<T> SqlQuery(DbSet<T> dbSet,string sql, params object[] parameters)
        {
            return dbSet.SqlQuery(sql, parameters);
        }


        /// <summary>
        /// A SQL query returning instances of any type, including primitive types,
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DbRawSqlQuery<T> SqlQuery(DbContext dbContext, string sql, params object[] parameters)
        {
            return dbContext.Database.SqlQuery<T>(sql, parameters);
        }

        /// <summary>
        /// ExecuteSqlCommnad method is useful in sending non-query commands to the database, such as the Insert, Update or Delete command. 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteSqlCommand(DbContext dbContext, string sql, params object[] parameters)
        {
            return dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        #endregion

        #region DBEntityEntry相关

        /// <summary>
        /// Gets a System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> object for
        //     the given entity providing access to information about the entity and the
        //     ability to perform actions on the entity.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static DbEntityEntry GetDbEntityEntry(DbContext dbContext, T entity)
        {
            return dbContext.Entry(entity);
        }


        public static void ShowDbEntityEntry(DbEntityEntry entry)
        {
            Console.WriteLine(entry.State);
            foreach (var propertyName in entry.CurrentValues.PropertyNames)
            {
                Console.WriteLine("Property Name: {0}", propertyName);
                
                //get original value
                var orgVal = entry.OriginalValues[propertyName];
                Console.WriteLine("     Original Value: {0}", orgVal);

                //get current values
                var curVal = entry.CurrentValues[propertyName];
                Console.WriteLine("     Current Value: {0}", curVal);
            }
        }

        /// <summary>
        /// DbEntityEntry enables you to set Added, Modified or Deleted EntityState to an entity 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="state"></param>
        public static void ChangeState(DbEntityEntry entry, EntityState state)
        {
            entry.State = state;
        }

        /// <summary>
        /// Reloads the entity from the database overwriting any property values with values from the database. The entity will be in the Unchanged state after calling this method.
        /// </summary>
        /// <param name="entry"></param>
        public static void Reload(DbEntityEntry entry)
        {
            entry.Reload();
        }

        #endregion
    }
}

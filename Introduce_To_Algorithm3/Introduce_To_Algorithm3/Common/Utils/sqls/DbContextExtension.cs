using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class DbContextExtension:DbContext
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


    }
}

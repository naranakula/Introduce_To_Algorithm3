﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Entity;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2
{
    /// <summary>
    /// 为MySql设置DbConfiguration.This step is optional but highly recommended, since it adds all the dependency resolvers for MySql classes. 
    /// This can by done by Adding the DbConfigurationTypeAttribute on the context class
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MySqlDbContext:EfDbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //ToDo:
        }
    }

}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.DbConfigurations
{
    /// <summary>
    /// DbConfiguration的主要作用是在不使用配置文件的情况下注册Provider
    ///  The config file takes precedence over code-based configuration. 
    /// </summary>
    public class MsEfConfiguration:DbConfiguration
    {
        public MsEfConfiguration()
        {
            //设置Provider
            SetProviderServices(SqlProviderServices.ProviderInvariantName,SqlProviderServices.Instance);

            //设置默认的连接工厂
            SetDefaultConnectionFactory(new SqlConnectionFactory());

            //设置数据库初始化方式
            this.SetDatabaseInitializer<EfDbContext>(null);

        }
    }
}

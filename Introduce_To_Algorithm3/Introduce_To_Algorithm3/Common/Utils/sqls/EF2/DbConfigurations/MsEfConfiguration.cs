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
    /// </summary>
    public class MsEfConfiguration:DbConfiguration
    {
        public MsEfConfiguration()
        {
            SetProviderServices(SqlProviderServices.ProviderInvariantName,SqlProviderServices.Instance);

            SetDefaultConnectionFactory(new SqlConnectionFactory());

        }
    }
}

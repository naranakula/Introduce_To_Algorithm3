using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.EntityFramework;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.DbConfigurations
{
    /// <summary>
    /// DbConfiguration的主要作用是在不使用配置文件的情况下注册Provider
    /// </summary>
    public class OracleEfConfiguration:DbConfiguration
    {
        public OracleEfConfiguration()
        {
            //注册ef的provider
            SetProviderServices("Oracle.ManagedDataAccess.Client",EFOracleProviderServices.Instance);
        }
    }
}

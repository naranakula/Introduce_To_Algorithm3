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
    public static class EFHelper
    {
        /// <summary>
        /// 将高层的DbContext转换为较低阶的ObjectContext
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static ObjectContext ConvertTo(this DbContext dbContext)
        {
            IObjectContextAdapter ioca = dbContext as IObjectContextAdapter;
            return ioca == null ? null : ioca.ObjectContext;
        }
    }
}

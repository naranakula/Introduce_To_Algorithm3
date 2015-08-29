using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF
{
    public class BaseContext:DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameOrConnString"></param>
        public BaseContext(string nameOrConnString):base(nameOrConnString)
        {
            
        }

    }
}

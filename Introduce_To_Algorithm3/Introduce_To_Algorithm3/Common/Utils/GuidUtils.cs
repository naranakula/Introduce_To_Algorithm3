using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public static class GuidUtils
    {
        /// <summary>
        /// the format is:
        /// 8-4-4-12
        /// xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

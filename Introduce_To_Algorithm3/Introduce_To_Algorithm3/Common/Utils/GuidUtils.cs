using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// GUID有16个字节
    /// </summary>
    public static class GuidUtils
    {
        /// <summary>
        /// the format is: 字母小写 36位
        /// 8-4-4-4-12
        /// xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 32位小写格式 
        /// xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        /// </summary>
        /// <returns></returns>
        public static string GetGuid2()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}

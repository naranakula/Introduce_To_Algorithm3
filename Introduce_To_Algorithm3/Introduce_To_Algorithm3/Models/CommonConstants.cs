using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3.Models
{



    /// <summary>
    /// 程序通用常量
    /// </summary>
    public static class CommonConstants
    {

        /// <summary>
        /// 程序启动时间
        /// </summary>
        public static readonly DateTime AppStartTime = DateTime.Now;

        /// <summary>
        /// 应用程序Id
        /// </summary>
        public static readonly string AppId = ConfigUtils.GetString("AppId", "f26cd7d34c8c402f96bcf8b0b896d6c6");


        /// <summary>
        /// 程序主标题
        /// </summary>
        public static readonly string MainTitle = ConfigUtils.GetString("MainTitle");


    }



}

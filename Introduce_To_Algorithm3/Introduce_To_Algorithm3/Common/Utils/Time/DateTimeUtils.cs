using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// get a now string represent
        /// </summary>
        public static String NowString
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); }
        }


        /// <summary>
        /// 解析字符串到dateTime
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="format">
        /// y:年
        /// M:月
        /// d:日
        /// H:时
        /// m:分
        /// s:秒
        /// </param>
        /// <returns></returns>
        public static DateTime Parse(string dateTimeString,string format)
        {
            return DateTime.ParseExact(dateTimeString, format, CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// 尝试解析字符串到datetime
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="format">
        /// y:年
        /// M:月
        /// d:日
        /// H:时
        /// m:分
        /// s:秒
        /// </param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string dateTimeString,string format,out DateTime result)
        {
            return DateTime.TryParseExact(dateTimeString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }


    }
}

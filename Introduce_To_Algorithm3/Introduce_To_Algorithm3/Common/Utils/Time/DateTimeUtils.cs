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
        /// 
        /// UTC时间(0时区的时间)+8小时 = 本地(北京)时间
        /// Now是本地时间，在不同的地方结果可能不同
        /// UtcNow是0时区的时间，不同的地方保证结果一样
        ///  整个地球分为二十四时区，每个时区都有自己的本地时间。在国际无线电通信场合，为了统一起见，使用一个统一的时间，称为通用协调时(UTC, Universal Time Coordinated)。UTC是0时区的时间
        /// UTC + 时区差 ＝ 本地时间
        /// 
        /// 
        /// 
        /// get a now string represent
        /// f 秒的小数精度为一位。其余数字被截断。 
        ///        ff 秒的小数精度为两位。其余数字被截断。 
        ///fff 秒的小数精度为三位。其余数字被截断。 
        ///ffff 秒的小数精度为四位。其余数字被截断。 
        ///fffff 秒的小数精度为五位。其余数字被截断。 
        ///ffffff 秒的小数精度为六位。其余数字被截断。 
        ///fffffff 秒的小数精度为七位。其余数字被截断。 
        ///最多7位
        /// </summary>
        public static String NowString
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); }
        }

        /// <summary>
        /// 返回本地时间
        /// </summary>
        public static DateTime Now
        {
            get { return DateTime.Now;}
        }

        /// <summary>
        /// 获取UTC时间
        /// </summary>
        public static DateTime UtcNow
        {
            get { return DateTime.UtcNow;}
        }

        /// <summary>
        /// 获取本地时区
        /// </summary>
        /// <returns></returns>
        public static TimeZoneInfo GetLocalTimeZone()
        {
            return TimeZoneInfo.Local;
        }


        /// <summary>
        /// 获取本地时区-UTC时区的时间差
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetUtcOffset()
        {
            //获取表示本地时区的 TimeZoneInfo 对象
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.Local;
            return timeZoneInfo.BaseUtcOffset;
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
        /// 解析字符串到dateTime
        /// 如果解析失败返回null
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="format"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static DateTime? ParseSafe(string dateTimeString,string format,Action<Exception> exceptionHandler = null)
        {
            try
            {
                return DateTime.ParseExact(dateTimeString, format, CultureInfo.InvariantCulture);
            }
            catch(Exception ex)
            {
                if(exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
                return null;
            }
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

        /// <summary>
        /// 尝试解析日期字符串
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="formats">格式数组，至少有一个格式匹配</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string dateTimeString,string[] formats,out DateTime result)
        {
            return DateTime.TryParseExact(dateTimeString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }


    }
}

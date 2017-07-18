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

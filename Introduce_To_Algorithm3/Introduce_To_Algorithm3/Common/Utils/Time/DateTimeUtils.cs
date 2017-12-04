using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Text;
using System.Web.Caching;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

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
        ///F和f的区别是显不显示结尾0
        ///最多7位
        ///K 表示 DateTime.Kind 属性的不同值，即“Local”、“Utc”或“Unspecified”。此说明符以文本形式设置 Kind 值并保留时区。
        ///如果 Kind 值为“Local”，则此说明符等效于“zzz”说明符，用于显示本地时间偏移量，例如“-07:00”。
        ///对于“Utc”类型值，该说明符显示字符“Z”以表示 UTC 日期。
        ///对于“Unspecified”类型值，该说明符等效于“”（无任何内容）。
        /// </summary>
        public static String NowString
        {
            get
            {
                //14长度的字符串日期表示，建议使用它进行数据交换
                String date14Str = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                String date17Str = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.CurrentCulture);
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK", CultureInfo.InvariantCulture);
            }
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
            return DateTime.ParseExact(dateTimeString, format, CultureInfo.CurrentCulture);
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
                return DateTime.ParseExact(dateTimeString, format, CultureInfo.CurrentCulture);
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
            return DateTime.TryParseExact(dateTimeString, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out result);
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
            return DateTime.TryParseExact(dateTimeString, formats, CultureInfo.CurrentCulture, DateTimeStyles.None, out result);
        }


        #region unix时间戳

        //unix时间戳是自UTC时间 1970年1月1日午夜所经过的秒数

            /// <summary>
            /// 获取当前的unix时间戳
            /// </summary>
            /// <returns></returns>
        public static long Epoch()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// 将unix时间戳转换为本地时间
        /// </summary>
        /// <param name="epoch">unix时间戳是自UTC时间 1970年1月1日午夜所经过的秒数</param>
        /// <returns></returns>
        public static DateTime FromEpoch(long epoch)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
            return startTime.AddSeconds(epoch);
        }


        #endregion

        #region 从网络上获取真实时间

        /// <summary>
        /// 从网络获取时间
        /// 如果返回为null,基本可以认为公网不可用
        /// </summary>
        /// <returns></returns>
        public static DateTime? GetNetworkDateTime()
        {
            var result = GetNetworkDateTime("http://www.baidu.com");

            if (result == null)
            {
                result = GetNetworkDateTime("http://www.taobao.com");
            }

            if (result == null)
            {
                result = GetNetworkDateTime("http://www.qq.com/");
            }

            if (result == null)
            {
                result = GetNetworkDateTime("http://www.jd.com/");
            }

            if (result == null)
            {
                result = GetNetworkDateTime("http://www.360.com/");
            }

            if (result == null)
            {
                result = GetNetworkDateTime("http://www.163.com/");
            }

            return result;
        }


        /// <summary>
        /// 从网络获取时间
        /// </summary>
        /// <param name="url"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static DateTime? GetNetworkDateTime(string url,Action<Exception> exceptionHandler =null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }


            NLogHelper.Trace(url);

            WebRequest request = null;
            WebResponse response = null;
            try
            {
                request = WebRequest.Create(url);
                request.Timeout = 6000;//单位毫秒
                request.Credentials = CredentialCache.DefaultCredentials;
                request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                response = request.GetResponse();
                var headers = response.Headers;
                string dateStr = null;
                foreach (var item in headers.AllKeys)
                {
                    if (item == "Date")
                    {
                        //Http响应的Date
                        dateStr = headers[item];
                        break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(dateStr))
                {
                    //Convert将尽量尝试解析
                    return Convert.ToDateTime(dateStr);
                }

                return null;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return null;
            }
            finally
            {
                if (request != null)
                {
                    try
                    {
                        request.Abort();
                    }
                    catch 
                    {
                    }
                }

                if (response != null)
                {
                    try
                    {
                        response.Close();
                    }
                    catch (Exception e)
                    {
                    }
                    
                }
            }

        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// a class extend the function of string
    /// </summary>
    public static class StringUtils
    {
        #region 常用

        /// <summary>
        /// format
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        /// <summary>
        /// 截短字符串
        /// 如果字符串为null，返回string.Empty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimEx(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return s.Trim();
        }

        /// <summary>
        /// 忽略空白，比较字符串是否相同
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool EqualsEx(this string s1, string s2,bool ignoreCase = true)
        {
            if (string.IsNullOrWhiteSpace(s1) && string.IsNullOrWhiteSpace(s2))
            {
                return true;
            }

            if (s1 == null || s2 == null)
            {
                //此时s1 , s2不可能同时为null
                return false;
            }

            if (ignoreCase)
            {
                return String.Equals(s1.Trim(), s2.Trim(), StringComparison.CurrentCultureIgnoreCase);
            }
            else
            {
                return s1.Trim() == s2.Trim();
            }
        }

        /// <summary>
        /// 组合字符串，并且如果为null，则用String.Empty连接
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="isTrim">是否去掉字符串前后的空格</param>
        /// <returns></returns>
        public static string CombineEx(string[] arr,bool isTrim = true)
        {
            if (arr == null || arr.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var item in arr)
            {
                if (item != null)
                {
                    sb.Append(isTrim ? item.Trim() : item);
                }
            }

            return sb.ToString();
        }

        #endregion

        #region UnCommon
        /// <summary>
        /// remove s2 from the start of s1, if s2 is exactly the start part of s1. else s1 is returned.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static string SubtractStart(this string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return s1;
            }

            if (s1.StartsWith(s2))
            {
                return s1.Substring(s2.Length);
            }
            else
            {
                return s1;
            }
        }

        /// <summary>
        /// remove s2 from the end of s1, if s2 is exactly the end part of s1. else s1 is returned.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static string SubtractEnd(this string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return s1;
            }

            if (s1.EndsWith(s2))
            {
                return s1.Substring(0, s1.Length - s2.Length);
            }
            else
            {
                return s1;
            }
        }
        #endregion
    }
}
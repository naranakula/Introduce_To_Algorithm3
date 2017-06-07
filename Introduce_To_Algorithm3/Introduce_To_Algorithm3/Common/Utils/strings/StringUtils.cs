﻿using System;
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
            if (s == null)
            {
                return string.Empty;
            }

            if (args == null || args.Length == 0)
            {
                return s;
            }

            return string.Format(s, args);
        }
        
        /// <summary>
        /// 忽略字符串前后空白，比较字符串是否相同
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool EqualsEx(string s1, string s2, bool ignoreCase = true)
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
        /// 分割字符串  取消空白项，去掉结果前后空白
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<String> Split(String source, params char[] separator)
        {
            List<String> list = new List<string>();
            if (string.IsNullOrWhiteSpace(source))
            {
                return list;
            }

            //返回结果永不为null
            String [] arr = source.Split(separator);

            foreach (var item in arr)
            {
                if (!String.IsNullOrWhiteSpace(item))
                {
                    list.Add(item.Trim());
                }
            }

            return list;
        } 

        /// <summary>
        /// 截短字符串
        /// 如果字符串为null，返回string.Empty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimEx(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return s.Trim();
        }

        /// <summary>
        /// 截短字符串
        /// 如果字符串为null，返回string.Empty
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxLength">截断字符串并限制其最大长度，如果超过，取前面的部分</param>
        /// <returns></returns>
        public static string TrimToLength(string s, int maxLength = 1000)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            string result = s.Trim();
            if (result.Length <= maxLength)
            {
                return result;
            }
            else
            {
                return result.Substring(0, maxLength);
            }
        }


        /// <summary>
        /// 组合字符串，并且如果数据项为null，则用String.Empty连接
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="isTrim">是否去掉字符串前后的空格</param>
        /// <param name="separator">分隔符，只保留中间部分分隔符</param>
        /// <returns></returns>
        public static string CombineEx(string[] arr, string separator = "", bool isTrim = true)
        {
            if (arr == null || arr.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < arr.Length; i++)
            {
                var item = arr[i];
                if (item != null)
                {
                    sb.Append(isTrim ? item.Trim() : item);
                }
                if (i != arr.Length - 1)
                {
                    //不是最后一个
                    sb.Append(separator);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 组合字符串，并且如果数据项为null，则用String.Empty连接
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator">分隔符，只保留中间部分分隔符</param>
        /// <param name="isTrim">是否去掉字符串前后的空格</param>
        /// <returns></returns>
        public static string CombineEx(List<string> list, string separator = "", bool isTrim = true)
        {
            if (list == null || list.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item != null)
                {
                    sb.Append(isTrim ? item.Trim() : item);
                }
                if (i != list.Count - 1)
                {
                    //不是最后一个
                    sb.Append(separator);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 判断数组是否包含指定字符串。
        /// 如果数组本身为null或者空，返回false.
        /// 
        /// 比较字符串是空、null、空白是相同的。
        /// 比较字符串时忽略开头结尾的空白，忽略大小写
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ContainsEx(String[] arr, string s)
        {
            if (arr == null || arr.Length == 0)
            {
                return false;
            }

            return arr.Any(item => EqualsEx(item, s));
        }

        /// <summary>
        /// 判断列表是否包含指定字符串。
        /// 如果列表本身为null或者空，返回false.
        /// 
        /// 比较字符串是空、null、空白是相同的。
        /// 比较字符串时忽略开头结尾的空白，忽略大小写
        /// </summary>
        /// <param name="list"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ContainsEx(List<string> list, string s)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }

            return list.Any(item => EqualsEx(item, s));
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Quartz.Impl.Triggers;
using TinyPinyin.Core;
using ToolGood.Words;

namespace Introduce_To_Algorithm3.Common.Utils.strings
{
    /// <summary>
    /// a class extend the function of string
    /// </summary>
    public static class StringUtils
    {

        #region 常用

        #region 字节数组和字符串 Base64转换 base64比x2短

        /*
         * byte是表示[0,255]的无符号整数
         * 将3个byte共24位转换为4个6位的编码
         * 6位可以表示[0,63]
         * [0,63]对应的编码ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/
         * 在字符串结尾外加0到2个=字符作为辅助，0个=表示byte个数是3的倍数，
         * 2个=表示剩余1个byte（编译成2个字符），1个=表示剩余2个byte（编译成3个字符）
         * 最后字符串长度是4的倍数
         * 保证 arr = FromBase64String(ToBase64String(arr)) if arr != null
         * 
         * 
         */

        /// <summary>
        /// 将byte数组转为base64字符串
        /// 2个=表示剩余1个byte（编译成2个字符），1个=表示剩余2个byte（编译成3个字符）
        /// 最后字符串长度是4的倍数
        /// 保证 arr = FromBase64String(ToBase64String(arr)) if arr != null
        /// </summary>
        /// <param name="arr">如果arr是null或空数组，返回String.Empty</param>
        /// <returns>如果arr是null或空数组，返回String.Empty</returns>
        public static string ToBase64String(byte[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                return string.Empty;
            }
            
            return Convert.ToBase64String(arr);
        }

        /// <summary>
        /// 将base64字符串s转换为byte数组
        /// 忽略\t,\r,\n,空格，但是如果有其它非法字符抛出异常
        /// 如果s是null或者空白，返回空数组
        /// </summary>
        /// <param name="s">如果s是null或者空白，返回空数组</param>
        /// <returns></returns>
        public static byte[] FromBase64String(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return new byte[] {};
            }

            return Convert.FromBase64String(s);
        }

        #endregion


        #region byte与x2

        /// <summary>
        /// 转换为两位16进制小写字符串
        /// </summary>
        /// <param name="byteIn"></param>
        /// <returns></returns>
        public static string FromByte(byte byteIn)
        {
            //x2小写 X2大写  CultureInfo加不加都可以
            return byteIn.ToString("x2"/*,CultureInfo.CurrentCulture*/);
        }

        /// <summary>
        /// 将x2或者X2格式字符串转换为byte
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte ToByte(string str)
        {
            return Convert.ToByte(str,16);
        }

        #endregion

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
                return string.Equals(s1.Trim(), s2.Trim(), StringComparison.CurrentCulture);
            }
        }

        /// <summary>
        /// 比较byte数组
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="isNullEqualsEmpty"></param>
        /// <returns></returns>
        public static bool EqualsEx(byte[] b1,byte[] b2,bool isNullEqualsEmpty = true)
        {
            if(b1==null && b2 == null)
            {
                return true;
            }


            if(b1==null)
            {
                if (isNullEqualsEmpty)
                {
                    if (b2.Length == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            if (b2 == null)
            {
                if (isNullEqualsEmpty)
                {
                    if(b1.Length == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            //b1和b2 不为null

            if(b1.Length != b2.Length)
            {
                return false;
            }

            //同一引用做性能优化
            if(b1 == b2)
            {
                return true;
            }

            int length = b1.Length;
            //长度相同
            for(int i = 0; i < length; i++)
            {
                if(b1[i] != b2[i])
                {
                    return false;
                }
            }

            return true;


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
        /// 数组为null或者为空
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(byte[] b)
        {
            return b == null || b.Length == 0;
        }


        /// <summary>
        /// 判断字符串s是以start开始的，如果start是null或空白，直接返回true
        /// 忽略字符串前后空白
        /// </summary>
        /// <param name="s"></param>
        /// <param name="start"></param>
        /// <param name="isIgnoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static bool StartWith(string s,string start,bool isIgnoreCase = true)
        {

            if (string.IsNullOrWhiteSpace(start))
            {
                //start为空白，直接返回true
                return true;
            }

            if (string.IsNullOrWhiteSpace(s))
            {
                //start肯定不为空白
                return false;
            }

            //此时s start肯定不为null或空白

            string normalizedS = s.Trim();
            string normalizedStart = start.Trim();

            if (isIgnoreCase)
            {
                return normalizedS.StartsWith(normalizedStart, StringComparison.CurrentCultureIgnoreCase);
            }
            else
            {
                return normalizedS.StartsWith(normalizedStart);
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


        ///// <summary>
        ///// 组合字符串，并且如果数据项为null，则用String.Empty连接
        ///// </summary>
        ///// <param name="enumerable"></param>
        ///// <param name="isTrim">是否去掉字符串前后的空格</param>
        ///// <param name="separator">分隔符，只保留中间部分分隔符</param>
        ///// <returns></returns>
        //public static string CombineEx(IEnumerable<String> enumerable, string separator = ",", bool isTrim = true)
        //{
        //    if (enumerable == null)
        //    {
        //        return string.Empty;
        //    }

        //    StringBuilder sb = new StringBuilder();

        //    //取消空白
        //    var tempList = enumerable.Where(r => !String.IsNullOrWhiteSpace(r)).ToList();


        //    for (int i = 0; i < tempList.Count; i++)
        //    {
        //        var item = tempList[i];
        //        sb.Append(isTrim ? item.Trim() : item);
        //        if (i != tempList.Count - 1)
        //        {
        //            //不是最后一个
        //            sb.Append(separator);
        //        }
        //    }

        //    return sb.ToString();
        //}

        /// <summary>
        /// 组合字符串，并且如果数据项为null，则用String.Empty连接
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator">分隔符，只保留中间部分分隔符</param>
        /// <param name="isTrim">是否去掉字符串前后的空格</param>
        /// <returns></returns>
        public static string CombineEx(List<string> list, string separator = ",", bool isTrim = true)
        {
            if (list == null || list.Count == 0)
            {
                return string.Empty;
            }

            //选出不为空白的
            var tempList = list.Where(r => !String.IsNullOrWhiteSpace(r)).ToList();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < tempList.Count; i++)
            {
                var item = tempList[i];
                sb.Append(isTrim ? item.Trim() : item);
                if (i != tempList.Count - 1)
                {
                    //不是最后一个
                    sb.Append(separator);
                }
            }

            return sb.ToString();
        }

        ///// <summary>
        ///// 判断数组是否包含指定字符串。
        ///// 如果数组本身为null或者空，返回false.
        ///// 
        ///// 比较字符串是空、null、空白是相同的。
        ///// 比较字符串时忽略开头结尾的空白，忽略大小写
        ///// </summary>
        ///// <param name="arr"></param>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public static bool ContainsEx(String[] arr, string s)
        //{
        //    if (arr == null || arr.Length == 0)
        //    {
        //        return false;
        //    }

        //    return arr.Any(item => EqualsEx(item, s));
        //}

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

        #region 第三方

        #region  nuget TinyPinyin.Core  .net4.5不能用

        /// <summary>
        /// 生成的拼音不包含声调，均为大写
        /// 中国 - ZHONGGUO
        /// </summary>
        /// <param name="text"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetPinyin(string text, string separator = ",")
        {
            return PinyinHelper.GetPinyin(text,separator);
        }

        #endregion


        #region  nuget ToolGood.Words敏感词过滤  https://github.com/toolgood/ToolGood.Words

        /// <summary>
        /// WordsHelper.GetPinYin("我爱中国");//WoAiZhongGuo
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetPinYin2(string text)
        {
            return WordsHelper.GetPinYin(text);
        }

        #endregion



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
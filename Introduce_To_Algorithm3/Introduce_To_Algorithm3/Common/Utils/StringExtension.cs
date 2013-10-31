using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// a class extend the function of string
    /// </summary>
    public static class StringExtension
    {
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
    }
}
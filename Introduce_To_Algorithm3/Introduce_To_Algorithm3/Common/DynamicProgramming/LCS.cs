using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.DynamicProgramming
{
    /// <summary>
    /// longest common subsequence
    /// common subsequence has two kinds: 1)consecutive 2)unconsecutive
    /// </summary>
    public class LCS
    {
        /// <summary>
        /// find consecutive lcs
        /// construct a matrix [s1.Length,s2.Length].
        /// if s1[i] = s2[j],then mark matrix[i,j] 1
        /// find the  diagonal line which marks 1
        /// it runs at O(s1.Length*s2.Length)
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static string LcsConsecutive(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return string.Empty;
            }

            if (s1 == s2)
            {
                return s1;
            }

            //the last postion of lcs in s1
            int index = 0;
            //the length of lcs
            int length = 0;
            int[,] matrix = new int[s1.Length,s2.Length];
            for (int i = 0; i < s1.Length; i++)
            {
                for (int j = 0; j < s2.Length; j++)
                {
                    //the one in diagonal line before current one
                    int n = (i - 1 >= 0 && j - 1 >= 0) ? matrix[i - 1, j - 1] : 0;
                    matrix[i, j] = (s1[i] == s2[j]) ? n + 1 : 0;
                    if (matrix[i, j] > length)
                    {
                        length = matrix[i, j];
                        index = i;
                    }
                }
            }
            if (length == 0)
            {
                return string.Empty;
            }
            return s1.Substring(index - length + 1, length);
        }



        /// <summary>
        /// find the unconsecutive lcs of s1 and s2
        /// for two sequence:
        /// X = {x1,x2, ... ... ,xm}
        /// Y = {y1,y2, ... ... ,yn}
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static string LcsUnConsecutive(string s1, string s2)
        {
            
        }

    }
}

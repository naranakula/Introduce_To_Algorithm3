using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.DynamicProgramming
{
    public class EditDistance
    {
        /// <summary>
        /// compute the edit distance from source to string dest<br />
        /// note: the edit distance from source to dest equals the edit distance from dest to source
        /// suppose we can only allow three basic string operation: 
        /// 1)delete a character   2)replace a character with another character   3)insert a new character
        /// the edit distance is the min step we need to change a string source to target
        /// </summary>
        /// <returns></returns>
        public static int LevenshteinDistance(string source, string dest)
        {
            if (source == null && dest == null)
            {
                return 0;
            }

            if (string.IsNullOrEmpty(source))
            {
                return dest.Length;
            }
            if (string.IsNullOrEmpty(dest))
            {
                return source.Length;
            }

            int m = dest.Length;
            int n = source.Length;
            int[,] matrix = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++)
            {
                //init first column
                matrix[i, 0] = i;
            }
            for (int i = 0; i <= m; i++)
            {
                //init first row
                matrix[0, i] = i;
            }

            char ch1, ch2;
            int temp = 0;
            //核心算法
            /*
             * 扫描两字符串（n*m级的），如果：str1[i] == str2[j]，用temp记录它，为0。否则temp记为1。
             * 然后在矩阵d[i,j]赋于d[i-1,j]+1 删除 、d[i,j-1]+1 插入、d[i-1,j-1]+temp 替换三者的最小值。 
             * 扫描完后，返回矩阵的最后一个值d[n][m]即是它们的距离。
             */
            /*
             如果我们可以使用k个操作数把s[1…i]转换为t[1…j-1]，我们只需要把t[j]加在最后面就能将s[1…i]转换为t[1…j]，操作数为k+1
如果我们可以使用k个操作数把s[1…i-1]转换为t[1…j]，我们只需要把s[i]从最后删除就可以完成转换，操作数为k+1
如果我们可以使用k个操作数把s[1…i-1]转换为t[1…j-1]，我们只需要在需要的情况下
（s[i] != t[j]）把s[i]替换为t[j]，所需的操作数为k+cost（cost代表是否需要转换，如
果s[i]==t[j]，则cost为0，否则为1）。
将s[1…n]转换为t[1…m]当然需要将所有的s转换为所有的t，所以，d[n,m]（表格的右
下角）就是我们所需的结果。
             */ 
            for (int i = 1; i <= n; i++)
            {
                ch1 = source[i - 1];
                for (int j = 1; j <= m; j++)
                {
                    ch2 = dest[j - 1];
                    if (ch1.Equals(ch2))
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    matrix[i, j] = System.Math.Min(System.Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + temp);
                }
            }

            //打印数组
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= m; j++)
                {
                    Console.Write(" {0} ", matrix[i, j]);
                }
                Console.WriteLine("");
            }

            return matrix[n, m];
        }


        /// <summary>
        /// 计算字符串相似度(值越大字符串越相似)
        /// 计算相似度的公式： 1-编辑距离/max(source.length,dest.length)
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static double LevenshteinDistancePercent(string str1, string str2)
        {
            int val = LevenshteinDistance(str1, str2);
            return 1 - (double)val / System.Math.Max(str1.Length, str2.Length);
        }

    }
}

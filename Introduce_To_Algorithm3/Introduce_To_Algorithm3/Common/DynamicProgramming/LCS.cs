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
    public static class LCS
    {
        #region
        /// <summary>
        /// find consecutive lcs
        /// construct a matrix [s1.Length,s2.Length].
        /// if s1[i] = s2[j],then mark matrix[i,j] 1
        /// find the  diagonal line which marks 1
        /// it runs at O(s1.Length*s2.Length)
        /// 
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>if s1 and s2 are completely different, the result is String.Empty</returns>
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
            int[,] matrix = new int[s1.Length, s2.Length];
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
        #endregion


        #region
        /// <summary>
        /// find the unconsecutive lcs of s1 and s2
        /// for two sequence:
        /// X = {x1,x2, ... ... ,xm}
        /// Y = {y1,y2, ... ... ,yn}
        /// and
        /// Z = {z1,z2, ... ... ,zk} is lcs of X and Y
        /// Using dynamic programming to solve the problem, the optimal subsequence is:
        /// if xm=yn,then zk=xm, the optimal subsequence is lcs of  {x1,x2, ... ... ,xm-1} and {y1,y2, ... ... ,yn-1}
        /// if xm!=yn, the optimal subsequence is the max lcs of  {x1,x2, ... ... ,xm} and {y1,y2, ... ... ,yn-1} or 
        /// {x1,x2, ... ... ,xm-1} and {y1,y2, ... ... ,yn}
        /// 
        /// 
        /// it runs very slowly,(exponential-time) because of much overlap subproblem
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>if s1 and s2 are completely different, the result is String.Empty</returns>
        public static string LcsUnConsecutiveRecursive(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return string.Empty;
            }

            if (s1 == s2)
            {
                return s1;
            }
            return LcsUnConsecutiveRecursive(s1, s1.Length - 1, s2, s2.Length - 1);
        }



        private static string LcsUnConsecutiveRecursive(string s1, int s1End, string s2, int s2End)
        {
            if (s1End < 0 || s2End < 0)
            {
                return string.Empty;
            }

            if (s1[s1End] == s2[s2End])
            {
                return LcsUnConsecutiveRecursive(s1, s1End - 1, s2, s2End - 1) + s1[s1End];
            }
            else
            {
                string t1 = LcsUnConsecutiveRecursive(s1, s1End - 1, s2, s2End);
                string t2 = LcsUnConsecutiveRecursive(s1, s1End, s2, s2End - 1);

                return t1.Length > t2.Length ? t1 : t2;
            }
        }

        #endregion


        #region memory all the result that calculated
        /// <summary>
        /// find the unconsecutive lcs of s1 and s2
        /// for two sequence:
        /// X = {x1,x2, ... ... ,xm}
        /// Y = {y1,y2, ... ... ,yn}
        /// and
        /// Z = {z1,z2, ... ... ,zk} is lcs of X and Y
        /// Using dynamic programming to solve the problem, the optimal subsequence is:
        /// if xm=yn,then zk=xm, the optimal subsequence is lcs of  {x1,x2, ... ... ,xm-1} and {y1,y2, ... ... ,yn-1}
        /// if xm!=yn, the optimal subsequence is the max lcs of  {x1,x2, ... ... ,xm} and {y1,y2, ... ... ,yn-1} or 
        /// {x1,x2, ... ... ,xm-1} and {y1,y2, ... ... ,yn}
        /// 
        /// we memory all the result calculated, then to save a lot time
        /// it runs at O(mn), because at most we have O(mn) subproblem.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>if s1 and s2 are completely different, the result is String.Empty</returns>
        public static string LcsUnConsecutiveUsingTable(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return string.Empty;
            }

            if (s1 == s2)
            {
                return s1;
            }

            //memory all the result which we calculated
            Dictionary<SubSequence, string> calculated = new Dictionary<SubSequence, string>();

            return LcsUnConsecutiveUsingTable(s1, s1.Length - 1, s2, s2.Length - 1, ref calculated);
        }


        private static string LcsUnConsecutiveUsingTable(string s1, int s1End, string s2, int s2End,
                                                         ref Dictionary<SubSequence, string> calculated)
        {
            if (s1End < 0 || s2End < 0)
            {
                return string.Empty;
            }

            if (s1[s1End] == s2[s2End])
            {
                SubSequence sub = new SubSequence(s1End - 1, s2End - 1);

                if (calculated.ContainsKey(sub))
                {
                    //the sub problem is calculated
                    string result = calculated[sub] + s1[s1End];
                    // add current problem to table
                    calculated[new SubSequence(s1End, s2End)] = result;
                    return result;
                }
                else
                {
                    //the sub problem is not calculated
                    string result = LcsUnConsecutiveUsingTable(s1, s1End - 1, s2, s2End - 1, ref calculated);
                    //add sub problem to the table
                    calculated[sub] = result;
                    result += s1[s1End];
                    //add current problem to the table
                    calculated[new SubSequence(s1End, s2End)] = result;
                    return result;
                }
            }
            else
            {
                SubSequence sub1 = new SubSequence(s1End - 1, s2End);
                SubSequence sub2 = new SubSequence(s1End, s2End - 1);

                string t1 = calculated.ContainsKey(sub1) ? calculated[sub1] : LcsUnConsecutiveUsingTable(s1, s1End - 1, s2, s2End, ref calculated);
                string t2 = calculated.ContainsKey(sub2) ? calculated[sub2] : LcsUnConsecutiveUsingTable(s1, s1End, s2, s2End - 1, ref calculated);

                if (t1.Length > t2.Length)
                {
                    calculated[sub1] = t1;
                    calculated[new SubSequence(s1End, s2End)] = t1;
                    return t1;
                }
                else
                {
                    calculated[sub2] = t2;
                    calculated[new SubSequence(s1End, s2End)] = t2;
                    return t2;
                }
            }
        }

        /// <summary>
        /// mark a sub problem
        /// </summary>
        private class SubSequence : IEquatable<SubSequence>
        {
            public int S1End { get; set; }
            public int S2End { get; set; }

            public SubSequence()
            {
            }

            public SubSequence(int s1End, int s2End)
            {
                S1End = s1End;
                S2End = s2End;
            }

            public bool Equals(SubSequence other)
            {
                if (other == null)
                {
                    return false;
                }

                return S1End == other.S1End && S2End == other.S2End;
            }


            public override int GetHashCode()
            {
                return S1End * 7 + S2End * 13 + 11;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as SubSequence);
            }
        }
        #endregion


        #region

        /// <summary>
        /// using dynamic programming to find lcs of s1 and s2
        /// it runs at O(mn)
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static string Lcs(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return string.Empty;
            }

            if (s1 == s2)
            {
                return s1;
            }

            //matrix[i,j] store the length of lcs of s1[0,i] and s2[0,j]
            int[,] matrix = new int[s1.Length, s2.Length];
            Orientation[,] oris = new Orientation[s1.Length, s2.Length];
            for (int i = 0; i < s1.Length; i++)
            {
                for (int j = 0; j < s2.Length; j++)
                {
                    //the one in diagonal line before current one
                    if (s1[i] == s2[j])
                    {
                        matrix[i, j] = matrix.Item(i - 1, j - 1) + 1;
                        //this will be part of lcs
                        oris[i, j] = Orientation.LeftUp;
                    }
                    else if (matrix.Item(i - 1, j) >= matrix.Item(i, j - 1))
                    {
                        matrix[i, j] = matrix.Item(i - 1, j);
                        //move up
                        oris[i, j] = Orientation.Up;
                    }
                    else
                    {
                        matrix[i, j] = matrix.Item(i, j - 1);
                        //move left
                        oris[i, j] = Orientation.Left;
                    }
                }
            }


            //construct lcs
            int l1 = s1.Length - 1, l2 = s2.Length - 1;
            Stack<char> stack = new Stack<char>();
            while (l1 >= 0 && l2 >= 0)
            {
                switch (oris[l1, l2])
                {
                    case Orientation.LeftUp:
                        stack.Push(s1[l1]);
                        l1--;
                        l2--;
                        break;
                    case Orientation.Left:
                        l2--;
                        break;
                    case Orientation.Up:
                        l1--;
                        break;

                }
            }
            StringBuilder sb = new StringBuilder();

            while (stack.Count > 0)
            {
                sb.Append(stack.Pop());
            }

            return sb.ToString();
        }





        private static int Item(this int[,] matrix, int i, int j)
        {
            if (i < 0 || j < 0)
            {
                return 0;
            }

            return matrix[i, j];
        }

        private enum Orientation
        {
            None,
            Left,
            Up,
            LeftUp,
        }

        #endregion
    }
}
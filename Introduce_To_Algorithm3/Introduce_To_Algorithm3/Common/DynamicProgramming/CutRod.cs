using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.DynamicProgramming
{
    public class CutRod
    {
        /// <summary>
        /// it runs at O(2^n)
        /// </summary>
        /// <param name="p">p[i] is the price for rod of length i.if no price at position i, p[i] = 0</param>
        /// <param name="n">a rod of length n</param>
        /// <returns>cut a rod of length n and return the max revenue</returns>
        public static int Cut_Rod(int[] p,int n)
        {
            if (n <= 0) return 0;
            int max = int.MinValue;

            for(int i=1;i<=n;i++)
            {
                //r(n) = max(pi + r(n-i))
                //find the max of cut 0.....n-1
                max = System.Math.Max(max, Price(p, i) + Cut_Rod(p, n - i));
            }

            return max;
        }

        /// <summary>
        /// return price of rod whose length is i
        /// </summary>
        /// <param name="p"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int Price(int[] p,int i)
        {
            return p.Length>i ? p[i] : 0;
        }


        /// <summary>
        /// it runs in polynomial time O(n^2) because it remember all the calculated subproblems
        /// </summary>
        /// <param name="p">p[i] is the price for rod of length i.if no price at position i, p[i] = 0</param>
        /// <param name="n">a rod of length n</param>
        /// <returns>cut a rod of length n and return the max revenue</returns>
        public static int MemoizedCutRod(int[] p,int n)
        {
            //memory the result calculated
            int []r = new int[n + 1];
            for (int i = 0; i < r.Length; i++)
            {
                r[i] = int.MinValue;
            }

            return MemoizedCutRodAux(p, n, r);
        }

        private static int MemoizedCutRodAux(int[] p, int n, int[] r)
        {
            if(r[n]>=0)
            {
                return r[n];
            }

            int sum = 0;
            if(n==0)
            {
                sum = 0;
            }
            else
            {
                sum = int.MinValue;
                for (int i = 1; i <= n; i++)
                {
                    sum = System.Math.Max(sum, Price(p,i)+MemoizedCutRodAux(p, n - i, r));
                }
            }
            r[n] = sum;
            return sum;
        }


        /// <summary>
        /// it runs in polynomial time O(n^2) because it remember all the calculated subproblems
        /// </summary>
        /// <param name="p">p[i] is the price for rod of length i.if no price at position i, p[i] = 0</param>
        /// <param name="n">a rod of length n</param>
        /// <returns>cut a rod of length n and return the max revenue</returns>
        public static int BottomUpCutRod(int[] p,int n)
        {
            //memory the result calculated
            int[] r = new int[n + 1];
            for (int i = 1; i <= n; i++)
            {
                int sum = int.MinValue;
                for (int j = 1; j <= i; j++)
                {
                    sum = System.Math.Max(sum, Price(p, j) + r[i - j]);
                }
                r[i] = sum;
            }
            return r[n];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="n"></param>
        /// <param name="lists">complete list of piece sizes in an optimal decomposition of a rod of length n</param>
        /// <returns></returns>
        public static int ExtendedBottomUpCutRod(int[] p,int n,out List<int> lists)
        {
            int[] r = new int[n+1];
            int[] s = new int[n+1];
            for (int i = 1; i <= n; i++)
            {
                int sum = int.MinValue;
                for (int j = 1; j <= i; j++)
                {
                    if(sum<(Price(p, j) + r[i - j]))
                    {
                        sum = Price(p, j) + r[i - j];
                        s[i] = j;
                    }
                }
                r[i] = sum;
            }
            int tmp = n;
            lists = new List<int>();
            while (tmp>0)
            {
                lists.Add(s[tmp]);
                tmp = tmp - s[tmp];
            }
            return r[n];
        }
    }
}

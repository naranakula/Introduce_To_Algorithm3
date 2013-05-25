using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    /// <summary>
    /// Fibonacci number F0=0 F1=1 F2=1   F(n) = F(n-1) + F(n-2)
    /// </summary>
    public class Fibonacci
    {
        /// <summary>
        /// It runs at O((5^0.5 - 1)/2)）^n
        /// 
        /// 
        /// {{1,1},{1,0}}^n = {{Fn+1,Fn},{Fn,Fn-1}}
        /// So you can use divide and conquer to find a way to run at Lg(n)
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Fn(int n)
        {
            if (n == 1 || n == 2) return 1;
            return Fn(n - 2) + Fn(n - 1);
        }

        /// <summary>
        /// it runs at O(n)
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int FnLinear(int n)
        {
            int fn_1 = 1, fn_2 = 1;
            if (n == 1 || n == 2) return 1;
            int result = 1;
            for (int i = 3; i <= n; i++)
            {
                result = fn_1 + fn_2;
                fn_2 = fn_1;
                fn_1 = result;
            }
            return result;
        }
    }
}

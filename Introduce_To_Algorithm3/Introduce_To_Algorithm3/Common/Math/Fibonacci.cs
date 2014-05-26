using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static long Fn(int n)
        {
            Debug.Assert(n>=0);
            if (n == 0) return 0;
            if (n == 1 || n == 2) return 1;
            return Fn(n - 2) + Fn(n - 1);
        }

        /// <summary>
        /// it runs at O(n)
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long FnLinear(int n)
        {
            Debug.Assert(n >= 0);
            if (n == 0) return 0;
            if (n == 1 || n == 2) return 1;
            long result = 1,fn1 = 1, fn2 = 1;;
            for (int i = 3; i <= n; i++)
            {
                result = fn1 + fn2;
                fn1 = fn2;
                fn2 = result;
            }
            return result;
        }
    }
}

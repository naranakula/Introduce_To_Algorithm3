using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    public static class NumUtils
    {
        /// <summary>
        /// is num prime?
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsPrime(int num)
        {
            int sqrt = (int)System.Math.Sqrt(num);
            for (int i = 2; i <= sqrt; i++)
            {
                if(num%i == 0)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// is num prime
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsPrime(long num)
        {
            long sqrt = (long)System.Math.Sqrt(num);
            for (long i = 2; i <= sqrt; i++)
            {
                if (num % i == 0)
                {
                    return false;
                }
            }
            return true;
        }




        /// <summary>
        /// find great common divider
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static int Gcd(int num1, int num2)
        {
            //Trace.Assert(num1>=0 && num2>=0);
            if (num1 == 0 || num2 == 0)
            {
                return System.Math.Max(num1, num2);
            }

            return Gcd(num2, num1%num2);
        }

        /// <summary>
        /// find great common divier
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static long Gcd(long num1, long num2)
        {
            //Trace.Assert(num1 >= 0 && num2 >= 0);
            if (num1 == 0 || num2 == 0)
            {
                return System.Math.Max(num1, num2);
            }

            return Gcd(num2, num1%num2);
        }
    }
}

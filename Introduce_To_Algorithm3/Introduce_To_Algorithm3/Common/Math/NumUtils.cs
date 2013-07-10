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
            if (num < 2) return false;
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
            if (num < 2) return false;
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

        /// <summary>
        /// a factorial of n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long A(int n)
        {
            Trace.Assert(n>=0);
            long result = 1;
            for (int i = 1; i <=n; i++)
            {
                result *= i;
            }
            return result;
        }

        /// <summary>
        /// a combination
        /// </summary>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static long C(int n, int r)
        {
            //return  A(n)/(A(r)*A(n-r));
            long res = 1;
            for (int i = 1; i <= r; i++)
            {
                res *= n - i + 1;
            }
            for (int i = 1; i <= r; i++)
            {
                res /=i;
            }
            
            return res;
        }


        /// <summary>
        /// is a year leap year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(uint year)
        {
            return (year%4 == 0 && year%100 != 0) || (year%400 == 0);
        }

        /// <summary>
        /// are two double equal
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool Equals(this double d1, double d2)
        {
            return System.Math.Abs(d1 - d2) < 1E-12;
        }


        /// <summary>
        /// are two float equal
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public static bool Equals(this float f1, float f2)
        {
            return System.Math.Abs(f1 - f2) < 1E-10;
        }

    }
}
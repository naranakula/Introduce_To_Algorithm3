using System;
using System.Collections.Generic;
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
        public static  bool IsPrime(int num)
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
    }
}

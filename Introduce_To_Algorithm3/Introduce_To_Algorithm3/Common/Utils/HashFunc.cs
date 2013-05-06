using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class HashFunc
    {
        /// <summary>
        /// hash o to [0,size-1]
        /// </summary>
        /// <param name="o"></param>
        /// <param name="size">if the size is prime, the result is better</param>
        /// <returns></returns>
        public int HashReminder(object o,int size)
        {
            return System.Math.Abs(o.GetHashCode())%size;
        }

        /// <summary>
        /// hash o to [0,size-1]
        /// </summary>
        /// <param name="o"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public int HashMultiply(object o,int size)
        {
            double d = ((System.Math.Sqrt(5) - 1)/2*o.GetHashCode());
            d = d - (int) d;
            return (int)(d*size);
        }
    }


    public class UniversalHash
    {
        private int size;
        private int a, b, p;
        private int[] primes = { 1000003, 1000033, 1000037, 1000039, 1000081, 1000099, 1000117, 1000121, 1000133, 1000151, 1000159, 1000171, 1000183, 1000187, 1000193, 1000199, 1000211, 1000213, 1000231, 1000249, 1000253, 1000273, 1000289, 1000291, 1000303, 1000313, 1000333, 1000357, 1000367, 1000381, 1000393, 1000397, 1000403, 1000409, 1000423, 1000427, 1000429, 1000453, 1000457, 1000507, 1000537, 1000541, 1000547, 1000577, 1000579, 1000589, 1000609, 1000619, 1000621, 1000639, 1000651, 1000667, 1000669, 1000679, 1000691, 1000697, 1000721, 1000723, 1000763, 1000777, 1000793, 1000829, 1000847, 1000849, 1000859, 1000861, 1000889, 1000907, 1000919, 1000921, 1000931, 1000969, 1000973, 1000981, 1000999, 1001003, 1001017, 1001023, 1001027, 1001041, 1001069, 1001081, 1001087, 1001089, 1001093, 1001107, 1001123, 1001153, 1001159, 1001173, 1001177, 1001191, 1001197, 1001219, 1001237, 1001267, 1001279, 1001291, 1001303, 1001311, };
        /// <summary>
        /// map object to [0,size-1]
        /// </summary>
        /// <param name="size"></param>
        public UniversalHash(int size)
        {
            this.size = size;
            Random rand = new Random();
            p = primes[rand.Next(primes.Length)];
            a = rand.Next(1, p);
            b = rand.Next(p);
        }

        
        /// <summary>
        /// h(k) = (a*o.gethashcode+b)mod P mode size
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int GetHashCode(object o)
        {
            return (a*o.GetHashCode()+b)%p%size;
        }
    }
}

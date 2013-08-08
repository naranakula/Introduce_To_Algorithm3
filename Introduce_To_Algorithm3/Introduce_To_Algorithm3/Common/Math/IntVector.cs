using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    public class IntVector:Vector<int>
    {
        public IntVector(int size) : base(size)
        {
        }

        /// <summary>
        /// calculate the inner product
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public virtual int InnerProduct(IntVector vector)
        {
            if (Count != vector.Count)
            {
                throw new Exception("vector must have the same dimens");
            }

            int result = 0;
            for (int i = 0; i < Count; i++)
            {
                result += Get(i)*vector.Get(i);
            }

            return result;
        }


        public virtual int[,] OuterProduct(IntVector vector)
        {
            if (Count != vector.Count)
            {
                throw new Exception("vector must have the same dimens");
            }

            int[,] result = new int[Count,Count];
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Count; j++)
                {
                    result[i, j] = Get(i)*vector.Get(j);
                }
            }

            return result;
        }

        public virtual double EuclideanNorm()
        {
            double result = 0;
            for (int i = 0; i < Count; i++)
            {
                result += Get(i)*Get(i);
            }

            return System.Math.Sqrt(result);
        }



    }
}

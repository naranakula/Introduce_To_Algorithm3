using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Math
{
    /// <summary>
    /// a vector of double
    /// </summary>
    public class DoubleVector:Vector<double>
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="size"></param>
        public DoubleVector(int size) : base(size)
        {
        }

        public DoubleVector(double[] array) : base(array.Length)
        {
            for (int i = 0; i < array.Length; i++)
            {
                this[i] = array[i];
            }
        }

        public double[] ToArray()
        {
            return this._vector;
        }


        /// <summary>
        /// calculate the inner product
        /// a = [a1, a2,…, an] and b = [b1, b2,…, bn]
        /// a*b = b*a = a1*b1 + a2*b2 +......+an*bn
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public virtual double InnerProduct(DoubleVector vector)
        {
            if (Count != vector.Count)
            {
                throw new Exception("vector must have the same dimens");
            }

            double result = 0;

            for (int i = 0; i < Count; i++)
            {
                result += Get(i) * vector.Get(i);
            }

            return result;
        }


        public virtual double[,] OuterProduct(DoubleVector vector)
        {
            if (Count != vector.Count)
            {
                throw new Exception("vector must have the same dimens");
            }

            Double[,] result = new double[Count, Count];
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Count; j++)
                {
                    result[i, j] = Get(i) * vector.Get(j);
                }
            }

            return result;
        }

        public virtual double EuclideanNorm()
        {
            double result = 0;

            for (int i = 0; i < Count; i++)
            {
                result += Get(i) * Get(i);
            }

            return System.Math.Sqrt(result);
        }

        /// <summary>
        /// sqrt(    (x1-x2)^2 + (y1-y2)^2 + (z1-z2)^2 + .....     )
        /// </summary>
        /// <param name="intVector"></param>
        /// <returns></returns>
        public virtual double DistanceTo(DoubleVector inVector)
        {
            double square = 0;

            for (int i = 0; i < Count; i++)
            {
                square += System.Math.Pow((this[i] - inVector[i]), 2);
            }

            return System.Math.Sqrt(square);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    /// <summary>
    /// find the max sub array.
    /// the array should be contains both positive and negative numbers.
    /// If the array item are all negative or zero,the result returned is 0.
    /// </summary>
    public class MaxSubArray
    {
        /// <summary>
        /// brute force to get the result . it runs at O(n^2)
        /// if beginIndex or endIndex is negative, the array are all negative
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int MaxSubArrayByBruteForce(int[] array)
        {
            int beginIndex, endIndex;
            return MaxSubArrayByBruteForce(array, out beginIndex, out endIndex);
        }

        /// <summary>
        /// brute force to get the result . it runs at O(n^2).
        /// the maxSubArray at[beginIndex,endIndex].
        /// </summary>
        /// <param name="array"></param>
        /// <param name="beginIndex">if beginIndex or endIndex is negative, the array are all negative</param>
        /// <param name="endIndex"></param>
        /// <param name="minCount"> true then the differece between endIndex and beginIndex must be smallest  </param>
        /// <returns></returns>
        public static int MaxSubArrayByBruteForce(int[] array, out int beginIndex, out int endIndex, bool minCount = false)
        {
            if (array == null || array.Length == 0)
            {
                throw new Exception("array shouldn't be null or empty");
            }

            beginIndex = -10;
            //init beginIndex and endIndex negative, but they should not be same
            endIndex = -1;
            int result = 0;
            for (int i = 0; i < array.Length; i++)
            {
                int sum = 0;
                for (int j = i; j < array.Length; j++)
                {
                    sum += array[j];
                    if (sum > result)
                    {
                        result = sum;
                        beginIndex = i;
                        endIndex = j;
                    }
                    //update endindex and beginindex if you find a smaller count items.
                    if (sum == result && minCount && (j - i) < (endIndex - beginIndex)/*this is the reason why beginIndex and endIndex should not be init same negative */)
                    {
                        beginIndex = i;
                        endIndex = j;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// divide the arr in to two part. [lo,mid) mid (mid+1,hi].
        /// the max subarray at [lo,mid) (mid+1,hi] or contains mid
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int MaxSubArrayByDivideAndConquer(int[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new Exception("array shouldn't be null or empty");
            }
            return MaxSubArrayByDivideAndConquer(array, 0, array.Length - 1);
        }

        /// <summary>
        /// divide the arr in to two part. [lo,mid] across mid (mid+1,hi].
        /// the max subarray at [lo,mid] (mid+1,hi] or across mid
        /// if beginIndex or endIndex is negative, the array are all negative
        /// </summary>
        /// <param name="array"></param>
        /// <param name="hi"> </param>
        /// <param name="lo"> </param>
        /// <returns></returns>
        private static int MaxSubArrayByDivideAndConquer(int[] array, int lo, int hi)
        {
            if (lo > hi)
            {
                return 0;
            }
            if (lo == hi)
            {
                if (array[lo] < 0)
                {
                    return 0;
                }
                else
                {
                    return array[lo];
                }
            }

            int mid = (lo + hi) / 2;
            int leftMax = MaxSubArrayByDivideAndConquer(array, lo, mid);
            int rightMax = MaxSubArrayByDivideAndConquer(array, mid + 1, hi);
            int crossMax = FindMaxCrossSubArray(array, lo, mid, hi);
            int max = System.Math.Max(leftMax, rightMax);
            max = System.Math.Max(crossMax, max);
            return max;
        }


        /// <summary>
        /// it returns a tuple containing the indices demarcating a maximum subarray that crosses the midpoint,
        /// </summary>
        /// <param name="array"></param>
        /// <param name="lo"></param>
        /// <param name="mid"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        private static int FindMaxCrossSubArray(int[] array, int lo, int mid, int hi)
        {
            int lSum = 0, rSum = 0;
            int sum = 0;
            for (int i = mid; i >= lo; i--)
            {
                sum += array[i];
                if (lSum < sum)
                {
                    lSum = sum;
                }
            }

            sum = 0;
            for (int i = mid + 1; i <= hi; i++)
            {
                sum += array[i];
                if (rSum < sum)
                {
                    rSum = sum;
                }
            }
            return lSum + rSum;
        }


        /// <summary>
        /// find maxsubarray quickly in O(n).
        /// Determine a maximum subarray of the for j+1 in constant time based on knowing a maximum subarray ending at index j .
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int MaxSubArrayQuick(int[] array)
        {
            int beginIndex, endIndex;
            return MaxSubArrayQuick(array, out beginIndex, out endIndex);
        }

        /// <summary>
        /// find maxsubarray quickly in O(n).
        /// Determine a maximum subarray of the for j+1 in constant time based on knowing a maximum subarray ending at index j .
        /// </summary>
        /// <param name="array"></param>
        /// <param name="beginIndex"> </param>
        /// <param name="endIndex"> </param>
        /// <returns></returns>
        public static int MaxSubArrayQuick(int[] array, out int beginIndex, out int endIndex)
        {
            endIndex = -1;
            beginIndex = 0;
            if (array == null || array.Length == 0)
            {
                throw new Exception("array shouldn't be null or empty");
            }

            int tempBeginIndex = -1;

            int sum = 0, maxSum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
                if(sum>maxSum)
                {
                    maxSum = sum;
                    endIndex = i;
                    if (endIndex >= tempBeginIndex)
                    {
                        beginIndex = tempBeginIndex;
                    }
                }
                if(sum<=0)
                {
                    sum = 0;
                    tempBeginIndex = i + 1;
                }
            }
            return maxSum;
        }
    }
}

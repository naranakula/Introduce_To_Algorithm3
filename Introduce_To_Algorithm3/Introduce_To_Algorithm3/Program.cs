using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Search;
using Introduce_To_Algorithm3.Common.Sort;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            int len = rand.Next(0, 10);
            int[] arr = new int[len];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rand.Next(10);
                Console.Write(arr[i]+"  ");
            }
            Console.WriteLine();
            arr = InsertionSort<int>.Sort(arr);
            if (!IsSorted(arr))
            {
                throw new Exception("array is not sorted");
            }
            arr.ToList().ForEach(Console.WriteLine);
            Console.WriteLine();
            Console.WriteLine(BinarySearch<int>.Search(arr, 7));
        }



        /// <summary>
        /// decide whether a array is sorted between [lo,hi]
        /// it sorted if the array is empty or just has one element
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static bool IsSorted(int[] arr)
        {
            if (arr == null || arr.Length <= 1)
            {
                return true;
            }

            bool isSorted = true;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i].CompareTo(arr[i + 1]) > 0)
                {
                    isSorted = false;
                    break;
                }
            }
            return isSorted;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GreedyAlgorithm
{
    /// <summary>
    /// 0-1 knapsack problem:
    /// a thief robbed a store and found n itmes. each item values vi dollars and weights wi pounds. but the thief can take at most K pounds.
    /// How many valus can he take at most.
    /// 
    /// fractional knapsack problem like 0-1 knapsack problem but you can take part of an item.
    /// fractional kanpsack problem can use greedy algorithm to solve(choose the one has max vi/wi), but 0-1 knapsack problem cann't
    /// </summary>
    public class Knapsack
    {
        #region fractional kanpsack

        /// <summary>
        /// using greedy algorithm
        /// </summary>
        /// <param name="items"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static double FractionKanpsack(List<KnapsackItem> items, double limit)
        {
            if (items == null || items.Count <= 0 || limit <= 0)
            {
                return 0;
            }

            KnapsackItem[] arr = items.OrderBy(i => i.Value/i.Weight).ToArray();
            return FractionKanpsack(arr, arr.Length - 1, limit);
        }


        private static double FractionKanpsack(KnapsackItem[] items, int end, double limit)
        {
            if (end < 0 || items == null || limit<=0)
            {
                return 0.0;
            }

            if (items[end].Weight > limit)
            {
                //you can not get the whole item,just part of
                return limit*items[end].Value/items[end].Weight;
            }
            else
            {
                return items[end].Value + FractionKanpsack(items, end - 1, limit - items[end].Weight);
            }
        }

        #endregion


        #region 0-1 Kanpsack

        /// <summary>
        /// using dynamic programing
        /// </summary>
        /// <param name="items"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static double DpKanpsack(List<KnapsackItem> items, double limit)
        {
            if (items == null || items.Count <= 0 || limit <= 0)
            {
                return 0;
            }

            List<KnapsackItem> arr = items.OrderBy(i => i.Value / i.Weight).Where(i=>i.Weight<=limit).ToList();
            if (arr.Count <= 0)
            {
                return 0;
            }
            double max = 0.0;
            for (int i = 0; i < arr.Count; i++)
            {
                double result = arr[i].Value + DpKanpsack(arr.Where((item,j)=>i!=j).ToList(), limit - arr[i].Value);
                if (max < result)
                {
                    max = result;
                }
            }

            return max;
        }


        #endregion
    }



    public class KnapsackItem
    {
        public double Value { get; set; }
        /// <summary>
        /// weight must be greater than 0
        /// </summary>
        public double Weight { get; set; }
    }
}

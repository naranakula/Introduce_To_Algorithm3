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

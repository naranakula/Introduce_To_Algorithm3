using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.DynamicProgramming;
using Introduce_To_Algorithm3.Common.GreedyAlgorithm;
using Introduce_To_Algorithm3.Common.Math;
using Introduce_To_Algorithm3.Common.Search;
using Introduce_To_Algorithm3.Common.Sort;
using Introduce_To_Algorithm3.Common.Structs;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Activity> testAcs = new List<Activity>();
            for (int i = 0; i < 10; i++)
            {
                testAcs.Add(new Activity() { BeginTime = DateTime.Now.AddHours(i), FinishTime = DateTime.Now.AddHours(i+1) });
                testAcs.Add(new Activity() { BeginTime = DateTime.Now.AddHours(i), FinishTime = DateTime.Now.AddHours(i + 1) });
            }
            testAcs.Add(new Activity(){BeginTime = DateTime.Now,FinishTime = DateTime.Now.AddHours(1)});
            List<Activity> temp = new List<Activity>();
            temp = ActivitySelection.DPSelection(testAcs);
            temp = ActivitySelection.GreedySelection(testAcs);
            Console.ReadKey();
        }
    }
}

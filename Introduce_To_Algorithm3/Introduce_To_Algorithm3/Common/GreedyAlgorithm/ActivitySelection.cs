using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GreedyAlgorithm
{
    /// <summary>
    /// we have activity set {a1,a2,.....,an},each activity ai has a start time si and a finish time fi.
    /// In activity selection problem, we wish to select a maximum-size subset of mutually compatible activities.
    /// Activity ai and aj are compatible if [si,fi) and [sj,fj) do not overlap
    /// </summary>
    public class ActivitySelection
    {
        #region dynamic programming solution
        /// <summary>
        /// Using dynamic programming to solve AC.
        /// Find a substruct,if ak in the final result,then we divide problem into two part, find the optimal solution from {am} where all  the am's finish time &lt;= ak's start time.  {an} where an's start time &gt= ak's finish time 
        /// 
        /// if u want better performance, memory all the result calculted.
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        public static List<Activity> DPSelection(List<Activity> activities)
        {
            if (activities == null)
            {
                return new List<Activity>();
            }
            if (activities.Count <= 1)
            {
                List<Activity> results = new List<Activity>();
                results.AddRange(activities);
                return results;
            }
            
            List<Activity> maxResult = new List<Activity>();
            for (int i = 0; i < activities.Count; i++)
            {
                //find all the activities whose finish time <= current's starttime
                List<Activity> leftAcs = new List<Activity>();
                leftAcs.AddRange(from r in activities where (r.FinishTime<= activities[i].BeginTime && r != activities[i]) select  r);

                //find all the activities whose start time >= current's finish time
                List<Activity> rightAcs = new List<Activity>();
                rightAcs.AddRange(from r in activities where (r.BeginTime>=activities[i].FinishTime && r!=activities[i]) select  r);

                List<Activity> leftResults = DPSelection(leftAcs);
                List<Activity> rightResults = DPSelection(rightAcs);
                List<Activity> result = new List<Activity>();
                result.AddRange(leftResults);
                result.Add(activities[i]);
                result.AddRange(rightResults);
                if (result.Count > maxResult.Count)
                {
                    maxResult = result;
                }
            }

            return maxResult;
        }
        #endregion

        #region greedy

        /// <summary>
        /// using greedy algorithm as solution
        /// first sort all activities by finish time. select an activity each time and leave as much as resources for the rest activity
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        public static List<Activity> GreedySelection(List<Activity> activities)
        {
            if (activities == null)
            {
                return new List<Activity>();
            }
            if (activities.Count <= 1)
            {
                List<Activity> results = new List<Activity>();
                results.AddRange(activities);
                return results;
            }

            List<Activity> result = new List<Activity>();
            activities.Sort();
            for (int i = 0; i < activities.Count; i++)
            {
                if (result.Count <= 0)
                {
                    result.Add(activities[i]);
                }
                else
                {
                    if (!Activity.Overlap(activities[i], result.Last()))
                    {
                        result.Add(activities[i]);
                    }
                }
            }
            return result;
        }


        #endregion

    }


    /// <summary>
    /// represent a Activity
    /// </summary>
    public class Activity:IComparable<Activity>
    {
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// FinishTime > beginTime
        /// </summary>
        public DateTime FinishTime { get; set; }


        public static bool Overlap(Activity activity1, Activity activity2)
        {
            return (activity1.FinishTime > activity2.BeginTime && activity1.BeginTime < activity2.FinishTime);
        }

        public static bool Compatiable(Activity activity1, Activity activity2)
        {
            return !Overlap(activity1, activity2);
        }

        public int CompareTo(Activity other)
        {
            return FinishTime.CompareTo(other.FinishTime);
        }
    }
}

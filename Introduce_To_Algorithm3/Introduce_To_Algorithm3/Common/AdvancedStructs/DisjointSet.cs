using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    /// <summary>
    /// some application involve grouping n distinct elements into a collection of disjoint sets
    /// usually need to perform two operation: finding the unique set that contains a given element and uniting two sets
    /// </summary>
    public static class DisjointSet
    {

        #region find component for graph

        /// <summary>
        /// find component
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static List<HashSet<int>> ConnectedComponent(this Graph.Graph graph)
        {
            if (graph == null) return new List<HashSet<int>>();

            //
            List<HashSet<int>> lists = new List<HashSet<int>>();
            for (int i = 0; i < graph.V; i++)
            {
                HashSet<int> set = new HashSet<int>();
                set.Add(i);
                lists.Add(set);
            }

            List<Tuple<int, int>> tuples = new List<Tuple<int, int>>();
            for (int i = 0; i < graph.V; i++)
            {
                foreach (int j in graph.Adj(i))
                {
                    bool found = false;
                    foreach (var t in tuples)
                    {
                        if ((t.Item1 == i && t.Item2 == j) || (t.Item1 == j && t.Item2 == i))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        tuples.Add(new Tuple<int, int>(i, j));
                }
            }

            foreach (var tuple in tuples)
            {
                HashSet<int> set1 = null;
                HashSet<int> set2 = null;
                foreach (var list in lists)
                {
                    if (list.Contains(tuple.Item1))
                        set1 = list;
                    if (list.Contains(tuple.Item2))
                        set2 = list;
                    if (set1 != null && set2 != null)
                        break;
                }

                if (set1 == set2)
                {
                    continue;
                }

                lists.Remove(set1);
                lists.Remove(set2);
                set1.UnionWith(set2);
                lists.Add(set1);
            }

            return lists;
        }

        /// <summary>
        /// find component
        /// 
        /// it runs at O(n^2)
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static List<HashSet<int>> ConnectedComponent(List<Tuple<int, int>> tuples)
        {
            HashSet<int> sets = new HashSet<int>();
            foreach (var tuple in tuples)
            {
                sets.Add(tuple.Item1);
                sets.Add(tuple.Item2);
            }

            List<HashSet<int>> lists = new List<HashSet<int>>();
            foreach (int s in sets)
            {
                HashSet<int> set1 = new HashSet<int>();
                set1.Add(s);
                lists.Add(set1);
            }


            foreach (var tuple in tuples)
            {
                HashSet<int> set1 = null;
                HashSet<int> set2 = null;
                foreach (var list in lists)
                {
                    if (list.Contains(tuple.Item1))
                        set1 = list;
                    if (list.Contains(tuple.Item2))
                        set2 = list;
                    if (set1 != null && set2 != null)
                        break;
                }

                if (set1 == set2)
                {
                    continue;
                }

                lists.Remove(set1);
                lists.Remove(set2);
                set1.UnionWith(set2);
                lists.Add(set1);
            }

            return lists;
        }
        #endregion

        #region disjoint set forest



        #endregion

    }
}

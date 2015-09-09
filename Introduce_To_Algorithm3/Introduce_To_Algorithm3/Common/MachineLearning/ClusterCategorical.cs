using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// 聚簇分类
    /// 具体思想是：每个分组先分配一个种子数据，对于剩下的数据项，确定分配到哪一个分组产生最大的CU,然后分配到该分组。
    /// 好的方法是选择尽可能互不相同的 m 个元组作为种子元组。 这里就又该是分类效用发挥作用了，可以计算所有可能的候选种子元组集的 CU，使用具有最佳 CU（最大值表示最不相同）的元组作为种子元组。 如上所述，一般不可能检查每个可能的种子元组集，因此我们重复选择 m 个随机元组，计算这些随机元组的 CU，并使用具有最佳 CU 的元组集作为种子。

    /// </summary>
    public class ClusterCategorical
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numClusters"></param>
        /// <param name="rawData"></param>
        public ClusterCategorical(int numClusters, string[][] rawData)
        {
            
        }

        private int[] Cluster(int numRestarts, out double cu)
        {
            throw new NotImplementedException();
        }

        public static void TestMain()
        {
            Console.WriteLine("Begin categorical data clustering demo");

            string[][] rawData = new string[7][];
            rawData[0] = new string[] { "Blue", "Small", "False" };
            rawData[1] = new string[] { "Green", "Medium", "True" };
            rawData[2] = new string[] { "Red", "Large", "False" };
            rawData[3] = new string[] { "Red", "Small", "True" };
            rawData[4] = new string[] { "Green", "Medium", "False" };
            rawData[5] = new string[] { "Yellow", "Medium", "False" };
            rawData[6] = new string[] { "Red", "Large", "False" };
            Console.WriteLine("Raw unclustered data: ");
            Console.WriteLine(" Color Size Heavy");
            Console.WriteLine("-----------------------------");

            ShowData(rawData);

            int numClusters = 2;
            Console.WriteLine("Setting numClusters to "+numClusters);
            int numRestarts = 4;
            Console.WriteLine("Setting numRestarts to "+numRestarts);

            Console.WriteLine("Starting clustering using greedy CU algorithm");

            ClusterCategorical cc = new ClusterCategorical(numClusters,rawData);
            double cu;
            int[] clustering = cc.Cluster(numRestarts, out cu);
            Console.WriteLine("Clustering complete");
            Console.WriteLine("Final clustering in internal form:");
            ShowVector(clustering, true);
            Console.WriteLine("Final CU value = "+cu.ToString("F4"));

            Console.WriteLine("Raw data grouped by cluster:");
            ShowClustering(numClusters, clustering, rawData);
            Console.WriteLine("end categorical data");
        }

        private static void ShowClustering(int numClusters, int[] clustering, string[][] rawData)
        {
            throw new NotImplementedException();
        }

        private static void ShowVector(int[] clustering, bool p)
        {
            throw new NotImplementedException();
        }

        

        private static void ShowData(string[][] rawData)
        {
        }
    }
}

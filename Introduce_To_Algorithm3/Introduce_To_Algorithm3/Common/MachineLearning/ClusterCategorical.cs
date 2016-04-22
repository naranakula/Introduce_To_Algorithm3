using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// 详细见书：machine learning using C# succinctly
    /// 聚簇分类
    /// 具体思想是：每个分组先分配一个种子数据，对于剩下的数据项，确定分配到哪一个分组产生最大的CU,然后分配到该分组。
    /// 好的方法是选择尽可能互不相同的 m 个元组作为种子元组。 这里就又该是分类效用发挥作用了，可以计算所有可能的候选种子元组集的 CU，使用具有最佳 CU（最大值表示最不相同）的元组作为种子元组。 如上所述，一般不可能检查每个可能的种子元组集，因此我们重复选择 m 个随机元组，计算这些随机元组应用算法产生的 CU，并使用具有最佳 CU 的分组作为最后分组。
    /// 最后的分组是有初始的种子和检查的顺序决定

    /// </summary>
    public class ClusterCategorical
    {
        /// <summary>
        /// 分组的数目
        /// </summary>
        private int numClusters;

        /// <summary>
        /// 分组 clusterings[i]=j 表示第i个元素的分组是j组
        /// </summary>
        private int[] clusterings;

        /// <summary>
        /// dataAsInts[i]表示第i个元素  dataAsInts[i][j]=m 表示第i个元素的第j个属性的属性值是m
        /// </summary>
        private int[][] dataAsInts;

        /// <summary>
        /// 原生数据
        /// </summary>
        private string[][] rawDatas;

        /// <summary>
        /// valueCounts[i][j][k]=m i表示属性的分类  j表示属性的值  k表示分组  m表示该分组中的属性的值的个数
        /// </summary>
        private int[][][] valueCounts;

        /// <summary>
        /// clusterCounts[i]=j  表示i个分组的元素的个数是j
        /// </summary>
        private int[] clusterCounts;

        /// <summary>
        /// 随机数
        /// </summary>
        private Random rnd;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numClusters">分组数目</param>
        /// <param name="rawData">rowData表示要分组的原生数据</param>
        public ClusterCategorical(int numClusters, string[][] rawData)
        {
            this.numClusters = numClusters;
            this.rawDatas = rawData;
            if (numClusters<=1)
            {
                throw  new Exception("分组数不能小于1");
            }

            if (rawData == null || numClusters > rawData.Length)
            {
                throw new Exception("分组大于要元素数目");
            }
            
            //表示转换
            MakeDataMatrix(rawData);
            //分配数据结构
            Allocate();
        }

        /// <summary>
        /// 执行算法的主要部分
        /// </summary>
        /// <param name="numRestarts">重试次数</param>
        /// <param name="cu"></param>
        /// <returns></returns>
        public int[] Cluster(int numRestarts, out double cu)
        {
            int numRows = dataAsInts.Length;
            double currCU=0, bestCU = -1;
            int[] bestClustering = new int[numRows];
            for (int i = 0; i < numRestarts; i++)
            {
                //为了保持每次运行的结果一致，实际情况下应该使用更随机的，如当前时间
                //int seed = i + i*i;
                int seed = i;//use i as rnd seed
                int[] currClustrings = ClusterOnce(seed, out currCU);
                if (currCU > bestCU)
                {
                    bestCU = currCU;
                    Array.Copy(currClustrings,bestClustering,numRows);
                }
            }
            cu = bestCU;
            return bestClustering;
        }

        /// <summary>
        /// 执行算法一次
        /// </summary>
        /// <param name="seed">随机数的种子</param>
        /// <param name="catUtility"></param>
        /// <returns></returns>
        private int[] ClusterOnce(int seed, out double catUtility)
        {
            this.rnd = new Random(seed);
            //this.rnd = new Random();//更好的方法是使用时间相关的默认种子

            //初始化
            Initialize();

            //选择初始的分组种子
            int numTrials = dataAsInts.Length;
            int[] goodIndexes = GetGoodIndices(numTrials);
            for (int i = 0; i < numClusters; i++)
            {
                //将初始的分组种子赋给每个分组一个
                Assign(goodIndexes[i],i);
            }

            int numRows = dataAsInts.Length;
            //随机重排待分组元素
            int[] rndSequence = new int[numRows];
            for (int i = 0; i < numRows; i++)
            {
                rndSequence[i] = i;
            }

            
            Shuffle(rndSequence);

            //算法的核心：
            /*
             * 接下来检测每个未被份组的元素，将其放到各个分组
             * 判断每次产生的cu，最大的cu的分组选为最终的分组
             * 
             */
            for (int i = 0; i < numRows; i++)
            {
                int idx = rndSequence[i];//选择要处理的元素
                if (clusterings[idx] >= 0)
                {
                    //已经分组的元素
                    continue;
                }

                double[] candidateCU = new double[numClusters];
                for (int j = 0; j < numClusters; j++)
                {
                    //判断每次放置，新分组产生的cu
                    Assign(idx,j);
                    candidateCU[j] = CategoryUtility();//计算cu
                    Unassign(idx,j);//undo assign
                }

                //获取最好的cu的分组
                int bestCluster = MaxIndex(candidateCU);
                Assign(idx,bestCluster);
            }

            //获取最后的cu
            catUtility = CategoryUtility();
            int[] result = new int[numRows];
            Array.Copy(this.clusterings,result,numRows);
            return result;
        }

        /// <summary>
        /// 将字符串表示的原生数据，转换为整数表示
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private int[][] MakeDataMatrix(string[][] rawData)
        {
            int numRows = rawData.Length;
            int numCols = rawData[0].Length;

            this.dataAsInts = new int[numRows][];

            for (int i = 0; i < numRows; i++)
            {
                dataAsInts[i] = new int[numCols];
            }

            for (int col = 0; col < numCols; col++)
            {
                int idx = 0;
                Dictionary<String,int> dict = new Dictionary<string, int>();

                for (int row = 0; row < numRows; row++)
                {
                    string s = rawData[row][col].Trim().ToLower();
                    //计算每个属性值的int，每个属性类别从0开始
                    if (!dict.ContainsKey(s))
                    {
                        dict.Add(s,idx);
                        idx++;
                    }
                }

                for (int row = 0; row < numRows; row++)
                {
                    string s = rawData[row][col].Trim().ToLower();
                    int v = dict[s];
                    this.dataAsInts[row][col] = v;
                }

            }

            return dataAsInts;
        }

        /// <summary>
        /// 为本算法使用的数据结构分配内存空间
        /// </summary>
        private void Allocate()
        {
            int numRows = dataAsInts.Length;
            int numCols = dataAsInts[0].Length;

            this.clusterings = new int[numRows];
            //最后一个存放所有的元素的数目
            this.clusterCounts = new int[numClusters+1];

            this.valueCounts = new int[numCols][][];
            for (int col = 0; col < numCols; col++)
            {
                int maxVal = 0;
                //获取列数-1
                for (int i = 0; i < numRows; i++)
                {
                    if (dataAsInts[i][col]>maxVal)
                    {
                        maxVal = dataAsInts[i][col];
                    }
                }

                this.valueCounts[col] = new int[maxVal+1][];
            }

            for (int i = 0; i < valueCounts.Length; i++)
            {
                for (int j = 0; j < valueCounts[i].Length; j++)
                {
                    //最后一个存放所有分组中的的属性分类i的属性值j的和
                    this.valueCounts[i][j] = new int[numClusters+1];
                }
            }

        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            for (int i = 0; i < clusterings.Length; i++)
            {
                clusterings[i] = -1;
            }

            for (int i = 0; i < clusterCounts.Length; i++)
            {
                clusterCounts[i] = 0;
            }

            for (int i = 0; i < valueCounts.Length; i++)
            {
                for (int j = 0; j < valueCounts[i].Length; j++)
                {
                    for (int k = 0; k < valueCounts[i][j].Length; k++)
                    {
                        valueCounts[i][j][k] = 0;
                    }
                }
            }

        }

        /// <summary>
        /// 计算cu
        /// 最难的部分
        /// </summary>
        /// <returns></returns>
        private double CategoryUtility()
        {
            //获取共有多少个元素
            int numTuplesAssigned = clusterCounts[clusterCounts.Length - 1];
            //每个分组的概率
            double[] clusterProbs = new double[this.numClusters];

            for (int i = 0; i < numClusters; i++)
            {
                clusterProbs[i] = clusterCounts[i]*1.0/numTuplesAssigned;
            }

            //计算无条件概率，方程式右边项
            double unconditional = 0.0;//保存无条件概率
            for (int i = 0; i < valueCounts.Length; i++)
            {
                for (int j = 0; j < valueCounts[i].Length; j++)
                {
                    //last cell holds sum
                    int sum = valueCounts[i][j][numClusters];
                    double p = (sum*1.0)/numTuplesAssigned;
                    unconditional += p*p;
                }
            }

            //numCluster个条件概率
            double[] conditionals = new double[numClusters];
            for (int k = 0; k < numClusters; k++)
            {
                //每个属性种类
                for (int i = 0; i < valueCounts.Length; i++)
                {
                    //每个属性值
                    for (int j = 0; j < valueCounts[i].Length; j++)
                    {
                        //条件概率
                        double p = (valueCounts[i][j][k]*1.0)/clusterCounts[k];
                        conditionals[k] += (p*p);
                    }
                }
            }

            //大括号的差
            double summation = 0.0;
            for (int i = 0; i < numClusters; i++)
            {
                summation += clusterProbs[i]*(conditionals[i]-unconditional);
            }

            return summation/numClusters;
        }

        /// <summary>
        /// 返回最好的cu所在的分组索引
        /// </summary>
        /// <param name="cus"></param>
        /// <returns></returns>
        private static int MaxIndex(double[] cus)
        {
            double bestCU = 0.0;
            int indexOfBestCU = 0;
            for (int i = 0; i < cus.Length; i++)
            {
                if (cus[i]>bestCU)
                {
                    indexOfBestCU = i;
                    bestCU = cus[i];
                }
            }

            return indexOfBestCU;
        }

        /// <summary>
        /// 重排元素
        /// </summary>
        /// <param name="indices"></param>
        private void Shuffle(int[] indices)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                int ri = rnd.Next(i, indices.Length);
                int tmp = indices[i];
                indices[i] = indices[ri];
                indices[ri] = tmp;
            }
        }

        /// <summary>
        /// 将元素dataIndex放到分组clusterId
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <param name="clusterId"></param>
        private void Assign(int dataIndex, int clusterId)
        {
            clusterings[dataIndex] = clusterId;

            //更新valueCounts clusterCounts
            for (int i = 0; i < valueCounts.Length; i++)
            {
                int v = dataAsInts[dataIndex][i];
                ++valueCounts[i][v][clusterId];
                ++valueCounts[i][v][numClusters];
            }

            ++clusterCounts[clusterId];
            ++clusterCounts[numClusters];
        }

        /// <summary>
        /// 将元素dataIndex从分组clusterId中剔除
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <param name="clusterId"></param>
        private void Unassign(int dataIndex, int clusterId)
        {
            clusterings[dataIndex] = -1;
            //更新valueCounts clusterCounts
            for (int i = 0; i < valueCounts.Length; i++)
            {
                int v = dataAsInts[dataIndex][i];
                --valueCounts[i][v][clusterId];
                --valueCounts[i][v][numClusters];
            }

            --clusterCounts[clusterId];
            --clusterCounts[numClusters];

        }

        /// <summary>
        /// 获取好的初始种子
        /// 首先选出来的种子必须不同
        /// 第二种子之间的差异越大越好
        /// </summary>
        /// <param name="numTrials">重试次数</param>
        /// <returns></returns>
        private int[] GetGoodIndices(int numTrials)
        {
            int numRows = dataAsInts.Length;
            int numCols = dataAsInts[0].Length;

            int[] result = new int[numClusters];

            //找到的初始分组的原则是使他们尽可能不同
            int largestDiff = -1;
            for (int trial = 0; trial < numTrials; trial++)
            {
                int[] candidates = Reservoir(numClusters, numRows);
                int numDifferences = 0;//不同属性的个数

                for (int i = 0; i < candidates.Length-1; i++)
                {
                    for (int j = i+1; j < candidates.Length; j++)
                    {
                        //比较属性值不同的个数
                        int aRow = candidates[i];
                        int bRow = candidates[j];

                        for (int col = 0; col < numCols; col++)
                        {
                            if (dataAsInts[aRow][col] != dataAsInts[bRow][col])
                            {
                                //属性值不同
                                numDifferences++;
                            }
                        }

                    }
                }

                if (numDifferences>largestDiff)
                {
                    largestDiff = numDifferences;
                    Array.Copy(candidates,result,numClusters);
                }

            }

            return result;
        }

        /// <summary>
        /// 查找n个不同的样本，GetGoodIndices的辅助方法
        /// 从range中选取n个不同的样本
        /// 蓄水池抽样，从range个抽取n个不同的样本，每个的抽到的概率相同
        /// 
        /// 具体的算法解释：
        /// 该方法类似于从数据流中选取n个数据，因为我们不知道最终数据流中有多少数据，我们选取前n个，对于每下一个我们知道它被选取的概率是n/目前观察到的数量，如果随机数小于这个数，我们选取他，并在现有的数中随机挑一个替换
        /// </summary>
        /// <param name="n"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private int[] Reservoir(int n, int range)
        {
            //保存最后的结果
            int[] result = new int[n];

            for (int i = 0; i < n; i++)
            {
                result[i] = i;
            }

            for (int i = n; i < range; i++)
            {
                int j = rnd.Next(0, i + 1);//包含下界，不包含上界
                if (j<n)
                {
                    result[j] = i;
                }
            }

            return result;
        }



        #region 演示程序，不具备通用性

        /// <summary>
        /// 测试程序
        /// </summary>
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
            //分组的个数
            int numClusters = 2;
            Console.WriteLine("Setting numClusters to "+numClusters);
            //重试的次数
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


        /// <summary>
        /// 显示最终结果
        /// </summary>
        /// <param name="numClusters"></param>
        /// <param name="clustering"></param>
        /// <param name="rawData"></param>
        private static void ShowClustering(int numClusters, int[] clustering, string[][] rawData)
        {
            Console.WriteLine("-----------------------------------------------");
            for (int k = 0; k < numClusters; k++)
            {
                for (int i = 0; i < rawData.Length; i++)
                {
                    if (clustering[i] == k)
                    {
                        //是第k个分组的，打印该元素
                        Console.Write(i.ToString().PadLeft(2)+"  ");

                        for (int j = 0; j < rawData[i].Length; j++)
                        {
                            Console.Write(rawData[i][j].ToString().PadRight(8)+"  ");
                        }
                        Console.WriteLine();
                    }
                }

                Console.WriteLine("---------------------------------------");
            }
        }

        /// <summary>
        /// 显示分组
        /// </summary>
        /// <param name="clustering"></param>
        /// <param name="newLine"></param>
        private static void ShowVector(int[] clustering, bool newLine)
        {
            for (int i = 0; i < clustering.Length; i++)
            {
                Console.Write(clustering[i]+"  ");
            }
            Console.WriteLine();
            if (newLine)
            {
                Console.WriteLine();
            }
        }

        
        /// <summary>
        /// 显示源数据
        /// </summary>
        /// <param name="rawData"></param>
        private static void ShowData(string[][] rawData)
        {
            for (int i = 0; i < rawData.Length; i++)
            {
                Console.Write("["+i+"]");
                for (int j = 0; j < rawData[i].Length; j++)
                {
                    Console.Write(rawData[i][j].ToString().PadRight(8)+"  ");
                }
                Console.WriteLine();
            }
        }

        #endregion

    }
}

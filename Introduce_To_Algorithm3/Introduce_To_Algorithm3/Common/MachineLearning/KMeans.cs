using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Math;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// K Means Cluster Algorithm
    /// 输入：聚类个数K,以及N个数据元素
    /// 输出：满足方差最小标准的K个聚类
    /// 算法过程：
    /// 1)从N个点中随机选取K个作为中心
    /// 2）对每个点测量其到中心的距离，并将其归于最近中心的类
    /// 3）重新计算各个类的中心
    /// 4）重复计算2）3），直至中心与原中心相等或者小于指定的阀值，算法结束
    /// </summary>
    public class KMeans
    {
        #region Member
        /// <summary>
        /// 底层存储
        /// </summary>
        private DoubleMatrix _matrix;


        /// <summary>
        /// The k OF K means
        /// </summary>
        private readonly int K;

        /// <summary>
        /// 在收敛前最大的循环次数
        /// </summary>
        public const int LoopLimit = 10000;

        /// <summary>
        /// 当前中心与上一次中心点距离的阀值，当小于该值时算法结束
        /// </summary>
        public const double ThresholdLimit = 1.0;

        /// <summary>
        /// 随机触发器
        /// </summary>
        private Random rand = new Random();

        /// <summary>
        /// 分组结果
        /// </summary>
        private List<HashSet<int>> clusterResult; 

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="k"></param>
        public KMeans(DoubleMatrix matrix, int k)
        {
            this.K = k;
            this._matrix = matrix;
        }
        #endregion


        #region 核心算法

        /// <summary>
        /// 核心聚簇算法
        /// 1)从N个点中随机选取K个作为中心
        /// 2）对每个点测量其到中心的距离，并将其归于最近中心的类
        /// 3）重新计算各个类的中心
        /// 4）重复计算2）3），直至中心与原中心相等或者小于指定的阀值，算法结束
        /// 
        /// //注：该算法目前假设所有的数据均有效
        /// </summary>
        public void Cluster()
        {
            //第一步从N个点中随机选取K个点作为中心
            double[][] tempcentriods = Init();

            int count = 0;
            while (count++ < LoopLimit)
            {
                double[][] centriods = tempcentriods;
                //第二步对每个点测量到中心的距离,并将其归于最近中心的类
                List<HashSet<int>> list = new List<HashSet<int>>();
                for (int i = 0; i < K; i++)
                {
                    list.Add(new HashSet<int>());
                }

                for (int i = 0; i < _matrix.Rows; i++)
                {
                    int min = 0;
                    double minDis = Double.MaxValue;
                    for (int j = 0; j < K; j++)
                    {
                        double curDis = DistanceSquare(centriods[j], _matrix.Row(i));
                        if (minDis > curDis)
                        {
                            minDis = curDis;
                            min = j;
                        }
                    }

                    list[min].Add(i);
                }

                //第三部重新计算中心点
                double [][] newCent = new double[K][];
                for (int i = 0; i < K; i++)
                {
                    newCent[i] = new double[_matrix.Columns];
                }

                for (int i = 0; i < K; i++)
                {
                    double [] sum = new double[_matrix.Columns];
                    foreach (var index in list[i])
                    {
                        double[] temp = _matrix.Row(index);
                        for (int j = 0; j < temp.Length; j++)
                        {
                            sum[j] += temp[j];
                        }
                    }

                    for (int j = 0; j < sum.Length; j++)
                    {
                        sum[j] = sum[j]/list[i].Count;
                    }

                    newCent[i] = sum;
                }

                clusterResult = list;
                bool isEnd = true;

                for (int i = 0; i < K; i++)
                {
                    if (DistanceSquare(newCent[i], centriods[i]) > 1)
                    {
                        isEnd = false;
                        break;
                    }
                }

                if (isEnd)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public double[][] Init()
        {
            //1）从N个点中获取K个作为中心
            double[,] matrix = _matrix.Copy();//获取底层数据的备份
            int row = matrix.GetLength(0);
            int column = matrix.GetLength(1);
            
            //随机重排
            for (int i = 0; i < row-1; i++)
            {
                int j = rand.Next(i, row);

                double[] temp = new double[column];
                for (int k = 0; k < column; k++)
                {
                    temp[k] = matrix[j, k];
                }

                for (int k = 0; k < column; k++)
                {
                    matrix[j, k] = matrix[i, k];
                    matrix[i, k] = temp[k];
                }
            }

            double[][] centriods = new double[K][];

            for (int i = 0; i < K; i++)
            {
                centriods[i] = new double[column];
                for (int j = 0; j < column; j++)
                {
                    centriods[i][j] = matrix[i, j];
                }
            }

            return centriods;
        }

        /// <summary>
        /// 两点之间距离的平方
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public double DistanceSquare(double[] point1, double[] point2)
        {
            double result = 0;
            for (int i = 0; i < point1.GetLength(0); i++)
            {
                result += (point1[i] - point2[i]) * (point1[i] - point2[i]);
            }

            return result;
        }

        #endregion

    }
}

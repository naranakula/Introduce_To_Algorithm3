using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <summary>
        /// 底层存储
        /// </summary>
        private double[,] _matrix;

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
        /// 构造函数
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="k"></param>
        public KMeans(double[,] matrix, int k)
        {
            this.K = k;
            this._matrix = matrix;
        }


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
            //第一步从N个点中随机选取K个作为中心
            Init();

            //第二步对每个点测量其到中心的距离
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>返回</returns>
        public double[,] Init()
        {
            //1）从N个点中随机选取K个作为中心
            int row = _matrix.GetLength(0);
            int column = _matrix.GetLength(1);
            double[,] matrix = new double[row,column];
            //复制原数据
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    matrix[i, j] = _matrix[i, j];
                }
            }

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

            //取初始化中心点
            double[,] centriods = new double[K,column];

            for (int i = 0; i < K; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    centriods[i,j] = matrix[i, j];
                }
            }

            return centriods;
        }

        /// <summary>
        /// 中心点
        /// </summary>
        private double[,] centriods;

        #endregion


    }
}

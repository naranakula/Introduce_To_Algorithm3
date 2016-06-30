using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// 遗传算法
    /// 选择 交叉 变异
    /// </summary>
    public class GeneticAlgorithm:IGeneticAlgorithm
    {
        #region 遗传算法需要调节的三个参数
        /// <summary>
        /// 种群大小
        /// </summary>
        public int PopulationSize { get; set; }
        /// <summary>
        /// 交叉率
        /// </summary>
        public double CrossoverRate { get; set; }
        /// <summary>
        /// 变异率
        /// </summary>
        public double MutationRate { get; set; }
        #endregion

        /// <summary>
        /// 精英数
        /// </summary>
        private int elitismCount;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="populationSize"></param>
        /// <param name="mutationRate"></param>
        /// <param name="crossoverRate"></param>
        /// <param name="elitismCount"></param>
        public GeneticAlgorithm(int populationSize, double mutationRate, double crossoverRate, int elitismCount)
        {
            this.PopulationSize = populationSize;
            this.MutationRate = mutationRate;
            this.CrossoverRate = crossoverRate;
            this.elitismCount = elitismCount;
        }

    }

    /// <summary>
    /// 一个测试问题，查找所有位置为1的字符串
    /// </summary>
    public class AllOnesGA:GeneticAlgorithm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="populationSize"></param>
        /// <param name="mutationRate"></param>
        /// <param name="crossoverRate"></param>
        /// <param name="elitismCount"></param>
        public AllOnesGA(int populationSize, double mutationRate, double crossoverRate, int elitismCount) : base(populationSize, mutationRate, crossoverRate, elitismCount)
        {
        }

        public static void TestMain()
        {
            AllOnesGA ga = new AllOnesGA(100,0.01,0.95,0);
        }
    }

    /// <summary>
    /// 遗传算法的接口
    /// </summary>
    public interface IGeneticAlgorithm
    {
        /// <summary>
        /// 种群大小
        /// </summary>
        int PopulationSize { get; set; }

        /// <summary>
        /// 交叉率
        /// </summary>
        double CrossoverRate { get; set; }

        /// <summary>
        /// 变异率
        /// </summary>
        double MutationRate { get; set; }


    }

}

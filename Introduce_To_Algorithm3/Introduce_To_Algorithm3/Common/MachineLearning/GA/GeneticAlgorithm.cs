using System;
using System.Collections;
using System.Data.Odbc;
using System.Text;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.MachineLearning.GA
{
    #region 遗传算法

    /// <summary>
    /// 遗传算法
    /// 选择 交叉 变异
    /// 遗传算法的基本思想如下：
    ///     1、初始化种群
    ///     2、计算种群中各个对象的适应度，检测是否达到终止条件，如果未达到终止添加进入3，否则结束
    ///     3、选择、交叉、变异，产生新的种群，进入2
    /// 
    /// 遗传算法结束的条件可以是下面的条件：
    ///     演化的代数到达a maximum number
    ///     运行的时间到了
    ///     找到满足一定条件的结果
    ///     找到了最优解
    ///     连续n代当前最优解无变化
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

        /// <summary>
        /// 保留的精英数 0表示保留1个，1表示保留2个
        /// 即每一代到下一代保留最好的的个数,最好不要只保留一个
        /// </summary>
        public int ElitismCount { get; set; }


        /// <summary>
        /// 随机数产生器 获取当前时间相关的随机数
        /// </summary>
        public static Random Rand = new Random();

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="populationSize"></param>
        /// <param name="mutationRate"></param>
        /// <param name="crossoverRate"></param>
        /// <param name="elitismCount"></param>
        public GeneticAlgorithm(int populationSize, double mutationRate, double crossoverRate,int elitismCount)
        {
            this.PopulationSize = populationSize;
            this.MutationRate = mutationRate;
            this.CrossoverRate = crossoverRate;
            this.ElitismCount = elitismCount;
        }

    }

    #endregion

    #region AllOne测试

    /// <summary>
    /// 一个测试问题，查找所有位置为1的字符串
    /// </summary>
    public class AllOnesGA:GeneticAlgorithm
    {

        
        /// <summary>
        /// 最高进化多少代
        /// </summary>
        public const int MaxGenerationLimit = 1000;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="populationSize"></param>
        /// <param name="mutationRate"></param>
        /// <param name="crossoverRate"></param>
        /// <param name="elitismCount"></param>
        public AllOnesGA(int populationSize, double mutationRate, double crossoverRate, int elitismCount) : base(populationSize, mutationRate, crossoverRate,elitismCount)
        {
        }

        /// <summary>
        /// 初始化种群大小
        /// </summary>
        /// <param name="chromosomeLength"></param>
        /// <returns></returns>
        public Population InitPopulation(int chromosomeLength)
        {
            Population population = new Population(this.PopulationSize,chromosomeLength);
            return population;
        }

        /// <summary>
        /// 计算适应度
        /// </summary>
        /// <param name="individual"></param>
        /// <returns></returns>
        public double CalcFitness(Individual individual)
        {
            int correctGenes = 0;
            for (int i = 0; i < individual.GetChromosomeLength(); i++)
            {
                if (individual.GetGene(i) == 1)
                {
                    correctGenes++;
                }
            }

            //计算适应度， 1占得比例
            double fitness = correctGenes*1.0/individual.GetChromosomeLength();

            individual.SetFitness(fitness);
            return fitness;
        }

        /// <summary>
        /// 计算种群的适应度
        /// </summary>
        /// <param name="population"></param>
        public void EvalPopulation(Population population)
        {
            double populationFitness = 0;
            foreach (var individual in population.GetIndividuals())
            {
                populationFitness += CalcFitness(individual);
            }

            population.SetPopulationFitness(populationFitness);
        }

        /// <summary>
        /// 使用轮盘赌算法来选择一个个体，
        /// 选择的概率与适应度成比例
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public Individual SelectParent(Population population)
        {
            Individual[] individuals = population.GetIndividuals();

            //获取种群的适应度
            double populationFitness = population.GetPopulationFitness();
            //轮盘赌旋转到的位置
            double rouletteWheelPosition = Rand.NextDouble()*populationFitness;

            double spinWheel = 0;
            foreach (var item in individuals)
            {
                spinWheel += item.GetFitness();
                if (spinWheel >= rouletteWheelPosition)
                {
                    return item;
                }
            }

            return individuals[population.Size() - 1];
        }

        /// <summary>
        /// 选择和交叉
        /// 交叉采用各个基因随机从两个父亲中选择一个
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public Population CrossoverPopulation(Population population)
        {
            Population newPopulation = new Population(population.Size());

            //按照适应度从高到低便利
            for (int i = 0; i < population.Size(); i++)
            {
                //按照适应度从高到低 
                //注：这种实现是有性能问题的，不要每次getFittest排序
                Individual parent1 = population.GetFittest(i);
                //ElitismCount表示直接保留到下一代的当前最优解的个数
                if (Rand.NextDouble() < this.CrossoverRate && i > this.ElitismCount)
                {
                    //初始化后台
                    Individual offspring = new Individual(parent1.GetChromosomeLength());
                    //找到第二个父类
                    Individual parent2 = SelectParent(population);
                    //从两个父类中随机选择每个基因，来交叉
                    for (int geneIndex = 0; geneIndex < parent1.GetChromosomeLength(); geneIndex++)
                    {
                        if (Rand.NextDouble() < 0.5)
                        {
                            offspring.SetGene(geneIndex,parent1.GetGene(geneIndex));
                        }
                        else
                        {
                            offspring.SetGene(geneIndex, parent2.GetGene(geneIndex));
                        }
                    }

                    newPopulation.SetIndividual(i,offspring);
                }
                else
                {
                    //保留前elitismCount个最好的个体
                    newPopulation.SetIndividual(i,parent1);
                }
            }

            return newPopulation;
        }

        /// <summary>
        /// 种群是否到达了结束条件
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public bool IsTerminationConditionMet(Population population)
        {
            foreach(Individual individual in population.GetIndividuals())
            {
                //本问题我们知道最优解是1
                if (System.Math.Abs(individual.GetFitness() - 1) < 0.001)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 测试程序
        /// </summary>
        public static void TestMain()
        {
            AllOnesGA ga = new AllOnesGA(100,0.01,0.95,3);
            //初始化种群
            Population population = ga.InitPopulation(50);
            //计算适应度
            ga.EvalPopulation(population);
            //认为初始种群是第一代
            int generation = 1;

            while (ga.IsTerminationConditionMet(population) == false)
            {
                //排序，并打印最好的
                NLogHelper.Info("最好的解决方案："+population.GetFittest(0).ToString());

                //选择和交叉
                population = ga.CrossoverPopulation(population);

                //变异

                ga.EvalPopulation(population);
                generation++;
                if (generation > MaxGenerationLimit)
                {
                    NLogHelper.Info("到达了最大代次数："+generation);
                    break;
                }
            }

            NLogHelper.Info("最终最好的方案："+population.GetFittest(0).ToString()+"，代数："+generation);
        }
    }

    /// <summary>
    /// 种群中的一个个体
    /// </summary>
    public class Individual
    {
        /// <summary>
        /// 染色体
        /// </summary>
        private int[] chromosome;

        /// <summary>
        /// 适应度
        /// </summary>
        private double fitness = -1;

        /// <summary>
        /// 随机数产生器  初始为跟当前时间相关的随机数产生器
        /// </summary>
        private static Random rnd = new Random();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="chromosome"></param>
        public Individual(int[] chromosome)
        {
            this.chromosome = chromosome;
        }

        /// <summary>
        /// 构造函数
        /// 随机初始化
        /// </summary>
        /// <param name="chromosomeLength">染色体长度</param>
        public Individual(int chromosomeLength)
        {
            this.chromosome = new int[chromosomeLength];
            for (int gene = 0; gene < chromosomeLength; gene++)
            {
                if (rnd.NextDouble() > 0.5)
                {
                    this.chromosome[gene] = 1;
                }
                else
                {
                    this.chromosome[gene] = 0;
                }
            }
        }

        /// <summary>
        /// 返回染色体
        /// </summary>
        /// <returns></returns>
        public int[] GetChromosome()
        {
            return this.chromosome;
        }

        /// <summary>
        /// 返回染色体长度
        /// </summary>
        /// <returns></returns>
        public int GetChromosomeLength()
        {
            return this.chromosome.Length;
        }

        /// <summary>
        /// 设置offset位置的基因
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="gene"></param>
        public void SetGene(int offset, int gene)
        {
            this.chromosome[offset] = gene;
        }

        /// <summary>
        /// 获取offset位置的基因
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public int GetGene(int offset)
        {
            return this.chromosome[offset];
        }

        /// <summary>
        /// 设置适应度
        /// </summary>
        /// <param name="fitness"></param>
        public void SetFitness(double fitness)
        {
            this.fitness = fitness;
        }

        /// <summary>
        /// 获取适应度
        /// </summary>
        /// <returns></returns>
        public double GetFitness()
        {
            return this.fitness;
        }

        /// <summary>
        /// 覆盖tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < chromosome.Length; i++)
            {
                sb.Append(chromosome[i]);
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// 种群
    /// </summary>
    public class Population
    {
        /// <summary>
        /// 种群
        /// </summary>
        private Individual[] population;
        
        /// <summary>
        /// 种群的适应度 = 所有个体适应度之和
        /// </summary>
        private double populationFitness = -1;

        /// <summary>
        /// 初始化与时间相关的随机数
        /// </summary>
        private static  Random random = new Random();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="populationSize">种群大小</param>
        public Population(int populationSize)
        {
            this.population = new Individual[populationSize];
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="populationSize">种群大小</param>
        /// <param name="chromosomeLength">染色体长度</param>
        public Population(int populationSize, int chromosomeLength)
        {
            this.population = new Individual[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                Individual individual = new Individual(chromosomeLength);
                this.population[i] = individual;
            }
        }

        /// <summary>
        /// 获取种群的底层表示
        /// </summary>
        /// <returns></returns>
        public Individual[] GetIndividuals()
        {
            return this.population;
        }

        /// <summary>
        /// 获取第offset个最大适应度的个体
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Individual GetFittest(int offset)
        {
            Array.Sort(this.population,new Comparison<Individual>((individual1, individual2) =>
            {
                //倒序排列
                if (individual1.GetFitness() > individual2.GetFitness())
                {
                    return -1;
                }
                else if(individual1.GetFitness()<individual2.GetFitness())
                {
                    return 1;
                }

                return 0;
            }));

            return population[offset];
        }

        /// <summary>
        /// 设置种群适应度
        /// </summary>
        /// <param name="fitness"></param>
        public void SetPopulationFitness(double fitness)
        {
            this.populationFitness = fitness;
        }

        /// <summary>
        /// 获取种群适应度
        /// </summary>
        /// <returns></returns>
        public double GetPopulationFitness()
        {
            return this.populationFitness;
        }

        /// <summary>
        /// 获取种群的大小
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return this.population.Length;
        }

        /// <summary>
        /// 设置offset位置的个体
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="individual"></param>
        public void SetIndividual(int offset, Individual individual)
        {
            population[offset] = individual;
        }

        /// <summary>
        /// 获取offset位置的个体
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Individual GetIndividual(int offset)
        {
            return population[offset];
        }

        /// <summary>
        /// 随机重排
        /// </summary>
        public void Shuffle()
        {
            //-1是因为最后一个只能是它自己
            for (int i = 0; i < population.Length-1; i++)
            {
                int index = random.Next(i, population.Length);
                Individual temp = population[index];
                population[index] = population[i];
                population[i] = temp;
            }
        }
    }

    #endregion

    #region 遗传算法接口

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

    #endregion

}

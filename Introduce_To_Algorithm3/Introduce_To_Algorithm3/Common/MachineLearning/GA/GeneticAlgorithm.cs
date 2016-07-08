using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Text;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using NPOI.SS.Formula.Functions;

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
    public class GeneticAlgorithm : IGeneticAlgorithm
    {
        #region 遗传算法需要调节的三个参数   实际运行中可以考虑根据代数自动调整参数
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
        /// 保留的精英数 1表示保留1个，2表示保留2个
        /// 即每一代到下一代保留最好的的个数,最好不要只保留一个
        /// 优化方案：让最好的个体也参与交叉变异，每次用List保存下来最好的。
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
        public GeneticAlgorithm(int populationSize, double mutationRate, double crossoverRate, int elitismCount)
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
    public class AllOnesGA : GeneticAlgorithm
    {


        /// <summary>
        /// 最高进化多少代
        /// </summary>
        public const int MaxGenerationLimit = 10000;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="populationSize"></param>
        /// <param name="mutationRate"></param>
        /// <param name="crossoverRate"></param>
        /// <param name="elitismCount"></param>
        public AllOnesGA(int populationSize, double mutationRate, double crossoverRate, int elitismCount)
            : base(populationSize, mutationRate, crossoverRate, elitismCount)
        {
        }

        /// <summary>
        /// 初始化种群大小
        /// </summary>
        /// <param name="chromosomeLength"></param>
        /// <param name="isRand">是否随机初始化</param>
        /// <returns></returns>
        public virtual Population InitPopulation(int chromosomeLength,bool isRand = true)
        {
            Population population = new Population(this.PopulationSize, chromosomeLength,isRand);
            return population;
        }

        /// <summary>
        /// 计算适应度
        /// </summary>
        /// <param name="individual"></param>
        /// <returns></returns>
        public virtual double CalcFitness(Individual individual)
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
            double fitness = correctGenes * 1.0 / individual.GetChromosomeLength();

            individual.SetFitness(fitness);
            return fitness;
        }

        /// <summary>
        /// 计算种群的适应度
        /// </summary>
        /// <param name="population"></param>
        public virtual void EvalPopulation(Population population)
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
        public virtual Individual SelectParent(Population population)
        {
            Individual[] individuals = population.GetIndividuals();

            //获取种群的适应度
            double populationFitness = population.GetPopulationFitness();
            //轮盘赌旋转到的位置
            double rouletteWheelPosition = Rand.NextDouble() * populationFitness;

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
        public virtual Population CrossoverPopulation(Population population)
        {
            Population newPopulation = new Population(population.Size());

            //按照适应度从高到低排序
            population.Sort();
            for (int i = 0; i < population.Size(); i++)
            {
                //按照适应度从高到低 
                //注：这种实现是有性能问题的，不要每次getFittest排序
                Individual parent1 = population.GetFittest(i);
                //ElitismCount表示直接保留到下一代的当前最优解的个数
                if (Rand.NextDouble() < this.CrossoverRate && i >= this.ElitismCount)
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
                            offspring.SetGene(geneIndex, parent1.GetGene(geneIndex));
                        }
                        else
                        {
                            offspring.SetGene(geneIndex, parent2.GetGene(geneIndex));
                        }
                    }

                    newPopulation.SetIndividual(i, offspring);
                }
                else
                {
                    //保留前elitismCount个最好的个体
                    newPopulation.SetIndividual(i, parent1);
                }
            }

            return newPopulation;
        }

        /// <summary>
        /// 变异种群
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public virtual Population MutatePopulation(Population population)
        {
            //初始化一个新的种群
            Population newPopulation = new Population(population.Size());

            //遍历当前的种群 适应度从高到低顺序 不同于交叉，在变异过程中population的个体已经变了，排序也变了，但只考虑初始的顺序
            population.Sort();
            for (int index = 0; index < population.Size(); index++)
            {
                Individual individual = population.GetFittest(index);

                if (index >= this.ElitismCount)
                {
                    //保留ElitismCount个最好解不动

                    for (int geneIndex = 0; geneIndex < individual.GetChromosomeLength(); geneIndex++)
                    {
                        //变异
                        if (Rand.NextDouble() < this.MutationRate)
                        {
                            //变异
                            int newGene = 1;
                            //反转当前基因位  0变为1 1变为0
                            if (individual.GetGene(geneIndex) == 1)
                            {
                                newGene = 0;
                            }

                            individual.SetGene(geneIndex, newGene);
                        }
                    }
                }
                newPopulation.SetIndividual(index, individual);
            }

            return newPopulation;
        }


        /// <summary>
        /// 种群是否到达了结束条件
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public virtual bool IsTerminationConditionMet(Population population)
        {
            foreach (Individual individual in population.GetIndividuals())
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
            //保留3个最优解
            AllOnesGA ga = new AllOnesGA(100, 0.001, 0.95, 10);
            //初始化种群
            Population population = ga.InitPopulation(50);
            //计算适应度
            ga.EvalPopulation(population);
            //认为初始种群是第一代
            int generation = 1;

            while (ga.IsTerminationConditionMet(population) == false)
            {
                //排序，并打印最好的
                if (generation % 50 == 0)
                {
                    population.Sort();
                    NLogHelper.Info("最好的解决方案：" + population.GetFittest(0).ToString());
                }
                //选择和交叉  交叉中做了排序
                population = ga.CrossoverPopulation(population);
                //种群变了，要重新计算
                ga.EvalPopulation(population);
                //变异   变异中做了排序
                population = ga.MutatePopulation(population);
                //种群变了，要重新计算
                ga.EvalPopulation(population);
                generation++;
                if (generation > MaxGenerationLimit)
                {
                    NLogHelper.Info("到达了最大代次数：" + generation);
                    break;
                }
            }

            population.Sort();
            Individual finalIndividual = population.GetFittest(0);
            NLogHelper.Info(String.Format("最终最好的解决方案：{0},最终的代数：{1},最好的适应度：{2}", finalIndividual, generation, finalIndividual.GetFitness()));
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
        ///  在旅行商问题中，一个合格的染色体 如 1，2，3、、、
        /// 包含所有的城市基因，且不能有重复
        /// 基因的顺序表示访问的顺序
        /// </summary>
        /// <param name="chromosomeLength">染色体长度</param>
        /// <param name="isRandom">true 随机初始化；false 顺序初始化</param>
        public Individual(int chromosomeLength,bool isRandom = true)
        {
            if (isRandom)
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
            else
            {
                this.chromosome = new int[chromosomeLength];
                for (int gene = 0; gene < chromosomeLength; gene++)
                {
                    this.chromosome[gene] = gene;
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
        /// 是否包含指定基因
        /// </summary>
        /// <param name="gene"></param>
        /// <returns></returns>
        public bool ContainsGene(int gene)
        {
            for (int i = 0; i < this.chromosome.Length; i++)
            {
                if (this.chromosome[i] == gene)
                {
                    return true;
                }
            }

            return false;
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
        private static Random random = new Random();

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
        /// <param name="isRand">是否随机初始化</param>
        public Population(int populationSize, int chromosomeLength,bool isRand=true)
        {
            this.population = new Individual[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                Individual individual = new Individual(chromosomeLength,isRand);
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
        /// 按适应度从高到低排序
        /// </summary>
        public void Sort()
        {
            Array.Sort(this.population, new Comparison<Individual>((individual1, individual2) =>
            {
                //倒序排列
                if (individual1.GetFitness() > individual2.GetFitness())
                {
                    return -1;
                }
                else if (individual1.GetFitness() < individual2.GetFitness())
                {
                    return 1;
                }

                return 0;
            }));
        }


        /// <summary>
        /// 获取第offset个最大适应度的个体
        /// 在调用该方法置前，要先排序
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Individual GetFittest(int offset)
        {
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
            for (int i = 0; i < population.Length - 1; i++)
            {
                int index = random.Next(i, population.Length);
                Individual temp = population[index];
                population[index] = population[i];
                population[i] = temp;
            }
        }
    }

    #endregion

    #region RobotController 机器人

    /// <summary>
    /// 机器人 根据指令 在 迷宫中行驶
    /// </summary>
    public class Robot
    {
        /// <summary>
        /// 方向
        /// </summary>
        private enum Direction
        {
            NORTH,
            EAST,
            SOUTH,
            WEST
        }

        /// <summary>
        /// 机器人当前x位置
        /// </summary>
        private int xPosition;

        /// <summary>
        /// 机器人y位置
        /// </summary>
        private int yPosition;

        /// <summary>
        /// 方向
        /// </summary>
        private Direction heading;

        /// <summary>
        /// 最大允许移动的次数
        /// </summary>
        private int maxMoves;

        /// <summary>
        /// 当前移动的次数
        /// </summary>
        private int moves;

        /// <summary>
        /// 传感器编码后的值  0-63
        /// </summary>
        private int sensorVal;

        /// <summary>
        /// sensor对应的action 0-3  不动 00 前移01  左转 10 右转11
        /// </summary>
        private int[] sensorActions;

        /// <summary>
        /// 迷宫
        /// </summary>
        private Maze maze;

        /// <summary>
        /// 路径
        /// </summary>
        private List<int[]> route;

        /// <summary>
        /// 初始化控制器
        /// </summary>
        /// <param name="sensorActions"></param>
        /// <param name="maze"></param>
        /// <param name="maxMoves"></param>
        public Robot(int[] sensorActions, Maze maze, int maxMoves)
        {
            this.sensorActions = this.calcSensorActions(sensorActions);
            this.maze = maze;
            //获取起始位置
            int[] startPos = this.maze.GetStartPosition();
            this.xPosition = startPos[0];
            this.yPosition = startPos[1];
            this.sensorVal = -1;
            this.heading = Direction.EAST;
            this.maxMoves = maxMoves;
            this.moves = 0;
            this.route = new List<int[]>();
            this.route.Add(startPos);
        }

        /// <summary>
        /// run the robot's action based on sensor input
        /// </summary>
        public void Run()
        {
            while (true)
            {
                this.moves++;

                //break if the robot stop moving  如果不动，实际上可以根据传感器变更方向，而不是返回
                if (this.GetNextAction() == 0)
                {
                    return;
                }

                //break if we read the goal
                if (this.maze.GetPositionValue(this.xPosition, this.yPosition) == 4)
                {
                    return;
                }

                //到达最大移动次数
                if (this.moves > this.maxMoves)
                {
                    return;
                }

                //Run action
                this.MakeNextAction();
            }
        }

        /// <summary>
        /// runs the next action
        /// </summary>
        private void MakeNextAction()
        {
            //移动方式有四种：不动 00 前移01  左转 10 右转11
            if (this.GetNextAction() == 1)
            {
                //前移
                int currentX = this.xPosition;
                int currentY = this.yPosition;

                //move depending on current direction
                if (Direction.NORTH == this.heading)
                {
                    this.yPosition += -1;
                    if (this.yPosition < 0)
                    {
                        this.yPosition = 0;
                    }
                }
                else if (Direction.EAST == this.heading)
                {
                    this.xPosition += 1;
                    if (this.xPosition > this.maze.GetMaxX())
                    {
                        this.xPosition = this.maze.GetMaxX();
                    }
                }
                else if (Direction.SOUTH == this.heading)
                {
                    this.yPosition += 1;
                    if (this.yPosition > this.maze.GetMaxY())
                    {
                        this.yPosition = this.maze.GetMaxY();
                    }
                }
                else if (Direction.WEST == this.heading)
                {
                    this.xPosition += -1;
                    if (this.xPosition < 0)
                    {
                        this.xPosition = 0;
                    }
                }

                //当是墙时，we can't move here
                if (this.maze.IsWall(this.xPosition, this.yPosition))
                {
                    //不能移动，置回原来位置
                    this.xPosition = currentX;
                    this.yPosition = currentY;
                }
                else
                {
                    if (currentX != this.xPosition || currentY != this.yPosition)
                    {
                        //移动了1个位置 添加到路径中
                        this.route.Add(this.GetPosition());
                    }
                }
            }
            else if (this.GetNextAction() == 2)
            {
                //左转 10 右转11
                if (Direction.NORTH == this.heading)
                {
                    this.heading = Direction.WEST;
                }
                else if (Direction.EAST == this.heading)
                {
                    this.heading = Direction.NORTH;
                }
                else if (Direction.SOUTH == this.heading)
                {
                    this.heading = Direction.EAST;
                }
                else if (Direction.WEST == this.heading)
                {
                    this.heading = Direction.SOUTH;
                }
            }
            else if (this.GetNextAction() == 3)
            {
                //右转11
                if (Direction.NORTH == this.heading)
                {
                    this.heading = Direction.EAST;
                }
                else if (Direction.EAST == this.heading)
                {
                    this.heading = Direction.SOUTH;
                }
                else if (Direction.SOUTH == this.heading)
                {
                    this.heading = Direction.WEST;
                }
                else if (Direction.WEST == this.heading)
                {
                    this.heading = Direction.NORTH;
                }
            }

            //重置传感器 传感器取值范围[0,63]
            this.sensorVal = -1;
        }

        /// <summary>
        /// 获取机器当前位置
        /// </summary>
        /// <returns></returns>
        private int[] GetPosition()
        {
            return new int[] { this.xPosition, this.yPosition };
        }

        /// <summary>
        /// 获取下一次行动值
        /// </summary>
        /// <returns></returns>
        private int GetNextAction()
        {
            return this.sensorActions[this.GetSensorValue()];
        }

        /// <summary>
        /// 获取当前传感器的值  0到64
        /// </summary>
        /// <returns></returns>
        private int GetSensorValue()
        {
            if (this.sensorVal > -1)
            {
                //传感器值已经计算出来了，直接返回
                return this.sensorVal;
            }

            //从高位到低位 依次是 B R L FR FL F
            bool frontSensor, frontLeftSensor, frontRightSensor, leftSensor, rightSensor, backSensor;
            frontSensor = frontLeftSensor = frontRightSensor = leftSensor = rightSensor = backSensor = false;
            if (this.GetHeading() == Direction.NORTH)
            {
                frontSensor = this.maze.IsWall(this.xPosition, this.yPosition - 1);
                frontLeftSensor = this.maze.IsWall(this.xPosition - 1, this.yPosition - 1);
                frontRightSensor = this.maze.IsWall(this.xPosition + 1, this.yPosition - 1);
                leftSensor = this.maze.IsWall(this.xPosition - 1, this.yPosition);
                rightSensor = this.maze.IsWall(this.xPosition + 1, this.yPosition);
                backSensor = this.maze.IsWall(this.xPosition, this.yPosition + 1);
            }
            else if (this.GetHeading() == Direction.EAST)
            {
                frontSensor = this.maze.IsWall(this.xPosition + 1, this.yPosition);
                frontLeftSensor = this.maze.IsWall(this.xPosition + 1, this.yPosition - 1);
                frontRightSensor = this.maze.IsWall(this.xPosition + 1, this.yPosition + 1);
                leftSensor = this.maze.IsWall(this.xPosition, this.yPosition - 1);
                rightSensor = this.maze.IsWall(this.xPosition, this.yPosition + 1);
                backSensor = this.maze.IsWall(this.xPosition - 1, this.yPosition);
            }
            else if (this.GetHeading() == Direction.SOUTH)
            {
                frontSensor = this.maze.IsWall(this.xPosition, this.yPosition + 1);
                frontLeftSensor = this.maze.IsWall(this.xPosition + 1, this.yPosition + 1);
                frontRightSensor = this.maze.IsWall(this.xPosition - 1, this.yPosition + 1);
                leftSensor = this.maze.IsWall(this.xPosition + 1, this.yPosition);
                rightSensor = this.maze.IsWall(this.xPosition - 1, this.yPosition);
                backSensor = this.maze.IsWall(this.xPosition, this.yPosition - 1);
            }
            else if (this.GetHeading() == Direction.WEST)
            {
                frontSensor = this.maze.IsWall(this.xPosition - 1,this.yPosition);
                frontLeftSensor = this.maze.IsWall(this.xPosition - 1, this.yPosition + 1);
                frontRightSensor = this.maze.IsWall(this.xPosition - 1, this.yPosition - 1);
                leftSensor = this.maze.IsWall(this.xPosition, this.yPosition + 1);
                rightSensor = this.maze.IsWall(this.xPosition, this.yPosition - 1);
                backSensor = this.maze.IsWall(this.xPosition + 1, this.yPosition);
            }

            // Calculate sensor value
            int tempSensorVal = 0;
            if (frontSensor == true)
            {
                tempSensorVal += 1;
            }
            if (frontLeftSensor == true)
            {
                tempSensorVal += 2;
            }
            if (frontRightSensor == true)
            {
                tempSensorVal += 4;
            }
            if (leftSensor == true)
            {
                tempSensorVal += 8;
            }
            if (rightSensor == true)
            {
                tempSensorVal += 16;
            }
            if (backSensor == true)
            {
                tempSensorVal += 32;
            }
            this.sensorVal = tempSensorVal;
            return tempSensorVal;
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <returns></returns>
        public List<int[]> GetRoute()
        {
            return this.route;
        }


        /// <summary>
        /// 获取方向
        /// </summary>
        /// <returns></returns>
        private Direction GetHeading()
        {
            return this.heading;
        }

        /// <summary>
        /// map robot's sensor data to action from binary string
        /// </summary>
        /// <param name="sensorActionsStr">传感器输入 binary ga 染色体</param>
        /// <returns>将传感器输入映射为action</returns>
        private int[] calcSensorActions(int[] sensorActionsStr)
        {
            //how many actions are there
            int numActions = (int)sensorActionsStr.Length / 2;
            int[] sensorActions = new int[numActions];
            
            //Loop through action
            for (int sensorValue = 0; sensorValue < numActions; sensorValue++)
            {
                //不动 00 前移01  左转 10 右转11
                int sensorAction = 0;

                //高位加2
                if (sensorActionsStr[sensorValue * 2] == 1)
                {
                    sensorAction += 2;
                }

                //低位加1
                if (sensorActionsStr[sensorValue * 2 + 1] == 1)
                {
                    sensorAction += 1;
                }

                //add to sensor action map
                //sensorValue表示传感器输入 sensorAction表示对应的action
                sensorActions[sensorValue] = sensorAction;
            }

            return sensorActions;
        }

        /// <summary>
        /// 打印路径
        /// </summary>
        /// <returns></returns>
        public String PrintRoute()
        {
            StringBuilder sBuilder = new StringBuilder();
            foreach (var routeStep in this.route)
            {
                sBuilder.Append("{"+routeStep[0]+","+routeStep[1]+"},");
            }

            return sBuilder.ToString().TrimEnd(',');
        }

    }

    /// <summary>
    /// 机器人控制器
    /// 有六个传感器，前面3个，后面1个，左边1个右边1个  移动方式有四种：不动 00 前移01  左转 10 右转11
    /// 使用128位 表示输入输出
    /// 这里有一个明显的缺点，即只根据当前的传感器来移动，没有考虑上下文 即即使同一个传感器输入，但考虑之前的移动，当前的移动应该有所不同
    /// 
    /// 本例中使用了锦标赛选择和单点交叉
    /// 选择的标准仍然是适应度越高，参与交叉的概率越大
    /// 单点交叉：随机选择一个交叉位置，这个位置之前的来自parent1，这个位置之后来自parent2
    /// 两点交叉：随机选择两个交叉位置，两个之间的部分来自parent1,其他部分来自parent2
    /// </summary>
    public class RobotController : AllOnesGA
    {
        /// <summary>
        /// 遗传算法应用的最大代数
        /// </summary>
        public static int MaxGenerations = 1000;

        /// <summary>
        /// 每次锦标赛种群选择的size  tournament selection  锦标赛选择用于交叉个体参与锦标赛的个数
        /// 锦标赛是随机选择TournamentSize个个体，然后从中选择适应度最好的
        /// 一种优化方案是设置一个概率p,如0.6， p的概率选择最好的，如果没有选择最好的，再p的概率选择第二好的，直到选到最后一个
        /// </summary>
        public int TournamentSize;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="populationSize">种群大小</param>
        /// <param name="mutationRate">变异率</param>
        /// <param name="crossoverRate">交叉率</param>
        /// <param name="elitismCount">保留的最优个数</param>
        /// <param name="tournamentSize">每次锦标赛种群选择的size</param>
        public RobotController(int populationSize, double mutationRate, double crossoverRate, int elitismCount, int tournamentSize)
            : base(populationSize, mutationRate, crossoverRate, elitismCount)
        {
            this.TournamentSize = tournamentSize;
        }

        /// <summary>
        /// 计算适应度
        /// </summary>
        /// <param name="individual"></param>
        /// <param name="maze"></param>
        /// <returns></returns>
        public virtual double CalcFitness(Individual individual,Maze maze)
        {
            int[] chromosome = individual.GetChromosome();
            //最大100步
            Robot robot = new Robot(chromosome,maze,100);
            robot.Run();
            //走的步数越多，适应度越大
            int fitness = maze.ScoreRoute(robot.GetRoute());
            individual.SetFitness(fitness);
            return fitness;
        }

        /// <summary>
        /// 评估种群
        /// </summary>
        /// <param name="population"></param>
        /// <param name="maze"></param>
        public virtual void EvalPopulation(Population population, Maze maze)
        {
            double populationFitness = 0;
            foreach (var individual in population.GetIndividuals())
            {
                populationFitness += this.CalcFitness(individual, maze);
            }

            population.SetPopulationFitness(populationFitness);
        }

        /// <summary>
        /// 是否满足结束条件
        /// </summary>
        /// <param name="generation"></param>
        /// <param name="maxGeneration"></param>
        /// <returns></returns>
        public virtual bool IsTerminationConditionMet(int generation, int maxGeneration)
        {
            if (generation > maxGeneration)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 使用锦标赛选择个体  锦标赛是随机选择TournamentSize个个体，然后从中选择适应度最好的
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public override Individual SelectParent(Population population)
        {
            //创建锦标赛团体
            Population tournament = new Population(this.TournamentSize);
            //种群重排
            population.Shuffle();

            for (int i = 0; i < this.TournamentSize; i++)
            {
                Individual tournamentIndividual = population.GetIndividual(i);
                tournament.SetIndividual(i,tournamentIndividual);
            }

            // 锦标赛是随机选择TournamentSize个个体，然后从中选择适应度最好的
            // 一种优化方案是设置一个概率p,如0.6， p的概率选择最好的，如果没有选择最好的，再p的概率选择第二好的，直到选到最后一个

            tournament.Sort();
            return tournament.GetFittest(0);
        }

        /// <summary>
        /// 选择和交叉
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public override Population CrossoverPopulation(Population population)
        {
            //创建结果
            Population resultPopulation = new Population(population.Size());

            for (int populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                //因为selectParent会重排，所以每次循环要排序 这里实际上还是可以优化的
                //优化方法：创建population的副本，参与selectparent
                population.Sort();
                Individual parent1 = population.GetFittest(populationIndex);
                if (Rand.NextDouble() < this.CrossoverRate && populationIndex >= this.ElitismCount)
                {
                    //需要交叉
                    //初始化后代 
                    Individual offspring = new Individual(parent1.GetChromosomeLength());
                    //选择第二个父
                    Individual parent2 = this.SelectParent(population);

                    //获取随机交换点， 可以考虑交换点不要在最前或最后的位置
                    int swapPoint = (int) (Rand.NextDouble()*(parent1.GetChromosomeLength() + 1));
                    for (int geneIndex = 0; geneIndex < parent1.GetChromosomeLength(); geneIndex++)
                    {
                        if (geneIndex < swapPoint)
                        {
                            offspring.SetGene(geneIndex, parent1.GetGene(geneIndex));
                        }
                        else
                        {
                            offspring.SetGene(geneIndex,parent2.GetGene(geneIndex));
                        }
                    }

                    resultPopulation.SetIndividual(populationIndex,offspring);
                }
                else
                {
                    resultPopulation.SetIndividual(populationIndex,parent1);
                }
            }

            return resultPopulation;
        }

        /// <summary>
        /// 测试主体
        /// </summary>
        public static void TestMain()
        {
            /**
             * 0 Empty
             * 1 Wall
             * 2 Starting Position
             * 3 Route
             * 4 Goal Position
             */

            int[][] mazeArr = new int[9][];
            mazeArr[0] = new int[] { 0, 0, 0, 0, 1, 0, 1, 3, 2 };
            mazeArr[1] = new int[] { 1, 0, 1, 1, 1, 0, 1, 3, 1 };
            mazeArr[2] = new int[] { 1, 0, 0, 1, 3, 3, 3, 3, 1 };
            mazeArr[3] = new int[] { 3, 3, 3, 1, 3, 1, 1, 0, 1 };
            mazeArr[4] = new int[] { 3, 1, 3, 3, 3, 1, 1, 0, 0 };
            mazeArr[5] = new int[] { 3, 3, 1, 1, 1, 1, 0, 1, 1 };
            mazeArr[6] = new int[] { 1, 3, 0, 1, 3, 3, 3, 3, 3 };
            mazeArr[7] = new int[] { 0, 3, 1, 1, 3, 1, 0, 1, 3 };
            mazeArr[8] = new int[] { 1, 3, 3, 3, 3, 1, 1, 1, 4 };

            //初始化迷宫
            Maze maze = new Maze(mazeArr);

            RobotController ga = new RobotController(200, 0.05, 0.9, 2, 10);

            //初始化种群
            Population population = ga.InitPopulation(128);

            //评估种群
            ga.EvalPopulation(population,maze);

            int generation = 1;

            while (!ga.IsTerminationConditionMet(generation,MaxGenerations))
            {
                //打印最优个体
                if (generation%50 == 0)
                {
                    population.Sort();
                    var indivial = population.GetFittest(0);
                    NLogHelper.Info("G"+generation+" best solution ="+indivial+" 适应度="+indivial.GetFitness());
                }

                //应用交叉 交叉中做了排序 
                //原来的交叉是可以的，这里使用了一个新的交叉方案
                population = ga.CrossoverPopulation(population);


                //评估种群
                ga.EvalPopulation(population, maze);

                //应用变异 变异中做了排序
                population = ga.MutatePopulation(population);

                //评估种群
                ga.EvalPopulation(population, maze);

                generation++;
            }

            population.Sort();
            var fitestItem = population.GetFittest(0);
            //打印最终结果  本问题中最优解适应度29
            NLogHelper.Info("最终结果："+fitestItem+"  适应度="+fitestItem.GetFitness());
        }
    }

    /// <summary>
    /// 迷宫
    /// 0 Empty 1表示墙 2表示起点 3Route可通行的路  4终点 Goal position
    /// </summary>
    public class Maze
    {
        /// <summary>
        /// 迷宫底层表示
        /// </summary>
        private int[][] maze;

        /// <summary>
        /// 迷宫的起始位置
        /// </summary>
        private int[] startPosition = { -1, -1 };

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maze"></param>
        public Maze(int[][] maze)
        {
            this.maze = maze;
        }

        /// <summary>
        /// 获取起始位置
        /// </summary>
        /// <returns></returns>
        public int[] GetStartPosition()
        {
            if (this.startPosition[0] != -1 && this.startPosition[1] != -1)
            {
                return this.startPosition;
            }

            //默认起始位置
            startPosition = new int[] { 0, 0 };

            for (int rowIndex = 0; rowIndex < this.maze.Length; rowIndex++)
            {
                for (int colIndex = 0; colIndex < this.maze[rowIndex].Length; colIndex++)
                {
                    if (this.maze[rowIndex][colIndex] == 2)
                    {
                        //2表示起点
                        this.startPosition = new int[] { rowIndex, colIndex };
                        return this.startPosition;
                    }
                }
            }

            return startPosition;
        }

        /// <summary>
        /// 获取迷宫上的位置值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetPositionValue(int x, int y)
        {
            if (x < 0 || y < 0 || x >= this.maze.Length || y >= this.maze[0].Length)
            {
                return 1;
            }

            return this.maze[x][y];
        }

        /// <summary>
        /// 是否是墙
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsWall(int x, int y)
        {
            return this.GetPositionValue(x, y) == 1;
        }

        /// <summary>
        /// 获取最大的x
        /// </summary>
        /// <returns></returns>
        public int GetMaxX()
        {
            return this.maze.Length - 1;
        }

        /// <summary>
        /// 获取最大的y
        /// </summary>
        /// <returns></returns>
        public int GetMaxY()
        {
            return this.maze[0].Length - 1;
        }

        /// <summary>
        /// 对路径打分 作为适应度函数 
        /// 评估正确的路径，不考虑回头路， 实际上应该有更好的适应度函数，如回头路适当的减分等
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public int ScoreRoute(List<int[]> route)
        {
            int score = 0;

            bool[,] visited = new bool[this.GetMaxX() + 1, this.GetMaxY() + 1];

            foreach (var routeStep in route)
            {
                int[] step = routeStep;

                if (this.maze[step[0]][step[1]] == 3 && visited[step[0], step[1]] == false)
                {
                    //正确的移动加分
                    score++;
                    //移除reward
                    visited[step[0], step[1]] = true;
                }
            }

            return score;
        }




    }


    #endregion

    #region 旅行商问题

    /*
     * 有一个推销员，要到n个城市推销商品，他要找出一个包含所有n个城市的具有最短路程的环路。
     * 除起点外，每个城市只能访问一次
     * 
     * 一种解决方案：随机找一个起始点， 找到离该点最近的点，前进到该点，再找到离当前位置最近的点，直到结束
     * 
     * 采用Ordered cross, 假设要从parent1和parent2中产生offspring，
     *      第一步：从parent1中产生一个子集，加入offspring中相同的位置，然后从parent2对应的子集分割点之后遍历，加入offspring从头开始的位置
     */

    public class TravelingSalesman : RobotController
    {
        /// <summary>
        /// 最大的代数
        /// </summary>
        public const int MaxGenerations = 3000;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="populationSize"></param>
        /// <param name="mutationRate"></param>
        /// <param name="crossoverRate"></param>
        /// <param name="elitismCount"></param>
        /// <param name="tournamentSize"></param>
        public TravelingSalesman(int populationSize, double mutationRate, double crossoverRate, int elitismCount, int tournamentSize)
            : base(populationSize, mutationRate, crossoverRate, elitismCount, tournamentSize)
        {

        }

        /// <summary>
        /// 计算适应度
        /// </summary>
        /// <param name="individual"></param>
        /// <param name="cities"></param>
        /// <returns></returns>
        public virtual double CalcFitness(Individual individual, City[] cities)
        {
            Route route = new Route(individual, cities);
            // 路径越长适应度越低， 但倒数这个方法是值得商量的
            double fitness = 1 / route.GetDistance();

            //存储适应度
            individual.SetFitness(fitness);
            return fitness;
        }

        /// <summary>
        /// 计算种群的适应度
        /// </summary>
        /// <param name="population"></param>
        /// <param name="cities"></param>
        public virtual void EvalPopulation(Population population, City[] cities)
        {
            double populationFitness = 0;
            //计算每个个体的适应度
            foreach (Individual individual in population.GetIndividuals())
            {
                populationFitness += this.CalcFitness(individual, cities);
            }

            //种群的适应度为平均适应度，之前均为适应度之和
            double avgFitness = populationFitness / population.Size();
            population.SetPopulationFitness(avgFitness);
        }

        /// <summary>
        /// 交叉
        /// 采用Ordered cross, 假设要从parent1和parent2中产生offspring，
        ///       第一步：从parent1中产生一个子集，加入offspring中相同的位置，然后从parent2对应的子集分割点之后遍历，加入offspring从头开始的位置
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public override Population CrossoverPopulation(Population population)
        {
            //创建一个新的种群
            Population resultPopulation = new Population(population.Size());

            //遍历当前种群
            for (int populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                //从高到低排序  放到这个位置是因为后面种群随机重排了
                population.Sort();
                //获取parent1
                Individual parent1 = population.GetFittest(populationIndex);
                
                //应用交叉
                if (Rand.NextDouble() < this.CrossoverRate && populationIndex >= this.ElitismCount)
                {
                    //找到parent2 使用锦标赛选择
                    Individual parent2 = this.SelectParent(population);

                    //创建空白后代
                    int[] offSpringChromosome = new int[parent1.GetChromosomeLength()];
                    for (int i = 0; i < offSpringChromosome.Length; i++)
                    {
                        //空白后代
                        offSpringChromosome[i] = -1;
                    }

                    Individual offspring = new Individual(offSpringChromosome);

                    //从parent1中获取连续子集
                    int subPos1 = (int) (Rand.NextDouble()*parent1.GetChromosomeLength());
                    int subPos2 = (int) (Rand.NextDouble() * parent1.GetChromosomeLength());

                    int startPos = System.Math.Min(subPos1, subPos2);
                    int endPos = System.Math.Max(subPos1, subPos2);
                    //从parent1中取连续子集
                    for (int i = startPos; i < endPos; i++)
                    {
                        offspring.SetGene(i,parent1.GetGene(i));
                    }

                    //遍历parent2分割后半部分
                    for (int i = endPos; i < parent2.GetChromosomeLength(); i++)
                    {
                        if (!offspring.ContainsGene(parent2.GetGene(i)))
                        {
                            for (int j = 0; j < offspring.GetChromosomeLength(); j++)
                            {
                                if (offspring.GetGene(j) < 0)
                                {
                                    offspring.SetGene(j,parent2.GetGene(i));
                                    break;
                                }
                            }
                        }
                    }

                    //遍历parent2前半部分
                    for (int i = 0; i < endPos; i++)
                    {
                        if (!offspring.ContainsGene(parent2.GetGene(i)))
                        {
                            for (int j = 0; j < offspring.GetChromosomeLength(); j++)
                            {
                                if (offspring.GetGene(j) < 0)
                                {
                                    offspring.SetGene(j, parent2.GetGene(i));
                                    break;
                                }
                            }
                        }
                    }

                    resultPopulation.SetIndividual(populationIndex,offspring);
                }
                else
                {
                    resultPopulation.SetIndividual(populationIndex,parent1);
                }
            }

            return resultPopulation;
        }

        /// <summary>
        /// 根据变异率选择变异点，然后再随机找一个进行交换 这叫做 swap mutation
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public override Population MutatePopulation(Population population)
        {
            Population resultPopulation = new Population(this.PopulationSize);

            //从高到低排序
            population.Sort();
            //循环
            for (int populationIndex = 0; populationIndex < population.Size(); populationIndex++)
            {
                Individual individual = population.GetFittest(populationIndex);

                if (populationIndex >= this.ElitismCount)
                {
                    for (int geneIndex = 0; geneIndex < individual.GetChromosomeLength(); geneIndex++)
                    {
                        if (Rand.NextDouble() < this.MutationRate)
                        {
                            int secondIndex = (int) (Rand.NextDouble()*individual.GetChromosomeLength());
                            int gene1 = individual.GetGene(secondIndex);
                            int gene2 = individual.GetGene(geneIndex);
                            //交换
                            individual.SetGene(geneIndex,gene1);
                            individual.SetGene(secondIndex,gene2);
                        }
                    }

                    resultPopulation.SetIndividual(populationIndex, individual);
                }
                else
                {
                    resultPopulation.SetIndividual(populationIndex,individual);
                }
            }

            return resultPopulation;
        }

        /// <summary>
        /// 测试方法
        /// </summary>
        public static void TestMain()
        {
            //城市个数
            int numCities = 100;
            City[] cities = new City[numCities];

            //随机初始化位置
            for (int i = 0; i < numCities; i++)
            {
                int xPos = (int) (Rand.NextDouble()*100);
                int yPos = (int)(Rand.NextDouble() * 100);
                cities[i] = new City(xPos,yPos);
            }

            TravelingSalesman ga = new TravelingSalesman(100,0.001,0.9,2,5);

            //顺序初始化
            Population population = ga.InitPopulation(cities.Length,false);

            //评估种群
            ga.EvalPopulation(population,cities);

            //当前的代数
            int generation = 1;
            //是否结束条件满足
            while (!ga.IsTerminationConditionMet(generation, MaxGenerations))
            {
                //打印最好的个体
                if (generation%50 == 0)
                {
                    population.Sort();
                    Route route = new Route(population.GetFittest(0),cities);
                    NLogHelper.Info("G" + generation+", best distance:"+route.GetDistance());
                }

                //应用交叉 交叉中做了排序
                population = ga.CrossoverPopulation(population);

                //评估种群
                ga.EvalPopulation(population,cities);

                //应用变异 变异中做了排序
                ga.MutatePopulation(population);

                //评估种群
                ga.EvalPopulation(population,cities);


                //增加代数
                generation++;
            }

            //打印最终结果
            population.Sort();
            Route resultRoute = new Route(population.GetFittest(0), cities);
            NLogHelper.Info("最终结果：G" + generation + ", best distance:" + resultRoute.GetDistance());
        }

    }

    /// <summary>
    /// 城市
    /// </summary>
    public class City
    {
        /// <summary>
        /// x坐标
        /// </summary>
        private int x;
        /// <summary>
        /// y坐标
        /// </summary>
        private int y;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public City(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 距离·
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public double DistanceFrom(City city)
        {
            double distance =
                System.Math.Sqrt((city.GetX() - this.x)*(city.GetX() - this.x) +
                                 (city.GetY() - this.y)*(city.GetY() - y));

            return distance;
        }

        /// <summary>
        /// 获取x坐标
        /// </summary>
        /// <returns></returns>
        public int GetX()
        {
            return this.x;
        }

        /// <summary>
        /// 获取y坐标
        /// </summary>
        /// <returns></returns>
        public int GetY()
        {
            return this.y;
        }

    }

    /// <summary>
    /// 可以以路径的长度作为适应度，长度越大适应度越小，长度越小，适应度越大
    /// </summary>
    public class Route
    {
        /// <summary>
        /// 路径
        /// </summary>
        private City[] route;

        /// <summary>
        /// 距离
        /// </summary>
        private double distance = -1;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="individual"></param>
        /// <param name="cities"></param>
        public Route(Individual individual, City[] cities)
        {
            //获取染色体
            int[] chromosome = individual.GetChromosome();
            //创建路径
            this.route = new City[cities.Length];

            for (int geneIndex = 0; geneIndex < chromosome.Length; geneIndex++)
            {
                this.route[geneIndex] = cities[chromosome[geneIndex]];
            }
        }

        /// <summary>
        /// 获取距离
        /// </summary>
        /// <returns></returns>
        public double GetDistance()
        {
            if (this.distance > 0)
            {
                return this.distance;
            }

            //循环路径，计算距离
            double totalDistance = 0;

            for (int cityIndex = 0; cityIndex+1 < this.route.Length; cityIndex++)
            {
                totalDistance += this.route[cityIndex].DistanceFrom(this.route[cityIndex + 1]);
            }
            //回到原点
            totalDistance += this.route[this.route.Length - 1].DistanceFrom(this.route[0]);
            return totalDistance;
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

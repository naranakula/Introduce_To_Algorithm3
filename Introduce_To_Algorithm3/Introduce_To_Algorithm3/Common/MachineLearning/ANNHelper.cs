﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// Neural networks using c-sharp succinctly
    /// </summary>
    public class ANNHelper
    {



        public static void TestMain()
        {
            double[][] trainData = new double[8][]; 
            trainData[0] = new double[] { 1.5, 2.0, -1 }; 
            trainData[1] = new double[] { 2.0, 3.5, -1 }; 
            trainData[2] = new double[] { 3.0, 5.0, -1 };
            trainData[3] = new double[] { 3.5, 2.5, -1 };
            trainData[4] = new double[] { 4.5, 5.0, 1 }; 
            trainData[5] = new double[] { 5.0, 7.0, 1 }; 
            trainData[6] = new double[] { 5.5, 8.0, 1 }; 
            trainData[7] = new double[] { 6.0, 6.0, 1 };

            int numInput = 2;

            Perceptron p = new Perceptron(numInput);
            double alpha = 0.001;
            int maxEpochs = 100;

            double[] weights = p.Train(trainData, alpha, maxEpochs);







        }

    }

    #region  感知器

    /// <summary>
    /// 感知器模拟单个神经元，神经元的输入模拟直接信号或者另外一个神经元的输出。
    /// 感知器需要找出一系列weights和一个bias，用来计算输出。
    /// </summary>
    public class Perceptron
    {
        /// <summary>
        /// 输入x data的个数
        /// </summary>
        private int numInput;
        /// <summary>
        /// 表示x datas 
        /// </summary>
        public double[] inputs;
        /// <summary>
        /// 用来计算输出结果的权重
        /// </summary>
        public double[] weights;
        /// <summary>
        /// 用来计算输出结果的偏差
        /// </summary>
        public double bias;
        /// <summary>
        /// 一个整数标志的最终分类
        /// </summary>
        private int output;
        /// <summary>
        /// 原生的输出，没有应用激励函数的原生输出
        /// </summary>
        private double originOutput;
        /// <summary>
        /// 随机计数器
        /// </summary>
        private Random rnd;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numInput"></param>
        public Perceptron(int numInput)
        {
            this.numInput = numInput;
            this.inputs = new double[numInput];
            this.weights = new double[numInput];
            //设置固定的种子是为了每次运行能够重现
            this.rnd = new Random(0); //new Random();

            //随机初始化weights
            InitializeWeights();
        }

        /// <summary>
        /// 初始化权重和bias
        /// </summary>
        private void InitializeWeights()
        {
            double lo = -0.01;
            double hi = 0.01;
            double range = hi - lo;
            for (int i = 0; i < weights.Length; i++)
            {
                //取值范围在[lo,hi)之间
                weights[i] = range*rnd.NextDouble() + lo;
            }

            bias = range*rnd.NextDouble() + lo;
        }

        /// <summary>
        /// 使用输入和感知器的权重和bias来计算输出
        /// </summary>
        /// <param name="xValues"></param>
        /// <returns></returns>
        public double ComputeOriginOutput(double[] xValues)
        {
            if (xValues.Length < numInput)
            {
                throw new Exception("xValues长度不足");
            }

            for (int i = 0; i < numInput; i++)
            {
                //copy一份xvalues，在实际使用中不要这部分
                this.inputs[i] = xValues[i];
            }

            double result = 0;

            for (int i = 0; i < numInput; i++)
            {
                result += xValues[i]*weights[i];
            }

            result += this.bias;

            this.originOutput = result;
            return result;
        }

        /// <summary>
        /// 计算最终分类结果 一个整数表示
        /// </summary>
        /// <param name="xValues"></param>
        /// <returns></returns>
        public int ComputeOuput(double[] xValues)
        {
            double result =  ComputeOriginOutput(xValues);

            this.output = Activation(result);

            return this.output;
        }

        /// <summary>
        /// 活化函数
        /// 根据一个通过权重和bias计算的分数输出标志最终分类的整数
        /// 该算法表示一个二项分类，-1和1  （0和1是另一种可选的方法）
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private int Activation(double origin)
        {

            if (origin >= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 随机重排
        /// 通常随机重排矩阵会提高正确率
        /// </summary>
        /// <param name="sequences"></param>
        private void Shuffle(int[] sequences)
        {
            int size = sequences.Length;
            for (int i = 0; i < size; i++)
            {
                int r = rnd.Next(i, size);
                int tmp = sequences[r];
                sequences[r] = sequences[i];
                sequences[i] = tmp;
            }
        }

        /// <summary>
        /// 训练过程
        /// </summary>
        /// <param name="trainData">训练数据</param>
        /// <param name="alpha">学习速率</param>
        /// <param name="maxEpochs">最大尝试次数</param>
        /// <returns></returns>
        public double[] Train(double[][] trainData, double alpha, int maxEpochs)
        {
            //当前尝试次数
            int epoch = 0;

            int[] sequence = new int[trainData.Length];
            for (int i = 0; i < sequence.Length; i++)
            {
                sequence[i] = i;
            }

            //当前要处理的数据
            double[] xValues = new double[numInput];
            //要处理数据的目标分类y值
            int desired = 0;

            while (epoch < maxEpochs)
            {
                epoch++;
                //重排，提高正确率
                Shuffle(sequence);

                for (int i = 0; i < trainData.Length; i++)
                {
                    int idx = sequence[i];
                    //原地计算会更好
                    Array.Copy(trainData[idx],xValues,numInput);
                    desired = (int)trainData[idx][numInput];//-1 or 1
                    int computed = ComputeOuput(xValues);
                    Update(computed, desired, alpha);
                }
                //另外一种退出方式是weights和alpha不在变化
            }

            double[] result = new double[numInput + 1]; 
            Array.Copy(this.weights, result, numInput);
            result[result.Length - 1] = bias; // Last cell. 
            return result;
        }

        /// <summary>
        /// 核心算法
        /// 根据计算值和目标值 更新 weights和bias
        /// </summary>
        /// <param name="computed">计算值</param>
        /// <param name="desired">目标值</param>
        /// <param name="alpha">更新速率</param>
        private void Update(int computed, int desired, double alpha)
        {
            if (computed == desired)
            {
                return;//we're good
            }

            int delta = computed - desired;

            for (int i = 0; i < this.weights.Length; i++)
            {
                double diff = System.Math.Abs(alpha*delta*inputs[i]);

                if (delta > 0)
                {
                    weights[i] -= diff;
                }
                else
                {
                    weights[i] += diff;
                }
            }

            //更新bias
            bias -= alpha*delta;
        }

    }

    #endregion
}

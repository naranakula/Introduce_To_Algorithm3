using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading;
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

    #region 反向传播

    /// <summary>
    /// 反向传播
    /// </summary>
    public class BackPropagation
    {

        #region private member
        /// <summary>
        /// 输入节点的数目
        /// </summary>
        private int numInput;
        /// <summary>
        /// 隐藏节点的数目
        /// </summary>
        private int numHidden;
        /// <summary>
        /// 输出节点的数目
        /// </summary>
        private int numOutput;


        private double[] inputs;
        /// <summary>
        /// 从输入节点到隐藏节点的权重
        /// ihWeights[0][2]是从输入节点0到隐藏节点2的权重
        /// </summary>
        private double[][] ihWeights;
        /// <summary>
        /// hBiases[i] = 隐藏节点i的偏差
        /// </summary>
        private double[] hBiases;
        /// <summary>
        /// 输出 隐藏层应用tanh函数之后的输出
        /// </summary>
        private double[] hOutputs;

        /// <summary>
        /// hoWeights[i][j] = 从隐藏节点i到输出节点j的权重
        /// </summary>
        private double[][] hoWeights;
        /// <summary>
        /// 输出偏差
        /// </summary>
        private double[] oBiases;
        /// <summary>
        /// 输出  最终的输出
        /// </summary>
        private double[] outputs;

        /// <summary>
        /// Output gradients for back-propagation
        /// 个数为numOutput
        /// 输出层的梯度 = softmax的导数*(desired - computed)
        /// softmax的导数 = x*(1-x)
        /// </summary>
        private double[] oGrads;

        /// <summary>
        /// Hidden gradients for back-propagation
        /// 个数为numHidden
        /// 隐藏层的梯度 = tanh的导数*(∑[输出梯度*howeights])
        /// </summary>
        private double[] hGrads;

        //delta值加到原来的weights和bias上产生新的weights和bias
        /// <summary>
        /// 负向反馈的动量
        /// 输入 隐藏结点weights的delta
        /// delta = learning rate * downstream gradient*upstream input
        /// 另外 还可以加上momentum动能因子  = momentum常数*上次迭代的delta
        /// </summary>
        private double[][] ihPrevWeightsDelta; // For momentum with back-propagation. 
        /// <summary>
        /// 隐藏结点bias的delta
        /// delta = learning rate * downstream gradient
        /// 另外 还可以加上momentum动能因子  = momentum常数*上次迭代的delta
        /// </summary>
        private double[] hPrevBiasesDelta;
        /// <summary>
        /// 隐藏层 输出层  的weights的delta
        /// delta = learning rate * downstream gradient*upstream input
        /// 另外 还可以加上momentum动能因子  = momentum常数*上次迭代的delta
        /// </summary>
        private double[][] hoPrevWeightsDelta;
        /// <summary>
        /// 输出层的bias的delta
        /// delta = learning rate * downstream gradient
        /// 另外 还可以加上momentum动能因子  = momentum常数*上次迭代的delta
        /// </summary>
        private double[] oPrevBiasesDelta;

        /// <summary>
        /// 随机数
        /// </summary>
        private Random rnd;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numInput">输入项个数</param>
        /// <param name="numHidden">隐藏项个数</param>
        /// <param name="numOutput">输出项个数</param>
        public BackPropagation(int numInput, int numHidden, int numOutput)
        {
            this.numInput = numInput;
            this.numHidden = numHidden;
            this.numOutput = numOutput;

            this.inputs = new double[numInput];
            this.ihWeights = MakeMatrix(numInput, numHidden);
            this.hBiases = new double[numHidden];
            this.hOutputs = new double[numHidden];

            this.hoWeights = MakeMatrix(numHidden, numOutput);
            this.oBiases = new double[numOutput];
            this.outputs = new double[numOutput];

            oGrads = new double[numOutput];
            hGrads = new double[numHidden];

            ihPrevWeightsDelta = MakeMatrix(numInput, numHidden);
            hPrevBiasesDelta = new double[numHidden];
            hoPrevWeightsDelta = MakeMatrix(numHidden, numOutput);
            oPrevBiasesDelta = new double[numOutput];

            InitMatrix(ihPrevWeightsDelta,0.011);
            InitVector(hPrevBiasesDelta,0.011);
            InitMatrix(hoPrevWeightsDelta,0.011);
            InitVector(oPrevBiasesDelta,0.011);
        }


        /// <summary>
        /// 设置权重
        /// </summary>
        /// <param name="weights"></param>
        private void SetWeights(double[] weights)
        {
            //Pointer into weights parameter
            int k = 0;

            //input to hidden weights
            for (int i = 0; i < numInput; i++)
            {
                for (int j = 0; j < numHidden; j++)
                {
                    ihWeights[i][j] = weights[k++];
                }
            }

            //hidden bias
            for (int i = 0; i < numHidden; i++)
            {
                hBiases[i] = weights[k++];
            }

            //hidden to output
            for (int i = 0; i < numHidden; i++)
            {
                for (int j = 0; j < numOutput; j++)
                {
                    hoWeights[i][j] = weights[k++];
                }
            }

            //output bias
            for (int i = 0; i < numOutput; i++)
            {
                oBiases[i] = weights[k++];
            }

        }

        /// <summary>
        /// 训练找到权重
        /// </summary>
        /// <param name="tValues"></param>
        /// <param name="xValues"></param>
        /// <param name="learnRate"></param>
        /// <param name="momentum"></param>
        /// <param name="maxEpochs"></param>
        private void FindWeights(double[][] tValues, double[][] xValues, double learnRate, double momentum, int maxEpochs)
        {
            /*
             * 循环执行，直到退出条件满足
             *    FeedForward
             *    BackPropagation
             */

            int epoch = 0;
            while (epoch <= maxEpochs)
            {
                //迭代了 maxEpochs+1 次，另一种方式是迭代直到weights不再改变
                int rows = xValues.Length;
                for (int i = 0; i < rows; i++)
                {
                    //随机重排训练数据，会提高正确率
                    double[] yValues = ComputeOutputs(xValues[i]);
                    //yValues保存到了outputs中
                    UpdateWeights(tValues[i], learnRate, momentum);
                }
                epoch++;
            }
        }

        /// <summary>
        /// 更新权重
        /// </summary>
        /// <param name="tValues"></param>
        /// <param name="learnRate"></param>
        /// <param name="momentum">动量</param>
        private void UpdateWeights(double[] tValues, double learnRate, double momentum)
        {
            if (tValues.Length != numOutput)
            {
                throw  new Exception("target array's length should be numOutput");
            }

            //计算输出的 梯度  assumes softmax
            for (int i = 0; i < oGrads.Length; i++)
            {
                double derivative = (1 - outputs[i])*outputs[i];//softmax的导数 y(1-y)
                oGrads[i] = derivative*(tValues[i] - outputs[i]);
            }

            //计算hidden layer的gradient  assume tanh
            for (int i = 0; i < hGrads.Length; i++)
            {
                double derivative = (1 - hOutputs[i])*(1 + hOutputs[i]);//计算 hyperbolic tangent函数的导数  (1-y)(1+y)
                double sum = 0;
                //求 downstream gradient*hidden-to-output weight 的和
                for (int j = 0; j < numOutput; j++)
                {
                    sum += oGrads[j]*hoWeights[i][j];
                }
                hGrads[i] = derivative*sum;
            }

            //更新input to hidden的weights
            for (int i = 0; i < ihWeights.Length; i++)
            {
                for (int j = 0; j < ihWeights[i].Length; j++)
                {
                    double delta = learnRate*hGrads[j]*inputs[i];
                    ihWeights[i][j] += delta;
                    ihWeights[i][j] += momentum*ihPrevWeightsDelta[i][j];
                    ihPrevWeightsDelta[i][j] = delta;//save the delta
                }
            }

            //更新hidden的bias
            for (int i = 0; i < hBiases.Length; i++)
            {
                double delta = learnRate*hGrads[i];
                hBiases[i] += delta;
                hBiases[i] += momentum*hPrevBiasesDelta[i];
                hPrevBiasesDelta[i] = delta;//save delta
            }

            //更新hidden to output 的 weights
            for (int i = 0; i < hoWeights.Length; i++)
            {
                for (int j = 0; j < hoWeights[i].Length; j++)
                {
                    double delta = learnRate*oGrads[j]*hOutputs[i];
                    hoWeights[i][j] += delta;
                    hoWeights[i][j] += momentum*hoPrevWeightsDelta[i][j];
                    hoPrevWeightsDelta[i][j] = delta;//save delta
                }
            }

            //更新 output 的 bias
            for (int i = 0; i < oBiases.Length; i++)
            {
                double delta = learnRate*oGrads[i];
                oBiases[i] += delta;
                oBiases[i] += momentum*oPrevBiasesDelta[i];
                oPrevBiasesDelta[i] = delta;//save delta
            }
        }

        /// <summary>
        /// 获取权重
        /// </summary>
        /// <returns></returns>
        private double[] GetWeights()
        {
            int numWeights = (numInput*numHidden) + numHidden + (numHidden*numOutput) + numOutput;

            double[] result = new double[numWeights];
            int k = 0;

            for (int i = 0; i < numInput; i++)
            {
                for (int j = 0; j < numHidden; j++)
                {
                    result[k++] = ihWeights[i][j];
                }
            }

            for (int i = 0; i < numHidden; i++)
            {
                result[k++] = hBiases[i];
            }

            for (int i = 0; i < numHidden; i++)
            {
                for (int j = 0; j < numOutput; j++)
                {
                    result[k++] = hoWeights[i][j];
                }
            }

            for (int i = 0; i < numOutput; i++)
            {
                result[k++] = oBiases[i];
            }

            return result;
        }

        /// <summary>
        /// 构建一个rows行cols列的矩阵
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        private static double[][] MakeMatrix(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                result[i] = new double[cols];
            }

            return result;
        }

        /// <summary>
        /// 初始化vector
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        private static void InitVector(double[] vector, double value)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = value;
            }
        }

        /// <summary>
        /// 计算输出
        /// </summary>
        /// <param name="xValues"></param>
        /// <returns></returns>
        private double[] ComputeOutputs(double[] xValues)
        {
            double[] hSums = new double[numHidden];
            double[] oSums = new double[numOutput];

            for (int i = 0; i < numInput; i++)
            {
                inputs[i] = xValues[i];
            }

            for (int j = 0; j < numHidden; j++)
            {
                for (int i = 0; i < numInput; i++)
                {
                    hSums[j] += inputs[i]*ihWeights[i][j];
                }
            }

            for (int i = 0; i < numHidden; i++)
            {
                hSums[i] += hBiases[i];
            }

            for (int i = 0; i < numHidden; i++)
            {
                hOutputs[i] = HyperTan(hSums[i]);
            }

            for (int j = 0; j < numOutput; j++)
            {
                for (int i = 0; i < numHidden; i++)
                {
                    oSums[j] += hOutputs[i]*hoWeights[i][j];
                }
            }

            for (int i = 0; i < numOutput; i++)
            {
                oSums[i] += oBiases[i];
            }

            double[] softOut = Softmax(oSums);

            for (int i = 0; i < outputs.Length; i++)
            {
                outputs[i] = softOut[i];
            }

            double[] result = new double[numOutput];
            for (int i = 0; i < outputs.Length; i++)
            {
                result[i] = outputs[i];
            }

            return result;
        }

        /// <summary>
        /// 活化函数
        /// </summary>
        /// <param name="oSums"></param>
        /// <returns></returns>
        private double[] Softmax(double[] oSums)
        {
            double max = oSums[0];
            for (int i = 0; i < oSums.Length; i++)
            {
                if (oSums[i]>max)
                {
                    max = oSums[i];
                }
            }

            double scale = 0;
            for (int i = 0; i < oSums.Length; i++)
            {
                scale += System.Math.Exp(oSums[i] - max);
            }

            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; i++)
            {
                result[i] = System.Math.Exp(oSums[i] - max)/scale;
            }

            return result;
        }

        /// <summary>
        /// tanh
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private double HyperTan(double v)
        {
            if (v < -20)
            {
                return -1;
            }
            else if (v > 20)
            {
                return 1;
            }
            else
            {
                return System.Math.Tanh(v);
            }
        }

        /// <summary>
        /// 初始化matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="value"></param>
        private static void InitMatrix(double[][] matrix, double value)
        {
            int rows = matrix.Length;
            int cols = matrix[0].Length;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i][j] = value;
                }
            }
        }

        public static void TestMain()
        {
            BackPropagation bp = new BackPropagation(3,4,2);

            double[] weights = new double[26]
            {
                0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.09, 0.10, 0.11, 0.12, 0.13, 0.14, 0.15, 0.16, 0.17, 0.18, 0.19, 0.20, 0.21, 0.22, 0.23, 0.24, 0.25, 0.26
            };

            bp.SetWeights(weights);
            
            //学习速率
            double learnRate = 0.05; 
            //动量 早期的back-propagation没有使用momentum，使用动量主要是为了当学习速率较小是加速训练，设置动量为0，可以忽略该项。
            double momentum = 0.01; 
            //最大尝试次数
            int maxEpochs = 1000;

            //每个输入对应一个输出
            double[][] xValues = new double[1][];
            xValues[0] = new double[]{ 1.0, 2.0, 3.0 }; // Inputs. 
            double[][] tValues = new double[1][];
            tValues[0] = new double[]{ 0.2500, 0.7500 }; // Target outputs.

            bp.FindWeights(tValues, xValues, learnRate, momentum, maxEpochs);
            double[] bestWeights = bp.GetWeights();
        }



    }
    
    #endregion

    #region 增量训练
    
    /// <summary>
    /// 对于每一个训练项，计算一个误差，用来更新bias和weights
    /// </summary>
    public class IncrementalTraining
    {
        private int numInput;
        private int numHidden;
        private int numOutput;

        private static Random rnd;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numInput"></param>
        /// <param name="numHidden"></param>
        /// <param name="numOutput"></param>
        public IncrementalTraining(int numInput, int numHidden, int numOutput)
        {
            
        }


        /// <summary>
        /// 训练
        /// </summary>
        /// <param name="trainData"></param>
        /// <param name="maxEpochs"></param>
        /// <param name="learnRate"></param>
        /// <param name="momentum"></param>
        private void Train(double[][] trainData, int maxEpochs, double learnRate, double momentum)
        {
            throw new NotImplementedException();
        }




        public static void TestMain()
        {
            Console.WriteLine("\nBegin neural network training demo");
            Console.WriteLine("\nData is the famous Iris flower set."); 
            Console.WriteLine("Predict species from sepal length, width, petal length, width"); 
            Console.WriteLine("Iris setosa = 0 0 1, versicolor = 0 1 0, virginica = 1 0 0 \n");
            Console.WriteLine("Raw data resembles:"); 
            Console.WriteLine(" 5.1, 3.5, 1.4, 0.2, Iris setosa"); 
            Console.WriteLine(" 7.0, 3.2, 4.7, 1.4, Iris versicolor"); 
            Console.WriteLine(" 6.3, 3.3, 6.0, 2.5, Iris virginica"); 
            Console.WriteLine(" ......\n"); 
            double[][] allData = new double[150][]; 
            allData[0] = new double[] { 5.1, 3.5, 1.4, 0.2, 0, 0, 1 };
            allData[1] = new double[] { 4.9, 3.0, 1.4, 0.2, 0, 0, 1 }; // Iris setosa = 0 0 1 
            allData[2] = new double[] { 4.7, 3.2, 1.3, 0.2, 0, 0, 1 }; // Iris versicolor = 0 1 0
            allData[3] = new double[] { 4.6, 3.1, 1.5, 0.2, 0, 0, 1 }; // Iris virginica = 1 0 0 
            allData[4] = new double[] { 5.0, 3.6, 1.4, 0.2, 0, 0, 1 };
            allData[5] = new double[] { 5.4, 3.9, 1.7, 0.4, 0, 0, 1 };
            allData[6] = new double[] { 4.6, 3.4, 1.4, 0.3, 0, 0, 1 };
            allData[7] = new double[] { 5.0, 3.4, 1.5, 0.2, 0, 0, 1 };
            allData[8] = new double[] { 4.4, 2.9, 1.4, 0.2, 0, 0, 1 };
            allData[9] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };
            allData[10] = new double[] { 5.4, 3.7, 1.5, 0.2, 0, 0, 1 };
            allData[11] = new double[] { 4.8, 3.4, 1.6, 0.2, 0, 0, 1 };
            allData[12] = new double[] { 4.8, 3.0, 1.4, 0.1, 0, 0, 1 };
            allData[13] = new double[] { 4.3, 3.0, 1.1, 0.1, 0, 0, 1 };
            allData[14] = new double[] { 5.8, 4.0, 1.2, 0.2, 0, 0, 1 };
            allData[15] = new double[] { 5.7, 4.4, 1.5, 0.4, 0, 0, 1 };
            allData[16] = new double[] { 5.4, 3.9, 1.3, 0.4, 0, 0, 1 };
            allData[17] = new double[] { 5.1, 3.5, 1.4, 0.3, 0, 0, 1 };
            allData[18] = new double[] { 5.7, 3.8, 1.7, 0.3, 0, 0, 1 };
            allData[19] = new double[] { 5.1, 3.8, 1.5, 0.3, 0, 0, 1 };
            allData[20] = new double[] { 5.4, 3.4, 1.7, 0.2, 0, 0, 1 };
            allData[21] = new double[] { 5.1, 3.7, 1.5, 0.4, 0, 0, 1 };
            allData[22] = new double[] { 4.6, 3.6, 1.0, 0.2, 0, 0, 1 };
            allData[23] = new double[] { 5.1, 3.3, 1.7, 0.5, 0, 0, 1 };
            allData[24] = new double[] { 4.8, 3.4, 1.9, 0.2, 0, 0, 1 };
            allData[25] = new double[] { 5.0, 3.0, 1.6, 0.2, 0, 0, 1 };
            allData[26] = new double[] { 5.0, 3.4, 1.6, 0.4, 0, 0, 1 };
            allData[27] = new double[] { 5.2, 3.5, 1.5, 0.2, 0, 0, 1 };
            allData[28] = new double[] { 5.2, 3.4, 1.4, 0.2, 0, 0, 1 };
            allData[29] = new double[] { 4.7, 3.2, 1.6, 0.2, 0, 0, 1 };
            allData[30] = new double[] { 4.8, 3.1, 1.6, 0.2, 0, 0, 1 };
            allData[31] = new double[] { 5.4, 3.4, 1.5, 0.4, 0, 0, 1 };
            allData[32] = new double[] { 5.2, 4.1, 1.5, 0.1, 0, 0, 1 };
            allData[33] = new double[] { 5.5, 4.2, 1.4, 0.2, 0, 0, 1 };
            allData[34] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };
            allData[35] = new double[] { 5.0, 3.2, 1.2, 0.2, 0, 0, 1 };
            allData[36] = new double[] { 5.5, 3.5, 1.3, 0.2, 0, 0, 1 };
            allData[37] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };
            allData[38] = new double[] { 4.4, 3.0, 1.3, 0.2, 0, 0, 1 };
            allData[39] = new double[] { 5.1, 3.4, 1.5, 0.2, 0, 0, 1 };
            allData[40] = new double[] { 5.0, 3.5, 1.3, 0.3, 0, 0, 1 };
            allData[41] = new double[] { 4.5, 2.3, 1.3, 0.3, 0, 0, 1 };
            allData[42] = new double[] { 4.4, 3.2, 1.3, 0.2, 0, 0, 1 };
            allData[43] = new double[] { 5.0, 3.5, 1.6, 0.6, 0, 0, 1 };
            allData[44] = new double[] { 5.1, 3.8, 1.9, 0.4, 0, 0, 1 };
            allData[45] = new double[] { 4.8, 3.0, 1.4, 0.3, 0, 0, 1 };
            allData[46] = new double[] { 5.1, 3.8, 1.6, 0.2, 0, 0, 1 };
            allData[47] = new double[] { 4.6, 3.2, 1.4, 0.2, 0, 0, 1 };
            allData[48] = new double[] { 5.3, 3.7, 1.5, 0.2, 0, 0, 1 };
            allData[49] = new double[] { 5.0, 3.3, 1.4, 0.2, 0, 0, 1 };
            allData[50] = new double[] { 7.0, 3.2, 4.7, 1.4, 0, 1, 0 };
            allData[51] = new double[] { 6.4, 3.2, 4.5, 1.5, 0, 1, 0 };
            allData[52] = new double[] { 6.9, 3.1, 4.9, 1.5, 0, 1, 0 };
            allData[53] = new double[] { 5.5, 2.3, 4.0, 1.3, 0, 1, 0 };
            allData[54] = new double[] { 6.5, 2.8, 4.6, 1.5, 0, 1, 0 };
            allData[55] = new double[] { 5.7, 2.8, 4.5, 1.3, 0, 1, 0 };
            allData[56] = new double[] { 6.3, 3.3, 4.7, 1.6, 0, 1, 0 };
            allData[57] = new double[] { 4.9, 2.4, 3.3, 1.0, 0, 1, 0 };
            allData[58] = new double[] { 6.6, 2.9, 4.6, 1.3, 0, 1, 0 };
            allData[59] = new double[] { 5.2, 2.7, 3.9, 1.4, 0, 1, 0 };
            allData[60] = new double[] { 5.0, 2.0, 3.5, 1.0, 0, 1, 0 };
            allData[61] = new double[] { 5.9, 3.0, 4.2, 1.5, 0, 1, 0 };
            allData[62] = new double[] { 6.0, 2.2, 4.0, 1.0, 0, 1, 0 };
            allData[63] = new double[] { 6.1, 2.9, 4.7, 1.4, 0, 1, 0 };
            allData[64] = new double[] { 5.6, 2.9, 3.6, 1.3, 0, 1, 0 };
            allData[65] = new double[] { 6.7, 3.1, 4.4, 1.4, 0, 1, 0 };
            allData[66] = new double[] { 5.6, 3.0, 4.5, 1.5, 0, 1, 0 };
            allData[67] = new double[] { 5.8, 2.7, 4.1, 1.0, 0, 1, 0 };
            allData[68] = new double[] { 6.2, 2.2, 4.5, 1.5, 0, 1, 0 };
            allData[69] = new double[] { 5.6, 2.5, 3.9, 1.1, 0, 1, 0 };
            allData[70] = new double[] { 5.9, 3.2, 4.8, 1.8, 0, 1, 0 };
            allData[71] = new double[] { 6.1, 2.8, 4.0, 1.3, 0, 1, 0 };
            allData[72] = new double[] { 6.3, 2.5, 4.9, 1.5, 0, 1, 0 };
            allData[73] = new double[] { 6.1, 2.8, 4.7, 1.2, 0, 1, 0 };
            allData[74] = new double[] { 6.4, 2.9, 4.3, 1.3, 0, 1, 0 };
            allData[75] = new double[] { 6.6, 3.0, 4.4, 1.4, 0, 1, 0 };
            allData[76] = new double[] { 6.8, 2.8, 4.8, 1.4, 0, 1, 0 };
            allData[77] = new double[] { 6.7, 3.0, 5.0, 1.7, 0, 1, 0 };
            allData[78] = new double[] { 6.0, 2.9, 4.5, 1.5, 0, 1, 0 };
            allData[79] = new double[] { 5.7, 2.6, 3.5, 1.0, 0, 1, 0 };
            allData[80] = new double[] { 5.5, 2.4, 3.8, 1.1, 0, 1, 0 };
            allData[81] = new double[] { 5.5, 2.4, 3.7, 1.0, 0, 1, 0 };
            allData[82] = new double[] { 5.8, 2.7, 3.9, 1.2, 0, 1, 0 };
            allData[83] = new double[] { 6.0, 2.7, 5.1, 1.6, 0, 1, 0 };
            allData[84] = new double[] { 5.4, 3.0, 4.5, 1.5, 0, 1, 0 };
            allData[85] = new double[] { 6.0, 3.4, 4.5, 1.6, 0, 1, 0 };
            allData[86] = new double[] { 6.7, 3.1, 4.7, 1.5, 0, 1, 0 };
            allData[87] = new double[] { 6.3, 2.3, 4.4, 1.3, 0, 1, 0 };
            allData[88] = new double[] { 5.6, 3.0, 4.1, 1.3, 0, 1, 0 };
            allData[89] = new double[] { 5.5, 2.5, 4.0, 1.3, 0, 1, 0 };
            allData[90] = new double[] { 5.5, 2.6, 4.4, 1.2, 0, 1, 0 };
            allData[91] = new double[] { 6.1, 3.0, 4.6, 1.4, 0, 1, 0 };
            allData[92] = new double[] { 5.8, 2.6, 4.0, 1.2, 0, 1, 0 };
            allData[93] = new double[] { 5.0, 2.3, 3.3, 1.0, 0, 1, 0 };
            allData[94] = new double[] { 5.6, 2.7, 4.2, 1.3, 0, 1, 0 };
            allData[95] = new double[] { 5.7, 3.0, 4.2, 1.2, 0, 1, 0 };
            allData[96] = new double[] { 5.7, 2.9, 4.2, 1.3, 0, 1, 0 };
            allData[97] = new double[] { 6.2, 2.9, 4.3, 1.3, 0, 1, 0 };
            allData[98] = new double[] { 5.1, 2.5, 3.0, 1.1, 0, 1, 0 };
            allData[99] = new double[] { 5.7, 2.8, 4.1, 1.3, 0, 1, 0 };
            allData[100] = new double[] { 6.3, 3.3, 6.0, 2.5, 1, 0, 0 };
            allData[101] = new double[] { 5.8, 2.7, 5.1, 1.9, 1, 0, 0 };
            allData[102] = new double[] { 7.1, 3.0, 5.9, 2.1, 1, 0, 0 };
            allData[103] = new double[] { 6.3, 2.9, 5.6, 1.8, 1, 0, 0 };
            allData[104] = new double[] { 6.5, 3.0, 5.8, 2.2, 1, 0, 0 };
            allData[105] = new double[] { 7.6, 3.0, 6.6, 2.1, 1, 0, 0 };
            allData[106] = new double[] { 4.9, 2.5, 4.5, 1.7, 1, 0, 0 };
            allData[107] = new double[] { 7.3, 2.9, 6.3, 1.8, 1, 0, 0 };
            allData[108] = new double[] { 6.7, 2.5, 5.8, 1.8, 1, 0, 0 };
            allData[109] = new double[] { 7.2, 3.6, 6.1, 2.5, 1, 0, 0 };
            allData[110] = new double[] { 6.5, 3.2, 5.1, 2.0, 1, 0, 0 };
            allData[111] = new double[] { 6.4, 2.7, 5.3, 1.9, 1, 0, 0 };
            allData[112] = new double[] { 6.8, 3.0, 5.5, 2.1, 1, 0, 0 };
            allData[113] = new double[] { 5.7, 2.5, 5.0, 2.0, 1, 0, 0 };
            allData[114] = new double[] { 5.8, 2.8, 5.1, 2.4, 1, 0, 0 };
            allData[115] = new double[] { 6.4, 3.2, 5.3, 2.3, 1, 0, 0 };
            allData[116] = new double[] { 6.5, 3.0, 5.5, 1.8, 1, 0, 0 };
            allData[117] = new double[] { 7.7, 3.8, 6.7, 2.2, 1, 0, 0 };
            allData[118] = new double[] { 7.7, 2.6, 6.9, 2.3, 1, 0, 0 };
            allData[119] = new double[] { 6.0, 2.2, 5.0, 1.5, 1, 0, 0 };
            allData[120] = new double[] { 6.9, 3.2, 5.7, 2.3, 1, 0, 0 };
            allData[121] = new double[] { 5.6, 2.8, 4.9, 2.0, 1, 0, 0 };
            allData[122] = new double[] { 7.7, 2.8, 6.7, 2.0, 1, 0, 0 };
            allData[123] = new double[] { 6.3, 2.7, 4.9, 1.8, 1, 0, 0 };
            allData[124] = new double[] { 6.7, 3.3, 5.7, 2.1, 1, 0, 0 };
            allData[125] = new double[] { 7.2, 3.2, 6.0, 1.8, 1, 0, 0 };
            allData[126] = new double[] { 6.2, 2.8, 4.8, 1.8, 1, 0, 0 };
            allData[127] = new double[] { 6.1, 3.0, 4.9, 1.8, 1, 0, 0 };
            allData[128] = new double[] { 6.4, 2.8, 5.6, 2.1, 1, 0, 0 };
            allData[129] = new double[] { 7.2, 3.0, 5.8, 1.6, 1, 0, 0 };
            allData[130] = new double[] { 7.4, 2.8, 6.1, 1.9, 1, 0, 0 };
            allData[131] = new double[] { 7.9, 3.8, 6.4, 2.0, 1, 0, 0 };
            allData[132] = new double[] { 6.4, 2.8, 5.6, 2.2, 1, 0, 0 };
            allData[133] = new double[] { 6.3, 2.8, 5.1, 1.5, 1, 0, 0 };
            allData[134] = new double[] { 6.1, 2.6, 5.6, 1.4, 1, 0, 0 };
            allData[135] = new double[] { 7.7, 3.0, 6.1, 2.3, 1, 0, 0 };
            allData[136] = new double[] { 6.3, 3.4, 5.6, 2.4, 1, 0, 0 };
            allData[137] = new double[] { 6.4, 3.1, 5.5, 1.8, 1, 0, 0 };
            allData[138] = new double[] { 6.0, 3.0, 4.8, 1.8, 1, 0, 0 };
            allData[139] = new double[] { 6.9, 3.1, 5.4, 2.1, 1, 0, 0 };
            allData[140] = new double[] { 6.7, 3.1, 5.6, 2.4, 1, 0, 0 };
            allData[141] = new double[] { 6.9, 3.1, 5.1, 2.3, 1, 0, 0 };
            allData[142] = new double[] { 5.8, 2.7, 5.1, 1.9, 1, 0, 0 };
            allData[143] = new double[] { 6.8, 3.2, 5.9, 2.3, 1, 0, 0 };
            allData[144] = new double[] { 6.7, 3.3, 5.7, 2.5, 1, 0, 0 };
            allData[145] = new double[] { 6.7, 3.0, 5.2, 2.3, 1, 0, 0 };
            allData[146] = new double[] { 6.3, 2.5, 5.0, 1.9, 1, 0, 0 };
            allData[147] = new double[] { 6.5, 3.0, 5.2, 2.0, 1, 0, 0 };
            allData[148] = new double[] { 6.2, 3.4, 5.4, 2.3, 1, 0, 0 };
            allData[149] = new double[] { 5.9, 3.0, 5.1, 1.8, 1, 0, 0 };

            Console.WriteLine("Creating 80% training and 20% test data matrices"); 
            double[][] trainData = null; 
            double[][] testData = null; 
            MakeTrainTest(allData, 72, out trainData, out testData); // seed = 72 gives a pretty demo.

            IncrementalTraining it = new IncrementalTraining(4,7,3);

            int maxEpochs = 1000;
            double learnRate = 0.05;
            double momentum = 0.01;
            it.Train(trainData, maxEpochs, learnRate, momentum);

        }

        /// <summary>
        /// 将数据分为trainPercent比例训练数据，1-trainPercent  测试数据
        /// </summary>
        /// <param name="data">所有的数据</param>
        /// <param name="seed">随机数数据</param>
        /// <param name="trainData">输出训练数据</param>
        /// <param name="testData">输出测试数据</param>
        /// <param name="trainPercent">训练数据的比例</param>
        private static void MakeTrainTest(double[][] data, int seed, out double[][] trainData, out double[][] testData, double trainPercent=0.8)
        {
            Random rnd = new Random(seed);
            int totalRows = data.Length;
            int numTrainRows = (int)(totalRows * trainPercent);
            int numTestRows = totalRows - numTrainRows;

            double[][] copy = new double[totalRows][];
            for (int i = 0; i < copy.Length; i++)
            {
                copy[i] = data[i];
            }

            //随机重排
            for (int i = 0; i < copy.Length; i++)
            {
                int r = rnd.Next(i, copy.Length);
                double[] tmp = copy[r];
                copy[r] = copy[i];
                copy[i] = tmp;
            }

            trainData = new double[numTrainRows][];
            for (int i = 0; i < numTrainRows; i++)
            {
                trainData[i] = copy[i];
            }

            testData = new double[numTestRows][];
            for (int i = 0; i < numTestRows; i++)
            {
                testData[i] = copy[i + numTrainRows];
            }
        }

    }

    #endregion

}

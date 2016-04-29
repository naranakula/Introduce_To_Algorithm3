using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// 神经网络分类器
    /// 
    /// 使用粒子群优化算法
    /// Machine Learning using C# succinctly.pdf
    /// 另一种方式是反向传播（这种未实现）
    /// </summary>
    public class NeuralNetworkClassifier
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
        /// 随机数
        /// </summary>
        private Random rnd;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numInput"></param>
        /// <param name="numHidden"></param>
        /// <param name="numOutput"></param>
        public NeuralNetworkClassifier(int numInput, int numHidden, int numOutput)
        {
            // TODO: Complete member initialization
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
            //实际情况下，应该是随机的
            this.rnd = new Random(0);
        }

        #endregion

        #region 公共方法

        private double[][] MakeMatrix(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int r = 0; r < result.Length; ++r)
            {
                result[r] = new double[cols];
            }

            return result;
        }

        /// <summary>
        /// 核心方法
        /// 使用粒子群优化算法来进行训练
        /// </summary>
        /// <param name="trainData">训练数据</param>
        /// <param name="numParticles">例子数</param>
        /// <param name="maxEpochs">最大重试次数</param>
        /// <returns></returns>
        private double[] Train(double[][] trainData, int numParticles, int maxEpochs)
        {
            int numWeights = (this.numInput*this.numHidden) + this.numHidden + (this.numHidden*this.numOutput) + this.numOutput;//不同位置的weights和biases

            //设置相关本地变量
            int epoch = 0;//当前尝试次数
            double minX = -10;//每个权重最小值  通常只有归一化后才做限制
            double maxX = 10;
            double w = 0.729;//惯性权重 限制点移动的幅度
            double c1 = 1.49445;//感知权重 当前粒子最优位置的影响程度
            double c2 = 1.49445;//全局权重  全局粒子最优位置的影响程度
            double r1, r2;//[0,1)的感知或全局随机

            Particle[] swarm = new Particle[numParticles];
            double[] bestGlobalPosition = new double[numWeights];
            double bestGlobalError = double.MaxValue;

            for (int i = 0; i < swarm.Length; i++)
            {
                double[] randomPosition = new double[numWeights];
                for (int j = 0; j < randomPosition.Length; j++)
                {
                    randomPosition[j] = (maxX - minX)*rnd.NextDouble() + minX;//[-10.10)的范围
                }

                double error = MeanSquaredError(trainData, randomPosition);
                double[] randomVelocity = new double[numWeights];//方向
                for (int j = 0; j < randomVelocity.Length; j++)
                {
                    double lo = 0.1*minX;
                    double hi = 0.1*maxX;
                    randomVelocity[j] = (hi - lo)*rnd.NextDouble() + lo;//[-1,1)之间
                }

                swarm[i] = new Particle(randomPosition,error,randomVelocity,randomPosition,error);

                //当前粒子是否有最好的位置
                if (swarm[i].error < bestGlobalError)
                {
                    bestGlobalError = swarm[i].error;
                    swarm[i].position.CopyTo(bestGlobalPosition,0);
                }
            }

            //处理粒子随机化
            int[] sequence = new int[numParticles];
            for (int i = 0; i < sequence.Length; i++)
            {
                sequence[i] = i;
            }

            //算法开始
            while (epoch < maxEpochs)
            {
                epoch++;
                double[] newVelocity = new double[numWeights];
                double[] newPosition = new double[numWeights];
                double newError;

                Shuffle(sequence);

                for (int pi = 0; pi < swarm.Length; pi++)
                {
                    int i = sequence[pi];
                    Particle currP = swarm[i];

                    for (int j = 0; j < currP.velocity.Length; j++)
                    {
                        r1 = rnd.NextDouble();
                        r2 = rnd.NextDouble();
                        newVelocity[j] = (w*currP.velocity[j]) + (c1*r1*(currP.bestPosition[j]-currP.position[j])) + (c2*r2*(bestGlobalPosition[j]-currP.position[j]));
                    }
                    newVelocity.CopyTo(currP.velocity,0);

                    //计算新的position
                    for (int j = 0; j < currP.position.Length; j++)
                    {
                        newPosition[j] = currP.position[j] + newVelocity[j];

                        if (newPosition[j] < minX)
                        {
                            newPosition[j] = minX;
                        }
                        else if (newPosition[j] > maxX)
                        {
                            newPosition[j] = maxX;
                        }
                    }

                    newPosition.CopyTo(currP.position,0);

                    //更新错误评估
                    newError = MeanSquaredError(trainData, newPosition);
                    currP.error = newError;
                    if (newError < currP.bestError)
                    {
                        newPosition.CopyTo(currP.bestPosition,0);
                        currP.bestError = newError;
                    }

                    if (newError < bestGlobalError)
                    {
                        newPosition.CopyTo(bestGlobalPosition,0);
                        bestGlobalError = newError;
                    }

                }

                


                //一种提早退出的方式是设置exitError，当bestGlobalError<exitError时，退出
            }


            SetWeights(bestGlobalPosition); // best position is a set of weights
            double[] retResult = new double[numWeights];
            Array.Copy(bestGlobalPosition, retResult, retResult.Length);
            return retResult; ;
        }

        private double MeanSquaredError(double[][] trainData, double[] weights)
        {
            //设置权重
            this.SetWeights(weights);

            double[] xValues = new double[numInput]; // inputs 
            double[] tValues = new double[numOutput]; // targets
            double sumSquaredError = 0;
            for (int i = 0; i < trainData.Length; i++)
            {
                //following assumes data has all x-values first, followed by y-values!
                Array.Copy(trainData[i],xValues,numInput);//提取输入
                Array.Copy(trainData[i],numInput,tValues,0,numOutput);//提取目标值
                double[] yValues = this.ComputeOutputs(xValues);
                for (int j = 0; j < yValues.Length; j++)
                {
                    sumSquaredError += (yValues[j] - tValues[j])*(yValues[j] - tValues[j]);
                }
            }
            return sumSquaredError/trainData.Length;
        }

        /// <summary>
        /// 重排
        /// </summary>
        /// <param name="sequence"></param>
        private void Shuffle(int[] sequence)
        {
            for (int i = 0; i < sequence.Length; i++)
            {
                int ri = rnd.Next(i, sequence.Length);
                int tmp = sequence[ri];
                sequence[ri] = sequence[i];
                sequence[i] = tmp;
            }
        }


        private void SetWeights(double[] weights)
        {
            //Copy weights and biases in weights[] array to i-h weights,i-h biases,h-o weights, h-o biases

            int numWeights = (numInput*numHidden) + (numHidden*numOutput) + numHidden + numOutput;

            if (weights.Length != numWeights)
            {
                throw  new Exception("Bad weights array length");
            }

            int k = 0;

            for (int i = 0; i < numInput; i++)
            {
                for (int j = 0; j < numHidden; j++)
                {
                    ihWeights[i][j] = weights[k++];
                }
            }

            for (int i = 0; i < numHidden; i++)
            {
                hBiases[i] = weights[k++];
            }

            for (int i = 0; i < numHidden; i++)
            {
                for (int j = 0; j < numOutput; j++)
                {
                    hoWeights[i][j] = weights[k++];
                }
            }

            for (int i = 0; i < numOutput; i++)
            {
                oBiases[i] = weights[k++];
            }

        }

        /// <summary>
        /// 计算精确度
        /// </summary>
        /// <param name="testData"></param>
        /// <returns></returns>
        private double Accuracy(double[][] testData)
        {
            int numCorrect = 0;
            int numWrong = 0;

            double[] xValues = new double[numInput];//输入
            double[] tValues = new double[numOutput];//目标值
            double[] yValues;//计算的y

            for (int i = 0; i < testData.Length; i++)
            {
                Array.Copy(testData[i], xValues, numInput); // parse test data 
                Array.Copy(testData[i], numInput, tValues, 0, numOutput);

                yValues = this.ComputeOutputs(xValues);
                int maxIndex = MaxIndex(yValues); // which cell in yValues has largest value?

                if (System.Math.Abs(tValues[maxIndex] - 1) < 0.00001)
                {
                    ++numCorrect;
                }
                else
                {
                    ++numWrong;
                }
            }

            return (numCorrect*1.0)/(numCorrect + numWrong);
        }

        private int MaxIndex(double[] vector)
        {
            // index of largest value 
            int bigIndex = 0; 
            double biggestVal = vector[0];
            for (int i = 0; i < vector.Length; ++i)
            {
                if (vector[i] > biggestVal)
                {
                    biggestVal = vector[i]; 
                    bigIndex = i;
                }
            } 
            return bigIndex;
        }

        /// <summary>
        /// 计算输出
        /// </summary>
        /// <param name="xValues"></param>
        /// <returns></returns>
        public double[] ComputeOutputs(double[] xValues)
        {
            double[] hSums = new double[numHidden]; // hidden nodes sums scratch array
            double[] oSums = new double[numOutput]; // output nodes sums

            for (int i = 0; i < xValues.Length; i++)
            {
                this.inputs[i] = xValues[i];
            }

            for (int j = 0; j < numHidden; j++)
            {
                for (int i = 0; i < numInput; i++)
                {
                    hSums[j] += this.inputs[i]*this.ihWeights[i][j];
                }
            }

            for (int i = 0; i < numHidden; i++)
            {
                hSums[i] += this.hBiases[i];
            }

            for (int i = 0; i < numHidden; i++)
            {
                this.hOutputs[i] = HyperTan(hSums[i]);
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
            Array.Copy(softOut,outputs,softOut.Length);

            double[] retResult = new double[numOutput];
            Array.Copy(this.outputs, retResult, retResult.Length);
            return retResult;
        }

        /// <summary>
        /// 计算softmax
        /// </summary>
        /// <param name="oSums"></param>
        /// <returns></returns>
        private double[] Softmax(double[] oSums)
        {
            //determine max output-sum
            double max = oSums[0];
            for (int i = 0; i < oSums.Length; i++)
            {
                if (oSums[i]>max)
                {
                    max = oSums[i];
                }
            }

            //determine scaling factor
            double scale = 0;
            for (int i = 0; i < oSums.Length; i++)
            {
                //实际上-max可以约掉，主要是避免超过double的表示范围
                scale += System.Math.Exp(oSums[i]-max);
            }

            double[] result = new double[oSums.Length];

            for (int i = 0; i < oSums.Length; i++)
            {
                result[i] = System.Math.Exp(oSums[i]-max)/scale;
            }

            return result;
        }

        private double HyperTan(double x)
        {
            if (x < -20.0)
            {
                return -1.0; // approximation is correct to 30 decimals
            }
            else if (x > 20.0)
            {
                return 1.0;
            }
            else
            {
                return System.Math.Tanh(x);
            }
        }


        #endregion

        #region 测试

        public static void TestMain()
        {
            Console.WriteLine("Begin neural network demo");
            Console.WriteLine("Goal is to predict species of Iris flower");
            Console.WriteLine("Raw data looks like:");
            Console.WriteLine("blue, 1.4, 0.3, setosa");
            Console.WriteLine("pink, 4.9, 1.5, versicolor");
            Console.WriteLine("teal, 5.6, 1.8, virginica \n");

            double[][] trainData = new double[24][];

            trainData[0] = new double[] { 1, 0, 1.4, 0.3, 1, 0, 0 }; 
            trainData[1] = new double[] { 0, 1, 4.9, 1.5, 0, 1, 0 }; 
            trainData[2] = new double[] { -1, -1, 5.6, 1.8, 0, 0, 1 }; 
            trainData[3] = new double[] { -1, -1, 6.1, 2.5, 0, 0, 1 }; 
            trainData[4] = new double[] { 1, 0, 1.3, 0.2, 1, 0, 0 };
            trainData[5] = new double[] { 0, 1, 1.4, 0.2, 1, 0, 0 };
            trainData[6] = new double[] { 1, 0, 6.6, 2.1, 0, 0, 1 }; 
            trainData[7] = new double[] { 0, 1, 3.3, 1.0, 0, 1, 0 }; 
            trainData[8] = new double[] { -1, -1, 1.7, 0.4, 1, 0, 0 }; 
            trainData[9] = new double[] { 0, 1, 1.5, 0.1, 0, 1, 1 }; 
            trainData[10] = new double[] { 0, 1, 1.4, 0.2, 1, 0, 0 }; 
            trainData[11] = new double[] { 0, 1, 4.5, 1.5, 0, 1, 0 };
            trainData[12] = new double[] { 1, 0, 1.4, 0.2, 1, 0, 0 };
            trainData[13] = new double[] { -1, -1, 5.1, 1.9, 0, 0, 1 };
            trainData[14] = new double[] { 1, 0, 6.0, 2.5, 0, 0, 1 }; 
            trainData[15] = new double[] { 1, 0, 3.9, 1.4, 0, 1, 0 }; 
            trainData[16] = new double[] { 0, 1, 4.7, 1.4, 0, 1, 0 }; 
            trainData[17] = new double[] { -1, -1, 4.6, 1.5, 0, 1, 0 };
            trainData[18] = new double[] { -1, -1, 4.5, 1.7, 0, 0, 1 };
            trainData[19] = new double[] { 0, 1, 4.5, 1.3, 0, 1, 0 }; 
            trainData[20] = new double[] { 1, 0, 1.5, 0.2, 1, 0, 0 }; 
            trainData[21] = new double[] { 0, 1, 5.8, 2.2, 0, 0, 1 }; 
            trainData[22] = new double[] { 0, 1, 4.0, 1.3, 0, 1, 0 };
            trainData[23] = new double[] { -1, -1, 5.8, 1.8, 0, 0, 1 };

            double[][] testData = new double[6][];
            testData[0] = new double[] { 1, 0, 1.5, 0.2, 1, 0, 0 };
            testData[1] = new double[] { -1, -1, 5.9, 2.1, 0, 0, 1 };
            testData[2] = new double[] { 0, 1, 1.4, 0.2, 1, 0, 0 };
            testData[3] = new double[] { 0, 1, 4.7, 1.6, 0, 1, 0 }; 
            testData[4] = new double[] { 1, 0, 4.6, 1.3, 0, 1, 0 }; 
            testData[5] = new double[] { 1, 0, 6.3, 1.8, 0, 0, 1 };

            Console.WriteLine("Encoded training data is:");
            ShowData(trainData, 5, 1, true);

            Console.WriteLine("Encoded test data is:");
            ShowData(testData,2,1,true);

            Console.WriteLine("Creating a 4-input, 6-hidden, 3-output neural network");
            Console.WriteLine("Using tanh and softmax activations");

            const int numInput = 4;
            const int numHidden = 6;
            const int numOutput = 3;
            NeuralNetworkClassifier classifier = new NeuralNetworkClassifier(numInput,numHidden,numOutput);

            int numParticles = 12;
            int maxEpochs = 500;
            Console.WriteLine("Setting numParticles = "+numParticles);
            Console.WriteLine("Setting maxEpochs = "+maxEpochs);

            Console.WriteLine("Beginning training using Particle Swarm Optimization");
            double[] bestWeights = classifier.Train(trainData, numParticles, maxEpochs);
            Console.WriteLine("Final neural network weights and bias values: ");
            ShowVector(bestWeights, 10, 3, true);

            classifier.SetWeights(bestWeights);

            double trainAc = classifier.Accuracy(trainData);
            Console.WriteLine("Accuracy on training data = "+trainAc.ToString("F4"));

            double testAc = classifier.Accuracy(testData);
            Console.WriteLine("Accuracy on test data = "+testAc.ToString("F4"));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="valsPerRow"></param>
        /// <param name="decimals">几位小数</param>
        /// <param name="newLine"></param>
        private static void ShowVector(double[] vector, int valsPerRow, int decimals, bool newLine)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (i%valsPerRow == 0)
                {
                    Console.WriteLine();
                }

                Console.Write(vector[i].ToString("F"+decimals).PadLeft(decimals+4)+" ");
            }

            if (newLine)
            {
                Console.WriteLine();
            }

        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="numRows">显示前几行</param>
        /// <param name="decimals"></param>
        /// <param name="indices"></param>
        private static void ShowData(double[][] data, int numRows, int decimals, bool indices)
        {
            for (int i = 0; i < numRows; i++)
            {
                if (indices)
                {
                    Console.Write("["+i.ToString().PadLeft(2)+"]");
                }

                for (int j = 0; j < data[i].Length; j++)
                {
                    double v = data[i][j];

                    if (v >= 0)
                    {
                        Console.Write("  ");
                    }

                    Console.Write(v.ToString("F"+decimals)+"  ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("........");

            int lastRow = data.Length - 1;

            if (indices == true)
            {
                Console.Write("[" + lastRow.ToString().PadLeft(2) + "] ");
            }

            for (int j = 0; j < data[lastRow].Length; ++j)
            {
                double v = data[lastRow][j];


                if (v >= 0)
                {
                    Console.Write("  ");
                }

                Console.Write(v.ToString("F" + decimals) + "    ");
            }

            Console.WriteLine("\n");
        }

        #endregion

        /// <summary>
        /// 粒子辅助类
        /// </summary>
        private class  Particle
        {
            /// <summary>
            /// 神经网络的权重
            /// </summary>
            public double[] position;

            /// <summary>
            /// 测量预测的舒适程度
            /// </summary>
            public double error;

            /// <summary>
            /// 速度和方向矢量
            /// </summary>
            public double[] velocity;

            /// <summary>
            /// 当前粒子的最优权重
            /// </summary>
            public double[] bestPosition;

            /// <summary>
            /// 最小的错误
            /// </summary>
            public double bestError;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="position"></param>
            /// <param name="error"></param>
            /// <param name="velocity"></param>
            /// <param name="bestPosition"></param>
            /// <param name="bestError"></param>
            public Particle(double[] position, double error, double[] velocity, double[] bestPosition, double bestError)
            {
                this.position = new double[position.Length];
                position.CopyTo(this.position,0);
                this.error = error;
                this.velocity = new double[velocity.Length];
                velocity.CopyTo(this.velocity,0);
                this.bestPosition = new double[bestPosition.Length];
                bestPosition.CopyTo(this.bestPosition,0);
                this.bestError = bestError;
            }



        }

    }
}

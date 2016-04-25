using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// LogisticRegressionWithSimplexOptimization
    /// 
    /// 详见：Machine Learning Using C# Succinctly
    /// </summary>
    public class LogisticClassifier
    {
        #region LogisticClassifier

        /// <summary>
        /// 特征的数量
        /// </summary>
        private int numFeatures;

        /// <summary>
        /// 计算的权重
        /// </summary>
        private double[] weights;

        /// <summary>
        /// 随机数
        /// </summary>
        private Random rnd;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numFeatures"></param>
        public LogisticClassifier(int numFeatures)
        {
            // TODO: Complete member initialization

            if (numFeatures < 1)
            {
                throw new Exception("特征数不能小于1");
            }

            this.numFeatures = numFeatures;
            //权值值b0=weights[0]  bi=weights[i]
            this.weights = new double[numFeatures + 1];
        }

        /// <summary>
        /// 计算将一个测试样例应用的weights上计算出来的值 结果范围在(0,1)
        /// </summary>
        /// <param name="dataItems"></param>
        /// <param name="pWeights"></param>
        /// <returns></returns>
        public double ComputeOutput(double[] dataItems, double[] pWeights)
        {
            double x = 0;
            x += pWeights[0];//常数量b0
            for (int i = 1; i < pWeights.Length; i++)
            {
                x += pWeights[i] * dataItems[i - 1];
            }

            return 1.0 / (1.0 + System.Math.Exp(-x));
        }

        /// <summary>
        /// 计算一个二项值，结果非0即1
        /// </summary>
        /// <param name="dataItems"></param>
        /// <param name="pWeights"></param>
        /// <returns></returns>
        public int ComputeDependent(double[] dataItems, double[] pWeights)
        {
            double result = ComputeOutput(dataItems, pWeights);
            //当result接近0.5时，表示结果不太可信， 接近0或1时，分类越有可信度
            if (result <= 0.5)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }



        /// <summary>
        /// 逻辑回归的主要算法
        /// </summary>
        /// <param name="trainData">训练数据</param>
        /// <param name="maxtry">最大应用算法的重试次数</param>
        /// <param name="seed">随机数种子</param>
        /// <returns></returns>
        private double[] Train(double[][] trainData, int maxtry, int seed)
        {
            this.rnd = new Random(seed);
            Solution[] solutions = new Solution[3];//best  worst  other

            for (int i = 0; i < 3; i++)
            {
                solutions[i] = new Solution(numFeatures);
                solutions[i].weights = RandomSolutionWts();
                solutions[i].error = Error(trainData, solutions[i].weights);
            }

            //排序后 0=best  2=worst
            int best = 0;
            int other = 1;
            int worst = 2;
            int tryCount = 0;

            while (tryCount < maxtry)
            {
                //当error降到指定的范围，可以认为是收敛了
                ++tryCount;

                //排序
                Array.Sort(solutions);
                double[] bestWts = solutions[0].weights;
                double[] otherWts = solutions[1].weights;
                double[] worstWts = solutions[2].weights;

                double[] centroidWts = CentroidWts(otherWts, bestWts);

                //计算expanded  如果expanded好于worst，替代worst
                double[] expandedWts = ExpandedWts(centroidWts, worstWts);
                double expandedError = Error(trainData, expandedWts);
                if (expandedError < solutions[worst].error)
                {
                    //替代worst
                    Array.Copy(expandedWts, worstWts, numFeatures + 1);
                    solutions[worst].error = expandedError;
                    continue;
                }

                //计算reflected, 如果reflected好于worst，替代worst
                double[] reflectedWts = ReflectedWts(centroidWts, worstWts);
                double reflectedError = Error(trainData, reflectedWts);
                if (reflectedError < solutions[worst].error)
                {
                    Array.Copy(reflectedWts, worstWts, numFeatures + 1);
                    solutions[worst].error = reflectedError;
                    continue;
                }

                //如上 计算contracted
                double[] contractedWts = ContractedWts(centroidWts, worstWts);
                double contractedError = Error(trainData, contractedWts);
                if (contractedError < solutions[worst].error)
                {
                    Array.Copy(contractedWts, worstWts, numFeatures + 1);
                    solutions[worst].error = contractedError;
                    continue;
                }

                //生成一个随机的solution
                double[] randomSolWts = RandomSolutionWts();
                double randomSolError = Error(trainData, randomSolWts);
                if (randomSolError < solutions[worst].error)
                {
                    Array.Copy(randomSolWts, worstWts, numFeatures + 1);
                    solutions[worst].error = randomSolError;
                    continue;
                }

                //收缩， 将 worst other 向 best移动
                //移动worst
                for (int i = 0; i < numFeatures + 1; i++)
                {
                    worstWts[i] = (worstWts[i] + bestWts[i]) / 2.0;
                }

                solutions[worst].error = Error(trainData, worstWts);

                //收缩other
                for (int i = 0; i < numFeatures + 1; i++)
                {
                    otherWts[i] = (otherWts[i] + bestWts[i]) / 2.0;
                }

                solutions[other].error = Error(trainData, otherWts);
            }
            Array.Sort(solutions);
            Array.Copy(solutions[best].weights, this.weights, numFeatures + 1);
            return this.weights;
        }

        /// <summary>
        /// 计算contracted
        /// </summary>
        /// <param name="centroidWts"></param>
        /// <param name="worstWts"></param>
        /// <returns></returns>
        private double[] ContractedWts(double[] centroidWts, double[] worstWts)
        {
            double rho = -0.5;

            double[] result = new double[numFeatures + 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = centroidWts[i] + rho * (centroidWts[i] - worstWts[i]);
            }

            return result;

        }

        /// <summary>
        /// 计算reflected
        /// </summary>
        /// <param name="centroidWts"></param>
        /// <param name="worstWts"></param>
        /// <returns></returns>
        private double[] ReflectedWts(double[] centroidWts, double[] worstWts)
        {
            double alpha = 1.0;
            double[] result = new double[numFeatures + 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = centroidWts[i] + alpha * (centroidWts[i] - worstWts[i]);
            }

            return result;
        }

        /// <summary>
        /// 产生随机的解决方案
        /// </summary>
        /// <returns></returns>
        private double[] RandomSolutionWts()
        {
            double[] result = new double[numFeatures + 1];

            //最高 最低值根据实际情况调整
            double lo = -10.0;
            double hi = 10.0;

            for (int i = 0; i < result.Length; i++)
            {
                //期望值是0  范围在[-10.0,10.0)
                result[i] = (hi - lo) * rnd.NextDouble() + lo;
            }

            return result;
        }

        /// <summary>
        /// 计算中心点
        /// </summary>
        /// <param name="otherWts"></param>
        /// <param name="bestWts"></param>
        /// <returns></returns>
        private double[] CentroidWts(double[] otherWts, double[] bestWts)
        {
            double[] result = new double[this.numFeatures + 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (otherWts[i] + bestWts[i]) / 2.0;
            }

            return result;
        }

        /// <summary>
        /// 计算Expanded
        /// </summary>
        /// <param name="centroidWts"></param>
        /// <param name="worstWts"></param>
        /// <returns></returns>
        private double[] ExpandedWts(double[] centroidWts, double[] worstWts)
        {
            double gamma = 2.0;
            double[] result = new double[numFeatures + 1];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = centroidWts[i] + gamma * (centroidWts[i] - worstWts[i]);
            }

            return result;
        }

        /// <summary>
        /// 计算训练数据的正确率
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="bestWeights"></param>
        /// <returns></returns>
        private double Accuracy(double[][] datas, double[] bestWeights)
        {
            int numCorrect = 0;//正确的个数
            int numWrong = 0;//错误的个数

            int yIndex = datas[0].Length - 1;//训练数据的正确性结果的索引

            for (int i = 0; i < datas.Length; i++)
            {
                double computed = ComputeDependent(datas[i], bestWeights);

                double desired = datas[i][yIndex];//0 or 1

                //computed == desired
                if (System.Math.Abs(computed - desired) < 0.000001)
                {
                    ++numCorrect;
                }
                else
                {
                    ++numWrong;
                }
            }

            return (numCorrect * 1.0) / (numCorrect + numWrong);
        }

        /// <summary>
        /// 均方差  表示错误率  
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="bestWeights"></param>
        /// <returns></returns>
        private double Error(double[][] datas, double[] bestWeights)
        {
            int yIndex = datas[0].Length - 1;
            double sumSquaredError = 0;

            for (int i = 0; i < datas.Length; i++)
            {
                double computed = ComputeOutput(datas[i], bestWeights);
                double desired = datas[i][yIndex];//0 or 1
                sumSquaredError += (computed - desired) * (computed - desired);
            }

            return sumSquaredError / datas.Length;
        }

        #endregion

        #region Solution

        /// <summary>
        /// 辅助类  表示一个最优解
        /// </summary>
        private class Solution : IComparable<Solution>
        {
            /// <summary>
            /// 求出来的weights
            /// </summary>
            public double[] weights;

            /// <summary>
            /// 表示当前weights下的 mean squared error
            /// </summary>
            public double error;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="numFeatures"></param>
            public Solution(int numFeatures)
            {
                this.weights = new double[numFeatures + 1];
                this.error = 0.0;
            }


            /// <summary>
            /// ICompareable接口
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public int CompareTo(Solution other)
            {
                if (this.error < other.error)
                {
                    return -1;
                }
                else if (this.error > other.error)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }


        #endregion

        #region 测试程序

        public static void TestMain()
        {
            Console.WriteLine("Begin Logistic Regression Binary Classification demo");
            double[][] data = new double[30][];
            data[0] = new double[] { 48, +1, 4.40, 0 };
            data[1] = new double[] { 60, -1, 7.89, 1 };
            data[2] = new double[] { 51, -1, 3.48, 0 };
            data[3] = new double[] { 66, -1, 8.41, 1 };
            data[4] = new double[] { 40, +1, 3.05, 0 };
            data[5] = new double[] { 44, +1, 4.56, 0 };
            data[6] = new double[] { 80, -1, 6.91, 1 };
            data[7] = new double[] { 52, -1, 5.69, 0 };
            data[8] = new double[] { 56, -1, 4.01, 0 };
            data[9] = new double[] { 55, -1, 4.48, 0 };
            data[10] = new double[] { 72, +1, 5.97, 0 };
            data[11] = new double[] { 57, -1, 6.71, 1 };
            data[12] = new double[] { 50, -1, 6.40, 0 };
            data[13] = new double[] { 80, -1, 6.67, 1 };
            data[14] = new double[] { 69, +1, 5.79, 0 };
            data[15] = new double[] { 39, -1, 5.42, 0 };
            data[16] = new double[] { 68, -1, 7.61, 1 };
            data[17] = new double[] { 47, +1, 3.24, 0 };
            data[18] = new double[] { 45, +1, 4.29, 0 };
            data[19] = new double[] { 79, +1, 7.44, 1 };
            data[20] = new double[] { 44, -1, 2.55, 0 };
            data[21] = new double[] { 52, +1, 3.71, 0 };
            data[22] = new double[] { 80, +1, 7.56, 1 };
            data[23] = new double[] { 76, -1, 7.80, 1 };
            data[24] = new double[] { 51, -1, 5.94, 0 };
            data[25] = new double[] { 46, +1, 5.52, 0 };
            data[26] = new double[] { 48, -1, 3.25, 0 };
            data[27] = new double[] { 58, +1, 4.71, 0 };
            data[28] = new double[] { 44, +1, 2.52, 0 };
            data[29] = new double[] { 68, -1, 8.38, 1 };

            Console.WriteLine("Raw data:");
            Console.WriteLine(" Age Sex Kidney Died");
            Console.WriteLine("=======================================");
            ShowData(data, 5, 2, true);

            Console.WriteLine("Normalizing age and kidney data");
            int[] columns = new int[] { 0, 2 };
            //返回值保存每个属性项的平均值和标准差,第一行是平均值，第二行是标准差
            double[][] means = Normalize(data, columns);

            Console.WriteLine("Normalized  data: ");
            ShowData(data, 5, 2, true);

            Console.WriteLine("Creating train (80%) and test (20%) matrices");

            double[][] trainData;
            double[][] testData;
            double trainPercent = 0.8;//训练数据比例

            MakeTrainTest(data, 0, out trainData, out testData, trainPercent);

            Console.WriteLine("Normalized training data: ");
            ShowData(trainData, 3, 2, true);

            int numFeatures = 3;
            Console.WriteLine("Creating LR binary classifier");

            LogisticClassifier lc = new LogisticClassifier(numFeatures);
            int maxEpochs = 100;//最大循环次数，应用回归算法的最大次数
            //33是随机数的种子
            double[] bestWeights = lc.Train(trainData, maxEpochs, 33);

            Console.WriteLine("Best weights found:");
            ShowVector(bestWeights, 4, true);

            double trainAccuracy = lc.Accuracy(trainData, bestWeights);
            Console.WriteLine("Prediction accuracy on training data = " + trainAccuracy.ToString("F4"));
            double testAccuracy = lc.Accuracy(testData, bestWeights);
            Console.WriteLine("Prediction accuracy on test data = " + testAccuracy.ToString("F4"));

            Console.WriteLine("End LR binary classification demo");

        }

        /// <summary>
        /// 将数据分为trainPercent比例训练数据，1-trainPercent  测试数据
        /// </summary>
        /// <param name="data">所有的数据</param>
        /// <param name="seed">随机数数据</param>
        /// <param name="trainData">输出训练数据</param>
        /// <param name="testData">输出测试数据</param>
        /// <param name="trainPercent">训练数据的比例</param>
        private static void MakeTrainTest(double[][] data, int seed, out double[][] trainData, out double[][] testData, double trainPercent)
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

        /// <summary>
        /// 打印一维向量
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="decimals"></param>
        /// <param name="newLine"></param>
        private static void ShowVector(double[] vector, int decimals, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
            {
                Console.Write(vector[i].ToString("F" + decimals) + " ");
            } 
            Console.WriteLine("");
            if (newLine == true)
            {
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// 创建分组， 80%用于训练，20%用于测试
        /// </summary>
        /// <param name="data"></param>
        /// <param name="seed"></param>
        /// <param name="trainData"></param>
        /// <param name="testData"></param>
        private static void MakeTrainTest(double[][] data, int seed, out double[][] trainData, out double[][] testData)
        {
            MakeTrainTest(data, seed, out trainData, out testData, 0.8);
        }

        /// <summary>
        /// 归一化数据
        /// 归一化后数值应该在[-5,5]之间，超出这个范围都偏离均值太多，应该检查
        /// 理论上，不需要归一化，但归一化通常提供更好的正确率
        /// </summary>
        /// <param name="data"></param>
        /// <param name="columns"></param>
        /// <returns>返回值保存每个属性项的平均值和标准差,第一行是平均值，第二行是标准差</returns>
        private static double[][] Normalize(double[][] data, int[] columns)
        {
            int numRows = data.Length;
            int numCols = data[0].Length;
            //result保存每个属性项的平均值和标准差
            //第一行是平均值，第二行是标准差
            double[][] result = new double[2][];
            for (int i = 0; i < 2; i++)
            {
                result[i] = new double[numCols];
            }

            //可以只计算columns指定的列
            for (int c = 0; c < numCols; c++)
            {
                double sum = 0.0;
                for (int r = 0; r < numRows; r++)
                {
                    sum += data[r][c];
                }
                double mean = sum / numRows;
                result[0][c] = mean;

                //计算标准差
                double sumSquares = 0;
                for (int r = 0; r < numRows; r++)
                {
                    sumSquares += (data[r][c] - mean) * (data[r][c] - mean);
                }

                double stdDev = System.Math.Sqrt(sumSquares / numRows);
                result[1][c] = stdDev;
            }

            //高斯归一化
            for (int c = 0; c < columns.Length; c++)
            {
                int j = columns[c];//需要归一化的列
                double mean = result[0][j];
                double stdDev = result[1][j];

                for (int i = 0; i < numRows; i++)
                {
                    data[i][j] = (data[i][j] - mean) / stdDev;
                }
            }

            return result;
        }

        static void Normalize(double[][] rawData, int[] columns, double[][] means)
        { // normalize columns using supplied means and standard devs 
            int numRows = rawData.Length; 
            for (int c = 0; c < columns.Length; ++c) // each specified col
            {
                int j = columns[c]; // column to normalize 
                double mean = means[0][j]; 
                double stdDev = means[1][j];
                for (int i = 0; i < numRows; ++i) // each row 
                {
                    rawData[i][j] = (rawData[i][j] - mean) / stdDev;
                }
            }
        }

        /// <summary>
        /// 打印原生数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="numRows"></param>
        /// <param name="decimals"></param>
        /// <param name="indices">是否显示索引</param>
        private static void ShowData(double[][] data, int numRows, int decimals, bool indices)
        {
            for (int i = 0; i < numRows; i++)
            {
                if (indices == true)
                {
                    Console.Write("["+i.ToString().PadLeft(2)+"] ");
                }

                for (int j = 0; j < data[i].Length; j++)
                {
                    double v = data[i][j];
                    if (v>=0)
                    {
                        Console.Write(" ");
                    }

                    Console.Write(v.ToString("F"+decimals)+"    ");
                }

                Console.WriteLine();
            }


            Console.WriteLine("......");

            int lastRow = data.Length - 1;
            if (indices == true)
            {
                Console.Write("[" + lastRow.ToString().PadLeft(2) + "] ");
            }

            for (int i = 0; i < data[lastRow].Length; i++)
            {
                double v = data[lastRow][i];
                if (v>=0.0)
                {
                    Console.Write("    ");
                }
                Console.Write(v.ToString("F" + decimals) + " ");
            }

            Console.WriteLine();
        }

        #endregion

    }
}


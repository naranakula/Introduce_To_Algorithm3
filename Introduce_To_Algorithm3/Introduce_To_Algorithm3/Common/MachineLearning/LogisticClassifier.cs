using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
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
        private int numFeatures;

        public LogisticClassifier(int numFeatures)
        {
            // TODO: Complete member initialization
            this.numFeatures = numFeatures;
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
            throw new NotImplementedException();
        }


        private double Accuracy(double[][] trainData, double[] bestWeights)
        {
            throw new NotImplementedException();
        }



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
            int[] columns = new int[]{0,2};
            double[][] means = Normalize(data, columns);

            Console.WriteLine("Normalized  data: ");
            ShowData(data,5,2,true);

            Console.WriteLine("Creating train (80%) and test (20%) matrices");

            double[][] trainData;
            double[][] testData;
            double trainPercent = 0.8;//训练数据比例

            MakeTrainTest(data, 0, out trainData,out testData,trainPercent);

            Console.WriteLine("Normalized training data: ");
            ShowData(trainData,3,2,true);

            int numFeatures = 3;
            Console.WriteLine("Creating LR binary classifier");

            LogisticClassifier lc = new LogisticClassifier(numFeatures);
            int maxEpochs = 100;//最大循环次数，应用回归算法的最大次数
            //33是随机数的种子
            double[] bestWeights = lc.Train(trainData, maxEpochs, 33);

            Console.WriteLine("Best weights found:");
            ShowVector(bestWeights, 4, true);

            double trainAccuracy = lc.Accuracy(trainData, bestWeights);
            Console.WriteLine("Prediction accuracy on training data = "+trainAccuracy.ToString("F4"));
            double testAccuracy = lc.Accuracy(testData, bestWeights);
            Console.WriteLine("Prediction accuracy on test data = "+testAccuracy.ToString("F4"));

            Console.WriteLine("End LR binary classification demo");

        }

        /// <summary>
        /// 将数据分为trainPercent比例训练数据，1-trainPercent  测试数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="p1"></param>
        /// <param name="trainData"></param>
        /// <param name="testData"></param>
        /// <param name="trainPercent"></param>
        private static void MakeTrainTest(double[][] data, int p1, out double[][] trainData, out double[][] testData, double trainPercent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 打印一维向量
        /// </summary>
        /// <param name="bestWeights"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private static void ShowVector(double[] bestWeights, int p1, bool p2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建分组， 80%用于训练，20%用于测试
        /// </summary>
        /// <param name="data"></param>
        /// <param name="p"></param>
        /// <param name="trainData"></param>
        /// <param name="testData"></param>
        private static void MakeTrainTest(double[][] data, int p, out double[][] trainData, out double[][] testData)
        {
            throw new NotImplementedException();
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
                double mean = sum/numRows;
                result[0][c] = mean;

                //计算标准差
                double sumSquares = 0;
                for (int r = 0; r < numRows; r++)
                {
                    sumSquares += (data[r][c] - mean)*(data[r][c] - mean);
                }

                double stdDev = System.Math.Sqrt(sumSquares/numRows);
                result[1][c] = stdDev;
            }

            //高斯归一化
            for (int c = 0; c < columns.Length; c++)
            {
                int j = columns[c];//需要归一化的列
                double mean = result[0][c];
                double stdDev = result[1][c];

                for (int i = 0; i < numRows; i++)
                {
                    data[i][j] = (data[i][j] - mean)/stdDev;
                }
            }

            return result;
        }

        /// <summary>
        /// 打印原生数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        private static void ShowData(double[][] data, int p1, int p2, bool p3)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}

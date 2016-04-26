using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// Machine learning using C# Succinctly的算法
    /// 
    /// 朴素贝叶斯应用于枚举类型，如果要应用于数值类型，则将其先转换为枚举类型
    /// 通常等宽界限或等数量界限的策略
    /// </summary>
    public class NaiveBayer
    {

        #region private member

        private Dictionary<string, int>[] stringToInts;
        private int[][][] jointCounts;
        private int[] dependentCounts;

        private string[][] trainDatas;
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public NaiveBayer()
        {
            this.stringToInts = null;
            this.jointCounts = null;
            this.dependentCounts = null;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 训练过程
        /// </summary>
        /// <param name="trainData"></param>
        private void Train(string[][] trainData)
        {
            this.trainDatas = trainData;
            int numRows = trainData.Length;
            int numCols = trainData[0].Length;

            this.stringToInts = new Dictionary<string, int>[numCols];

            for (int col = 0; col < numCols; col++)
            {
                stringToInts[col] = new Dictionary<string, int>();
                int idx = 0;
                for (int row = 0; row < numRows; row++)
                {
                    string s = trainData[row][col].ToLower().Trim();
                    if (!stringToInts[col].ContainsKey(s))
                    {
                        stringToInts[col].Add(s, idx);
                        ++idx;
                    }
                }
            }

            //numCols - 1 是特征数
            this.jointCounts = new int[numCols - 1][][];
            //jointCounts[i][i][k] = m  i表示第几个特征种类  j表示该种类下的特征值 k 表示最终分类  m表示个数
            for (int c = 0; c < numCols - 1; c++)
            {
                int count = stringToInts[c].Count;
                jointCounts[c] = new int[count][];
            }

            for (int i = 0; i < jointCounts.Length; i++)
            {
                for (int j = 0; j < jointCounts[i].Length; j++)
                {
                    //最终种类数  本例中实际是binary classification
                    jointCounts[i][j] = new int[stringToInts[numCols - 1].Count];
                }
            }

            //做平滑处理，每个cell初始为1，（实际上应该为0，设置为1是为了平滑处理）
            int countSum = 0;//countSum现在没使用
            for (int i = 0; i < jointCounts.Length; i++)
            {
                for (int j = 0; j < jointCounts[i].Length; j++)
                {
                    for (int k = 0; k < jointCounts[i][j].Length; k++)
                    {
                        jointCounts[i][j][k] = 1;
                        countSum++;
                    }
                }
            }


            for (int i = 0; i < numRows; i++)
            {
                //dependent value 
                string yString = trainData[i][numCols - 1].ToLower().Trim();
                int depIndex = stringToInts[numCols - 1][yString];//corresponding index
                for (int j = 0; j < numCols - 1; j++)
                {
                    int attIndex = j;
                    string xString = trainData[i][j].Trim().ToLower();
                    int valIndex = stringToInts[j][xString];
                    ++jointCounts[attIndex][valIndex][depIndex];
                }
            }

            this.dependentCounts = new int[stringToInts[numCols - 1].Count];

            for (int i = 0; i < dependentCounts.Length; i++)
            {
                dependentCounts[i] = numCols - 1;//特征数  平滑处理
            }

            for (int i = 0; i < numRows; i++)
            {
                string yString = trainData[i][numCols - 1].ToLower().Trim();
                int yIndex = stringToInts[numCols - 1][yString];
                ++dependentCounts[yIndex];
            }

        }

        /// <summary>
        /// 方法的准确率
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private double Accuracy(string[][] data)
        {
            int numCorrect = 0;
            int numWrong = 0;

            int numRows = data.Length;
            int numCols = data[0].Length;

            for (int i = 0; i < numRows; i++)
            {
                string yValue = data[i][numCols - 1].Trim().ToLower();
                string[] xValues = new string[numCols - 1];
                Array.Copy(data[i], xValues, numCols - 1);
                var results = Probability(xValues);
                //results[0]概率越大越可信
                if (yValue.Equals(results[0].Target))
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
        /// 计算概率
        /// </summary>
        /// <param name="yValue"></param>
        /// <param name="xValues"></param>
        /// <returns></returns>
        private double Probability(string yValue, string[] xValues)
        {
            int numFeatures = xValues.Length;

            int numOfYValues = stringToInts[numFeatures].Count;
            //binary  实际上 stringToInts[numFeatures].Count = 2
            double[][] conditionals = new double[numOfYValues][];
            for (int i = 0; i < numOfYValues; i++)
            {
                conditionals[i] = new double[numFeatures];
            }

            double[] unconditionals = new double[numOfYValues];
            int numRows = trainDatas.Length;
            //计算无条件概率
            foreach (var item in stringToInts[numFeatures])
            {
                unconditionals[item.Value] = (dependentCounts[item.Value] * 1.0) / (numRows * 1.0);
            }

            int[] x = new int[numFeatures];
            for (int i = 0; i < numFeatures; i++)
            {
                string s = xValues[i].ToLower().Trim();
                x[i] = this.stringToInts[i][s];
            }

            for (int k = 0; k < numOfYValues; k++)
            {
                for (int i = 0; i < numFeatures; i++)
                {
                    int attIndex = i;
                    int valIndex = x[i];
                    int depIndex = k;
                    conditionals[k][i] = (jointCounts[attIndex][valIndex][depIndex] * 1.0) / (dependentCounts[depIndex] * 1.0);
                }
            }

            List<Item> results = new List<Item>();
            double sum = 0;
            foreach (var dicItem in stringToInts[numFeatures])
            {
                Item item = new Item();
                item.Target = dicItem.Key;
                item.Reliability = 1.0 * unconditionals[dicItem.Value];
                for (int i = 0; i < numFeatures; i++)
                {
                    item.Reliability *= conditionals[dicItem.Value][i];
                }
                sum += item.Reliability;
                results.Add(item);
            }

            return results.First(r => r.Target.Equals(yValue.Trim().ToLower())).Reliability / sum;
        }


        /// <summary>
        /// 计算概率
        /// </summary>
        /// <param name="xValues"></param>
        /// <returns>返回值按将序排列</returns>
        private List<Item> Probability(string[] xValues)
        {
            int numFeatures = xValues.Length;

            int numOfYValues = stringToInts[numFeatures].Count;
            //binary  实际上 stringToInts[numFeatures].Count = 2
            double[][] conditionals = new double[numOfYValues][];
            for (int i = 0; i < numOfYValues; i++)
            {
                conditionals[i] = new double[numFeatures];
            }

            double[] unconditionals = new double[numOfYValues];
            int numRows = trainDatas.Length;
            //计算无条件概率
            foreach (var item in stringToInts[numFeatures])
            {
                unconditionals[item.Value] = (dependentCounts[item.Value] * 1.0) / (numRows * 1.0);
            }

            int[] x = new int[numFeatures];
            for (int i = 0; i < numFeatures; i++)
            {
                string s = xValues[i].ToLower().Trim();
                x[i] = this.stringToInts[i][s];
            }

            for (int k = 0; k < numOfYValues; k++)
            {
                for (int i = 0; i < numFeatures; i++)
                {
                    int attIndex = i;
                    int valIndex = x[i];
                    int depIndex = k;
                    conditionals[k][i] = (jointCounts[attIndex][valIndex][depIndex] * 1.0) / (dependentCounts[depIndex] * 1.0);
                }
            }

            List<Item> results = new List<Item>();
            double sum = 0;
            foreach (var dicItem in stringToInts[numFeatures])
            {
                Item item = new Item();
                item.Target = dicItem.Key;
                item.Reliability = 1.0 * unconditionals[dicItem.Value];
                for (int i = 0; i < numFeatures; i++)
                {
                    item.Reliability *= conditionals[dicItem.Value][i];
                }
                sum += item.Reliability;
                results.Add(item);
            }

            for (int i = 0; i < results.Count; i++)
            {
                results[i].Reliability = results[i].Reliability / sum;
            }

            return results.OrderByDescending(r => r.Reliability).ToList();
        }

        #endregion

        #region 测试

        public static void TestMain()
        {
            Console.WriteLine("Begin Naive Bayes classification demo");
            Console.WriteLine("Goal is to predict (liberal/conservative) from job, sex and income");
            string[][] rawData = new string[30][];
            rawData[0] = new string[] { "analyst", "male", "high", "conservative" };
            rawData[1] = new string[] { "barista", "female", "low", "liberal" };
            rawData[2] = new string[] { "cook", "male", "medium", "conservative" };
            rawData[3] = new string[] { "doctor", "female", "medium", "conservative" };
            rawData[4] = new string[] { "analyst", "female", "low", "liberal" };
            rawData[5] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[6] = new string[] { "analyst", "male", "medium", "conservative" };
            rawData[7] = new string[] { "cook", "female", "low", "liberal" };
            rawData[8] = new string[] { "doctor", "female", "medium", "liberal" };
            rawData[9] = new string[] { "cook", "female", "low", "liberal" };
            rawData[10] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[11] = new string[] { "cook", "female", "high", "liberal" };
            rawData[12] = new string[] { "barista", "female", "medium", "liberal" };
            rawData[13] = new string[] { "analyst", "male", "low", "liberal" };
            rawData[14] = new string[] { "doctor", "female", "high", "conservative" };
            rawData[15] = new string[] { "barista", "female", "medium", "conservative" };
            rawData[16] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[17] = new string[] { "barista", "male", "high", "conservative" };
            rawData[18] = new string[] { "doctor", "female", "medium", "liberal" };
            rawData[19] = new string[] { "analyst", "male", "low", "liberal" };
            rawData[20] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[21] = new string[] { "cook", "male", "medium", "conservative" };
            rawData[22] = new string[] { "doctor", "female", "high", "conservative" };
            rawData[23] = new string[] { "analyst", "male", "high", "conservative" };
            rawData[24] = new string[] { "barista", "female", "medium", "liberal" };
            rawData[25] = new string[] { "doctor", "male", "medium", "conservative" };
            rawData[26] = new string[] { "analyst", "female", "medium", "conservative" };
            rawData[27] = new string[] { "analyst", "male", "medium", "conservative" };
            rawData[28] = new string[] { "doctor", "female", "medium", "liberal" };
            rawData[29] = new string[] { "barista", "male", "medium", "conservative" };

            Console.WriteLine("The raw data is: ");
            ShowData(rawData, 5, true);

            Console.WriteLine("Splitting data into 80%-20% train test sets");
            String[][] trainData;
            String[][] testData;
            MakeTrainTest(rawData, 15, out trainData, out testData);//seed = 15
            Console.WriteLine("Done");

            Console.WriteLine("Training data: ");
            ShowData(trainData, 5, true);
            Console.WriteLine("Test data: ");
            ShowData(testData, 5, true);

            Console.WriteLine("Creating Naive Bayes Classifier object");
            NaiveBayer naiveBayer = new NaiveBayer();
            //训练数据
            naiveBayer.Train(trainData);
            Console.WriteLine("Done");

            double trainAccuracy = naiveBayer.Accuracy(trainData);
            Console.WriteLine("Accuracy of model on the train data = " + trainAccuracy.ToString("F4"));

            double testAccuracy = naiveBayer.Accuracy(testData);
            Console.WriteLine("Accuracy of model on test data = " + testAccuracy.ToString("F4"));

            Console.WriteLine("Predicting politics for job = barista, sex = female,income = medium");
            string[] features = new string[] { "barista", "female", "medium" };

            string liberal = "liberal";
            double pLiberal = naiveBayer.Probability(liberal, features);
            Console.WriteLine("Probability of liberal = " + pLiberal.ToString("F4"));

            //预测conservative
            string conservative = "conservative";
            double pConservative = naiveBayer.Probability(conservative, features);
            Console.WriteLine("Probability of conservative = " + pConservative.ToString("F4"));

            Console.WriteLine("End Naive Bayes classification demo");


        }

        /// <summary>
        /// 分割训练数据和测试数据
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="seed"></param>
        /// <param name="trainData"></param>
        /// <param name="testData"></param>
        /// <param name="percent"></param>
        private static void MakeTrainTest(string[][] rawData, int seed, out string[][] trainData, out string[][] testData, double percent = 0.8)
        {
            int totRows = rawData.Length;
            int numTrainRows = (int)(totRows * percent);
            int numTestRows = totRows - numTrainRows;

            Random rnd = new Random(seed);

            trainData = new string[numTrainRows][];
            testData = new string[numTestRows][];

            string[][] copy = new string[totRows][]; // ref copy of all data 
            for (int i = 0; i < copy.Length; ++i)
            {
                copy[i] = rawData[i];
            }

            for (int i = 0; i < copy.Length; ++i) // scramble order 
            {
                int r = rnd.Next(i, copy.Length);
                string[] tmp = copy[r];
                copy[r] = copy[i];
                copy[i] = tmp;
            }

            for (int i = 0; i < numTrainRows; ++i)
            {
                trainData[i] = copy[i];
            }

            for (int i = 0; i < numTestRows; ++i)
            {
                testData[i] = copy[i + numTrainRows];
            }
        }

        /// <summary>
        /// 打印源数据
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="numRows"></param>
        /// <param name="indices"></param>
        private static void ShowData(string[][] rawData, int numRows, bool indices)
        {
            for (int i = 0; i < numRows; ++i)
            {
                if (indices == true)
                {
                    Console.Write("[" + i.ToString().PadLeft(2) + "] ");
                }
                for (int j = 0; j < rawData[i].Length; ++j)
                {
                    string s = rawData[i][j]; 
                    Console.Write(s.PadLeft(14) + " ");
                } 
                Console.WriteLine("");
            }

            if (numRows != rawData.Length - 1)
            {
                Console.WriteLine(". . .");
            } 
            int lastRow = rawData.Length - 1;
            if (indices == true)
            {
                Console.Write("[" + lastRow.ToString().PadLeft(2) + "] ");
            }
            for (int j = 0; j < rawData[lastRow].Length; ++j)
            {
                string s = rawData[lastRow][j];
                Console.Write(s.PadLeft(14) + " ");
            } 
            Console.WriteLine("\n");
        }

        #endregion

        private class Item
        {
            /// <summary>
            /// 目标分类
            /// </summary>
            public string Target { get; set; }

            /// <summary>
            /// 可信度
            /// </summary>
            public double Reliability { get; set; }
        }

    }
}

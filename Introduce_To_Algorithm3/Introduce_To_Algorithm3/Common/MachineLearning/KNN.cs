using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Math;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// the k nearest neighbors
    /// pros: High accuracy, insensitive to outliers
    /// cons: time and memory costs
    /// </summary>
    public static class KNearestNeighbor
    {
        /// <summary>
        /// the theory of knn:
        /// 1)calculte the distance between testVector and ech vector in dataSet
        /// 2)sort the distances in increasing order
        /// 3)take the k lowest distance
        /// 4)find the majority classify among those k items
        /// 5)return the majority class as our prediction for the classify of testVector
        /// </summary>
        /// <param name="testPoint"></param>
        /// <param name="dataSet"></param>
        /// <param name="labels">the class of each item in dataset, it should have same count as dataset</param>
        /// <param name="k">the  k of k nearest neighbor</param>
        /// <returns></returns>
        public static string KNN(double[] testVector,List<double[]> dataSet, string[] labels,int k = 10 )
        {
            DoubleVector testDoubleVector = new DoubleVector(testVector);
            List<DoubleVector> dataSetList = new List<DoubleVector>();
            for (int i = 0; i < dataSet.Count; i++)
            {
                dataSetList.Add(new DoubleVector(dataSet[i]));
            }

            return KNN(testDoubleVector, dataSetList, labels, k);
        }

        /// <summary>
        /// norm input using (oldValue - minValue)/(maxValue-minValue)
        /// </summary>
        /// <param name="testVector"></param>
        /// <param name="dataSet"></param>
        /// <param name="normedTestVector"></param>
        /// <param name="normedDataSet"></param>
        public static void Norm(double[] testVector, List<double[]> dataSet,out double[] normedTestVector,out List<double[]> normedDataSet)
        {
            int len = testVector.Length;
            normedTestVector = new double[len];
            normedDataSet = new List<double[]>();

            double[] minVals = new double[len];
            double[] maxVals = new double[len];
            double[] ranges = new double[len];
            for (int i = 0; i < len; i++)
            {
                minVals[i] = (from r in dataSet select r[i]).Min();
                maxVals[i] = (from r in dataSet select r[i]).Max();
                ranges[i] = maxVals[i] - minVals[i];
            }

            for (int i = 0; i < len; i++)
            {
                normedTestVector[i] = (testVector[i]-minVals[i])/ranges[i];
            }

            foreach (double[] data in dataSet)
            {
                double[] newData = new double[len];
                for (int i = 0; i < len; i++)
                {
                    newData[i] = (data[i] - minVals[i])/ranges[i];
                }

                normedDataSet.Add(newData);
            }

        }

        /// <summary>
        /// the theory of knn:
        /// 1)calculte the distance between testVector and ech vector in dataSet
        /// 2)sort the distances in increasing order
        /// 3)take the k lowest distance
        /// 4)find the majority classify among those k items
        /// 5)return the majority class as our prediction for the classify of testVector
        /// </summary>
        /// <param name="testVector"></param>
        /// <param name="dataSet"></param>
        /// <param name="labels">the class of each item in dataset, it should have same count as dataset</param>
        /// <param name="k">the  k of k nearest neighbor</param>
        /// <returns></returns>
        public static string KNN(DoubleVector testVector, List<DoubleVector> dataSet, string[] labels, int k)
        {
            double[] distances = new double[dataSet.Count];

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = testVector.DistanceTo(dataSet[i]);
            }

            var tupleList = distances.Zip(labels, (d, s) => new Tuple<double, string>(d, s));

            return tupleList.OrderBy(tuple => tuple.Item1).Take(k).GroupBy(g=>g.Item2).OrderByDescending(g=>g.Count()).First().Key;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Introduce_To_Algorithm3.Common.MachineLearning.Tests
{
    [TestClass()]
    public class KNearestNeighborTests
    {
        [TestMethod()]
        public void KNNTest()
        {
            double[] testVector = {0, 0};
            string[] labels = {"A", "A", "B", "B"};
            List<double[]> dataset = new List<double[]> { new double[] { 1.0, 1.1 }, new double[] { 1.0, 1.0 }, new double[] { 0, 0 }, new double[] { 0, 0.1 } };
            string s = KNearestNeighbor.KNN(testVector,dataset,labels,3);
            Console.WriteLine(s);
        }
    }
}

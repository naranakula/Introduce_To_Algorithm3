using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Introduce_To_Algorithm3.Common.MachineLearning.Tests
{
    [TestClass()]
    public class LogisticRegressionTests
    {
        [TestMethod()]
        public void GradAscentTest()
        {
            string file = @"C:\db\testset.txt";
            var lines = File.ReadAllLines(file);
            double[,] dataMat = new double[lines.Length,3];
            double[,] labelMat = new double[lines.Length,1];
            for (int i = 0; i < lines.Length; i++)
            {
                var temp = lines[i].Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                dataMat[i, 0] = 1.0;
                dataMat[i, 1] = double.Parse(temp[0]);
                dataMat[i, 2] = double.Parse(temp[1]);
                labelMat[i, 0] = double.Parse(temp[2]);
            }
            var result = LogisticRegression.GradAscent(dataMat, labelMat);
            Console.WriteLine(result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    public  static class DecisionTree
    {
        /// <summary>
        /// calc shannon entropy.
        /// </summary>
        /// <param name="labels"></param>
        /// <returns></returns>
        public static double ShannonEntropy(IEnumerable<string> labels)
        {
            List<int> valuesInts = (from r in labels group r by r into g select g.Count()).ToList();
            int total = labels.Count();
            double entropy = 0;
            foreach (var val in valuesInts)
            {
                double prob = val*1.0/total;
                entropy -= prob*System.Math.Log(prob, 2);
            }

            return entropy;
        }

        /// <summary>
        /// split data 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="index">the index of feature what to split</param>
        /// <param name="featureValue">the value of feature to split</param>
        public static List<DataSetItem> SplitDataSet(IEnumerable<DataSetItem> dataSet, int index, string featureValue)
        {
            List<DataSetItem> items = new List<DataSetItem>();
            foreach (DataSetItem item in dataSet)
            {
                if (item.Features[index] == featureValue)
                {
                    DataSetItem setItem = new DataSetItem();
                    var temp = item.Features.ToList();
                    temp.RemoveAt(index);
                    setItem.Features = temp.ToArray();
                    setItem.ClassName = item.ClassName;
                    items.Add(setItem);
                }
            }
            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="labels">label name for each feature column</param>
        /// <param name="node">the parent node</param>
        private static void CreateTree(List<DataSetItem> dataSet, List<string> labels,DecisionTreeNode node)
        {
            var classList = (from r in dataSet select r.ClassName).ToList();
            if (classList.Distinct().Count() <= 1)
            {
                //stop when all class are equal
                DecisionTreeNode child = new DecisionTreeNode(){ClassName = classList[0]};
                node.ChildrenList.Add(child);
                return;
            }

            if (dataSet[0].Features == null || dataSet[0].Features.Length <= 0)
            {
                //no more feature to split,choose the most frequent class as classname
                DecisionTreeNode child = new DecisionTreeNode(){ClassName = MajorityClass(classList),IsMajority = true};
                node.ChildrenList.Add(child);
                return;
            }

            int bestIndex = ChooseBestFeatureToSplit(dataSet);
            string bestFeatLabel = labels[bestIndex];
            labels.RemoveAt(bestIndex);
            var uniqueVals = (from r in dataSet select r.Features[bestIndex]).Distinct().ToList();
            foreach (var featVal in uniqueVals)
            {
                DecisionTreeNode child = new DecisionTreeNode(){label = bestFeatLabel,FeatValue = featVal};
                node.ChildrenList.Add(child);
                CreateTree(SplitDataSet(dataSet,bestIndex,featVal),labels.ToList(),child);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="labels"></param>
        /// <returns>the root of decison tree. root has no meaning. it just lead to first split</returns>
        public static DecisionTreeNode CreateTree(List<DataSetItem> dataSet, List<string> labels)
        {
            DecisionTreeNode root = new DecisionTreeNode();
            CreateTree(dataSet, labels, root);
            return root;
        }

        /// <summary>
        /// choose the best feature to split
        /// the information gain after split should be maximum
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns>-1,if the number of data in dataset less than 1(or all have same class name) or no more feature to split. or >=0 index of the feature to split</returns>
        public static int ChooseBestFeatureToSplit(IEnumerable<DataSetItem> dataSet)
        {

            var labels = from r in dataSet select r.ClassName;
            if (labels.Count() <= 1)
            {
                //all have same class name
                return -1;
            }

            if (dataSet.First().Features.Length <= 0)
            {
                //no more feature to split, the most frequency class wins.
                return -1;
            }

            double baseEntropy = ShannonEntropy(labels);
            double beatInfoGain = double.MinValue;
            int bestFeature = -1;
            for (int i = 0; i < dataSet.First().Features.Length; i++)
            {
                var uniqueVals = (from r in dataSet select r.Features[i]).Distinct();
                double newEntropy = 0;
                foreach (string featureVal in uniqueVals)
                {
                    var subDataSet = SplitDataSet(dataSet, i, featureVal);
                    double prob = subDataSet.Count*1.0/dataSet.Count();
                    newEntropy += prob*ShannonEntropy(from r in subDataSet select r.ClassName);
                }
                double infoGain = baseEntropy - newEntropy;
                if (infoGain > beatInfoGain)
                {
                    beatInfoGain = infoGain;
                    bestFeature = i;
                }
            }

            return bestFeature;
        }

        /// <summary>
        /// choose the one has most frequency
        /// </summary>
        /// <param name="labels"></param>
        /// <returns></returns>
        public static string MajorityClass(IEnumerable<string> labels)
        {
            return (from r in labels group r by r into g select g).OrderBy(g => g.Count()).Last().Key;
        }
    }

    public class DataSetItem
    {
        /// <summary>
        /// features of item. each item should have same number of features
        /// </summary>
        public string[] Features;

        /// <summary>
        /// the name of class where item belongs to
        /// </summary>
        public string ClassName;
    }

    public class DecisionTreeNode
    {
        /// <summary>
        /// the label name on which to split
        /// null or empty if it is a leaf
        /// </summary>
        public string label;

        public string FeatValue;

        /// <summary>
        /// all children
        /// </summary>
        public List<DecisionTreeNode> ChildrenList = new List<DecisionTreeNode>();

        /// <summary>
        /// the className.
        /// null or empty if it not a leaf
        /// </summary>
        public string ClassName;

        /// <summary>
        /// true, choose the most frequent class as classname
        /// </summary>
        public bool IsMajority;

        public bool IsLeaf
        {
            get { return ChildrenList.Count <= 0; }
        }
    }
}

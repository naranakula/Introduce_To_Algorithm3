using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// bayes theory:choose the largest probability, it commonly be used to classify document
    /// 1)prepare data which include text document and its class
    /// 2)create a word dictionary. 
    /// 3)convert a document to feature vector.feature vector should have the same length bit as dictionary word count. each bit is 0 or 1 represent a word in dictionary exists in document or not 
    /// </summary>
    public class NaiveBayes
    {
        /// <summary>
        /// create a word dictionary.each word only appear once 
        /// </summary>
        public List<string> CreateVocabDict()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// convert a document to feature vector.feature vector should have the same length bit as dictionary word count. each bit is 0 or 1 represent a word in dictionary exists in document or not 
        /// </summary>
        /// <param name="vocabDict"></param>
        /// <param name="document">first you need to convert a text document to word list</param>
        /// <returns></returns>
        public bool[] Document2Vector(List<String> vocabDict, List<string> document)
        {
            bool[] vector = new bool[vocabDict.Count];
            foreach (string word in document)
            {
                int index = vocabDict.IndexOf(word);
                if (index >= 0)
                {
                    vector[index] = true;
                }
            }

            return vector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet">每个项对应一个文档</param>
        /// <param name="classList">每个相对应文档的分类</param>
        /// <returns>tuple第一项为值是在某个类发生的情况下，单词出现的概率 ； tuple的第二项计算每个类出现的概率</returns>
        public  Tuple<Dictionary<Tuple<string, int>, double>, Dictionary<string, double>> TrainBayes(
            List<bool[]> dataSet, string[] classList)
        {
            //每个类出现的概率
            var group = from r in classList group r by r into g select new {ClassName = g.Key, Count = g.Count()};
            int numClasses = classList.Length;
            //词典的单词数
            int wordCount = dataSet[0].Length;
            //计算每个类出现的概率
            Dictionary<string, double> pClassDict = new Dictionary<string, double>();
            foreach (var g in group)
            {
                pClassDict.Add(g.ClassName, g.Count*1.0/numClasses);
            }

            //计算在某个类发生的情况下，单词出现的概率
            //tuple第一项为类，第二项为单词在词典中的位置，值是在某个类发生的情况下，单词出现的概率
            Dictionary<Tuple<string, int>, double> pClassWordDict = new Dictionary<Tuple<string, int>, double>();
            //指定类下，单词出现的次数
            Dictionary<string, int[]> classNum = new Dictionary<string, int[]>();
            //制定类下，所有单词出现的次数
            Dictionary<string, int> classAllNum = new Dictionary<string, int>();
            foreach (var key in pClassDict.Keys)
            {
                classNum.Add(key, new int[wordCount]);
                classAllNum.Add(key, 0);
            }
            for (int i = 0; i < dataSet.Count; i++)
            {
                string key = classList[i];
                for (int j = 0; j < dataSet[i].Length; j++)
                {
                    if (dataSet[i][j])
                    {
                        classNum[key][j]++;
                        classAllNum[key]++;
                    }
                }
            }

            foreach (var key in pClassDict.Keys)
            {
                for (int i = 0; i < wordCount; i++)
                {
                    pClassWordDict.Add(new Tuple<string, int>(key, i), classNum[key][i]*1.0/classAllNum[key]);
                }
            }

            return new Tuple<Dictionary<Tuple<string, int>, double>, Dictionary<string, double>>(pClassWordDict,
                pClassDict);
        }



        /// <summary>
        /// 调优后的贝叶斯分类
        /// 1)解决P(Fj|Ci)=0，导致整个结果为0的情况
        /// 2）P(F1|Ci)*P(F2|Ci)*……*P(Fn|Ci) * P(Ci)多个小数相乘，会使结果有可能省略到0.调高单个P(Fj|Ci)
        /// </summary>
        /// <param name="dataSet">每个项对应一个文档</param>
        /// <param name="classList">每个相对应文档的分类</param>
        /// <returns>tuple第一项为值是在某个类发生的情况下，单词出现的概率 ； tuple的第二项计算每个类出现的概率</returns>
        public  Tuple<Dictionary<Tuple<string, int>, double>, Dictionary<string, double>> TrainBayesTuned(
            List<bool[]> dataSet, string[] classList)
        {
            //每个类出现的概率
            var group = from r in classList group r by r into g select new {ClassName = g.Key, Count = g.Count()};
            int numClasses = classList.Length;
            //词典的单词数
            int wordCount = dataSet[0].Length;
            //计算每个类出现的概率
            Dictionary<string, double> pClassDict = new Dictionary<string, double>();
            foreach (var g in group)
            {
                pClassDict.Add(g.ClassName, g.Count*1.0/numClasses);
            }

            //计算在某个类发生的情况下，单词出现的概率
            //tuple第一项为类，第二项为单词在词典中的位置，值是在某个类发生的情况下，单词出现的概率
            Dictionary<Tuple<string, int>, double> pClassWordDict = new Dictionary<Tuple<string, int>, double>();
            //指定类下，单词出现的次数
            Dictionary<string, int[]> classNum = new Dictionary<string, int[]>();
            //制定类下，所有单词出现的次数
            Dictionary<string, int> classAllNum = new Dictionary<string, int>();
            foreach (var key in pClassDict.Keys)
            {
                //将每个单词在每个类中的次数初始化为1，解决P(Fj|Ci)=0，导致整个结果为0的情况
                int[] init = new int[wordCount];
                for (int i = 0; i < init.Length; i++)
                {
                    init[i] = 1;
                }
                classNum.Add(key, init);
                //这里的2没有什么特殊意义，只要是>=1的小数(不能太大)都没问题
                classAllNum.Add(key, 2);
            }
            for (int i = 0; i < dataSet.Count; i++)
            {
                string key = classList[i];
                for (int j = 0; j < dataSet[i].Length; j++)
                {
                    if (dataSet[i][j])
                    {
                        classNum[key][j]++;
                        classAllNum[key]++;
                    }
                }
            }

            foreach (var key in pClassDict.Keys)
            {
                for (int i = 0; i < wordCount; i++)
                {
                    //函数f(x) 与ln(f(x) )的在相同区域内同时增加或者减少，并且在相同点上取到极值
                    pClassWordDict.Add(new Tuple<string, int>(key, i),
                        System.Math.Log(classNum[key][i]*1.0/classAllNum[key]));
                }
            }

            return new Tuple<Dictionary<Tuple<string, int>, double>, Dictionary<string, double>>(pClassWordDict,
                pClassDict);
        }


        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="pClassWordDict"></param>
        /// <param name="pClassDict"></param>
        /// <param name="newDocument"></param>
        /// <returns></returns>
        public string Classify(Dictionary<Tuple<string, int>, double> pClassWordDict,
            Dictionary<string, double> pClassDict, bool[] newDocument)
        {
            //获取所有单词出现的列
            List<int> existList = new List<int>();
            for (int i = 0; i < newDocument.Length; i++)
            {
                if (newDocument[i])
                {
                    existList.Add(i);
                }
            }

            //新文档在每个类中出现的概率
            Dictionary<string, double> classProbDict = new Dictionary<string, double>();

            foreach (string key in pClassDict.Keys)
            {
                //P(F1|Ci)*P(F2|Ci)*……*P(Fn|Ci) * P(Ci)
                double prob = pClassDict[key];
                foreach (int i in existList)
                {
                    prob *= pClassWordDict[new Tuple<string, int>(key, i)];
                }

                classProbDict.Add(key, prob);
            }

            //可以返回文档在所有类的概率，也可以返回最有可能的类。
            return (from r in classProbDict orderby r.Value select r).Last().Key;
        }

        /// <summary>
        /// 调优的分类只能使用调优的训练结果
        /// </summary>
        /// <param name="pClassWordDict"></param>
        /// <param name="pClassDict"></param>
        /// <param name="newDocument"></param>
        /// <returns></returns>
        public string ClassifyTuned(Dictionary<Tuple<string, int>, double> pClassWordDict,
            Dictionary<string, double> pClassDict, bool[] newDocument)
        {
            //获取所有单词出现的列
            List<int> existList = new List<int>();
            for (int i = 0; i < newDocument.Length; i++)
            {
                if (newDocument[i])
                {
                    existList.Add(i);
                }
            }

            //新文档在每个类中出现的概率
            Dictionary<string, double> classProbDict = new Dictionary<string, double>();

            foreach (string key in pClassDict.Keys)
            {
                //P(F1|Ci)*P(F2|Ci)*……*P(Fn|Ci) * P(Ci)
                //ln (a*b) = ln(a)+ln(b)
                double prob = System.Math.Log(pClassDict[key]);
                foreach (int i in existList)
                {
                    prob += pClassWordDict[new Tuple<string, int>(key, i)];
                }

                classProbDict.Add(key, prob);
            }

            //可以返回文档在所有类的概率，也可以返回最有可能的类。
            return (from r in classProbDict orderby r.Value select r).Last().Key;
        }
        ////////////////////////////////////////////////////////////////////////////////
        /// //////////////////////////////////////////////////////////

        /// <summary>
        /// convert a document to feature vector.feature vector should have the same length bit as dictionary word count. each position represent a word in dictionary appears how many times in document
        /// </summary>
        /// <param name="vocabDict"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public int[] Document2VectorWordsCount(List<String> vocabDict, List<string> document)
        {
            int[] vector = new int[vocabDict.Count];
            foreach (string word in document)
            {
                int index = vocabDict.IndexOf(word);
                if (index >= 0)
                {
                    vector[index]++;
                }
            }

            return vector;
        }

        public Tuple<Dictionary<Tuple<string, int>, double>, Dictionary<string, double>> TrainBayes(
    List<int[]> dataSet, string[] classList)
        {
            //每个类出现的概率
            var group = from r in classList group r by r into g select new { ClassName = g.Key, Count = g.Count() };
            int numClasses = classList.Length;
            //词典的单词数
            int wordCount = dataSet[0].Length;
            //计算每个类出现的概率
            Dictionary<string, double> pClassDict = new Dictionary<string, double>();
            foreach (var g in group)
            {
                pClassDict.Add(g.ClassName, g.Count * 1.0 / numClasses);
            }

            //计算在某个类发生的情况下，单词出现的概率
            //tuple第一项为类，第二项为单词在词典中的位置，值是在某个类发生的情况下，单词出现的概率
            Dictionary<Tuple<string, int>, double> pClassWordDict = new Dictionary<Tuple<string, int>, double>();
            //指定类下，单词出现的次数
            Dictionary<string, int[]> classNum = new Dictionary<string, int[]>();
            //制定类下，所有单词出现的次数
            Dictionary<string, int> classAllNum = new Dictionary<string, int>();
            foreach (var key in pClassDict.Keys)
            {
                classNum.Add(key, new int[wordCount]);
                classAllNum.Add(key, 0);
            }
            for (int i = 0; i < dataSet.Count; i++)
            {
                string key = classList[i];
                for (int j = 0; j < dataSet[i].Length; j++)
                {
                    classNum[key][j] += dataSet[i][j];
                    classAllNum[key] += dataSet[i][j];
                }
            }

            foreach (var key in pClassDict.Keys)
            {
                for (int i = 0; i < wordCount; i++)
                {
                    pClassWordDict.Add(new Tuple<string, int>(key, i), classNum[key][i] * 1.0 / classAllNum[key]);
                }
            }

            return new Tuple<Dictionary<Tuple<string, int>, double>, Dictionary<string, double>>(pClassWordDict,
                pClassDict);
        }

        /// <summary>
        /// 调优后的贝叶斯分类
        /// 1)解决P(Fj|Ci)=0，导致整个结果为0的情况
        /// 2）P(F1|Ci)*P(F2|Ci)*……*P(Fn|Ci) * P(Ci)多个小数相乘，会使结果有可能省略到0.调高单个P(Fj|Ci)
        /// </summary>
        /// <param name="dataSet">每个项对应一个文档</param>
        /// <param name="classList">每个相对应文档的分类</param>
        /// <returns>tuple第一项为值是在某个类发生的情况下，单词出现的概率 ； tuple的第二项计算每个类出现的概率</returns>
        public  Tuple<Dictionary<Tuple<string, int>, double>, Dictionary<string, double>> TrainBayesTuned(
            List<int[]> dataSet, string[] classList)
        {
            //每个类出现的概率
            var group = from r in classList group r by r into g select new { ClassName = g.Key, Count = g.Count() };
            int numClasses = classList.Length;
            //词典的单词数
            int wordCount = dataSet[0].Length;
            //计算每个类出现的概率
            Dictionary<string, double> pClassDict = new Dictionary<string, double>();
            foreach (var g in group)
            {
                pClassDict.Add(g.ClassName, g.Count * 1.0 / numClasses);
            }

            //计算在某个类发生的情况下，单词出现的概率
            //tuple第一项为类，第二项为单词在词典中的位置，值是在某个类发生的情况下，单词出现的概率
            Dictionary<Tuple<string, int>, double> pClassWordDict = new Dictionary<Tuple<string, int>, double>();
            //指定类下，单词出现的次数
            Dictionary<string, int[]> classNum = new Dictionary<string, int[]>();
            //制定类下，所有单词出现的次数
            Dictionary<string, int> classAllNum = new Dictionary<string, int>();
            foreach (var key in pClassDict.Keys)
            {
                //将每个单词在每个类中的次数初始化为1，解决P(Fj|Ci)=0，导致整个结果为0的情况
                int[] init = new int[wordCount];
                for (int i = 0; i < init.Length; i++)
                {
                    init[i] = 1;
                }
                classNum.Add(key, init);
                //这里的2没有什么特殊意义，只要是>=1的小数(不能太大)都没问题
                classAllNum.Add(key, 2);
            }
            for (int i = 0; i < dataSet.Count; i++)
            {
                string key = classList[i];
                for (int j = 0; j < dataSet[i].Length; j++)
                {
                    classNum[key][j] += dataSet[i][j];
                    classAllNum[key] += dataSet[i][j];
                }
            }

            foreach (var key in pClassDict.Keys)
            {
                for (int i = 0; i < wordCount; i++)
                {
                    //函数f(x) 与ln(f(x) )的在相同区域内同时增加或者减少，并且在相同点上取到极值
                    pClassWordDict.Add(new Tuple<string, int>(key, i),
                        System.Math.Log(classNum[key][i] * 1.0 / classAllNum[key]));
                }
            }

            return new Tuple<Dictionary<Tuple<string, int>, double>, Dictionary<string, double>>(pClassWordDict,
                pClassDict);
        }



        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="pClassWordDict"></param>
        /// <param name="pClassDict"></param>
        /// <param name="newDocument"></param>
        /// <returns></returns>
        public string Classify(Dictionary<Tuple<string, int>, double> pClassWordDict,
            Dictionary<string, double> pClassDict, int[] newDocument)
        {
            //获取所有单词出现的列
            List<int> existList = new List<int>();
            for (int i = 0; i < newDocument.Length; i++)
            {
                if (newDocument[i]>0)
                {
                    existList.Add(i);
                }
            }

            //新文档在每个类中出现的概率
            Dictionary<string, double> classProbDict = new Dictionary<string, double>();

            foreach (string key in pClassDict.Keys)
            {
                //P(F1|Ci)*P(F2|Ci)*……*P(Fn|Ci) * P(Ci)
                double prob = pClassDict[key];
                foreach (int i in existList)
                {
                    prob *= pClassWordDict[new Tuple<string, int>(key, i)];
                }

                classProbDict.Add(key, prob);
            }

            //可以返回文档在所有类的概率，也可以返回最有可能的类。
            return (from r in classProbDict orderby r.Value select r).Last().Key;
        }

        /// <summary>
        /// 调优的分类只能使用调优的训练结果
        /// </summary>
        /// <param name="pClassWordDict"></param>
        /// <param name="pClassDict"></param>
        /// <param name="newDocument"></param>
        /// <returns></returns>
        public string ClassifyTuned(Dictionary<Tuple<string, int>, double> pClassWordDict,
            Dictionary<string, double> pClassDict, int[] newDocument)
        {
            //获取所有单词出现的列
            List<int> existList = new List<int>();
            for (int i = 0; i < newDocument.Length; i++)
            {
                if (newDocument[i]>0)
                {
                    existList.Add(i);
                }
            }

            //新文档在每个类中出现的概率
            Dictionary<string, double> classProbDict = new Dictionary<string, double>();

            foreach (string key in pClassDict.Keys)
            {
                //P(F1|Ci)*P(F2|Ci)*……*P(Fn|Ci) * P(Ci)
                //ln (a*b) = ln(a)+ln(b)
                double prob = System.Math.Log(pClassDict[key]);
                foreach (int i in existList)
                {
                    prob += pClassWordDict[new Tuple<string, int>(key, i)];
                }

                classProbDict.Add(key, prob);
            }

            //可以返回文档在所有类的概率，也可以返回最有可能的类。
            return (from r in classProbDict orderby r.Value select r).Last().Key;
        }

    }
}

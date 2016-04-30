using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3.Common.MachineLearning
{
    /// <summary>
    /// 机器学习辅助类
    /// </summary>
    public static class MlHelper
    {

        #region 矩阵相关

        #region 创建矩阵

        /// <summary>
        /// 创建一个rows行cols列的矩阵，矩阵的每一项值值为val
        /// </summary>
        /// <param name="rows">矩阵的行数</param>
        /// <param name="cols">矩阵的列</param>
        /// <param name="val">矩阵每一项的值</param>
        /// <returns></returns>
        public static double[][] Same(int rows, int cols, double val)
        {
            double[][] matrix = new double[rows][];

            for (int r = 0; r < rows; r++)
            {
                matrix[r] = new double[cols];
            }

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < matrix[r].Length; c++)
                {
                    matrix[r][c] = val;
                }
            }

            return matrix;
        }


        /// <summary>
        /// 创建一个rows行 cols列的每一项都是0的矩阵
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static double[][] Zero(int rows, int cols)
        {
            return Same(rows, cols, 0);
        }


        /// <summary>
        /// 创建一个rows行 cols列的每一项都是1的矩阵
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static double[][] One(int rows, int cols)
        {
            return Same(rows, cols, 1);
        }

        /// <summary>
        /// 创建一个rows行cols列的矩阵，矩阵的每一项值值为[0,1)之间的随机小数
        /// </summary>
        /// <param name="rows">矩阵的行数</param>
        /// <param name="cols">矩阵的列</param>
        /// <returns></returns>
        public static double[][] Rand(int rows, int cols)
        {
            double[][] matrix = new double[rows][];

            for (int r = 0; r < rows; r++)
            {
                matrix[r] = new double[cols];
            }

            Random rnd = new Random();//使用与时间相关的默认种子
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < matrix[r].Length; c++)
                {
                    matrix[r][c] = rnd.NextDouble();
                }
            }

            return matrix;
        }


        #endregion

        #region 矩阵操作相关

        /// <summary>
        /// 返回一个新矩阵，新矩阵的每一项等于原来的项加上指定的val，
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double[][] Add(double[][] matrix, double val)
        {
            int rows = matrix.Length;

            double[][] result = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new double[matrix[i].Length];
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                {
                    result[i][j] = matrix[i][j] + val;
                }
            }

            return result;
        }

        /// <summary>
        /// 返回一个新矩阵，新矩阵的每一项等于原来的项乘以指定的val，
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double[][] Multiply(double[][] matrix, double val)
        {
            int rows = matrix.Length;

            double[][] result = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new double[matrix[i].Length];
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                {
                    result[i][j] = matrix[i][j] * val;
                }
            }

            return result;
        }

        /// <summary>
        /// 创建一个新的矩阵，并对矩阵的原来每一项执行一个定制的计算
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static double[][] Func(double[][] matrix, Func<double, double> func)
        {
            int rows = matrix.Length;

            double[][] result = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new double[matrix[i].Length];
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                {
                    result[i][j] = func(matrix[i][j]);
                }
            }

            return result;
        }

        /// <summary>
        /// 返回一个新的向量，对原来的向量每一项执行定制的操作
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static double[] Func(double[] vector, Func<double, double> func)
        {
            int cols = vector.Length;

            double[] result = new double[cols];

            for (int i = 0; i < cols; i++)
            {
                result[cols] = func(vector[i]);
            }

            return result;
        }

        #region 方差 标准差 平均值
        /// <summary>
        /// 求每一列的均值
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[] Mean(double[][] matrix)
        {
            int cols = matrix[0].Length;
            double[] result = new double[cols];

            int rows = matrix.Length;
            for (int i = 0; i < rows; i++)
            {
                for (int c = 0; c < cols; c++)
                {
                    result[c] += matrix[i][c];
                }
            }

            for (int c = 0; c < cols; c++)
            {
                result[c] = result[c]/rows;
            }

            return result;
        }

        /// <summary>
        /// 计算方差
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[] Variance(double[][] matrix)
        {
            double[] mean = Mean(matrix);
            //列数
            int cols = mean.Length;
            double[] vars = new double[cols];
            int rows = matrix.Length;//行数
            for (int i = 0; i < rows; i++)
            {
                for (int c = 0; c < cols; c++)
                {
                    vars[c] += (matrix[i][c] - mean[c])*(matrix[i][c] - mean[c]);
                }
            }

            for (int c = 0; c < cols; c++)
            {
                vars[c] = vars[c]/rows;
            }

            return vars;
        }


        /// <summary>
        /// 计算标准差
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[] StandardDeviation(double[][] matrix)
        {
            //求方差
            double[] vars = Variance(matrix);

            for (int i = 0; i < vars.Length; i++)
            {
                //计算标准差
                vars[i] = System.Math.Sqrt(vars[i]);
            }

            return vars;
        }

        #endregion

        #endregion

        #region 返回矩阵的字符串表示

        /// <summary>
        /// 返回矩阵的字符串表示
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static string ToString(double[][] matrix)
        {
            if (matrix == null)
            {
                return "matrix is null";
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < matrix.Length; i++)
            {
                sb.Append("[ " + i.ToString().PadRight(6) + " ]  ");//显示行号

                for (int j = 0; j < matrix[i].Length; j++)
                {
                    sb.Append(matrix[i][j].ToString("F3").PadLeft(10));
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// 显示一个一维向量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static string ToString(double[] vector)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < vector.Length; i++)
            {
                sb.Append(vector[i].ToString("F3").PadLeft(10));
            }

            return sb.ToString();
        }

        #endregion

        #region 矩阵归一化

        /// <summary>
        /// 对指定的列进行最大最小归一化
        /// 计算公式  (x-min)/(max-min)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="column">需要进行归一化的列</param>
        public static void MinMaxNormal(double[][] data, int column)
        {
            
        }
        
        #endregion

        #endregion

        #region 枚举值转数值

        /// <summary>
        /// 对于x 值得  1-of-(N-1) Effect encoding
        /// 如果只有两个值 则编码为 -1 和 1
        /// </summary>
        /// <param name="index">第 index 个x值</param>
        /// <param name="N">有多少个不同的x值</param>
        /// <returns></returns>
        public static double[] EffectsEncoding(int index, int N)
        {
            if (N <= 1)
            {
                throw new Exception("N应该大于1");
            }

            if (N == 2)
            {
                //实际上N=2这个判断  可以去掉,这里是为了显示说明
                if (index == 0)
                {
                    return new double[1]{1};
                }
                else if (index == 1)
                {
                    return new double[1]{-1};
                }
            }


            double[] result = new double[N-1];
            if (index == N - 1)
            {
                //最后一个全是-1
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = -1;
                }
            }
            else
            {
                //1-of-(N-1) effect encoding
                //index为1，其它位置全为0
                result[index] = 1;
            }

            return result;
        }

        /// <summary>
        /// 对于y值的 1-Of-N Dummy Encoding，
        /// </summary>
        /// <param name="index">第几个y值</param>
        /// <param name="N">总共有多少y值</param>
        /// <returns></returns>
        public static double[] DummyEncoding(int index, int N)
        {
            double[] result = new double[N];
            //index位置为1 其余全为0
            //对于只有两个的y值，可以使用0和1来替代
            result[index] = 1;

            return result;
        }

        #endregion



    }
}

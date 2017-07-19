using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// 依赖于Nuget的CsvHelper
    /// https://github.com/JoshClose/CsvHelper
    /// 用于读写.csv文件
    /// csv文件以纯文本形式存储表格数据（数字和文本）。
    /// </summary>
    public static class CsvUtils
    {
        #region 读

        /// <summary>
        /// 读取CSV文件到对应的类，默认类使用属性自动映射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvFile">csv文件</param>
        /// <returns></returns>
        public static List<T> ReadList<T>(string csvFile)
        {
            using(StreamReader streamReader = new StreamReader(csvFile, Encoding.UTF8))
            {
                using (CsvReader csvReader = new CsvReader(streamReader))
                {
                    return csvReader.GetRecords<T>().ToList();
                }
                
            }
        }

        /// <summary>
        /// 按行读取csv文件
        /// </summary>
        /// <param name="csvFile"></param>
        /// <param name="rowReader">行的读取函数</param>
        public static List<T> ReadList<T>(string csvFile,Func<CsvReader,T> rowReader) where T:class
        {
            using (StreamReader streamReader = new StreamReader(csvFile, Encoding.UTF8))
            {
                using (CsvReader csvReader = new CsvReader(streamReader))
                {

                    List<T> list = new List<T>();

                    //设置是否有头记录
                    csvReader.Configuration.HasHeaderRecord = false;//默认是true的
                    //默认csvReader认为是有HeaderRecord的，首次自动跳过Header，从第二行数据行开始
                    //当没有更多行时返回false
                    while (csvReader.Read())
                    {
                        //列从0开始计数
                        //string col1 = csvReader.GetField(0);//读取第一列
                        //int col2 = csvReader.GetField<int>(1);//读取第二列
                        //csvReader.TryGetField<int>(2,out col2);//尝试读取第三列

                        //csvReader.GetRecord<T>();//读取整行记录
                        if(rowReader != null)
                        {
                            T item = rowReader(csvReader);
                            if (item != null)
                            {
                                list.Add(item);
                            }
                        }

                    }

                    return list;
                }
            }
        }



        #endregion

        #region 写

        /// <summary>
        /// 如果文件存在覆盖，文件不存在新建
        /// 默认类使用属性自动映射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvFile"></param>
        /// <param name="list"></param>
        public static void WriteCsvFile<T>(string csvFile,List<T> list)
        {
            using(StreamWriter streamWriter = new StreamWriter(csvFile, false, Encoding.UTF8))
            {
                using(CsvWriter writer = new CsvWriter(streamWriter))
                {
                    writer.WriteRecords(list);
                }
            }
        }

        /// <summary>
        /// 写csv文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvFile"></param>
        /// <param name="list"></param>
        /// <param name="rowWriter"></param>
        public static void WriteCsvFile<T>(string csvFile, List<T> list, Action<CsvWriter,T> rowWriter) where T:class
        {
            using (StreamWriter streamWriter = new StreamWriter(csvFile, false, Encoding.UTF8))
            {
                using (CsvWriter writer = new CsvWriter(streamWriter))
                {
                    //不设置头部，默认是true的
                    writer.Configuration.HasHeaderRecord = false;
                    
                    foreach(T item in list)
                    {
                        if(item == null)
                        {
                            continue;
                        }

                        //writer.WriteField("str");//写第一列
                        //writer.WriteField(1);//写第二列

                        rowWriter(writer, item);

                        //开始写下一行
                        writer.NextRecord();
                    }
                }
            }
        }



        #endregion

    }
}

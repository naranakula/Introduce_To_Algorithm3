using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public class CodeCounter
    {
        /// <summary>
        /// get line count
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static int GetCodeLines(string dir,string pattern = "*.cs")
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            if (!di.Exists)
            {
                return 0;
            }

            FileInfo[] fis = di.GetFiles(pattern, SearchOption.AllDirectories);

            int count = 0;
            foreach (FileInfo fi in fis)
            {
                using (StreamReader sr = new StreamReader(fi.FullName,Encoding.UTF8))
                {
                    while (sr.ReadLine() != null)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 将代码中的注释去掉
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="pattern"></param>
        /// <param name="encodingStr"></param>
        public static void RemoveComments(string dir,String pattern="*.cs",string encodingStr = "utf-8")
        {
            Encoding encoding = Encoding.GetEncoding(encodingStr);
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                return;
            }

            //获取所有文件
            FileInfo[] fileInfos = dirInfo.GetFiles(pattern, SearchOption.AllDirectories);

            foreach(FileInfo fileItem in fileInfos)
            {
                StringBuilder sb = new StringBuilder();
                using(StreamReader reader = new StreamReader(fileItem.FullName, encoding))
                {
                    string str = null;
                    while((str = reader.ReadLine()) != null)
                    {
                        string strTrim = str.Trim();
                        if (strTrim.StartsWith(@"//") && !strTrim.Contains(@"/*") && !strTrim.Contains("*/"))
                        {
                            //注释行
                            continue;
                        }
                        else
                        {
                            sb.AppendLine(str);
                        }
                    }
                }

                using(StreamWriter writer = new StreamWriter(fileItem.FullName, append: false, encoding: encoding))
                {
                    writer.WriteLine(sb.ToString());
                }
            }

        }
    }
}

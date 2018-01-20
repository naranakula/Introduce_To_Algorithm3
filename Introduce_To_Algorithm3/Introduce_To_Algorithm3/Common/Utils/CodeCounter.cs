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
        public static int GetCodeLines(string dir, string pattern = "*.cs")
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
                using (StreamReader sr = new StreamReader(fi.FullName, Encoding.UTF8))
                {
                    while (sr.ReadLine() != null)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        #region 去掉注释

        /// <summary>
        /// 将代码中的//注释去掉，并且备份数据
        /// 仅仅去掉//注释
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="pattern"></param>
        /// <param name="encodingStr">编码</param>
        /// <param name="isBak">是否备份文件</param>
        public static void RemoveComments(string dir, String pattern = "*.cs", string encodingStr = "utf-8",bool isBak=true)
        {
            Encoding encoding = Encoding.GetEncoding(encodingStr);
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                return;
            }

            //获取所有文件
            FileInfo[] fileInfos = dirInfo.GetFiles(pattern, SearchOption.AllDirectories);

            #region 备份
            if (isBak)
            {
                string datePart = DateTime.Now.ToString("yyyyMMddHHmmss");
                string extensionPart = "." + datePart + ".bak";
                foreach (FileInfo fileItem in fileInfos)
                {
                    string fullName = fileItem.FullName;
                    string bakFullName = fullName + extensionPart;
                    //备份文件
                    File.Copy(fullName, bakFullName, true);
                }
            }
            #endregion


            foreach (FileInfo fileItem in fileInfos)
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader reader = new StreamReader(fileItem.FullName, encoding))
                {
                    string str = null;
                    while ((str = reader.ReadLine()) != null)
                    {
                        string strTrim = str.Trim();
                        if (strTrim.StartsWith(@"//") && !strTrim.Contains(@"/*") && !strTrim.Contains(@"*/"))
                        {
                            //注释行
                            //目标是使别人难以读懂,并没有移除全部所有的注释
                            continue;
                        }
                        else
                        {
                            sb.AppendLine(str);
                        }
                    }
                }

                using (StreamWriter writer = new StreamWriter(fileItem.FullName, append: false, encoding: encoding))
                {
                    writer.Write(sb.ToString());
                }
            }

        }


        #endregion

    }
}

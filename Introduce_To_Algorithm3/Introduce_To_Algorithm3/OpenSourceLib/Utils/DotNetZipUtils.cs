using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace Introduce_To_Algorithm3.OpenSourceLib.Utils
{
    /// <summary>
    /// dotnetzip工具类
    /// </summary>
    public class DotNetZipUtils
    {
        /// <summary>
        /// 将多个zip文件压缩
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="targetFile"></param>
        public static void ZipFiles(List<string> fileList, string targetFile)
        {
            using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
            {
                foreach (var file in fileList)
                {
                    zipFile.AddFile(file);
                }

                zipFile.Save(targetFile);
            }
        }
    }
}
